using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeInputField : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InputField input = gameObject.GetComponent<InputField>();
        string text = input.text;
        input.text = "";
        input.text = text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
