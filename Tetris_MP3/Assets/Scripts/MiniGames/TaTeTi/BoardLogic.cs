using UnityEngine;
using System.Collections;

public class BoardLogic : MonoBehaviour {

	public GameObject[] fichas;
	GameObject[] fichasToPlay;
	int contador = 0;
	string[,] grid = new string[3,3];
	int layerCount = 2;

	// Use this for initialization
	void Start () {
		fichasToPlay = new GameObject[9];
		for (int i = 0; i < fichasToPlay.Length; i++) {
			GameObject token;
			if (i % 2 == 0) {
				token = (GameObject)Instantiate (fichas[0], new Vector3(1,0,0), Quaternion.identity);
			} else {
				token = (GameObject)Instantiate (fichas[1], new Vector3(1,0,0), Quaternion.identity);
			}
			fichasToPlay [i] = token;
		}
		fichasToPlay [0].gameObject.GetComponent<Renderer>().sortingOrder = layerCount;
		fichasToPlay [0].transform.position = new Vector3 (0, 0, 0);
	}

	void startToken(GameObject token){
		token.transform.localScale = new Vector3 (1.2F, 1.2F, 1);
		token.transform.position = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (fichasToPlay[contador].tag == "TaTeTi_Red") {
			if (Input.GetKeyDown ("w")){
				if (fichasToPlay[contador].transform.position.y <= 0) {
					fichasToPlay[contador].transform.Translate (0, 3.2F, 0);
					}
			}
			if (Input.GetKeyDown ("s")) {
				if (fichasToPlay[contador].transform.position.y >= 0) {
					fichasToPlay[contador].transform.Translate (0, -3.2F, 0);
				}
			}
			if (Input.GetKeyDown ("a")) {
				if (fichasToPlay[contador].transform.position.x >= 0) {
					fichasToPlay[contador].transform.Translate (-3.2F, 0, 0);
				}
			}
			if (Input.GetKeyDown ("d")) {
				if (fichasToPlay[contador].transform.position.x <= 0) {
					fichasToPlay[contador].transform.Translate (3.2F, 0, 0);
				}
			}
			if (Input.GetKeyDown ("g")) {
				if (checkFree(fichasToPlay[contador].transform.position, fichasToPlay[contador])) {
					updateGrid (fichasToPlay [contador]);
					fichasToPlay[contador].transform.localScale = new Vector3 (1, 1, 1);
					checkGameOver ();
					contador++;
					startToken (fichasToPlay [contador]);
					layerCount++;
					fichasToPlay [contador].gameObject.GetComponent<Renderer>().sortingOrder = layerCount;
				}
			}
		} else if (fichasToPlay[contador].tag == "TaTeTi_Blue") {
			if (Input.GetKeyDown ("up")) {
				if (fichasToPlay[contador].transform.position.y <= 0) {
					fichasToPlay[contador].transform.Translate (0, 3.2F, 0);
				}
			}
			if (Input.GetKeyDown ("down")) {
				if (fichasToPlay[contador].transform.position.y >= 0) {
					fichasToPlay[contador].transform.Translate (0, -3.2F, 0);
				}
			}
			if (Input.GetKeyDown ("left")) {
				if (fichasToPlay[contador].transform.position.x >= 0) {
					fichasToPlay[contador].transform.Translate (-3.2F, 0, 0);
				}
			}
			if (Input.GetKeyDown ("right")) {
				if (fichasToPlay[contador].transform.position.x <= 0) {
					fichasToPlay[contador].transform.Translate (3.2F, 0, 0);
				}
			}
			if (Input.GetKeyDown ("l")) {
				if (checkFree(fichasToPlay[contador].transform.position, fichasToPlay[contador])) {
					updateGrid (fichasToPlay [contador]);
					fichasToPlay[contador].transform.localScale = new Vector3 (1, 1, 1);
					checkGameOver ();
					contador++;
					startToken (fichasToPlay [contador]);
					layerCount++;
					fichasToPlay [contador].gameObject.GetComponent<Renderer>().sortingOrder = layerCount;
				}
			}
		}
	}

	void updateGrid(GameObject obj){
		float y = obj.transform.position.y;
		float x = obj.transform.position.x;
		if (y == 3.2F) {
			y = 2;
		} else if (y == -3.2F) {
			y = 0;
		} else if (y == 0F) {
			y = 1;
		}
		if (x == 3.2F) {
			x = 2;
		} else if (x == -3.2F) {
			x = 0;
		} else if (x == 0F) {
			x = 1;
		}
		string tag = obj.tag;
		grid [(int)y, (int)x] = tag;
	}

	void checkGameOver(){
		if (grid[0,0] != null && grid[1,1] != null && grid[2,2] != null){
			if (grid [0, 0].Equals (grid [1, 1]) && grid [0, 0].Equals (grid [2, 2])) {
				print ("Done");
			}
		}
		if (grid [0, 2] != null && grid [1, 1] != null && grid [2, 0] != null) {
			if (grid [0, 2].Equals (grid [1, 1]) && grid [0, 2].Equals (grid [2, 0])) {
				print ("Done");
			}
		}
		if (grid [0, 0] != null && grid [0, 1] != null && grid [0, 2] != null) {
			if (grid [0, 0].Equals (grid [0, 1]) && grid [0, 0].Equals (grid [0, 2])) {
				print ("Done");
			}
		}
		if (grid [1, 0] != null && grid [1, 1] != null && grid [1, 2] != null) {
			if (grid [1, 0].Equals (grid [1, 1]) && grid [1, 0].Equals (grid [1, 2])) {
				print ("Done");
			}
		}
		if (grid [2, 0] != null && grid [2, 1] != null && grid [2, 2] != null) {
			if (grid [2, 0].Equals (grid [2, 1]) && grid [2, 0].Equals (grid [2, 2])) {
				print ("Done");
			}
		}
		if (grid [0, 0] != null && grid [1, 0] != null && grid [2, 0] != null) {
			if (grid [0, 0].Equals (grid [1, 0]) && grid [0, 0].Equals (grid [2, 0])) {
				print ("Done");
			}
		}
		if (grid [0, 1] != null && grid [1, 1] != null && grid [2, 1] != null) {
			if (grid [0, 1].Equals (grid [1, 1]) && grid [0, 1].Equals (grid [2, 1])) {
				print ("Done");
			}
		}
		if (grid [0, 2] != null && grid [1, 2] != null && grid [2, 2] != null) {
			if (grid [0, 2].Equals (grid [1, 2]) && grid [0, 2].Equals (grid [2, 2])) {
				print ("Done");
			}
		}
	}

	bool checkFree(Vector3 position, GameObject obj){
		GameObject[] objs = (GameObject[]) FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in objs) {
			if (go.transform.position == position && go != obj) {
				return false;
			}
		}
		return true;
	}
}
