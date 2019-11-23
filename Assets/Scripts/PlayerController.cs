using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float speed;
    public float rotationSpeed;
    // public float jumpspeed;
    public Color color = Color.white;
    PowerUps powerups;
    public Light led;
    public AudioSource tilePickupAudio;
    bool eat = true;
    bool jump = true;
    public bool carry = false;
    public GameObject pauseMenu;
    public GameObject controller;
    public RobotIcon Icon;
    public GameObject wheel1;
    public GameObject wheel2;
    bool paused = false;
    GameObject carryThing;
    GameManager gm;
    MusicManager mm;
    float currVerRot = 0;
    float currHorRot = 0;
    public Transform axel;
    Animator animator;
    bool stationary = true;
    public Transform cameraAnchorV;
    public Transform cameraAnchorH;
    private float curspeed;
    public float acceleration;
    float cameraSetBack = 2.5f;
    public bool control;
    bool hitWall = false;
    float lastHit = 0;
    public float msgDispTimer = 0;
    public Text msgDisp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 40;
        curspeed = 0f;
        acceleration = 2f;
        rotationSpeed = 75;
        rb.freezeRotation = true;
        powerups = rb.gameObject.GetComponent<PowerUps>();
        gm = FindObjectOfType<GameManager>();
        mm = FindObjectOfType<MusicManager>();
        tilePickupAudio = GetComponent<AudioSource>();
        Icon = FindObjectOfType<RobotIcon>();
        animator = GetComponent<Animator>();
        control = false;
        msgDisp.text = "";
    }
    private void Update()
    {
        if (transform.position.y < -100)
        {
            gm.LoseGame();

        }
        if (Input.GetAxis("Vertical") != 0)
        {
            curspeed += acceleration;
            if (curspeed > speed)
            {
                curspeed = speed;
            }
        }
        else if (curspeed != 0)
        {
            curspeed -= acceleration;
        }
        float translationx = Input.GetAxis("Vertical") * curspeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        float rotationv = Input.GetAxis("Camera Vertical") * rotationSpeed;
        float rotationh = Input.GetAxis("Camera Horizontal") * rotationSpeed;

        axel.Rotate(0, 0, -0.1f * translationx);
        translationx *= Time.deltaTime;
        rotationv *= Time.deltaTime;
        rotationh *= Time.deltaTime;
        rotation *= Time.deltaTime;

        if (currHorRot != 0 && translationx != 0)
        {
            transform.Rotate(0, currHorRot / 5, 0);
            cameraAnchorH.transform.Rotate(0, -currHorRot / 5, 0.0f);
            currHorRot -= currHorRot / 5;
        }
        transform.Rotate(0, rotation, 0);
        Vector3 forward_direction = transform.TransformDirection(Vector3.left);
        Vector3 forward_velocity = new Vector3(28 * forward_direction.z * translationx, rb.velocity.y, -28 * forward_direction.x * translationx);
        rb.velocity = forward_velocity;

        if (stationary && translationx != 0)
        {
            if (speed == 80)
            {
                animator.SetTrigger("startedSprinting");
            }
            else
            {
                animator.SetTrigger("startedWalking");
            }
        }
        stationary = translationx == 0;

        currVerRot = 0;

        if (rotationh != 0 && (currHorRot < 90 && currHorRot > -90))
        {
            currHorRot += rotationh;
            cameraAnchorH.transform.Rotate(0, rotationh, 0.0f);
            cameraSetBack = 2.5f;
        }
        else if (rotationh == 0 && (currHorRot > 0.01 || currHorRot < -0.01) && cameraSetBack < 0)
        {
            cameraAnchorH.transform.Rotate(0, -currHorRot / 10, 0.0f);
            currHorRot -= currHorRot / 10;
        }
        else if (rotationh == 0 && currHorRot < 0.1 && currHorRot > -0.1 && cameraSetBack < 0)
        {
            cameraAnchorH.transform.Rotate(0, -currHorRot, 0.0f);
            currHorRot = 0;
        }
        else if (rotationh == 0)
        {
            cameraSetBack -= Time.deltaTime;
        }




        if (Input.GetButtonDown("Fire1") && jump == true && paused == false)
        {
            rb.velocity += new Vector3(0, 40, 0);
            tilePickupAudio.PlayOneShot(mm.jump);
            animator.SetTrigger("startedJumping");
            jump = false;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward = new Vector3(-5 * forward.x, 8, -5 * forward.z);
            powerups.Createbox(transform.position + forward, color);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            powerups.GetEnginePower(transform.position);
        }

        var hitColliders = Physics.OverlapSphere(transform.position, 4);
        bool teleHere = false;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "tele" && powerups.tele_num > 0)
            {
                teleHere = true;
                break;
            }
        }
        if (Input.GetButtonDown("Fire3") && (color == Color.blue) && teleHere)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "tele" && powerups.tele_num > 0)
                {
                    Destroy(hitColliders[i].gameObject);
                    if (powerups.yellowbox2 != null)
                    {
                        Debug.Log("get");
                        powerups.yellowbox1 = powerups.yellowbox2;
                    }
 
                   

                    
                    powerups.tele_num--;
                }
            }
        }
        else if (Input.GetButtonDown("Fire3") && carry == false)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {

                if (hitColliders[i].tag == "move")
                {

                    carryThing = (hitColliders[i].gameObject);
                    carry = true;
                    tilePickupAudio.PlayOneShot(mm.pickUpBox);
                }
                //tilePickupAudio.PlayOneShot(mm.blastAudio);
            }
        }
        else if (Input.GetButtonDown("Fire2") && carry)
        {

            if (hitColliders.Length < 5)
            {
                Vector3 forward = transform.TransformDirection(Vector3.left);
                forward = new Vector3(3 * forward.z, 2, -3 * forward.x);
                carryThing.transform.position = transform.position + forward;
                carry = false;
                tilePickupAudio.PlayOneShot(mm.putDownBox);
            }
            else if (msgDisp)
            {
                msgDispTimer = 2;
                string msg = "Cannot place box here.";
                msgDisp.text = msg;
            }

        }


        if (carry && carryThing)
        {
            carryThing.transform.position = transform.position + new Vector3(0, 15, 0);
        }

        if (Input.GetKey("r"))
        {
            gm.RestartLevel();
        }

        if (Input.GetButtonDown("Jump") && (color == Color.red))
        {


            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "blast")
                {
                    Destroy(hitColliders[i].gameObject);
                }
                //tilePickupAudio.PlayOneShot(mm.blastAudio);
            }


        }

        if (Input.GetButton("Jump") && color == Color.green)
        {
            tilePickupAudio.PlayOneShot(mm.runfasterAudio);
            speed = 80;
        }
        else
        {
            speed = 40;
        }

        if (Input.GetButtonDown("Jump") && (color == Color.blue) && powerups.tele_num < 2)
        {
            Vector3 forward = transform.TransformDirection(Vector3.left);
            forward = new Vector3(10 * forward.z, 8, -10 * forward.x);
            powerups.Createtele(transform.position + forward, color);





        }
        else if (Input.GetButtonDown("Jump"))
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "tele" && powerups.tele_num == 2)
                {

                    float d1 = Vector3.Distance(powerups.yellowbox1.transform.position, transform.position);
                    float d2 = Vector3.Distance(powerups.yellowbox2.transform.position, transform.position);
                    if (d1 < d2)
                    {
                        tilePickupAudio.PlayOneShot(mm.teleportAudio);
                        Vector3 off = 2 * powerups.yellowbox2.transform.TransformDirection(Vector3.up);

                        transform.position = powerups.yellowbox2.transform.position + new Vector3(off.x, 0, off.z);
                    }
                    else if (d1 > d2)
                    {
                        tilePickupAudio.PlayOneShot(mm.teleportAudio);
                        Vector3 off = 2 * powerups.yellowbox1.transform.TransformDirection(Vector3.up);
                        transform.position = powerups.yellowbox1.transform.position + new Vector3(off.x, 0, off.z);
                    }
                }

                if (hitColliders[i].tag == "Fixedtele")
                {
                    teleController tc = hitColliders[i].GetComponent<teleController>();
                    GameObject other = tc.teleport_other;

                    tilePickupAudio.PlayOneShot(mm.teleportAudio);
                    Vector3 off = 2 * other.transform.TransformDirection(Vector3.up);
                    transform.position = other.transform.position + new Vector3(off.x, 0, off.z);
                }
            }
        }

        if (Input.GetButtonDown("Restart"))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            paused = true;
        }

        if (Input.GetButtonDown("Fire1") && paused && control == false)
        {
            pauseMenu.SetActive(false);
            jump = true;
            Time.timeScale = 1;
            paused = false;
        }

        if (Input.GetButtonDown("Fire2") && paused && control == false)
        {
            Time.timeScale = 1;
            paused = false;
            pauseMenu.SetActive(false);
            gm.RestartLevel();
        }

        if (Input.GetButtonDown("Fire3") && paused && control == false)
        {
            Time.timeScale = 1;
            paused = false;
            pauseMenu.SetActive(false);
            gm.MainMenu();
        }

        if (Input.GetButtonDown("Carry") && paused && control == false)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(false);
            controller.SetActive(true);
            control = true;
        }

        if (Input.GetButtonDown("Fire1") && paused && control == true)
        {
            Time.timeScale = 1;
            controller.SetActive(false);
            pauseMenu.SetActive(false);
            control = false;
        }

        if (!hitWall)
        {
            lastHit += Time.deltaTime;
        }
        msgDispTimer -= Time.deltaTime;
        if (msgDispTimer < 0)
        {
            msgDispTimer = 0;
        }
        if (msgDisp)
        {
            msgDisp.color = new Color(1, 1, 1, msgDispTimer / 2);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("sand"))
        {
            jump = false;
            StartCoroutine(dekroy(collision.collider.gameObject));
        }
        else if ((collision.collider.gameObject.CompareTag("wall") || collision.collider.gameObject.CompareTag("blast")) && lastHit > 1f)
        {
            hitWall = true;
            jump = false;
            lastHit = 0;
        }else{
            jump = true;
        }
    }

    IEnumerator dekroy(GameObject o)
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(o);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if ((collision.collider.gameObject.CompareTag("wall") || collision.collider.gameObject.CompareTag("blast")))
        {
            hitWall = false;
            jump = true;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        eatPower(other);
        if (other.gameObject.CompareTag("hole"))
        {
            gm.LoseGame();
        }
        else if (other.gameObject.CompareTag("finish"))
        {
            Debug.Log("win");
            gm.WinLevel();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("green") || other.gameObject.CompareTag("red") || other.gameObject.CompareTag("blue"))
        {
            eat = true;
        }
    }


    public void redPower()
    {
        ChangeColor(Color.red);
        color = Color.red;
        tilePickupAudio.PlayOneShot(mm.redAudio);
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Drill;
    }


    public void bluePower()
    {
        ChangeColor(Color.blue);
        tilePickupAudio.PlayOneShot(mm.blueAudio);
        color = Color.blue;
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Teleport;
    }

    public void greenPower()
    {
        tilePickupAudio.PlayOneShot(mm.greenAudio);
        ChangeColor(Color.green);
        color = Color.green;
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Rocket;
    }

    public void whitePower()
    {
        ChangeColor(Color.white);
        color = Color.white;
        Icon.GetComponent<Image>().color = Color.black;
        Icon.GetComponent<Image>().sprite = null;
    }

    void ChangeColor(Color color)
    {
        Material mymat1 = wheel1.GetComponent<Renderer>().material;
        mymat1.SetColor("_EmissionColor", color);
        Material mymat2 = wheel2.GetComponent<Renderer>().material;
        mymat2.SetColor("_EmissionColor", color);
    }



    void eatPower(Collider item)
    {
        
        if ((item.gameObject.CompareTag("green")) || (item.gameObject.CompareTag("blue")) ||
            (item.gameObject.CompareTag("red")))
        {
            if (eat){
                if (color == Color.blue){
                    GameObject spark = Instantiate(powerups.bluespark, transform.position+ new Vector3(0,3,0), Quaternion.identity);
                    eat = false;
                }
                else if (color == Color.green){
                    GameObject spark = Instantiate(powerups.greenspark, transform.position+ new Vector3(0,3,0), Quaternion.identity);
                    eat = false;
                }
                else if (color == Color.red){
                    GameObject spark = Instantiate(powerups.redspark, transform.position+ new Vector3(0,3,0), Quaternion.identity);
                    eat = false;
                }


                if (item.gameObject.CompareTag("green"))
                {
                    item.gameObject.SetActive(false);
                    greenPower();
                }
                else if (item.gameObject.CompareTag("blue"))
                {
                    item.gameObject.SetActive(false);
                    bluePower();
                }
                else if (item.gameObject.CompareTag("red"))
                {
                    item.gameObject.SetActive(false);
                    redPower();
                }
            }
        }
    }
}

