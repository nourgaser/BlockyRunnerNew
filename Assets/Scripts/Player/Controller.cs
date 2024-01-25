using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float groundSideForce;
    [SerializeField]
    private float airSideForce;

    private float currentSideForce;

    [SerializeField]
    private float jumpForce;

    private bool moveLeft;
    private bool moveRight;
    private bool shouldJump;

    private float quarterOfScreen;

    private PlayerCollisionHandler collisionHandler;

    private bool inAir = false;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();

        collisionHandler = GetComponent<PlayerCollisionHandler>();

        quarterOfScreen = Screen.width / 4;

        collisionHandler.wentInAir += () => { currentSideForce = airSideForce; inAir = true; };
        collisionHandler.wentOnGround += () => { currentSideForce = groundSideForce; inAir = false; };
        currentSideForce = groundSideForce;
    }

    private void FixedUpdate()
    {
        if (moveLeft)
        {
            rb.AddForce(-currentSideForce * Time.fixedDeltaTime, 0, 0);
        }
        if (moveRight)
        {
            rb.AddForce(currentSideForce * Time.fixedDeltaTime, 0, 0);
        }
        if (shouldJump && !inAir)
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
                if ((touch.position.x > quarterOfScreen && touch.position.x < (3 * quarterOfScreen))) jump = true;
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
        if ((Input.GetKey(KeyCode.Space)))
        {
            jump = true;
        }


        if (right) moveRight = true;
        else moveRight = false;

        if (left) moveLeft = true;
        else moveLeft = false;

        if (jump && !inAir) shouldJump = true;
    }
}
