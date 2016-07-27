using UnityEngine;
using System.Collections;

public class AutoDestroy_BlackHole : MonoBehaviour {

	public float delay = 0f;

	// Update is called once per frame
	void Update () {
	
	}

	// Use this for initialization
	void Start () {
		Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay); 
	}

}
