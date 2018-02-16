using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public abstract class ServerData : Data{

	public void ValidateAndExecute(ClientInformations clientInformations, IPEndPoint sender){
		
		if(Validate(clientInformations, sender)){
			Execute(clientInformations);
		}
		else{
			throw new BadDataException("Data is corrupted");
		}
	}

	protected abstract void Execute(ClientInformations clientInformations);

	protected abstract bool Validate(ClientInformations clientInformations, IPEndPoint sender);

	protected bool IsConnected(Client client, IPEndPoint sender){
		return (sender == client.serverEndPoint) && (client.isConnected);
	}
}
