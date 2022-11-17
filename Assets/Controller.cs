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

    [SerializeField]
    private float boostTargetSpeed;

    [SerializeField]
    private float slowTargetSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.acceleration.x < -0.1)
        {
            rb.AddForce(new Vector3(-sideForce * Time.deltaTime, 0, 0));
        }

        if (Input.GetKey(KeyCode.D) || Input.acceleration.x > 0.1)
        {
            rb.AddForce(new Vector3(sideForce * Time.deltaTime, 0, 0));
        }

        if (!inAir && (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0))
        {
            inAir = true;
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Floor":
                inAir = false;
                break;
            case "Bouncy":
                rb.AddForce(collision.GetContact(0).normal * Mathf.Max(collision.impulse.magnitude, baseBounceForce), ForceMode.Impulse);
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
