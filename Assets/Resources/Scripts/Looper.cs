using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looper : MonoBehaviour {
    List<Loop> loopList;
    public GameObject gameManager;
    MyProcess chuck;
    bool isRecording;
    bool isPlayingLoop;
    //a private variable that holds the index of the loop that will be recorded when the user starts recording
    int currentLoopIndex;
    int selectedLoopIndex;
    public InputField newLoopNameField;
    // Use this for initialization
    void Start () {
        loopList = new List<Loop>();
        isRecording = false;
        chuck = gameManager.GetComponent<GameManager>().chuck;
        if (loopList.Count > 0) selectedLoopIndex = loopList.Count - 1;
        else selectedLoopIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ToggleLoopPlaying() {
        if (loopList[selectedLoopIndex]!=null) {
            //if its not playing a loop
            if (!isPlayingLoop) {
                //start playing

            }
            else if (isPlayingLoop) {
                //stop the loop

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

}
