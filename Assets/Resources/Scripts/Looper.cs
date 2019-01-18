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
    public MyProcess chuck;
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
        chuck = gameManager.GetComponent<GameManager>().chuck;
        loopList = new List<Loop>();
        //read existing loops from the loops folder
        string[] fileNames = ReadLoopsFromFolder();
        foreach (string s in fileNames) {
            Loop loop = new Loop(s, gameObject.GetComponent<Looper>());
            loopScroller.GetComponent<LoopScroller>().AddElement(loop.name, false);
            loopList.Add(loop);
        }
        isRecording = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void ToggleLoopPlaying(string name) {
        foreach (Loop loop in loopList) {
            if (loop.name ==name) {
                loop.TogglePlaying();
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
            //create the loop object and add it to loopList as well as add the UI element with LoopScroller
            Loop newLoop = new Loop(name, gameObject.GetComponent<Looper>());
            loopScroller.GetComponent<LoopScroller>().AddElement(name, true);
            loopList.Add(newLoop);
            chuck.ExecuteCommand("chuck + Chuck_Scripts//record.ck:"+name);
            isRecording = true;
        }
        //if its already recording, set the state to not recording
        else {
            isRecording = false;
            //start the loop playing after its been recorded
            loopList[loopList.Count - 1].TogglePlaying();

            //user must press tab for chuck to recognize the end of recording.
            //this makes sense because the user wont have time to grab the mouse and click a button at the end of their loop
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
