using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerengine_tut : MonoBehaviour
{
    // Start is called before the first frame update
    public Text instruction;
    public int stage;
    public GameObject engine0;
    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public GameObject portal1;
    public GameObject portal2;
    private GameObject robot;
    public string[] stageInstructions = {"Get Blue Spark," +
                                         "Put spark on engine 0 (Press B)",
                                         "Use Predefined Teleports (RB)",
                                         "Put Green Spark on engine 1" + "Build the bridge",
                                         "Get Back to engine 0 and Get Spark Again (Press X)" ,
                                         "Put Red Spark on engine1"};

    private engineController cs0;
    private engineController cs1;
    public PlayerController pc;
    public GameObject HLarrow;
    private GameObject arrow= null;
    private int[] arrows = {1, 1};
    private bool check1;
    private bool check2;
    private bool checkPortal;
    private bool check3;
    private bool display;


    void highlight(engineController target){
        Vector3 position = new Vector3(target.transform.position.x, target.transform.position.y + 23, target.transform.position.z);
        arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
    }

    void Start()
    {
        instruction.text = stageInstructions[0];
        engine0 = GameObject.Find("engine0");
        cs0  = engine0.GetComponent<engineController>();

        robot = GameObject.Find("NewModelRobot");
        stage = 0;
        check1 = true;
        check2 = true;
        check3 = true;
        checkPortal= true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (red && stage == 3 && check3 && robot.GetComponent<PlayerController>().color != Color.red)
        {
            Destroy(arrow);
            Vector3 position = new Vector3(red.transform.position.x, red.transform.position.y + 10, red.transform.position.z);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180, 0, 0));
          
      
        }

        if (green && stage == 2 && check2 && robot.GetComponent<PlayerController>().color != Color.green)
        {
    
            Destroy(arrow);
            Vector3 position = new Vector3(green.transform.position.x, green.transform.position.y + 10, green.transform.position.z);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180, 0, 0));
         
        }
        if (blue && stage == 0 && check1 && robot.GetComponent<PlayerController>().color != Color.blue)
        {
            Destroy(arrow);

            Vector3 position = new Vector3(blue.transform.position.x, blue.transform.position.y + 10, blue.transform.position.z);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180, 0, 0));
            
          
        }

        if (stage == 1 && checkPortal)
        {
            Destroy(arrow);
            robot.GetComponent<PlayerController>().canPortal = false;

            Vector3 position = new Vector3(portal1.transform.position.x, portal1.transform.position.y + 15, portal1.transform.position.z);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180, 0, 0));
            

        }

        if (stage == 2 && checkPortal)
        {
          

            


        }
        if ( robot.GetComponent<PlayerController>().color == Color.blue && stage == 0)
        {
            Destroy(arrow);
            highlight(cs0);
            check1 = false;

        }
        if (robot.GetComponent<PlayerController>().color == Color.red && stage == 3)
        {
            Destroy(arrow);
            highlight(cs0);
            check3 = false;

        }
        if (robot.GetComponent<PlayerController>().color == Color.green && stage == 2 && robot.transform.position.z > (-100))
        {
            checkPortal = false;
            Destroy(arrow);
            highlight(cs0);
            
           


        }
        //if (stage == 0 && pc.canMove == true && arrows[0] == 1) {
        //    highlight(cs0);
        //    arrows[0] = 0;
        //}

        

        if (cs0.color == Color.blue && stage < 2) {
           
            stage = 1;
        }

        if (robot.transform.position.z < (-150) && stage == 2 && robot.GetComponent<PlayerController>().color == Color.green)
        {


            Destroy(arrow);
            Vector3 position = new Vector3(portal2.transform.position.x, portal2.transform.position.y + 15, portal2.transform.position.z);
            arrow = Instantiate(HLarrow, position, Quaternion.Euler(180, 0, 0));
        }

        if (robot.transform.position.z < ( -150) && stage <3)
        {
            
            
            stage = 2;
            checkPortal = false;
        }

        if (cs0.color == Color.green && stage < 3)
        {
            Destroy(arrow);
            stage = 3;
            
            highlight(cs0);
            
        }

        if (cs0.color == Color.red && stage  == 3)
        {
            Destroy(arrow);
            stage = 4;
        }
        if (pc.cc.chat.text == "Cool!!!!!!")
        {
            pc.cc.chat.text = stageInstructions[stage];
        }


        if (robot.GetComponent<PlayerController>().canMove == false)
        {
            display = false;
            instruction.text = "";
        }
        //else if (instruction != null )instruction.text = stageInstructions[stage];
        else if (instruction != null && robot.GetComponent<PlayerController>().canMove) pc.cc.chat.text = stageInstructions[stage];
    }
}
