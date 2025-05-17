using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float horizontalInput;
    public float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //move forward
        //transform.Translate(1, 0, 0);

        //move forward again
        //transform.Translate(Vector2.right);

        //move forward to the right at set speed (smoothly)
        //transform.Translate(Vector2.right * Time.deltaTime * speed);

        //get player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //move player forward with vertical input
        transform.Translate(Vector2.right * Time.deltaTime * speed * verticalInput);

        //rotate player with horizontal input
        //transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime * horizontalInput);

        //rotate player with horiztontal input
        //reverse when moving backwards
        if (verticalInput < 0)
        {
            //backwards
            transform.Rotate(Vector3.forward, -turnSpeed * Time.deltaTime * horizontalInput);

        }
        else
        {
            //forwards
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime * horizontalInput);

        }
    }
}
