using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float sideForce;
    [SerializeField]
    private float baseBounceForce = 1;

    [SerializeField]
    private float jumpForce;

    private bool inAir;
    private bool moveLeft;
    private bool moveRight;
    private bool shouldJump;

    [SerializeField]
    private float boostTargetSpeed;

    [SerializeField]
    private float slowTargetSpeed;

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
    // Update is called once per frame
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

        if (jump) shouldJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Floor":
                inAir = false;
                break;
            case "Bouncy":
                rb.AddForce(collision.GetContact(0).normal * Mathf.Max(collision.impulse.magnitude*1.3f, baseBounceForce), ForceMode.Impulse);
                break;
            case "Boost":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = boostTargetSpeed;
                break;
            case "Slow":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = slowTargetSpeed;
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Boost":
            case "Slow":
                GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                gm.speedLimit = gm.speedLimitDefault;
                break;
        }
    }

}
