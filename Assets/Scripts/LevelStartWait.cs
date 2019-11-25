using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartWait : MonoBehaviour
{

    public PlayerController pc;
    public float waitTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        pc.enabled = false;
        StartCoroutine(waitForStartCamera());
    }

    IEnumerator waitForStartCamera()
    {
        yield return new WaitForSeconds(waitTime);
        pc.enabled = true;
    }
}
