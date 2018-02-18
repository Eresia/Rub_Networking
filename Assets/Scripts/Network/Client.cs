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

	public ClientInformations clientInformations;

	public Client(Network network, string address, int port, World world, int maxActionPerFrame) : base(){
		IPAddress serverAddress = IPAddress.Parse(address);
		isConnected = false;
		SetClientInformations(world);
		serverEndPoint = new IPEndPoint(serverAddress, port);

		Init(new UdpClient(), network, maxActionPerFrame);
	}

	public override void Launch(){
		base.Launch();
		SendDataToServer(new ConnexionData());
	}

	public void SendDataToServer(ClientData message){
		SendData(serverEndPoint, message);
	}

	private void SetClientInformations(World world){
		clientInformations.client = this;
		clientInformations.world = world;
		clientInformations.clientId = -1;
	}

	protected override DataParser GetParser(){
		return new ClientParser(clientInformations);
	}
}
