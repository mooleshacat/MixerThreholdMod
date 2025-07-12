using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Polling;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Polling
{
	// Token: 0x02000ACB RID: 2763
	public class PollPanel : MonoBehaviour
	{
		// Token: 0x06004A23 RID: 18979 RVA: 0x001376A0 File Offset: 0x001358A0
		private void Awake()
		{
			PollManager pollManager = this.PollManager;
			pollManager.onActivePollReceived = (Action<PollData>)Delegate.Combine(pollManager.onActivePollReceived, new Action<PollData>(this.DisplayActivePoll));
			PollManager pollManager2 = this.PollManager;
			pollManager2.onConfirmedPollReceived = (Action<PollData>)Delegate.Combine(pollManager2.onConfirmedPollReceived, new Action<PollData>(this.DisplayConfirmedPoll));
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00137708 File Offset: 0x00135908
		private void Update()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.heldButton != -1)
			{
				this.buttonPressTime += Time.deltaTime;
				if (this.buttonPressTime >= 0.8f)
				{
					this.FinalizeButtonPress(this.heldButton);
				}
			}
			else
			{
				this.buttonPressTime = Mathf.Clamp(this.buttonPressTime - Time.deltaTime, 0f, 0.8f);
				this.heldButton = -1;
			}
			for (int i = 0; i < this.buttonFills.Count; i++)
			{
				if (this.selectedButton == i)
				{
					this.buttonFills[i].fillAmount = 1f;
				}
				else if (this.heldButton == i || this.lastHeldButton == i)
				{
					this.buttonFills[i].fillAmount = this.buttonPressTime / 0.8f;
				}
				else
				{
					this.buttonFills[i].fillAmount = 0f;
				}
			}
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x001377FC File Offset: 0x001359FC
		public void DisplayActivePoll(PollData poll)
		{
			Console.Log("Displaying active poll: " + poll.question, null);
			this.ActivePill.SetActive(true);
			this.ClosedPill.SetActive(false);
			this.QuestionLabel.text = poll.question;
			this.buttons = this.CreateButtons(poll);
			foreach (Button button in this.buttons)
			{
				this.buttonFills.Add(button.transform.Find("Fill").GetComponent<Image>());
			}
			this.InstructionLabel.gameObject.SetActive(true);
			this.ConfirmationMessageLabel.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			int buttonIndex;
			if (PollManager.TryGetExistingPollResponse(poll.pollId, out buttonIndex))
			{
				this.DisplaySubmittedAnswer(buttonIndex);
			}
			this.Rebuild();
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x00137900 File Offset: 0x00135B00
		public void DisplayConfirmedPoll(PollData poll)
		{
			Console.Log("Displaying confirmed poll: " + poll.question, null);
			this.ActivePill.SetActive(false);
			this.ClosedPill.SetActive(true);
			this.QuestionLabel.text = poll.question;
			this.buttons = this.CreateButtons(poll);
			foreach (Button button in this.buttons)
			{
				this.buttonFills.Add(button.transform.Find("Fill").GetComponent<Image>());
				button.interactable = false;
			}
			if (poll.winnerIndex >= 0)
			{
				this.buttons[poll.winnerIndex].transform.Find("Winner").gameObject.SetActive(true);
				this.buttons[poll.winnerIndex].transform.Find("Outline").gameObject.SetActive(true);
			}
			this.InstructionLabel.gameObject.SetActive(false);
			this.ConfirmationMessageLabel.gameObject.SetActive(true);
			this.ConfirmationMessageLabel.text = poll.confirmationMessage;
			base.gameObject.SetActive(true);
			this.Rebuild();
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00137A64 File Offset: 0x00135C64
		private void DisplaySubmittedAnswer(int buttonIndex)
		{
			if (buttonIndex < 0 || buttonIndex >= this.buttons.Count)
			{
				Console.LogError("Button index out of range: " + buttonIndex.ToString(), null);
				return;
			}
			foreach (Button button in this.buttons)
			{
				button.GetComponent<Button>().interactable = false;
			}
			this.selectedButton = buttonIndex;
			this.buttons[buttonIndex].transform.Find("Outline").gameObject.SetActive(true);
			this.buttons[buttonIndex].transform.Find("Tick").gameObject.SetActive(true);
			this.InstructionLabel.gameObject.SetActive(false);
			this.ConfirmationMessageLabel.text = "Your vote has been recorded.\n Thank you!";
			this.ConfirmationMessageLabel.color = this.TextColor_Green;
			this.ConfirmationMessageLabel.gameObject.SetActive(true);
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x00137B7C File Offset: 0x00135D7C
		private void Rebuild()
		{
			PollPanel.<>c__DisplayClass27_0 CS$<>8__locals1 = new PollPanel.<>c__DisplayClass27_0();
			CS$<>8__locals1.layout = base.transform.parent.GetComponent<VerticalLayoutGroup>();
			CS$<>8__locals1.layout.gameObject.SetActive(false);
			CS$<>8__locals1.layout.enabled = false;
			LayoutRebuilder.ForceRebuildLayoutImmediate(CS$<>8__locals1.layout.GetComponent<RectTransform>());
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Rebuild>g__Wait|0());
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x00137BE4 File Offset: 0x00135DE4
		private List<Button> CreateButtons(PollData data)
		{
			List<Button> list = new List<Button>();
			for (int i = 0; i < data2.answers.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonPrefab, this.ButtonContainer);
				gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = data2.answers[i];
				int buttonIndex = i;
				EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.Entry entry = new EventTrigger.Entry
				{
					eventID = 2
				};
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.ButtonPressed(buttonIndex);
				});
				eventTrigger.triggers.Add(entry);
				list.Add(gameObject.GetComponent<Button>());
			}
			return list;
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00137CB4 File Offset: 0x00135EB4
		private void ButtonPressed(int buttonIndex)
		{
			if (!this.buttons[buttonIndex].GetComponent<Button>().interactable)
			{
				Console.LogWarning("Button " + buttonIndex.ToString() + " is not interactable, ignoring press.", null);
				return;
			}
			Console.Log("Button pressed: " + buttonIndex.ToString(), null);
			if (this.lastHeldButton != buttonIndex)
			{
				this.buttonPressTime = 0.1f;
			}
			this.heldButton = buttonIndex;
			this.lastHeldButton = this.heldButton;
			this.PollManager.GenerateAppTicket();
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00137D40 File Offset: 0x00135F40
		private void FinalizeButtonPress(int buttonIndex)
		{
			PollPanel.<>c__DisplayClass30_0 CS$<>8__locals1 = new PollPanel.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.buttonIndex = buttonIndex;
			this.selectedButton = CS$<>8__locals1.buttonIndex;
			this.heldButton = -1;
			this.buttons[CS$<>8__locals1.buttonIndex].transform.Find("Outline").gameObject.SetActive(true);
			foreach (Button button in this.buttons)
			{
				button.GetComponent<Button>().interactable = false;
			}
			this.SubmissionStartSound.Play();
			base.StartCoroutine(CS$<>8__locals1.<FinalizeButtonPress>g__Submit|0());
		}

		// Token: 0x04003675 RID: 13941
		public const float BUTTON_PRESS_TIME = 0.8f;

		// Token: 0x04003676 RID: 13942
		public const string ResponseSubmittedMessage = "Your vote has been recorded.\n Thank you!";

		// Token: 0x04003677 RID: 13943
		public GameObject ButtonPrefab;

		// Token: 0x04003678 RID: 13944
		public Color TextColor_Green;

		// Token: 0x04003679 RID: 13945
		public Color TextColor_Red;

		// Token: 0x0400367A RID: 13946
		[Header("References")]
		public PollManager PollManager;

		// Token: 0x0400367B RID: 13947
		public GameObject Container;

		// Token: 0x0400367C RID: 13948
		public GameObject ActivePill;

		// Token: 0x0400367D RID: 13949
		public GameObject ClosedPill;

		// Token: 0x0400367E RID: 13950
		public TextMeshProUGUI QuestionLabel;

		// Token: 0x0400367F RID: 13951
		public RectTransform ButtonContainer;

		// Token: 0x04003680 RID: 13952
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x04003681 RID: 13953
		public TextMeshProUGUI ConfirmationMessageLabel;

		// Token: 0x04003682 RID: 13954
		public AudioSourceController SubmissionStartSound;

		// Token: 0x04003683 RID: 13955
		public AudioSourceController SubmissionSuccessSound;

		// Token: 0x04003684 RID: 13956
		public AudioSourceController SubmissionFailSound;

		// Token: 0x04003685 RID: 13957
		private List<Button> buttons = new List<Button>();

		// Token: 0x04003686 RID: 13958
		private List<Image> buttonFills = new List<Image>();

		// Token: 0x04003687 RID: 13959
		private int heldButton = -1;

		// Token: 0x04003688 RID: 13960
		private int selectedButton = -1;

		// Token: 0x04003689 RID: 13961
		private int lastHeldButton = -1;

		// Token: 0x0400368A RID: 13962
		private float buttonPressTime;
	}
}
