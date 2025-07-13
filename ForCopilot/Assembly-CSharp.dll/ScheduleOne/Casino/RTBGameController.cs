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
using ScheduleOne.Casino.UI;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x020007A4 RID: 1956
	public class RTBGameController : CasinoGameController
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060034E7 RID: 13543 RVA: 0x000DD10A File Offset: 0x000DB30A
		// (set) Token: 0x060034E8 RID: 13544 RVA: 0x000DD112 File Offset: 0x000DB312
		public RTBGameController.EStage CurrentStage { get; private set; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060034E9 RID: 13545 RVA: 0x000DD11B File Offset: 0x000DB31B
		// (set) Token: 0x060034EA RID: 13546 RVA: 0x000DD123 File Offset: 0x000DB323
		public bool IsQuestionActive { get; private set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060034EB RID: 13547 RVA: 0x000DD12C File Offset: 0x000DB32C
		// (set) Token: 0x060034EC RID: 13548 RVA: 0x000DD134 File Offset: 0x000DB334
		public float LocalPlayerBet { get; private set; } = 10f;

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x000DD13D File Offset: 0x000DB33D
		// (set) Token: 0x060034EE RID: 13550 RVA: 0x000DD145 File Offset: 0x000DB345
		public float LocalPlayerBetMultiplier { get; private set; } = 1f;

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060034EF RID: 13551 RVA: 0x000DD14E File Offset: 0x000DB34E
		public float MultipliedLocalPlayerBet
		{
			get
			{
				return this.LocalPlayerBet * this.LocalPlayerBetMultiplier;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x000DD15D File Offset: 0x000DB35D
		// (set) Token: 0x060034F1 RID: 13553 RVA: 0x000DD165 File Offset: 0x000DB365
		public float RemainingAnswerTime { get; private set; } = 6f;

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x000DD16E File Offset: 0x000DB36E
		public bool IsLocalPlayerInCurrentRound
		{
			get
			{
				return this.playersInCurrentRound.Contains(Player.Local);
			}
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000DD180 File Offset: 0x000DB380
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.RTBGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000DD194 File Offset: 0x000DB394
		protected override void Open()
		{
			base.Open();
			Singleton<RTBInterface>.Instance.Open(this);
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000DD1A7 File Offset: 0x000DB3A7
		protected override void Close()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			Singleton<RTBInterface>.Instance.Close();
			base.Close();
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000DD1CD File Offset: 0x000DB3CD
		protected override void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!base.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.IsLocalPlayerInCurrentRound)
			{
				action.Used = true;
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			base.Exit(action);
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000DD20C File Offset: 0x000DB40C
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.CurrentStage == RTBGameController.EStage.WaitingForPlayers && this.AreAllPlayersReady())
			{
				for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
				{
					this.AddPlayerToCurrentRound(this.Players.GetPlayer(i).NetworkObject);
				}
				this.SetStage(RTBGameController.EStage.RedOrBlack);
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000DD26C File Offset: 0x000DB46C
		[ObserversRpc(RunLocally = true)]
		private void SetStage(RTBGameController.EStage stage)
		{
			this.RpcWriter___Observers_SetStage_2502303021(stage);
			this.RpcLogic___SetStage_2502303021(stage);
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000DD290 File Offset: 0x000DB490
		private void RunRound(RTBGameController.EStage stage)
		{
			RTBGameController.<>c__DisplayClass50_0 CS$<>8__locals1 = new RTBGameController.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stage = stage;
			this.SetBetMultiplier(RTBGameController.GetNetBetMultiplier(CS$<>8__locals1.stage - 1));
			base.StartCoroutine(CS$<>8__locals1.<RunRound>g__RunRound|0());
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000DD2D1 File Offset: 0x000DB4D1
		[ObserversRpc(RunLocally = true)]
		private void SetBetMultiplier(float multiplier)
		{
			this.RpcWriter___Observers_SetBetMultiplier_431000436(multiplier);
			this.RpcLogic___SetBetMultiplier_431000436(multiplier);
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000DD2E7 File Offset: 0x000DB4E7
		[ObserversRpc(RunLocally = true)]
		private void EndGame()
		{
			this.RpcWriter___Observers_EndGame_2166136261();
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000DD2F8 File Offset: 0x000DB4F8
		public void RemoveLocalPlayerFromGame(bool payout, float cameraDelay = 0f)
		{
			RTBGameController.<>c__DisplayClass53_0 CS$<>8__locals1 = new RTBGameController.<>c__DisplayClass53_0();
			CS$<>8__locals1.cameraDelay = cameraDelay;
			CS$<>8__locals1.<>4__this = this;
			this.RequestRemovePlayerFromCurrentRound(Player.Local.NetworkObject);
			this.Players.SetPlayerScore(Player.Local, 0);
			if (payout)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.LocalPlayerBet * this.LocalPlayerBetMultiplier, true, false);
			}
			if (this.onLocalPlayerExitRound != null)
			{
				this.onLocalPlayerExitRound();
			}
			base.StartCoroutine(CS$<>8__locals1.<RemoveLocalPlayerFromGame>g__Wait|0());
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000DD376 File Offset: 0x000DB576
		private bool IsCurrentRoundEmpty()
		{
			return this.playersInCurrentRound.Count == 0;
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000DD388 File Offset: 0x000DB588
		private float GetAnswerIndex(RTBGameController.EStage stage, PlayingCard.CardData card)
		{
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				if (card.Suit == PlayingCard.ECardSuit.Hearts || card.Suit == PlayingCard.ECardSuit.Diamonds)
				{
					return 1f;
				}
				return 2f;
			}
			else if (stage == RTBGameController.EStage.HigherOrLower)
			{
				PlayingCard.CardData card2 = this.drawnCards[this.drawnCards.Count - 2];
				if (this.GetCardNumberValue(card) >= this.GetCardNumberValue(card2))
				{
					return 1f;
				}
				return 2f;
			}
			else
			{
				if (stage != RTBGameController.EStage.InsideOrOutside)
				{
					if (stage == RTBGameController.EStage.Suit)
					{
						switch (card.Suit)
						{
						case PlayingCard.ECardSuit.Spades:
							return 4f;
						case PlayingCard.ECardSuit.Hearts:
							return 1f;
						case PlayingCard.ECardSuit.Diamonds:
							return 3f;
						case PlayingCard.ECardSuit.Clubs:
							return 2f;
						}
					}
					Console.LogError("GetAnswerIndex not implemented for stage " + stage.ToString(), null);
					return 0f;
				}
				PlayingCard.CardData card3 = this.drawnCards[this.drawnCards.Count - 2];
				PlayingCard.CardData card4 = this.drawnCards[this.drawnCards.Count - 3];
				int num = Mathf.Min(this.GetCardNumberValue(card3), this.GetCardNumberValue(card4));
				int num2 = Mathf.Max(this.GetCardNumberValue(card3), this.GetCardNumberValue(card4));
				int cardNumberValue = this.GetCardNumberValue(card);
				if (cardNumberValue >= num && cardNumberValue <= num2)
				{
					return 1f;
				}
				return 2f;
			}
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000DD4D0 File Offset: 0x000DB6D0
		[ObserversRpc(RunLocally = true)]
		private void NotifyAnswer(float answerIndex)
		{
			this.RpcWriter___Observers_NotifyAnswer_431000436(answerIndex);
			this.RpcLogic___NotifyAnswer_431000436(answerIndex);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000DD4F4 File Offset: 0x000DB6F4
		[ObserversRpc(RunLocally = true)]
		private void QuestionDone()
		{
			this.RpcWriter___Observers_QuestionDone_2166136261();
			this.RpcLogic___QuestionDone_2166136261();
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000DD510 File Offset: 0x000DB710
		private void GetQuestionsAndAnswers(RTBGameController.EStage stage, out string question, out string[] answers)
		{
			question = "";
			answers = new string[0];
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				question = "What will the next card be?";
				answers = new string[]
				{
					"Red",
					"Black"
				};
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				question = "Will the next card be higher or lower?";
				answers = new string[]
				{
					"Higher",
					"Lower"
				};
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				question = "Will the next card be inside or outside?";
				answers = new string[]
				{
					"Inside",
					"Outside"
				};
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				question = "What will the suit of the next card be?";
				answers = new string[]
				{
					"Hearts",
					"Clubs",
					"Diamonds",
					"Spades"
				};
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000DD5C8 File Offset: 0x000DB7C8
		private void ResetCards()
		{
			for (int i = 0; i < this.Cards.Length; i++)
			{
				this.Cards[i].SetFaceUp(false, true);
				this.Cards[i].GlideTo(this.CardDefaultPositions[i].position, this.CardDefaultPositions[i].rotation, 0.5f, true);
			}
			this.cardsInDeck = new List<PlayingCard.CardData>();
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 13; k++)
				{
					PlayingCard.CardData item = default(PlayingCard.CardData);
					item.Suit = (PlayingCard.ECardSuit)j;
					item.Value = k + PlayingCard.ECardValue.Ace;
					this.cardsInDeck.Add(item);
				}
			}
			this.drawnCards.Clear();
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000DD67C File Offset: 0x000DB87C
		[ObserversRpc(RunLocally = true)]
		private void AddPlayerToCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(player);
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000DD69D File Offset: 0x000DB89D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestRemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000DD6B4 File Offset: 0x000DB8B4
		[ObserversRpc(RunLocally = true)]
		private void RemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000DD6D8 File Offset: 0x000DB8D8
		private PlayingCard.CardData PullCardFromDeck()
		{
			PlayingCard.CardData cardData = this.cardsInDeck[UnityEngine.Random.Range(0, this.cardsInDeck.Count)];
			this.cardsInDeck.Remove(cardData);
			this.drawnCards.Add(cardData);
			return cardData;
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000DD71C File Offset: 0x000DB91C
		public void SetLocalPlayerBet(float bet)
		{
			if (!base.IsOpen)
			{
				return;
			}
			this.LocalPlayerBet = bet;
			if (this.onLocalPlayerBetChange != null)
			{
				this.onLocalPlayerBetChange();
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000DD741 File Offset: 0x000DB941
		public bool AreAllPlayersReady()
		{
			return this.Players.CurrentPlayerCount != 0 && this.GetPlayersReadyCount() == this.Players.CurrentPlayerCount;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000DD768 File Offset: 0x000DB968
		public int GetPlayersReadyCount()
		{
			int num = 0;
			for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
			{
				if (!(this.Players.GetPlayer(i) == null) && this.Players.GetPlayerData(i).GetData<bool>("Ready"))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000DD7BE File Offset: 0x000DB9BE
		public void SetLocalPlayerAnswer(float answer)
		{
			base.LocalPlayerData.SetData<float>("Answer", answer, true);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000DD7D4 File Offset: 0x000DB9D4
		public int GetAnsweredPlayersCount()
		{
			int num = 0;
			for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
			{
				if (!(this.Players.GetPlayer(i) == null) && this.playersInCurrentRound.Contains(this.Players.GetPlayer(i)) && this.Players.GetPlayerData(i).GetData<float>("Answer") > 0.1f)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000DD848 File Offset: 0x000DBA48
		public void ToggleLocalPlayerReady()
		{
			bool flag = base.LocalPlayerData.GetData<bool>("Ready");
			flag = !flag;
			base.LocalPlayerData.SetData<bool>("Ready", flag, true);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000DD87D File Offset: 0x000DBA7D
		private int GetCardNumberValue(PlayingCard.CardData card)
		{
			if (card.Value == PlayingCard.ECardValue.Ace)
			{
				return 14;
			}
			return (int)card.Value;
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000DD891 File Offset: 0x000DBA91
		public static float GetNetBetMultiplier(RTBGameController.EStage stage)
		{
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				return 2f;
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				return 3f;
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				return 4f;
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				return 20f;
			}
			return 1f;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000DD918 File Offset: 0x000DBB18
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetStage_2502303021));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetBetMultiplier_431000436));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndGame_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_NotifyAnswer_431000436));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_QuestionDone_2166136261));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerToCurrentRound_3323014238));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238));
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000DD9F4 File Offset: 0x000DBBF4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000DDA0D File Offset: 0x000DBC0D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000DDA1C File Offset: 0x000DBC1C
		private void RpcWriter___Observers_SetStage_2502303021(RTBGameController.EStage stage)
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
			writer.Write___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generated(stage);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000DDAD4 File Offset: 0x000DBCD4
		private void RpcLogic___SetStage_2502303021(RTBGameController.EStage stage)
		{
			this.CurrentStage = stage;
			if (!this.IsLocalPlayerInCurrentRound && !InstanceFinder.IsServer)
			{
				return;
			}
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				this.RunRound(RTBGameController.EStage.RedOrBlack);
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				this.RunRound(RTBGameController.EStage.HigherOrLower);
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				this.RunRound(RTBGameController.EStage.InsideOrOutside);
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				this.RunRound(RTBGameController.EStage.Suit);
			}
			if (this.onStageChange != null)
			{
				this.onStageChange(stage);
			}
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000DDB38 File Offset: 0x000DBD38
		private void RpcReader___Observers_SetStage_2502303021(PooledReader PooledReader0, Channel channel)
		{
			RTBGameController.EStage stage = GeneratedReaders___Internal.Read___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStage_2502303021(stage);
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000DDB74 File Offset: 0x000DBD74
		private void RpcWriter___Observers_SetBetMultiplier_431000436(float multiplier)
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
			writer.WriteSingle(multiplier, 0);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000DDC2F File Offset: 0x000DBE2F
		private void RpcLogic___SetBetMultiplier_431000436(float multiplier)
		{
			this.LocalPlayerBetMultiplier = multiplier;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000DDC38 File Offset: 0x000DBE38
		private void RpcReader___Observers_SetBetMultiplier_431000436(PooledReader PooledReader0, Channel channel)
		{
			float multiplier = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetBetMultiplier_431000436(multiplier);
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000DDC78 File Offset: 0x000DBE78
		private void RpcWriter___Observers_EndGame_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000DDD21 File Offset: 0x000DBF21
		private void RpcLogic___EndGame_2166136261()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			this.ResetCards();
			this.SetStage(RTBGameController.EStage.WaitingForPlayers);
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000DDD44 File Offset: 0x000DBF44
		private void RpcReader___Observers_EndGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000DDD70 File Offset: 0x000DBF70
		private void RpcWriter___Observers_NotifyAnswer_431000436(float answerIndex)
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
			writer.WriteSingle(answerIndex, 0);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000DDE2C File Offset: 0x000DC02C
		private void RpcLogic___NotifyAnswer_431000436(float answerIndex)
		{
			if (!this.IsLocalPlayerInCurrentRound)
			{
				return;
			}
			if (base.LocalPlayerData.GetData<float>("Answer") == answerIndex)
			{
				Console.Log("Correct answer!", null);
				this.Players.SetPlayerScore(Player.Local, Mathf.RoundToInt(this.MultipliedLocalPlayerBet));
				if (this.onLocalPlayerCorrect != null)
				{
					this.onLocalPlayerCorrect();
					return;
				}
			}
			else
			{
				Console.Log("Incorrect answer!", null);
				this.RemoveLocalPlayerFromGame(false, 2f);
				if (this.onLocalPlayerIncorrect != null)
				{
					this.onLocalPlayerIncorrect();
				}
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000DDEBC File Offset: 0x000DC0BC
		private void RpcReader___Observers_NotifyAnswer_431000436(PooledReader PooledReader0, Channel channel)
		{
			float answerIndex = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___NotifyAnswer_431000436(answerIndex);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x000DDEFC File Offset: 0x000DC0FC
		private void RpcWriter___Observers_QuestionDone_2166136261()
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
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x000DDFA8 File Offset: 0x000DC1A8
		private void RpcLogic___QuestionDone_2166136261()
		{
			if (!this.IsLocalPlayerInCurrentRound)
			{
				return;
			}
			if (!this.IsQuestionActive)
			{
				return;
			}
			if (base.LocalPlayerData.GetData<float>("Answer") == 0f)
			{
				this.SetLocalPlayerAnswer(1f);
			}
			this.IsQuestionActive = false;
			if (this.onQuestionDone != null)
			{
				this.onQuestionDone();
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000DE004 File Offset: 0x000DC204
		private void RpcReader___Observers_QuestionDone_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___QuestionDone_2166136261();
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000DE030 File Offset: 0x000DC230
		private void RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000DE0E8 File Offset: 0x000DC2E8
		private void RpcLogic___AddPlayerToCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Add(component);
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000DE120 File Offset: 0x000DC320
		private void RpcReader___Observers_AddPlayerToCurrentRound_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000DE15C File Offset: 0x000DC35C
		private void RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000DE203 File Offset: 0x000DC403
		private void RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			this.RemovePlayerFromCurrentRound(player);
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000DE20C File Offset: 0x000DC40C
		private void RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000DE24C File Offset: 0x000DC44C
		private void RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x000DE304 File Offset: 0x000DC504
		private void RpcLogic___RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			if (player == null)
			{
				return;
			}
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Remove(component);
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000DE348 File Offset: 0x000DC548
		private void RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000DE383 File Offset: 0x000DC583
		protected override void dll()
		{
			base.Awake();
			this.ResetCards();
		}

		// Token: 0x0400256F RID: 9583
		public const int BET_MINIMUM = 10;

		// Token: 0x04002570 RID: 9584
		public const int BET_MAXIMUM = 500;

		// Token: 0x04002571 RID: 9585
		public const float ANSWER_MAX_TIME = 6f;

		// Token: 0x04002572 RID: 9586
		[Header("References")]
		public Transform PlayCameraTransform;

		// Token: 0x04002573 RID: 9587
		public Transform FocusedCameraTransform;

		// Token: 0x04002574 RID: 9588
		public PlayingCard[] Cards;

		// Token: 0x04002575 RID: 9589
		public Transform[] CardDefaultPositions;

		// Token: 0x04002576 RID: 9590
		public Transform ActiveCardPosition;

		// Token: 0x04002577 RID: 9591
		public Transform[] DockedCardPositions;

		// Token: 0x04002579 RID: 9593
		public Action<RTBGameController.EStage> onStageChange;

		// Token: 0x0400257A RID: 9594
		public Action<string, string[]> onQuestionReady;

		// Token: 0x0400257B RID: 9595
		public Action onQuestionDone;

		// Token: 0x0400257C RID: 9596
		public Action onLocalPlayerCorrect;

		// Token: 0x0400257D RID: 9597
		public Action onLocalPlayerIncorrect;

		// Token: 0x0400257E RID: 9598
		public Action onLocalPlayerBetChange;

		// Token: 0x0400257F RID: 9599
		public Action onLocalPlayerExitRound;

		// Token: 0x04002584 RID: 9604
		private List<Player> playersInCurrentRound = new List<Player>();

		// Token: 0x04002585 RID: 9605
		private List<PlayingCard.CardData> cardsInDeck = new List<PlayingCard.CardData>();

		// Token: 0x04002586 RID: 9606
		private List<PlayingCard.CardData> drawnCards = new List<PlayingCard.CardData>();

		// Token: 0x04002587 RID: 9607
		private bool dll_Excuted;

		// Token: 0x04002588 RID: 9608
		private bool dll_Excuted;

		// Token: 0x020007A5 RID: 1957
		public enum EStage
		{
			// Token: 0x0400258A RID: 9610
			WaitingForPlayers,
			// Token: 0x0400258B RID: 9611
			RedOrBlack,
			// Token: 0x0400258C RID: 9612
			HigherOrLower,
			// Token: 0x0400258D RID: 9613
			InsideOrOutside,
			// Token: 0x0400258E RID: 9614
			Suit
		}
	}
}
