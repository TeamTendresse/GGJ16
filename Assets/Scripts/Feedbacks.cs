using UnityEngine;
using System.Collections;

public class Feedbacks : MonoBehaviour {
	AudioSource audioSrc ;
	// Use this for initialization
	void Start () {
		audioSrc = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playVictory(){
		audioSrc.Play() ;
	}
}
