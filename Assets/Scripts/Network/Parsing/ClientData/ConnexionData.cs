using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class ConnexionData : ClientData {

	protected override void Execute(Server server, IPEndPoint actualClient){
		
	}

	protected override bool Validate(Server server, IPEndPoint actualClient){
		return false;
	}
}
