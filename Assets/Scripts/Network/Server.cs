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

	public void Launch(int port){
		clients = new List<IPEndPoint>();
		base.Launch(IPAddress.Any, port);
	}

	protected override DataParser GetParser(){
		return new ServerParser(clients);
	}

	public void SendDataToAllClients(Data message){
		foreach(IPEndPoint c in clients){
			SendData(c, message);
		}
	}
	
}
