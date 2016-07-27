using UnityEngine;
using System.Collections;

public class MainGameControl : MonoBehaviour {

	public GameObject gameBoard;

	public GameObject[] boards = new GameObject[4];

	// Use this for initialization

	void Start () {
		Vector3 position = new Vector3 (0, 0, 0);
		boards[0] = (GameObject) Instantiate (gameBoard, position, Quaternion.identity);
		boards[0].name = "Board1";
		position = new Vector3 (12, 0, 0);
		boards[1] = (GameObject) Instantiate (gameBoard, position, Quaternion.identity);
		boards[1].name = "Board2";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void shootNewLine(string name, string what){
		for (int i = 0; i < boards.Length; i++) {
			if (boards[i] != null && boards[i].transform.name != name){
				boards[i].GetComponent<GameBoard> ().shootLine (what);
			}
		}
	}
}
