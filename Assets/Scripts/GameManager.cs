using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;

    private Player player;
    private DotSpawner dotSpawner;
    private Text message;

    private float startTimer;
    private bool started;
    private bool unlocked;
    private float sleepTimer;

    private GameManager() { }

    public GameManager instance ()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<GameManager>();
        }
        return _instance;
    }

	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        dotSpawner = GameObject.FindObjectOfType<DotSpawner>();
        message = RectTransform.FindObjectOfType<Text>();

        dotSpawner.setMode(DotSpawner.ModeSpawner.silent);
        startTimer = 0f;
        started = false;
        unlocked = false;
        Screen.sleepTimeout = 10;
	}
	
	void Update ()
    {
        if (!started)
        {
            if (startTimer >= 2f)
            {
                message.enabled = false;
                started = true;
                dotSpawner.setMode(DotSpawner.ModeSpawner.locked);
            }
            startTimer += Time.deltaTime;
        }
        else if (!unlocked)
        {
            if (player.hasDoneUnlockSign)
            {
                unlocked = true;
                dotSpawner.setMode(DotSpawner.ModeSpawner.unlocked);
            }
        }
        else
        {
            if (player.sleepTimer >= 10f)
            {
                dotSpawner.setMode(DotSpawner.ModeSpawner.locked);
                unlocked = false;
                player.hasDoneUnlockSign = false;
            }
        }
	}
}
