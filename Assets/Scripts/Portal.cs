using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Material mat1;
    public Material mat2;
    float which = 0;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (which < 0.1)
        {
            renderer.material = mat1;
        } else
        {
            renderer.material = mat2;
        }
        which += Time.deltaTime;
        if (which > 0.2)
        {
            which = 0;
        }
            
    }
}
