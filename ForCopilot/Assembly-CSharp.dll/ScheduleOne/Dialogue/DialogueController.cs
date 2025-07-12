using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006D0 RID: 1744
	public class DialogueController : MonoBehaviour
	{
		// Token: 0x06003004 RID: 12292 RVA: 0x000C9874 File Offset: 0x000C7A74
		protected virtual void Start()
		{
			this.handler = base.GetComponent<DialogueHandler>();
			this.npc = this.handler.NPC;
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000C98D6 File Offset: 0x000C7AD6
		private void Update()
		{
			this.lastGreetingTime += Time.deltaTime;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000C98EC File Offset: 0x000C7AEC
		private void Hovered()
		{
			if (this.CanStartDialogue() && (((this.GetActiveChoices().Count > 0 || this.lastGreetingTime > DialogueController.GREETING_COOLDOWN) && this.DialogueEnabled) || this.OverrideContainer != null))
			{
				this.IntObj.SetMessage("Talk to " + this.npc.GetNameAddress());
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000C996B File Offset: 0x000C7B6B
		public void StartGenericDialogue(bool allowExit = true)
		{
			this.Interacted();
			this.GenericDialogue.SetAllowExit(allowExit);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000C9980 File Offset: 0x000C7B80
		private void Interacted()
		{
			this.GenericDialogue.SetAllowExit(true);
			this.dialogueQueued = true;
			base.Invoke("Unqueue", 1f);
			if (this.OverrideContainer != null)
			{
				this.handler.InitializeDialogue(this.OverrideContainer, this.UseDialogueBehaviour, "ENTRY");
				return;
			}
			if (this.GetActiveChoices().Count > 0)
			{
				this.shownChoices = this.GetActiveChoices();
				bool flag;
				EVOLineType evolineType;
				this.cachedGreeting = this.GetActiveGreeting(out flag, out evolineType);
				this.handler.InitializeDialogue(this.GenericDialogue, this.UseDialogueBehaviour, "ENTRY");
				if (flag && evolineType != EVOLineType.None)
				{
					this.npc.PlayVO(evolineType);
					return;
				}
			}
			else
			{
				bool flag2;
				EVOLineType lineType;
				this.handler.ShowWorldspaceDialogue(this.GetActiveGreeting(out flag2, out lineType), 5f);
				this.lastGreetingTime = 0f;
				if (flag2)
				{
					this.npc.PlayVO(lineType);
				}
			}
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000C9A68 File Offset: 0x000C7C68
		private void Unqueue()
		{
			this.dialogueQueued = false;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000C9A74 File Offset: 0x000C7C74
		private string GetActiveGreeting(out bool playVO, out EVOLineType voLineType)
		{
			playVO = false;
			string result;
			if (this.GetCustomGreeting(out result, out playVO, out voLineType))
			{
				return result;
			}
			playVO = true;
			voLineType = EVOLineType.Greeting;
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(400, 1200))
			{
				return this.handler.Database.GetLine(EDialogueModule.Greetings, "morning_greeting");
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(1200, 1800))
			{
				return this.handler.Database.GetLine(EDialogueModule.Greetings, "afternoon_greeting");
			}
			return this.handler.Database.GetLine(EDialogueModule.Greetings, "night_greeting");
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000C9B08 File Offset: 0x000C7D08
		private List<DialogueController.DialogueChoice> GetActiveChoices()
		{
			List<DialogueController.DialogueChoice> list = new List<DialogueController.DialogueChoice>();
			foreach (DialogueController.DialogueChoice dialogueChoice in this.Choices)
			{
				if (dialogueChoice.ShouldShow())
				{
					list.Add(dialogueChoice);
				}
			}
			list.Sort((DialogueController.DialogueChoice a, DialogueController.DialogueChoice b) => b.Priority.CompareTo(a.Priority));
			return list;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000C9B90 File Offset: 0x000C7D90
		protected virtual bool GetCustomGreeting(out string greeting, out bool playVO, out EVOLineType voLineType)
		{
			greeting = string.Empty;
			playVO = false;
			voLineType = EVOLineType.Greeting;
			for (int i = 0; i < this.GreetingOverrides.Count; i++)
			{
				if (this.GreetingOverrides[i].ShouldShow)
				{
					greeting = this.GreetingOverrides[i].Greeting;
					playVO = this.GreetingOverrides[i].PlayVO;
					voLineType = this.GreetingOverrides[i].VOType;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000C9C0F File Offset: 0x000C7E0F
		public virtual int AddDialogueChoice(DialogueController.DialogueChoice data, int priority = 0)
		{
			data.Priority = priority;
			this.Choices.Add(data);
			return this.Choices.Count - 1;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000C9C31 File Offset: 0x000C7E31
		public virtual int AddGreetingOverride(DialogueController.GreetingOverride data)
		{
			this.GreetingOverrides.Add(data);
			return this.Choices.Count - 1;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000C9C4C File Offset: 0x000C7E4C
		public virtual bool CanStartDialogue()
		{
			return (Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None || Player.Local.CrimeData.TimeSinceSighted >= 5f) && !Singleton<ManagementClipboard>.Instance.IsEquipped && !this.npc.behaviour.CallPoliceBehaviour.Active && !this.npc.behaviour.CombatBehaviour.Active && !this.npc.behaviour.CoweringBehaviour.Active && !this.npc.behaviour.RagdollBehaviour.Active && !this.npc.behaviour.HeavyFlinchBehaviour.Active && !this.npc.behaviour.ConsumeProductBehaviour.Active && !this.npc.behaviour.FleeBehaviour.Active && !this.npc.behaviour.GenericDialogueBehaviour.Active && this.npc.IsConscious && !this.dialogueQueued;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000C9D72 File Offset: 0x000C7F72
		public virtual string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && dialogueLabel == "ENTRY")
			{
				return this.cachedGreeting;
			}
			return dialogueText;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000C9D9B File Offset: 0x000C7F9B
		public virtual string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			return choiceText;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000C9DA0 File Offset: 0x000C7FA0
		public virtual void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && dialogueLabel == "ENTRY")
			{
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				for (int i = 0; i < list.Count; i++)
				{
					DialogueChoiceData dialogueChoiceData = new DialogueChoiceData();
					dialogueChoiceData.ChoiceText = list[i].ChoiceText;
					dialogueChoiceData.ChoiceLabel = "GENERIC_CHOICE_" + i.ToString();
					existingChoices.Add(dialogueChoiceData);
				}
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000C9E1C File Offset: 0x000C801C
		public virtual void ChoiceCallback(string choiceLabel)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && choiceLabel.Contains("GENERIC_CHOICE_"))
			{
				int num = int.Parse(choiceLabel.Substring("GENERIC_CHOICE_".Length));
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				if (num >= 0 && num < list.Count)
				{
					DialogueController.DialogueChoice dialogueChoice = list[num];
					if (dialogueChoice.onChoosen != null)
					{
						dialogueChoice.onChoosen.Invoke();
					}
					if (dialogueChoice.Conversation != null)
					{
						this.handler.InitializeDialogue(dialogueChoice.Conversation);
					}
				}
			}
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000C9EAC File Offset: 0x000C80AC
		public virtual bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (DialogueHandler.activeDialogue == this.GenericDialogue && choiceLabel.Contains("GENERIC_CHOICE_"))
			{
				int num = int.Parse(choiceLabel.Substring("GENERIC_CHOICE_".Length));
				List<DialogueController.DialogueChoice> list = this.shownChoices;
				if (num >= 0 && num < list.Count)
				{
					return list[num].IsValid(out invalidReason);
				}
			}
			invalidReason = string.Empty;
			return true;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000C9F18 File Offset: 0x000C8118
		public void SetOverrideContainer(DialogueContainer container)
		{
			this.OverrideContainer = container;
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000C9F21 File Offset: 0x000C8121
		public void ClearOverrideContainer()
		{
			this.OverrideContainer = null;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000C9F2A File Offset: 0x000C812A
		public virtual bool DecideBranch(string branchLabel, out int index)
		{
			index = 0;
			return false;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000C9F30 File Offset: 0x000C8130
		public void SetDialogueEnabled(bool enabled)
		{
			this.DialogueEnabled = enabled;
		}

		// Token: 0x040021CA RID: 8650
		public static float GREETING_COOLDOWN = 5f;

		// Token: 0x040021CB RID: 8651
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x040021CC RID: 8652
		public DialogueContainer GenericDialogue;

		// Token: 0x040021CD RID: 8653
		[Header("Settings")]
		public bool DialogueEnabled = true;

		// Token: 0x040021CE RID: 8654
		public bool UseDialogueBehaviour = true;

		// Token: 0x040021CF RID: 8655
		public List<DialogueController.DialogueChoice> Choices = new List<DialogueController.DialogueChoice>();

		// Token: 0x040021D0 RID: 8656
		public List<DialogueController.GreetingOverride> GreetingOverrides = new List<DialogueController.GreetingOverride>();

		// Token: 0x040021D1 RID: 8657
		public DialogueContainer OverrideContainer;

		// Token: 0x040021D2 RID: 8658
		protected NPC npc;

		// Token: 0x040021D3 RID: 8659
		protected DialogueHandler handler;

		// Token: 0x040021D4 RID: 8660
		private float lastGreetingTime = 20f;

		// Token: 0x040021D5 RID: 8661
		private List<DialogueController.DialogueChoice> shownChoices = new List<DialogueController.DialogueChoice>();

		// Token: 0x040021D6 RID: 8662
		private bool dialogueQueued;

		// Token: 0x040021D7 RID: 8663
		private string cachedGreeting = string.Empty;

		// Token: 0x020006D1 RID: 1745
		[Serializable]
		public class DialogueChoice
		{
			// Token: 0x0600301B RID: 12315 RVA: 0x000C9FA0 File Offset: 0x000C81A0
			public bool ShouldShow()
			{
				if (this.shouldShowCheck != null)
				{
					return this.shouldShowCheck(this.Enabled);
				}
				return this.Enabled;
			}

			// Token: 0x0600301C RID: 12316 RVA: 0x000C9FC2 File Offset: 0x000C81C2
			public bool IsValid(out string invalidReason)
			{
				if (this.isValidCheck != null)
				{
					return this.isValidCheck(out invalidReason);
				}
				invalidReason = string.Empty;
				return true;
			}

			// Token: 0x040021D8 RID: 8664
			public bool Enabled = true;

			// Token: 0x040021D9 RID: 8665
			public string ChoiceText;

			// Token: 0x040021DA RID: 8666
			public DialogueContainer Conversation;

			// Token: 0x040021DB RID: 8667
			public UnityEvent onChoosen = new UnityEvent();

			// Token: 0x040021DC RID: 8668
			public DialogueController.DialogueChoice.ShouldShowCheck shouldShowCheck;

			// Token: 0x040021DD RID: 8669
			public DialogueController.DialogueChoice.IsChoiceValid isValidCheck;

			// Token: 0x040021DE RID: 8670
			public int Priority;

			// Token: 0x020006D2 RID: 1746
			// (Invoke) Token: 0x0600301F RID: 12319
			public delegate bool ShouldShowCheck(bool enabled);

			// Token: 0x020006D3 RID: 1747
			// (Invoke) Token: 0x06003023 RID: 12323
			public delegate bool IsChoiceValid(out string invalidReason);
		}

		// Token: 0x020006D4 RID: 1748
		[Serializable]
		public class GreetingOverride
		{
			// Token: 0x040021DF RID: 8671
			public string Greeting;

			// Token: 0x040021E0 RID: 8672
			public bool ShouldShow;

			// Token: 0x040021E1 RID: 8673
			public bool PlayVO;

			// Token: 0x040021E2 RID: 8674
			public EVOLineType VOType;
		}
	}
}
