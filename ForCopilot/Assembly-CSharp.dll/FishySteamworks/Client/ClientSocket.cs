using System;
using System.Diagnostics;
using System.Threading;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;

namespace FishySteamworks.Client
{
	// Token: 0x02000C9C RID: 3228
	public class ClientSocket : CommonSocket
	{
		// Token: 0x06005AB7 RID: 23223 RVA: 0x0017E238 File Offset: 0x0017C438
		private void CheckTimeout()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				if ((float)(stopwatch.ElapsedMilliseconds / 1000L) > this._connectTimeout)
				{
					this.StopConnection();
				}
				Thread.Sleep(50);
			}
			while (base.GetLocalConnectionState() == 1);
			stopwatch.Stop();
			this._timeoutThread.Abort();
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x0017E290 File Offset: 0x0017C490
		internal bool StartConnection(string address, ushort port, bool peerToPeer)
		{
			try
			{
				if (this._onLocalConnectionStateCallback == null)
				{
					this._onLocalConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnLocalConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				if (!peerToPeer && array == null)
				{
					base.SetLocalConnectionState(0, false);
					return false;
				}
				base.SetLocalConnectionState(1, false);
				this._connectTimeout = Time.unscaledTime + 8000f;
				this._timeoutThread = new Thread(new ThreadStart(this.CheckTimeout));
				this._timeoutThread.Start();
				this._hostSteamID = new CSteamID(ulong.Parse(address));
				SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
				steamNetworkingIdentity.SetSteamID(this._hostSteamID);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.ConnectP2P(ref steamNetworkingIdentity, 0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					steamNetworkingIPAddr.SetIPv6(array, port);
					this._socket = SteamNetworkingSockets.ConnectByIPAddress(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(0, false);
				return false;
			}
			return true;
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x0017E3B4 File Offset: 0x0017C5B4
		private void OnLocalConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			if (args.m_info.m_eState == 3)
			{
				base.SetLocalConnectionState(2, false);
				return;
			}
			if (args.m_info.m_eState == 4 || args.m_info.m_eState == 5)
			{
				this.Transport.NetworkManager.Log("Connection was closed by peer, " + args.m_info.m_szEndDebug);
				this.StopConnection();
				return;
			}
			this.Transport.NetworkManager.Log("Connection state changed: " + args.m_info.m_eState.ToString() + " - " + args.m_info.m_szEndDebug);
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x0017E464 File Offset: 0x0017C664
		internal bool StopConnection()
		{
			if (this._timeoutThread != null && this._timeoutThread.IsAlive)
			{
				this._timeoutThread.Abort();
			}
			if (this._socket != HSteamNetConnection.Invalid)
			{
				if (this._onLocalConnectionStateCallback != null)
				{
					this._onLocalConnectionStateCallback.Dispose();
					this._onLocalConnectionStateCallback = null;
				}
				SteamNetworkingSockets.CloseConnection(this._socket, 0, string.Empty, false);
				this._socket = HSteamNetConnection.Invalid;
			}
			if (base.GetLocalConnectionState() == null || base.GetLocalConnectionState() == 3)
			{
				return false;
			}
			base.SetLocalConnectionState(3, false);
			base.SetLocalConnectionState(0, false);
			return true;
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x0017E500 File Offset: 0x0017C700
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(this._socket, this.MessagePointers, 256);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					ArraySegment<byte> arraySegment;
					byte b;
					base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out arraySegment, out b);
					this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(arraySegment, b, this.Transport.Index));
				}
			}
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x0017E574 File Offset: 0x0017C774
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			EResult eresult = base.Send(this._socket, segment, channelId);
			if (eresult == 3 || eresult == 8)
			{
				this.Transport.NetworkManager.Log("Connection to server was lost.");
				this.StopConnection();
				return;
			}
			if (eresult != 1)
			{
				this.Transport.NetworkManager.LogError("Could not send: " + eresult.ToString());
			}
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x0017E5EA File Offset: 0x0017C7EA
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != 2)
			{
				return;
			}
			SteamNetworkingSockets.FlushMessagesOnConnection(this._socket);
		}

		// Token: 0x04004295 RID: 17045
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onLocalConnectionStateCallback;

		// Token: 0x04004296 RID: 17046
		private CSteamID _hostSteamID = CSteamID.Nil;

		// Token: 0x04004297 RID: 17047
		private HSteamNetConnection _socket;

		// Token: 0x04004298 RID: 17048
		private Thread _timeoutThread;

		// Token: 0x04004299 RID: 17049
		private float _connectTimeout = -1f;

		// Token: 0x0400429A RID: 17050
		private const float CONNECT_TIMEOUT_DURATION = 8000f;
	}
}
