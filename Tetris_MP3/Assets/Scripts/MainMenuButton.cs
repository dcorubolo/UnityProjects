using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseEnter(){
		float x = 0.6F;
		float y = 0.6F;
		float z = 0.6F;
		this.transform.localScale = new Vector3 (x, y, z);
	}

	void OnMouseExit(){
		float x = 0.45F;
		float y = 0.45F;
		float z = 0.45F;
		this.transform.localScale = new Vector3 (x, y, z);
	}

	void OnMouseDown(){
		Application.LoadLevel (1);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
