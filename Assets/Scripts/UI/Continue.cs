﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Text textdisplay;
    public string[] sentences ;
    private int index;
    public float typingSpeed;
    bool check;
    public Text continuetext;
    public GameObject engine;
    public GameObject allengines;
    public GameObject allIcons;
    private bool checkTut;
    public string myname;
    private bool multiple = false;
    private bool four = false;
    GameManager gm;
    

    void Start()
    {
        
        StartCoroutine(Type());
        gm = FindObjectOfType<GameManager>();
        
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textdisplay.text += letter;
            Debug.Log(textdisplay.text);
            yield return new WaitForSeconds(typingSpeed);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (textdisplay.text == sentences[index])
        {
            check = true;

            if ((SceneManager.GetActiveScene().name == "Introduction")  &&index == 0 && Input.GetButtonDown("Fire2"))
            {
                continuetext.enabled = false;
                
            }

            if ((SceneManager.GetActiveScene().name == "Introduction") && index == 4 && Input.GetButtonDown("Fire1"))
            {
                sentences[5] = "Thanks!!!!!! ";
                
            }
            if ((SceneManager.GetActiveScene().name == "Introduction") && index == 4 && Input.GetButtonDown("Fire2"))
            {
                sentences[5] = "Stupid Human!!!!!! No matter what, you gonna help me, otherwise I will die!!!!! ";
                

            }
            if ((SceneManager.GetActiveScene().name == "Introduction") && index == 7 && Input.GetButtonDown("Fire1"))
            {
                sentences[8] = "That's is fine, I will teach you all the color abilities ";
                checkTut = true;
                

            }
            if ((SceneManager.GetActiveScene().name == "Introduction") && index == 7 && Input.GetButtonDown("Fire2"))
            {
                sentences[8] = "You really have some skills. You can skip the tutorial now ";
         
                checkTut = false;


            }

            if ((SceneManager.GetActiveScene().name == "Introduction") && index == 9 )
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    myname = "My Spark Master";
                } else if (Input.GetButtonDown("Fire2"))
                {
                    myname = "My Sir";
                    
                }
                else if (Input.GetButtonDown("Fire3"))
                {
                    myname = "My Lady";
                  
                }
                else if (Input.GetButtonDown("Carry"))
                {
                    myname = "My Idiot";
   
                }
                Name.Myname= myname;

                if (checkTut)
                {
                    sentences[10] = myname + "! Nice to meet you!                                                     Let's us start the tutorial ";
                }
                else
                {
                    sentences[10] = myname + "! Nice to meet you!                                                      Let's us start the puzzle";
                }
                




            }

            

            if ((SceneManager.GetActiveScene().name == "Introduction2") && engine && index >0 && index < 3)
            {
                engine.SetActive(true);
            }
            if ((SceneManager.GetActiveScene().name == "Introduction2") && allengines && index >= 3)
            {
                engine.SetActive(false);
                allengines.SetActive(true);
            }

            if ((SceneManager.GetActiveScene().name == "Introduction2") && allIcons && index >= 4)
            {
                engine.SetActive(false);
                allIcons.SetActive(true);
            }

            // continuetext.SetActive(true);
            if (index < sentences.Length - 1)
            {
                if (SceneManager.GetActiveScene().name == "Introduction")
                {
                    if ( index == 0 || index == 4 ){
                        continuetext.enabled = false;
                        multiple = true;
                        four = false;
                    }else if ( index == 7 )
                        {
                        continuetext.text = "A: No            B: Yes";
                        continuetext.enabled = true;
                        multiple = true;
                        four = false;
                    }else if (index == 9)
                    {
                        continuetext.enabled = false;
                        multiple = false;
                        four = true;
                    }
                    else
                    {
                        continuetext.text = "Press A to Continue";
                        continuetext.enabled = true;
                        multiple = false;
                        four = false;
                    }
                }
                else
                {
                    //continuetext.enabled = true;
                }
                
            }
            else
            {
                //continuetext.text = "A: No            B: Yes";
                continuetext.enabled = true;
            }
            
        }

        if (check && Input.GetButtonDown("Fire1") && index < sentences.Length - 1)
        {
            continuetext.enabled = false;
           // continuetext.SetActive(false);
            index++;
            textdisplay.text = "";
            check = false;
            
            StartCoroutine(Type());
        }
        if (check && Input.GetButtonDown("Fire2") && index < sentences.Length - 1 && (multiple || four))
        {
            continuetext.enabled = false;
            // continuetext.SetActive(false);
            index++;
            textdisplay.text = "";
            check = false;

            StartCoroutine(Type());
        }

        if (check && Input.GetButtonDown("Fire3") && index < sentences.Length - 1 && four)
        {
            continuetext.enabled = false;
            // continuetext.SetActive(false);
            index++;
            textdisplay.text = "";
            check = false;

            StartCoroutine(Type());
        }

        if (check && Input.GetButtonDown("Carry") && index < sentences.Length - 1 && four)
        {
            continuetext.enabled = false;
            // continuetext.SetActive(false);
            index++;
            textdisplay.text = "";
            check = false;

            StartCoroutine(Type());
        }

        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            if (check && Input.GetButtonDown("Fire1") && index == sentences.Length - 1)
            {
                if (Input.GetButtonDown("Fire1") && checkTut)
                {
                    gm.StartGame();
                }


            }

            if (check && Input.GetButtonDown("Fire1") && index == sentences.Length - 1)
            {

                if (Input.GetButtonDown("Fire1") && checkTut == false)
                {
                    gm.StartPuzzle();
                }

            }
        }

        if (SceneManager.GetActiveScene().name == "Introduction2")
        {
            if (check && Input.GetButtonDown("Fire1") && index == sentences.Length - 1)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    gm.tut2();
                }


            }

            if (check && Input.GetButtonDown("Fire2") && index == sentences.Length - 1)
            {

                if (Input.GetButtonDown("Fire2"))
                {
                    gm.StartPuzzle();
                }

            }

        }

        void startcount()
        {
            continuetext.enabled = false;
            // continuetext.SetActive(false);
            index++;
            textdisplay.text = "";
            check = false;
            StartCoroutine(Type());
        }



  

    }
    
}
