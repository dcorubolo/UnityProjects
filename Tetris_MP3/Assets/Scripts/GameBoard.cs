using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour {

	public GameObject blackHole;
	public int maxWidth = 6;
	public int maxHeigth = 20;
	public Transform[,] grid;
	public GameObject[] candy;
	public GameObject[] specials;
	bool updatePaused = false;

	// Use this for initialization
	void Start () {
		grid = new Transform[maxHeigth,maxWidth];
		for (int i = 0; i < 4; i++) {
			GridBrain.spawnLine (i, grid, candy, this);
		}
		InvokeRepeating ("checkToDelete", 1f, 0.1f);
		InvokeRepeating ("newLine", 5f, 4f);
		//Invoke ("spawnSalchicha", 7f);
		/* Esto imprime toda la grilla, sirvio para confirmar que el primer parametro
		 * es para la coordenada y y el segundo para la coordenada x
		 * 
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 20; j++) {
				print("Parent= " + this.name + "x=" + i + " y=" + j + " ficha= " + grid[j,i]);
			}
		}*/
	}

	public void gridPrint(){
		string result = "";
		for (int j = 19; j > -1; j--) {				
			for (int i = 0; i < 6; i++) {
				result += grid [j, i];
				result += "\t";
			}
			result += "\n";
		}
		print (result);
	}

		
	// Update is called once per frame
	void Update () {
		GridBrain.checkearFichaParaBajar (maxHeigth, maxWidth, grid);
	}

	public bool waitToDelete(){
		updatePaused = true;
		StartCoroutine (waitForSeconds ());
		return updatePaused;
	}

	IEnumerator waitForSeconds(){
		yield return new WaitForSeconds (1);
		updatePaused = false;
	}

	public void shootLine(string what){
		if (what.Equals("Salchichon")){
			print ("Wingardium salchicha");
			GridBrain.spawnSalchicha (grid, specials [1], specials[0], this);
		}
		else if (what.Equals("Banana")){
			print ("Wingardium Banana!");
			GridBrain.spawnBanana (grid, specials [2], specials[0], this);
		}
	}

	public void checkToDelete(){
		if (!checkGameOver ()) {
			GridBrain.checkToDelete (maxHeigth, maxWidth, grid, this);
		} else{
			print ("Game Over player " + transform.name);
			Time.timeScale = 0;
		}
	}

	public void newLine(){
		GridBrain.spawnLineFromBelow (grid, candy, this);
	}

	public void move(Vector3 pos){
		int pos1x = (int)pos.x;
		int pos1y = (int)pos.y;
		int pos2x = (int)pos1x + 1;
		int pos2y = (int)pos.y;
		if (grid [pos1y, pos1x] == null) {
			if (grid [pos2y, pos2x] == null) {
				print ("Nothing to move");
			} else if (specialsWontMove(pos2y, pos2x)) {
				print ("It's tooooo big!");
			} else {
				grid [pos1y, pos1x] = grid [pos2y, pos2x];
				grid [pos1y, pos1x].position += new Vector3 (-1, 0, 0);
				grid [pos2y, pos2x] = null;
			}
		} else if (grid [pos2y, pos2x] == null) {
			if (grid [pos1y, pos1x] == null) {
				print ("Nothing to move");
			} else if (specialsWontMove(pos1y, pos1x)) {
				print ("It's tooooo big!");
			} else {
				grid [pos2y, pos2x] = grid [pos1y, pos1x];
				grid [pos2y, pos2x].position += new Vector3 (1, 0, 0);
				grid[pos1y, pos1x] = null;
			}
		} else {
			if (!(specialsWontMove (pos1y, pos1x) || specialsWontMove (pos2y, pos2x))) {
				intercambiarFichas (pos1x, pos1y, pos2x, pos2y);
			}
		}
	}

	private void intercambiarFichas(int pos1x, int pos1y, int pos2x, int pos2y){
		Transform temp = grid [pos1y, pos1x];
		grid [pos1y, pos1x] = grid [pos2y, pos2x];
		grid [pos1y, pos1x].position += new Vector3 (-1, 0, 0);
		grid [pos2y, pos2x] = temp;
		grid [pos2y, pos2x].position += new Vector3 (1, 0, 0);
	}

	bool checkGameOver(){
		for (int i = 0; i < 6; i++) {
			if (grid [16, i] != null && grid[15,i] != null) {
				return true;
			}
		}
		return false;
	}

	bool specialsWontMove(int posy, int posx){
		print ("entro aca");
		if (grid[posy, posx].tag == "Salchichon") {
			return true;
		}
		if (grid[posy, posx].tag == "dummy") {
			return true;
		}
		if (grid[posy, posx].tag == "Banana") {
			return true;
		}
		print ("Entro acaaaaaa abajooooooo yeaaaah");
		return false;
	}
}
