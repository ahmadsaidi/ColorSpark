using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class basictut : MonoBehaviour
{
    public Text instruction;
    private PlayerController pc;
    private GameObject robot;
    public GameObject blast1;

    public GameObject green;
    public GameObject red;
    public GameObject blue;
    public GameObject box;
    public GameObject HLarrow;
    public GameObject Ramp;
    Shader outline;
    private GameObject arrow= null;
    private bool redisntpresent = true;
    private bool greenisntpresent = true;
    private int[] PauseStates = {1,5,6};
    int state = 0;



    void highlight(GameObject target){
        Vector3 position = new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z);
        arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
    } 

    
    // Start is called before the first frame update
    void Start()
    {
        // gm = FindObjectOfType<GameManager>();

        robot = GameObject.Find("NewModelRobot");
        pc = robot.GetComponent<PlayerController>();
        red.SetActive(false);
        blue.SetActive(false);
        green.SetActive(false);
        box.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && (PauseStates.Contains(state))){
            state +=1;
        }
        if (PauseStates.Contains(state)){
            pc.msgDisp.text = "Press A to continue";
            pc.msgDispTimer = 1000000000;
        }else{
            pc.msgDisp.text = "  ";
        }

        if (state == 0){
            pc.enabled = false;
            pc.chat = false;
            pc.cc.chat.text = "I cannot walk through it...";
            Vector3 position = new Vector3(Ramp.transform.position.x + 18, Ramp.transform.position.y + 15, Ramp.transform.position.z-5);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 1.05f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 1.05f);
            pc.msgDisp.text = "";
            state = 1;
        }
        
        // state 2 is active state player try to jump over the trench
        if (state == 2){
            pc.enabled = true;
            pc.cc.chat.text = "Maybe I can try to jump over it(A to jump)";
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 0.0f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 0.0f);
            Destroy(arrow);
            state = 3;
        }
        // jump tut finish show green and fly through gaps
        if (robot.transform.position.z<67 && state ==3 ){
            state =4;
        }

        if (state == 4){
            pc.enabled = false;
            pc.cc.chat.text = "That cliff is too large.\n I need some extra help";
            pc.rb.velocity = new Vector3(0,0,0);
            green.SetActive(true);
            highlight(green);
            state = 5;
        }
        if (state == 6){
            pc.cc.chat.text = "Look at that! lets get some Power Ups\n (green speeds you up)";
        }
        // state 7 is active state
        if (state == 7){
            pc.enabled = true;
        }
        if (state == 7 && pc.color == Color.green){
            Destroy(arrow);
            pc.msgDisp.text = "Hold RB to activate speed up && JUMP!";
        }

    }
}
