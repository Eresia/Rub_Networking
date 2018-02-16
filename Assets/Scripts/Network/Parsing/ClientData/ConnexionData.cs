using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class ConnexionData : ClientData {

	protected override void Execute(ServerInformations serverInformations, IPEndPoint actualClient){
		CustomDebug.Log("Execute Connexion", VerboseLevel.ALL);
		serverInformations.server.clients.Add(actualClient);
		serverInformations.server.SendData(actualClient, new AcceptConnexionData(serverInformations.world.worldGeneration));
	}

	protected override bool Validate(ServerInformations serverInformations, IPEndPoint actualClient){
		CustomDebug.Log("Try to validate Connexion", VerboseLevel.ALL);
		return !IsConnected(serverInformations.server, actualClient);
	}
}
