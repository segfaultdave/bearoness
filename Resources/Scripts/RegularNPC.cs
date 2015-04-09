﻿using UnityEngine;
using System.Collections;

public class RegularNPC : MonoBehaviour {

	// the player
	private GameObject bear;
	private Bear bearScript;

	// for patrolling
	private float randomWalkCoodown;
	private float prevStartWalkTime;
	private bool isStopped;
	private const float maxSpeed = 3f;
	private const float minStopTime = 0.3f;
	private const float maxStopTime = 2f;

	// for when player is too suspicious
	private bool isScared;
	private const float runAwaySpeed = 10f;
	private const float runAwaySuspicionPercentage = 99f;
	private const float comeBackSuspicionPercentage = 30f;

	// how much a collision should increase suspicion by
	private const float suspicionIncreaseUponCollision = 10f;

	// audio
	private AudioSource[] audios;

	// Use this for initialization
	void Start () 
	{
		isStopped = Random.value >= 0.5f;
		prevStartWalkTime = 0f;
		isScared = false;

		bear = GameObject.Find ("Bear");
		bearScript = bear.GetComponent<Bear>();
		audios = GetComponents<AudioSource>();
	}

	// bear runs into this NPC
	void OnCollisionEnter(Collision collision)
	{	

		// bear run into this NPC
		if(collision.gameObject.name.Equals("Bear"))
		{
			bearScript.IncreaseSuspicion(suspicionIncreaseUponCollision);
			audios[0].Play();
		}
	}

	// random walk this NPC
	void Patrol()
	{
		// patrol cooldown
		if(Time.time - prevStartWalkTime > randomWalkCoodown)
		{
			// reset cooldown
			randomWalkCoodown = Random.Range(minStopTime, maxStopTime);
			prevStartWalkTime = Time.time;

			// if stopped, walk. If walking, stop
			if(isStopped)
			{
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			else
			{
				float x = Random.Range(-maxSpeed,maxSpeed);
				float z = Random.Range(-maxSpeed,maxSpeed);
				GetComponent<Rigidbody>().velocity = new Vector3(x,0f,z);
			}
			isStopped = !isStopped;
		}
	}

	// should I be scared? Is there a bear in the room?
	void CheckFear()
	{
		if(bearScript.suspicionPercent > runAwaySuspicionPercentage && !isScared)
		{
			isScared = true;

		}
		else if(bearScript.suspicionPercent <= runAwaySuspicionPercentage && isScared)
		{
			isScared = false;
		}
	}

	// there is a bear in the room omg run away ahhhhhhhhhhhhhhhhh
	void RunAway()
	{
		Vector3 tp = GetComponent<Transform>().position;
		Vector3 bp = bear.GetComponent<Transform>().position;
		
		// set direction and magnitude
		Vector3 d = tp - bp;
		d.Normalize();
		d *= runAwaySpeed;

		// gotta run
		GetComponent<Rigidbody>().velocity = d;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		// should I be scared?
		CheckFear();
	

		if(!isScared)	// random walk if not scared
		{
			Patrol();
		}
		else					// run away if scared
		{
			RunAway();
		}
		

	}
}
