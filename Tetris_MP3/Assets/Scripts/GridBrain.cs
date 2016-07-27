using UnityEngine;
using System.Collections;

public class GridBrain : MonoBehaviour {

	bool waitingToDelete;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void spawnLine(int lineNumber, Transform[,] grid, GameObject[] candy, GameBoard parent){
		for (int i = 0; i < 6; i++) {
			spawnFicha (candy, lineNumber, parent, grid, i);
		}
	}

	static void spawnFicha(GameObject[] candy, int lineNumber, GameBoard parent, Transform[,] grid, int x){
		int j = Random.Range (0, candy.Length);	
		Vector3 position = new Vector3 (x, lineNumber, 0);
		GameObject newCandy = (GameObject)Instantiate (candy [j], position, Quaternion.identity);
		newCandy.transform.parent = null;
		newCandy.transform.parent = parent.transform;
		newCandy.transform.localPosition = newCandy.transform.position;
		grid [lineNumber, x] = newCandy.transform;
	}

	public static void spawnLineFromBelow(Transform[,] grid, GameObject[] candy, GameBoard parent){
		checkearFichaParaSubir (20, 6, grid);
		for (int i = 0; i < 6; i++) {
			int j = Random.Range(0, candy.Length);	
			Vector3 position = new Vector3 (i, 0, 0);
			GameObject newCandy = (GameObject) Instantiate (candy [j], position, Quaternion.identity);
			newCandy.transform.parent = null;
			newCandy.transform.parent = parent.transform;
			newCandy.transform.localPosition = newCandy.transform.position;
			grid [0, i] = newCandy.transform;
		}
	}

	public static void spawnSalchicha(Transform[,] grid, GameObject salchicha, GameObject dummy, GameBoard parent){
		Vector3 position = new Vector3 (0, 19, 0);
		GameObject newSalchicha = (GameObject)Instantiate (salchicha, position, Quaternion.identity);
		newSalchicha.transform.parent = null;
		newSalchicha.transform.parent = parent.transform;
		newSalchicha.transform.localPosition = newSalchicha.transform.position;
		newSalchicha.name = "Salchichotaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
		grid [19, 0] = newSalchicha.transform;
	}

	public static void spawnBanana(Transform[,] grid, GameObject banana, GameObject dummy, GameBoard parent){
		Vector3 position = new Vector3 (0, 18, 0);
		GameObject newBanana = (GameObject)Instantiate (banana, position, Quaternion.identity);
		newBanana.transform.parent = null;
		newBanana.transform.parent = parent.transform;
		newBanana.transform.localPosition = newBanana.transform.position;
		newBanana.name = "Bananasaaaaaaaaaaaaaaaaa";
		grid [18, 0] = newBanana.transform;
		GameObject dummy01 = (GameObject)Instantiate (new GameObject(), new Vector3(0,19,0), Quaternion.identity);
		dummy01.tag = "dummy";
		grid [19, 0] = dummy01.transform;
		grid [19, 1] = dummy01.transform;
		grid [19, 2] = dummy01.transform;
		grid [19, 3] = dummy01.transform;
		grid [18, 1] = dummy01.transform;
		grid [18, 2] = dummy01.transform;
		grid [18, 3] = dummy01.transform;
	}

	public static int getLastLineIndex(GameBoard board){
		for (int i = board.maxHeigth-1; i > 0; i--) {
			for (int j = 0; j < board.maxWidth; j++) {
				if (board.grid[i,j] != null) {
					return i;
				}
			}
		}
		return 0;
	}

	public void deleteFicha(int[] toDelete, Transform[,] grid){
		for (int i = 0; i < toDelete.Length; i+=2) {
			Destroy (grid [(int)toDelete [i],(int) toDelete [i + 1]].gameObject);
			grid [(int)toDelete[i], (int) toDelete[i+1]] = null;
		}
	}

