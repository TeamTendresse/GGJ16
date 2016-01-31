using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private Player player;
    private DotSpawner dotSpawner;
    private FeerieSpawner feerieSpawner;
    private Text message;
    private Button[] timeButtons;

    private float startTimer;
    private bool started;
    private bool unlocked;
    private float sleepTimer;

    private GameManager() { }

    public static GameManager instance ()
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
        feerieSpawner = GameObject.FindObjectOfType<FeerieSpawner>();
        message = GameObject.Find("Message").GetComponent<Text>();
        timeButtons = RectTransform.FindObjectsOfType<Button>();

        hideButtons();
        dotSpawner.setMode(DotSpawner.ModeSpawner.silent);
        feerieSpawner.setSilence(true);
        startTimer = 0f;
        started = false;
        unlocked = false;
        Screen.sleepTimeout = 10;
        
	}
	
	void Update ()
    {
        if (!started)
        {
            if (startTimer >= 2f || player.hasTouched)
            {
                message.enabled = false;
                started = true;
                dotSpawner.setMode(DotSpawner.ModeSpawner.locked);
                feerieSpawner.setSilence(false);
                showButtons();
            }
            startTimer += Time.deltaTime;
        }
        else if (!unlocked)
        {
            if (player.hasDoneUnlockSign)
            {
                unlocked = true;
                dotSpawner.setMode(DotSpawner.ModeSpawner.unlocked);
                hideButtons();
            }
        }
        else
        {
            if (player.sleepTimer >= 10f)
            {
                dotSpawner.setMode(DotSpawner.ModeSpawner.locked);
                unlocked = false;
                player.hasDoneUnlockSign = false;
                showButtons();
            }
        }
	}

    void showButtons ()
    {
        for (int i = 0; i < timeButtons.Length; i++)
        {
            timeButtons[i].enabled = true;
            timeButtons[i].GetComponent<Image>().enabled = true;
            timeButtons[i].GetComponentInChildren<Text>().enabled = true;
        }
    }

    void hideButtons()
    {
        for (int i = 0; i < timeButtons.Length; i++)
        {
            timeButtons[i].enabled = false;
            timeButtons[i].GetComponent<Image>().enabled = false;
            timeButtons[i].GetComponentInChildren<Text>().enabled = false;
        }
    }

    public void lockApp ()
    {
        dotSpawner.setMode(DotSpawner.ModeSpawner.locked);
        unlocked = false;
        player.hasDoneUnlockSign = false;
        showButtons();
    }
}
