using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Server : NetworkObject {

	public struct ClientToken{
		public int id;

		public ClientToken(int id){
			this.id = id;
		}
	}

	[HideInInspector]
	public Dictionary<IPEndPoint, ClientToken> clients;

	private ServerInformations serverInformations;

	private int actualId;

	public Server(Network network, int port, World world, int maxActionPerFrame) : base(){
		clients = new Dictionary<IPEndPoint, ClientToken>();
		actualId = 0;
		SetServerInformations(world);
		
		Init(new UdpClient(new IPEndPoint(IPAddress.Any, port)), network, maxActionPerFrame);
	}

	protected void SetServerInformations(World world){
		serverInformations.server = this;
		serverInformations.world = world;
	}

	protected override DataParser GetParser(){
		return new ServerParser(serverInformations);
	}

	public void SendDataToAllClients(Data message){
		foreach(IPEndPoint c in clients.Keys){
			SendData(c, message);
		}
	}

	public int CreateNewClient(IPEndPoint newClient){
		int id = -1;
		if(!clients.ContainsKey(newClient)){
			id = actualId;
			clients.Add(newClient, new ClientToken(id));
			actualId++;
		}
		return id;
	}
	
}
