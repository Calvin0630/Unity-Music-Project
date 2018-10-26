using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVarInitializer : MonoBehaviour {
    //this script is to change the variable on and off at the beginning so that unity detects the initial value. 
    //if this script is not here the value could be true, but setting.txt would say false
	// Use this for initialization
	void Awake () {
        Toggle t = gameObject.GetComponent<Toggle>();
        //have u tried turning it on and off again?
        t.isOn = !t.isOn;
        t.isOn = !t.isOn;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
