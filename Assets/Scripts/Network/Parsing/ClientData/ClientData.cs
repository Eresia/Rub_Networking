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

		alreadyValidate = CheckAllValidation();
		return alreadyValidate;
	}

	public void ValidateAndExecute(ServerInformations serverInformations, IPEndPoint actualClient){
		this.serverInformations = serverInformations;
		this.actualClient = actualClient;

		CustomDebug.Log("Try to validate " + GetType() +  " From " + actualClient.Address.ToString() + ":" + actualClient.Port.ToString(), VerboseLevel.ALL);
		if(alreadyValidate || CheckAllValidation()){
			CustomDebug.Log("Execute " + GetType() +  " From " + actualClient.Address.ToString() + ":" + actualClient.Port.ToString(), VerboseLevel.ALL);
			if(Execute()){
				AddMainThreadAction();
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

	protected virtual void AddMainThreadAction(){
		serverInformations.server.AddMainThreadAction(this);
	}

	protected virtual bool CheckAllValidation(){
		return (ValidateConnexionState() && Validate());
	}

	private bool ValidateConnexionState(){
		return !(NeedConnexion() ^ IsConnected());
	}
}
