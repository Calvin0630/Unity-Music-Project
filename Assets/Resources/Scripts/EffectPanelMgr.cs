using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPanelMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void BringPanelToFront(RectTransform r) {
        r.SetAsLastSibling();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
