using UnityEngine;
using System.Collections;
using PotFunctions;

public class spawnBalls : MonoBehaviour {
	public GameObject[] balls = new GameObject[4];
	public float speed;
	public float casovniRazmik;


	// Use this for initialization
	void Start () {
		spawn ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void spawn (){
		StartCoroutine(waitSeconds());
	}

	IEnumerator waitSeconds(){
		yield return new WaitForSeconds(casovniRazmik);
		GameObject ball = (GameObject)Instantiate(balls[Random.Range (0,3)]);
		ball.GetComponent<moveBalls> ().setSpeed (speed);
		spawn ();
		yield break;
	}

}
