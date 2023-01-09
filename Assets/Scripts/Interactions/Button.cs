using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    GameObject relatedObject;

    [SerializeField]
    Material unlockedMaterial;

    enum Effect
    {
        UNLOCK,
        DESTROY
    }

    [SerializeField]
    private Effect effect = Effect.DESTROY;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        GetComponent<MeshCollider>().isTrigger = false;

        switch (effect)
        {
            case Effect.UNLOCK:
                Unlock();
                break;
            case Effect.DESTROY:
                Destroy();
                break;
        }

    }

    private void Unlock()
    {
        Rigidbody rb = relatedObject.GetComponent<Rigidbody>();

        foreach (var child in relatedObject.transform.GetComponentsInChildren<Rigidbody>())
        {
            child.mass = 0.05f;
            child.tag = "Untagged";
            child.constraints = RigidbodyConstraints.None;
            child.GetComponent<MeshRenderer>().material = unlockedMaterial;
        }
    }

    private void Destroy()
    {
        GameObject.Destroy(relatedObject);
    }
}
