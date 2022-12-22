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
    private float boostTargetSpeed;

    [SerializeField]
    private float slowTargetSpeed;

    private Rigidbody rb;
    private Controller controller;

    [SerializeField]
    private float obstacleCollisionTolerance = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<Controller>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Floor":
                controller.inAir = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY; //lock to floor
                break;
            case "Bouncy":
                rb.AddForce(collision.GetContact(0).normal * Mathf.Max(collision.impulse.magnitude * 1.15f, baseBounceForce), ForceMode.Impulse);
                break;
            case "Boost":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = boostTargetSpeed;
                break;
            case "Slow":
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speedLimit = slowTargetSpeed;
                break;
            case "Obstacle":
                if (collidedWithObstacle != null && collision.impulse.magnitude > obstacleCollisionTolerance)
                {
                    Debug.Log($"Died at chunk {collision.transform.parent.name} at block {collision.transform.name}");
                    collidedWithObstacle.Invoke();
                }
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
