using UnityEngine;
using System.Collections;

public class FeerieSpawner : MonoBehaviour {
	public GameObject[] SoundFeeries ;
	public int id ;
	private SoundFeerie SF ;
	// Use this for initialization
	void Start () {
		int tirage = (int)Random.Range(0, SoundFeeries.Length);
	}
	
	// Update is called once per frame
	public void selectSound(int id){
		Instantiate(SoundFeeries[id], new Vector3(0, 0, 0), Quaternion.identity);
		SF = GameObject.FindObjectOfType<SoundFeerie>();
	}

	// Update is called once per frame
	public void changeSound(int id){
		Destroy(SF.gameObject) ;
		Instantiate(SoundFeeries[id], new Vector3(0, 0, 0), Quaternion.identity);
		SF = GameObject.FindObjectOfType<SoundFeerie>();
	}

	public void setSilence(bool t){
		SF.setSilence(t) ;
	}
}
