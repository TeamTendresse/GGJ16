using UnityEngine;
using System.Collections;

public class Feedbacks : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playVictory(){
		AudioSource audioSrc = gameObject.GetComponent<AudioSource>();
		audioSrc.Play() ;
	}
}
