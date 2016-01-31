using UnityEngine;
using System.Collections;

public class Rituals : MonoBehaviour
{
    public Vector2[] times;

    private Player player;
    private Profile profile;

    private FeerieSpawner feerieSpawner;

    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        profile = GameObject.FindObjectOfType<Profile>();

        feerieSpawner = GameObject.FindObjectOfType<FeerieSpawner>();

        setTheRituals() ;
    }
	
	void Update ()
    {
        
	}

    public void setTheRituals(){
        int signId = 0 ;
        int currentHour = int.Parse(string.Format("{0:HH}", System.DateTime.Now));
        //hour = ampm == "AM" ? hour : (hour % 12) + 12;
        for (int i = 0; i < times.Length; i++)
        {
            if (currentHour >= times[i].x && currentHour <= times[i].y)
            {
                signId = i+1 ;
            }
        }
        Debug.Log(currentHour + " " + signId) ;
        feerieSpawner.selectSound(signId) ;
    }

    public bool isTheRightTime (int signId)
    {
        int currentHour = int.Parse(string.Format("{0:HH}", System.DateTime.Now));
        for (int i = 0; i < times.Length; i++)
        {
            if (currentHour >= times[i].x && currentHour <= times[i].y)
            {
                if (i + 1 == signId)
                {
                    profile.setScore(profile.getScore() + 1);
                    return true;
                }
            }
        }
        return false;
    }

    public void showSign (int signId)
    {
        player.showSignFromId(signId);
    }
}
