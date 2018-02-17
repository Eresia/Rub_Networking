using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class ServerParser : DataParser {

	private ServerInformations serverInformations;

	public ServerParser(ServerInformations serverInformations){
		this.serverInformations = serverInformations;
	}

	public override void Parse(IPEndPoint client, byte[] data){
		try{
			MemoryStream memoryStream = new MemoryStream(data);
			object obj = formatter.Deserialize(memoryStream);

			if(obj is ClientData){
				ClientData parsedData = (ClientData) obj;
				CustomDebug.Log("Object received : " + parsedData.GetType(), VerboseLevel.ALL);
				parsedData.ValidateAndExecute(serverInformations, client);
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
