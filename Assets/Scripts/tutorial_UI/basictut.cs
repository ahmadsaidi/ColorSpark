using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class basictut : MonoBehaviour
{
    private PowerUps powerups;
    private PlayerController pc;
    public GameObject blast;
    // canvas objects
    public GameObject RobotWindow;
    public GameObject EngineWindow;
    public GameObject Background;
    public GameObject ChatWindow;

    public GameObject green;
    public GameObject red;
    public GameObject blue;
    public GameObject goldenSpark;
    public GameObject box;
    public GameObject HLarrow;
    public GameObject Ramp;
    Shader outline;
    private GameObject arrow= null;
    private bool redisntpresent = true;
    private bool greenisntpresent = true;
    private int[] PauseStates = {-1,1,5,6,8,9,12,13,16,21,22,24};
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

        pc = GetComponent<PlayerController>();
        powerups = GetComponent<PowerUps>();
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
            Background.SetActive(false);
        }
        if (state == -1){
            pc.chat = false;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            pc.cc.chat.text = "Hey, This is Max02, automated robot on Ark0-1.";
            Messager.text = "I will contact you through this channel\n Press A to continue";
            

        }

        if (state == 0){
            pc.cc.chat.text = "I cannot walk through it...";
            Vector3 position = new Vector3(Ramp.transform.position.x + 19, Ramp.transform.position.y + 15, Ramp.transform.position.z-5);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 1.05f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 1.05f);
            state = 1;
        }
        if (state == 1){
            pc.canMove = false;
        }
        
        // jump tut
        if (state == 2){
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            pc.canMove = true;
            pc.cc.chat.text = "Maybe I can try to jump over it";
            Messager.text = "Press A to jump";
            state = 3;
        }
        if (transform.position.z<67 && state ==3 ){
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 0.0f);
            Ramp.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 0.0f);
            Destroy(arrow);
            state =4;
        }





        // green spark tut
        if (state == 4){
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            pc.cc.chat.text = "That cliff is too large.\n I need some extra help";
            pc.rb.velocity = new Vector3(0,0,0);
            green.SetActive(true);
            highlight(green);
            state = 5;
        }
        if (state == 6){
            pc.cc.chat.text = "Look at that! lets get some Power Ups\n (green power speeds me up)";
        }
        if (state == 7){
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
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
            EngineWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            Messager.text = "Lets Dash!\n press RB and Jump!";
            pc.canMove = true;
        }
        // in case jump failed
        if (transform.position.y < -80 && state <= 10){
            pc.transform.position = new Vector3(-255,-9,55);
            pc.transform.rotation =  Quaternion.Euler(0,180,0);
        }else if (transform.position.y < -80 && state > 10){
            pc.transform.position = new Vector3(-255,-9,-28);
            pc.transform.rotation =  Quaternion.Euler(0,180,0);
        }

        if (transform.position.z < -15 && state == 10 && transform.position.y > -10){
            pc.canMove = false;
            state = 11;
        }



        // red spart tut

        if (state == 11){
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            red.SetActive(true);
            highlight(red);
            state = 12;
        }
        if (state == 12){
            pc.cc.chat.text = "I must get through this Anti-radiation Cyrstal Wall. ";
        }
        if (state == 13){
            pc.cc.chat.text = "LOOK at what I found! \n (red spark allows you to blast crystals)";
        }
        if (state == 14){
            pc.canMove = true;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            Messager.text = "pick up the red sparkling!";
        }
        if (state == 14 && pc.color == Color.red){
            Destroy(arrow);
            Vector3 position = new Vector3(blast.transform.position.x - 17, blast.transform.position.y + 12, blast.transform.position.z-3);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
            state = 15;
        }
        if(state == 15 && pc.color == Color.red){
            Messager.text = "Lets blow the wall away!";
        }
        if (state == 15 && pc.color != Color.red){
            Messager.text = "pick up the red sparkling!";
        }
        if(state == 15 && pc.color == Color.red && transform.position.z<-64.25f){
            Messager.text = "Press RB to blast!";
        }

        if (blast == null && state == 15){
            Destroy(arrow);
            blue.SetActive(true);
            highlight(blue); 
            state = 16;
        }

        // blue spark tut
        if (state == 16){
            pc.canMove = false;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            pc.cc.chat.text = "Did you see that blue spark?\n Go get it, I want to show you something.";
        }
        if (state == 17){
            pc.canMove = true;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            Messager.text = "pick up the blue sparkling!";
        }
        if(state == 17 && pc.color == Color.blue){
            pc.canMove = false;
            Destroy(arrow);
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            pc.cc.chat.text = "Nice job, now create a portal here\n (I can build portals on the map with blue spark)";
            Messager.text = "press RB to create a portal";
        }
        if (state == 17 && Input.GetButtonDown("Jump")){
            state = 18;
        }
        if (state == 18){
            pc.cc.chat.text = "I can only create 2 portals, be careful!\n (But I can take portals back!)";
            Messager.text = "press X to take the portal back";
        }
        if (state == 18 && Input.GetButtonDown("Fire3")){
            state = 19;
        }
        if (state == 19){
            pc.canMove = true;
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            pc.cc.chat.text = "Lets build portals!";
            Messager.text = "build two portals(RB)\n Note: you can only create portal on empty space";
        }
        if (state == 20){
            pc.canMove = false;
            GameObject[] portals = GameObject.FindGameObjectsWithTag("tele");
            foreach (GameObject portal in portals){Destroy(portal);}
            pc.whitePower();
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            transform.position = new Vector3(-254,0,-92);
            pc.transform.rotation =  Quaternion.Euler(0,180,0);
            highlight(goldenSpark);
            state = 21;
        }
        if (state == 21){
            pc.cc.chat.text = "Did you see that GOLDEN SPARK? It's so Crucial!";
        }
        if (state == 22){
            pc.cc.chat.text = "It's Hanging too high, I dont think I can Jump that high";
        }
        if (state == 23){
            Destroy(arrow);
            box.SetActive(true);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 1.1f);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 1f);
            highlight(box);
            state = 24;
        }
        if (state == 24){
            pc.cc.chat.text = "There is a Box, I can carry and stand on it, Nice!";
        }
        if (state == 25){
            pc.canMove = true;
            pc.cc.chat.text = "Help me get the GOLDEN SPARK!";
            ChatWindow.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
            state = 26;
        }
        if (state == 26 ){
            Messager.text = "Go get that box";
            
        }
        if (state == 27 ){
            Messager.text = "Press X to pick up the box";
        }
        if (state == 27 && pc.carry){
            Destroy(arrow);
            highlight(goldenSpark);
            state = 28;
        }
        if (state == 28){
            Messager.text = "Help MAX02 get the gold sparkling\n You can Press B to drop the box";
        }
        Debug.Log(state);

    }


    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "tele" && powerups.tele_num == 2 && state == 19){
            state = 20;
        }
        if (collision.gameObject.tag == "move" && state == 26){
            arrow.SetActive(false);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 0f);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 0f);
            state = 27;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "move" && state == 27){
            arrow.SetActive(true);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Outline", 1.1f);
            box.GetComponent<Renderer>().sharedMaterial.SetFloat("_Limiter", 1f);
            state = 26;
        }
    }

}
