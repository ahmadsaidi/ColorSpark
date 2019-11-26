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


    // Start is called before the first frame update
    void Start()
    {
        color = Color.white;
        //mm = FindObjectOfType<MusicManager>();
        pu = FindObjectOfType<PowerUps>();
        if (objectToFloat)
        {


            Transform box = objectToFloat.transform.GetChild(0);
            begining = (box.transform.position);
            Vector3 targetposition = box.transform.position + new Vector3(0, floatHeight, 0);
            ending = targetposition;







        }
        main = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        player = GameObject.FindGameObjectsWithTag("Player")[0];


    }
    // Update is called once per frame
    void Update()
    {
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
            Vector3 playerStartPos = player.transform.position;
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            Vector3 off = player.transform.position - main.GetComponent<Camera>().transform.position;
            CPC_Point end = new CPC_Point(floatCamera.transform.position, floatCamera.transform.rotation);
            path.points.Add(end);
            path.looped = false;
            player.GetComponent<PlayerController>().canMove = false;

            main.GetComponent<cameraCollision>().focus = true;
            path.PlayPath(3);

            StartCoroutine(startFloat());
            IEnumerator startFloat()
            {
                yield return new WaitForSeconds(3);
                floatCamera.SetActive(true);
                main.GetComponent<Camera>().enabled = false;
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
                yield return new WaitForSeconds(1.5f);

                main.GetComponent<Camera>().enabled = true;
                floatCamera.SetActive(false);
                path.points = new List<CPC_Point>();
                CPC_Point newPos = new CPC_Point(floatCamera.transform.position, floatCamera.transform.rotation);
                path.points.Add(newPos);
                start.position = player.transform.position - off;
                if (Mathf.Abs(playerStartPos.y - player.transform.position.y) > 0.1)
                {
                    start.position += new Vector3(0, 16, 0);
                }
                path.points.Add(start);
                path.PlayPath(3);
                yield return new WaitForSeconds(3);

                main.GetComponent<cameraCollision>().focus = false;
                player.GetComponent<PlayerController>().canMove = true;
            }

            Icon.GetComponent<Image>().color = Color.white;
            Icon.GetComponent<Image>().sprite = Icon.Float;
        }

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.red);
        EngineAbility.color = Color.red;
        Ability.color = Color.red;
        Ability.text = "Float";

    }


    public void blue()
    {
        if (color == Color.blue && yellowbox1 && yellowbox2)
        {
            path.points = new List<CPC_Point>();
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            GameObject temp1 = new GameObject();
            temp1.transform.position = yellowbox1.transform.position + 30*yellowbox1.transform.TransformDirection(Vector3.up)+new Vector3(0, 5, 0);
            temp1.transform.LookAt(yellowbox1.transform.position);
            path.points.Add(new CPC_Point(temp1.transform.position, temp1.transform.rotation));

            temp1.transform.position = yellowbox2.transform.position + 30 * yellowbox2.transform.TransformDirection(Vector3.up) + new Vector3(0, 5, 0);
            temp1.transform.LookAt(yellowbox2.transform.position);
            path.points.Add(new CPC_Point(temp1.transform.position, temp1.transform.rotation));
            path.looped = true;
            path.PlayPath(7.5f);
            //slideDoors(true);
            StartCoroutine(buildTele());
            IEnumerator buildTele()
            {
                player.GetComponent<PlayerController>().canMove = false;

                main.GetComponent<cameraCollision>().focus = true;
                
                yellowbox1.SetActive(true);
                yield return new WaitForSeconds(1.5f);

                yellowbox2.SetActive(true);
                yield return new WaitForSeconds(6f);

                main.GetComponent<cameraCollision>().focus = false;
                player.GetComponent<PlayerController>().canMove = true;
                Destroy(temp1);

            }



        }
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Teleport;

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.blue);
        EngineAbility.color = Color.blue;
        Ability.color = Color.blue;
        Ability.text = "Portal";
    }

    public void green()
    {
        if (color == Color.green && bridge)
        {
            player.GetComponent<PlayerController>().canMove = false;

            main.GetComponent<cameraCollision>().focus = true;

            path.points = new List<CPC_Point>();
            CPC_Point start = new CPC_Point(main.GetComponent<Camera>().transform.position, main.GetComponent<Camera>().transform.rotation);
            path.points.Add(start);
            Camera bridgeCam = bridge.GetComponent<Camera>();
            CPC_Point end = new CPC_Point(bridgeCam.transform.position, bridgeCam.transform.rotation);
            path.points.Add(end);
            path.looped = true;
            path.PlayPath(6);
            
            StartCoroutine(buildBridge());

            IEnumerator buildBridge()
            {
                yield return new WaitForSeconds(3);
                path.PausePath();
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

                path.ResumePath();
                yield return new WaitForSeconds(3);
                main.GetComponent<cameraCollision>().focus = false;
                player.GetComponent<PlayerController>().canMove = true;

            }



        }
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.plateform;

        Material m = transform.gameObject.GetComponent<Renderer>().material;
        m.SetColor("_EmissionColor", Color.green);
        EngineAbility.color = Color.green;
        Ability.color = Color.green;
       Ability.text = "Build";

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
