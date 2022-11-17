using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Rigidbody playerRb;

    public float speedLimitDefault;
    
    [HideInInspector]
    public float speedLimit;

    private GameObject currChunk;
    private GameObject nextChunk;


    private void Start() {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        speedLimit = speedLimitDefault;
        currChunk = GameObject.Find("DemoChunk");
        nextChunk = GameObject.Instantiate(Resources.Load("Prefabs/DemoChunk"), new Vector3(0, 0, currChunk.transform.position.z + 50), Quaternion.identity) as GameObject;
    }
    
    void Update()
    {
        if (playerRb.velocity.z < speedLimit)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, speedLimit);
        }

        if (playerRb.position.z > nextChunk.transform.position.z + 2) SwapChunks();
    }

    void SwapChunks() {
        GameObject.Destroy(currChunk);
        currChunk = nextChunk;
        nextChunk = GameObject.Instantiate(Resources.Load("Prefabs/DemoChunk"), new Vector3(0, 0, currChunk.transform.position.z + 50), Quaternion.identity) as GameObject;
    }
}
