using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	public Animator animator;
	public bool doorOpen;



	// Use this for initialization
	void Start () {
		doorOpen = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (doorOpen == true) {
			animator.SetBool ("doorOpen", true);
			GameManager.Instance.GoToLevel1 ();

		}
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			GameManager.Instance.doorActive = true;
		}
	}

}
