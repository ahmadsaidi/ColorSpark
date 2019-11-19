using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class tut_yellow : MonoBehaviour
{
    public GameObject player;
    public GameObject engine;

    private GameObject b1;
    private GameObject b2;
    Text txt;
    // keeps track of which step the tut is going at now
    int stage = 0;

    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        // player.GetComponent<PlayerController>().greenPower();
        
    }
    private void Update()
    {
        b1 = player.GetComponent<PowerUps>().yellowbox1;
        b2 = player.GetComponent<PowerUps>().yellowbox2;
        if (player.GetComponent<PlayerController>().color == Color.yellow)
        {
            txt.text = "Yellow spark allows you to create 2 portals you can pass through \npress 'RB' to create portal and pass them \nNow press 'RB' to create a portal on this side of the room";
        }
        if (b1 && !b2 && player.transform.position.x <-66){
            txt.text = "walk toward the other side of the room";
        }
        if (b1 && !b2 && player.transform.position.x >-66){
            txt.text = "press 'RB' to create a portal here";
        }

        // if both b1 & b2 are created
        if(b1 != null && b2 != null){
            Debug.Log((b1.transform.position.x < -66 && b2.transform.position.x < -66));
            if ((b1.transform.position.x > -66 && b2.transform.position.x > -66 )|| 
            (b1.transform.position.x < -66 && b2.transform.position.x < -66)){
                txt.text = "You are stucked, you should place one portal on each side \nSo you can pick up the blue spark and teleport back\n Now press 'start' to restart";
            }else if(player.GetComponent<PlayerController>().color == Color.blue){
                txt.text = "walk toward the portal you just created, press 'RB' to teleport";
            }else{
                txt.text = "Press B to  expell the yellow power \n Then go pick up the blue spark";
            }

        }
        
        if (!b1 && !b2 && player.transform.position.x >-66){
            txt.text = "You are stucked, you should place one portal on each side \nSo you can pick up the blue spark and teleport back\n Now press 'start' to restart";
        }

        if (player.transform.position.x < -66 && player.GetComponent<PlayerController>().color == Color.blue)
        {
            txt.text = "walk toward the blue engine";
        }


        if (Vector3.Distance(player.transform.position, engine.transform.position)<10 && player.GetComponent<PlayerController>().color == Color.blue)
        {   
            txt.text = "press 'B' to the place the spark on the engine & finish the tutorial";
        }

        
    }
}