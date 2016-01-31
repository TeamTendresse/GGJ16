using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    public bool isSavingGestures;
    public float minGestureLength;
    public Feedbacks feedbacks;

    private GestureRecognitionController gestureRecognitionController;
    private DotSpawner dotSpawner;

    private List<Vector2> points;
    private bool isDown;
    private Vector2 lastMousePosition;
    private int nextId;
    [SerializeField]
    private List<Gesture> savedGestures;
    private float OnGuiTimer;
    public bool hasDoneUnlockSign { get; set; }
    public float sleepTimer { get; private set; }

    void Start ()
    {
        gestureRecognitionController = GestureRecognitionController.instance;
        List<Gesture> gestures = new List<Gesture>();
        for (int i = 0; i < savedGestures.Count; i++)
        {
            gestures.Add(new Gesture());
            for (int j = 0; j < savedGestures[i].points.Count; j++)
            {
                gestures[i].points.Add(savedGestures[i].points[j]);
            }
        }
        gestureRecognitionController.Init(gestures);
        dotSpawner = GameObject.FindObjectOfType<DotSpawner>();

        points = new List<Vector2>();
        isDown = false;
        lastMousePosition = Vector2.zero;
        nextId = 0;
        hasDoneUnlockSign = false;
        sleepTimer = 0f;
    }
    
    void OnGUI ()
    {
        if (isSavingGestures)
        {
            if (Event.current.button == 1)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                if (Vector2.Distance(mousePosition, lastMousePosition) != 0)
                {
                    points.Add(mousePosition);
                    lastMousePosition = mousePosition;
                }
            }
        }
    }

    public void startSavingGestures()
    {
        savedGestures = new List<Gesture>();
        Debug.Log("Start saving gestures");
    }

    public void addGesture()
    {
        savedGestures.Add(new Gesture(points));
        Debug.Log("Saved a new gesture");
        points = new List<Vector2>();
        lastMousePosition = Vector2.zero;
        Debug.Log("Saving a new gesture");
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            gestureRecognitionController.addGesture(nextId, points);
            nextId++;
        }

        if (Input.GetMouseButtonDown(0))
        {
            points.Clear();
            isDown = true;
            sleepTimer = 0f;
        }

        if (isDown)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            if (Vector2.Distance(mousePosition, lastMousePosition) != 0)
            {
                points.Add(mousePosition);
                lastMousePosition = mousePosition;
            }
            sleepTimer = 0f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDown)
            {
                isDown = false;
                if (points.Count >= minGestureLength)
                {
                    Result res;
                    if (gestureRecognitionController.isSignOk(points, out res))
                    {
                        if (dotSpawner._Mode == DotSpawner.ModeSpawner.locked)
                        {
                            if (!hasDoneUnlockSign && int.Parse(res.name) == 0 && res.score >= gestureRecognitionController.minRecognitionScore)
                            {
                                hasDoneUnlockSign = true;
                                showSign(savedGestures[System.Int32.Parse(res.name)].points);
                            }
                        }
                        else if (dotSpawner._Mode == DotSpawner.ModeSpawner.unlocked)
                        {
                            showSign(savedGestures[System.Int32.Parse(res.name)].points);
                        }
                    }
                }
            }
            sleepTimer = 0f;
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    points.Clear();
                    isDown = true;
                    break;
                case TouchPhase.Moved:
                    if (isDown)
                    {
                        Vector2 mousePosition = Input.mousePosition;
                        mousePosition.y = Screen.height - mousePosition.y;
                        if (Vector2.Distance(mousePosition, lastMousePosition) != 0)
                        {
                            points.Add(mousePosition);
                            lastMousePosition = mousePosition;
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (isDown)
                    {
                        isDown = false;
                        if (points.Count >= minGestureLength)
                        {
                            Result res;
                            if (gestureRecognitionController.isSignOk(points, out res))
                            {
                                if (dotSpawner._Mode == DotSpawner.ModeSpawner.locked)
                                {
                                    if (!hasDoneUnlockSign && int.Parse(res.name) == 0 && res.score >= gestureRecognitionController.minRecognitionScore)
                                    {
                                        hasDoneUnlockSign = true;
                                        showSign(savedGestures[System.Int32.Parse(res.name)].points);
                                    }
                                }
                                else if (dotSpawner._Mode == DotSpawner.ModeSpawner.unlocked)
                                {
                                    showSign(savedGestures[System.Int32.Parse(res.name)].points);
                                }
                            }
                        }
                    }
                    break;
            }
            sleepTimer = 0f;
        }
#endif
        sleepTimer += Time.deltaTime;
    }

    public List<Vector2> getGesturePoints(int id){
        return savedGestures[id].points;
    }

    public void showSign (List<Vector2> points)
    {
        feedbacks.playVictory() ;
        dotSpawner.showSign(points);
    }
}
