using UnityEngine;
using System.Collections;
using PotFunctions;

public class nadzorPoti : MonoBehaviour {
	
	public Transform root;
	public Transform rootDaljice, rootDaljiceDesno;
	public GameObject tockaPot;
	public GameObject tockaPotIzbrana;
	public Material barvaCrte;
	Pot vseTocke, kopijaTock;
	int spominPoti;

	void Start(){					//pozicija na krivulji, 0 - 1 0=zacPoint, 1=konPoint
		vseTocke = new Pot (root, rootDaljice, rootDaljiceDesno);
		vseTocke.poveziTockeVPot (barvaCrte);
		vseTocke.narisiPot (tockaPotIzbrana);

		//vseTocke.narisiVsePoti (tockaPot);
	}

	// Update is called once per frame
	void Update () {
		//bomo dal to v novo skripto za input (ker pač keys, touch...)
		if(Input.GetKeyDown("a")){
			vseTocke.flipKrizisce(1); //flipa prvega
			vseTocke.narisiPot (tockaPotIzbrana);
		}
		else if(Input.GetKeyDown("i")){
			vseTocke.flipKrizisce(2);	//flipa gornjega
			vseTocke.narisiPot (tockaPotIzbrana);
		}
		else if(Input.GetKeyDown("j")){
			vseTocke.flipKrizisce(3);	//flipa spodnjega
			vseTocke.narisiPot (tockaPotIzbrana);
		}
	}

	public Pot getPot(){
		return(vseTocke);
	}

	public Pot kamSedaj(int predKaterimKriziscemSmo){
		if(predKaterimKriziscemSmo == 1){
			return(vseTocke.gor);
		}
		else if(predKaterimKriziscemSmo == 2){
			return(vseTocke.gor.gor);
		}
		else if(predKaterimKriziscemSmo == 3){
			return(vseTocke.gor.dol);
		}
		else if(predKaterimKriziscemSmo == 4){
			return(vseTocke.gor.gor.gor);
		}
		else if(predKaterimKriziscemSmo == 5){
			return(vseTocke.gor.gor.dol);
		}
		else if(predKaterimKriziscemSmo == 6){
			return(vseTocke.gor.dol.gor);
		}
		else if(predKaterimKriziscemSmo == 7){
			return(vseTocke.gor.dol.dol);
		}
		else return (vseTocke);
			
	}
}
