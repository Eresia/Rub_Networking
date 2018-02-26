using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class Server : NetworkObject {

	public struct ClientToken{
		public int id {get; private set;}

		public float timeout {get; private set;}

		public ClientToken(int id) : this(id, 0f){}

		public ClientToken(int id, float timeout){
			this.id = id;
			this.timeout = timeout;
		}

		public ClientToken IncrementTimeout(float deltaTime){
			timeout += deltaTime;
			return this;
		}

		public ClientToken ResetTimeout(){
			timeout = 0;
			return this;
		}
	}

	public ConcurrentDictionary<IPEndPoint, ClientToken> clients {get ; private set;}

	public ServerInformations serverInformations {get ; set;}

	private int actualId;

	private List<int> removedClients;

	public Server(Network network, int port, World world, int maxActionPerFrame, float timeout) : base(){
		clients = new ConcurrentDictionary<IPEndPoint, ClientToken>();
		actualId = 0;
		SetServerInformations(world);
		removedClients = new List<int>();
		
		Init(new UdpClient(new IPEndPoint(IPAddress.Any, port)), network, maxActionPerFrame, timeout);
	}

	protected void SetServerInformations(World world){
		ServerInformations info = new ServerInformations();
		info.server = this;
		info.world = world;
		serverInformations = info;
	}

	protected override DataParser GetParser(){
		return new ServerParser(serverInformations);
	}

	public int[] CheckTimeout(float deltaTime){
		IPEndPoint[] connectedClients = clients.Keys.ToArray();
		int[] result;
		foreach(IPEndPoint c in connectedClients){
			clients[c] = clients[c].IncrementTimeout(deltaTime);
			if(clients[c].timeout > timeout){
				RemoveClient(c);
			}
		}
		result = removedClients.ToArray();
		removedClients.Clear();
		return result;
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

	public override void BadReceive(){
		socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
	}

	public void RemoveClient(IPEndPoint client){
		ClientToken token;
		if(clients.TryRemove(client, out token)){
			removedClients.Add(token.id);
			CustomDebug.LogWarning("Remove Client " + client.Address.ToString() + ":" + client.Port.ToString(), VerboseLevel.IMPORTANT);
		}
	}

	public void ResetTimeout(IPEndPoint client){
		if(clients.ContainsKey(client)){
			clients[client] = clients[client].ResetTimeout();
		}
		
	}
	
}
