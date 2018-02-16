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

	protected override void Execute(ClientInformations clientInformations){
		CustomDebug.Log("Execute Accept Connexion", VerboseLevel.ALL);
		clientInformations.client.isConnected = true;
		clientInformations.client.AddMainThreadAction(new CreateMapAction(clientInformations.world, worldGeneration, clientInformations.character));
	}

	protected override bool Validate(ClientInformations clientInformations, IPEndPoint sender){
		CustomDebug.Log("Try to validate Accept Connexion", VerboseLevel.ALL);
		return !IsConnected(clientInformations.client, sender);
	}
}
