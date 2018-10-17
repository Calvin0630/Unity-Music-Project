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

    WasapiCapture capture = new WasapiLoopbackCapture();
    // Use this for initialization
    void Start () {
        cubes = new List<GameObject>();
        InstantiateBlocks();
        aCapture = gameObject.GetComponent<AudioCapture>();

        capture.Initialize();
        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);
    }
	
	// Update is called once per frame
	void Update () {

        // Since this is being changed on a seperate thread we do this to be safe
        lock (aCapture.barData) {
            SetCubeSizes(aCapture.barData);

        }
    }

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
    public void StartCapturingSound() {

    }
}
