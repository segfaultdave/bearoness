﻿using UnityEngine;
using System.Collections;

public class MovieEnding : MonoBehaviour {
	
	private MovieTexture movTexture;

	// Use this for initialization
	void Start () 
	{
		movTexture = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
		movTexture.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!movTexture.isPlaying)
		{
			print("dsf");
		}
	}
}
