using UnityEngine;
using System.Collections;

public class TuplaPosicion {

	public int posicionX;
	public int posicionY;

	public TuplaPosicion(int x, int y){
		posicionX = x;
		posicionY = y;
	}

	public bool compararTupla(TuplaPosicion tupla){
		if (tupla != null)
		if (this.posicionX == tupla.posicionX)
		if (this.posicionY == tupla.posicionY)
			return true;
		return false;
	}

	public override string ToString(){
		return "Pos x= " + posicionX + " Pos y= " + posicionY;
	}
}
