using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class tut_blue : MonoBehaviour
{
    public GameObject player;
    public GameObject cube;
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
        
        
        if (player.GetComponent<PlayerController>().color == Color.blue)
        {
            txt.text = "Blue spark increases your force to move light, unstable (semi-transparent) objects \nHold 'RB' key while you are blue to push them \nNow walk toward the transparent cube";
        }
        if (Vector3.Distance(player.transform.position, cube.transform.position)<15 && player.GetComponent<PlayerController>().color == Color.blue)
        {   
            txt.text = "The destination is too high that you cannot approach.\nHold 'RB' key and push the cube toward the destination to make a stair";
        }
        if(cube.transform.position.x>-95 && cube.transform.position.z<104 && cube.transform.position.z>78){
            txt.text = "Jump onto the box to reach the destination";
        }
        
    }
}