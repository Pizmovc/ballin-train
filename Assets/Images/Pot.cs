using UnityEngine;
using System.Collections;


public class Pot: MonoBehaviour {
	public int toggleKrizisca;	//1 gor, -1 dol, 0 konec
	public Vector2 lokacija;	//transform.position od točke
	public Vector2 levo;		//position od leve kontrolne točke
	public Vector2 desno;		//position od desne kontrolne točke
	public Pot gor;				//zgornje križišče
	public Pot dol;				//spodnje križišče

//spremeni enačbo za računanje smooth sailinga
	//konstruktor za dodajanje daljice na začetek poti
	public Pot(Transform rootDaljice, Transform rootDaljiceDesno, Pot endDaljice){
		lokacija = rootDaljice.position;
		desno = new Vector2(2*(rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X desne kontrolne točke začetka daljice za smooth sailing
		endDaljice.levo = new Vector2((rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X leve kontrolne točke konca daljice za smooth sailing
		toggleKrizisca = 1;	//vedno 1 (gre "gor", samo da gre naravnost) 
		gor = endDaljice;
		dol = null;
	}

	//konstruktor za izgradnjo poti brez daljice
	public Pot(Transform root){
		lokacija = root.position;
		int stevecOtrok = 0;

		//pogleda vse otroke trenutne točke in jih "rekurzivno" doda na pot
		foreach (Transform child in root)
		{
			stevecOtrok++;
			//gornje križišče
			if(child.position.y > root.position.y){
				gor =  new Pot(child);
			}
			//spodnje križišče
			else if (child.position.y < root.position.y){
				dol = new Pot(child);
			}
			//leva/desna kontrolna točka
			else{
				//leva kontrolna točka
				if(child.position.x < root.position.x){
					levo = child.position;
				}
				//desna kontrolna točka
				else{
					desno = child.position;
				}
				
			}
			
		}
		//če ima samo enega otroka, potem je to zadnja točka (vrata)
		if(stevecOtrok == 1)
			toggleKrizisca = 0;
		//drugače ni končna točka in postavimo default vrednosti
		else
			toggleKrizisca = 1;
		
	}

	//flipa podano križišče
	public void flipKrizisce(int katero){
		//to je prvo
		if(katero == 1)
			toggleKrizisca *= -1;
		//zgornje
		else if (katero == 2)
			gor.toggleKrizisca *= -1;
		//spodnje
		else if (katero == 3)
			dol.toggleKrizisca *= -1;
			
	}		
}
