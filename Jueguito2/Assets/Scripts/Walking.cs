using UnityEngine;
using System.Collections;

public class Walking : MonoBehaviour {

	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter(Collision other){
		print("Collision detected with a trigger object");
	}

	// Update is called once per frame
	void Update () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}
}