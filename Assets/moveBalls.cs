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
		transform.position = krivulja.lokacija;
		spominPoti = krivulja.toggleToIndex();
		p = krivulja.tocke [spominPoti, 1];
	}

	void nastaviNaslOdsek(){

		krivulja = GameObject.FindGameObjectWithTag ("nadzornik").GetComponent<nadzorPoti> ().kamSedaj (predKaterimKriziscemSmo);
		spominPoti = krivulja.toggleToIndex();
		if (spominPoti == -1) {
			Destroy (gameObject);
		}
		else
			p = krivulja.tocke[spominPoti,0];
		if(predKaterimKriziscemSmo == 1 && spominPoti == 0)
			predKaterimKriziscemSmo = 2;
		else if(predKaterimKriziscemSmo == 1 && spominPoti == 1)
			predKaterimKriziscemSmo = 3;
		else if(predKaterimKriziscemSmo == 2 && spominPoti == 0)
			predKaterimKriziscemSmo = 4;
		else if(predKaterimKriziscemSmo == 2 && spominPoti == 1)
			predKaterimKriziscemSmo = 5;
		else if(predKaterimKriziscemSmo == 3 && spominPoti == 0)
			predKaterimKriziscemSmo = 6;
		else if(predKaterimKriziscemSmo == 3 && spominPoti == 1)
			predKaterimKriziscemSmo = 7;

	}

	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Slerp(transform.position, p, Time.deltaTime * speed);

		Vector3 Distance = (Vector3)p - transform.position;

		if (Distance.sqrMagnitude < 0.2f*0.2f) {

			if(stevec < krivulja.stEltov[spominPoti]){
				p = krivulja.tocke[spominPoti,stevec++];
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
