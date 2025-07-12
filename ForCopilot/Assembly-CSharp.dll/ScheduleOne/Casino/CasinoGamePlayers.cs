using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x0200079A RID: 1946
	public class CasinoGamePlayers : NetworkBehaviour
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000DB525 File Offset: 0x000D9725
		public int CurrentPlayerCount
		{
			get
			{
				return this.Players.Count((Player p) => p != null);
			}
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x000DB551 File Offset: 0x000D9751
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CasinoGamePlayers_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000DB568 File Offset: 0x000D9768
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.CurrentPlayerCount > 0)
			{
				this.SetPlayerList(connection, this.GetPlayerObjects());
				foreach (Player player in this.Players)
				{
					if (!(player == null) && this.playerScores[player] != 0)
					{
						this.SetPlayerScore(connection, player.NetworkObject, this.playerScores[player]);
					}
				}
			}
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000DB5DB File Offset: 0x000D97DB
		public void AddPlayer(Player player)
		{
			this.RequestAddPlayer(player.NetworkObject);
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000DB5E9 File Offset: 0x000D97E9
		public void RemovePlayer(Player player)
		{
			this.RequestRemovePlayer(player.NetworkObject);
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000DB5F7 File Offset: 0x000D97F7
		public void SetPlayerScore(Player player, int score)
		{
			this.RequestSetScore(player.NetworkObject, score);
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000DB606 File Offset: 0x000D9806
		public int GetPlayerScore(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			if (this.playerScores.ContainsKey(player))
			{
				return this.playerScores[player];
			}
			return 0;
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000DB62F File Offset: 0x000D982F
		public Player GetPlayer(int index)
		{
			if (index < this.Players.Length)
			{
				return this.Players[index];
			}
			return null;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000DB646 File Offset: 0x000D9846
		public int GetPlayerIndex(Player player)
		{
			return ArrayExt.IndexOf<Player>(this.Players, player);
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000DB654 File Offset: 0x000D9854
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestAddPlayer(NetworkObject playerObject)
		{
			this.RpcWriter___Server_RequestAddPlayer_3323014238(playerObject);
			this.RpcLogic___RequestAddPlayer_3323014238(playerObject);
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000DB678 File Offset: 0x000D9878
		private void AddPlayerToArray(Player player)
		{
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] == null)
				{
					this.Players[i] = player;
					return;
				}
			}
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000DB6B0 File Offset: 0x000D98B0
		[ServerRpc(RequireOwnership = false)]
		private void RequestRemovePlayer(NetworkObject playerObject)
		{
			this.RpcWriter___Server_RequestRemovePlayer_3323014238(playerObject);
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000DB6C8 File Offset: 0x000D98C8
		private void RemovePlayerFromArray(Player player)
		{
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] == player)
				{
					this.Players[i] = null;
					return;
				}
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000DB700 File Offset: 0x000D9900
		[ServerRpc(RequireOwnership = false)]
		private void RequestSetScore(NetworkObject playerObject, int score)
		{
			this.RpcWriter___Server_RequestSetScore_4172557123(playerObject, score);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000DB710 File Offset: 0x000D9910
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetPlayerScore(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPlayerScore_1865307316(conn, playerObject, score);
				this.RpcLogic___SetPlayerScore_1865307316(conn, playerObject, score);
			}
			else
			{
				this.RpcWriter___Target_SetPlayerScore_1865307316(conn, playerObject, score);
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000DB760 File Offset: 0x000D9960
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetPlayerList(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPlayerList_204172449(conn, playerObjects);
				this.RpcLogic___SetPlayerList_204172449(conn, playerObjects);
			}
			else
			{
				this.RpcWriter___Target_SetPlayerList_204172449(conn, playerObjects);
			}
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000DB7A1 File Offset: 0x000D99A1
		public CasinoGamePlayerData GetPlayerData()
		{
			return this.GetPlayerData(Player.Local);
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000DB7AE File Offset: 0x000D99AE
		public CasinoGamePlayerData GetPlayerData(Player player)
		{
			if (!this.playerDatas.ContainsKey(player))
			{
				this.playerDatas.Add(player, new CasinoGamePlayerData(this, player));
			}
			return this.playerDatas[player];
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000DB7DD File Offset: 0x000D99DD
		public CasinoGamePlayerData GetPlayerData(int index)
		{
			if (index < this.Players.Length && this.Players[index] != null)
			{
				return this.GetPlayerData(this.Players[index]);
			}
			return null;
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000DB80A File Offset: 0x000D9A0A
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerBool(NetworkObject playerObject, string key, bool value)
		{
			this.RpcWriter___Server_SendPlayerBool_77262511(playerObject, key, value);
			this.RpcLogic___SendPlayerBool_77262511(playerObject, key, value);
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000DB830 File Offset: 0x000D9A30
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerBool(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerBool_1748594478(conn, playerObject, key, value);
				this.RpcLogic___ReceivePlayerBool_1748594478(conn, playerObject, key, value);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerBool_1748594478(conn, playerObject, key, value);
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000DB889 File Offset: 0x000D9A89
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerFloat(NetworkObject playerObject, string key, float value)
		{
			this.RpcWriter___Server_SendPlayerFloat_2931762093(playerObject, key, value);
			this.RpcLogic___SendPlayerFloat_2931762093(playerObject, key, value);
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000DB8B0 File Offset: 0x000D9AB0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerFloat(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
				this.RpcLogic___ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000DB90C File Offset: 0x000D9B0C
		private NetworkObject[] GetPlayerObjects()
		{
			NetworkObject[] array = new NetworkObject[this.PlayerLimit];
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] != null)
				{
					array[i] = this.Players[i].NetworkObject;
				}
			}
			return array;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000DB97C File Offset: 0x000D9B7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_RequestAddPlayer_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayer_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_RequestSetScore_4172557123));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerScore_1865307316));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetPlayerScore_1865307316));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerList_204172449));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetPlayerList_204172449));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerBool_77262511));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerBool_1748594478));
			base.RegisterTargetRpc(9U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerBool_1748594478));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerFloat_2931762093));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerFloat_2317689966));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerFloat_2317689966));
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000DBAC5 File Offset: 0x000D9CC5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000DBAD8 File Offset: 0x000D9CD8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000DBAE8 File Offset: 0x000D9CE8
		private void RpcWriter___Server_RequestAddPlayer_3323014238(NetworkObject playerObject)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000DBB90 File Offset: 0x000D9D90
		private void RpcLogic___RequestAddPlayer_3323014238(NetworkObject playerObject)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component != null && !this.Players.Contains(component))
			{
				this.AddPlayerToArray(component);
				if (!this.playerScores.ContainsKey(component))
				{
					this.playerScores.Add(component, 0);
				}
				if (!this.playerDatas.ContainsKey(component))
				{
					this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
				}
			}
			this.SetPlayerList(null, this.GetPlayerObjects());
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DBC0C File Offset: 0x000D9E0C
		private void RpcReader___Server_RequestAddPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RequestAddPlayer_3323014238(playerObject);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000DBC4C File Offset: 0x000D9E4C
		private void RpcWriter___Server_RequestRemovePlayer_3323014238(NetworkObject playerObject)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000DBCF4 File Offset: 0x000D9EF4
		private void RpcLogic___RequestRemovePlayer_3323014238(NetworkObject playerObject)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component != null && this.Players.Contains(component))
			{
				this.RemovePlayerFromArray(component);
			}
			this.SetPlayerList(null, this.GetPlayerObjects());
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000DBD34 File Offset: 0x000D9F34
		private void RpcReader___Server_RequestRemovePlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestRemovePlayer_3323014238(playerObject);
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000DBD68 File Offset: 0x000D9F68
		private void RpcWriter___Server_RequestSetScore_4172557123(NetworkObject playerObject, int score)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteInt32(score, 1);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000DBE21 File Offset: 0x000DA021
		private void RpcLogic___RequestSetScore_4172557123(NetworkObject playerObject, int score)
		{
			this.SetPlayerScore(null, playerObject, score);
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000DBE2C File Offset: 0x000DA02C
		private void RpcReader___Server_RequestSetScore_4172557123(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestSetScore_4172557123(playerObject, score);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000DBE74 File Offset: 0x000DA074
		private void RpcWriter___Observers_SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteInt32(score, 1);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000DBF3C File Offset: 0x000DA13C
		private void RpcLogic___SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerScores.ContainsKey(component))
			{
				this.playerScores.Add(component, score);
			}
			else
			{
				this.playerScores[component] = score;
			}
			if (this.onPlayerScoresChanged != null)
			{
				this.onPlayerScoresChanged.Invoke();
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000DBF98 File Offset: 0x000DA198
		private void RpcReader___Observers_SetPlayerScore_1865307316(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerScore_1865307316(null, playerObject, score);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000DBFEC File Offset: 0x000DA1EC
		private void RpcWriter___Target_SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteInt32(score, 1);
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000DC0B4 File Offset: 0x000DA2B4
		private void RpcReader___Target_SetPlayerScore_1865307316(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPlayerScore_1865307316(base.LocalConnection, playerObject, score);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000DC104 File Offset: 0x000DA304
		private void RpcWriter___Observers_SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated(playerObjects);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000DC1BC File Offset: 0x000DA3BC
		private void RpcLogic___SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			this.Players = new Player[this.PlayerLimit];
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				this.Players[i] = null;
			}
			for (int j = 0; j < playerObjects.Length; j++)
			{
				if (!(playerObjects[j] == null))
				{
					Player component = playerObjects[j].GetComponent<Player>();
					if (component != null)
					{
						this.Players[j] = component;
						if (!this.playerScores.ContainsKey(component))
						{
							this.playerScores.Add(component, 0);
						}
						if (!this.playerDatas.ContainsKey(component))
						{
							this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
						}
					}
				}
			}
			if (this.onPlayerListChanged != null)
			{
				this.onPlayerListChanged.Invoke();
			}
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000DC278 File Offset: 0x000DA478
		private void RpcReader___Observers_SetPlayerList_204172449(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject[] playerObjects = GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerList_204172449(null, playerObjects);
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000DC2B4 File Offset: 0x000DA4B4
		private void RpcWriter___Target_SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated(playerObjects);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000DC36C File Offset: 0x000DA56C
		private void RpcReader___Target_SetPlayerList_204172449(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject[] playerObjects = GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPlayerList_204172449(base.LocalConnection, playerObjects);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000DC3A4 File Offset: 0x000DA5A4
		private void RpcWriter___Server_SendPlayerBool_77262511(NetworkObject playerObject, string key, bool value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000DC465 File Offset: 0x000DA665
		public void RpcLogic___SendPlayerBool_77262511(NetworkObject playerObject, string key, bool value)
		{
			this.ReceivePlayerBool(null, playerObject, key, value);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000DC474 File Offset: 0x000DA674
		private void RpcReader___Server_SendPlayerBool_77262511(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerBool_77262511(playerObject, key, value);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000DC4D4 File Offset: 0x000DA6D4
		private void RpcWriter___Observers_ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000DC5A4 File Offset: 0x000DA7A4
		private void RpcLogic___ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerDatas.ContainsKey(component))
			{
				this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
			}
			this.playerDatas[component].SetData<bool>(key, value, false);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000DC5F8 File Offset: 0x000DA7F8
		private void RpcReader___Observers_ReceivePlayerBool_1748594478(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerBool_1748594478(null, playerObject, key, value);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000DC658 File Offset: 0x000DA858
		private void RpcWriter___Target_ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendTargetRpc(9U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000DC728 File Offset: 0x000DA928
		private void RpcReader___Target_ReceivePlayerBool_1748594478(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerBool_1748594478(base.LocalConnection, playerObject, key, value);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000DC784 File Offset: 0x000DA984
		private void RpcWriter___Server_SendPlayerFloat_2931762093(NetworkObject playerObject, string key, float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteSingle(value, 0);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000DC84A File Offset: 0x000DAA4A
		public void RpcLogic___SendPlayerFloat_2931762093(NetworkObject playerObject, string key, float value)
		{
			this.ReceivePlayerFloat(null, playerObject, key, value);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000DC858 File Offset: 0x000DAA58
		private void RpcReader___Server_SendPlayerFloat_2931762093(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerFloat_2931762093(playerObject, key, value);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000DC8C0 File Offset: 0x000DAAC0
		private void RpcWriter___Observers_ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteSingle(value, 0);
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000DC998 File Offset: 0x000DAB98
		private void RpcLogic___ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerDatas.ContainsKey(component))
			{
				this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
			}
			this.playerDatas[component].SetData<float>(key, value, false);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000DC9EC File Offset: 0x000DABEC
		private void RpcReader___Observers_ReceivePlayerFloat_2317689966(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerFloat_2317689966(null, playerObject, key, value);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000DCA50 File Offset: 0x000DAC50
		private void RpcWriter___Target_ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteSingle(value, 0);
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000DCB24 File Offset: 0x000DAD24
		private void RpcReader___Target_ReceivePlayerFloat_2317689966(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerFloat_2317689966(base.LocalConnection, playerObject, key, value);
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000DCB82 File Offset: 0x000DAD82
		private void dll()
		{
			this.Players = new Player[this.PlayerLimit];
		}

		// Token: 0x04002531 RID: 9521
		public int PlayerLimit = 4;

		// Token: 0x04002532 RID: 9522
		private Player[] Players;

		// Token: 0x04002533 RID: 9523
		public UnityEvent onPlayerListChanged;

		// Token: 0x04002534 RID: 9524
		public UnityEvent onPlayerScoresChanged;

		// Token: 0x04002535 RID: 9525
		private Dictionary<Player, int> playerScores = new Dictionary<Player, int>();

		// Token: 0x04002536 RID: 9526
		private Dictionary<Player, CasinoGamePlayerData> playerDatas = new Dictionary<Player, CasinoGamePlayerData>();

		// Token: 0x04002537 RID: 9527
		private bool dll_Excuted;

		// Token: 0x04002538 RID: 9528
		private bool dll_Excuted;
	}
}
