using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Text textdisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    bool check;
    public Text continuetext;
    public GameObject engine;
    public GameObject allengines;
    public GameObject allIcons;
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
                continuetext.enabled = true;
            }
            else
            {
                continuetext.text = "A: No            B: Yes";
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

        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            if (check && Input.GetButtonDown("Fire1") && index == sentences.Length - 1)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    gm.StartGame();
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



  

    }
    
}
