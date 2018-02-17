using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public class AcceptConnexionData : ServerData {

	public World.WorldGeneration worldGeneration;

	public AcceptConnexionData(World.WorldGeneration worldGeneration){
		this.worldGeneration = worldGeneration;
	}

	protected override bool Validate(){
		CustomDebug.Log("Try to validate Accept Connexion", VerboseLevel.ALL);
		return !IsConnected();
	}

	protected override bool Execute(){
		CustomDebug.Log("Execute Accept Connexion", VerboseLevel.ALL);
		clientInformations.client.isConnected = true;
		return true;
	}

	public override void ExecuteOnMainThread(){
		clientInformations.world.GenerateMap(worldGeneration);
	}
}
