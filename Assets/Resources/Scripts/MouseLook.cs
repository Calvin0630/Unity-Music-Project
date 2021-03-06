﻿using System;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public bool enableXRotation;
    public bool enableYRotation;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start() {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update() {
        float mouseX;
        float mouseY;
        if (enableXRotation) mouseX = Input.GetAxis("Mouse X");
        else mouseX = 0;
        if (enableYRotation) mouseY = Input.GetAxis("Mouse X");
        else mouseY = 0;

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}