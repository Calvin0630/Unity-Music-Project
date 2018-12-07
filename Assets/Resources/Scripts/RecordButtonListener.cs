using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButtonListener : MonoBehaviour {
    Toggle toggle;
	// Use this for initialization
	void Start () {
        toggle = gameObject.GetComponent<Toggle>();

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Tab")) {
            if (toggle.isOn == false) {
                toggle.isOn = true;
            }
            else if (toggle.isOn == true) {
                toggle.isOn = false;
            }

        }
	}
}
