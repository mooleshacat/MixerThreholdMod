using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Casino.UI;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x0200078C RID: 1932
	public class BlackjackGameController : CasinoGameController
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x000D816E File Offset: 0x000D636E
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x000D8176 File Offset: 0x000D6376
		public BlackjackGameController.EStage CurrentStage { get; private set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000D817F File Offset: 0x000D637F
		// (set) Token: 0x060033EE RID: 13294 RVA: 0x000D8187 File Offset: 0x000D6387
		public Player PlayerTurn { get; private set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x000D8190 File Offset: 0x000D6390
		// (set) Token: 0x060033F0 RID: 13296 RVA: 0x000D8198 File Offset: 0x000D6398
		public float LocalPlayerBet { get; private set; } = 10f;

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x000D81A1 File Offset: 0x000D63A1
		// (set) Token: 0x060033F2 RID: 13298 RVA: 0x000D81A9 File Offset: 0x000D63A9
		public int DealerScore { get; private set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000D81B2 File Offset: 0x000D63B2
		// (set) Token: 0x060033F4 RID: 13300 RVA: 0x000D81BA File Offset: 0x000D63BA
		public int LocalPlayerScore { get; private set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000D81C3 File Offset: 0x000D63C3
		// (set) Token: 0x060033F6 RID: 13302 RVA: 0x000D81CB File Offset: 0x000D63CB
		public bool IsLocalPlayerBlackjack { get; private set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060033F7 RID: 13303 RVA: 0x000D81D4 File Offset: 0x000D63D4
		// (set) Token: 0x060033F8 RID: 13304 RVA: 0x000D81DC File Offset: 0x000D63DC
		public bool IsLocalPlayerBust { get; private set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060033F9 RID: 13305 RVA: 0x000D81E5 File Offset: 0x000D63E5
		public bool IsLocalPlayerInCurrentRound
		{
			get
			{
				return this.playersInCurrentRound.Contains(Player.Local);
			}
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000D81F7 File Offset: 0x000D63F7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.BlackjackGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000D820C File Offset: 0x000D640C
		protected override void Open()
		{
			base.Open();
			Singleton<BlackjackInterface>.Instance.Open(this);
			this.localFocusCameraTransform = this.FocusedCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
			this.localFinalCameraTransform = this.FinalCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000D8264 File Offset: 0x000D6464
		protected override void Close()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType.None, 0f);
			}
			Singleton<BlackjackInterface>.Instance.Close();
			base.Close();
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000D828A File Offset: 0x000D648A
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
				this.RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType.None, 0f);
			}
			base.Exit(action);
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000D82CC File Offset: 0x000D64CC
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers && this.AreAllPlayersReady())
			{
				for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
				{
					this.AddPlayerToCurrentRound(this.Players.GetPlayer(i).NetworkObject);
				}
				this.StartGame();
			}
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D832C File Offset: 0x000D652C
		private List<Player> GetClockwisePlayers()
		{
			List<Player> list = new List<Player>();
			Player player = this.Players.GetPlayer(3);
			Player player2 = this.Players.GetPlayer(1);
			Player player3 = this.Players.GetPlayer(0);
			Player player4 = this.Players.GetPlayer(2);
			if (player != null)
			{
				list.Add(player);
			}
			if (player2 != null)
			{
				list.Add(player2);
			}
			if (player3 != null)
			{
				list.Add(player3);
			}
			if (player4 != null)
			{
				list.Add(player4);
			}
			return list;
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000D83B8 File Offset: 0x000D65B8
		[ObserversRpc(RunLocally = true)]
		private void StartGame()
		{
			this.RpcWriter___Observers_StartGame_2166136261();
			this.RpcLogic___StartGame_2166136261();
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000D83D4 File Offset: 0x000D65D4
		[ObserversRpc(RunLocally = true)]
		private void NotifyPlayerScore(NetworkObject player, int score, bool blackjack)
		{
			this.RpcWriter___Observers_NotifyPlayerScore_2864061566(player, score, blackjack);
			this.RpcLogic___NotifyPlayerScore_2864061566(player, score, blackjack);
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000D8405 File Offset: 0x000D6605
		private Transform[] GetPlayerCardPositions(int playerIndex)
		{
			switch (playerIndex)
			{
			case 0:
				return this.Player1CardPositions;
			case 1:
				return this.Player2CardPositions;
			case 2:
				return this.Player3CardPositions;
			case 3:
				return this.Player4CardPositions;
			default:
				return null;
			}
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000D843C File Offset: 0x000D663C
		[ObserversRpc(RunLocally = true)]
		private void SetRoundEnded(bool ended)
		{
			this.RpcWriter___Observers_SetRoundEnded_1140765316(ended);
			this.RpcLogic___SetRoundEnded_1140765316(ended);
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000D8452 File Offset: 0x000D6652
		private void AddCardToPlayerHand(int playerIndex, PlayingCard card)
		{
			this.AddCardToPlayerHand(playerIndex, card.CardID);
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000D8464 File Offset: 0x000D6664
		[ObserversRpc(RunLocally = true)]
		private void AddCardToPlayerHand(int playerindex, string cardID)
		{
			this.RpcWriter___Observers_AddCardToPlayerHand_2801973956(playerindex, cardID);
			this.RpcLogic___AddCardToPlayerHand_2801973956(playerindex, cardID);
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000D8490 File Offset: 0x000D6690
		[ObserversRpc(RunLocally = true)]
		private void AddCardToDealerHand(string cardID)
		{
			this.RpcWriter___Observers_AddCardToDealerHand_3615296227(cardID);
			this.RpcLogic___AddCardToDealerHand_3615296227(cardID);
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000D84B1 File Offset: 0x000D66B1
		private List<PlayingCard> GetPlayerCards(int playerIndex)
		{
			switch (playerIndex)
			{
			case 0:
				return this.player1Hand;
			case 1:
				return this.player2Hand;
			case 2:
				return this.player3Hand;
			case 3:
				return this.player4Hand;
			default:
				return null;
			}
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000D84E8 File Offset: 0x000D66E8
		private int GetHandScore(List<PlayingCard> cards, bool countFaceDown = true)
		{
			int num = 0;
			foreach (PlayingCard playingCard in cards)
			{
				if (countFaceDown || playingCard.IsFaceUp)
				{
					num += this.GetCardValue(playingCard, true);
				}
			}
			if (num > 21)
			{
				using (List<PlayingCard>.Enumerator enumerator = cards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Value == PlayingCard.ECardValue.Ace)
						{
							num -= 10;
						}
						if (num <= 21)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x000D8598 File Offset: 0x000D6798
		private int GetCardValue(PlayingCard card, bool aceAsEleven = true)
		{
			if (card.Value == PlayingCard.ECardValue.Ace)
			{
				if (!aceAsEleven)
				{
					return 1;
				}
				return 11;
			}
			else
			{
				if (card.Value == PlayingCard.ECardValue.Jack || card.Value == PlayingCard.ECardValue.Queen || card.Value == PlayingCard.ECardValue.King)
				{
					return 10;
				}
				return (int)card.Value;
			}
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000D85D4 File Offset: 0x000D67D4
		private PlayingCard DrawCard()
		{
			PlayingCard playingCard = this.playStack[0];
			this.playStack.RemoveAt(0);
			PlayingCard.CardData cardData = this.cardValuesInDeck[UnityEngine.Random.Range(0, this.cardValuesInDeck.Count)];
			this.cardValuesInDeck.Remove(cardData);
			this.drawnCardsValues.Add(cardData);
			playingCard.SetCard(cardData.Suit, cardData.Value, true);
			return playingCard;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000D8644 File Offset: 0x000D6844
		private void ResetCards()
		{
			if (InstanceFinder.IsServer)
			{
				for (int i = 0; i < this.Cards.Length; i++)
				{
					this.Cards[i].SetFaceUp(false, true);
					this.Cards[i].GlideTo(this.DefaultCardPositions[i].position, this.DefaultCardPositions[i].rotation, 0.5f, true);
				}
			}
			this.cardValuesInDeck = new List<PlayingCard.CardData>();
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 13; k++)
				{
					PlayingCard.CardData item = default(PlayingCard.CardData);
					item.Suit = (PlayingCard.ECardSuit)j;
					item.Value = k + PlayingCard.ECardValue.Ace;
					this.cardValuesInDeck.Add(item);
				}
			}
			this.playStack = new List<PlayingCard>();
			this.playStack.AddRange(this.Cards);
			this.player1Hand.Clear();
			this.player2Hand.Clear();
			this.player3Hand.Clear();
			this.player4Hand.Clear();
			this.dealerHand.Clear();
			this.drawnCardsValues.Clear();
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000D874F File Offset: 0x000D694F
		[ObserversRpc(RunLocally = true)]
		private void EndGame()
		{
			this.RpcWriter___Observers_EndGame_2166136261();
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000D8760 File Offset: 0x000D6960
		public void RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType payout, float cameraDelay = 0f)
		{
			BlackjackGameController.<>c__DisplayClass83_0 CS$<>8__locals1 = new BlackjackGameController.<>c__DisplayClass83_0();
			CS$<>8__locals1.cameraDelay = cameraDelay;
			CS$<>8__locals1.<>4__this = this;
			this.RequestRemovePlayerFromCurrentRound(Player.Local.NetworkObject);
			this.Players.SetPlayerScore(Player.Local, 0);
			float payout2 = this.GetPayout(this.LocalPlayerBet, payout);
			if (payout2 > 0f)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(payout2, true, false);
			}
			if (this.onLocalPlayerRoundCompleted != null)
			{
				this.onLocalPlayerRoundCompleted(payout);
			}
			if (this.onLocalPlayerExitRound != null)
			{
				this.onLocalPlayerExitRound();
			}
			base.StartCoroutine(CS$<>8__locals1.<RemoveLocalPlayerFromGame>g__Wait|0());
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000D87F9 File Offset: 0x000D69F9
		public float GetPayout(float bet, BlackjackGameController.EPayoutType payout)
		{
			switch (payout)
			{
			case BlackjackGameController.EPayoutType.Blackjack:
				return bet * 2.5f;
			case BlackjackGameController.EPayoutType.Win:
				return bet * 2f;
			case BlackjackGameController.EPayoutType.Push:
				return bet;
			default:
				return 0f;
			}
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x000D8828 File Offset: 0x000D6A28
		private bool IsCurrentRoundEmpty()
		{
			return this.playersInCurrentRound.Count == 0;
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000D8838 File Offset: 0x000D6A38
		[ObserversRpc(RunLocally = true)]
		private void AddPlayerToCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(player);
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000D8859 File Offset: 0x000D6A59
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestRemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000D8870 File Offset: 0x000D6A70
		[ObserversRpc(RunLocally = true)]
		private void RemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000D8891 File Offset: 0x000D6A91
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

		// Token: 0x06003414 RID: 13332 RVA: 0x000D88B6 File Offset: 0x000D6AB6
		public bool AreAllPlayersReady()
		{
			return this.Players.CurrentPlayerCount != 0 && this.GetPlayersReadyCount() == this.Players.CurrentPlayerCount;
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000D88DC File Offset: 0x000D6ADC
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

		// Token: 0x06003416 RID: 13334 RVA: 0x000D8934 File Offset: 0x000D6B34
		public void ToggleLocalPlayerReady()
		{
			bool flag = base.LocalPlayerData.GetData<bool>("Ready");
			flag = !flag;
			base.LocalPlayerData.SetData<bool>("Ready", flag, true);
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000D89F0 File Offset: 0x000D6BF0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartGame_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_NotifyPlayerScore_2864061566));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_SetRoundEnded_1140765316));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_AddCardToPlayerHand_2801973956));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_AddCardToDealerHand_3615296227));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_EndGame_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerToCurrentRound_3323014238));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238));
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000D8AE3 File Offset: 0x000D6CE3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000D8AFC File Offset: 0x000D6CFC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000D8B0C File Offset: 0x000D6D0C
		private void RpcWriter___Observers_StartGame_2166136261()
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
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000D8BB8 File Offset: 0x000D6DB8
		private void RpcLogic___StartGame_2166136261()
		{
			BlackjackGameController.<>c__DisplayClass70_0 CS$<>8__locals1 = new BlackjackGameController.<>c__DisplayClass70_0();
			CS$<>8__locals1.<>4__this = this;
			this.ResetCards();
			this.CurrentStage = BlackjackGameController.EStage.Dealing;
			this.PlayerTurn = null;
			this.IsLocalPlayerBlackjack = false;
			this.IsLocalPlayerBust = false;
			if (InstanceFinder.IsServer)
			{
				this.SetRoundEnded(false);
			}
			if (this.IsLocalPlayerInCurrentRound)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.LocalPlayerBet, true, false);
				this.Players.SetPlayerScore(Player.Local, Mathf.RoundToInt(this.LocalPlayerBet));
				base.LocalPlayerData.SetData<bool>("Ready", false, true);
			}
			CS$<>8__locals1.clockwisePlayers = this.GetClockwisePlayers();
			if (this.gameRoutine != null)
			{
				Console.LogWarning("Game routine already running, stopping...", null);
				base.StopCoroutine(this.gameRoutine);
			}
			this.gameRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartGame>g__GameRoutine|0());
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000D8C8C File Offset: 0x000D6E8C
		private void RpcReader___Observers_StartGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartGame_2166136261();
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000D8CB8 File Offset: 0x000D6EB8
		private void RpcWriter___Observers_NotifyPlayerScore_2864061566(NetworkObject player, int score, bool blackjack)
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
			writer.WriteInt32(score, 1);
			writer.WriteBoolean(blackjack);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000D8D90 File Offset: 0x000D6F90
		private void RpcLogic___NotifyPlayerScore_2864061566(NetworkObject player, int score, bool blackjack)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (component.IsLocalPlayer)
			{
				this.LocalPlayerScore = score;
				this.IsLocalPlayerBlackjack = blackjack;
				if (score > 21)
				{
					this.IsLocalPlayerBust = true;
				}
			}
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x000D8DD0 File Offset: 0x000D6FD0
		private void RpcReader___Observers_NotifyPlayerScore_2864061566(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(1);
			bool blackjack = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___NotifyPlayerScore_2864061566(player, score, blackjack);
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000D8E34 File Offset: 0x000D7034
		private void RpcWriter___Observers_SetRoundEnded_1140765316(bool ended)
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
			writer.WriteBoolean(ended);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000D8EEA File Offset: 0x000D70EA
		private void RpcLogic___SetRoundEnded_1140765316(bool ended)
		{
			this.roundEnded = ended;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000D8EF4 File Offset: 0x000D70F4
		private void RpcReader___Observers_SetRoundEnded_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool ended = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetRoundEnded_1140765316(ended);
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000D8F30 File Offset: 0x000D7130
		private void RpcWriter___Observers_AddCardToPlayerHand_2801973956(int playerindex, string cardID)
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
			writer.WriteInt32(playerindex, 1);
			writer.WriteString(cardID);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000D8FF8 File Offset: 0x000D71F8
		private void RpcLogic___AddCardToPlayerHand_2801973956(int playerindex, string cardID)
		{
			PlayingCard playingCard = this.Cards.FirstOrDefault((PlayingCard x) => x.CardID == cardID);
			if (playingCard == null)
			{
				return;
			}
			switch (playerindex)
			{
			case 0:
				if (!this.player1Hand.Contains(playingCard))
				{
					this.player1Hand.Add(playingCard);
					return;
				}
				break;
			case 1:
				if (!this.player2Hand.Contains(playingCard))
				{
					this.player2Hand.Add(playingCard);
					return;
				}
				break;
			case 2:
				if (!this.player3Hand.Contains(playingCard))
				{
					this.player3Hand.Add(playingCard);
					return;
				}
				break;
			case 3:
				if (!this.player4Hand.Contains(playingCard))
				{
					this.player4Hand.Add(playingCard);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000D90B8 File Offset: 0x000D72B8
		private void RpcReader___Observers_AddCardToPlayerHand_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int playerindex = PooledReader0.ReadInt32(1);
			string cardID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCardToPlayerHand_2801973956(playerindex, cardID);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000D910C File Offset: 0x000D730C
		private void RpcWriter___Observers_AddCardToDealerHand_3615296227(string cardID)
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
			writer.WriteString(cardID);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000D91C4 File Offset: 0x000D73C4
		private void RpcLogic___AddCardToDealerHand_3615296227(string cardID)
		{
			PlayingCard playingCard = this.Cards.FirstOrDefault((PlayingCard x) => x.CardID == cardID);
			if (playingCard == null)
			{
				return;
			}
			if (!this.dealerHand.Contains(playingCard))
			{
				this.dealerHand.Add(playingCard);
			}
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000D921C File Offset: 0x000D741C
		private void RpcReader___Observers_AddCardToDealerHand_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string cardID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCardToDealerHand_3615296227(cardID);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000D9258 File Offset: 0x000D7458
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
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000D9301 File Offset: 0x000D7501
		private void RpcLogic___EndGame_2166136261()
		{
			this.PlayerTurn = null;
			this.CurrentStage = BlackjackGameController.EStage.WaitingForPlayers;
			this.ResetCards();
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000D9318 File Offset: 0x000D7518
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

		// Token: 0x0600342D RID: 13357 RVA: 0x000D9344 File Offset: 0x000D7544
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000D93FC File Offset: 0x000D75FC
		private void RpcLogic___AddPlayerToCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			Console.Log("Adding player to current round: " + component.PlayerName, null);
			if (!this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Add(component);
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000D944C File Offset: 0x000D764C
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

		// Token: 0x06003430 RID: 13360 RVA: 0x000D9488 File Offset: 0x000D7688
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
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000D952F File Offset: 0x000D772F
		private void RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			this.RemovePlayerFromCurrentRound(player);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000D9538 File Offset: 0x000D7738
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

		// Token: 0x06003433 RID: 13363 RVA: 0x000D9578 File Offset: 0x000D7778
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
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000D9630 File Offset: 0x000D7830
		private void RpcLogic___RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			Console.Log("Removing player from current round: " + component.PlayerName, null);
			if (this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Remove(component);
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000D9680 File Offset: 0x000D7880
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

		// Token: 0x06003436 RID: 13366 RVA: 0x000D96BB File Offset: 0x000D78BB
		protected override void dll()
		{
			base.Awake();
			this.ResetCards();
		}

		// Token: 0x040024D3 RID: 9427
		public const int BET_MINIMUM = 10;

		// Token: 0x040024D4 RID: 9428
		public const int BET_MAXIMUM = 1000;

		// Token: 0x040024D5 RID: 9429
		public const float PAYOUT_RATIO = 1f;

		// Token: 0x040024D6 RID: 9430
		public const float BLACKJACK_PAYOUT_RATIO = 1.5f;

		// Token: 0x040024DE RID: 9438
		[Header("References")]
		public PlayingCard[] Cards;

		// Token: 0x040024DF RID: 9439
		public Transform[] DefaultCardPositions;

		// Token: 0x040024E0 RID: 9440
		public Transform[] FocusedCameraTransforms;

		// Token: 0x040024E1 RID: 9441
		public Transform[] FinalCameraTransforms;

		// Token: 0x040024E2 RID: 9442
		public Transform[] Player1CardPositions;

		// Token: 0x040024E3 RID: 9443
		public Transform[] Player2CardPositions;

		// Token: 0x040024E4 RID: 9444
		public Transform[] Player3CardPositions;

		// Token: 0x040024E5 RID: 9445
		public Transform[] Player4CardPositions;

		// Token: 0x040024E6 RID: 9446
		public Transform[] DealerCardPositions;

		// Token: 0x040024E7 RID: 9447
		private List<Player> playersInCurrentRound = new List<Player>();

		// Token: 0x040024E8 RID: 9448
		private List<PlayingCard> playStack = new List<PlayingCard>();

		// Token: 0x040024E9 RID: 9449
		private List<PlayingCard> player1Hand = new List<PlayingCard>();

		// Token: 0x040024EA RID: 9450
		private List<PlayingCard> player2Hand = new List<PlayingCard>();

		// Token: 0x040024EB RID: 9451
		private List<PlayingCard> player3Hand = new List<PlayingCard>();

		// Token: 0x040024EC RID: 9452
		private List<PlayingCard> player4Hand = new List<PlayingCard>();

		// Token: 0x040024ED RID: 9453
		private List<PlayingCard> dealerHand = new List<PlayingCard>();

		// Token: 0x040024EE RID: 9454
		private List<PlayingCard.CardData> cardValuesInDeck = new List<PlayingCard.CardData>();

		// Token: 0x040024EF RID: 9455
		private List<PlayingCard.CardData> drawnCardsValues = new List<PlayingCard.CardData>();

		// Token: 0x040024F0 RID: 9456
		protected Transform localFocusCameraTransform;

		// Token: 0x040024F1 RID: 9457
		protected Transform localFinalCameraTransform;

		// Token: 0x040024F2 RID: 9458
		public Action onLocalPlayerBetChange;

		// Token: 0x040024F3 RID: 9459
		public Action onLocalPlayerExitRound;

		// Token: 0x040024F4 RID: 9460
		public Action onInitialCardsDealt;

		// Token: 0x040024F5 RID: 9461
		public Action onLocalPlayerReadyForInput;

		// Token: 0x040024F6 RID: 9462
		public Action onLocalPlayerBust;

		// Token: 0x040024F7 RID: 9463
		public Action<BlackjackGameController.EPayoutType> onLocalPlayerRoundCompleted;

		// Token: 0x040024F8 RID: 9464
		private bool roundEnded;

		// Token: 0x040024F9 RID: 9465
		private Coroutine gameRoutine;

		// Token: 0x040024FA RID: 9466
		private bool dll_Excuted;

		// Token: 0x040024FB RID: 9467
		private bool dll_Excuted;

		// Token: 0x0200078D RID: 1933
		public enum EStage
		{
			// Token: 0x040024FD RID: 9469
			WaitingForPlayers,
			// Token: 0x040024FE RID: 9470
			Dealing,
			// Token: 0x040024FF RID: 9471
			PlayerTurn,
			// Token: 0x04002500 RID: 9472
			DealerTurn,
			// Token: 0x04002501 RID: 9473
			Ending
		}

		// Token: 0x0200078E RID: 1934
		public enum EPayoutType
		{
			// Token: 0x04002503 RID: 9475
			None,
			// Token: 0x04002504 RID: 9476
			Blackjack,
			// Token: 0x04002505 RID: 9477
			Win,
			// Token: 0x04002506 RID: 9478
			Push
		}
	}
}
