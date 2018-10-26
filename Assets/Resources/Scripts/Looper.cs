using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour {
    List<Loop> loopList;
    public GameObject gameManager;
    MyProcess chuck;
    bool isRecording;
    // Use this for initialization
    void Start () {
        loopList = new List<Loop>();
        isRecording = false;
        chuck = gameManager.GetComponent<GameManager>().chuck;
        chuck.ExecuteCommand("chuck + record.ck:test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void RecordLoop() {
        //if its not already recording, start
        if (!isRecording) {
            chuck.ExecuteCommand("chuck + record.ck:UwU");
            isRecording = true;
        }
        //if its already recording, stop
        else {
            chuck.RemoveShred("record.ck");
            isRecording = false;
        }
    }

    public void PlayLoop() {

    }

}
