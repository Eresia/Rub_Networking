using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public abstract class ServerData : Data{

	[System.NonSerialized]
	protected ClientInformations clientInformations;

	[System.NonSerialized]
	protected IPEndPoint sender;

	[System.NonSerialized]
	private bool alreadyValidate;

	public bool OnlyValidate(ClientInformations clientInformations, IPEndPoint sender){
		this.clientInformations = clientInformations;
		this.sender = sender;

		alreadyValidate = ValidateConnexionState() && Validate();
		return alreadyValidate;
	}

	public void ValidateAndExecute(ClientInformations clientInformations, IPEndPoint sender){

		this.clientInformations = clientInformations;
		this.sender = sender;
		
		CustomDebug.Log("Try to validate " + GetType(), VerboseLevel.ALL);
		if(alreadyValidate || (ValidateConnexionState() && Validate())){
			CustomDebug.Log("Execute " + GetType(), VerboseLevel.ALL);
			if(Execute()){
				clientInformations.client.AddMainThreadAction(this);
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
		return (clientInformations.client.isConnected);
	}

	private bool ValidateConnexionState(){
		if(NeedConnexion()){
			return IsConnected() && (sender.Equals(clientInformations.client.serverEndPoint));
		}
		else{
			return (!IsConnected());
		}
	}
}
