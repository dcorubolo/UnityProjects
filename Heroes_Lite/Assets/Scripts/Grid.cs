using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public GameObject cell;
	Vector3 [,] grid = new Vector3[11,15];

	public Vector3[,] getGrid(){
		return grid;
	}

	// Use this for initialization
	void Start () {
		float x = 0;
		float y = 0;
		float z = -3;
		for (int i = 0; i < 11; i++) {
			if (i % 2 == 0) {
				x = 0.45F;
			} else {
				x = 0F;
			}
			for (int j = 0; j < 15; j++) {
				//if (i % 2 == 0) {
					x += 0.90F;
				//}
				//print (i + " --- " + j);
				//Quaternion rotation = new Quaternion (-90, 0, 0, 0);
				Vector3 position = new Vector3 (x, y, z);
				GameObject hexCell = (GameObject) Instantiate (cell, position, Quaternion.identity);
				grid [i, j] = position;
			}
			y += 0.75F;
		}
		GameObject[] cells = GameObject.FindGameObjectsWithTag ("cell");
		foreach (var cel in cells) {
			cel.transform.rotation = Quaternion.AngleAxis (270, Vector3.right);
			cel.layer = 0;
		}
		loadCreatures ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void loadCreatures(){
		
	}
}
