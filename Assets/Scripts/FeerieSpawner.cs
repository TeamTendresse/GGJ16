﻿using UnityEngine;
using System.Collections;

public class FeerieSpawner : MonoBehaviour {
	public GameObject[] SoundFeeries ;
	public int id;
	// Use this for initialization
	void Start () {
		int tirage = (int)Random.Range(0, SoundFeeries.Length);
		selectSound(id) ;
	}
	
	// Update is called once per frame
	public void selectSound(int id){
		Instantiate(SoundFeeries[id], new Vector3(0, 0, 0), Quaternion.identity);
	}
}
