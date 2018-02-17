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

	public void Launch(string address, int port, World world){
		IPAddress serverAddress = IPAddress.Parse(address);
		isConnected = false;
		SetClientInformations(world);

		serverEndPoint = new IPEndPoint(serverAddress, port);
		
		base.Launch(new UdpClient());
		SendDataToServer(new ConnexionData());
	}

	public void SendDataToServer(ClientData message){
		SendData(serverEndPoint, message);
	}

	private void SetClientInformations(World world){
		clientInformations.client = this;
		clientInformations.world = world;
	}

	protected override DataParser GetParser(){
		return new ClientParser(clientInformations);
	}
}
