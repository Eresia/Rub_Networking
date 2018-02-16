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

	public override void Parse(IPEndPoint client, byte[] data, ConcurrentQueue<NetworkAction> actionQueue){
		try{
			ClientData parsedData;
			using (var ms = new MemoryStream())
			{
				object obj = formatter.Deserialize(ms);
				if(obj is ClientData){
					parsedData = (ClientData) obj;
				}
				else{
					throw new BadDataException("Not a valid Data");
				}
			}

			parsedData.ValidateAndExecute(serverInformations, client);

		} catch(BadDataException e){
			Debug.LogWarning("Bad Message ! " + e.Message);
		} catch(Exception e){
			Debug.LogError(e);
		}
		
	}
}
