using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockController : MonoBehaviour {
 
	public Vector3 originalPosition;
	public float range;
	public float period;
	float _timer = 0;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;


	}

	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
		if (period == 0){
			period = 1f;
		}
			
		float value = Mathf.Sin (_timer / period);
	


		this.transform.position = originalPosition + Vector3.right * range * value;
	}

	public void SetMoveValue(float range_, float period_)
	{
		range = range_;
		period = period_;
	}

	void OnCollisionEnter(Collision col){
		

	}
}
