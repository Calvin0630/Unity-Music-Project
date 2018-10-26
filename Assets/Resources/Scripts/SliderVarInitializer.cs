using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVarInitializer : MonoBehaviour {

    //a stupid script that adds and subtracts a small value from the slider so that other scripts can have the relevant variable because
    //Sliders dont do nice Initialization. Fuck Unity!!!!!!!!!!!!!
    void Awake() {

        gameObject.gameObject.GetComponent<Slider>().value = gameObject.gameObject.GetComponent<Slider>().value + 0.001f;
        gameObject.gameObject.GetComponent<Slider>().value = gameObject.gameObject.GetComponent<Slider>().value - 0.001f;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
