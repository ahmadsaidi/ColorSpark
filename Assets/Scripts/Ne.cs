
using UnityEngine;

public class Ne : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < 10)
        {
            if (Input.GetKey("w"))
            {

                rb.AddForce(0, 0, 100 * Time.deltaTime, ForceMode.VelocityChange);

            }
            else if (Input.GetKey("d"))
            {
                rb.AddForce(100 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
            else if (Input.GetKey("a"))
            {
                rb.AddForce(-100 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
            else if (Input.GetKey("s"))
            {
                rb.AddForce(0, 0, -100 * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
        if (Input.GetKeyUp("w"))
        {
            rb.velocity = Vector3.Scale(rb.velocity, new Vector3(0.5f,0.5f,0.5f));
        }
        
    }
}
