using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public abstract class ClientData : Data {

	public void ValidateAndExecute(Server server, IPEndPoint actualClient){
		if(Validate(server, actualClient)){
			Execute(server, actualClient);
		}
		else{
			throw new BadDataException("Data is corrupted");
		}
	}

	protected abstract void Execute(Server server, IPEndPoint actualClient);

	protected abstract bool Validate(Server server, IPEndPoint actualClient);
}
