using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class ConnexionData : ClientData {
	
	protected override bool Validate(){
		CustomDebug.Log("Try to validate Connexion", VerboseLevel.ALL);
		return !IsConnected();
	}

	protected override bool Execute(){
		CustomDebug.Log("Execute Connexion", VerboseLevel.ALL);
		serverInformations.server.CreateNewClient(actualClient);
		serverInformations.server.SendData(actualClient, new AcceptConnexionData(serverInformations.world.worldGeneration));
		return true;
	}

	public override void ExecuteOnMainThread(){
		GameManager.instance.playerGestion.CreateCharacter(serverInformations.server.clients[actualClient].id, serverInformations.world);
	}
}
