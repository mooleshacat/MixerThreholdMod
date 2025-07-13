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
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x02000796 RID: 1942
	public class CardController : NetworkBehaviour
	{
		// Token: 0x0600344E RID: 13390 RVA: 0x000DA564 File Offset: 0x000D8764
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CardController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000DA583 File Offset: 0x000D8783
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardValue(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.RpcWriter___Server_SendCardValue_3709737967(cardId, suit, value);
			this.RpcLogic___SendCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000DA5AC File Offset: 0x000D87AC
		[ObserversRpc(RunLocally = true)]
		private void SetCardValue(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.RpcWriter___Observers_SetCardValue_3709737967(cardId, suit, value);
			this.RpcLogic___SetCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000DA5DD File Offset: 0x000D87DD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardFaceUp(string cardId, bool faceUp)
		{
			this.RpcWriter___Server_SendCardFaceUp_310431262(cardId, faceUp);
			this.RpcLogic___SendCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000DA5FC File Offset: 0x000D87FC
		[ObserversRpc(RunLocally = true)]
		private void SetCardFaceUp(string cardId, bool faceUp)
		{
			this.RpcWriter___Observers_SetCardFaceUp_310431262(cardId, faceUp);
			this.RpcLogic___SetCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000DA625 File Offset: 0x000D8825
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardGlide(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.RpcWriter___Server_SendCardGlide_2833372058(cardId, position, rotation, glideTime);
			this.RpcLogic___SendCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000DA654 File Offset: 0x000D8854
		[ObserversRpc(RunLocally = true)]
		private void SetCardGlide(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.RpcWriter___Observers_SetCardGlide_2833372058(cardId, position, rotation, glideTime);
			this.RpcLogic___SetCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000DA68D File Offset: 0x000D888D
		private PlayingCard GetCard(string cardId)
		{
			return this.cardDictionary[cardId];
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x000DA6BC File Offset: 0x000D88BC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendCardValue_3709737967));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardValue_3709737967));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendCardFaceUp_310431262));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardFaceUp_310431262));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendCardGlide_2833372058));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardGlide_2833372058));
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x000DA764 File Offset: 0x000D8964
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x000DA777 File Offset: 0x000D8977
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x000DA788 File Offset: 0x000D8988
		private void RpcWriter___Server_SendCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
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
			writer.WriteString(cardId);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated(suit);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000DA849 File Offset: 0x000D8A49
		public void RpcLogic___SendCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.SetCardValue(cardId, suit, value);
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000DA854 File Offset: 0x000D8A54
		private void RpcReader___Server_SendCardValue_3709737967(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			PlayingCard.ECardSuit suit = GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds(PooledReader0);
			PlayingCard.ECardValue value = GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x000DA8B4 File Offset: 0x000D8AB4
		private void RpcWriter___Observers_SetCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
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
			writer.WriteString(cardId);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated(suit);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated(value);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000DA984 File Offset: 0x000D8B84
		private void RpcLogic___SetCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.SetCard(suit, value, false);
			}
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000DA9AC File Offset: 0x000D8BAC
		private void RpcReader___Observers_SetCardValue_3709737967(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			PlayingCard.ECardSuit suit = GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds(PooledReader0);
			PlayingCard.ECardValue value = GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x000DAA0C File Offset: 0x000D8C0C
		private void RpcWriter___Server_SendCardFaceUp_310431262(string cardId, bool faceUp)
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
			writer.WriteString(cardId);
			writer.WriteBoolean(faceUp);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000DAAC0 File Offset: 0x000D8CC0
		public void RpcLogic___SendCardFaceUp_310431262(string cardId, bool faceUp)
		{
			this.SetCardFaceUp(cardId, faceUp);
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000DAACC File Offset: 0x000D8CCC
		private void RpcReader___Server_SendCardFaceUp_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			bool faceUp = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000DAB1C File Offset: 0x000D8D1C
		private void RpcWriter___Observers_SetCardFaceUp_310431262(string cardId, bool faceUp)
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
			writer.WriteString(cardId);
			writer.WriteBoolean(faceUp);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000DABE0 File Offset: 0x000D8DE0
		private void RpcLogic___SetCardFaceUp_310431262(string cardId, bool faceUp)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.SetFaceUp(faceUp, false);
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000DAC08 File Offset: 0x000D8E08
		private void RpcReader___Observers_SetCardFaceUp_310431262(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			bool faceUp = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000DAC54 File Offset: 0x000D8E54
		private void RpcWriter___Server_SendCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
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
			writer.WriteString(cardId);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteSingle(glideTime, 0);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000DAD2C File Offset: 0x000D8F2C
		public void RpcLogic___SendCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.SetCardGlide(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x000DAD3C File Offset: 0x000D8F3C
		private void RpcReader___Server_SendCardGlide_2833372058(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			float glideTime = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x000DADB8 File Offset: 0x000D8FB8
		private void RpcWriter___Observers_SetCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
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
			writer.WriteString(cardId);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteSingle(glideTime, 0);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000DAEA0 File Offset: 0x000D90A0
		private void RpcLogic___SetCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.GlideTo(position, rotation, glideTime, false);
			}
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000DAECC File Offset: 0x000D90CC
		private void RpcReader___Observers_SetCardGlide_2833372058(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			float glideTime = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000DAF44 File Offset: 0x000D9144
		private void dll()
		{
			this.cards = new List<PlayingCard>(base.GetComponentsInChildren<PlayingCard>());
			foreach (PlayingCard playingCard in this.cards)
			{
				playingCard.SetCardController(this);
				if (this.cardDictionary.ContainsKey(playingCard.CardID))
				{
					Debug.LogError("Card ID " + playingCard.CardID + " already exists in the dictionary.");
				}
				else
				{
					this.cardDictionary.Add(playingCard.CardID, playingCard);
				}
			}
		}

		// Token: 0x0400251C RID: 9500
		private List<PlayingCard> cards = new List<PlayingCard>();

		// Token: 0x0400251D RID: 9501
		private Dictionary<string, PlayingCard> cardDictionary = new Dictionary<string, PlayingCard>();

		// Token: 0x0400251E RID: 9502
		private bool dll_Excuted;

		// Token: 0x0400251F RID: 9503
		private bool dll_Excuted;
	}
}
