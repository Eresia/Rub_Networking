using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Client : NetworkObject {

	public bool isConnected {get ; private set;}

	public IPEndPoint serverEndPoint {get ; private set;}

	public int clientId {get ; private set;}

	public ClientInformations clientInformations;

	public Client(Network network, string address, int port, World world, int maxActionPerFrame) : base(){
		IPAddress serverAddress = IPAddress.Parse(address);
		isConnected = false;
		clientId = -1;
		SetClientInformations(world);
		serverEndPoint = new IPEndPoint(serverAddress, port);

		Init(new UdpClient(), network, maxActionPerFrame);
	}

	public override void Launch(){
		base.Launch();
		SendData(new ConnexionData());
	}

	public void SendData(ClientData message){
		try{
			base.SendData(serverEndPoint, message);
		} catch(ObjectDisposedException){
			CustomDebug.LogWarning("Object disposed, Close Client", VerboseLevel.IMPORTANT);
			socket.Close();
			Application.Quit();
		}
	}

	private void SetClientInformations(World world){
		clientInformations.client = this;
		clientInformations.world = world;
	}

	protected override DataParser GetParser(){
		return new ClientParser(clientInformations);
	}

	public void SetConnexion(int id){
		isConnected = true;
		clientId = id;
	}
}
