using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour {
	public Sprite[] EnemySprites;
	public RuntimeAnimatorController[] EnemyAnimator;

	public SpriteRenderer Renderer;
	public Animator Animator_;

	public float speed = 6.0f;
	public float gravity = 20.0f;

	public bool faceLeft = false;
	public Vector3 originalPosition;
	public float range;
	public float period;
	float _timer = 0;

	// Use this for initialization
	void Start () {
		int randIdx = Random.Range (0, 2);
	
		Renderer.sprite = EnemySprites [randIdx];
		Animator_.runtimeAnimatorController = EnemyAnimator [randIdx];

		originalPosition = transform.position;
	}

	public void SetMoveValue(float range_, float period_)
	{
		range = range_;
		period = period_;
	}
	
	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
		float value = Mathf.Sin (_timer / period);

		var tempPos = this.transform.position;
		this.transform.position = originalPosition + Vector3.right * range * value;

		/*if ((tempPos.x - this.transform.position.x) < 0) {
			this.transform.localScale = new Vector3 (1, 1, 1);
		} else if((tempPos.x - this.transform.position.x) > 0) {
			this.transform.localScale = new Vector3 (-1, 1, 1);
		}*/
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			GameManager.Instance.decreaseLife ();
		}
	}
}
