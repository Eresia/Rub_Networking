using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class ConnexionData : ClientData {

	[System.NonSerialized]
	private int clientId;
	
	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		CustomDebug.Log("Execute Connexion", VerboseLevel.ALL);
		clientId = serverInformations.server.CreateNewClient(actualClient);
		if(clientId != -1){
			serverInformations.server.SendData(actualClient, new AcceptConnexionData(serverInformations.world.worldGeneration, clientId));
			CustomDebug.Log("Client " + actualClient.Address.ToString() + ":" + actualClient.Port.ToString() + " connected", VerboseLevel.IMPORTANT);
			return true;
		}
		else{
			CustomDebug.Log("Cant connect client " + actualClient.Address.ToString() + ":" + actualClient.Port.ToString(), VerboseLevel.IMPORTANT);
			return false;
		}
	}

	public override void ExecuteOnMainThread(){
		SynchronizedCharacter.CreateCharacter(clientId, GameManager.instance.characterPrefab, serverInformations.world);
	}

	protected override bool NeedConnexion(){
		return false;
	}
}
