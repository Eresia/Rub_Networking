using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public abstract class ClientData : Data {

	[System.NonSerialized]
	public ServerInformations serverInformations;

	[System.NonSerialized]
	public IPEndPoint actualClient;

	protected bool IsConnected(){
		Debug.Log(serverInformations);
		Debug.Log(serverInformations.server);
		Debug.Log(serverInformations.server.clients);
		return serverInformations.server.clients.ContainsKey(actualClient);
	}

	public void ValidateAndExecute(ServerInformations serverInformations, IPEndPoint actualClient){
		this.serverInformations = serverInformations;
		this.actualClient = actualClient;

		if(Validate()){
			if(Execute()){
				serverInformations.server.AddMainThreadAction(this);
			}
		}
		else{
			throw new BadDataException("Data is corrupted");
		}
	}

	protected abstract bool Validate();

	protected abstract bool Execute();

	public abstract void ExecuteOnMainThread();
}
