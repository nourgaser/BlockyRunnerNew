using UnityEngine;

public class DestroyPassedChunk : MonoBehaviour
{
    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        gm.passedChunk += Destroy;
    }

    private byte countdown = 2;
    void Destroy()
    {
        countdown--;

        if (countdown == 0)
        {
            gm.passedChunk -= Destroy;
            GameObject.Destroy(gameObject);
        }
    }
}
