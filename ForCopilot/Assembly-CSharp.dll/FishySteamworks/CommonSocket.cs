using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using Steamworks;

namespace FishySteamworks
{
	// Token: 0x02000C96 RID: 3222
	public abstract class CommonSocket
	{
		// Token: 0x06005A61 RID: 23137 RVA: 0x0017CCB7 File Offset: 0x0017AEB7
		internal LocalConnectionState GetLocalConnectionState()
		{
			return this._connectionState;
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x0017CCC0 File Offset: 0x0017AEC0
		protected virtual void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			if (connectionState == this._connectionState)
			{
				return;
			}
			this._connectionState = connectionState;
			if (server)
			{
				this.Transport.HandleServerConnectionState(new ServerConnectionStateArgs(connectionState, this.Transport.Index));
				return;
			}
			this.Transport.HandleClientConnectionState(new ClientConnectionStateArgs(connectionState, this.Transport.Index));
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x0017CD1C File Offset: 0x0017AF1C
		internal virtual void Initialize(Transport t)
		{
			this.Transport = t;
			int num = this.Transport.GetMTU(0);
			num = Math.Max(num, this.Transport.GetMTU(1));
			this.InboundBuffer = new byte[num];
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x0017CD5C File Offset: 0x0017AF5C
		protected byte[] GetIPBytes(string address)
		{
			if (string.IsNullOrEmpty(address))
			{
				return null;
			}
			IPAddress ipaddress;
			if (!IPAddress.TryParse(address, out ipaddress))
			{
				this.Transport.NetworkManager.LogError("Could not parse address " + address + " to IPAddress.");
				return null;
			}
			return ipaddress.GetAddressBytes();
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x0017CDA8 File Offset: 0x0017AFA8
		protected EResult Send(HSteamNetConnection steamConnection, ArraySegment<byte> segment, byte channelId)
		{
			if (segment.Array.Length - 1 <= segment.Offset + segment.Count)
			{
				byte[] array = segment.Array;
				Array.Resize<byte>(ref array, array.Length + 1);
				array[array.Length - 1] = channelId;
			}
			else
			{
				segment.Array[segment.Offset + segment.Count] = channelId;
			}
			segment = new ArraySegment<byte>(segment.Array, segment.Offset, segment.Count + 1);
			GCHandle gchandle = GCHandle.Alloc(segment.Array, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject() + segment.Offset;
			int num = (channelId == 1) ? 0 : 8;
			long num2;
			EResult eresult = SteamNetworkingSockets.SendMessageToConnection(steamConnection, intPtr, (uint)segment.Count, num, ref num2);
			if (eresult != 1)
			{
				this.Transport.NetworkManager.LogWarning(string.Format("Send issue: {0}", eresult));
			}
			gchandle.Free();
			return eresult;
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x0017CE94 File Offset: 0x0017B094
		internal void ClearQueue(ConcurrentQueue<LocalPacket> queue)
		{
			LocalPacket localPacket;
			while (queue.TryDequeue(out localPacket))
			{
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x0017CEB8 File Offset: 0x0017B0B8
		internal void ClearQueue(Queue<LocalPacket> queue)
		{
			while (queue.Count > 0)
			{
				ByteArrayPool.Store(queue.Dequeue().Data);
			}
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x0017CED8 File Offset: 0x0017B0D8
		protected void GetMessage(IntPtr ptr, byte[] buffer, out ArraySegment<byte> segment, out byte channel)
		{
			SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(ptr);
			int cbSize = steamNetworkingMessage_t.m_cbSize;
			Marshal.Copy(steamNetworkingMessage_t.m_pData, buffer, 0, cbSize);
			SteamNetworkingMessage_t.Release(ptr);
			channel = buffer[cbSize - 1];
			segment = new ArraySegment<byte>(buffer, 0, cbSize - 1);
		}

		// Token: 0x0400426A RID: 17002
		private LocalConnectionState _connectionState;

		// Token: 0x0400426B RID: 17003
		protected bool PeerToPeer;

		// Token: 0x0400426C RID: 17004
		protected Transport Transport;

		// Token: 0x0400426D RID: 17005
		protected IntPtr[] MessagePointers = new IntPtr[256];

		// Token: 0x0400426E RID: 17006
		protected byte[] InboundBuffer;

		// Token: 0x0400426F RID: 17007
		protected const int MAX_MESSAGES = 256;
	}
}
