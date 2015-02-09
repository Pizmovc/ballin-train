using UnityEngine;
using System.Collections;

public class Krivulja : MonoBehaviour {
	
	public Transform root;
	public Transform rootDaljice, rootDaljiceDesno;
	public GameObject tocka;
	float t;
	Pot vseTocke, zacetekDaljice, kopijaTock;
	Vector2 q1, q0;
	int spominPoti;

	void Start(){
		t = 0;						//pozicija na krivulji, 0 - 1 0=zacPoint, 1=konPoint
		vseTocke= new Pot (root);	//zgradimo pot
		q0 = vseTocke.lokacija; 	//izračunamo začetno točko (naj bi bila zacPoint
		transform.Translate(q0);	//premaknemo krogljico na začetno točko

		zacetekDaljice = new Pot (rootDaljice, rootDaljiceDesno, vseTocke);	//v pot dodamo daljico s konstruktorjem ki delo opravi za nas
		vseTocke = zacetekDaljice;	//postavimo začetek poti na začetek daljice
		vseTocke.narisiVsePoti (tocka);
		kopijaTock = vseTocke.gor;	//si shranimo pot (za input)
	}

	// Update is called once per frame
	void Update () {
		//vseTocke.narisiPot (tocka);
		//bomo dal to v novo skripto za input (ker pač keys, touch...)
		if(Input.GetKeyDown("a"))
			kopijaTock.flipKrizisce(1); //flipa prvega
		else if(Input.GetKeyDown("i"))
			kopijaTock.flipKrizisce(2);	//flipa gornjega
		else if(Input.GetKeyDown("j"))
			kopijaTock.flipKrizisce(3);	//flipa spodnjega

		//ko krogljica začne (t=0) na novem odseku poti, si zapomni kjer mora iti
		if(t == 0){
			spominPoti = vseTocke.toggleKrizisca;
			t += 0.008f;
		}
		//če še nismo prišli do konca
		else if (t < 1) { 	
			if(Time.timeScale == 0.0f)	//če je pavza, potem ne premikamo virusa
				return;
			t += 0.008f;	// povečamo korak, 1 / s tem korakom = število vmesnih točk, pač basically hitrost
			//če potujemo po zgornji poti
			if(spominPoti == 1)
				q1 = izracunajBezierTocko (t, vseTocke.lokacija, vseTocke.desno, vseTocke.gor.levo, vseTocke.gor.lokacija); //izračuna naslednjo točko
			//če potujemo po spodnji poti
			else if (spominPoti == -1)
				q1 = izracunajBezierTocko (t, vseTocke.lokacija, vseTocke.desno, vseTocke.dol.levo, vseTocke.dol.lokacija);
			//če smo prišli do koncne točke (vrata)
			else
				t = 1;
			transform.position = Vector2.Lerp(q0,q1,1);	//premakne od točke q0 do točke q1, 3 param ni pomemben???
			q0=q1;		//nastavimo začetno točko na končno točko
		}
		//če smo prišli do konca te krivulje, nastavimo na novo krivuljo
		else { 
			//če smo potovali navzgor
			if (spominPoti == 1){
				vseTocke = vseTocke.gor;
				t = 0;
			}
			//če smo potovali navzdol
			else if (spominPoti == -1){
				vseTocke = vseTocke.dol;
				t = 0;
			}
			//drugače smo prišli do konca
			else
				Destroy(gameObject); //krogljica prleti v vrata

		}
	}

	//izračuna vmesne točke na bezierjevi krivulji in jo vrne
	Vector2 izracunajBezierTocko(float t, Vector2 zac, Vector2 con1, Vector2 con2, Vector2 kon){
		return(Mathf.Pow(1-t,3)*zac + 3*Mathf.Pow((1-t),2)*t*con1 + 3*(1-t)*Mathf.Pow(t,2)*con2 + Mathf.Pow(t,3) * kon); //po formuli [x,y]=(1–t)3P0+3(1–t)2tP1+3(1–t)t2P2+t3P3
	}

}
