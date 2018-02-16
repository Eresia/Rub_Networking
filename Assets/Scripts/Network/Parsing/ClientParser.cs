using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class ClientParser : DataParser {

	private ClientInformations clientInformations;

	public ClientParser(ClientInformations clientInformations){
		this.clientInformations = clientInformations;
	}

	public override void Parse(IPEndPoint client, byte[] data, ConcurrentQueue<NetworkAction> actionQueue){
		try{
			MemoryStream memoryStream = new MemoryStream(data);
			object obj = formatter.Deserialize(memoryStream);

			if(obj is ServerData){
				ServerData parsedData = (ServerData) obj;
				CustomDebug.Log("Object received : " + parsedData.GetType(), VerboseLevel.ALL);
				parsedData.ValidateAndExecute(clientInformations, client);
			}
			else{
				throw new BadDataException("Not a valid Data " + obj.GetType());
			}
		} catch(BadDataException e){
			Debug.LogWarning("Bad Message ! " + e.Message);
		} catch(Exception err){
			throw err;
		}
	}
}
