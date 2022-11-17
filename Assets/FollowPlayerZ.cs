using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerZ : MonoBehaviour
{
    private Transform playerTransform;

    private float offset;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position.z - playerTransform.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, offset + playerTransform.position.z);
    }
}
