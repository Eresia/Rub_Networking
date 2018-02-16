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

	private bool isClosed;

	public virtual void Launch(IPAddress address, int port)
	{
		isClosed = false;
		receivePoint = new IPEndPoint(address, port);
		socket = new UdpClient(receivePoint);
		actionQueue = new ConcurrentQueue<NetworkAction>();
		parser = GetParser();
		StartCoroutine(MakeActionsCoroutine());
		socket.BeginReceive(new AsyncCallback(ReceiveCallback), new IPEndPoint(0, 0));
	}

	private IEnumerator MakeActionsCoroutine(){
		while(!isClosed){
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

	public void AddMainThreadAction(NetworkAction action){
		actionQueue.Enqueue(action);
	}

	public void SendData(IPEndPoint client, Data message){
		if(message != null){
			byte[] buffer = parser.ToBytes(message);
			CustomDebug.Log("Send : " + message.GetType(), VerboseLevel.ALL);
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
			Close();
		}
		catch (Exception err)
		{
			Close();
			Debug.LogException(err);
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
			Close();
		}
		catch (Exception err)
		{
			Close();
			Debug.LogException(err);
		}
	}

	public void Close(){
		socket.Close();
		isClosed = true;
	}

	void OnApplicationQuit()
	{
		if(socket != null){
			Close();
		}
	}
}
