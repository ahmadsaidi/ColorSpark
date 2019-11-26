﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCollision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    float distance;
    public bool focus;

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
        focus = false;
    }
    void Update()
    {
        if (!focus)
        {
            Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
            RaycastHit hit;
            if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit) && hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "move")
            {
                distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                distance = maxDistance;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        }
        else
        {

        }

    }
}
