using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
   public  Vector3 begin;
    public bool stay;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        begin = transform.position;
  
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
