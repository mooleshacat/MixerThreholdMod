using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000580 RID: 1408
	public class MessagingManager : NetworkSingleton<MessagingManager>
	{
		// Token: 0x060021DD RID: 8669 RVA: 0x0008B7A3 File Offset: 0x000899A3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Messaging.MessagingManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0008B7B8 File Offset: 0x000899B8
		public override void OnSpawnServer(NetworkConnection connection)
		{
			MessagingManager.<>c__DisplayClass2_0 CS$<>8__locals1 = new MessagingManager.<>c__DisplayClass2_0();
			CS$<>8__locals1.connection = connection;
			CS$<>8__locals1.<>4__this = this;
			base.OnSpawnServer(CS$<>8__locals1.connection);
			if (CS$<>8__locals1.connection.IsLocalClient)
			{
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<OnSpawnServer>g__SendMessages|0());
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0008B800 File Offset: 0x00089A00
		public MSGConversation GetConversation(NPC npc)
		{
			if (!this.ConversationMap.ContainsKey(npc))
			{
				Console.LogError("No conversation found for " + npc.fullName, null);
				return null;
			}
			return this.ConversationMap[npc];
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0008B834 File Offset: 0x00089A34
		public void Register(NPC npc, MSGConversation convs)
		{
			if (this.ConversationMap.ContainsKey(npc))
			{
				Console.LogError("Conversation already registered for " + npc.fullName, null);
				return;
			}
			this.ConversationMap.Add(npc, convs);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0008B868 File Offset: 0x00089A68
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMessage(Message m, bool notify, string npcID)
		{
			this.RpcWriter___Server_SendMessage_2134336246(m, notify, npcID);
			this.RpcLogic___SendMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0008B890 File Offset: 0x00089A90
		[ObserversRpc(RunLocally = true)]
		private void ReceiveMessage(Message m, bool notify, string npcID)
		{
			this.RpcWriter___Observers_ReceiveMessage_2134336246(m, notify, npcID);
			this.RpcLogic___ReceiveMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0008B8C1 File Offset: 0x00089AC1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMessageChain(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.RpcWriter___Server_SendMessageChain_3949292778(m, npcID, initialDelay, notify);
			this.RpcLogic___SendMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0008B8F0 File Offset: 0x00089AF0
		[ObserversRpc(RunLocally = true)]
		private void ReceiveMessageChain(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.RpcWriter___Observers_ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
			this.RpcLogic___ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0008B929 File Offset: 0x00089B29
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendResponse(int responseIndex, string npcID)
		{
			this.RpcWriter___Server_SendResponse_2801973956(responseIndex, npcID);
			this.RpcLogic___SendResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0008B948 File Offset: 0x00089B48
		[ObserversRpc(RunLocally = true)]
		private void ReceiveResponse(int responseIndex, string npcID)
		{
			this.RpcWriter___Observers_ReceiveResponse_2801973956(responseIndex, npcID);
			this.RpcLogic___ReceiveResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0008B971 File Offset: 0x00089B71
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerMessage(int sendableIndex, int sentIndex, string npcID)
		{
			this.RpcWriter___Server_SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
			this.RpcLogic___SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0008B998 File Offset: 0x00089B98
		[ObserversRpc(RunLocally = true)]
		private void ReceivePlayerMessage(int sendableIndex, int sentIndex, string npcID)
		{
			this.RpcWriter___Observers_ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
			this.RpcLogic___ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x0008B9CC File Offset: 0x00089BCC
		[TargetRpc]
		private void ReceiveMSGConversationData(NetworkConnection conn, string npcID, MSGConversationData data)
		{
			this.RpcWriter___Target_ReceiveMSGConversationData_2662241369(conn, npcID, data);
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0008B9EB File Offset: 0x00089BEB
		[ServerRpc(RequireOwnership = false)]
		public void ClearResponses(string npcID)
		{
			this.RpcWriter___Server_ClearResponses_3615296227(npcID);
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0008B9F8 File Offset: 0x00089BF8
		[ObserversRpc]
		private void ReceiveClearResponses(string npcID)
		{
			this.RpcWriter___Observers_ReceiveClearResponses_3615296227(npcID);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0008BA0F File Offset: 0x00089C0F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void ShowResponses(string npcID, List<Response> responses, float delay)
		{
			this.RpcWriter___Server_ShowResponses_995803534(npcID, responses, delay);
			this.RpcLogic___ShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0008BA38 File Offset: 0x00089C38
		[ObserversRpc(RunLocally = true)]
		private void ReceiveShowResponses(string npcID, List<Response> responses, float delay)
		{
			this.RpcWriter___Observers_ReceiveShowResponses_995803534(npcID, responses, delay);
			this.RpcLogic___ReceiveShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0008BA7C File Offset: 0x00089C7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendMessage_2134336246));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveMessage_2134336246));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendMessageChain_3949292778));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveMessageChain_3949292778));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendResponse_2801973956));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveResponse_2801973956));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerMessage_1952281135));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerMessage_1952281135));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveMSGConversationData_2662241369));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_ClearResponses_3615296227));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveClearResponses_3615296227));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_ShowResponses_995803534));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveShowResponses_995803534));
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0008BBCB File Offset: 0x00089DCB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0008BBE4 File Offset: 0x00089DE4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0008BBF4 File Offset: 0x00089DF4
		private void RpcWriter___Server_SendMessage_2134336246(Message m, bool notify, string npcID)
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
			writer.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated(m);
			writer.WriteBoolean(notify);
			writer.WriteString(npcID);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0008BCB5 File Offset: 0x00089EB5
		public void RpcLogic___SendMessage_2134336246(Message m, bool notify, string npcID)
		{
			this.ReceiveMessage(m, notify, npcID);
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x0008BCC0 File Offset: 0x00089EC0
		private void RpcReader___Server_SendMessage_2134336246(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Message m = GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds(PooledReader0);
			bool notify = PooledReader0.ReadBoolean();
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x0008BD20 File Offset: 0x00089F20
		private void RpcWriter___Observers_ReceiveMessage_2134336246(Message m, bool notify, string npcID)
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
			writer.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated(m);
			writer.WriteBoolean(notify);
			writer.WriteString(npcID);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x0008BDF0 File Offset: 0x00089FF0
		private void RpcLogic___ReceiveMessage_2134336246(Message m, bool notify, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendMessage(m, notify, false);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x0008BE34 File Offset: 0x0008A034
		private void RpcReader___Observers_ReceiveMessage_2134336246(PooledReader PooledReader0, Channel channel)
		{
			Message m = GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds(PooledReader0);
			bool notify = PooledReader0.ReadBoolean();
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0008BE94 File Offset: 0x0008A094
		private void RpcWriter___Server_SendMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
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
			writer.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated(m);
			writer.WriteString(npcID);
			writer.WriteSingle(initialDelay, 0);
			writer.WriteBoolean(notify);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x0008BF67 File Offset: 0x0008A167
		public void RpcLogic___SendMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.ReceiveMessageChain(m, npcID, initialDelay, notify);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x0008BF74 File Offset: 0x0008A174
		private void RpcReader___Server_SendMessageChain_3949292778(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			MessageChain m = GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds(PooledReader0);
			string npcID = PooledReader0.ReadString();
			float initialDelay = PooledReader0.ReadSingle(0);
			bool notify = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0008BFEC File Offset: 0x0008A1EC
		private void RpcWriter___Observers_ReceiveMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
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
			writer.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated(m);
			writer.WriteString(npcID);
			writer.WriteSingle(initialDelay, 0);
			writer.WriteBoolean(notify);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0008C0D0 File Offset: 0x0008A2D0
		private void RpcLogic___ReceiveMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendMessageChain(m, initialDelay, notify, false);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0008C118 File Offset: 0x0008A318
		private void RpcReader___Observers_ReceiveMessageChain_3949292778(PooledReader PooledReader0, Channel channel)
		{
			MessageChain m = GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds(PooledReader0);
			string npcID = PooledReader0.ReadString();
			float initialDelay = PooledReader0.ReadSingle(0);
			bool notify = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x0008C18C File Offset: 0x0008A38C
		private void RpcWriter___Server_SendResponse_2801973956(int responseIndex, string npcID)
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
			writer.WriteInt32(responseIndex, 1);
			writer.WriteString(npcID);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x0008C245 File Offset: 0x0008A445
		public void RpcLogic___SendResponse_2801973956(int responseIndex, string npcID)
		{
			this.ReceiveResponse(responseIndex, npcID);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0008C250 File Offset: 0x0008A450
		private void RpcReader___Server_SendResponse_2801973956(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int responseIndex = PooledReader0.ReadInt32(1);
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x0008C2A4 File Offset: 0x0008A4A4
		private void RpcWriter___Observers_ReceiveResponse_2801973956(int responseIndex, string npcID)
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
			writer.WriteInt32(responseIndex, 1);
			writer.WriteString(npcID);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0008C36C File Offset: 0x0008A56C
		private void RpcLogic___ReceiveResponse_2801973956(int responseIndex, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			MSGConversation msgconversation = this.ConversationMap[npc];
			if (msgconversation.currentResponses.Count <= responseIndex)
			{
				Console.LogWarning("Response index out of range for " + npc.fullName, null);
				return;
			}
			msgconversation.ResponseChosen(msgconversation.currentResponses[responseIndex], false);
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0008C3E0 File Offset: 0x0008A5E0
		private void RpcReader___Observers_ReceiveResponse_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int responseIndex = PooledReader0.ReadInt32(1);
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0008C434 File Offset: 0x0008A634
		private void RpcWriter___Server_SendPlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
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
			writer.WriteInt32(sendableIndex, 1);
			writer.WriteInt32(sentIndex, 1);
			writer.WriteString(npcID);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0008C4FF File Offset: 0x0008A6FF
		public void RpcLogic___SendPlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
		{
			this.ReceivePlayerMessage(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0008C50C File Offset: 0x0008A70C
		private void RpcReader___Server_SendPlayerMessage_1952281135(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int sendableIndex = PooledReader0.ReadInt32(1);
			int sentIndex = PooledReader0.ReadInt32(1);
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0008C578 File Offset: 0x0008A778
		private void RpcWriter___Observers_ReceivePlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
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
			writer.WriteInt32(sendableIndex, 1);
			writer.WriteInt32(sentIndex, 1);
			writer.WriteString(npcID);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0008C654 File Offset: 0x0008A854
		private void RpcLogic___ReceivePlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendPlayerMessage(sendableIndex, sentIndex, false);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0008C698 File Offset: 0x0008A898
		private void RpcReader___Observers_ReceivePlayerMessage_1952281135(PooledReader PooledReader0, Channel channel)
		{
			int sendableIndex = PooledReader0.ReadInt32(1);
			int sentIndex = PooledReader0.ReadInt32(1);
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0008C700 File Offset: 0x0008A900
		private void RpcWriter___Target_ReceiveMSGConversationData_2662241369(NetworkConnection conn, string npcID, MSGConversationData data)
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
			writer.WriteString(npcID);
			writer.Write___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(8U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0008C7C4 File Offset: 0x0008A9C4
		private void RpcLogic___ReceiveMSGConversationData_2662241369(NetworkConnection conn, string npcID, MSGConversationData data)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].Load(data);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0008C808 File Offset: 0x0008AA08
		private void RpcReader___Target_ReceiveMSGConversationData_2662241369(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			MSGConversationData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveMSGConversationData_2662241369(base.LocalConnection, npcID, data);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0008C850 File Offset: 0x0008AA50
		private void RpcWriter___Server_ClearResponses_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0008C8F7 File Offset: 0x0008AAF7
		public void RpcLogic___ClearResponses_3615296227(string npcID)
		{
			this.ReceiveClearResponses(npcID);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0008C900 File Offset: 0x0008AB00
		private void RpcReader___Server_ClearResponses_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ClearResponses_3615296227(npcID);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0008C934 File Offset: 0x0008AB34
		private void RpcWriter___Observers_ReceiveClearResponses_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0008C9EC File Offset: 0x0008ABEC
		private void RpcLogic___ReceiveClearResponses_3615296227(string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].ClearResponses(false);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0008CA30 File Offset: 0x0008AC30
		private void RpcReader___Observers_ReceiveClearResponses_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveClearResponses_3615296227(npcID);
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0008CA64 File Offset: 0x0008AC64
		private void RpcWriter___Server_ShowResponses_995803534(string npcID, List<Response> responses, float delay)
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
			writer.WriteString(npcID);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated(responses);
			writer.WriteSingle(delay, 0);
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0008CB2A File Offset: 0x0008AD2A
		public void RpcLogic___ShowResponses_995803534(string npcID, List<Response> responses, float delay)
		{
			this.ReceiveShowResponses(npcID, responses, delay);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0008CB38 File Offset: 0x0008AD38
		private void RpcReader___Server_ShowResponses_995803534(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			List<Response> responses = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds(PooledReader0);
			float delay = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0008CBA0 File Offset: 0x0008ADA0
		private void RpcWriter___Observers_ReceiveShowResponses_995803534(string npcID, List<Response> responses, float delay)
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
			writer.WriteString(npcID);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated(responses);
			writer.WriteSingle(delay, 0);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x0008CC78 File Offset: 0x0008AE78
		private void RpcLogic___ReceiveShowResponses_995803534(string npcID, List<Response> responses, float delay)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].ShowResponses(responses, delay, false);
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0008CCBC File Offset: 0x0008AEBC
		private void RpcReader___Observers_ReceiveShowResponses_995803534(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			List<Response> responses = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds(PooledReader0);
			float delay = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0008CD1E File Offset: 0x0008AF1E
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x040019E8 RID: 6632
		protected Dictionary<NPC, MSGConversation> ConversationMap = new Dictionary<NPC, MSGConversation>();

		// Token: 0x040019E9 RID: 6633
		private bool dll_Excuted;

		// Token: 0x040019EA RID: 6634
		private bool dll_Excuted;
	}
}
