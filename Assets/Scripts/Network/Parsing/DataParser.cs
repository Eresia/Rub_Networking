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

	public abstract void Parse(IPEndPoint client, byte[] data);

	public byte[] ToBytes(Data data){
		byte[] result;

		MemoryStream memoryStream = new MemoryStream();
		formatter.Serialize(memoryStream, data);
		result = memoryStream.GetBuffer();
		memoryStream.Close();

		return result;
	}
}
