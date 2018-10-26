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
    Vector3[] verts;
    int[] tris;
    Vector2[] uvs;
    Material backgroundMat;
    int dataSize;
    float[] gaussianKernel;
    public int gaussianKernelSize;
    public float gaussianKernelStdDev;
    public float gaussianKernelAlpha;


    WasapiCapture capture = new WasapiLoopbackCapture();
    // Use this for initialization
    void Start () {
        meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        mesh = meshFilter.mesh;
        verts = new Vector3[4096 * 4];
        uvs = new Vector2[4096 * 4];
        tris = new int[4096 * 6];
        backgroundMat = Resources.Load("Material/FourierMaterial") as Material;
        meshRenderer.material = backgroundMat;
        gaussianKernel = Convoluter.GetGaussian(gaussianKernelSize, gaussianKernelStdDev, gaussianKernelAlpha);

        aCapture = gameObject.GetComponent<AudioCapture>();
        capture.Initialize();
        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);
        
        dataSize = 4096;
    }
	
	// Update is called once per frame
	void Update () {

        // Since this is being changed on a seperate thread we do this to be safe
        lock (aCapture.barData) {
            DrawMesh(aCapture.barData);

        }
    }

    public void DrawMesh(float[] data) {
        float zDistance = 1000;
        if (data.Length != 4096) {
            //Debug.LogError("Expected data length to be 4096!!!@!#34%%$#@");
            return;
        }
        float scaleFactor = 500;
        //some processing and gaussian smoothing for looks
        for (int i=0;i<data.Length;i++) { 
            data[i] *= scaleFactor;
        }
        gaussianKernel = Convoluter.GetGaussian(gaussianKernelSize, gaussianKernelStdDev, gaussianKernelAlpha);
        data = Convoluter.Convolve(gaussianKernel, data);
        
        //after convolving with gaussian the data must be normalized to fit on the screen
        //start by finding the biggest value in the array
        float max = 0;
        for (int i=0;i<data.Length;i++) {
            if (data[i] > max) max = data[i];
        }
        //if the max value is off the screen
        float highestVisibleYValue = zDistance * Mathf.Tan(Mathf.PI*Camera.main.fieldOfView/(180*2));
        //Debug.Log(highestVisibleYValue);
        if (max>highestVisibleYValue) {
            scaleFactor = highestVisibleYValue / max;
            for (int i = 0; i < data.Length; i++) {
                data[i] *= scaleFactor;
            }
        }
        
        //verts, tris and uvs are arrays to store mesh info. they are already allocated

        //setting verts
        //size: 4*4096
        for (int i=0;i<4096;i++) {
            //top left
            verts[4 * i] = new Vector3(0.5f*i-1024, data[i],zDistance);
            //top right
            verts[4 * i + 1] = new Vector3(0.5f * i + 0.5f - 1024, data[i], zDistance);
            //bottom left
            verts[4 * i + 2] = new Vector3(0.5f*i - 1024, -data[i], zDistance);
            //bottom right
            verts[4 * i + 3] = new Vector3(0.5f*i + 0.5f - 1024, -data[i], zDistance);
        }
        //Debug.Log("Max: " + max);
        //seting uvs
        //size:4*4096
        //need to translate the domain of 0,0 - 1,1 into world space
        //ie -yMax,0 - yMax, 4096
        float yMax = zDistance  * Mathf.Tan(Mathf.PI*Camera.main.fieldOfView/(180*2));
        for (int i=0;i<4096;i++) {
            uvs[4*i] = new Vector2((float)i/4096, data[i]/yMax) ;
            uvs[4 * i + 1] = new Vector2((float)i / 4096, data[i] / yMax);
            uvs[4 * i + 2] = new Vector2((float)i / 4096, -data[i] / yMax);
            uvs[4 * i + 3] = new Vector2((float)i / 4096, -data[i] / yMax);
        }
        Vector2 maxUV = Vector2.zero;
        for (int i=0;i<uvs.Length;i++) {
            if (uvs[i].magnitude > maxUV.magnitude) maxUV = uvs[i];
        }


        //setting tris
        //size: 6*4096
        for (int i=0;i<4096;i++) {
            tris[6 * i] = 4 * i;
            tris[6 * i + 1] = 4 * i + 1;
            tris[6 * i + 2] = 4 * i + 3;
            tris[6 * i + 3] = 4 * i;
            tris[6 * i + 4] = 4 * i + 3;
            tris[6 * i + 5] = 4 * i + 2;
        }
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

    }
    public void StartCapturingSound() {

    }
}
