﻿using UnityEngine;
using System.Collections;

public class IslandController : MonoBehaviour {
	public int health = 3;
	public int islandType; //1 - Grass, 2 - Stone

	Vector3 islandVelocity;

	public bool connected; //True if the island is connected to the player

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Destroys the island if it's too far from the camera
		if(!connected && (Mathf.Abs(transform.position.x - GameConstants.camPos.x) > GameConstants.maxCamDistance || Mathf.Abs(transform.position.y - GameConstants.camPos.y) > GameConstants.maxCamDistance)) {
			IslandSpawner.spawnIsland();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Island") {
			if(!connected) {
				IslandSpawner.spawnIsland();
				Destroy(gameObject);
			}
		}
	}

	//A bridge is built if build is not 0
	public void connect(int build) {
		if(!connected) {
			if(build != 0) {
				PlayerController.islandFound = true;
			}

			connected = true;
			WorldController.addConnectedIsland(islandVelocity);
			rigidbody2D.velocity = new Vector3(0f, 0f, 0f);
		}
	}

	public void setVelocity(int v) {
		//Sets a random island speed
		islandVelocity = new Vector3(Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), Random.Range(-1 * GameConstants.islandMaxSpeed, GameConstants.islandMaxSpeed), 0f);

		if(v == 0) {
			rigidbody2D.velocity = new Vector3(0f, 0f, 0f);
		}
		else {
			rigidbody2D.velocity = islandVelocity + WorldController.worldVelocity;
		}
	}
}
