using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class engineController : MonoBehaviour
{

    public GameObject yellowbox1;
    public GameObject yellowbox2;
    public GameObject bridge;
    public GameObject objectToFloat;
    public GameObject floatCamera;
    public int floatHeight;
    public Color color;
    Vector3 begining;
    Vector3 ending;
    public bool trigger = false;

    PowerUps pu;
    public bool flo = true;
    public bool fall = false;
    int count = 0;
    public EngineIcon Icon;
    Animator left;
    Animator right;
    GameObject main;
    GameObject player;
    //MusicManager mm;
    public CPC_CameraPath path;
    public GameObject redGlow;
    public Text Ability;
    public Text EngineAbility;
    bool playing = false;
    CPC_Point robot;
    Vector3 ps, off;
    Coroutine running;


    // Start is called before the first frame update
    void Start()
    {
        color = Color.white;
        //mm = FindObjectOfType<MusicManager>();
        pu = FindObjectOfType<PowerUps>();

        main = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if (objectToFloat)
        {

            GameObject box = objectToFloat.transform.GetChild(0).gameObject;
            begining = box.gameObject.GetComponent<Float>().begin;
            Vector3 targetposition = box.gameObject.GetComponent<Float>().begin + new Vector3(0, floatHeight, 0);
            ending = targetposition;


        }


    }
    // Update is called once per frame
    void Update()
    {
        
       
        if (objectToFloat)
        {
            
            GameObject box = objectToFloat.transform.GetChild(0).gameObject;
            if (begining != box.gameObject.GetComponent<Float>().begin)
            {
                begining = box.gameObject.GetComponent<Float>().begin;
                Vector3 targetposition = box.gameObject.GetComponent<Float>().begin + new Vector3(0, floatHeight, 0);
                ending = targetposition;
            }
            
           
        }
        if (flo && trigger)
        {
            if (redGlow)
            {
                redGlow.SetActive(true);
            }
            

            for (int i = 0; i < objectToFloat.transform.childCount; i++)
            {
                Transform box = objectToFloat.transform.GetChild(i);
                float velocity = 6f;
                Vector3 targetposition = box.transform.position + new Vector3(0, 1, 0);
                float newPosition = Mathf.SmoothDamp(box.transform.position.y, targetposition.y, ref velocity, 6f);
                box.transform.position = new Vector3(box.transform.position.x, newPosition, box.transform.position.z);
            }

            //Debug.Log(objectToFloat.transform.GetChild(0).transform.position);
            //Debug.Log(ending);
            if (objectToFloat.transform.GetChild(0).transform.position.y > ending.y)
            {
                StartCoroutine(wait());

                IEnumerator wait()
                {
                    flo = false;
                    trigger = false;

                    yield return new WaitForSeconds(3f);

                    trigger = color == Color.red;
                    fall = true;
                }

            }

        }

        if (fall && trigger)
        {
            if (redGlow)
            {
                redGlow.SetActive(true);
            }

            Transform[] ts = objectToFloat.GetComponentsInChildren<Transform>();

            for (int i = 0; i < objectToFloat.transform.childCount; i++)
            {

                Transform box = objectToFloat.transform.GetChild(i);
                float velocity = -6f;
                Vector3 targetposition = box.transform.position - new Vector3(0, 1, 0);
                float newPosition = Mathf.SmoothDamp(box.transform.position.y, targetposition.y, ref velocity, 6f);
                box.transform.position = new Vector3(box.transform.position.x, newPosition, box.transform.position.z);
            }

            if (objectToFloat.transform.GetChild(0).transform.position.y < begining.y)
            {

                StartCoroutine(wait());

                IEnumerator wait()
                {
                    fall = false;
                    trigger = false;
                    
                    yield return new WaitForSeconds(3f);
                    trigger = color == Color.red;
                    flo = true;

                }
            }

        }

        if (Input.GetButtonDown("Fire1") && playing)
        {
            StartCoroutine(interuptPath());
            playing = false;
            player.GetComponent<PlayerController>().msgDispTimer = 0;
        } else if (playing)
        {
            player.GetComponent<PlayerController>().msgDisp.text = "Press A to Skip.";
            player.GetComponent<PlayerController>().msgDispTimer = 2f;
        }



    }




    public void red()
    {
        if (color == Color.red && objectToFloat)
        {
            if (objectToFloat.transform.childCount <= 0)
            {
                return;
            }
            path.points = new List<CPC_Point>();
            ps = player.transform.position;
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            robot = start;
            off = player.transform.position - main.GetComponent<Camera>().transform.position;
            CPC_Point end = new CPC_Point(floatCamera.transform.position, floatCamera.transform.rotation);
            path.points.Add(end);
            path.looped = false;
            player.GetComponent<PlayerController>().canMove = false;

            main.GetComponent<cameraCollision>().focus = true;
            path.PlayPath(3);

            StartCoroutine(startFloat());
            running = StartCoroutine(cameraPathRed());

            Icon.GetComponent<Image>().color = Color.white;
            Icon.GetComponent<Image>().sprite = Icon.Float;
        }

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.red);
        EngineAbility.color = Color.red;
        Ability.color = Color.red;
        Ability.text = "Float";

    }

    IEnumerator startFloat()
    {
        yield return new WaitForSeconds(3);
        Transform box = objectToFloat.transform.GetChild(0);

        if (color == Color.red && objectToFloat)
        {
            //fall = false;
            //flo = true;
            trigger = true;
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropRedAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropRedAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropRedAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropRedAudio);


        }
    }


    IEnumerator cameraPathRed()
    {
        playing = true;
        yield return new WaitForSeconds(3);
        path.points = new List<CPC_Point>();
        CPC_Point newPos = new CPC_Point(floatCamera.transform.position, floatCamera.transform.rotation);
        path.points.Add(newPos);
        robot.position = player.transform.position - off;
        yield return new WaitForSeconds(0.1f);

        if (ps.y - player.transform.position.y > 0.01)
        {
            robot.position += new Vector3(0, -16, 0);
        } else if (ps.y - player.transform.position.y < -0.01)
        {
            robot.position += new Vector3(0, 16, 0);
        }
        path.points.Add(robot);
        path.PlayPath(3);
        yield return new WaitForSeconds(3);
        playing = false;
        player.GetComponent<PlayerController>().msgDispTimer = 0;
        main.GetComponent<cameraCollision>().focus = false;
        player.GetComponent<PlayerController>().canMove = true;
    }

    public void blue()
    {
        if (color == Color.blue && yellowbox1 && yellowbox2)
        {
            path.points = new List<CPC_Point>();
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            robot = start;
            GameObject temp1 = new GameObject();
            temp1.transform.position = yellowbox1.transform.position + 25*yellowbox1.transform.TransformDirection(Vector3.up)+new Vector3(0, 0, 30);
            temp1.transform.LookAt(yellowbox1.transform.position + new Vector3(0,15,0));
            path.points.Add(new CPC_Point(temp1.transform.position, temp1.transform.rotation));

            temp1.transform.position = yellowbox2.transform.position + 25 * yellowbox2.transform.TransformDirection(Vector3.up) + new Vector3(0, 0, 30);
            temp1.transform.LookAt(yellowbox2.transform.position + new Vector3(0, 15, 0));
            path.points.Add(new CPC_Point(temp1.transform.position, temp1.transform.rotation));
            path.looped = true;
            path.PlayPath(7.5f);
            //slideDoors(true);
            StartCoroutine(buildTele(temp1));
            StartCoroutine(cameraPathBlue());



        }
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Teleport;

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Name.blue);
        EngineAbility.color = Name.blue;
        Ability.color = Name.blue;
        Ability.text = "Portal";
    }

    IEnumerator buildTele(GameObject temp1)
    {

        yellowbox1.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        yellowbox2.SetActive(true);
        yield return new WaitForSeconds(6f);
        
        Destroy(temp1);

    }

    IEnumerator cameraPathBlue()
    {
        playing = true;
        player.GetComponent<PlayerController>().canMove = false;
        main.GetComponent<cameraCollision>().focus = true;

        yield return new WaitForSeconds(7.5f);
        main.GetComponent<cameraCollision>().focus = false;
        player.GetComponent<PlayerController>().canMove = true;
        player.GetComponent<PlayerController>().msgDispTimer = 0;
        playing = false;
    }

    public void green()
    {
        if (color == Color.green && bridge)
        {


            path.points = new List<CPC_Point>();
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            robot = start;
            Camera bridgeCam = bridge.GetComponent<Camera>();
            CPC_Point end = new CPC_Point(bridgeCam.transform.position, bridgeCam.transform.rotation);
            path.points.Add(end);
            path.looped = true;
            path.PlayPath(6);
            
            StartCoroutine(buildBridge());
            running = StartCoroutine(cameraPathGreen());
            
            
        }
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.plateform;

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.green);
        EngineAbility.color = Color.green;
        Ability.color = Color.green;
       Ability.text = "Build";

    }

    IEnumerator buildBridge()
    {
        yield return new WaitForSeconds(3);
        //Camera bridgeCam = bridge.GetComponent<Camera>();
        //bridgeCam.enabled = true;
        //main.GetComponent<Camera>().enabled = false;
        for (int i = 0; i < bridge.transform.childCount; i++)
        {

            GameObject piece = bridge.transform.GetChild(i).gameObject;
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropGreenAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropGreenAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropGreenAudio);
            pu.pc.tilePickupAudio.PlayOneShot(pu.pc.mm.dropGreenAudio);
            piece.SetActive(true);

            yield return new WaitForSeconds(1f);

        }
        //main.GetComponent<Camera>().enabled = true;
        //bridgeCam.enabled = false;
        //main.GetComponent<cameraCollision>().focus = false;
    }

    IEnumerator cameraPathGreen()
    {
        playing = true;
        player.GetComponent<PlayerController>().canMove = false;
        main.GetComponent<cameraCollision>().focus = true;
        yield return new WaitForSeconds(3);
        path.PausePath();
        yield return new WaitForSeconds(bridge.transform.childCount);
        path.ResumePath();
        yield return new WaitForSeconds(3);
        main.GetComponent<cameraCollision>().focus = false;
        player.GetComponent<PlayerController>().canMove = true;
        player.GetComponent<PlayerController>().msgDispTimer = 0;
        playing = false;
    }

    IEnumerator interuptPath()
    {
        path.PausePath();
        path.points = new List<CPC_Point>();
        CPC_Point start;
        

        if (color == Color.red || color == Color.green)
        {
            StopCoroutine(running);
        }
        start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);

        path.points.Add(start);
        path.points.Add(robot);
        path.SetCurrentTimeInWaypoint(0);
        path.SetCurrentWayPoint(0);
        path.looped = false;
        path.UpdateTimeInSeconds(1);
        path.ResumePath();
        yield return new WaitForSeconds(1);
        path.points = new List<CPC_Point>();
        main.GetComponent<cameraCollision>().focus = false;
        player.GetComponent<PlayerController>().canMove = true;
    }

    public void white()
    {
        if (yellowbox1 || yellowbox2)
        {
            yellowbox1.SetActive(false);
            yellowbox2.SetActive(false);
        }

        trigger = false;
        if (redGlow)
        {
            redGlow.SetActive(false);
        }

       

        if (bridge)
        {
            StartCoroutine(collapseBridge());

            IEnumerator collapseBridge()
            {
                for (int i = 0; i < bridge.transform.childCount; i++)
                {
                    GameObject piece = bridge.transform.GetChild(i).gameObject;
                    piece.SetActive(false);
                    yield return new WaitForSeconds(0.1f);

                }

            }

            Icon.GetComponent<Image>().color = Color.black;
            Icon.GetComponent<Image>().sprite = null;
        };





        color = Color.white;
        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.black);
        EngineAbility.color = Color.white;
        Ability.color = Color.white;
        Ability.text = "";
    }

    void slideDoors(bool state)
    {
        left.SetBool("slide", state);
        right.SetBool("slide", state);
    }








}
