using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartWait : MonoBehaviour
{

    public PlayerController pc;
    public float waitTime = 10;
    GameObject mainCamera;
    bool playing = true;
    CPC_CameraPath p;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        pc.canMove = false;
        p = GetComponent<CPC_CameraPath>();
        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        StartCoroutine(waitForStartCamera());
    }

    void Update()
    {
        if (playing)
        {
            pc.msgDisp.text = "Press A to skip";
            pc.msgDispTimer = (Mathf.Cos(Time.time*5) + 1);
        }

        

    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Fire1") && playing)
        {
            p.StopPath();
            p.selectedCamera.enabled = false;
            mainCamera.GetComponent<Camera>().enabled = true;
            pc.canMove = true;
            playing = false;
            pc.msgDispTimer = 0;
            p.selectedCamera = mainCamera.GetComponent<Camera>();
            StopAllCoroutines();
        }
    }

    IEnumerator waitForStartCamera()
    {
        yield return new WaitForSeconds(waitTime);
        p.selectedCamera.enabled = false;
        mainCamera.GetComponent<Camera>().enabled = true;
        pc.canMove = true;
        playing = false;
        pc.msgDispTimer = 0;
        p.selectedCamera = mainCamera.GetComponent<Camera>();
    }
}
