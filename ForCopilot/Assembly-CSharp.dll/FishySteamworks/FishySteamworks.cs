using System;
using FishNet.Managing;
using FishNet.Transporting;
using FishySteamworks.Client;
using FishySteamworks.Server;
using Steamworks;
using UnityEngine;

namespace FishySteamworks
{
	// Token: 0x02000C98 RID: 3224
	public class FishySteamworks : Transport
	{
		// Token: 0x06005A6B RID: 23147 RVA: 0x0017CF8C File Offset: 0x0017B18C
		~FishySteamworks()
		{
			this.Shutdown();
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x0017CFB8 File Offset: 0x0017B1B8
		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			this._client = new ClientSocket();
			this._clientHost = new ClientHostSocket();
			this._server = new ServerSocket();
			this.CreateChannelData();
			this._client.Initialize(this);
			this._clientHost.Initialize(this);
			this._server.Initialize(this);
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x0017D018 File Offset: 0x0017B218
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x0017D020 File Offset: 0x0017B220
		private void Update()
		{
			this._clientHost.CheckSetStarted();
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x0017D02D File Offset: 0x0017B22D
		private void CreateChannelData()
		{
			this._mtus = new int[]
			{
				1048576,
				1200
			};
		}

		// Token: 0x06005A70 RID: 23152 RVA: 0x0017D04C File Offset: 0x0017B24C
		private bool InitializeRelayNetworkAccess()
		{
			bool result;
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				if (this.IsNetworkAccessAvailable())
				{
					this.LocalUserSteamID = SteamUser.GetSteamID().m_SteamID;
				}
				this._shutdownCalled = false;
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x0017D098 File Offset: 0x0017B298
		public bool IsNetworkAccessAvailable()
		{
			bool result;
			try
			{
				InteropHelp.TestIfAvailableClient();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x0017D0C4 File Offset: 0x0017B2C4
		public override string GetConnectionAddress(int connectionId)
		{
			return this._server.GetConnectionAddress(connectionId);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06005A73 RID: 23155 RVA: 0x0017D0D4 File Offset: 0x0017B2D4
		// (remove) Token: 0x06005A74 RID: 23156 RVA: 0x0017D10C File Offset: 0x0017B30C
		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06005A75 RID: 23157 RVA: 0x0017D144 File Offset: 0x0017B344
		// (remove) Token: 0x06005A76 RID: 23158 RVA: 0x0017D17C File Offset: 0x0017B37C
		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06005A77 RID: 23159 RVA: 0x0017D1B4 File Offset: 0x0017B3B4
		// (remove) Token: 0x06005A78 RID: 23160 RVA: 0x0017D1EC File Offset: 0x0017B3EC
		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		// Token: 0x06005A79 RID: 23161 RVA: 0x0017D221 File Offset: 0x0017B421
		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				return this._server.GetLocalConnectionState();
			}
			return this._client.GetLocalConnectionState();
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x0017D23D File Offset: 0x0017B43D
		public override RemoteConnectionState GetConnectionState(int connectionId)
		{
			return this._server.GetConnectionState(connectionId);
		}

		// Token: 0x06005A7B RID: 23163 RVA: 0x0017D24B File Offset: 0x0017B44B
		public override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
			Action<ClientConnectionStateArgs> onClientConnectionState = this.OnClientConnectionState;
			if (onClientConnectionState == null)
			{
				return;
			}
			onClientConnectionState(connectionStateArgs);
		}

		// Token: 0x06005A7C RID: 23164 RVA: 0x0017D25E File Offset: 0x0017B45E
		public override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
			Action<ServerConnectionStateArgs> onServerConnectionState = this.OnServerConnectionState;
			if (onServerConnectionState == null)
			{
				return;
			}
			onServerConnectionState(connectionStateArgs);
		}

		// Token: 0x06005A7D RID: 23165 RVA: 0x0017D271 File Offset: 0x0017B471
		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			Action<RemoteConnectionStateArgs> onRemoteConnectionState = this.OnRemoteConnectionState;
			if (onRemoteConnectionState == null)
			{
				return;
			}
			onRemoteConnectionState(connectionStateArgs);
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x0017D284 File Offset: 0x0017B484
		public override void IterateIncoming(bool server)
		{
			if (server)
			{
				this._server.IterateIncoming();
				return;
			}
			this._client.IterateIncoming();
			this._clientHost.IterateIncoming();
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x0017D2AB File Offset: 0x0017B4AB
		public override void IterateOutgoing(bool server)
		{
			if (server)
			{
				this._server.IterateOutgoing();
				return;
			}
			this._client.IterateOutgoing();
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06005A80 RID: 23168 RVA: 0x0017D2C8 File Offset: 0x0017B4C8
		// (remove) Token: 0x06005A81 RID: 23169 RVA: 0x0017D300 File Offset: 0x0017B500
		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		// Token: 0x06005A82 RID: 23170 RVA: 0x0017D335 File Offset: 0x0017B535
		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
			Action<ClientReceivedDataArgs> onClientReceivedData = this.OnClientReceivedData;
			if (onClientReceivedData == null)
			{
				return;
			}
			onClientReceivedData(receivedDataArgs);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06005A83 RID: 23171 RVA: 0x0017D348 File Offset: 0x0017B548
		// (remove) Token: 0x06005A84 RID: 23172 RVA: 0x0017D380 File Offset: 0x0017B580
		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		// Token: 0x06005A85 RID: 23173 RVA: 0x0017D3B5 File Offset: 0x0017B5B5
		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
			Action<ServerReceivedDataArgs> onServerReceivedData = this.OnServerReceivedData;
			if (onServerReceivedData == null)
			{
				return;
			}
			onServerReceivedData(receivedDataArgs);
		}

		// Token: 0x06005A86 RID: 23174 RVA: 0x0017D3C8 File Offset: 0x0017B5C8
		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			this._client.SendToServer(channelId, segment);
			this._clientHost.SendToServer(channelId, segment);
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x0017D3E4 File Offset: 0x0017B5E4
		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			this._server.SendToClient(channelId, segment, connectionId);
		}

		// Token: 0x06005A88 RID: 23176 RVA: 0x0017D3F4 File Offset: 0x0017B5F4
		public override int GetMaximumClients()
		{
			return this._server.GetMaximumClients();
		}

		// Token: 0x06005A89 RID: 23177 RVA: 0x0017D401 File Offset: 0x0017B601
		public override void SetMaximumClients(int value)
		{
			this._server.SetMaximumClients(value);
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x0017D40F File Offset: 0x0017B60F
		public override void SetClientAddress(string address)
		{
			this._clientAddress = address;
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x0017D418 File Offset: 0x0017B618
		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
			this._serverBindAddress = address;
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x0017D421 File Offset: 0x0017B621
		public override void SetPort(ushort port)
		{
			this._port = port;
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x0017D42A File Offset: 0x0017B62A
		public override bool StartConnection(bool server)
		{
			if (server)
			{
				return this.StartServer();
			}
			return this.StartClient(this._clientAddress);
		}

		// Token: 0x06005A8E RID: 23182 RVA: 0x0017D442 File Offset: 0x0017B642
		public override bool StopConnection(bool server)
		{
			if (server)
			{
				return this.StopServer();
			}
			return this.StopClient();
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x0017D454 File Offset: 0x0017B654
		public override bool StopConnection(int connectionId, bool immediately)
		{
			return this.StopClient(connectionId, immediately);
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x0017D45E File Offset: 0x0017B65E
		public override void Shutdown()
		{
			if (this._shutdownCalled)
			{
				return;
			}
			this._shutdownCalled = true;
			this.StopConnection(false);
			this.StopConnection(true);
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x0017D480 File Offset: 0x0017B680
		private bool StartServer()
		{
			if (!this.InitializeRelayNetworkAccess())
			{
				base.NetworkManager.LogError("RelayNetworkAccess could not be initialized.");
				return false;
			}
			if (!this.IsNetworkAccessAvailable())
			{
				base.NetworkManager.LogError("Server network access is not available.");
				return false;
			}
			this._server.ResetInvalidSocket();
			if (this._server.GetLocalConnectionState() != null)
			{
				base.NetworkManager.LogError("Server is already running.");
				return false;
			}
			bool flag = this._client.GetLocalConnectionState() > 0;
			if (flag)
			{
				this._client.StopConnection();
			}
			bool flag2 = this._server.StartConnection(this._serverBindAddress, this._port, (int)this._maximumClients, this._peerToPeer);
			if (flag2 && flag)
			{
				this.StartConnection(false);
			}
			return flag2;
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x0017D539 File Offset: 0x0017B739
		private bool StopServer()
		{
			return this._server != null && this._server.StopConnection();
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x0017D550 File Offset: 0x0017B750
		private bool StartClient(string address)
		{
			if (this._server.GetLocalConnectionState() == null)
			{
				if (this._client.GetLocalConnectionState() != null)
				{
					base.NetworkManager.LogError("Client is already running.");
					return false;
				}
				if (this._clientHost.GetLocalConnectionState() != null)
				{
					this._clientHost.StopConnection();
				}
				if (!this.InitializeRelayNetworkAccess())
				{
					base.NetworkManager.LogError("RelayNetworkAccess could not be initialized.");
					return false;
				}
				if (!this.IsNetworkAccessAvailable())
				{
					base.NetworkManager.LogError("Client network access is not available.");
					return false;
				}
				this._client.StartConnection(address, this._port, this._peerToPeer);
			}
			else
			{
				this._clientHost.StartConnection(this._server);
			}
			return true;
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x0017D608 File Offset: 0x0017B808
		private bool StopClient()
		{
			bool flag = false;
			if (this._client != null)
			{
				flag |= this._client.StopConnection();
			}
			if (this._clientHost != null)
			{
				flag |= this._clientHost.StopConnection();
			}
			return flag;
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x0017D644 File Offset: 0x0017B844
		private bool StopClient(int connectionId, bool immediately)
		{
			return this._server.StopConnection(connectionId);
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x0017D652 File Offset: 0x0017B852
		public override int GetMTU(byte channel)
		{
			if ((int)channel >= this._mtus.Length)
			{
				Debug.LogError(string.Format("Channel {0} is out of bounds.", channel));
				return 0;
			}
			return this._mtus[(int)channel];
		}

		// Token: 0x04004273 RID: 17011
		[NonSerialized]
		public ulong LocalUserSteamID;

		// Token: 0x04004274 RID: 17012
		[Tooltip("Address server should bind to.")]
		[SerializeField]
		private string _serverBindAddress = string.Empty;

		// Token: 0x04004275 RID: 17013
		[Tooltip("Port to use.")]
		[SerializeField]
		private ushort _port = 7770;

		// Token: 0x04004276 RID: 17014
		[Tooltip("Maximum number of players which may be connected at once.")]
		[Range(1f, 65535f)]
		[SerializeField]
		private ushort _maximumClients = 9001;

		// Token: 0x04004277 RID: 17015
		[Tooltip("True if using peer to peer socket.")]
		[SerializeField]
		private bool _peerToPeer;

		// Token: 0x04004278 RID: 17016
		[Tooltip("Address client should connect to.")]
		[SerializeField]
		private string _clientAddress = string.Empty;

		// Token: 0x04004279 RID: 17017
		private int[] _mtus;

		// Token: 0x0400427A RID: 17018
		private ClientSocket _client;

		// Token: 0x0400427B RID: 17019
		private ClientHostSocket _clientHost;

		// Token: 0x0400427C RID: 17020
		private ServerSocket _server;

		// Token: 0x0400427D RID: 17021
		private bool _shutdownCalled = true;

		// Token: 0x0400427E RID: 17022
		internal const int CLIENT_HOST_ID = 32767;
	}
}
