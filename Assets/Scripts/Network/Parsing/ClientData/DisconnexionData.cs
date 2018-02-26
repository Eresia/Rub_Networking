using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

[System.Serializable]
public class DisconnexionData : ClientData {
	
	protected override bool Validate(){
		return true;
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		serverInformations.server.RemoveClient(actualClient);
	}
}
