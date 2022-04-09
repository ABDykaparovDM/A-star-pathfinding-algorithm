using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{

    public static float MouseX = 0;
    public static float MouseY = 0;

    public static float TRx;

    public float DecreaseSmoothness = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        MouseX += Input.GetAxis("Mouse X");
        MouseY -= Input.GetAxis("Mouse Y");

        if (MouseY > 85f)
            MouseY = 85f;

        if (MouseY < -85f)
            MouseY = -85f;

        if (MouseX < -360f)
            MouseX = 0f;

        if (MouseX > 360f)
            MouseX = 0f;

        Quaternion Direction = Quaternion.Euler(MouseY, MouseX, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, Direction, Time.deltaTime * DecreaseSmoothness);
        TRx = transform.rotation.x;
    }
}
