using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ServerData : Data{

	public void ValidateAndExecute(ClientInformations clientInformations){
		
		if(Validate(clientInformations)){
			Execute(clientInformations);
		}
		else{
			throw new BadDataException("Data is corrupted");
		}
	}

	protected abstract void Execute(ClientInformations clientInformations);

	protected abstract bool Validate(ClientInformations clientInformations);

	protected bool IsConnected(Client client){
		return client.isConnected;
	}
}
