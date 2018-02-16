using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Client : NetworkObject {

	[HideInInspector]
	public bool isConnected;

	private ClientInformations clientInformations;

	public void Launch(string address, int port, World world, Character character){
		isConnected = false;
		SetClientInformations(world, character);
		base.Launch(IPAddress.Parse(address), port);
		SendDataToServer(new ConnexionData());
	}

	public void SendDataToServer(ClientData message){
		SendData(receivePoint, message);
	}

	private void SetClientInformations(World world, Character character){
		clientInformations.client = this;
		clientInformations.world = world;
		clientInformations.character = character;
	}

	protected override DataParser GetParser(){
		return new ClientParser(clientInformations);
	}
}
