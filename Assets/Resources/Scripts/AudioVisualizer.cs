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

    WasapiCapture capture = new WasapiLoopbackCapture();
    // Use this for initialization
    void Start () {
        meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        mesh = meshFilter.mesh;
        meshRenderer.material.color = Color.green;
        verts = new Vector3[4096 * 4];
        uvs = new Vector2[4096 * 4];
        tris = new int[4096 * 6];

        aCapture = gameObject.GetComponent<AudioCapture>();
        capture.Initialize();
        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);
    }
	
	// Update is called once per frame
	void Update () {

        // Since this is being changed on a seperate thread we do this to be safe
        lock (aCapture.barData) {
            DrawMesh(aCapture.barData);

        }
    }
    /*
    void InstantiateBlocks() {
        //spawns the balls in a partial circle centered around the origin
        //r=20 from theta = pi/4-3pi/4 where 0 is the x axis (to the right)
        //the roatation of each consecutive cube is .05 from the origin
        int ballCount = 4096;
        //There should be 4096 balls between the two angle below. Angle is from camera perspective.
        //balls have a thickness of 1 unit;

        //stating from the right
        float startingRotation = Mathf.PI / 4;
        //ending on the left
        float endingRotation = 3 * Mathf.PI / 4;
        float deltaRotation = (endingRotation-startingRotation)/4096;
        float currentRotation = startingRotation;
        float boxWidth = .1f;
        float r = boxWidth / Mathf.Tan(deltaRotation);
        //a loop to spawn the cubes and adds them to the cubes list
        while (currentRotation <= endingRotation) {
            Vector3 position = new Vector3(r * Mathf.Cos(currentRotation), 0, r * Mathf.Sin(currentRotation)); ;
            GameObject tmp = Instantiate(cubePrefab, position, Quaternion.identity);
            tmp.transform.localScale = new Vector3(1, boxWidth, 1);
            tmp.transform.SetParent(gameObject.transform);
            cubes.Add(tmp);
            currentRotation += deltaRotation;
        }
    }

    //expects the array size to be 4096
    void SetCubeSizes(float[] data) {
        if (data.Length != 4096) Debug.LogError("Expected an array of size 4096?@?@");
        for (int i = 0;i<data.Length;i++) {
            //hold the old position. So it can keep track of x and z whick dont change, only y
            Vector3 oldScale = cubes[i].transform.localScale;
            cubes[i].transform.localScale = new Vector3(oldScale.x, 500*data[i], oldScale.z);
        }
    }
    */
    public void DrawMesh(float[] data) {
        if (data.Length != 4096) {
            Debug.LogError("Expected data length to be 4096!!!@!#34%%$#@");
            return;
        }
        //verts, tris and uvs are arrays to store mesh info. they are already allocated

        float zDistance=1000;
        //setting verts
        //size: 4*4096
        float max = 0;
        float scaleValue = 500;
        for (int i=0;i<4096;i++) {
            if (data[i] > max) max = data[i];
            //top left
            verts[4 * i] = new Vector3(0.5f*i-1024, scaleValue*data[i],zDistance);
            //top right
            verts[4 * i + 1] = new Vector3(0.5f * i + 0.5f - 1024, scaleValue*data[i], zDistance);
            //bottom left
            verts[4 * i + 2] = new Vector3(0.5f*i - 1024, -scaleValue*data[i], zDistance);
            //bottom right
            verts[4 * i + 3] = new Vector3(0.5f*i + 0.5f - 1024, -scaleValue*data[i], zDistance);
        }
        Debug.Log("Max: " + max);
        //seting uvs
        //size:4*4096
        for (int i=0;i<4096*4;i++) {
            uvs[i] = Vector2.zero;
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

    }
    public void StartCapturingSound() {

    }
}
