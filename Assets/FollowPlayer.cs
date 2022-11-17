using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;

    private Vector3 offset;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - playerTransform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + playerTransform.position;
    }
}
