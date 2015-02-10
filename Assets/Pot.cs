using UnityEngine;
using System.Collections;

namespace PotFunctions{
	public class Pot: Component {
		public int toggleKrizisca;	//1 gor, -1 dol, 0 konec
		public Vector2 lokacija;	//transform.position od točke
		public Vector2 levo;		//position od leve kontrolne točke
		public Vector2 desno;		//position od desne kontrolne točke
		public Pot gor;				//zgornje križišče
		public Pot dol;				//spodnje križišče
		public float dolzinaOdseka = 0.1f;
		public int stTockZaNatancnost = 500;
		public Vector2[,] tocke = new Vector2[2,500];
		public int[] stEltov = new int[2];

		public Pot(){
		
		}

		public Pot(Transform root, Transform rootDaljice, Transform rootDaljiceDesno){
			Pot tempPot = new Pot ();
			tempPot.izgradiPot (root);
			dodajDaljico(rootDaljice, rootDaljiceDesno, tempPot);
		}

		//metoda za izgradnjo poti brez daljice
		public void izgradiPot(Transform root){
			lokacija = root.position;
			int stevecOtrok = 0;
			//pogleda vse otroke trenutne točke in jih "rekurzivno" doda na pot
			foreach (Transform child in root)
			{
				stevecOtrok++;
				//gornje križišče
				if(child.position.y > root.position.y){
					gor = new Pot ();
					gor.izgradiPot(child);
				}
				//spodnje križišče
				else if (child.position.y < root.position.y){
					dol = new Pot ();
					dol.izgradiPot(child);
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

		//metoda za dodajanje daljice na začetek poti
		public void dodajDaljico(Transform rootDaljice, Transform rootDaljiceDesno, Pot endDaljice){
			lokacija = rootDaljice.position;
			desno = new Vector2(2*(rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X desne kontrolne točke začetka daljice za smooth sailing
			toggleKrizisca = 2;	//vedno 2 (gre "gor", samo da gre naravnost in nima poti za dol)
			endDaljice.levo = new Vector2((rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X leve kontrolne točke konca daljice za smooth sailing
			gor = endDaljice;
			napolniTabeloTock (stTockZaNatancnost, dolzinaOdseka, new Vector2(99,99));
		}



		public void poveziTockeVPot(Material barva){
			GameObject[] potiObjects = new GameObject[4];
			LineRenderer[] potiLines = new LineRenderer[4];
			for(int i = 0; i < 4; i++){
				potiObjects[i] = new GameObject();
				potiObjects[i].AddComponent<LineRenderer> ();
				potiObjects[i].GetComponent<LineRenderer>().material = barva;
				potiObjects[i].GetComponent<LineRenderer>().SetWidth (0.1f, 0.1f);
				potiObjects[i].GetComponent<LineRenderer>().SetColors (Color.blue, Color.blue);
				potiLines[i] = potiObjects[i].GetComponent<LineRenderer>();
			}
			potiLines [0].SetVertexCount (stEltov [0] + gor.stEltov[0] + gor.gor.stEltov[0]);
			for (int i = 0; i < stEltov [0]; i++) {
				potiLines[0].SetPosition(i,tocke[0,i]);
			}
			for (int i = 0; i < gor.stEltov [0]; i++) {
				potiLines[0].SetPosition(i+stEltov[0],gor.tocke[0,i]);
			}
			for (int i = 0; i < gor.gor.stEltov [0]; i++) {
				potiLines[0].SetPosition(i+stEltov[0]+gor.stEltov [0],gor.gor.tocke[0,i]);
			}

			potiLines [1].SetVertexCount (stEltov [0] + gor.stEltov[0] + gor.gor.stEltov[1]);
			for (int i = 0; i < stEltov [0]; i++) {
				potiLines[1].SetPosition(i,tocke[0,i]);
			}
			for (int i = 0; i < gor.stEltov [0]; i++) {
				potiLines[1].SetPosition(i+stEltov[0],gor.tocke[0,i]);
			}
			for (int i = 0; i < gor.gor.stEltov [1]; i++) {
				potiLines[1].SetPosition(i+stEltov[0]+gor.stEltov [0],gor.gor.tocke[1,i]);
			}

			potiLines [2].SetVertexCount (stEltov [0] + gor.stEltov[1] + gor.dol.stEltov[0]);
			for (int i = 0; i < stEltov [0]; i++) {
				potiLines[2].SetPosition(i,tocke[0,i]);
			}
			for (int i = 0; i < gor.stEltov [1]; i++) {
				potiLines[2].SetPosition(i+stEltov[0],gor.tocke[1,i]);
			}
			for (int i = 0; i < gor.dol.stEltov [0]; i++) {
				potiLines[2].SetPosition(i+stEltov[0]+gor.stEltov [1],gor.dol.tocke[0,i]);
			}
			
			potiLines [3].SetVertexCount (stEltov [0] + gor.stEltov[1] + gor.dol.stEltov[1]);
			for (int i = 0; i < stEltov [0]; i++) {
				potiLines[3].SetPosition(i,tocke[0,i]);
			}
			for (int i = 0; i < gor.stEltov [1]; i++) {
				potiLines[3].SetPosition(i+stEltov[0],gor.tocke[1,i]);
			}
			for (int i = 0; i < gor.dol.stEltov [1]; i++) {
				potiLines[3].SetPosition(i+stEltov[0]+gor.stEltov [1],gor.dol.tocke[1,i]);
			}
		}
		
		public void narisiPot(GameObject tocka){
			foreach(GameObject temp in GameObject.FindGameObjectsWithTag("trenutnaPotTocka"))
			      GameObject.Destroy(temp);
			izrisiTrenutneKroglice (tocka);
		}

		public void izrisiTrenutneKroglice(GameObject tocka){
			if (toggleKrizisca > 0) {
				for(int i = 0; i < stEltov[0]; i++){
					Instantiate(tocka,new Vector3(tocke[0,i].x,tocke[0,i].y, -1),Quaternion.identity);
				} 
				gor.izrisiTrenutneKroglice(tocka);
			}
			else if(toggleKrizisca < 0){
				for(int i = 0; i < stEltov[1]; i++){
					Instantiate(tocka,new Vector3(tocke[1,i].x,tocke[1,i].y, -1),Quaternion.identity);
					
				}
				dol.izrisiTrenutneKroglice(tocka);
			}
		}

		public void narisiVsePoti(GameObject tocka){
			if (toggleKrizisca == 2) {
				for(int i = 0; i< stEltov[0]; i++){
					Instantiate(tocka,tocke[0,i],Quaternion.identity);
				} 
				gor.narisiVsePoti(tocka);
			}
			else if(toggleKrizisca != 0){
				for(int i = 0; i < stEltov[0]; i++)
					Instantiate(tocka,tocke[0,i],Quaternion.identity);

				for(int i = 0; i < stEltov[1]; i++)
					Instantiate(tocka,tocke[1,i],Quaternion.identity);

				gor.narisiVsePoti(tocka);
				dol.narisiVsePoti(tocka);
			}
		}

		public void napolniTabeloTock(int stTock, float dolzOdseka, Vector2 lastP){
			Vector2 p0g;
			Vector2 p0d;
				if (lastP == new Vector2(99,99)) {
				p0g = lokacija;
				p0d = lokacija;
			}
			else{
				p0g = lastP;
				p0d = lastP;
			}
			Vector2 p1g;
			Vector2 p1d;
			int stTockVTabeliG = 0;
			int stTockVTabeliD = 0;
			if(toggleKrizisca != 0){
				float t = 0;
				for(int i = 0; i < stTock; i++){
					t = (float)i/stTock;

					if(toggleKrizisca == 2){
						p1g = izracunajBezierTocko (t, lokacija, desno, gor.levo, gor.lokacija);
						if(Vector2.Distance(p0g,p1g) >= dolzOdseka){
							tocke[0,stTockVTabeliG] = p1g;
							stTockVTabeliG++;
							p0g = p1g;
						}
					}
					else{
						p1g = izracunajBezierTocko (t, lokacija, desno, gor.levo, gor.lokacija);
						if(Vector2.Distance(p0g,p1g) >= dolzOdseka){
							tocke[0,stTockVTabeliG] = p1g;
							stTockVTabeliG++;
							p0g = p1g;
						}
						p1d = izracunajBezierTocko (t, lokacija, desno, dol.levo, dol.lokacija);
						if(Vector2.Distance(p0d,p1d) >= dolzOdseka){
							tocke[1,stTockVTabeliD] = p1d;
							stTockVTabeliD++;
							p0d = p1d;
						}
					}
				}
				stEltov[0]= stTockVTabeliG;
				stEltov[1]= stTockVTabeliD;
				if(toggleKrizisca !=2)
					dol.napolniTabeloTock(stTock,dolzOdseka, p0d);
				gor.napolniTabeloTock(stTock, dolzOdseka, p0g);
			}
		}

		//flipa podano križišče
		public void flipKrizisce(int katero){
			//to je prvo
			if(katero == 1)
				gor.toggleKrizisca *= -1;
			//zgornje
			else if (katero == 2)
				gor.gor.toggleKrizisca *= -1;
			//spodnje
			else if (katero == 3)
				gor.dol.toggleKrizisca *= -1;
				
		}

		//izračuna vmesne točke na bezierjevi krivulji in jo vrne
		Vector2 izracunajBezierTocko(float t, Vector2 zac, Vector2 con1, Vector2 con2, Vector2 kon){
			return(Mathf.Pow(1-t,3)*zac + 3*Mathf.Pow((1-t),2)*t*con1 + 3*(1-t)*Mathf.Pow(t,2)*con2 + Mathf.Pow(t,3) * kon); //po formuli [x,y]=(1–t)3P0+3(1–t)2tP1+3(1–t)t2P2+t3P3
		}
	}
}