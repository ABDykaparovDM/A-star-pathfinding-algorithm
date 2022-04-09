using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float MouseX;
    public float MouseY;

    public float DirectionX;
    public float DirectionZ;

    public float Base_speed = 1f;
    float speed;
    public float SpeedCheck;

    public Rigidbody RB;

    public Vector3 PzeroPos;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        PzeroPos = transform.position;

        MouseX = PlayerCameraScript.MouseX;
        MouseY = PlayerCameraScript.MouseY;

        if(Input.GetKey(KeyCode.LeftShift))
            speed = Base_speed * 2f;
        else
        if(Input.GetKey(KeyCode.LeftControl))
            speed = Base_speed * 0.5f;
        else
            speed = Base_speed;


        DirectionZ = (Mathf.Cos(MouseX / 180f * Mathf.PI)) * speed;
        DirectionX = (Mathf.Sin(MouseX / 180f * Mathf.PI)) * speed;
       // /*
        if (Input.GetKey("a"))
            if (!Input.GetKey("d"))
            {
                transform.position = PzeroPos + new Vector3(-DirectionZ, 0f, DirectionX);
                //print("a");
            }

        if (Input.GetKey("d"))
            if (!Input.GetKey("a"))
            {
                transform.position = PzeroPos + new Vector3(DirectionZ, 0f, -DirectionX);
            }

        if (Input.GetKey("w"))
        {
            if (!Input.GetKey("s"))
            {
                transform.position = PzeroPos + new Vector3(DirectionX, 0f, DirectionZ);
            }

            if (Input.GetKey("a"))
            {
                if (!Input.GetKey("d"))
                {
                    PzeroPos = transform.position;
                    transform.position = PzeroPos + new Vector3(-DirectionZ, 0f, DirectionX);
                    //print("a");
                }
            }
            else
            if (Input.GetKey("d"))
            {
                if (!Input.GetKey("a"))
                {
                    PzeroPos = transform.position;
                    transform.position = PzeroPos + new Vector3(DirectionZ, 0f, -DirectionX);
                }
            }
        }
        
        if (Input.GetKey("s"))
        {
            if (!Input.GetKey("w"))
            {
                transform.position = PzeroPos + new Vector3(-DirectionX, 0f, -DirectionZ);
            }

            if (Input.GetKey("a"))
            {
                if (!Input.GetKey("d"))
                {
                    PzeroPos = transform.position;
                    transform.position = PzeroPos + new Vector3(-DirectionZ, 0f, DirectionX);
                    //print("a");
                }
            }
            else
            if (Input.GetKey("d"))
            {
                if (!Input.GetKey("a"))
                {
                    PzeroPos = transform.position;
                    transform.position = PzeroPos + new Vector3(DirectionZ, 0f, -DirectionX);
                }
            }
        }
        //*/
        /*
        if (Input.GetKey("a"))
            if (!Input.GetKey("d"))
            {
                RB.velocity = new Vector3(-DirectionZ, 0f, DirectionX);
                print("a");
            }*/

        SpeedCheck = Vector3.Distance(PzeroPos, transform.position) * 10f;                //Speedometer
    }
}
