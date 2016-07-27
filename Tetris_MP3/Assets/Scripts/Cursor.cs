using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent.name == "Board1") {
			if (Input.GetKeyDown ("w")) {
				transform.Translate (0, 1, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (0, -1, 0);
				}
			}
			if (Input.GetKeyDown ("s")) {
				transform.Translate (0, -1, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (0, 1, 0);
					//transform.TransformDirection(0,1,0);
				}
			}
			if (Input.GetKeyDown ("a")) {
				transform.Translate (-1, 0, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (1, 0, 0);
				}
			}
			if (Input.GetKeyDown ("d")) {
				transform.Translate (1, 0, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (-1, 0, 0);
				}
			}
			if (Input.GetKeyDown ("g")) {
				GameBoard board = transform.parent.GetComponent<GameBoard> ();
				board.move (transform.localPosition);
			}
		} else if (transform.parent.name == "Board2") {
			if (Input.GetKeyDown ("up")) {
				transform.Translate (0, 1, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (0, -1, 0);
				}
			}
			if (Input.GetKeyDown ("down")) {
				transform.Translate (0, -1, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (0, 1, 0);
					//transform.TransformDirection(0,1,0);
				}
			}
			if (Input.GetKeyDown ("left")) {
				transform.Translate (-1, 0, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (1, 0, 0);
				}
			}
			if (Input.GetKeyDown ("right")) {
				transform.Translate (1, 0, 0);
				if (!checkbounds (transform.localPosition)) {
					transform.Translate (-1, 0, 0);
				}
			}
			if (Input.GetKeyDown ("l")) {
				GameBoard board = transform.parent.GetComponent<GameBoard> ();
				board.move (transform.localPosition);
			}
			if (Input.GetKeyDown ("u")) {
				GameBoard board = transform.parent.GetComponent<GameBoard> ();
				board.gridPrint ();
			}
		}
	}

	bool checkbounds(Vector3 newPos){
		GameBoard board = transform.parent.GetComponent<GameBoard> ();
		if (newPos.x < 0) {
			return false;
		}
		if (newPos.x > 4) {
			return false;
		}
		if (newPos.y < 0) {
			return false;
		}
		if (newPos.y > GridBrain.getLastLineIndex(board)){
			return false;
		}
		return true;
	}
}
