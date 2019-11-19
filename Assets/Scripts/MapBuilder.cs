using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject floor;
    public GameObject eastwall;
    public GameObject westwall;
    public GameObject southwall;
    public GameObject northwall;
    public float width;
    public float height;
    public float xoffset;
    public float zoffset;
    public float yoffset;


    public bool have_wall;
    public bool has_floor;

    public float westoffsetz;
    public float eastoffsetz;
    public float southoffsetx;
    public float northoffsetx;

    public float westoffsetx;
    public float eastoffsetx;
    public float southoffsetz;
    public float northoffsetz;

    //public GameObject eastsouthCorner;
    // public GameObject westsouthCorner;



    void Start()
    {
        if (has_floor)
        {
            FloorGenerator();
        }

        if (have_wall){
            EastGenerator();
            WestGenerator();
            SouthGenerator();
            NorthGenerator();
        }

    }

    void FloorGenerator()
    {

        for (int x = 0; x < height *2; x++)
        {
            for (int z = 0; z < width *2; z++)
            {
                GameObject temp = Instantiate(floor);
                temp.transform.position = new Vector3(xoffset +  x * 50, yoffset, zoffset + z * 50);
            }
        }
    }

     void WestGenerator()
    {

       // GameObject corner = Instantiate(westsouthCorner);
     ///  corner.transform.position = new Vector3(xoffset + 15, 61, -zoffset );

        if (westwall)
        {
            for (int x = 0; x < height; x++)
            {
                GameObject temp = Instantiate(westwall);
                temp.transform.position = new Vector3(xoffset + x * 40 + westoffsetx, yoffset, -zoffset + westoffsetz );
   
            }
        }

    }

    void EastGenerator()
    {
        if (eastwall)
        {
            for (int x = 0; x < height ; x++)
            {
                GameObject temp = Instantiate(eastwall);
                temp.transform.position = new Vector3(xoffset + x * 40 + eastoffsetx, yoffset, zoffset + eastoffsetz );
            }
        }

        //GameObject corner = Instantiate(eastsouthCorner);
       // corner.transform.position = new Vector3(xoffset , 0, zoffset);


    }

    void SouthGenerator()
    {
        if (southwall){
            for (int x = 0; x < width ; x++)
            {
                GameObject temp = Instantiate(southwall);
                temp.transform.position = new Vector3(xoffset + southoffsetx, yoffset, zoffset + x * 40 + southoffsetz);
            }
        }


    }

    void NorthGenerator()
    {
        if (northwall)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject temp = Instantiate(northwall);
                temp.transform.position = new Vector3(-xoffset + northoffsetx, yoffset, zoffset + x * 40 + northoffsetz);
            }
        }


    }

    void CornerGenerator()
    {
        

    }



}
