using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.IO;

public abstract class DataParser {

	protected BinaryFormatter formatter;

	public DataParser(){
		formatter = new BinaryFormatter();
	}

	public abstract void Parse(IPEndPoint client, byte[] data, ConcurrentQueue<NetworkAction> actionQueue);

	public byte[] ToBytes(Data data){
		byte[] result;

		using (var ms = new MemoryStream())
		{
			formatter.Serialize(ms, data);
			result = ms.ToArray();
			ms.Close();
		}

		return result;
	}
}
