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
			toggleKrizisca = 3;	//vedno 2 (gre "gor", samo da gre naravnost in nima poti za dol)
			endDaljice.levo = new Vector2((rootDaljiceDesno.position.x + endDaljice.levo.x)/3, rootDaljice.position.y);	//izračunamo X leve kontrolne točke konca daljice za smooth sailing
			gor = endDaljice;
			napolniTabeloTock (stTockZaNatancnost, dolzinaOdseka, new Vector2(99,99));
		}

		public int toggleToIndex(){
			if(toggleKrizisca > 0)
				return(0);
			else if(toggleKrizisca < 0)
				return(1);
			else
				return(-1);
		}

		public void poveziTockeVPot(Material aktiven, Material neAktiven){
			GameObject[] potiObjects = new GameObject[2];
			int index;
			bool prvic = true;
			for(int j = 0; j<2; j++){
				index = (toggleToIndex() + j) % 2 ;
				potiObjects[index] = new GameObject();
				new Vector3(potiObjects[index].transform.position.y,potiObjects[index].transform.position.z,-1);
				potiObjects[index].AddComponent<LineRenderer>();
				//potiObjects[index].GetComponent<LineRenderer>().material = new Material(Shader.Find("Particles/Additive"));

				potiObjects[index].GetComponent<LineRenderer>().SetWidth (0.1f, 0.1f);
				if(prvic){
					//potiObjects[index].GetComponent<LineRenderer>().material.color = aktivna;
					potiObjects[index].GetComponent<LineRenderer>().material = aktiven;
					prvic = false;
				}
				else{
					potiObjects[index].GetComponent<LineRenderer>().material = neAktiven;
				}
				potiObjects[index].GetComponent<LineRenderer>().SetVertexCount (stEltov [index]);
				for (int i = 0; i < stEltov [index]; i++) {
					potiObjects[index].GetComponent<LineRenderer>().SetPosition(i,new Vector3(tocke[index,i].x,tocke[index,i].y,-0.01f));
				}
				//potiObjects[index].GetComponent<LineRenderer>().renderer.enabled = true;
				if(toggleKrizisca == 3)
					break;
			}
		}
		
		public void narisiPot(Material aktiven, Material neAktiven){
			poveziTockeVPot (aktiven, neAktiven);
			gor.poveziTockeVPot (aktiven, neAktiven);
			gor.gor.poveziTockeVPot (aktiven, neAktiven);
			gor.dol.poveziTockeVPot (aktiven, neAktiven);
		}

		public void izrisiTrenutneKroglice(GameObject tocka, string tag){
			GameObject temp;
			foreach(GameObject X in GameObject.FindGameObjectsWithTag(tag))
				GameObject.Destroy(X);

			if (toggleKrizisca > 0) {
				for(int i = 0; i < stEltov[0]; i++){
					temp = (GameObject)Instantiate(tocka,new Vector3(tocke[0,i].x,tocke[0,i].y, -1),Quaternion.identity);
					temp.tag = tag;
				}
				//gor.izrisiTrenutneKroglice(tocka, tag);
			}
			else if(toggleKrizisca < 0){
				for(int i = 0; i < stEltov[1]; i++){
					temp = (GameObject)Instantiate(tocka,new Vector3(tocke[1,i].x,tocke[1,i].y, -1),Quaternion.identity);
					temp.tag = tag;
					
				}
				//dol.izrisiTrenutneKroglice(tocka, tag);
			}
		}

		public void narisiVsePoti(GameObject tocka){
			if (toggleKrizisca == 3) {
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

					if(toggleKrizisca == 3){
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
				if(toggleKrizisca !=3)
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