using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool alive = true;

    private Rigidbody playerRb;

    public float speedLimitDefault;

    // [HideInInspector]
    public float speedLimit;

    [SerializeField]
    private float forwardForce;
    public float sideSpeedLimit;

    private GameObject currChunk;
    private GameObject nextChunk;



    [SerializeField]
    private short currentChapter = 1;
    
    [SerializeField]
    private short currentLevel = 2;
    
    [SerializeField]
    private short startChunk = 1;

    
    private short chunkCounter = 0;
    private short nextChunkId;


    private void Awake()
    {

        // clear the scene
        foreach (var chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            GameObject.Destroy(chunk);
        }

        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        speedLimit = speedLimitDefault;
        nextChunkId = startChunk;
    }

    private void Start()
    {
        currChunk = GameObject.Instantiate(Resources.Load("Prefabs/Chunk"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        nextChunk = GameObject.Instantiate(Resources.Load($"Prefabs/Chapters/{currentChapter}/{currentLevel}/{nextChunkId}"), new Vector3(0, 0, currChunk.transform.position.z + 50), Quaternion.identity) as GameObject;

        nextChunkId++;
        chunkCounter += 2; //spawned two empty chunks for the player to get oriented.

        PlayerCollisionHandler.collidedWithObstacle += onCollisionWithOsbtacle;
    }

    void Update()
    {
        if (playerRb.position.z > nextChunk.transform.position.z + 2) SwapChunks();
    }
    private void FixedUpdate()
    {
        if (alive)
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
    }

    void SwapChunks()
    {
        GameObject.Destroy(currChunk);
        currChunk = nextChunk;

        var resource = Resources.Load($"Prefabs/Chapters/{currentChapter}/{currentLevel}/{nextChunkId}");
        if (resource != null)
        {
            nextChunk = GameObject.Instantiate(Resources.Load($"Prefabs/Chapters/{currentChapter}/{currentLevel}/{nextChunkId}"), new Vector3(0, 0, chunkCounter * 50), Quaternion.identity) as GameObject;
        }
        else
        {
            Debug.Log($"Asset at Prefabs/Chapters/{currentChapter}/{currentLevel}/{nextChunkId} doesn't exist.");
            Debug.Log("Spawning empty chunk instead.");
            nextChunk = GameObject.Instantiate(Resources.Load($"Prefabs/Chunk"), new Vector3(0, 0, chunkCounter * 50), Quaternion.identity) as GameObject;
        }

        chunkCounter++;
        nextChunkId++;
    }

    void onCollisionWithOsbtacle()
    {
        PlayerCollisionHandler.collidedWithObstacle -= onCollisionWithOsbtacle; //unsubscribe self

        playerRb.gameObject.GetComponent<Controller>().enabled = false;
        alive = false;
        Invoke("Restart", 2);
    }

    void Restart()
    {
        alive = true;
        playerRb.gameObject.GetComponent<Controller>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
