using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    string DemoChunkPath = "Prefabs/Chapters/1/1/10";

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
    private short nextChunkId = 1;


    private short chunkCounter = 0;


    private void Awake()
    {

        // clear the scene
        foreach (var chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            GameObject.Destroy(chunk);
        }

        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        speedLimit = speedLimitDefault;
    }

    void SpawnChunk()
    {
        if (!AttemptSwap())
        {
            nextChunkId = 1;
            currentLevel++;
            if (!AttemptSwap())
            {
                currentLevel = 1;
                currentChapter++;

                if (!AttemptSwap())
                {
                    nextChunk = GameObject.Instantiate((GameObject)Resources.Load(DemoChunkPath), new Vector3(0, 0, chunkCounter * 50), Quaternion.identity);
                }
            }
        }

        chunkCounter++;
    }

    private void Start()
    {
        SwapChunks();
        SwapChunks();

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

        SpawnChunk();
    }

    bool AttemptSwap()
    {
        string path = $"Prefabs/Chapters/{currentChapter}/{currentLevel}/{nextChunkId}";
        GameObject chunk = (GameObject)Resources.Load(path);
        if (chunk != null)
        {
            nextChunk = GameObject.Instantiate(chunk, new Vector3(0, 0, chunkCounter * 50), Quaternion.identity);
            nextChunkId++;
        }
        else return false;
        return true;
    }

    void onCollisionWithOsbtacle()
    {
        PlayerCollisionHandler.collidedWithObstacle -= onCollisionWithOsbtacle; //unsubscribe self

        playerRb.gameObject.GetComponent<Controller>().enabled = false;
        alive = false;
        Invoke("Restart", 1);
    }

    void Restart()
    {
        alive = true;
        playerRb.gameObject.GetComponent<Controller>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
