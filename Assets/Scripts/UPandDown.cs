using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPandDown : MonoBehaviour
{   
    public float distancePerframe;
    public float moveDistance;
    private float countdistance = 0;
    private bool UP = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    private void FixedUpdate() {
        if ( UP){
            transform.position = new Vector3(transform.position.x, transform.position.y + distancePerframe, transform.position.z);
            countdistance += distancePerframe;
            if (countdistance >= moveDistance){
                countdistance = 0;
                UP = false;
            }
        }

        if (!UP){
            transform.position = new Vector3(transform.position.x, transform.position.y - distancePerframe, transform.position.z);
            countdistance +=distancePerframe;
            if (countdistance >= moveDistance){
                countdistance = 0;
                UP = true;
            }
        }        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