	static public void deleteFicha(TuplaPosicion[] toDelete, Transform[,] grid, GameBoard parent){
		/*
		for (int i = 0; i < toDelete.Length; i++) {
			if (toDelete [i] != null) {
				if (grid [toDelete [i].posicionX + 1, 0] != null) {
					if (grid [toDelete [i].posicionX + 1, 0].tag == "Salchichon") {
					} else if (grid [toDelete [i].posicionX + 1, 0].tag == "Banana") {
					} else {
						GameObject aux = (GameObject)Instantiate(parent.blackHole, (grid [toDelete [i].posicionX, toDelete [i].posicionY].gameObject).transform.position , Quaternion.identity);
						aux.transform.parent = parent.transform;
						grid [toDelete [i].posicionY, toDelete [i].posicionX] = aux.transform;
						//aux.transform.position = new Vector3 (toDelete [i].posicionY, toDelete [i].posicionX, 0);
					}
				}
			}
		}
		*/
		for (int i = 0; i < toDelete.Length; i++) {
			if (toDelete [i] != null) {
				if (grid[toDelete[i].posicionX +1, 0] != null) {
					if (grid[toDelete[i].posicionX +1, 0].tag == "Salchichon") {
						checkSalchichaToDestroy (0, toDelete[i].posicionX + 1, grid, parent);
					}
					if (grid[toDelete[i].posicionX +1, 0].tag == "Banana") {
						checkBananaToDestroy (0, toDelete[i].posicionX + 1, grid, parent);
					}
				}
				DestroyImmediate (grid [toDelete [i].posicionX, toDelete [i].posicionY].gameObject,true);
				grid [toDelete [i].posicionX, toDelete [i].posicionY] = null;
			}
		}
	}

	public static void checkearFichaParaBajar(int h, int w, Transform[,] grid){
		for (int i = 1; i < h; i++) {
			for (int j = 0; j < w; j++) {
				if (grid [i,j] == null || grid[i,j].tag == "dummy") {
					continue;
				}
				if (grid [i, j].tag == "Salchichon") {
					bajarSalchicha (j, i, grid);
				} else if (grid [i,j].tag == "Banana") {
					bajarBanana (j, i, grid);
				} else {
					bajarFicha (j, i, grid);
				}
			}
		}
	}

	public static void checkearFichaParaSubir(int h, int w, Transform[,] grid){
		for (int i = h-2; i >= 0; i--) {
			for (int j = w-1; j >= 0; j--) {
				subirFicha (j, i, grid);
			}
		}
	}

	static void bajarSalchicha(int posx, int posy, Transform[,] grid){
		goAgain:
		if (posy - 1 >= 0) {
			bool flag = true;
			//Aca reviso si hay algo abajo para ver si puedo bajarla o no.
			for (int i = 0; i < 6; i++) {
				if (grid [posy - 1, i] != null) {
					flag = false;
				}
			}
			if (flag) {
				for (int i = 0; i < 6; i++) {
					grid [posy -1, posx + i] = grid [posy, posx + i];
					grid [posy -1, posx + i].position += new Vector3 (0, -1, 0);
					grid [posy, posx + i] = null;
					goto goAgain;
				}
			}
		}
	}

	static void bajarBanana(int posx, int posy, Transform[,] grid){
		int size = 4;
		goAgain:
		if (posy - 1 >= 0) {
			bool flag = true;
			for (int i = 0; i < size; i++) {
				if (grid [posy - 1, i] != null) {
					flag = false;
				}
			}
			if (flag) {
				for (int i = 0; i < size; i++) {
					grid [posy-1, posx + i] = grid [posy, posx + i];
					grid [posy-1, posx + i].position += new Vector3 (0, -1, 0);
					grid [posy, posx + i] = null;
					grid [posy, posx + i] = grid [posy + 1, posx + i];
					grid [posy, posx + i].position += new Vector3 (0, -1, 0);
					grid [posy+1, posx + i] = null;
					}
				goto goAgain;
			}
		}
	}

	static void bajarFicha(int posx, int posy, Transform[,] grid){
		while (posy - 1 >= 0 && grid[posy - 1, posx] == null && grid[posy, posx] != null) {
			grid [posy -1, posx] = grid [posy, posx];
			grid [posy -1, posx].position += new Vector3 (0, -1, 0);
			grid [posy, posx] = null;
		}
	}

	static void subirFicha(int posx, int posy, Transform[,] grid){
		while (posy +1 >= 0 && grid[posy +1, posx] == null && grid[posy, posx] != null) {
			grid [posy +1, posx] = grid [posy, posx];
			grid [posy +1, posx].position += new Vector3 (0, +1, 0);
			grid [posy, posx] = null;
		}
	}

	static void checkSalchichaToDestroy(int posx, int posy, Transform[,] grid, GameBoard parent){
		if (grid[posy,posx] != null) {
			if (grid[posy,posx].tag == "Salchichon") {
				destroySalchicha (grid [posy, posx].gameObject, grid, parent);
			}
		}
	}

