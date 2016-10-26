using UnityEngine;
using System.Collections;
using Npgsql;


public class DatabaseAccess : MonoBehaviour{

	static NpgsqlConnection database = new NpgsqlConnection("Server=obligatoriodb.c3ctylzo2kcr.us-west-2.rds.amazonaws.com;User Id=root; Password=Password1!;Database=Heroes; Timeout=30;");

	public static NpgsqlConnection getDatabase(){
		return database;
	}

	public static bool isDatabaseConnected(){
		print (database.State.ToString ());
		if (database.State.ToString().Equals("Open")) {
			return true;
		}
		return false;
	}

	// Use this for initialization
	public static void connect () {
		if (!isDatabaseConnected()) {
			try {
				database.Open ();
			} catch (System.Exception ex) {
				print ("Database couldn't be opened, please re-check connection details" + ex.ToString ());
			}
		}
	}
}
