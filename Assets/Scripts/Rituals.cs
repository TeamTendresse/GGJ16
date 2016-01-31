using UnityEngine;
using System.Collections;

public class Rituals : MonoBehaviour
{
    public float minDailySignTime;
    public float maxDailySignTime;

    private Player player;
    private Profile profile;

    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        profile = GameObject.FindObjectOfType<Profile>();
    }
	
	void Update ()
    {
        int currentHour = int.Parse(string.Format("{0:hh}", System.DateTime.Now));
        if (currentHour >= minDailySignTime && currentHour <= maxDailySignTime)
        {
            
        }
	}
}
