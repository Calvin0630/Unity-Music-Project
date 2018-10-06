using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
	MyProcess chuck;
    public GameObject cubePrefab;
    List<GameObject> cubes;
    System.Random rand;
	// Use this for initialization
	void Start () {
        rand = new System.Random();
        chuck = new MyProcess();
		chuck.ExecuteCommand("chuck + virtual_keyboard.ck:70:.5:76", false);
		chuck.ExecuteCommand("chuck + sampler.ck:70:.5:76", false);
        cubes = new List<GameObject>();
        InstantiateBalls();
        NaudioDemo nadio = new NaudioDemo();


	}

	// Update is called once per frame
	void Update () {
        for (int i=0;i<cubes.Count;i++) {
            cubes[i].transform.localScale= new Vector3(1, 20 *Mathf.Tan(Mathf.PI-(Mathf.PI/2)*Mathf.Abs(Mathf.Sin(Time.time)))* Mathf.Sin( (.005f*i) +Time.time)* Mathf.Sin((.009f * i) + 1/2*Time.time), 1);
        }
	}

    //the camera is at the origin looking down the +ve z direction
    void InstantiateBalls() {
        //spawns the balls in a partial circle centered around the origin
        //r=20 from theta = pi/4-3pi/4 where 0 is the x axis (to the right)
        //the roatation of each consecutive cube is .05 from the origin
        float r = 1000;
        Debug.Log(Mathf.Asin(r / .5f));
        float deltaRotation = Mathf.Asin(.5f / r);
        float startingRotation = Mathf.PI / 4;
        float endingRotation= 3*Mathf.PI/4;
        float currentRotation = startingRotation;
        int k = 0;
        //a loop to spawn the cubes and adds them to the cubes list
        while (currentRotation<=endingRotation) {

            Vector3 position = new Vector3(r*Mathf.Cos(currentRotation),0,r*Mathf.Sin(currentRotation) ); ;
            GameObject tmp = Instantiate(cubePrefab, position, Quaternion.identity);
            tmp.transform.SetParent(gameObject.transform);
            cubes.Add(tmp);
            currentRotation += deltaRotation;
            k++;
        }
        Debug.Log(k + " cubes would be made");

    }

	void OnApplicationQuit()
    {
		chuck.Close();
		UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
