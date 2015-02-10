using UnityEngine;
using System.Collections;
using PotFunctions;

public class spawnBalls : MonoBehaviour {
	public GameObject[] balls = new GameObject[4];
	public float speed;

	// Use this for initialization
	void Start () {
		GameObject ball = (GameObject)Instantiate(balls[Random.Range (0,3)]);
		ball.GetComponent<moveBalls> ().setSpeed (speed);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