	static void checkBananaToDestroy(int posx, int posy, Transform[,] grid, GameBoard parent){
		if (grid[posy,posx] != null) {
			if (grid[posy,posx].tag == "Banana") {
				destroyBanana (grid [posy, posx].gameObject, grid, parent);
			}
		}
	}

	static void destroySalchicha(GameObject salchicha, Transform[,] grid, GameBoard parent){
		int posy = (int)salchicha.transform.localPosition.y;
		for (int i = 0; i < 6; i++) {
			grid [posy, i] = null;
		}
		DestroyImmediate (salchicha, true);
		spawnLine(posy,grid,parent.candy,parent);
	}

	static void destroyBanana(GameObject banana, Transform[,] grid, GameBoard parent){
		int posy = (int)banana.transform.localPosition.y;
		grid [posy + 1, 0] = null;
		spawnFicha(parent.candy, posy +1, parent, grid, 0);
		grid [posy +1, 1] = null;
		spawnFicha(parent.candy, posy +1, parent, grid, 1);
		grid [posy +1, 2] = null;
		spawnFicha(parent.candy, posy +1, parent, grid, 2);
		grid [posy +1, 3] = null;
		spawnFicha(parent.candy, posy +1, parent, grid, 3);
		grid [posy, 1] = null;
		spawnFicha(parent.candy, posy, parent, grid, 1);
		grid [posy, 2] = null;
		spawnFicha(parent.candy, posy, parent, grid, 2);
		grid [posy, 3] = null;
		spawnFicha(parent.candy, posy, parent, grid, 3);
		DestroyImmediate (banana, true);
	}

