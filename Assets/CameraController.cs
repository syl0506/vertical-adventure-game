using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = player.transform.position;
		Vector3 cameraPos = transform.position;
		if (playerPos.y >= cameraPos.y) {
			cameraPos.y = playerPos.y;
			transform.position = cameraPos;

		}


	}


}
