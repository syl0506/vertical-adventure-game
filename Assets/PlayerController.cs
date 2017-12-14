using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour {
	public Camera cam;


	public float speed = 0.1F;

	public float dropSpeed =4.0f;
	public float jumpSpeed = 2.5F;
	public Animator animator;

	private Vector3 moveDirection = Vector3.zero;
	public bool isGrounded;
	private float groundHit = 3f;
	public Vector3 lastPos;


	void Start() {
		//this.transform.position = new Vector3 (0, 1, 0);
		
	}


	void Update() {
		if (GameManager.Instance.gameState == GameState.inPlay) { 
			
		}



		MoveCharacter ();
		var screenPos = cam.WorldToScreenPoint(transform.position);
		if (screenPos.y < 0) {
			GameManager.Instance.decreaseLife ();
		}

		 

	}


	void FixedUpdate(){

		isGrounded = Physics.Raycast (this.transform.position, Vector3.down, 0.5f);
		if (isGrounded) {
			
			moveDirection.y = 0;
			lastPos = transform.position;
		}

	}





	void MoveCharacter(){

		bool isPressedMoveKey = false;

		if (Input.GetKey (KeyCode.RightArrow)) {
			moveDirection.x = 1;
			isPressedMoveKey = true;
			this.transform.localScale = new Vector3 (1, 1, 1);
			animator.SetBool ("isMoving", true);
		} 

		if (Input.GetKey (KeyCode.LeftArrow)) {
			moveDirection.x = -1;
			isPressedMoveKey = true;
			this.transform.localScale = new Vector3 (-1, 1, 1);
			animator.SetBool ("isMoving", true);
		} 


		if (Input.GetKey(KeyCode.Space) && isGrounded == true) {
			

			isGrounded = false;
			animator.SetBool ("isMoving", true);
			moveDirection.y += jumpSpeed;

		}


		if (!isPressedMoveKey /*isPressedMoveKey == false */) {

			moveDirection.x = 0;
			animator.SetBool ("isMoving", false);
		}

		if (!isGrounded) 
		{
			moveDirection.y -= dropSpeed * Time.deltaTime;

		}

		this.transform.position = this.transform.position + (moveDirection * speed );
	}


}