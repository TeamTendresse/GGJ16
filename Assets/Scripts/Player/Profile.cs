using UnityEngine;
using System.Collections;

public class Profile : MonoBehaviour
{
    public string setPlayerURL;
    public string getScoreURL;
    public string setScoreURL;

    private string id;
    public int score { get; private set; }

	void Start ()
    {
        id = SystemInfo.deviceUniqueIdentifier;

        StartCoroutine("getScore", id);
    }
	
	void Update ()
    {
	    
	}

    public int getScore ()
    {
        return score;
    }

    public void setScore(int newScore)
    {
        score = newScore;
    }

    IEnumerator setPlayer(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("idTelephone", id);

        WWW www = new WWW(setPlayerURL, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else
        {
            print("Finished setting new player " + id);
        }
    }

    IEnumerator getScore (string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("idTelephone", id);

        WWW www = new WWW(getScoreURL, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else
        {
            if (www.text.Contains("ERROR"))
            {
                print("Player does not exist " + id);
                StartCoroutine("setPlayer", id);
            }
            else
            {
                score = int.Parse(www.text.Trim());
                print("Finished getting score for player " + id);
            }
        }
    }

    IEnumerator setScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("idTelephone", id);
        form.AddField("score", score);

        WWW www = new WWW(setScoreURL, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else
        {
            print("Finished setting score for player " + id);
        }
    }
}
