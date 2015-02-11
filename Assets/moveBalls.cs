using UnityEngine;
using System.Collections;
using PotFunctions;

public class moveBalls : MonoBehaviour {
	Pot krivulja;
	int spominPoti;
	int predKaterimKriziscemSmo = 1;
	Vector2 p;
	float speed;
	int stevec = 1;
	// Use this for initialization
	void Start () {
		krivulja = GameObject.FindGameObjectWithTag ("nadzornik").GetComponent<nadzorPoti> ().getPot ();
		spominPoti = krivulja.toggleToIndex();
		p = krivulja.tocke [spominPoti, 1];
	}

	void nastaviNaslOdsek(){
		if (krivulja.toggleKrizisca == 0)
			Destroy(gameObject);
		else if (spominPoti == 0) {
			spominPoti = GameObject.FindGameObjectWithTag ("nadzornik").GetComponent<nadzorPoti> ().kamSedaj (predKaterimKriziscemSmo);
			krivulja = krivulja.gor;
			predKaterimKriziscemSmo = 2;
			p = krivulja.tocke[spominPoti,0];
		}
		else if (spominPoti == 1) {
			spominPoti = GameObject.FindGameObjectWithTag ("nadzornik").GetComponent<nadzorPoti> ().kamSedaj (predKaterimKriziscemSmo);
			krivulja = krivulja.dol;
			predKaterimKriziscemSmo = 3;
			p = krivulja.tocke[spominPoti,0];
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 Distance = (Vector3)p - transform.position;
		transform.position = Vector3.Slerp(transform.position, p, Time.deltaTime * speed);
		if (Distance.sqrMagnitude < 0.2f*0.2f) {
			if(stevec < krivulja.stEltov[spominPoti]){
				p = krivulja.tocke[spominPoti,stevec++];

				Debug.Log(p);
			}
			else{
				stevec = 0;
				nastaviNaslOdsek();
			}
		}


	}


	public void setSpeed(float hitrost){
		speed = hitrost;
	}
}
