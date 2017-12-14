using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {none, gameOver, restart, inPlay}
public enum GameLevel {splashScreen, level1, level2, level3}

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public GameState gameState = GameState.none;
	public GameLevel gameLevel = GameLevel.level1;
	public PlayerController playerController;
	private SpriteRenderer characterSprite;
	public DoorController doorController;

	public GameObject Character;
	public GameObject[] TileElements;
	private List<GameObject> _spawnedObject = new List<GameObject>();


	public bool doorActive;

	public float TileWidth;
	public float TileHeight;


	public enum TF
	{
		N = 0, //none
		C = 1, //character
		E = 2, //enemy
		P = 4, //platform
		L = 8,
		S = 16,
		M = 32,
		MP = 64, //move Platform

	}

	//1-character, 2-platform, 3-enemy, 4-door 
	public TF[,] Map1 = new TF[,]
	{
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.MP|TF.L,TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.P,	        TF.N},
		{TF.N,	    TF.N,      TF.E|TF.L,	 TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.P},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.P|TF.L,  TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.P,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.P,      TF.N,	     TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.MP|TF.M,	 TF.N,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.C,	    TF.N,    	TF.N,	        TF.N},
		{TF.N,	    TF.N,      TF.N,	     TF.P,	    TF.N,	    TF.N,	        TF.N}
	};
		
	public int life = 3;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	static bool HasFlag(TF a, TF b)
	{
		return (a & b) == b;
	}

	void CreateMap(TF[,] tileBlueprint)
	{
		int rowLength = tileBlueprint.GetLength (0);
		int colLength = tileBlueprint.GetLength (1);

		for (int r = rowLength -1; r >= 0 ; r--) {
			for (int c = 0; c < tileBlueprint.GetLength (1); c++) {

				Vector3 spawnPosition = new Vector3 ((c - colLength / 2) * TileWidth + this.transform.position.x, 
													 (rowLength - r) * TileHeight  + this.transform.position.y, 
													 0);

				bool isLeft = (c - colLength / 2) > 0 ? true : false;
				GameObject spwanedObject = null;

				TF currTF = tileBlueprint [r, c];

				if (HasFlag (currTF, TF.P)) {
					spwanedObject = Instantiate (TileElements [0], spawnPosition, Quaternion.identity, this.transform);

					float range = 1;
					if (HasFlag (currTF, TF.L))
						range *= 2f;
					else if (HasFlag (currTF, TF.M))
						range *= 1.5f;
					else if (HasFlag (currTF, TF.S))
						range *= 0.5f;

					var currScale = spwanedObject.transform.localScale;
					currScale.x = range; 
					spwanedObject.transform.localScale = currScale;

				} else if (HasFlag (currTF, TF.MP)) {
					spwanedObject = Instantiate (TileElements [2], spawnPosition, Quaternion.identity, this.transform);

					float range = 1;
					float period = 1;
					if (HasFlag (currTF, TF.L)) {
						range = 4.6f;
						period = 1f;
					}
		
					else if (HasFlag (currTF, TF.M))
						range *= 1.5f;
					else if (HasFlag (currTF, TF.S))
						range *= 0.5f;

					spwanedObject.GetComponent<MovingBlockController> ().SetMoveValue (range, period);
				
				}
				else if (HasFlag (currTF, TF.E)) { //Enemy
					spwanedObject = Instantiate (TileElements [1], spawnPosition, Quaternion.identity, this.transform);

					float range =1;
					float period = 1;
					if (HasFlag (currTF, TF.L)) {
						range = 6.56f;
						period = 0.84f;
					}
					else if (HasFlag (currTF, TF.M))
						range *= 1.5f;
					else if (HasFlag (currTF, TF.S))
						range *= 0.5f;

					spwanedObject.GetComponent<EnemyController> ().SetMoveValue (range, period);
				} 
				else if (HasFlag (currTF, TF.C)) {
					Character.transform.position = spawnPosition;
				}
					
				if (spwanedObject != null) {
					if (isLeft) {
						var currScale = spwanedObject.transform.localScale;
						currScale.x *= -1;
						spwanedObject.transform.localScale = currScale;

					}
						
					_spawnedObject.Add (spwanedObject);						
				}

			}
		}
	}

	// Use this for initialization
	void Start () {
		CreateMap (Map1);
		doorActive = false;
		Instance = this;
		gameLevel = GameLevel.level1;
		gameState = GameState.inPlay;
		characterSprite = playerController.GetComponentInChildren < SpriteRenderer> ();

		

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.P)) {

			for (int i = _spawnedObject.Count - 1; i >= 0; i--) {
				DestroyImmediate (_spawnedObject[i]);
			}
			_spawnedObject.Clear ();

			CreateMap (Map1);
		}

		if (doorActive == true && Input.GetKey (KeyCode.UpArrow)){
			doorController.doorOpen = true;
		}
		
		

	}

	public void decreaseLife(){

		//Decrease life if player falls out of the world 
		

		if (life > 0) {
			life--;
			gameState = GameState.restart;
			RestartGame ();
		} else {
			gameState = GameState.gameOver;
		}


		//Decrease life if player makes contact with enemy 


	}

	void RestartGame(){
		playerController.transform.position = playerController.lastPos;
		StartCoroutine (GetReady ());

		
	}

	public IEnumerator GetReady(){

		for (var i = 0; i < 5; i++) {
			characterSprite.enabled = false;
			yield return new WaitForSeconds (0.1f);

			characterSprite.enabled = true;
			yield return new WaitForSeconds (0.1f);

		}

		gameState = GameState.inPlay;

	}

	public IEnumerator GoToNextLevel(){

		playerController.animator.SetBool ("isNewStage", true);
		yield return new WaitForSeconds (3.0f);

		
	}


	public void GoToLevel1(){
		StartCoroutine (GoToNextLevel ());
		SceneManager.LoadScene ("Level_1");
	}
}
