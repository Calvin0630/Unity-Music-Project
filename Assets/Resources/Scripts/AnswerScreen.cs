using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script deactivates the object after a set time


public class AnswerScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable() {
        StartCoroutine(despawnAfterTime());
    }
    IEnumerator despawnAfterTime() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
