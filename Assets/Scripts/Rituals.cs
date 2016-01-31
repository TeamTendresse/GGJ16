using UnityEngine;
using System.Collections;

public class Rituals : MonoBehaviour
{
    public Vector2[] times;

    private Player player;
    private Profile profile;

    void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        profile = GameObject.FindObjectOfType<Profile>();
    }
	
	void Update ()
    {
        
	}

    public bool isTheRightTime (int signId)
    {
        int currentHour = int.Parse(string.Format("{0:hh}", System.DateTime.Now));
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
