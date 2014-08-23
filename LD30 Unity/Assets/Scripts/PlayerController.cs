﻿	using UnityEngine;
using System.Collections;

public class PlayerController : PersonController {

	float bridgeAngle = 0f; //Building bridge's angle
	float bridgeCurrentLength = 2f; //Current raycast for building the bridge
	float bridgeBuildSpeed = 1f; //Speed at which the bridge is building
	float bridgeBuildFastSpeed = 10f; //Speed at which the bridge is building when on an island
	float bridgeHeading; //Current building bridge heading
	Vector3 bridgeStart; //Bridge's starting position
	Vector3 bridgeEnd; //Bridge's end position
	GameObject bridgeStartingIsland; //Island you are standing on
	GameObject bridgeEndIsland; //Island you are trying to connect
	GameObject bridgeClone;

	Object bridge;

	public static bool islandFound = false;

	// Use this for initialization
	void Start () {
		bridge = Resources.Load("Prefabs/Bridge");
		connected = true;
		selected = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		//An island was found while building the bridge, connect the islands
		if(islandFound) {
			Debug.Log("Bridge starts at " + bridgeStart + " and ends at " + bridgeEnd);

			//Create the bridge
			bridgeClone = Instantiate(bridge, new Vector3(0f, 0f, GameConstants.bridgeDepth), Quaternion.identity) as GameObject;
			bridgeClone.SendMessage("setStartingPoint", bridgeStart);
			bridgeClone.SendMessage("setEndPoint", bridgeEnd);
			bridgeClone.SendMessage("setStartingIsland", bridgeStartingIsland);
			bridgeClone.SendMessage("setEndIsland", bridgeEndIsland);
			bridgeClone.SendMessage("setBridgeHeading", bridgeHeading);
			bridgeClone.SendMessage("buildBridge");


			islandFound = false;
			usingAbility = false;
		}
		if (selected) {
			//Clicks
			if(Input.GetMouseButtonDown(1)) {
				usingAbility = true;
				
				//Finds the current island the player is standing on
				RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
				if(hit.collider != null) {
					if(hit.collider.tag == "Island") {
						bridgeStartingIsland = hit.collider.gameObject;
					}
				}
				
				//Finds the angle to build the bridge
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 target = ray.origin;
				float xDiff = (target.x - transform.position.x);
				float yDiff = (target.y - transform.position.y);
				bridgeHeading = 0f;
				
				if(xDiff != 0) {
					bridgeHeading = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
				}
				
				if(xDiff < 0) {
					bridgeHeading += 180;
				}
				
				if(xDiff == 0) {
					if(yDiff > 0) {
						bridgeHeading = 90f;
					}
					else if(yDiff < 0) {
						bridgeHeading = 270f;
					}
				}
				bridgeAngle = bridgeHeading;
				bridgeCurrentLength = 0f;
				bridgeBuildSpeed = bridgeBuildFastSpeed;
				bridgeStart = transform.position;
				bridgeEnd = transform.position;
				
			}

			//Resets build once the mouse is released
			else if(Input.GetMouseButton(1)) {
				if(usingAbility) {
					Vector3 rayOrigin = transform.position + new Vector3(bridgeCurrentLength * Mathf.Cos(bridgeAngle * Mathf.Deg2Rad), bridgeCurrentLength * Mathf.Sin(bridgeAngle * Mathf.Deg2Rad), 0f);
					RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, 0f), 0.1f, GameConstants.islandLayerMask);
					
					//Keeps track of the start point of a bridge
					if(bridgeBuildSpeed == bridgeBuildFastSpeed) {
						bridgeStart = rayOrigin;
					}
					
					//Checks at the end of the building bridge if there is a connection
					bridgeCurrentLength += Time.deltaTime * bridgeBuildSpeed;
					
					bridgeBuildSpeed = 1f;
					
					//Checks if there is still island on the left
					if(hit.collider != null) {
						if(hit.collider.tag == "Island") {
							bridgeBuildSpeed = bridgeBuildFastSpeed;
							
							//Attempts to connect with that island if it isn't connected already
							hit.collider.SendMessage("connect", 1, SendMessageOptions.RequireReceiver);
							bridgeEnd = rayOrigin;
							
							bridgeEndIsland = hit.collider.gameObject;
						}
					}
				}
			}

			if(Input.GetMouseButtonUp(1)) {
				usingAbility = false;
			}
		}
	}
}
