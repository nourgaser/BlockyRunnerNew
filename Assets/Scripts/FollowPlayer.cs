using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;

    private Vector3 offset;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - playerTransform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + playerTransform.position;

        if (transform.position.y > 19.5f) transform.position = new Vector3(transform.position.x, 19.5f, transform.position.z);
    }
}
