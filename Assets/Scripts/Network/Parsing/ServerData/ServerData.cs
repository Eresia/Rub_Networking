using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public abstract class ServerData : Data{

	[System.NonSerialized]
	public ClientInformations clientInformations;

	[System.NonSerialized]
	public IPEndPoint sender;

	protected bool IsConnected(){
		return (sender == clientInformations.client.serverEndPoint) && (clientInformations.client.isConnected);
	}

	public void ValidateAndExecute(ClientInformations clientInformations, IPEndPoint sender){

		this.clientInformations = clientInformations;
		this.sender = sender;
		
		if(Validate()){
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
}
