﻿using UnityEngine;
using System.Collections;

public class PersonController : MonoBehaviour {

	float personRaycastOffset = 0.1f; //Raycast offset from the player so it doesn't collide on top of the player
	float moveDistanceCheck = 0.2f;	//Checks forward to see if there is something in front of the direction the character is moving
	float personSpeed = 2f;

	bool selected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(selected) {
			//Move up
			if(Input.GetKey(KeyCode.W)) {
				Vector3 rayOrigin = transform.position;
				rayOrigin.y += personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, personRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, personRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island up
				if(hit.collider != null || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.y += personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move left
			if(Input.GetKey(KeyCode.A)) {
				Vector3 rayOrigin = transform.position;
				rayOrigin.x -= personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(-1 * personRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(-1 * personRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);
				//Checks if there is still island on the left
				if(hit.collider != null || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.x -= personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move down
			if(Input.GetKey(KeyCode.S)) {
				Vector3 rayOrigin = transform.position;
				rayOrigin.y -= personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * personRaycastOffset), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(0f, -1 * personRaycastOffset), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island down
				if(hit.collider != null || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.y -= personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
			
			//Move right
			if(Input.GetKey(KeyCode.D)) {
				Vector3 rayOrigin = transform.position;
				rayOrigin.x += personRaycastOffset;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(personRaycastOffset, 0f), moveDistanceCheck, GameConstants.islandLayerMask);
				RaycastHit2D hitBridge = Physics2D.Raycast(rayOrigin, new Vector2(personRaycastOffset, 0f), moveDistanceCheck, GameConstants.bridgeLayerMask);
				
				//Checks if there is still island on the right
				if(hit.collider != null || hitBridge.collider != null) {
					Vector3 tmp = transform.position;
					tmp.x += personSpeed * Time.deltaTime;
					transform.position = tmp;
				}
			}
		}
	}
}
