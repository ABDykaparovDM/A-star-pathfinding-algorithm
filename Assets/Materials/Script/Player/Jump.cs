using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public bool AtTheGround;
    public Rigidbody RB;
    public float JumpForce = 300f;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter()
    {
        AtTheGround = true;
    }

    void OnCollisionStay()
    {
        AtTheGround = true;
        if (Input.GetKey(KeyCode.Space) && AtTheGround)
        {
            RB.AddForce(0f, JumpForce, 0f);
            AtTheGround = false;
        }
    }

    void OnCollisionLeave()
    {
        AtTheGround = false;
    }
}
