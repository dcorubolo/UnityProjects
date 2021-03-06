﻿using UnityEngine;
using System.Collections;
using Npgsql;
using System.Linq;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour {

	string player1Name = "Diego";
	string player2Name = "Gustavo";
	string[] playerList = new string[2];
	string gameName = "Partida1";
	Creature[] player1 = new Creature[7];
	Creature[] player2 = new Creature[7];
	bool isDBConnecting = false;
	bool isLoading = true;
	List<Creature> nextToMove = new List<Creature>();
	List<Creature> alreadyMoved = new List<Creature>();
	bool gameReady = false;
	bool nextTurn = true;
	public float speed = 10;
	Creature aux = null;
	Vector3 targetPosition = new Vector3(0,0,0);
	bool movingCreature = false;


	// Use this for initialization
	void Start () {
		StartCoroutine (connectToDatabase());
		StartCoroutine (loadGame ());
		StartCoroutine (fillGrid ());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameReady) {
			if (nextTurn) {
				if (nextToMove.Count () == 0) {
					getCreaturesToMove ();
					if (nextToMove.Count () == 0) {
						print ("No more creatures to move");	
					}
				} else {
					print ("Next turn started");
					aux = nextToMove.ElementAt (0);
					print (aux.getPositionx () + " --- " + aux.getPositiony ());
					getCreatureToMove (aux.getPositionx (), aux.getPositiony ());
					nextTurn = false;
				}
			}
			if (aux.GetType ().ToString () == "Creature") {
				if (Input.GetMouseButtonUp (0)) {
					RaycastHit hitInfo = new RaycastHit ();
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hitInfo)) {
						if (hitInfo.collider.name == "Hexagon(Clone)") {
							movingCreature = true;
							targetPosition = hitInfo.transform.position;
						} else if (hitInfo.collider.name == "SaveButton"){
							saveGame ();
						}
					}
					bool odd = false;
					if (hitInfo.transform.position.y == 0.75F ||
						hitInfo.transform.position.y == 2.25F ||
						hitInfo.transform.position.y == 3.75F ||
						hitInfo.transform.position.y == 5.25F ||
						hitInfo.transform.position.y == 6.75F) {
						odd = true;
					}
					nextToMove.ElementAt (0).setPosition (getGridPositiony (hitInfo.transform.position.x, odd), getGridPositionx (hitInfo.transform.position.y));
					alreadyMoved.Add (nextToMove.ElementAt (0));
					nextToMove.RemoveAt (0);
				}
			}
			if (movingCreature) {
				float step = speed * Time.deltaTime;
				GameObject.FindGameObjectWithTag("SelectedCreature").transform.position = Vector3.MoveTowards (GameObject.FindGameObjectWithTag("SelectedCreature").transform.position, targetPosition, step);
				if (GameObject.FindGameObjectWithTag("SelectedCreature").transform.position == targetPosition) {
					GameObject.FindGameObjectWithTag ("SelectedCreature").tag = "Creature";
					movingCreature = false;
					nextTurn = true;
				}
			}
		}
	}

	int getGridPositionx(float pos){
		int count = 0;
		pos -= 0.75F;
		while (pos > 0F) {
			pos -= 0.75F;
			count++;
		}
		return count;
	}

	int getGridPositiony(float pos, bool odd){
		int count = 0;
		if (!odd) {
			pos -= 0.45F;
		}
		return Mathf.CeilToInt((pos/0.9F)-1F);
	}

	void getCreaturesToMove(){
		foreach (Creature item in alreadyMoved) {
			nextToMove.AddRange (alreadyMoved);
		}
		orderCreaturesToMove ();
	}

	IEnumerator fillGrid(){
		while (isLoading) {
			yield return new WaitForSeconds (1F);
		}
		Vector3[,] grid = (Vector3[,])GetComponent<Grid> ().getGrid ();
		for (int i = 0; i < player1.Length; i++) {
			if (player1[i] != null) {
				GameObject creature = (GameObject) Instantiate(Resources.Load("Creatures/" + player1[i].getName(), typeof(GameObject)));
				creature.transform.position = grid[player1[i].getPositiony(),player1[i].getPositionx()];
				nextToMove.Add(player1[i]);
				yield return new WaitForSeconds (1);
			}
		}
		for (int i = 0; i < player2.Length; i++) {
			if (player2[i] != null) {
				if (player2[i].getPositionx() == 0) {
					player2 [i].setPosition (14, player2 [i].getPositiony ());
				}
				GameObject creature = (GameObject) Instantiate(Resources.Load("Creatures/" + player2[i].getName(), typeof(GameObject)));
				creature.transform.position = grid[player2[i].getPositiony(),player2[i].getPositionx()];
				creature.transform.localScale.Set(-1F,1F,1f);
				nextToMove.Add(player2[i]);
				yield return new WaitForSeconds (1);
			}
		}
		orderCreaturesToMove ();
		gameReady = true;
	}

	IEnumerator loadGame(){
		while (isDBConnecting) {
			yield return new WaitForSeconds (0.5F);
		}
		if (DatabaseAccess.isDatabaseConnected()) {
			NpgsqlCommand query = new NpgsqlCommand ("Select * from Creatures_Jugador_Partida",DatabaseAccess.getDatabase());
			NpgsqlDataReader data = query.ExecuteReader ();
			while (data.Read ()) {
				if (data [0].ToString() == player1Name) {
					player1 [(int)data [4]] = createCreature (data [2].ToString(), (int) data[3], (int) data[4], player1Name);
				} else {
					player2 [(int)data [4]] = createCreature (data [2].ToString(), (int) data[3], (int) data[4], player2Name);
				}
			}
		}
		isLoading = false;
	}

	void saveGame(){
		if (DatabaseAccess.isDatabaseConnected()) {
			playerList [0] = player1Name;
			playerList [1] = player2Name;
			for (int i = 0; i < playerList.Length; i++) {
				var trans = DatabaseAccess.getDatabase().BeginTransaction();

				NpgsqlCommand command = new NpgsqlCommand ("prepareToSaveGame", DatabaseAccess.getDatabase ());
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var parameter = command.CreateParameter ();
				parameter.ParameterName = "nomJugador";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = playerList[i];
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "nomPartida";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = gameName;
				command.Parameters.Add (parameter);
				var da = new Npgsql.NpgsqlDataAdapter(command);
				var ds = new System.Data.DataSet();
				da.Fill(ds);

				trans.Commit();
			}

			foreach (Creature item in nextToMove) {
				var trans = DatabaseAccess.getDatabase ().BeginTransaction ();
				print (item.getName ());
				NpgsqlCommand command = new NpgsqlCommand ("saveGame", DatabaseAccess.getDatabase ());
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var parameter = command.CreateParameter ();
				parameter.ParameterName = "nomJugador";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = item.getOwner();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "nomPartida";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = gameName;
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "nomCriatura";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = item.getName ();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "posx";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getPositionx ();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "posy";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getPositiony ();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "cant";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getCantidad ();
				command.Parameters.Add (parameter);
				var da = new Npgsql.NpgsqlDataAdapter (command);
				var ds = new System.Data.DataSet ();
				da.Fill (ds);

				trans.Commit ();
			}

			foreach (Creature item in alreadyMoved) {
				print (item.getName ());
				var trans = DatabaseAccess.getDatabase().BeginTransaction();

				NpgsqlCommand command = new NpgsqlCommand ("saveGame", DatabaseAccess.getDatabase ());
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var parameter = command.CreateParameter ();
				parameter.ParameterName = "nomJugador";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = item.getOwner ();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "nomPartida";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = gameName;
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "nomCriatura";
				parameter.DbType = System.Data.DbType.String;
				parameter.Value = item.getName();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "posx";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getPositionx();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "posy";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getPositiony();
				command.Parameters.Add (parameter);
				parameter = command.CreateParameter ();
				parameter.ParameterName = "cant";
				parameter.DbType = System.Data.DbType.Int32;
				parameter.Value = item.getCantidad();
				command.Parameters.Add (parameter);
				var da = new Npgsql.NpgsqlDataAdapter(command);
				var ds = new System.Data.DataSet();
				da.Fill(ds);

				trans.Commit();
			}
		}
	}

	Creature createCreature(string name, int posx, int posy, string owner){
		if (DatabaseAccess.isDatabaseConnected()) {
			NpgsqlCommand query = new NpgsqlCommand ("Select * from Criaturas where Nombre = '" + name + "'", DatabaseAccess.getDatabase ());
			NpgsqlDataReader data = query.ExecuteReader ();
			while (data.Read ()) {
				try {
					return new Creature (data[0].ToString(), (int)data [1], (int)data [2], (int) data[3], (int) data[4], (int) data[5], (int) data[6],
						(int) data[7], data[8].ToString(), data[9].ToString(), data[10].ToString(), posx, posy, 10, owner);
				} catch (System.Exception ex) {
					print (ex.ToString () + " the creature does not have ranged_attack");
					return new Creature (data[0].ToString(), (int)data [1], (int)data [2], 0, (int) data[4], (int) data[5], (int) data[6],
						(int) data[7], data[8].ToString(), data[9].ToString(), data[10].ToString(), posx, posy, 10, owner);	
				}
			}
		}
		return null;
	}

	IEnumerator connectToDatabase(){
		isDBConnecting = true;
		DatabaseAccess.connect ();
		yield return new WaitForSeconds (3);
		isDBConnecting = false;
	}

	void orderCreaturesToMove(){
		nextToMove = nextToMove.OrderByDescending<Creature, int> (si => si.getSpeed()).ToList ();
		foreach (Creature item in nextToMove) {
			print (item.getName());
		}
	}
		

	bool getCreatureToMove (int positionx, int positiony){
		GameObject[] possibleCreatures = GameObject.FindGameObjectsWithTag ("Creature");
		Vector3[,] grid = (Vector3[,])GetComponent<Grid> ().getGrid ();
		for (int i = 0; i < possibleCreatures.Length; i++) {
			if (possibleCreatures [i].transform.position.x ==  grid[positiony, positionx].x &&
				possibleCreatures [i].transform.position.y == grid[positiony, positionx].y) {
				possibleCreatures [i].tag = "SelectedCreature";
				return true;
			}
		}
		return false;
	}


}
