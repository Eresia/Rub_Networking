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

	[System.NonSerialized]
	private bool alreadyValidate;

	public bool OnlyValidate(ServerInformations serverInformations, IPEndPoint actualClient){
		this.serverInformations = serverInformations;
		this.actualClient = actualClient;

		alreadyValidate = ValidateConnexionState() && Validate();
		return alreadyValidate;
	}

	public void ValidateAndExecute(ServerInformations serverInformations, IPEndPoint actualClient){
		this.serverInformations = serverInformations;
		this.actualClient = actualClient;

		CustomDebug.Log("Try to validate " + GetType(), VerboseLevel.ALL);
		if(alreadyValidate || (ValidateConnexionState() && Validate())){
			CustomDebug.Log("Execute " + GetType(), VerboseLevel.ALL);
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

	protected virtual bool NeedConnexion(){
		return true;
	}

	private bool IsConnected(){
		return serverInformations.server.clients.ContainsKey(actualClient);
	}

	private bool ValidateConnexionState(){
		return !(NeedConnexion() ^ IsConnected());
	}
}
