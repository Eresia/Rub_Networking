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
			ServerData parsedData;
			using (var ms = new MemoryStream())
			{
				object obj = formatter.Deserialize(ms);
				if(obj is ServerData){
					parsedData = (ServerData) obj;
				}
				else{
					throw new BadDataException("Not a valid Data");
				}
			}

			parsedData.ValidateAndExecute(clientInformations);

		} catch(BadDataException e){
			Debug.LogWarning("Bad Message ! " + e.Message);
		} catch(Exception e){
			Debug.LogError(e);
		}
	}
}
