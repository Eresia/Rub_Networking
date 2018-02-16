using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public abstract class NetworkObject : MonoBehaviour {

	public int maxActionByFrame;

	protected UdpClient socket;
		
	protected IPEndPoint receivePoint;

	protected ConcurrentQueue<NetworkAction> actionQueue;

	private DataParser parser;

	public virtual void Launch(IPAddress address, int port)
	{
		socket = new UdpClient();
		receivePoint = new IPEndPoint(address, port);
		actionQueue = new ConcurrentQueue<NetworkAction>();
		parser = GetParser();
		StartCoroutine(MakeActionsCoroutine());
		socket.BeginReceive(new AsyncCallback(ReceiveCallback), receivePoint);
	}

	private IEnumerator MakeActionsCoroutine(){
		while(socket.Client != null){
			int nbAction = 0;
			while((!actionQueue.IsEmpty) && (nbAction < maxActionByFrame)){
				NetworkAction newAction;
				if(actionQueue.TryDequeue(out newAction)){
					newAction.Execute();
					nbAction++;
				}
			}
			yield return null;
		}
	}

	public void SendData(IPEndPoint client, Data message){
		if(message != null){
			byte[] buffer = parser.ToBytes(message);
			socket.BeginSend(buffer, buffer.Length, client, SendCallback, null);
		}
	}

	protected abstract DataParser GetParser();

	public void ReceiveCallback(IAsyncResult asyncResult){
		try{
			IPEndPoint sender = (IPEndPoint) asyncResult.AsyncState;
			byte[] buffer = socket.EndReceive(asyncResult, ref sender);

			parser.Parse(sender, buffer, actionQueue);

			socket.BeginReceive(new AsyncCallback(ReceiveCallback), receivePoint);
		}
		catch (ObjectDisposedException){
			Debug.Log("Connexion closed");
			socket.Close();
		}
		catch (Exception err)
		{
			Debug.Log(err);
			socket.Close();
		}
	}

	public void SendCallback(IAsyncResult asyncResult){
		try{
			if(socket.EndSend(asyncResult) == 0){
				Debug.LogWarning("Send empty message");
			}
		}
		catch (ObjectDisposedException){
			Debug.Log("Connexion closed");
			socket.Close();
		}
		catch (Exception err)
		{
			Debug.Log(err);
			socket.Close();
		}
	}

	void OnApplicationQuit()
	{
		if(socket != null){
			socket.Close();
		}
	}
}
