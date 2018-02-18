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
		MemoryStream memoryStream = new MemoryStream(data);
		
		try{
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
			CustomDebug.LogWarning("Bad Message ! " + e.Message, VerboseLevel.INFORMATIONS);
		} catch(Exception err){
			throw err;
		}
		memoryStream.Close();
		
	}
}
