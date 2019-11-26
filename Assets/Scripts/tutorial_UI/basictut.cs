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
    // canvas objects
    public GameObject RobotWindow;
    public GameObject EngineWindow;
    public GameObject Background;
    public GameObject ChatWindow;

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
    private int[] PauseStates = {-1,1,5,6,8,9};
    public Text Messager; 
    int state = -1;



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
            Background.SetActive(true);
            Messager.text = "Press A to continue";
            pc.canMove = false;
        }else{
            Messager.text = "  ";
            Background.SetActive(false);
        }
        if (state == -1){
            pc.chat = false;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            pc.cc.chat.text = "Hey, This is Max02, automated robot on Ark0-1.";
            Messager.text = "I will contact you through this channel\n Press A to continue";
            

        }

        if (state == 0){
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            pc.cc.chat.text = "I cannot walk through it...";
            Vector3 position = new Vector3(Ramp.transform.position.x + 18, Ramp.transform.position.y + 15, Ramp.transform.position.z-5);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 1.05f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 1.05f);
            state = 1;
        }
        if (state == 1){
            pc.canMove = false;
        }
        
        // state 2 is active state player try to jump over the trench
        if (state == 2){
            pc.canMove = true;
            pc.cc.chat.text = "Maybe I can try to jump over it";
            Messager.text = "Press A to jump";
            state = 3;
        }
        // jump tut finish 
        if (robot.transform.position.z<67 && state ==3 ){
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 0.0f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 0.0f);
            Destroy(arrow);
            state =4;
        }

        if (state == 4){
            pc.cc.chat.text = "That cliff is too large.\n I need some extra help";
            pc.rb.velocity = new Vector3(0,0,0);
            green.SetActive(true);
            highlight(green);
            state = 5;
        }
        if (state == 6){
            pc.cc.chat.text = "Look at that! lets get some Power Ups\n (green power speeds me up)";
        }
        // state 7 is active state
        if (state == 7){
            pc.canMove = true;
            Messager.text = "pick up the green sparkling!";
        }
        if (state == 7 && pc.color == Color.green){
            Destroy(arrow);
            pc.rb.velocity = new Vector3(0,0,0);
            state = 8;
            
        }
        if (state == 8){
            RobotWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            Messager.text = "the Robot Ability shows you the power I'm carrying now\n press A to continue";
        }
        if (state == 9){
            RobotWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            Messager.text = "the Engine Ability shows you the installed in the engine\n you will see later\n press A to continue";
            EngineWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
        }
        if (state ==10){
            Messager.text = "Lets Dash!\n press RB and Jump!";
            pc.canMove = true;
        }
        // in case jump failed
        if (robot.transform.position.y < -80 && state <= 10){
            pc.transform.position = new Vector3(-255,-9,55);
            // pc.transform.rotation = new Vector3(0,180,0);
        }else if (robot.transform.position.y < -80 && state <= 10){
            pc.transform.position = new Vector3(-255,-9,-28);
            // pc.transform.rotation = new Vector3(0,180,0);
        }

    }
}
