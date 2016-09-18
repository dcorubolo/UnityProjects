using UnityEngine;
using System.Collections;

public class Bateador : MonoBehaviour {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("a")) {
			anim.Play ("Bateador_Bateando");
		}
	}
}
