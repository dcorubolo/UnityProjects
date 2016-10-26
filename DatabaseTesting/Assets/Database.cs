using UnityEngine;
using System.Collections;
using Npgsql;
using Mono.Security;

public class Database : MonoBehaviour {
	
	NpgsqlConnection database = new NpgsqlConnection("Server=obligatoriodb.c3ctylzo2kcr.us-west-2.rds.amazonaws.com;User Id=root; Password=Password1!;Database=Heroes; Timeout=30;");

	// Use this for initialization

	void Start () {
		
		database.Open ();
		NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM ciudades;", database);
		NpgsqlDataReader result = command.ExecuteReader ();
		print ("TEST");
		print (database.State);


		while (result.Read ()) {
			print ("TEST");
			Debug.Log(string.Format("Hola! {0}",result[0] + "  " + result[1]));
		}


		database.Close();

	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
