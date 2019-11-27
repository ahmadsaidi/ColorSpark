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
    public GameObject engine1;
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


    void highlight(engineController target){
        Vector3 position = new Vector3(target.transform.position.x, target.transform.position.y + 20, target.transform.position.z);
        arrow = Instantiate(HLarrow, position, Quaternion.Euler(180,0,0)) ;
    }

    void Start()
    {
        instruction.text = stageInstructions[0];
        engine0 = GameObject.Find("engine0");
        engine1 = GameObject.Find("engine1");
        cs0  = engine0.GetComponent<engineController>();
        cs1  = engine1.GetComponent<engineController>();
        robot = GameObject.Find("NewModelRobot");
        stage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 0 && pc.canMove == true && arrows[0] == 1) {
            highlight(cs0);
            arrows[0] = 0;
        }

        if (cs0.color == Color.blue && stage < 2) {
            Destroy(arrow);
            arrows[0] = 1;
            stage = 1;
        }

        if (robot.transform.position.z < ( -150) && stage <3)
        {
            if (arrows[1] == 1 && pc.canMove == true) {
                arrows[1] = 0;
                arrows[0] = 1;
                highlight(cs1);
            }
            
            stage = 2;
        }

        if (cs1.color == Color.green && stage < 3)
        {
            Destroy(arrow);
            stage = 3;
            if (arrows[0] == 1) {
                arrows[0] = 0;
                highlight(cs0);
            }
        }

        if (cs0.color == Color.red && stage  == 3)
        {
            Destroy(arrow);
            stage = 4;
        }
        
        
        
        if (pc.canMove == false)
        {
            instruction.text = "";
        }
        else if (instruction != null )instruction.text = stageInstructions[stage];
    }
}
