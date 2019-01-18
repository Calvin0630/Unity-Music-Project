using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
using System.Threading;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.DSP;
using CSCore.Streams;


//captures audio coming out of the computer, performs an fft, AND DRAWS IT
public class AudioVisualizer : MonoBehaviour {
    AudioCapture aCapture;
    public GameObject cubePrefab;
    List<GameObject> cubes;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    //used for the mesh
    public int dataRowCount;
    List<float> dataQueue;
    Vector3[] verts;
    int[] tris;
    Vector2[] uvs;
    Material backgroundMat;
    int dataSize;
    float[] gaussianKernel;
    public int gaussianKernelSize;
    public float gaussianKernelStdDev;
    public float gaussianKernelAlpha;

    //counts the frames
    int frameCounter;
    WasapiCapture capture = new WasapiLoopbackCapture();
    // Use this for initialization
    void Start () {
        dataSize = GetComponent<AudioCapture>().numBars;
        meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        mesh = meshFilter.mesh;
        //each data point is represented by one vertex. trailing down the z axis distance by 1 unit per fft calculation
        //2048 rows
        dataQueue = new List<float>();
        //an extra row of verts for the first row
        verts = new Vector3[dataSize * (dataRowCount+1)];
        //initialize values to zero
        for (int i = 0; i < verts.Length; i++) verts[i] = Vector3.zero;
        uvs = new Vector2[dataSize * (dataRowCount+1)];
        tris = new int[(dataSize-1)*(dataSize - 1) * 2*3*2];
        backgroundMat = Resources.Load("Material/FourierMaterial") as Material;
        meshRenderer.material = backgroundMat;
        gaussianKernel = Convoluter.GetGaussian(gaussianKernelSize, gaussianKernelStdDev, gaussianKernelAlpha);

        aCapture = gameObject.GetComponent<AudioCapture>();
        capture.Initialize();
        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);
        frameCounter = 0;

    }
	
	// Update is called once per frame
	void Update () {
        //to keep track of the frame
        Debug.Log("frame: " + frameCounter);
        if (frameCounter > 5) {
            //Debug.Log("Pause");
            //Debug.Break();
        }
        frameCounter++;

        // Since this is being changed on a seperate thread we do this to be safe
        lock (aCapture.barData) {
            DrawMesh(aCapture.barData);

        }
    }

    public void DrawMesh(float[] data) {
        Debug.Log("data.Length" + data.Length);
        if (data.Length != 512) {
            Debug.LogError("Expected data length to be 512!!!@!#34%%$#@");
            return;
        }
        //some processing of the FFT data
        
        //start by normalizing the data
        float max = 0;
        for (int i = 0; i < data.Length; i++) {
            if (data[i]>max) {
                max = data[i];
            }
        }
        
        for (int i = 0; i < data.Length; i++) {
            data[i] /= max;
        }
        
        //done normalizing
        
        
        //and gaussian smoothing for looks
        
        gaussianKernel = Convoluter.GetGaussian(gaussianKernelSize, gaussianKernelStdDev, gaussianKernelAlpha);
        data = Convoluter.Convolve(gaussianKernel, data);
        
        //after convolving with gaussian the data must be normalized to fit on the screen
        //start by finding the biggest value in the array
        max = 0;
        for (int i=0;i<data.Length;i++) {
            if (data[i] > max) max = data[i];
        }
        for (int i = 0; i < data.Length; i++) {
            data[i] /= max;
        }
        float scaleFactor = 100;
        for (int i = 0; i < data.Length; i++) {
            data[i] *= scaleFactor;
        }


        //done manipulating data

        //start by removing the oldest data set
        //which is at the start of the queue
        //index, count
        if (dataQueue.Count > dataRowCount * dataSize) dataQueue.RemoveRange(0, dataSize);
        //push the new data to the end of the list
        dataQueue.AddRange(data);

        //verts, tris and uvs are arrays to store mesh info. they are already allocated

        //setting verts
        //when there is no sound verts should represent an array of vertices that make up a plane parallel to the xz plane
        //the start of verts is at 0,0,0 and it goes up on x axis.
        //each row on the z axis is a new sample of the fft data
        //size: dataRowCount*data.length
        //the number of rows that have data
        int activeRowCount = dataQueue.Count / dataSize;
        //Debug.Log("ActiveRowCount: " + activeRowCount);
        //Debug.Log("dataQueue.Count: " + dataQueue.Count);
        //Debug.Log("dataSize: " + dataSize);

        //make the first row of verts y=0
        for (int i = 0; i < dataSize; i++) {
            verts[i] = new Vector3(i, 0, -1);
        }
            //for each row
            for (int i =1;i<dataRowCount+1;i++) {
            //for each data point
            for (int j=0;j< dataSize; j++) {
                //Debug.Log("(i, j): (" + i + ", " + j + ")");
                //if there is data for this point
                if (dataSize * i + j < dataQueue.Count) {
                    //Debug.Log("dataSize * (activeRowCount-i-1) + j: " + (dataSize * (activeRowCount - i-1) + j));
                    float yValue = dataQueue[dataSize * (activeRowCount - i - 1) + j];

                    verts[dataSize * i + j] = new Vector3(j, yValue, i);
                }
                else {
                    //otherwise set y to 0
                    verts[dataSize * i + j] = new Vector3(j, 0, i);
                }
            }
        }
        //Debug.Log("Max: " + max);
        //seting uvs
        //size: dataRowCount*dataSize
        //need to translate the domain of 0,0 - 1,1 into world space
        //ie -yMax,0 - yMax, dataSize
        for (int i = 0; i < dataRowCount+1; i++) {
            //for each data point
            for (int j = 0; j < dataSize; j++) {
                float xVal = ((float)i) / (dataRowCount+1);
                float yVal = ((float)j) / dataSize;
                uvs[dataSize * i + j] = new Vector2(xVal, yVal);
                //uvs[dataSize * i + j] = Vector2.zero;
            }
        }


        //setting tris
        //size: (dataSize-1)*(dataRowCount-1) * 2*3

        for (int i=0;i<dataRowCount-1;i++) {
            for (int j=0;j< dataSize - 1;j++) {
                //make to quads that are identical, but face in the opposite diection

                //up facing quad
                tris[i * 12 * (dataSize -1) + 12*j]   = i * dataSize + j;
                tris[i * 12 * (dataSize -1) + 12*j+1] = (i + 1) * dataSize + j + 1;
                tris[i * 12 * (dataSize -1) + 12*j+2] = i * dataSize + j + 1;
                tris[i * 12 * (dataSize -1) + 12*j+3] = i * dataSize + j;
                tris[i * 12 * (dataSize -1) + 12*j+4] = (i + 1) * dataSize + j;
                tris[i * 12 * (dataSize -1) + 12*j+5] = (i + 1) * dataSize + j + 1;

                //down facing quad
                tris[i * 12 * (dataSize - 1) + 12 * j + 6] = i * dataSize + j;
                tris[i * 12 * (dataSize - 1) + 12 * j + 7] = i * dataSize + j + 1;
                tris[i * 12 * (dataSize - 1) + 12 * j + 8] = (i + 1) * dataSize + j + 1;
                tris[i * 12 * (dataSize - 1) + 12 * j + 9] = i * dataSize + j;
                tris[i * 12 * (dataSize - 1) + 12 * j + 10] = (i + 1) * dataSize + j + 1;
                tris[i * 12 * (dataSize - 1) + 12 * j + 11] = (i + 1) * dataSize + j;
            }
        }
        /*
            verts = new Vector3[dataSize * dataRowCount];
            uvs = new Vector2[dataSize * dataRowCount];
            tris = new int[(dataSize-1)*(dataRowCount-1) * 2*3];         
        */
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

    }
    public void StartCapturingSound() {

    }
}
