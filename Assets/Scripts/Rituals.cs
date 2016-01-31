using UnityEngine;
using System.Collections;

public class Rituals : MonoBehaviour
{
    public Vector2[] times;

    private Player player;
    private Profile profile;

    private FeerieSpawner feerieSpawner;

    private float checkHour ;
    private int actualRitual = 10 ;

    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        profile = GameObject.FindObjectOfType<Profile>();

        feerieSpawner = GameObject.FindObjectOfType<FeerieSpawner>();

        setTheRituals() ;
        checkHour = Time.time+500f ;
    }
	
	void Update ()
    {
        if(checkHour >= Time.time){
            setTheRituals() ;
        }
	}

    public void setTheRituals(){
        bool ritual = false ;
        int currentHour = int.Parse(string.Format("{0:HH}", System.DateTime.Now));
        //hour = ampm == "AM" ? hour : (hour % 12) + 12;
        for (int i = 0; i < times.Length; i++)
        {
            if (currentHour >= times[i].x && currentHour <= times[i].y)
            {
                ritual = true ;
                if(i+1 != actualRitual){
                    actualRitual = i+1 ;
                    feerieSpawner.selectSound(actualRitual) ;
                }
            }
        }

        if(!ritual && actualRitual != 0){
            actualRitual = 0 ;
            feerieSpawner.selectSound(actualRitual) ;
        }
        //Debug.Log(currentHour + " " + actualRitual) ;

        checkHour = Time.time +60f*60f ;
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
