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

	[HideInInspector]

	public IPEndPoint serverEndPoint;

	private ClientInformations clientInformations;

	public void Launch(string address, int port, World world, Character character){
		IPAddress serverAddress = IPAddress.Parse(address);
		isConnected = false;
		SetClientInformations(world, character);

		serverEndPoint = new IPEndPoint(serverAddress, port);
		
		base.Launch(new UdpClient());
		SendDataToServer(new ConnexionData());
	}

	public void SendDataToServer(ClientData message){
		SendData(serverEndPoint, message);
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
