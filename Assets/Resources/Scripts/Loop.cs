using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop {
    Looper looper;
    public string name;
    public bool isPlaying;
    MyProcess chuck;
	
    public Loop(string name_, Looper looper_) {
        name = name_;
        isPlaying = false;
        looper = looper_;
        chuck = looper.chuck;
    }
    /*
    public Loop(int id_, Looper looper_) {
        name = id_.ToString();
        isPlaying = false;
        looper = looper_;
        chuck = looper.chuck;
    }*/

    public void Record() {

    }
    public void TogglePlaying() {
        if (!isPlaying) {
            chuck.ExecuteCommand("chuck + Chuck_Scripts//PlayLoop.ck:"+ name);
            isPlaying = true;
        }
        else if (isPlaying) {
            chuck.ExecuteCommand("chuck + Chuck_Scripts//StopPlayLoop.ck:"+name);
            isPlaying = false;

        }

    }
    public List<Loop> ReadFromFile() {
        List<Loop> result = new List<Loop>();

        return result;
    }
}
