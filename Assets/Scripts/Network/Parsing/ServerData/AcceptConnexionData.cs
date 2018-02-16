using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AcceptConnexionData : ServerData {

	public World.WorldGeneration worldGeneration;

	public AcceptConnexionData(World.WorldGeneration worldGeneration){
		this.worldGeneration = worldGeneration;
	}

	protected override void Execute(ClientInformations clientInformations){
		clientInformations.client.isConnected = true;
		clientInformations.world.GenerateMap(worldGeneration);
		clientInformations.character.gameObject.SetActive(true);
	}

	protected override bool Validate(ClientInformations clientInformations){
		return !IsConnected(clientInformations.client);
	}
}
