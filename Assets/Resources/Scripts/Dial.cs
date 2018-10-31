using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;
using UnityEngine.Events;

public class Dial : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    // magnitue scales according to max/min
    public float magnitude;
    public float max, min;
    public OnDialTurn valueChangeEvent;
    Slider slider;

    IEnumerator rotateDial;
    void Start() {
        //just for initialisation
        rotateDial = RotateDial();
        magnitude = min;

    }
    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("clicksssss");
        StartCoroutine(rotateDial);
    }

    IEnumerator RotateDial() {
        while (true) {
            float theta; 
            yield return new WaitForSeconds(0.01f);
            float yAxis = Input.GetAxis("Mouse Y");
            if (magnitude + yAxis > max || magnitude + yAxis < min) continue;
            //floating point is imprecise
            else if (yAxis - min < 0.1) {
                magnitude += yAxis;
                valueChangeEvent.Invoke(magnitude);
                //yAxis (0-100), domain of rotation (0-300)
                //THE angle of rotation of the dial
                //magnitude(min-max) theta (0-300)
                theta = -300*(yAxis - min)/ (max - min);
                Debug.Log("magnitude: "+magnitude+" |theta: "+theta);
                gameObject.GetComponent<RectTransform>().rotation = new Quaternion(theta, 1, 0, 0);
            }

        }
    }
    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData) {
        StopCoroutine(rotateDial);
        Debug.Log("clickn't");
    }
    /*
    public void OnClick() {
        Debug.Log("clicksssss");
    }

    public void OnRelease () {
        Debug.Log("clickn't");
    }
    */
}
