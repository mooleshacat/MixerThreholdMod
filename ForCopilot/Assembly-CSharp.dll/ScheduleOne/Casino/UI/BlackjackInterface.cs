using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x020007B1 RID: 1969
	public class BlackjackInterface : Singleton<BlackjackInterface>
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x000DFA26 File Offset: 0x000DDC26
		// (set) Token: 0x06003580 RID: 13696 RVA: 0x000DFA2E File Offset: 0x000DDC2E
		public BlackjackGameController CurrentGame { get; private set; }

		// Token: 0x06003581 RID: 13697 RVA: 0x000DFA38 File Offset: 0x000DDC38
		protected override void Awake()
		{
			base.Awake();
			this.BetSlider.onValueChanged.AddListener(new UnityAction<float>(this.BetSliderChanged));
			this.ReadyButton.onClick.AddListener(new UnityAction(this.ReadyButtonClicked));
			this.HitButton.onClick.AddListener(new UnityAction(this.HitClicked));
			this.StandButton.onClick.AddListener(new UnityAction(this.StandClicked));
			this.InputContainerCanvasGroup.alpha = 0f;
			this.InputContainerCanvasGroup.interactable = false;
			this.ScoresContainerCanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000DFAF4 File Offset: 0x000DDCF4
		private void FixedUpdate()
		{
			if (this.CurrentGame == null)
			{
				return;
			}
			bool data = this.CurrentGame.LocalPlayerData.GetData<bool>("Ready");
			this.BetSlider.interactable = (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers && !data);
			if (data)
			{
				this.BetTitleLabel.text = "Waiting for other players...";
			}
			else
			{
				this.BetTitleLabel.text = "Place your bet and press 'ready'";
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers)
			{
				this.BetContainer.gameObject.SetActive(true);
				this.RefreshReadyButton();
			}
			else
			{
				this.BetContainer.gameObject.SetActive(false);
			}
			this.PlayerScoreLabel.text = this.CurrentGame.LocalPlayerScore.ToString();
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.DealerTurn || this.CurrentGame.CurrentStage == BlackjackGameController.EStage.Ending)
			{
				this.DealerScoreLabel.text = this.CurrentGame.DealerScore.ToString();
			}
			else
			{
				this.DealerScoreLabel.text = this.CurrentGame.DealerScore.ToString() + "+?";
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.PlayerTurn && this.CurrentGame.PlayerTurn != null)
			{
				if (this.CurrentGame.PlayerTurn.IsLocalPlayer)
				{
					this.WaitingLabel.text = "Your turn!";
				}
				else
				{
					this.WaitingLabel.text = "Waiting for " + this.CurrentGame.PlayerTurn.PlayerName + "...";
				}
				this.WaitingContainer.gameObject.SetActive(true);
				return;
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.DealerTurn)
			{
				this.WaitingLabel.text = "Dealer's turn...";
				this.WaitingContainer.gameObject.SetActive(true);
				return;
			}
			this.WaitingContainer.gameObject.SetActive(false);
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000DFCE8 File Offset: 0x000DDEE8
		public void Open(BlackjackGameController game)
		{
			this.CurrentGame = game;
			BlackjackGameController currentGame = this.CurrentGame;
			currentGame.onLocalPlayerBetChange = (Action)Delegate.Combine(currentGame.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
			BlackjackGameController currentGame2 = this.CurrentGame;
			currentGame2.onLocalPlayerExitRound = (Action)Delegate.Combine(currentGame2.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			BlackjackGameController currentGame3 = this.CurrentGame;
			currentGame3.onInitialCardsDealt = (Action)Delegate.Combine(currentGame3.onInitialCardsDealt, new Action(this.ShowScores));
			BlackjackGameController currentGame4 = this.CurrentGame;
			currentGame4.onLocalPlayerReadyForInput = (Action)Delegate.Combine(currentGame4.onLocalPlayerReadyForInput, new Action(this.LocalPlayerReadyForInput));
			BlackjackGameController currentGame5 = this.CurrentGame;
			currentGame5.onLocalPlayerBust = (Action)Delegate.Combine(currentGame5.onLocalPlayerBust, new Action(this.OnLocalPlayerBust));
			BlackjackGameController currentGame6 = this.CurrentGame;
			currentGame6.onLocalPlayerRoundCompleted = (Action<BlackjackGameController.EPayoutType>)Delegate.Combine(currentGame6.onLocalPlayerRoundCompleted, new Action<BlackjackGameController.EPayoutType>(this.OnLocalPlayerRoundCompleted));
			this.PlayerDisplay.Bind(game.Players);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.BetSlider.SetValueWithoutNotify(0f);
			game.SetLocalPlayerBet(10f);
			this.RefreshDisplayedBet();
			this.RefreshDisplayedBet();
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000DFE3C File Offset: 0x000DE03C
		public void Close()
		{
			if (this.CurrentGame != null)
			{
				BlackjackGameController currentGame = this.CurrentGame;
				currentGame.onLocalPlayerBetChange = (Action)Delegate.Remove(currentGame.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
				BlackjackGameController currentGame2 = this.CurrentGame;
				currentGame2.onLocalPlayerExitRound = (Action)Delegate.Remove(currentGame2.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
				BlackjackGameController currentGame3 = this.CurrentGame;
				currentGame3.onInitialCardsDealt = (Action)Delegate.Remove(currentGame3.onInitialCardsDealt, new Action(this.ShowScores));
				BlackjackGameController currentGame4 = this.CurrentGame;
				currentGame4.onLocalPlayerReadyForInput = (Action)Delegate.Remove(currentGame4.onLocalPlayerReadyForInput, new Action(this.LocalPlayerReadyForInput));
				BlackjackGameController currentGame5 = this.CurrentGame;
				currentGame5.onLocalPlayerBust = (Action)Delegate.Remove(currentGame5.onLocalPlayerBust, new Action(this.OnLocalPlayerBust));
				BlackjackGameController currentGame6 = this.CurrentGame;
				currentGame6.onLocalPlayerRoundCompleted = (Action<BlackjackGameController.EPayoutType>)Delegate.Remove(currentGame6.onLocalPlayerRoundCompleted, new Action<BlackjackGameController.EPayoutType>(this.OnLocalPlayerRoundCompleted));
			}
			this.CurrentGame = null;
			this.PlayerDisplay.Unbind();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000DFF6C File Offset: 0x000DE16C
		private void BetSliderChanged(float newValue)
		{
			this.CurrentGame.SetLocalPlayerBet(this.GetBetFromSliderValue(newValue));
			this.RefreshDisplayedBet();
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000DFF86 File Offset: 0x000DE186
		private float GetBetFromSliderValue(float sliderVal)
		{
			return Mathf.Lerp(10f, 1000f, Mathf.Pow(sliderVal, 2f));
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000DFFA4 File Offset: 0x000DE1A4
		private void RefreshDisplayedBet()
		{
			this.BetAmount.text = MoneyManager.FormatAmount(this.CurrentGame.LocalPlayerBet, false, false);
			this.BetSlider.SetValueWithoutNotify(Mathf.Sqrt(Mathf.InverseLerp(10f, 1000f, this.CurrentGame.LocalPlayerBet)));
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000DFFF8 File Offset: 0x000DE1F8
		private void RefreshReadyButton()
		{
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.CurrentGame.LocalPlayerBet)
			{
				this.ReadyButton.interactable = true;
				this.BetAmount.color = new Color32(84, 231, 23, byte.MaxValue);
			}
			else
			{
				this.ReadyButton.interactable = false;
				this.BetAmount.color = new Color32(231, 52, 23, byte.MaxValue);
			}
			if (this.CurrentGame.LocalPlayerData.GetData<bool>("Ready"))
			{
				this.ReadyLabel.text = "Cancel";
				return;
			}
			this.ReadyLabel.text = "Ready";
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000E00B4 File Offset: 0x000DE2B4
		private void LocalPlayerReadyForInput()
		{
			this.SelectionIndicator.gameObject.SetActive(false);
			this.InputContainerCanvasGroup.interactable = true;
			this.InputContainerAnimation.Play(this.InputContainerFadeIn.name);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000E00EA File Offset: 0x000DE2EA
		private void ShowScores()
		{
			this.ScoresContainerAnimation.Play(this.InputContainerFadeIn.name);
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000E0103 File Offset: 0x000DE303
		private void HideScores()
		{
			this.ScoresContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000E011C File Offset: 0x000DE31C
		private void HitClicked()
		{
			this.SelectionIndicator.transform.position = this.HitButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.LocalPlayerData.SetData<float>("Action", 1f, true);
			this.InputContainerCanvasGroup.interactable = false;
			this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000E0198 File Offset: 0x000DE398
		private void StandClicked()
		{
			this.SelectionIndicator.transform.position = this.StandButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.LocalPlayerData.SetData<float>("Action", 2f, true);
			this.InputContainerCanvasGroup.interactable = false;
			this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000E0214 File Offset: 0x000DE414
		private void LocalPlayerExitRound()
		{
			this.HideScores();
			if (this.InputContainerCanvasGroup.alpha > 0f)
			{
				this.InputContainerCanvasGroup.interactable = false;
				this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000E0251 File Offset: 0x000DE451
		private void ReadyButtonClicked()
		{
			this.CurrentGame.ToggleLocalPlayerReady();
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000E025E File Offset: 0x000DE45E
		private void OnLocalPlayerBust()
		{
			if (this.onBust != null)
			{
				this.onBust.Invoke();
			}
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000E0274 File Offset: 0x000DE474
		private void OnLocalPlayerRoundCompleted(BlackjackGameController.EPayoutType payout)
		{
			float payout2 = this.CurrentGame.GetPayout(this.CurrentGame.LocalPlayerBet, payout);
			this.PayoutLabel.text = MoneyManager.FormatAmount(payout2, false, false);
			switch (payout)
			{
			case BlackjackGameController.EPayoutType.None:
				if (!this.CurrentGame.IsLocalPlayerBust && this.onLose != null)
				{
					this.onLose.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Blackjack:
				this.PositiveOutcomeLabel.text = "Blackjack!";
				if (this.onBlackjack != null)
				{
					this.onBlackjack.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Win:
				this.PositiveOutcomeLabel.text = "Win!";
				if (this.onWin != null)
				{
					this.onWin.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Push:
				if (this.onPush != null)
				{
					this.onPush.Invoke();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x040025D4 RID: 9684
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040025D5 RID: 9685
		public CasinoGamePlayerDisplay PlayerDisplay;

		// Token: 0x040025D6 RID: 9686
		public RectTransform BetContainer;

		// Token: 0x040025D7 RID: 9687
		public TextMeshProUGUI BetTitleLabel;

		// Token: 0x040025D8 RID: 9688
		public Slider BetSlider;

		// Token: 0x040025D9 RID: 9689
		public TextMeshProUGUI BetAmount;

		// Token: 0x040025DA RID: 9690
		public Button ReadyButton;

		// Token: 0x040025DB RID: 9691
		public TextMeshProUGUI ReadyLabel;

		// Token: 0x040025DC RID: 9692
		public RectTransform WaitingContainer;

		// Token: 0x040025DD RID: 9693
		public TextMeshProUGUI WaitingLabel;

		// Token: 0x040025DE RID: 9694
		public TextMeshProUGUI DealerScoreLabel;

		// Token: 0x040025DF RID: 9695
		public TextMeshProUGUI PlayerScoreLabel;

		// Token: 0x040025E0 RID: 9696
		public Button HitButton;

		// Token: 0x040025E1 RID: 9697
		public Button StandButton;

		// Token: 0x040025E2 RID: 9698
		public Animation InputContainerAnimation;

		// Token: 0x040025E3 RID: 9699
		public CanvasGroup InputContainerCanvasGroup;

		// Token: 0x040025E4 RID: 9700
		public AnimationClip InputContainerFadeIn;

		// Token: 0x040025E5 RID: 9701
		public AnimationClip InputContainerFadeOut;

		// Token: 0x040025E6 RID: 9702
		public RectTransform SelectionIndicator;

		// Token: 0x040025E7 RID: 9703
		public Animation ScoresContainerAnimation;

		// Token: 0x040025E8 RID: 9704
		public CanvasGroup ScoresContainerCanvasGroup;

		// Token: 0x040025E9 RID: 9705
		public TextMeshProUGUI PositiveOutcomeLabel;

		// Token: 0x040025EA RID: 9706
		public TextMeshProUGUI PayoutLabel;

		// Token: 0x040025EB RID: 9707
		public UnityEvent onBust;

		// Token: 0x040025EC RID: 9708
		public UnityEvent onBlackjack;

		// Token: 0x040025ED RID: 9709
		public UnityEvent onWin;

		// Token: 0x040025EE RID: 9710
		public UnityEvent onLose;

		// Token: 0x040025EF RID: 9711
		public UnityEvent onPush;
	}
}
