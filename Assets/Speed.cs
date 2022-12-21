using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speed : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField]
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Velocity: ";
        text.text += rb.velocity.ToString();

        text.text += "\nFPS: ";
        text.text += Mathf.Round(1/Time.deltaTime).ToString();
    }
}
