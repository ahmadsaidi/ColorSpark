using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;
    public Transform obstruction;
    float rotationSpeed; 
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        obstruction = player.transform;
        rotationSpeed = FindObjectOfType<PlayerController>().rotationSpeed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Horizontal") * rotationSpeed *Time.deltaTime, Vector3.up) * offset;
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position+Vector3.up*5);
        //viewObstructed();
    }

    void viewObstructed()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -offset, out hit, 200f))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                obstruction = hit.transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                if (Vector3.Distance(obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, player.transform.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime);
                }
            }
            else
            {
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(player.transform.position, transform.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * Time.deltaTime);
                }
            }
        }
        
    }
}
