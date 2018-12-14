using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop {
    public string name;
	
    public Loop(string name_) {
        name = name_;
    }

    public Loop(int id_) {
        name = id_.ToString();
    }

    public void Record() {

    }
    public void togglePlaying() {

    }
    public List<Loop> ReadFromFile() {
        List<Loop> result = new List<Loop>();

        return result;
    }
}
