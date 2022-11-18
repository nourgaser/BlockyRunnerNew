using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Rigidbody playerRb;

    public float speedLimitDefault;

    // [HideInInspector]
    public float speedLimit;

    [SerializeField]
    private float forwardForce;
    public float sideSpeedLimit;

    private GameObject currChunk;
    private GameObject nextChunk;


    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        speedLimit = speedLimitDefault;
        // currChunk = GameObject.Find("DemoChunk");
        // nextChunk = GameObject.Instantiate(Resources.Load("Prefabs/DemoChunk"), new Vector3(0, 0, currChunk.transform.position.z + 50), Quaternion.identity) as GameObject;
        for (int i = 0; i < 30; i++)
        {
            var temp = GameObject.Instantiate(Resources.Load("Prefabs/Chunk"), new Vector3(0, 0, i * 50), Quaternion.identity) as GameObject;
            temp.name = (i + 1).ToString();
        }
    }

    void Update()
    {


        // if (playerRb.position.z > nextChunk.transform.position.z + 2) SwapChunks();
    }
    private void FixedUpdate()
    {
        if (playerRb.velocity.z < speedLimit)
        {
            // playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, speedLimit);
            playerRb.AddForce(0, 0, forwardForce * Time.fixedDeltaTime);
        }

        if (Mathf.Abs(playerRb.velocity.x) > sideSpeedLimit)
        {
            playerRb.velocity = new Vector3(sideSpeedLimit * (playerRb.velocity.x > 0 ? 1 : -1), playerRb.velocity.y, playerRb.velocity.z);
        }
    }

    void SwapChunks()
    {
        GameObject.Destroy(currChunk);
        currChunk = nextChunk;
        nextChunk = GameObject.Instantiate(Resources.Load("Prefabs/DemoChunk"), new Vector3(0, 0, currChunk.transform.position.z + 50), Quaternion.identity) as GameObject;
    }
}
