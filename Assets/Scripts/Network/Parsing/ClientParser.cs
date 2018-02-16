using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientParser : DataParser {

	public override void Parse(NetworkObject network, IPEndPoint client, byte[] data, ConcurrentQueue<NetworkAction> actionQueue){
		
	}
}
