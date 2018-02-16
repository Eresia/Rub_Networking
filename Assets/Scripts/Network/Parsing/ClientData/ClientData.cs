using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public abstract class ClientData : Data {

	public void ValidateAndExecute(ServerInformations serverInformations, IPEndPoint actualClient){
		if(Validate(serverInformations, actualClient)){
			Execute(serverInformations, actualClient);
		}
		else{
			throw new BadDataException("Data is corrupted");
		}
	}

	protected abstract void Execute(ServerInformations server, IPEndPoint actualClient);

	protected abstract bool Validate(ServerInformations server, IPEndPoint actualClient);

	protected bool IsConnected(Server server, IPEndPoint actualClient){
		return server.clients.Contains(actualClient);
	}
}
