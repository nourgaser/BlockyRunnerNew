using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings instance;

    public static Settings Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;

            Application.targetFrameRate = 144;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
