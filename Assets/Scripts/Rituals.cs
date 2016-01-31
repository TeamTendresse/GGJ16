using UnityEngine;
using System.Collections;

public class Rituals : MonoBehaviour
{
    public float minDailySignTime;
    public float maxDailySignTime;

    private Player player;


    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
	
	void Update ()
    {
        int currentHour = int.Parse(string.Format("{0:hh}", System.DateTime.Now));
        if (currentHour >= minDailySignTime && currentHour <= maxDailySignTime)
        {
            if (player.hasDoneSigned())
            {

            }
        }
	}
}
