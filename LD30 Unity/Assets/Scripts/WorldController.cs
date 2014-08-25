﻿using UnityEngine;
using System.Collections;

//Setups the world and keeps track of certain game states
public class WorldController : MonoBehaviour {
	public GameObject player;

	public static Vector3 worldVelocity;
	public static int numConnectedIslands = 0;

	public PersonController activePerson;

	public GameObject selectionRing;

	GameObject clone;

	int numTotalIslands = 20;

	// Use this for initialization
	void Start () {
		//Places the player at the origin
		player.transform.position = new Vector3(0f, 0f, 0f);

		//Spawns an initial island at the origin
		int islandSpawn = Random.Range(0, GameConstants.numIslands);
		clone = Instantiate(Resources.Load("Prefabs/Island_" + (islandSpawn + 1).ToString()), new Vector3(0f, 0f, GameConstants.islandDepth), Quaternion.identity) as GameObject;
		clone.SendMessage("connect", 0, SendMessageOptions.RequireReceiver);
		clone.SendMessage("setVelocity", 0, SendMessageOptions.RequireReceiver);
		activePerson = player.GetComponent<PersonController>();
		changeActive(activePerson);

		//Spawn other islands randomly
		for(int i = 0; i < numTotalIslands; i++) {
			IslandSpawner.spawnIsland();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void changeActive(PersonController newPerson) {
		if (activePerson != null) {
			activePerson.selected = false;
		} 
		else {
			selectionRing.renderer.enabled = true;
		}
		activePerson = newPerson;
		activePerson.selected = true;

		Vector3 tmpPos = activePerson.transform.position;
		tmpPos.z += 0.01f;
		selectionRing.transform.position = tmpPos;
		selectionRing.transform.parent = activePerson.transform;
	}

	public static void setWorldVelocity(Vector3 v) {
		worldVelocity = v;
	}

	//Adds a connected island and changes the world's velocity
	public static void addConnectedIsland(Vector3 speed) {
		numConnectedIslands++;
		worldVelocity += new Vector3(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands, speed[2] / numConnectedIslands);
	}

	//Removes a connected island and changes the world's velocity
	public static void removeConnectedIsland(Vector3 speed) {
		worldVelocity -= new Vector3(speed[0] / numConnectedIslands, speed[1] / numConnectedIslands, speed[2] / numConnectedIslands);
		numConnectedIslands--;
	}
}
