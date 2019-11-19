using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class tut_red : MonoBehaviour
{
    public GameObject player;
    Text txt;

    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        player.GetComponent<PlayerController>().greenPower();

    }
    private void Update()
    {
        
        if (player.GetComponent<PlayerController>().color == Color.green)
        {
            txt.text = "You can only carry use only one power up each time\nPress 'B' to expell your current power up";
        }
        if (player.GetComponent<PlayerController>().color == Color.white){
            txt.text = "Walk through the Red spark";
        }
        if (player.GetComponent<PlayerController>().color == Color.red){
            txt.text = "Red spark allows you to destroy \n unstable, light (semi-transparent) materials"+
            "\nNow walk toward the unstable wall infront of you";
        }
        if (player.GetComponent<PlayerController>().color == Color.red && player.transform.position.x > -464)
        {   
            txt.text = "Press 'RB' to destroy it!";
        }
        if (player.GetComponent<PlayerController>().color == Color.red && player.transform.position.x > -450)
        {   
            txt.text = "Walk forward for the next tutorial";
        }
    }
}