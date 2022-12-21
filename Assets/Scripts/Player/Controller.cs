using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float sideForce;
    [SerializeField]
    private float jumpForce;

    public bool inAir;
    private bool moveLeft;
    private bool moveRight;
    private bool shouldJump;

    private float quarterOfScreen;


    private void Start()
    {
        quarterOfScreen = Screen.width / 4;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (moveLeft)
        {
            rb.AddForce(-sideForce * Time.fixedDeltaTime, 0, 0);
        }
        if (moveRight)
        {
            rb.AddForce(sideForce * Time.fixedDeltaTime, 0, 0);
        }
        if (shouldJump)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            shouldJump = false;
            inAir = true;
        }
    }
    
    void Update()
    {

        bool left = false, right = false, jump = false;

        // TOUCH
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            foreach (Touch touch in touches)
            {
                if (touch.position.x > 3 * quarterOfScreen)
                {
                    right = true;
                }
                else if (touch.position.x < (quarterOfScreen))
                {
                    left = true;
                }
                if (!inAir && (touch.position.x > quarterOfScreen && touch.position.x < (3 * quarterOfScreen))) jump = true;
            }
        }

        // TILT
        // TODO: implement here

        // KEYBOARD
        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }
        if (!inAir && (Input.GetKeyDown(KeyCode.Space)))
        {
            jump = true;
        }


        if (right) moveRight = true;
        else moveRight = false;

        if (left) moveLeft = true;
        else moveLeft = false;

        if (jump)
        {
            rb.constraints = RigidbodyConstraints.None;
            shouldJump = true;
        }
    }
}