	public static void checkToDelete(int maxHeight, int maxWidth, Transform[,] grid, GameBoard parent){
		string parentName = parent.transform.name;
		MainGameControl main = GameObject.FindWithTag("MainCamera").GetComponent<MainGameControl>();
		object[] fiveH;
		object[] fourH;
		object[] threeH;
		object[] fiveV;
		object[] fourV;
		object[] threeV;
		TuplaPosicion[] fichasABorrar = new TuplaPosicion[120];
		int lastIndex = 0;
		for (int i = 0; i < maxHeight; i++) {
			for (int j = 0; j < maxWidth; j++) {
				if (grid [i,j] == null || grid[i,j].tag == "dummy") {
					continue;
				}
				if (grid[i,j].tag == "Salchichon") {
					continue;
				}
				fiveH = checkFiveH(j,i, grid);
				if ((bool)fiveH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fiveH [1];
					for (int k = 0; k < 5; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					print (i + " " + j);
					main.shootNewLine (parentName, "Salchichon");
					goto vertical;
				}
				fourH = checkFourH(j,i, grid);
				if ((bool)fourH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fourH [1];
					for (int k = 0; k < 4; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					goto vertical;
				}
				threeH = checkThreeH(j,i, grid);
				if ((bool)threeH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) threeH [1];
					for (int k = 0; k < 3; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					goto vertical;
				}
				vertical:
				fiveV = checkFiveV(j,i, grid);
				if ((bool)fiveV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fiveV [1];
					for (int k = 0; k < 5; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					main.shootNewLine (parentName, "Banana");
					continue;
				}
				fourV = checkFourV(j,i, grid);
				if ((bool)fourV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fourV [1];
					for (int k = 0; k < 4; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					continue;
				}
				threeV = checkThreeV(j,i, grid);
				if ((bool)threeV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) threeV [1];
					for (int k = 0; k < 3; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					continue;
				}
			}
		}
		if (fichasABorrar[0] != null) {
			//Aca tengo que meter algo para cuando borro las fichas
			//El script que chequea si bajar fichas o no tiene que quedar quieto, pero este tiene que seguir corriendo
		}
		removeDuplicatedTuples (fichasABorrar);
		deleteFicha (fichasABorrar, grid, parent);
	}

	static void removeDuplicatedTuples(TuplaPosicion[] tuplas){
		for (int i = 0; i < tuplas.Length; i++) {
			for (int j = 0; j < tuplas.Length; j++) {
				if (i == j) {
					continue;
				}
				if (tuplas[i] != null && tuplas [i].compararTupla (tuplas [j])) {
					tuplas [j] = null;
				}
			}
		}
	}

	static object[] checkFiveH(int posx, int posy, Transform[,] grid){
		if (posx + 4 < 6 ) {
			if (grid[posy, posx] != null && grid[posy, posx + 1] != null && grid[posy, posx + 2] != null && 
				grid[posy, posx + 3] != null && grid[posy, posx + 4] != null) {
				if (grid [posy, posx].tag == grid [posy, posx + 1].tag &&
					grid[posy, posx].tag == grid[posy, posx + 2].tag &&
					grid[posy, posx].tag == grid[posy, posx + 3].tag &&
					grid[posy, posx].tag == grid[posy, posx + 4].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy, posx + 1);
					TuplaPosicion fic3 = new TuplaPosicion (posy, posx + 2);
					TuplaPosicion fic4 = new TuplaPosicion (posy, posx + 3);
					TuplaPosicion fic5 = new TuplaPosicion (posy, posx + 4);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4, fic5 };
					object[] result = {true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	static object[] checkFourH(int posx, int posy, Transform[,] grid){
		if (posx + 3 < 6 ) {
			if (grid[posy, posx] != null && grid[posy, posx + 1] != null && grid[posy, posx + 2] != null && grid[posy, posx + 3] != null ) {
				if (grid [posy, posx].tag == grid [posy, posx + 1].tag &&
					grid[posy, posx].tag == grid[posy, posx + 2].tag &&
					grid[posy, posx].tag == grid[posy, posx + 3].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy, posx + 1);
					TuplaPosicion fic3 = new TuplaPosicion (posy, posx + 2);
					TuplaPosicion fic4 = new TuplaPosicion (posy, posx + 3);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4 };
					object [] result = {true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	static object[] checkThreeH(int posx, int posy, Transform[,] grid){
		if (posx + 2 < 6 ) {
			if (grid[posy, posx] != null && grid[posy, posx + 1] != null && grid[posy, posx + 2] != null) {
				if (grid [posy, posx].tag == grid [posy, posx + 1].tag &&
					grid[posy, posx].tag == grid[posy, posx + 2].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy, posx + 1);
					TuplaPosicion fic3 = new TuplaPosicion (posy, posx + 2);
					TuplaPosicion[] array = { fic1, fic2, fic3 };
					object [] result = {true, array};
					return result;				
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	static object[] checkFiveV(int posx, int posy, Transform[,] grid){
		if (posy+4 < 19){
			if (grid[posy, posx] != null && grid[posy + 1, posx] != null && grid[posy + 2, posx] != null && 
				grid[posy + 3, posx] != null && grid[posy + 4, posx] != null) {
				if (grid [posy, posx].tag == grid [posy + 1, posx].tag &&
					grid[posy, posx].tag == grid [posy + 2, posx].tag &&
					grid[posy, posx].tag == grid [posy + 3, posx].tag &&
					grid[posy, posx].tag == grid [posy + 4, posx].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy + 1, posx);
					TuplaPosicion fic3 = new TuplaPosicion (posy + 2, posx);
					TuplaPosicion fic4 = new TuplaPosicion (posy + 3, posx);
					TuplaPosicion fic5 = new TuplaPosicion (posy + 4, posx);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4, fic5 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	static object[] checkFourV(int posx, int posy, Transform[,] grid){
		if (posy+3 < 19){
			if (grid[posy, posx] != null && grid[posy + 1, posx] != null && grid[posy + 2, posx] != null && 
				grid[posy + 3, posx] != null) {
				if (grid [posy, posx].tag == grid [posy + 1, posx].tag &&
					grid[posy, posx].tag == grid [posy + 2, posx].tag &&
					grid[posy, posx].tag == grid [posy + 3, posx].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy + 1, posx);
					TuplaPosicion fic3 = new TuplaPosicion (posy + 2, posx);
					TuplaPosicion fic4 = new TuplaPosicion (posy + 3, posx);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	static object[] checkThreeV(int posx, int posy, Transform[,] grid){
		if (posy+2 < 19){
			if (grid[posy, posx] != null && grid[posy + 1, posx] != null && grid[posy + 2, posx] != null) {
				if (grid [posy, posx].tag == grid [posy + 1, posx].tag &&
					grid[posy, posx].tag == grid [posy + 2, posx].tag) {
					TuplaPosicion fic1 = new TuplaPosicion (posy, posx);
					TuplaPosicion fic2 = new TuplaPosicion (posy + 1, posx);
					TuplaPosicion fic3 = new TuplaPosicion (posy + 2, posx);
					TuplaPosicion[] array = { fic1, fic2, fic3 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}
}
