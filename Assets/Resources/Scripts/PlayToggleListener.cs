using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayToggleListener : MonoBehaviour {
    Toggle toggle;
    // Use this for initialization
    void Start() {
        toggle = gameObject.GetComponent<Toggle>();

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("PlayLoop")) {
            Debug.Log("Play");
            if (toggle.isOn == false) {
                toggle.isOn = true;
            }
            else if (toggle.isOn == true) {
                toggle.isOn = false;
            }

        }
    }
}
