using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatController : MonoBehaviour
{
    public PlayerController pc;
    public Text chat;
    public bool putdown;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        if ((SceneManager.GetActiveScene().name == "EngineLevel1"))
        {
            pc.chat = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene().name == "EngineLevel1"))
        {
            Vector3 p = pc.transform.position;
            if (p.x < -120 && p.y > -7 && p.z < -59)
            {
                chat.text = "Golden Spark~~~~~ I am coming";
            }
        }
      
        
    }
}
