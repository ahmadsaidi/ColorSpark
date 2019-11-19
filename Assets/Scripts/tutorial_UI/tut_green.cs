using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class tut_green : MonoBehaviour
{
    public GameObject player;
    Text txt;
    // keeps track of which step the tut is going at now
    int stage = 0;

    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }
    private void Update()
    {
        if (stage == 0 && Input.GetAxis("Horizontal") != 0)
        {
            txt.text = "If you get on the black trap on the floor you will die. \n Try Avoid it \nPush your left handle forward & backward to move";
            stage = 1;
        }
        if (stage == 1 && Input.GetAxis("Vertical") != 0)
        {
            txt.text = "Press 'A' to Jump across the barrier!";
            stage = 2;
        }
        if (stage == 2 &&  player.transform.position.x > -491)
        {
            txt.text = "Now walk through the green spark to get color spark";
            stage = 3;
        }
        if (player.GetComponent<PlayerController>().color == Color.green)
        {
            txt.text = "Green spark gives you power to speed up\nTry to hold down 'RB' key to activate";
            stage = 4;
        }
        if (stage == 4 && Input.GetButton("Jump") && player.GetComponent<PlayerController>().color == Color.green && Input.GetAxis("Vertical") != 0)
        {   
            txt.text = "Now rush toward the other side of trench!";
        }
    }
}