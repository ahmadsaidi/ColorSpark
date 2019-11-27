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
    public MusicManager mm;
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
    float dropCarryTimer = 0.5f;
    public bool canMove = true;
    private bool fixportal ;
    public ChatController cc;
    public bool chat;
    public GameObject body;
    public Text robotAbilty;
    public Text Abilty;
    public Text Chat;
    public Transform front;
    public Transform back;
    public Transform left;
    public Transform right;
    Vector3 carryStart;
    Vector3 carryTo;
    public Text max02;
    private bool OnWall = false;
    public GameObject fire;
    public GameObject blastFire;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 40;
        curspeed = 0f;
        acceleration = 2f;
        rotationSpeed = 75;
        rb.freezeRotation = true;
        powerups = GetComponent<PowerUps>();
        gm = FindObjectOfType<GameManager>();
        mm = FindObjectOfType<MusicManager>();
        tilePickupAudio = GetComponent<AudioSource>();
        Icon = FindObjectOfType<RobotIcon>();
        animator = GetComponent<Animator>();
        control = false;
        msgDisp.text = "";
        fixportal = true;
        cc =  FindObjectOfType<ChatController>();
        if (chat == false)
        {
            cc.gameObject.SetActive(false);
        }
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
            curspeed = 0;
        }
        float translationx = Input.GetAxis("Vertical") * curspeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        float rotationv = Input.GetAxis("Camera Vertical") * rotationSpeed;
        float rotationh = Input.GetAxis("Camera Horizontal") * rotationSpeed;

        axel.Rotate(0, 0, -0.1f * translationx);
        rotationv *= Time.deltaTime;
        rotationh *= Time.deltaTime;
        rotation *= Time.deltaTime;


        if (canMove){
            transform.Rotate(0, rotation, 0);
            Vector3 forward_direction = transform.TransformDirection(Vector3.left);
            Vector3 forward_velocity = new Vector3(1.1f * forward_direction.z * translationx, rb.velocity.y, -1.1f * forward_direction.x * translationx);
            rb.velocity = forward_velocity;
            if (currHorRot != 0 && translationx != 0)
            {
                transform.Rotate(0, currHorRot / 5, 0);
                cameraAnchorH.transform.Rotate(0, -currHorRot / 5, 0.0f);
                currHorRot -= currHorRot / 5;
            }

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
        }
        else{
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }


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



        var hitColliders = Physics.OverlapSphere(transform.position, 4);
        var hitColliderss = Physics.OverlapSphere(transform.position + new Vector3(0, 5, 0), 8);
        bool teleHere = false;
        bool engineHere = false;

        for (int i = 0; i < hitColliderss.Length; i++)
        {
            if (hitColliderss[i].tag == "tele" && powerups.tele_num > 0)
            {
                teleHere = true;
                break;
            }

        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "engine")
            {
                engineHere = true;
                break;
            }
        }


        if (Input.GetButtonDown("Fire1") && jump == true && paused == false && OnWall == false && canMove)
        {
            rb.velocity += new Vector3(0, 40, 0);
            tilePickupAudio.PlayOneShot(mm.jump);
            animator.SetTrigger("startedJumping");
            jump = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "move")
                {
                    if (chat)
                    {
                        cc.chat.text = "Jumping on a box is fun";
                    }

                    tilePickupAudio.PlayOneShot(mm.oh);
                }


            }


        } else if(Input.GetButtonDown("Fire1") && OnWall && paused == false && hitWall)
        {
            tilePickupAudio.PlayOneShot(mm.scared);
            if (chat)
            {
                cc.chat.text = Name.Myname + "," + "Do you wanna me dance on a wall or something???";
            }

            msgDispTimer = 2;
            string msg = "You can not Jump here";
            msgDisp.text = msg;
        }

        if (Input.GetButtonDown("Fire2") && color != Color.white)
        {
            if (Input.GetButtonDown("Fire2") && color != Color.white && canMove)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                if (engineHere == false)
                {

                    forward = new Vector3(-12 * forward.x, 5, -12 * forward.z);
                    powerups.Createbox(transform.position + forward, color);
                }
                else
                {
                    forward = new Vector3(-5 * forward.x, 5, -5 * forward.z);
                    powerups.Createbox(transform.position + forward, color);
                }


            }
        }

        else if (Input.GetButtonDown("Fire2") && color == Color.white && carry == false)
        {
            if (chat)
            {
                cc.chat.text = Name.Myname + "," +"Do you wanna drop my heart on the ground?";
            }
            tilePickupAudio.PlayOneShot(mm.question);
            msgDispTimer = 2;
            string msg = "There is nothing to drop";
            msgDisp.text = msg;
        }

        if (Input.GetButtonDown("Fire3") && canMove && !paused)
        {
            powerups.GetEnginePower(transform.position);
        }




        if (Input.GetButtonDown("Fire3") && (color == Color.blue) && teleHere)
        {


            for (int i = 0; i < hitColliderss.Length; i++)
            {
                if (chat)
                {
                    cc.chat.text = Name.Myname + "," + "Guess you put a portal in a wrong place, try to find a better place";
                }
                tilePickupAudio.PlayOneShot(mm.ah);
                if (hitColliderss[i].tag == "tele" && powerups.tele_num > 0)
                {
                    Destroy(hitColliderss[i].gameObject);
                    powerups.tele_num--;
                    //if (powerups.yellowbox1 != null && powerups.tele_num == 1)
                    //{
                    //    Debug.Log("get1");
                    //    powerups.yellowbox2 = null;
                    //}
                    //else if (powerups.yellowbox2 != null  && powerups.tele_num == 1)
                    //{
                    //    Debug.Log("get2");
                    //    powerups.yellowbox1 = powerups.yellowbox2;
                    //    powerups.yellowbox2 = null;
                    //}





                }
            }
        }
        else if (Input.GetButtonDown("Fire3") && carry == false)
        {
            bool boxhere = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {

                if (hitColliders[i].tag == "move")
                {
                    carryThing = (hitColliders[i].gameObject);
                    boxhere = true;


                    carry = true;
                    dropCarryTimer = 0.5f;
                    tilePickupAudio.PlayOneShot(mm.pickUpBox);
                    carryStart = carryThing.transform.position;
                    float df = (carryStart - front.position).sqrMagnitude;
                    float db = (carryStart - back.position).sqrMagnitude;
                    float dl = (carryStart - left.position).sqrMagnitude;
                    float dr = (carryStart - right.position).sqrMagnitude;
                    if (df < db && df < dr && df < dl)
                    {
                        carryTo = front.position;
                    } else if (db < dr && db < dl)
                    {
                        carryTo = back.position;
                    } else if(dr < dl)
                    {
                        carryTo = right.position;
                    } else
                    {
                        carryTo = left.position;
                    }
                    tilePickupAudio.PlayOneShot(mm.happy);
                    if (chat)
                    {
                        cc.chat.text = "I am carrying an object, where should I put it?";
                    }
                    for (int j = 0; j < carryThing.transform.childCount; j++)
                    {
                        Transform c = carryThing.transform.GetChild(j);
                        if (c.CompareTag("robot_light"))
                        {
                            c.gameObject.SetActive(true);
                        }
                    }
                }
                if (boxhere == false && engineHere == false && teleHere == false)
                {
                    if (chat)
                    {
                        cc.chat.text = Name.Myname + "," + "What do you wanna pick up? Maybe you should get closer";
                    }
                    tilePickupAudio.PlayOneShot(mm.question);
                    msgDispTimer = 2;
                    string msg = "There is nothing to pick up";
                    msgDisp.text = msg;

                }
                //tilePickupAudio.PlayOneShot(mm.blastAudio);
            }
        }
        else if (Input.GetButtonDown("Fire2") && carry)
        {

            if (hitColliders.Length < 5)
            {
                if (dropCarryTimer <= 0)
                {
                    Vector3 forward = transform.TransformDirection(Vector3.left);
                    forward = new Vector3(3 * forward.z, 2, -3 * forward.x);
                    carryThing.transform.position = transform.position + forward;
                    carry = false;
                    tilePickupAudio.PlayOneShot(mm.putDownBox);
                    dropCarryTimer = 0.5f;
                    if (carryThing.GetComponent<Float>()){
                        carryThing.GetComponent<Float>().begin = carryThing.transform.position;

                    }
                    for (int j = 0; j < carryThing.transform.childCount; j++)
                    {
                        Transform c = carryThing.transform.GetChild(j);
                        if (c.CompareTag("robot_light"))
                        {
                            c.gameObject.SetActive(false);
                        }
                    }
                    tilePickupAudio.PlayOneShot(mm.question);

                    if (chat)
                    {
                        cc.chat.text = "emmmmmmmm.......... " + Name.Myname + "," + "Do you think this is the right position to put this box???";
                    }
                }
            }
            else if (msgDisp)
            {
                msgDispTimer = 2;
                string msg = "Cannot place box here.";
                tilePickupAudio.PlayOneShot(mm.scared);
                msgDisp.text = msg;
                if (chat)
                {
                    cc.chat.text = "There are too many things here. I can  not put a box  here";
                }
            }


        }


        if (carry && carryThing)
        {
            //carryThing.transform.position = transform.position + new Vector3(0, 15, 0);
            if (dropCarryTimer > 0.25f)
            {
                carryThing.transform.position = Vector3.Slerp(carryStart, carryTo, 4 * (0.5f - dropCarryTimer));
            } else
            {
                carryThing.transform.position = Vector3.Slerp(carryTo, transform.position + new Vector3(0, 15, 0), 4 * (0.25f - dropCarryTimer));
            }
        }

        if (Input.GetKey("r"))
        {
            gm.RestartLevel();
        }

        if (Input.GetButtonDown("Jump") && (color == Color.red))
        {

            bool nothing = true;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "blast")
                {
                    Instantiate(blastFire, hitColliders[i].gameObject.transform.position + new Vector3(0,-10,0), Quaternion.identity);
                    
                    Destroy(hitColliders[i].gameObject);
                    tilePickupAudio.PlayOneShot(mm.blastAudio);
                    tilePickupAudio.PlayOneShot(mm.ability);
                    if (chat)
                    {
                        cc.chat.text = "Bomb!!!!!";
                    }

                    nothing = false;
                }

            }
            if (nothing)
            {
                if (chat)
                {
                    cc.chat.text = "Do you wanna me to blast air???";
                }
                msgDispTimer = 2;
                string msg = "There is nothing to blast";
                msgDisp.text = msg;
                ;
                tilePickupAudio.PlayOneShot(mm.question);
            }


        }

        if (Input.GetButton("Jump") && color == Color.green)
        {
            tilePickupAudio.PlayOneShot(mm.runfasterAudio);
            speed = 80;
            if (chat)
            {
                cc.chat.text = "Speed up!!!!!";
            }
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
            speed = 40;
        }

        if (Input.GetButtonDown("Jump") && (color == Color.blue) && powerups.tele_num < 2)
        {
            if (chat)
            {
                cc.chat.text = "Remember put portal in a spacious place. If you put portals in a corner or put two portals very close, believe me, you will wanna take them back";
            }
            Vector3 forward = transform.TransformDirection(Vector3.left);
            forward = new Vector3(5 * forward.z, 8, -5 * forward.x);
            powerups.Createtele(transform.position + forward, color);






        }
        //else if (Input.GetButtonDown("Jump"))
        //{



        //    for (int i = 0; i < hitColliders.Length; i++)
        //    {
        //        if (hitColliders[i].tag == "tele" && powerups.tele_num == 2)
        //        {

        //            float d1 = Vector3.Distance(powerups.yellowbox1.transform.position, transform.position);
        //            float d2 = Vector3.Distance(powerups.yellowbox2.transform.position, transform.position);
        //            if (d1 < d2)
        //            {
        //                tilePickupAudio.PlayOneShot(mm.teleportAudio);
        //                Vector3 off = 2 * powerups.yellowbox2.transform.TransformDirection(Vector3.up);

        //                transform.position = powerups.yellowbox2.transform.position + new Vector3(off.x, 0, off.z);
        //            }
        //            else if (d1 > d2)
        //            {
        //                tilePickupAudio.PlayOneShot(mm.teleportAudio);
        //                Vector3 off = 2 * powerups.yellowbox1.transform.TransformDirection(Vector3.up);
        //                transform.position = powerups.yellowbox1.transform.position + new Vector3(off.x, 0, off.z);
        //            }


        //        }

        //        if (hitColliders[i].tag == "Fixedtele")
        //        {
        //            teleController tc = hitColliders[i].GetComponent<teleController>();
        //            GameObject other = tc.teleport_other;

        //            tilePickupAudio.PlayOneShot(mm.teleportAudio);
        //            Vector3 off = 2 * other.transform.TransformDirection(Vector3.up);
        //            transform.position = other.transform.position + new Vector3(off.x, 0, off.z);



        //        }
        //    }
        //}




        if (Input.GetButtonDown("Restart")  && canMove == true)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            paused = true;
            canMove = false;
        }

        if (Input.GetButtonDown("Fire1") && paused && control == true)
        {
            Time.timeScale = 1;
            controller.SetActive(false);
            pauseMenu.SetActive(false);
            control = false;
            canMove = true;
            paused = false;
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
        dropCarryTimer = Mathf.Max(dropCarryTimer - Time.deltaTime, 0);
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("floor") || collision.collider.gameObject.CompareTag("move"))
        {
            jump = true;
        }

        if (collision.collider.gameObject.CompareTag("sand"))
        {
            jump = false;
            StartCoroutine(dekroy(collision.collider.gameObject));
        }
        else if ((collision.collider.gameObject.CompareTag("wall") || collision.collider.gameObject.CompareTag("blast")) && lastHit > 1f)
        {
            hitWall = true;
            lastHit = 0;
            tilePickupAudio.PlayOneShot(mm.question);
            if (chat)
            {
                cc.chat.text = "Damn, my head just hit the wall";
            }


        }

        if (collision.gameObject.tag == "tele" && powerups.tele_num == 2)
        {

            float d1 = Vector3.Distance(powerups.yellowbox1.transform.position, transform.position);
            float d2 = Vector3.Distance(powerups.yellowbox2.transform.position, transform.position);
            teleController tc1 = powerups.yellowbox1.gameObject.GetComponent<teleController>();
            teleController tc2 = powerups.yellowbox2.gameObject.GetComponent<teleController>();
            if (d1 < d2 && tc1)
            {
                StartCoroutine(startto1());
            }
            else if (d1 >= d2 && tc2)
            {
                StartCoroutine(startto2());
            }



            IEnumerator startto1()
            {
                Vector3 offset = powerups.yellowbox2.transform.position-powerups.yellowbox1.transform.position;
                offset.Normalize();
                float facing;
                Vector3 directionUP = powerups.yellowbox2.transform.TransformDirection(Vector3.forward);
                if (Vector3.Dot(offset,directionUP)>Vector3.Dot(offset, -directionUP)){
                    offset = 8 * directionUP;
                    facing = powerups.yellowbox2.transform.rotation.eulerAngles.y;
                }else{
                    offset = -8 * directionUP;
                    facing = powerups.yellowbox2.transform.rotation.eulerAngles.y + 180;
                }
                transform.position = powerups.yellowbox2.transform.position + new Vector3(offset.x, 0, offset.z);
                transform.rotation = Quaternion.Euler(0, facing, 0);
                tc1.tele = false;
                tilePickupAudio.PlayOneShot(mm.teleportAudio);
                yield return new WaitForSeconds(0.5f);
            }

            IEnumerator startto2()
            {
                Vector3 offset = powerups.yellowbox1.transform.position-powerups.yellowbox2.transform.position;
                offset.Normalize();
                float facing;
                Vector3 directionUP = powerups.yellowbox1.transform.TransformDirection(Vector3.forward);
                if (Vector3.Dot(offset,directionUP) > Vector3.Dot(offset, -directionUP)){
                    offset = 8 * directionUP;
                    facing = powerups.yellowbox1.transform.rotation.eulerAngles.y;
                }else{
                    offset = -8 * directionUP;
                    facing = powerups.yellowbox1.transform.rotation.eulerAngles.y + 180;

                }
                transform.position = powerups.yellowbox1.transform.position + new Vector3(offset.x, 0, offset.z);
                transform.rotation = Quaternion.Euler(0, facing, 0);
                tc2.tele = false;
                yield return new WaitForSeconds(0.5f);
                tilePickupAudio.PlayOneShot(mm.teleportAudio);





            }
            tilePickupAudio.PlayOneShot(mm.surprise);
            if (chat)
            {
                cc.chat.text = "Tell me where is the other side of the portal. It is not hell, right?";
            }
        }

    }

    IEnumerator dekroy(GameObject o)
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(o);
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.collider.gameObject.CompareTag("wall") || collision.collider.gameObject.CompareTag("blast")){
            OnWall = true;
        }else{
            OnWall = false;
        }
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
        }

        if (collision.gameObject.tag == "tele")
        {

            teleController tc2 = collision.gameObject.GetComponent<teleController>();

            StartCoroutine(wait());




            IEnumerator wait()
            {
                yield return new WaitForSeconds(1.5f);
                tc2.tele = true;


            }



        }



    }

    void OnTriggerEnter(Collider other)
    {
        eatPower(other);
        if (other.gameObject.CompareTag("hole"))
        {
            gm.LoseGame();
            tilePickupAudio.PlayOneShot(mm.sad);
        }
        else if (other.gameObject.CompareTag("finish"))
        {
            Debug.Log("win");
            gm.WinLevel();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fixedtele" )
        {
            teleController tc = other.gameObject.GetComponent<teleController>();
            if (tc.tele)
            {
                StartCoroutine(startto());

            }


            IEnumerator startto()
            {

                GameObject otherP = tc.teleport_other;
                teleController tc2 = otherP.gameObject.GetComponent<teleController>();
                tc2.tele = false;
                yield return new WaitForSeconds(1);

                //tilePickupAudio.PlayOneShot(mm.teleportAudio);
                tilePickupAudio.PlayOneShot(mm.dropBlueAudio);

                Vector3 off = 2 * otherP.transform.TransformDirection(Vector3.up);
                transform.position = otherP.transform.position + new Vector3(off.x, 0, off.z);
                if (chat)
                {
                    cc.chat.text = "Cool!!!!!!";

                }
                tilePickupAudio.PlayOneShot(mm.surprise);


            }



        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("green") || other.gameObject.CompareTag("red") || other.gameObject.CompareTag("blue"))
        {
            eat = true;
        }


        if (other.gameObject.tag == "Fixedtele")
        {

            teleController tc2 = other.gameObject.GetComponent<teleController>();

            StartCoroutine(wait());




            IEnumerator wait()
            {
                yield return new WaitForSeconds(1);
                tc2.tele = true;


            }



        }
    }


    public void redPower()
    {
        ChangeColor(Color.red);
        color = Color.red;
        tilePickupAudio.PlayOneShot(mm.redAudio);
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Drill;
        robotAbilty.text = "Blast";
        robotAbilty.color = Color.red;
        Abilty.color = Color.red;
        Chat.color = Color.red;
        max02.color = Color.red;
        tilePickupAudio.PlayOneShot(mm.ability);

        if (chat)
        {
            cc.chat.text = "Red power is inside me! I can blast red objects now!";
        }
    }


    public void bluePower()
    {
        ChangeColor(Color.blue);
        tilePickupAudio.PlayOneShot(mm.blueAudio);
        color = Color.blue;
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Teleport;
        robotAbilty.text = "Portal";
        robotAbilty.color = Color.blue;
        Abilty.color = Color.blue;
        Chat.color = Color.blue;
        max02.color = Color.blue;
        tilePickupAudio.PlayOneShot(mm.ability);
        if (chat)
        {
            cc.chat.text = "It is Blue Power!Power to generate two portals and then  travel across time and space!";
        }
    }

    public void greenPower()
    {
        tilePickupAudio.PlayOneShot(mm.greenAudio);
        ChangeColor(Color.green);
        color = Color.green;
        Icon.GetComponent<Image>().color = Color.white;
        Icon.GetComponent<Image>().sprite = Icon.Rocket;
        robotAbilty.text = "Rush";
        robotAbilty.color = Color.green;
        Abilty.color = Color.green;
        Chat.color = Color.green;
        max02.color = Color.green;
        tilePickupAudio.PlayOneShot(mm.ability);
        if (chat)
        {
            cc.chat.text = "Green is power of wind!Activate it for a long time , I can fly up high!";
        }
    }

    public void whitePower()
    {
        ChangeColor(Color.white);
        color = Color.white;
        Icon.GetComponent<Image>().color = Color.black;
        Icon.GetComponent<Image>().sprite = null;
        robotAbilty.text = "";
        robotAbilty.color = Color.white;
        Abilty.color = Color.white;
        Chat.color = Color.white;
        max02.color = Color.white;
        tilePickupAudio.PlayOneShot(mm.oh);
    }

    void ChangeColor(Color color)
    {

        foreach (Renderer joint in body.GetComponentsInChildren<Renderer>())
        {

            joint.material.SetColor("_EmissionColor", color);

        }


        //Material mymat1 = wheel1.GetComponent<Renderer>().material;
        //mymat1.SetColor("_EmissionColor", color);
        //Material mymat2 = wheel2.GetComponent<Renderer>().material;
        //mymat2.SetColor("_EmissionColor", color);
    }

    public void continueGame()
    {
        pauseMenu.SetActive(false);
        jump = true;
        Time.timeScale = 1;
        paused = false;
        canMove = true;
    }

    public void restart()
    {
        Time.timeScale = 1;
        paused = false;
        pauseMenu.SetActive(false);
        gm.RestartLevel();
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        paused = false;
        pauseMenu.SetActive(false);
        gm.MainMenu();
    }

    public void controls()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        controller.SetActive(true);
        control = true;
    }

    void eatPower(Collider item)
    {

        if ((item.gameObject.CompareTag("green")) || (item.gameObject.CompareTag("blue")) ||
            (item.gameObject.CompareTag("red")))
        {
            if (eat)
            {
                if (color == Color.blue)
                {
                    GameObject spark = Instantiate(powerups.bluespark, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                    eat = false;
                }
                else if (color == Color.green)
                {
                    GameObject spark = Instantiate(powerups.greenspark, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                    eat = false;
                }
                else if (color == Color.red)
                {
                    GameObject spark = Instantiate(powerups.redspark, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
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
