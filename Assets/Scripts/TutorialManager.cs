using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject[] messages;
    private int index;
    float time = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(messages[0]);
        Debug.Log(messages[1]);
    }

    // Update is called once per frame
    void Update()
    {
     // time = time - Time.deltaTime;
    //  if (time <= 0) { 
     //     index = 1;
    //  }
        for (int i = 0; i < messages.Length; i++)
        {
            if (i == index)
            {
                messages[index].SetActive(true);
            }
            else
            {
                messages[index].SetActive(false);
            }
        }

        
        
    }
}
