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

	public ConcurrentDictionary<IPEndPoint, ClientToken> clients {get ; private set;}

	public ServerInformations serverInformations;

	private int actualId;

	public Server(Network network, int port, World world, int maxActionPerFrame) : base(){
		clients = new ConcurrentDictionary<IPEndPoint, ClientToken>();
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

	public new void SendData(IPEndPoint client, Data message){
		try{
			base.SendData(client, message);
		} catch(ObjectDisposedException){
			CustomDebug.LogWarning("Object disposed, Client removed", VerboseLevel.IMPORTANT);
			RemoveClient(client);
		}
	}

	public void SendDataToAllClients(Data message){
		foreach(IPEndPoint c in clients.Keys){
			SendData(c, message);
		}
	}

	public int CreateNewClient(IPEndPoint newClient){
		int id = -1;
		if(clients.TryAdd(newClient, new ClientToken(actualId))){
			id = actualId;
			actualId++;
		}
		return id;
	}

	public void RemoveClient(IPEndPoint newClient){
		ClientToken token;
		clients.TryRemove(newClient, out token);
	}
	
}
