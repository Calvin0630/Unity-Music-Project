using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Looper : MonoBehaviour {
    public List<Loop> loopList;
    public GameObject gameManager;
    public GameObject loopScroller;
    MyProcess chuck;
    bool isRecording;
    bool isPlayingLoop;
    //a private variable that holds the index of the loop that will be recorded when the user starts recording
    int currentLoopIndex;
    int selectedLoopIndex;
    public InputField newLoopNameField;
    // Use this for initialization
    private void Awake() {
        

    }
    void Start () {
        loopList = new List<Loop>();
        //read existing loops from the loops folder
        string[] fileNames = ReadLoopsFromFolder();
        foreach (string s in fileNames) {
            Loop loop = new Loop(s);
            loopScroller.GetComponent<LoopScroller>().AddElement(loop.name);
            loopList.Add(loop);
        }
        isRecording = false;
        chuck = gameManager.GetComponent<GameManager>().chuck;
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void ToggleLoopPlaying(string name) {
        Debug.Log(name);
        Debug.Log("loopList==null   " + (loopList == null));
        Debug.Log("LooplistCount: " + loopList.Count);
        foreach (Loop loop in loopList) {
            if (loop.name ==name) {
                chuck.ExecuteCommand("chuck + Chuck_Scripts//PlayLoop:" + loop.name);
            }
        }
    }
    public void ToggleRecordLoop() {
        //if its not recording, start
        if (!isRecording) {
            string name = newLoopNameField.text;
            if (name == "") {
                name = loopList.Count.ToString();
            }
            Loop newLoop = new Loop(name);
            loopList.Add(newLoop);
            chuck.ExecuteCommand("chuck + Chuck_Scripts//record.ck:"+name);
            isRecording = true;
        }
        //if its already recording, set the state to not recording
        else {
            isRecording = false;
        }
    }

    public void PlayLoop() {

    }

    public string[] ReadLoopsFromFolder() {
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory()+"\\Loops", "*.wav");
        string[] fileNames = new string[files.Length];
        for (int i=0;i<files.Length;i++) {
            //remove the preceding path
            string s = Path.GetFileName(files[i]);
            //remove th ".wav" extension from the name
            s = s.Substring(0, s.Length - 4);
            fileNames[i] = s;
        }
        return fileNames;

    }

}
