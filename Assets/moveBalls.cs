using UnityEngine;
using System.Collections;
using PotFunctions;

public class moveBalls : MonoBehaviour {
	Pot krivulja;
	int spominPoti;
	Vector2 p;
	float speed;
	int stevec = 1;
	// Use this for initialization
	void Start () {
		krivulja = GameObject.FindGameObjectWithTag ("nadzornik").GetComponent<nadzorPoti> ().getPot ();
		spominPoti = 0;
		p = krivulja.tocke [spominPoti, 1];
		Debug.Log (krivulja.lokacija);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 Distance = (Vector3)p - transform.position;

		if (Distance.sqrMagnitude < 0.1f*0.1f) {
			if(krivulja.stEltov[spominPoti] > stevec){
				p = krivulja.tocke[spominPoti,stevec++];
			}
			else{

			}
		}
		transform.position = Vector3.Slerp(transform.position, p, Time.deltaTime * speed);
	}


	public void setSpeed(float hitrost){
		speed = hitrost;
	}
}
