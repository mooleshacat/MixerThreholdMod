using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x020007B3 RID: 1971
	public class RTBInterface : Singleton<RTBInterface>
	{
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x000E0557 File Offset: 0x000DE757
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x000E055F File Offset: 0x000DE75F
		public RTBGameController CurrentGame { get; private set; }

		// Token: 0x0600359A RID: 13722 RVA: 0x000E0568 File Offset: 0x000DE768
		protected override void Awake()
		{
			base.Awake();
			this.BetSlider.onValueChanged.AddListener(new UnityAction<float>(this.BetSliderChanged));
			this.ReadyButton.onClick.AddListener(new UnityAction(this.ReadyButtonClicked));
			for (int i = 0; i < this.AnswerButtons.Length; i++)
			{
				int index = i;
				this.AnswerButtons[i].onClick.AddListener(delegate()
				{
					this.AnswerButtonClicked(index);
				});
			}
			this.ForfeitButton.onClick.AddListener(new UnityAction(this.ForfeitClicked));
			this.QuestionCanvasGroup.alpha = 0f;
			this.QuestionCanvasGroup.interactable = false;
			this.Canvas.enabled = false;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000E063C File Offset: 0x000DE83C
		private void FixedUpdate()
		{
			if (this.CurrentGame == null)
			{
				return;
			}
			this.StatusLabel.text = this.GetStatusText();
			bool data = this.CurrentGame.LocalPlayerData.GetData<bool>("Ready");
			this.BetSlider.interactable = (this.CurrentGame.CurrentStage == RTBGameController.EStage.WaitingForPlayers && !data);
			if (data)
			{
				this.BetTitleLabel.text = "Waiting for other players...";
			}
			else
			{
				this.BetTitleLabel.text = "Place your bet and press 'ready'";
			}
			if (this.CurrentGame.CurrentStage == RTBGameController.EStage.WaitingForPlayers)
			{
				this.BetContainer.gameObject.SetActive(true);
				this.RefreshReadyButton();
				return;
			}
			this.BetContainer.gameObject.SetActive(false);
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000E06FC File Offset: 0x000DE8FC
		private string GetStatusText()
		{
			switch (this.CurrentGame.CurrentStage)
			{
			case RTBGameController.EStage.WaitingForPlayers:
				return string.Concat(new string[]
				{
					"Waiting for players... (",
					this.CurrentGame.GetPlayersReadyCount().ToString(),
					"/",
					this.CurrentGame.Players.CurrentPlayerCount.ToString(),
					")"
				});
			case RTBGameController.EStage.RedOrBlack:
				return "Round 1\nPredict if the next card will be red or black.\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.HigherOrLower:
				return "Round 2\nPredict if the next card will be higher or lower than the previous card.\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.InsideOrOutside:
				return "Round 3\nPredict if the next card will be inside or outside the previous two cards (Ace counts as 11).\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.Suit:
				return "Round 4\nPredict the suit of the next card.\nYou can also forfeit and cash out.";
			default:
				return "Unknown";
			}
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000E07A4 File Offset: 0x000DE9A4
		public void Open(RTBGameController game)
		{
			this.CurrentGame = game;
			RTBGameController currentGame = this.CurrentGame;
			currentGame.onQuestionReady = (Action<string, string[]>)Delegate.Combine(currentGame.onQuestionReady, new Action<string, string[]>(this.QuestionReady));
			RTBGameController currentGame2 = this.CurrentGame;
			currentGame2.onQuestionDone = (Action)Delegate.Combine(currentGame2.onQuestionDone, new Action(this.QuestionDone));
			RTBGameController currentGame3 = this.CurrentGame;
			currentGame3.onLocalPlayerCorrect = (Action)Delegate.Combine(currentGame3.onLocalPlayerCorrect, new Action(this.Correct));
			RTBGameController currentGame4 = this.CurrentGame;
			currentGame4.onLocalPlayerIncorrect = (Action)Delegate.Combine(currentGame4.onLocalPlayerIncorrect, new Action(this.Incorrect));
			RTBGameController currentGame5 = this.CurrentGame;
			currentGame5.onLocalPlayerBetChange = (Action)Delegate.Combine(currentGame5.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
			RTBGameController currentGame6 = this.CurrentGame;
			currentGame6.onLocalPlayerExitRound = (Action)Delegate.Combine(currentGame6.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			this.PlayerDisplay.Bind(game.Players);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.BetSlider.SetValueWithoutNotify(0f);
			game.SetLocalPlayerBet(10f);
			this.RefreshDisplayedBet();
			this.RefreshDisplayedBet();
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000E08F8 File Offset: 0x000DEAF8
		public void Close()
		{
			if (this.CurrentGame != null)
			{
				RTBGameController currentGame = this.CurrentGame;
				currentGame.onQuestionReady = (Action<string, string[]>)Delegate.Remove(currentGame.onQuestionReady, new Action<string, string[]>(this.QuestionReady));
				RTBGameController currentGame2 = this.CurrentGame;
				currentGame2.onQuestionDone = (Action)Delegate.Remove(currentGame2.onQuestionDone, new Action(this.QuestionDone));
				RTBGameController currentGame3 = this.CurrentGame;
				currentGame3.onLocalPlayerCorrect = (Action)Delegate.Remove(currentGame3.onLocalPlayerCorrect, new Action(this.Correct));
				RTBGameController currentGame4 = this.CurrentGame;
				currentGame4.onLocalPlayerIncorrect = (Action)Delegate.Remove(currentGame4.onLocalPlayerIncorrect, new Action(this.Incorrect));
				RTBGameController currentGame5 = this.CurrentGame;
				currentGame5.onLocalPlayerBetChange = (Action)Delegate.Remove(currentGame5.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
				RTBGameController currentGame6 = this.CurrentGame;
				currentGame6.onLocalPlayerExitRound = (Action)Delegate.Remove(currentGame6.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			}
			this.CurrentGame = null;
			this.PlayerDisplay.Unbind();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000E0A28 File Offset: 0x000DEC28
		private void BetSliderChanged(float newValue)
		{
			this.CurrentGame.SetLocalPlayerBet(this.GetBetFromSliderValue(newValue));
			this.RefreshDisplayedBet();
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000E0A42 File Offset: 0x000DEC42
		private float GetBetFromSliderValue(float sliderVal)
		{
			return Mathf.Lerp(10f, 500f, Mathf.Pow(sliderVal, 2f));
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000E0A60 File Offset: 0x000DEC60
		private void RefreshDisplayedBet()
		{
			this.BetAmount.text = MoneyManager.FormatAmount(this.CurrentGame.LocalPlayerBet, false, false);
			this.BetSlider.SetValueWithoutNotify(Mathf.Sqrt(Mathf.InverseLerp(10f, 500f, this.CurrentGame.LocalPlayerBet)));
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000E0AB4 File Offset: 0x000DECB4
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

		// Token: 0x060035A3 RID: 13731 RVA: 0x000E0B70 File Offset: 0x000DED70
		private void QuestionReady(string question, string[] answers)
		{
			this.QuestionLabel.text = question;
			this.SelectionIndicator.gameObject.SetActive(false);
			this.ForfeitLabel.text = "Forfeit and collect " + MoneyManager.FormatAmount(this.CurrentGame.MultipliedLocalPlayerBet, false, true);
			this.QuestionCanvasGroup.interactable = true;
			for (int i = 0; i < this.AnswerButtons.Length; i++)
			{
				if (answers.Length > i)
				{
					this.AnswerLabels[i].text = answers[i];
					this.AnswerButtons[i].gameObject.SetActive(true);
				}
				else
				{
					this.AnswerButtons[i].gameObject.SetActive(false);
				}
			}
			this.QuestionContainerAnimation.Play(this.QuestionContainerFadeIn.name);
			this.TimerSlider.value = 1f;
			base.StartCoroutine(this.<QuestionReady>g__Routine|38_0());
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000E0C54 File Offset: 0x000DEE54
		private void AnswerButtonClicked(int index)
		{
			this.SelectionIndicator.transform.position = this.AnswerButtons[index].transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.SetLocalPlayerAnswer((float)index + 1f);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000E0CA8 File Offset: 0x000DEEA8
		private void ForfeitClicked()
		{
			this.SelectionIndicator.transform.position = this.ForfeitButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.RemoveLocalPlayerFromGame(true, 0f);
			this.QuestionDone();
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000E0CFD File Offset: 0x000DEEFD
		private void QuestionDone()
		{
			this.QuestionCanvasGroup.interactable = false;
			this.QuestionContainerAnimation.Play(this.QuestionContainerFadeOut.name);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000E0D22 File Offset: 0x000DEF22
		private void LocalPlayerExitRound()
		{
			this.QuestionCanvasGroup.interactable = false;
			if (this.QuestionCanvasGroup.alpha > 0f)
			{
				this.QuestionContainerAnimation.Stop();
				this.QuestionCanvasGroup.alpha = 0f;
			}
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000E0D60 File Offset: 0x000DEF60
		private void Correct()
		{
			this.WinningsMultiplierLabel.text = Mathf.RoundToInt(this.CurrentGame.LocalPlayerBetMultiplier).ToString() + "x";
			if (this.CurrentGame.CurrentStage == RTBGameController.EStage.Suit)
			{
				if (this.onFinalCorrect != null)
				{
					this.onFinalCorrect.Invoke();
					return;
				}
			}
			else if (this.onCorrect != null)
			{
				this.onCorrect.Invoke();
			}
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000E0DCF File Offset: 0x000DEFCF
		private void Incorrect()
		{
			if (this.onIncorrect != null)
			{
				this.onIncorrect.Invoke();
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000E0DE4 File Offset: 0x000DEFE4
		private void ReadyButtonClicked()
		{
			this.CurrentGame.ToggleLocalPlayerReady();
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000E0DF9 File Offset: 0x000DEFF9
		[CompilerGenerated]
		private IEnumerator <QuestionReady>g__Routine|38_0()
		{
			while (this.CurrentGame != null && this.CurrentGame.IsQuestionActive)
			{
				this.TimerSlider.value = this.CurrentGame.RemainingAnswerTime / 6f;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x040025F4 RID: 9716
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040025F5 RID: 9717
		public CasinoGamePlayerDisplay PlayerDisplay;

		// Token: 0x040025F6 RID: 9718
		public TextMeshProUGUI StatusLabel;

		// Token: 0x040025F7 RID: 9719
		public RectTransform BetContainer;

		// Token: 0x040025F8 RID: 9720
		public TextMeshProUGUI BetTitleLabel;

		// Token: 0x040025F9 RID: 9721
		public Slider BetSlider;

		// Token: 0x040025FA RID: 9722
		public TextMeshProUGUI BetAmount;

		// Token: 0x040025FB RID: 9723
		public Button ReadyButton;

		// Token: 0x040025FC RID: 9724
		public TextMeshProUGUI ReadyLabel;

		// Token: 0x040025FD RID: 9725
		public TextMeshProUGUI WinningsMultiplierLabel;

		// Token: 0x040025FE RID: 9726
		[Header("Question and answers")]
		public RectTransform QuestionContainer;

		// Token: 0x040025FF RID: 9727
		public TextMeshProUGUI QuestionLabel;

		// Token: 0x04002600 RID: 9728
		public Slider TimerSlider;

		// Token: 0x04002601 RID: 9729
		public Button[] AnswerButtons;

		// Token: 0x04002602 RID: 9730
		public TextMeshProUGUI[] AnswerLabels;

		// Token: 0x04002603 RID: 9731
		public Button ForfeitButton;

		// Token: 0x04002604 RID: 9732
		public TextMeshProUGUI ForfeitLabel;

		// Token: 0x04002605 RID: 9733
		public Animation QuestionContainerAnimation;

		// Token: 0x04002606 RID: 9734
		public AnimationClip QuestionContainerFadeIn;

		// Token: 0x04002607 RID: 9735
		public AnimationClip QuestionContainerFadeOut;

		// Token: 0x04002608 RID: 9736
		public CanvasGroup QuestionCanvasGroup;

		// Token: 0x04002609 RID: 9737
		public RectTransform SelectionIndicator;

		// Token: 0x0400260A RID: 9738
		public UnityEvent onCorrect;

		// Token: 0x0400260B RID: 9739
		public UnityEvent onFinalCorrect;

		// Token: 0x0400260C RID: 9740
		public UnityEvent onIncorrect;
	}
}
