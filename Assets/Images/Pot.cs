using UnityEngine;
using System.Collections;


public class Pot: MonoBehaviour {
	public int toggleKrizisca;	//1 gor, -1 dol, 0 konec
	public Vector2 lokacija;	//transform.position od točke
	public Vector2 levo;		//position od leve kontrolne točke
	public Vector2 desno;		//position od desne kontrolne točke
	public Pot gor;				//zgornje križišče
	public Pot dol;				//spodnje križišče
	public int steviloTockNaKrivulji = 100;
	public Vector2[,] tocke = new Vector2[2,100];


//spremeni enačbo za računanje smooth sailinga
	//konstruktor za dodajanje daljice na začetek poti
	public Pot(Transform rootDaljice, Transform rootDaljiceDesno, Pot endDaljice){
		lokacija = rootDaljice.position;
		desno = new Vector2(2*(rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X desne kontrolne točke začetka daljice za smooth sailing
		endDaljice.levo = new Vector2((rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X leve kontrolne točke konca daljice za smooth sailing
		toggleKrizisca = 1;	//vedno 1 (gre "gor", samo da gre naravnost)
		gor = endDaljice;
		dol = null;
		this.napolniTabeloTock (100);
	}

	public void narisiPot(GameObject tocka){
		if (toggleKrizisca == 1) {
			for(int i = 0; i< steviloTockNaKrivulji; i++){
				Instantiate(tocka,tocke[0,i],Quaternion.identity);
			} 
			gor.narisiPot(tocka);
		}
		else if(toggleKrizisca == -1){
			for(int i = 0; i< steviloTockNaKrivulji; i++){
				Instantiate(tocka,tocke[1,i],Quaternion.identity);
			}
			dol.narisiPot(tocka);
		}
	}

	public void napolniTabeloTock(int stTock){
		float t = 0;
		for(int i = 0; i < stTock; i++){
			t = (float)i/stTock;
			Debug.Log("t : "+t);

			Debug.Log("gornji");
			if(gor != null){
				tocke[0,i] = izracunajBezierTocko (t, lokacija, desno, gor.levo, gor.lokacija);
				Debug.Log("lokacija originalne tocke: " + lokacija + " Lokacija tocke: " + tocke[0, i]); 
				
			}
			Debug.Log("spodnji");
			if(dol != null){
				tocke[1,i] = izracunajBezierTocko (t, lokacija, desno, dol.levo, dol.lokacija);
				Debug.Log("lokacija originalne tocke: " + lokacija + " Lokacija tocke: " + tocke[1, i]); 
				
			}
			else
				Debug.Log("Dol == null");

		}
		if(gor != null)
			gor.napolniTabeloTock(stTock);
		if(dol != null)
			dol.napolniTabeloTock(stTock);
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

	//izračuna vmesne točke na bezierjevi krivulji in jo vrne
	Vector2 izracunajBezierTocko(float t, Vector2 zac, Vector2 con1, Vector2 con2, Vector2 kon){
		return(Mathf.Pow(1-t,3)*zac + 3*Mathf.Pow((1-t),2)*t*con1 + 3*(1-t)*Mathf.Pow(t,2)*con2 + Mathf.Pow(t,3) * kon); //po formuli [x,y]=(1–t)3P0+3(1–t)2tP1+3(1–t)t2P2+t3P3
	}
}
