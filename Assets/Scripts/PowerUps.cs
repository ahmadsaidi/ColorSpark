using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{

    public GameObject bluespark;
    public GameObject greenspark;
    public GameObject redspark;
    public GameObject player;
    public GameObject tele;
    PlayerController control;
    public AudioSource tilePickupAudio;
    bool fast, teleport, highJump, push = false;
    PlayerController pc;
    public GameObject yellowbox1 =null;
    public GameObject yellowbox2 = null;
    public int tele_num = 0;
    MusicManager mm;
    public Text msgDisp;
    // public Color engine_color;


    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        mm = FindObjectOfType<MusicManager>();
        tilePickupAudio = mm.GetComponent<AudioSource>();


    }

    public void Createbox(Vector3 position, Color color)
    {
        if (pc.carry)
        {
            return;
        }

        var hitColliders = Physics.OverlapSphere(position, 6);
        Vector3 forward = pc.transform.TransformDirection(Vector3.forward);
        var hitCollidersFront = Physics.OverlapSphere(position + 5 * new Vector3(forward.x, 0, forward.z), 6);
        bool foundEngine = false;
        if (hitCollidersFront.Length > 2)
        {
            for (int i = 0; i < hitCollidersFront.Length; i++)
            {

                if (hitCollidersFront[i].tag == "engine")
                {
                    Vector3 newpos = hitCollidersFront[i].transform.position + new Vector3(0, 10, 0);
                    engineController gc = hitCollidersFront[i].GetComponent<engineController>();
                    pc.whitePower();
                    if (gc.color != Color.white)
                    {
                        GetEnginePower(position + 5 * new Vector3(forward.x, 0, forward.z));
                    }
                    foundEngine = true;
                    if (color == Color.blue )
                    {
                        GameObject spark = Instantiate(bluespark, newpos, Quaternion.identity);
                        spark.GetComponent<SparkController>().eat = false;
                        spark.GetComponent<SphereCollider>().isTrigger = false;
                        gc.color = Color.blue;
                        gc.blue();
                        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("tele");
                        for (var index = 0; index < gameObjects.Length; index++)
                        {
                            Destroy(gameObjects[index]);
                        }
                        yellowbox1 = null;
                        yellowbox2 = null;
                        tele_num = 0;
                        tilePickupAudio.PlayOneShot(mm.spark_to_engine);

                    }
                    if (color == Color.red )
                    {
                        GameObject spark = Instantiate(redspark, newpos, Quaternion.identity);
                        spark.transform.parent = hitCollidersFront[i].gameObject.transform;
                        spark.GetComponent<SparkController>().eat = false;
                        spark.GetComponent<SphereCollider>().isTrigger = false;
                        gc.color = Color.red;
                        gc.red();
                        tilePickupAudio.PlayOneShot(mm.spark_to_engine);

                    }
                    if (color == Color.green)
                    {
                        GameObject spark = Instantiate(greenspark, newpos, Quaternion.identity);
                        spark.GetComponent<SparkController>().eat = false;
                        spark.GetComponent<SphereCollider>().isTrigger = false;
                        gc.color = Color.green;
                        gc.green();
                        tilePickupAudio.PlayOneShot(mm.spark_to_engine);
                    }

                }

                // scripts for TUT1 
                if (hitCollidersFront[i].name == "mission")
                {
                    Vector3 newpos = hitCollidersFront[i].transform.position + new Vector3(0, 10, 0);
                    if (color == Color.blue)
                    {
                        GameManager gm = FindObjectOfType<GameManager>();
                        gm.WinLevel();
                    }
                }
            }
        }

        if (hitColliders.Length <= 2 && !foundEngine)
        {
            
            if (color == Color.blue)
            {
                GameObject spark = Instantiate(bluespark, position, Quaternion.identity);
                tilePickupAudio.PlayOneShot(mm.dropBlueAudio);
                pc.whitePower();
            }
            if (color == Color.red )
            {
                Instantiate(redspark, position, Quaternion.identity);
                tilePickupAudio.PlayOneShot(mm.dropRedAudio);
                pc.whitePower();
            }
            if (color == Color.green)
            {
                Instantiate(greenspark, position, Quaternion.identity);
                tilePickupAudio.PlayOneShot(mm.dropGreenAudio);
                pc.whitePower();
            }
        } else if (hitColliders.Length > 2 && !foundEngine && pc.color != Color.white)
        {
            string msg = "Cannot drop spark here.";
            if (msgDisp)
            {
                msgDisp.text = msg;
                pc.msgDispTimer = 2;
            }
        }
    }

    public void Createtele(Vector3 position, Color color)
    {
        var hitColliders = Physics.OverlapSphere(position, 6);
        Vector3 forward = pc.transform.TransformDirection(Vector3.left);
        var hitCollidersFront = Physics.OverlapSphere(position + 5 * new Vector3(forward.x, 0, forward.z), 6);
        Vector3 forwardDirection =  new Vector3 (-90 *forward.x, 0, -180 * forward.z);
        if (hitColliders.Length >= 1)
        {
            if (msgDisp)
            {
                string msg = "Cannot place portal here.";
                msgDisp.text = msg;
                pc.msgDispTimer = 2;
            }
            return;
        }
        if (tele_num == 0)
        {
            
            yellowbox1 = Instantiate(tele, position, Quaternion.Euler( 0 , 0,  0) );

        }
        if (tele_num == 1)
        {
            if (yellowbox1 == null)
            {
                yellowbox1 = Instantiate(tele, position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                yellowbox2 = Instantiate(tele, position, Quaternion.Euler(0, 0,  0));
            }
           // yellowbox2 = Instantiate(tele, position, Quaternion.Euler(-90, 0, -180 * forward.z));
        }
        tele_num++;

    }


    public void GetEnginePower(Vector3 position)
    {
        var hitColliders = Physics.OverlapSphere(position, 6);
        Vector3 newpos;
        Collider[] intersecting;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "engine" && pc.color == Color.white)
            {
                newpos = hitColliders[i].transform.position + new Vector3(0, 10, 0);
                intersecting = Physics.OverlapSphere(newpos, 3);
                engineController gc = hitColliders[i].GetComponent<engineController>();


                if (intersecting.Length < 1)
                {
                    return;
                }

                for (int j = 0; j < intersecting.Length; j++)
                {


                    if (intersecting[j].tag == "green")
                    {
                        intersecting[j].gameObject.SetActive(false);
                        pc.greenPower();
                        gc.white();
                        tilePickupAudio.PlayOneShot(mm.greenAudio);
                    }


                    if (intersecting[j].tag == "red")
                    {
                        intersecting[j].gameObject.SetActive(false);
                        pc.redPower();
                        gc.white();
                        tilePickupAudio.PlayOneShot(mm.redAudio);
                    }


                    if (intersecting[j].tag == "blue")
                    {
                        intersecting[j].gameObject.SetActive(false);
                        pc.bluePower();
                        GameObject Boxes = gc.objectToFloat;
                        gc.white();
                        gc.flo = false;
                        gc.fall = true;
                        tilePickupAudio.PlayOneShot(mm.blueAudio);
                    }




                }

            }
        }




    }








}









