using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Transporting;
using FishySteamworks.Client;
using Steamworks;

namespace FishySteamworks.Server
{
	// Token: 0x02000C99 RID: 3225
	public class ServerSocket : CommonSocket
	{
		// Token: 0x06005A98 RID: 23192 RVA: 0x0017D6B9 File Offset: 0x0017B8B9
		internal RemoteConnectionState GetConnectionState(int connectionId)
		{
			if (this._steamConnections.Second.ContainsKey(connectionId))
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x0017D6D1 File Offset: 0x0017B8D1
		internal void ResetInvalidSocket()
		{
			if (this._socket == HSteamListenSocket.Invalid)
			{
				base.SetLocalConnectionState(0, true);
			}
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x0017D6F0 File Offset: 0x0017B8F0
		internal bool StartConnection(string address, ushort port, int maximumClients, bool peerToPeer)
		{
			try
			{
				if (this._onRemoteConnectionStateCallback == null)
				{
					this._onRemoteConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnRemoteConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				this.PeerToPeer = peerToPeer;
				this.SetMaximumClients(maximumClients);
				this._nextConnectionId = 0;
				this._cachedConnectionIds.Clear();
				this._iteratingConnections = false;
				base.SetLocalConnectionState(1, true);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.CreateListenSocketP2P(0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					if (array != null)
					{
						steamNetworkingIPAddr.SetIPv6(array, port);
					}
					this._socket = SteamNetworkingSockets.CreateListenSocketIP(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(0, true);
				return false;
			}
			if (this._socket == HSteamListenSocket.Invalid)
			{
				base.SetLocalConnectionState(0, true);
				return false;
			}
			base.SetLocalConnectionState(2, true);
			return true;
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x0017D7F4 File Offset: 0x0017B9F4
		internal bool StopConnection()
		{
			if (this._socket != HSteamListenSocket.Invalid)
			{
				SteamNetworkingSockets.CloseListenSocket(this._socket);
				if (this._onRemoteConnectionStateCallback != null)
				{
					this._onRemoteConnectionStateCallback.Dispose();
					this._onRemoteConnectionStateCallback = null;
				}
				this._socket = HSteamListenSocket.Invalid;
			}
			this._pendingConnectionChanges.Clear();
			if (base.GetLocalConnectionState() == null)
			{
				return false;
			}
			base.SetLocalConnectionState(3, true);
			base.SetLocalConnectionState(0, true);
			return true;
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x0017D86C File Offset: 0x0017BA6C
		internal bool StopConnection(int connectionId)
		{
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					this._clientHost.StopConnection();
					return true;
				}
				return false;
			}
			else
			{
				HSteamNetConnection socket;
				if (this._steamConnections.Second.TryGetValue(connectionId, out socket))
				{
					return this.StopConnection(connectionId, socket);
				}
				this.Transport.NetworkManager.LogError(string.Format("Steam connection not found for connectionId {0}.", connectionId));
				return false;
			}
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x0017D8D8 File Offset: 0x0017BAD8
		private bool StopConnection(int connectionId, HSteamNetConnection socket)
		{
			SteamNetworkingSockets.CloseConnection(socket, 0, string.Empty, false);
			if (!this._iteratingConnections)
			{
				this.RemoveConnection(connectionId);
			}
			else
			{
				this._pendingConnectionChanges.Add(new ServerSocket.ConnectionChange(connectionId));
			}
			return true;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x0017D90C File Offset: 0x0017BB0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void OnRemoteConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			ulong steamID = args.m_info.m_identityRemote.GetSteamID64();
			if (args.m_info.m_eState == 1)
			{
				if (this._steamConnections.Count >= this.GetMaximumClients())
				{
					this.Transport.NetworkManager.Log(string.Format("Incoming connection {0} was rejected because would exceed the maximum connection count.", steamID));
					SteamNetworkingSockets.CloseConnection(args.m_hConn, 0, "Max Connection Count", false);
					return;
				}
				EResult eresult = SteamNetworkingSockets.AcceptConnection(args.m_hConn);
				if (eresult == 1)
				{
					this.Transport.NetworkManager.Log(string.Format("Accepting connection {0}", steamID));
					return;
				}
				this.Transport.NetworkManager.Log(string.Format("Connection {0} could not be accepted: {1}", steamID, eresult.ToString()));
				return;
			}
			else
			{
				if (args.m_info.m_eState != 3)
				{
					if (args.m_info.m_eState == 4 || args.m_info.m_eState == 5)
					{
						int connectionId;
						if (this._steamConnections.TryGetValue(args.m_hConn, out connectionId))
						{
							this.StopConnection(connectionId, args.m_hConn);
							return;
						}
					}
					else
					{
						this.Transport.NetworkManager.Log(string.Format("Connection {0} state changed: {1}", steamID, args.m_info.m_eState.ToString()));
					}
					return;
				}
				int num;
				if (this._cachedConnectionIds.Count <= 0)
				{
					int nextConnectionId = this._nextConnectionId;
					this._nextConnectionId = nextConnectionId + 1;
					num = nextConnectionId;
				}
				else
				{
					num = this._cachedConnectionIds.Dequeue();
				}
				int num2 = num;
				if (!this._iteratingConnections)
				{
					this.AddConnection(num2, args.m_hConn, args.m_info.m_identityRemote.GetSteamID());
					return;
				}
				this._pendingConnectionChanges.Add(new ServerSocket.ConnectionChange(num2, args.m_hConn, args.m_info.m_identityRemote.GetSteamID()));
				return;
			}
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x0017DAE8 File Offset: 0x0017BCE8
		private void AddConnection(int connectionId, HSteamNetConnection steamConnection, CSteamID steamId)
		{
			this._steamConnections.Add(steamConnection, connectionId);
			this._steamIds.Add(steamId, connectionId);
			this.Transport.NetworkManager.Log(string.Format("Client with SteamID {0} connected. Assigning connection id {1}", steamId.m_SteamID, connectionId));
			this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(2, connectionId, this.Transport.Index));
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x0017DB58 File Offset: 0x0017BD58
		private void RemoveConnection(int connectionId)
		{
			this._steamConnections.Remove(connectionId);
			this._steamIds.Remove(connectionId);
			this.Transport.NetworkManager.Log(string.Format("Client with ConnectionID {0} disconnected.", connectionId));
			this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(0, connectionId, this.Transport.Index));
			this._cachedConnectionIds.Enqueue(connectionId);
		}

		// Token: 0x06005AA1 RID: 23201 RVA: 0x0017DBC8 File Offset: 0x0017BDC8
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			this._iteratingConnections = true;
			foreach (HSteamNetConnection hsteamNetConnection in this._steamConnections.FirstTypes)
			{
				SteamNetworkingSockets.FlushMessagesOnConnection(hsteamNetConnection);
			}
			this._iteratingConnections = false;
			this.ProcessPendingConnectionChanges();
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x0017DC38 File Offset: 0x0017BE38
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() == null || base.GetLocalConnectionState() == 3)
			{
				return;
			}
			this._iteratingConnections = true;
			while (this._clientHostIncoming.Count > 0)
			{
				LocalPacket localPacket = this._clientHostIncoming.Dequeue();
				ArraySegment<byte> arraySegment = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(arraySegment, localPacket.Channel, 32767, this.Transport.Index));
			}
			foreach (KeyValuePair<HSteamNetConnection, int> keyValuePair in this._steamConnections.First)
			{
				HSteamNetConnection key = keyValuePair.Key;
				int value = keyValuePair.Value;
				int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(key, this.MessagePointers, 256);
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						ArraySegment<byte> arraySegment2;
						byte b;
						base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out arraySegment2, out b);
						this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(arraySegment2, b, value, this.Transport.Index));
					}
				}
			}
			this._iteratingConnections = false;
			this.ProcessPendingConnectionChanges();
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x0017DD7C File Offset: 0x0017BF7C
		private void ProcessPendingConnectionChanges()
		{
			foreach (ServerSocket.ConnectionChange connectionChange in this._pendingConnectionChanges)
			{
				if (connectionChange.IsConnect)
				{
					this.AddConnection(connectionChange.ConnectionId, connectionChange.SteamConnection, connectionChange.SteamId);
				}
				else
				{
					this.RemoveConnection(connectionChange.ConnectionId);
				}
			}
			this._pendingConnectionChanges.Clear();
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x0017DE04 File Offset: 0x0017C004
		internal void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					LocalPacket packet = new LocalPacket(segment, channelId);
					this._clientHost.ReceivedFromLocalServer(packet);
				}
				return;
			}
			HSteamNetConnection hsteamNetConnection;
			if (this._steamConnections.TryGetValue(connectionId, out hsteamNetConnection))
			{
				EResult eresult = base.Send(hsteamNetConnection, segment, channelId);
				if (eresult == 3 || eresult == 8)
				{
					this.Transport.NetworkManager.Log(string.Format("Connection to {0} was lost.", connectionId));
					this.StopConnection(connectionId, hsteamNetConnection);
					return;
				}
				if (eresult != 1)
				{
					this.Transport.NetworkManager.LogError("Could not send: " + eresult.ToString());
					return;
				}
			}
			else
			{
				this.Transport.NetworkManager.LogError(string.Format("ConnectionId {0} does not exist, data will not be sent.", connectionId));
			}
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x0017DEDC File Offset: 0x0017C0DC
		internal string GetConnectionAddress(int connectionId)
		{
			CSteamID csteamID;
			if (this._steamIds.TryGetValue(connectionId, out csteamID))
			{
				return csteamID.ToString();
			}
			this.Transport.NetworkManager.LogError(string.Format("ConnectionId {0} is invalid; address cannot be returned.", connectionId));
			return string.Empty;
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x0017DF2C File Offset: 0x0017C12C
		internal void SetMaximumClients(int value)
		{
			this._maximumClients = Math.Min(value, 32766);
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x0017DF3F File Offset: 0x0017C13F
		internal int GetMaximumClients()
		{
			return this._maximumClients;
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x0017DF47 File Offset: 0x0017C147
		internal void SetClientHostSocket(ClientHostSocket socket)
		{
			this._clientHost = socket;
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x0017DF50 File Offset: 0x0017C150
		internal void OnClientHostState(bool started)
		{
			FishySteamworks fishySteamworks = (FishySteamworks)this.Transport;
			CSteamID key;
			key..ctor(fishySteamworks.LocalUserSteamID);
			if (!started && this._clientHostStarted)
			{
				base.ClearQueue(this._clientHostIncoming);
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(0, 32767, this.Transport.Index));
				this._steamIds.Remove(key);
			}
			else if (started)
			{
				this._steamIds[key] = 32767;
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(2, 32767, this.Transport.Index));
			}
			this._clientHostStarted = started;
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x0017DFF8 File Offset: 0x0017C1F8
		internal void ReceivedFromClientHost(LocalPacket packet)
		{
			if (!this._clientHostStarted)
			{
				return;
			}
			this._clientHostIncoming.Enqueue(packet);
		}

		// Token: 0x04004284 RID: 17028
		private BidirectionalDictionary<HSteamNetConnection, int> _steamConnections = new BidirectionalDictionary<HSteamNetConnection, int>();

		// Token: 0x04004285 RID: 17029
		private BidirectionalDictionary<CSteamID, int> _steamIds = new BidirectionalDictionary<CSteamID, int>();

		// Token: 0x04004286 RID: 17030
		private int _maximumClients;

		// Token: 0x04004287 RID: 17031
		private int _nextConnectionId;

		// Token: 0x04004288 RID: 17032
		private HSteamListenSocket _socket = new HSteamListenSocket(0U);

		// Token: 0x04004289 RID: 17033
		private Queue<LocalPacket> _clientHostIncoming = new Queue<LocalPacket>();

		// Token: 0x0400428A RID: 17034
		private bool _clientHostStarted;

		// Token: 0x0400428B RID: 17035
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onRemoteConnectionStateCallback;

		// Token: 0x0400428C RID: 17036
		private Queue<int> _cachedConnectionIds = new Queue<int>();

		// Token: 0x0400428D RID: 17037
		private ClientHostSocket _clientHost;

		// Token: 0x0400428E RID: 17038
		private bool _iteratingConnections;

		// Token: 0x0400428F RID: 17039
		private List<ServerSocket.ConnectionChange> _pendingConnectionChanges = new List<ServerSocket.ConnectionChange>();

		// Token: 0x02000C9A RID: 3226
		public struct ConnectionChange
		{
			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06005AAC RID: 23212 RVA: 0x0017E066 File Offset: 0x0017C266
			public bool IsConnect
			{
				get
				{
					return this.SteamId.IsValid();
				}
			}

			// Token: 0x06005AAD RID: 23213 RVA: 0x0017E073 File Offset: 0x0017C273
			public ConnectionChange(int id)
			{
				this.ConnectionId = id;
				this.SteamId = CSteamID.Nil;
				this.SteamConnection = default(HSteamNetConnection);
			}

			// Token: 0x06005AAE RID: 23214 RVA: 0x0017E093 File Offset: 0x0017C293
			public ConnectionChange(int id, HSteamNetConnection steamConnection, CSteamID steamId)
			{
				this.ConnectionId = id;
				this.SteamConnection = steamConnection;
				this.SteamId = steamId;
			}

			// Token: 0x04004290 RID: 17040
			public int ConnectionId;

			// Token: 0x04004291 RID: 17041
			public HSteamNetConnection SteamConnection;

			// Token: 0x04004292 RID: 17042
			public CSteamID SteamId;
		}
	}
}
