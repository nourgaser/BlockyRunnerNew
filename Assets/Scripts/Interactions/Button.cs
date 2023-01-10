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
        Unlock,
        Destroy,
        Spawn
    }

    [SerializeField]
    private Effect effect = Effect.Destroy;

    private void OnTriggerEnter(Collider collision)
    {
        GetComponent<MeshCollider>().isTrigger = false;

        switch (effect)
        {
            case Effect.Unlock:
                Unlock();
                break;
            case Effect.Destroy:
                Destroy();
                break;
            case Effect.Spawn:
                Spawn();
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

    private void Spawn() {
        relatedObject.SetActive(true);
    }
}
