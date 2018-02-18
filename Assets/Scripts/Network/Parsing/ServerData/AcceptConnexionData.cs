using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public class AcceptConnexionData : ServerData {

	public World.WorldGeneration worldGeneration;

	public int clientId;

	public AcceptConnexionData(World.WorldGeneration worldGeneration, int clientId){
		this.worldGeneration = worldGeneration;
		this.clientId = clientId;
	}

	protected override bool Validate(){
		return (clientId >= 0);
	}

	protected override bool Execute(){
		clientInformations.client.SetConnexion(clientId);
		CustomDebug.Log("Connected to Server", VerboseLevel.IMPORTANT);
		return true;
	}

	public override void ExecuteOnMainThread(){
		clientInformations.world.GenerateMap(worldGeneration);
	}

	protected override bool NeedConnexion(){
		return false;
	}
}
