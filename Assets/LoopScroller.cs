using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScroller : MonoBehaviour {
    public Looper loopy;
    public RectTransform contentPane;
    public GameObject loopIconPrefab;
    public float loopButtonHeight;
    float scrollRectHeight;
    List<GameObject> loopIconList;
    void Awake() {
        loopIconList = new List<GameObject>();
        scrollRectHeight = gameObject.GetComponent<RectTransform>().rect.height;
        loopButtonHeight = 30;
    }
    // Use this for initialization
    void Start () {
       
        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddList(List<string> list) {
        foreach(string s in list) {
            AddElement(s);
        }
    }
    public void AddElement(string name) {
        GameObject tmp = Instantiate(loopIconPrefab);
        tmp.transform.SetParent(contentPane);
        float loopButtonHeight = 0;
        RectTransform tmpRect = tmp.GetComponent<RectTransform>();
        foreach (Transform t in tmp.transform) {
            if (t.gameObject.name == "Image") {
                loopButtonHeight = t.gameObject.GetComponent<RectTransform>().rect.height;
            }
            else if (t.gameObject.name =="PlayToggle") {
                t.onValueChanged.AddListener(loopy.GetComponent<Looper>().ToggleLoopPlaying(name));
            }
        }
        tmp.GetComponentInChildren<Text>().text = name;
        tmpRect.localScale = Vector3.one;
        //x is 0
        //the top of the cntentPane is 150, the bpttom is -150
        tmpRect.anchoredPosition = new Vector2( 0,
            -0.5f*scrollRectHeight+  (loopy.loopList.Count-1) * (loopButtonHeight+10));
        loopIconList.Add(tmp);
    }
}
