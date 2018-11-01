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
            float mouseY = Input.GetAxis("Mouse Y");
            if (magnitude + mouseY > max || magnitude + mouseY < min) continue;
            //floating point is imprecise
            else if (mouseY - min > 0.1||mouseY - min < -0.1) {
                magnitude += mouseY;
                valueChangeEvent.Invoke(magnitude);
                //magnitude (0-100), domain of rotation (0-300)
                //THE angle of rotation of the dial
                //magnitude(min-max) theta (0-300)
                theta = 300*(mouseY - min)/ (max - min);
                Debug.Log("mouseY: "+mouseY+" magnitude: "+magnitude+" |theta: "+theta);
#pragma warning disable CS0618 // Type or member is obsolete
                gameObject.GetComponent<RectTransform>().RotateAroundLocal(new Vector3(0, 0, 1), theta);
#pragma warning restore CS0618 // Type or member is obsolete
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
