using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
	MyProcess chuck;
	// Use this for initialization
	void Start () {
        chuck = new MyProcess();
		Debug.Log("Running Command");
		chuck.ExecuteCommand("chuck + virtual_keyboard.ck:70:.5:76");
        
		chuck.ExecuteCommand("chuck + sampler.ck:70:.5:76");
		Debug.Log("Done Command");
		//UnityEngine.Debug.Log("start: \n" +output+"\n :end");
	}

	// Update is called once per frame
	void Update () {

	}

    void StartQuiz() {

    }

	void OnApplicationQuit()
    {
		chuck.Close();
		UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
