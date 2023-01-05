using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockButton : MonoBehaviour
{
    [SerializeField]
    GameObject lockedBlock;

    [SerializeField]
    Material unlockedMaterial;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        GetComponent<MeshCollider>().isTrigger = false;
        Unlock();
    }

    private void Unlock() {
        Rigidbody rb = lockedBlock.GetComponent<Rigidbody>();

        foreach (var child in lockedBlock.transform.GetComponentsInChildren<Rigidbody>())
        {
            child.mass = 0.05f;
            child.tag = "Untagged";
            child.constraints = RigidbodyConstraints.None;
            child.GetComponent<MeshRenderer>().material = unlockedMaterial;
        }
    }
}
