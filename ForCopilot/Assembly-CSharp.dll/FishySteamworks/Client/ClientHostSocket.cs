using System;
using System.Collections.Generic;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using FishySteamworks.Server;

namespace FishySteamworks.Client
{
	// Token: 0x02000C9B RID: 3227
	public class ClientHostSocket : CommonSocket
	{
		// Token: 0x06005AAF RID: 23215 RVA: 0x0017E0AA File Offset: 0x0017C2AA
		internal void CheckSetStarted()
		{
			if (this._server != null && base.GetLocalConnectionState() == 1 && this._server.GetLocalConnectionState() == 2)
			{
				this.SetLocalConnectionState(2, false);
			}
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x0017E0D3 File Offset: 0x0017C2D3
		internal bool StartConnection(ServerSocket serverSocket)
		{
			this._server = serverSocket;
			this._server.SetClientHostSocket(this);
			if (this._server.GetLocalConnectionState() != 2)
			{
				return false;
			}
			this.SetLocalConnectionState(1, false);
			return true;
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x0017E101 File Offset: 0x0017C301
		protected override void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			base.SetLocalConnectionState(connectionState, server);
			if (connectionState == 2)
			{
				this._server.OnClientHostState(true);
				return;
			}
			this._server.OnClientHostState(false);
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x0017E128 File Offset: 0x0017C328
		internal bool StopConnection()
		{
			if (base.GetLocalConnectionState() == null || base.GetLocalConnectionState() == 3)
			{
				return false;
			}
			base.ClearQueue(this._incoming);
			this.SetLocalConnectionState(3, false);
			this.SetLocalConnectionState(0, false);
			this._server.SetClientHostSocket(null);
			return true;
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x0017E168 File Offset: 0x0017C368
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			while (this._incoming.Count > 0)
			{
				LocalPacket localPacket = this._incoming.Dequeue();
				ArraySegment<byte> arraySegment = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(arraySegment, localPacket.Channel, this.Transport.Index));
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x0017E1DA File Offset: 0x0017C3DA
		internal void ReceivedFromLocalServer(LocalPacket packet)
		{
			this._incoming.Enqueue(packet);
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x0017E1E8 File Offset: 0x0017C3E8
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			if (this._server.GetLocalConnectionState() != 2)
			{
				return;
			}
			LocalPacket packet = new LocalPacket(segment, channelId);
			this._server.ReceivedFromClientHost(packet);
		}

		// Token: 0x04004293 RID: 17043
		private ServerSocket _server;

		// Token: 0x04004294 RID: 17044
		private Queue<LocalPacket> _incoming = new Queue<LocalPacket>();
	}
}
