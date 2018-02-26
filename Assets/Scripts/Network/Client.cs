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

	public ClientInformations clientInformations {get ; set;}

	private float actualTime;

	public Client(Network network, string address, int port, World world, int maxActionPerFrame, float timeout) : base(){
		IPAddress serverAddress = IPAddress.Parse(address);
		isConnected = false;
		clientId = -1;
		SetClientInformations(world);
		serverEndPoint = new IPEndPoint(serverAddress, port);
		actualTime = 0;

		Init(new UdpClient(new IPEndPoint(serverAddress, 0)), network, maxActionPerFrame, timeout);
	}

	public override void Launch(){
		base.Launch();
		SendData(new ConnexionData());
	}

	private void SetClientInformations(World world){
		ClientInformations info = new ClientInformations();
		info.client = this;
		info.world = world;
		clientInformations = info;
	}

	public void CheckTimeout(float deltaTime){
		actualTime += deltaTime;
		if(actualTime > timeout){
			Close();
		}
	}

	public void ResetTime(){
		actualTime = 0;
	}

	public void SendData(ClientData message){
		try{
			base.SendData(serverEndPoint, message);
		} catch(ObjectDisposedException){
			CustomDebug.LogWarning("Object disposed, Close Client", VerboseLevel.IMPORTANT);
			Close();
		}
	}

	protected override DataParser GetParser(){
		return new ClientParser(clientInformations);
	}

	public void SetConnexion(int id){
		isConnected = true;
		clientId = id;
	}

	public override void Close(){
		base.Close();
		CustomDebug.LogWarning("Connexion Closed", VerboseLevel.IMPORTANT);
	}
}
