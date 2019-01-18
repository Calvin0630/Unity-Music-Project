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
    public void AddElement(string name, bool isPlaying) {
        GameObject tmp = Instantiate(loopIconPrefab);
        tmp.transform.SetParent(contentPane);
        float loopButtonHeight = 0;
        RectTransform tmpRect = tmp.GetComponent<RectTransform>();
        //set the Variables of the taggle
        foreach (Transform t in tmp.transform) {
            //set the main image size
            if (t.gameObject.name == "Image") {
                loopButtonHeight = t.gameObject.GetComponent<RectTransform>().rect.height;
            }
            //add a listener to the Play button
            else if (t.gameObject.name =="PlayToggle") {
                t.GetComponent<Toggle>().isOn = isPlaying;
                t.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
                    loopy.GetComponent<Looper>().ToggleLoopPlaying(name);
                });
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
    public GameObject GetElement(string name) {
        foreach (GameObject obj in loopIconList) {
            if (obj.name==name) {
                return obj;
            }
        }
        return null;
    }
}
