using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionHandler : MonoBehaviour
{
    public static UnityAction collidedWithObstacle;
    public static UnityAction collidedWithFloor;

    [SerializeField]
    private float baseBounceForce = 1;

    [SerializeField]
    private float bounceMultiplier = 1.35f;

    [SerializeField]
    private float bounceFlatMultiplier = 3f;

    [SerializeField]
    private float boostTargetSpeed;

    [SerializeField]
    private float slowTargetSpeed;

    private Rigidbody rb;
    private Controller controller;

    [SerializeField]
    private float obstacleCollisionTolerance = 1f;

    private GameObject bottomContact = null;

    public UnityAction wentInAir;
    public UnityAction wentOnGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<Controller>();
    }

    private void Update()
    {
        justEntered = null;
    }

    Collision justEntered;
    private void OnCollisionEnter(Collision collision)
    {
        justEntered = collision;

        if (collision.GetContact(0).normal == new Vector3(0, 1, 0))
        {
            if (bottomContact == null) wentOnGround.Invoke();
            bottomContact = collision.gameObject;
        }

        switch (collision.gameObject.tag)
        {
            case "Bouncy":
                rb.AddForce(collision.GetContact(0).normal * Mathf.Max(collision.impulse.magnitude * bounceMultiplier + 3f, baseBounceForce), ForceMode.Impulse);
                break;
            case "Boost":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = boostTargetSpeed;
                break;
            case "Slow":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = slowTargetSpeed;
                break;
            case "Obstacle":
            case "Locked":
                if (collidedWithObstacle != null && collision.impulse.magnitude > obstacleCollisionTolerance)
                {
                    Debug.Log($"Died at chunk {collision.transform.parent.name} at block {collision.transform.name} at point {collision.GetContact(0).point}");
                    collidedWithObstacle.Invoke();
                }
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (justEntered?.gameObject.tag == "Untagged" || justEntered?.gameObject.tag == "Floor" || justEntered == null)
        {
            if (collision.gameObject == bottomContact)
            {
                bottomContact = null;
                wentInAir.Invoke();
            }

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
}
