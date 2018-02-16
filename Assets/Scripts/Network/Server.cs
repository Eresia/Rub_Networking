using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Server : NetworkObject {

	[HideInInspector]
	public List<IPEndPoint> clients;

	private ServerInformations serverInformations;

	public void Launch(int port, World world){
		clients = new List<IPEndPoint>();
		SetServerInformations(world);
		base.Launch(IPAddress.Any, port);
	}

	private void SetServerInformations(World world){
		serverInformations.server = this;
		serverInformations.world = world;
	}

	protected override DataParser GetParser(){
		return new ServerParser(serverInformations);
	}

	public void SendDataToAllClients(Data message){
		foreach(IPEndPoint c in clients){
			SendData(c, message);
		}
	}
	
}
