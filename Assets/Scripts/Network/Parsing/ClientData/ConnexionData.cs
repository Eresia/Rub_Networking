using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class ConnexionData : ClientData {
	
	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		CustomDebug.Log("Execute Connexion", VerboseLevel.ALL);
		serverInformations.server.CreateNewClient(actualClient);
		serverInformations.server.SendData(actualClient, new AcceptConnexionData(serverInformations.world.worldGeneration));
		return true;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedCharacter.CreateCharacter(serverInformations.server.clients[actualClient].id, GameManager.instance.characterPrefab, serverInformations.world);
	}

	protected override bool NeedConnexion(){
		return false;
	}
}
