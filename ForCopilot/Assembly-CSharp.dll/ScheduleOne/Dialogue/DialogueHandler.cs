using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006ED RID: 1773
	public class DialogueHandler : MonoBehaviour
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000CB522 File Offset: 0x000C9722
		// (set) Token: 0x06003081 RID: 12417 RVA: 0x000CB52A File Offset: 0x000C972A
		public bool IsPlaying { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000CB533 File Offset: 0x000C9733
		// (set) Token: 0x06003083 RID: 12419 RVA: 0x000CB53B File Offset: 0x000C973B
		public NPC NPC { get; protected set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000CB544 File Offset: 0x000C9744
		private DialogueCanvas canvas
		{
			get
			{
				return Singleton<DialogueCanvas>.Instance;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x000CB54B File Offset: 0x000C974B
		// (set) Token: 0x06003086 RID: 12422 RVA: 0x000CB553 File Offset: 0x000C9753
		public List<DialogueModule> runtimeModules { get; private set; } = new List<DialogueModule>();

		// Token: 0x06003087 RID: 12423 RVA: 0x000CB55C File Offset: 0x000C975C
		protected virtual void Awake()
		{
			if (this.NPC == null)
			{
				this.NPC = base.GetComponentInParent<NPC>();
			}
			DialogueModule dialogueModule = base.gameObject.AddComponent<DialogueModule>();
			dialogueModule.ModuleType = EDialogueModule.Generic;
			dialogueModule.Entries = this.Database.GenericEntries;
			this.runtimeModules.Add(dialogueModule);
			this.runtimeModules.AddRange(this.Database.Modules);
			this.Database.Initialize(this);
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000CB5D8 File Offset: 0x000C97D8
		protected virtual void Start()
		{
			if (this.Database == null)
			{
				Console.LogWarning(this.NPC.fullName + " dialogue database isn't assigned! Using default database.", null);
				if (Singleton<DialogueManager>.Instance != null)
				{
					this.Database = Singleton<DialogueManager>.Instance.DefaultDatabase;
				}
				else
				{
					Console.LogError("DialogueManager instance is null. Cannot use default database.", null);
				}
			}
			if (this.VOEmitter == null && this.NPC != null)
			{
				this.VOEmitter = this.NPC.VoiceOverEmitter;
			}
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000CB665 File Offset: 0x000C9865
		public void InitializeDialogue(DialogueContainer container)
		{
			this.InitializeDialogue(container, true, "ENTRY");
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000CB674 File Offset: 0x000C9874
		public void InitializeDialogue(DialogueContainer dialogueContainer, bool enableDialogueBehaviour = true, string entryNodeLabel = "ENTRY")
		{
			DialogueHandler.<>c__DisplayClass35_0 CS$<>8__locals1 = new DialogueHandler.<>c__DisplayClass35_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.dialogueContainer = dialogueContainer;
			CS$<>8__locals1.entryNodeLabel = entryNodeLabel;
			if (CS$<>8__locals1.dialogueContainer == null)
			{
				Console.LogWarning("InitializeDialogue: provided dialogueContainer is null", null);
				return;
			}
			if (enableDialogueBehaviour)
			{
				this.NPC.behaviour.GenericDialogueBehaviour.SendTargetPlayer(Player.Local.NetworkObject);
				this.NPC.behaviour.GenericDialogueBehaviour.SendEnable();
				this.NPC.behaviour.Update();
			}
			if (this.WorldspaceRend.ShownText != null)
			{
				this.WorldspaceRend.HideText();
			}
			if (this.onConversationStart != null)
			{
				this.onConversationStart.Invoke();
			}
			CS$<>8__locals1.npc = base.GetComponentInParent<NPC>();
			if (CS$<>8__locals1.npc != null && CS$<>8__locals1.npc.Avatar.Anim.TimeSinceSitEnd < 0.5f && enableDialogueBehaviour)
			{
				base.StartCoroutine(CS$<>8__locals1.<InitializeDialogue>g__Wait|0());
				return;
			}
			CS$<>8__locals1.<InitializeDialogue>g__Open|1();
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000CB77C File Offset: 0x000C997C
		public void InitializeDialogue(string dialogueContainerName, bool enableDialogueBehaviour = true, string entryNodeLabel = "ENTRY")
		{
			DialogueContainer dialogueContainer = this.dialogueContainers.Find((DialogueContainer x) => x.name.ToLower() == dialogueContainerName.ToLower());
			if (dialogueContainer == null)
			{
				Console.LogWarning("InitializeDialogue: Could not find DialogueContainer with name '" + dialogueContainerName + "'", null);
				return;
			}
			this.InitializeDialogue(dialogueContainer, enableDialogueBehaviour, entryNodeLabel);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000CB7DC File Offset: 0x000C99DC
		public virtual bool CanBeginConversation()
		{
			return this.NPC.SyncAccessor_PlayerConversant == null;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000CB7EF File Offset: 0x000C99EF
		public void OverrideShownDialogue(string _overrideText)
		{
			this.overrideText = _overrideText;
			this.canvas.OverrideText(this.overrideText);
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000CB809 File Offset: 0x000C9A09
		public void StopOverride()
		{
			this.overrideText = string.Empty;
			this.canvas.StopTextOverride();
			if (DialogueHandler.activeDialogueNode != null)
			{
				this.ShowNode(DialogueHandler.activeDialogueNode);
			}
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000CB834 File Offset: 0x000C9A34
		public virtual void EndDialogue()
		{
			if (this.skipNextDialogueBehaviourEnd)
			{
				this.skipNextDialogueBehaviourEnd = false;
			}
			else
			{
				this.NPC.behaviour.GenericDialogueBehaviour.SendDisable();
			}
			foreach (DialogueEvent dialogueEvent in this.DialogueEvents)
			{
				if (!(dialogueEvent.Dialogue != DialogueHandler.activeDialogue) && dialogueEvent.onDialogueEnded != null)
				{
					dialogueEvent.onDialogueEnded.Invoke();
				}
			}
			this.canvas.EndDialogue();
			this.IsPlaying = false;
			DialogueHandler.activeDialogue = null;
			DialogueHandler.activeDialogueNode = null;
			this.NPC.SetConversant(null);
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000CB8CF File Offset: 0x000C9ACF
		public void SkipNextDialogueBehaviourEnd()
		{
			this.skipNextDialogueBehaviourEnd = true;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000CB8D8 File Offset: 0x000C9AD8
		protected virtual DialogueNodeData FinalizeDialogueNode(DialogueNodeData data)
		{
			return data;
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000CB8DC File Offset: 0x000C9ADC
		public void ShowNode(DialogueNodeData node)
		{
			node = this.FinalizeDialogueNode(node);
			DialogueHandler.activeDialogueNode = node;
			if (this.overrideText != string.Empty)
			{
				return;
			}
			string dialogueText = this.ModifyDialogueText(node.DialogueNodeLabel, node.DialogueText);
			this.CurrentChoices = new List<DialogueChoiceData>();
			foreach (DialogueChoiceData dialogueChoiceData in node.choices)
			{
				if (this.ShouldChoiceBeShown(dialogueChoiceData.ChoiceLabel))
				{
					this.CurrentChoices.Add(dialogueChoiceData);
				}
			}
			this.TempLinks.Clear();
			this.ModifyChoiceList(node.DialogueNodeLabel, ref this.CurrentChoices);
			List<string> list = new List<string>();
			foreach (DialogueChoiceData dialogueChoiceData2 in this.CurrentChoices)
			{
				list.Add(this.ModifyChoiceText(dialogueChoiceData2.ChoiceLabel, dialogueChoiceData2.ChoiceText));
			}
			this.DialogueCallback(node.DialogueNodeLabel);
			if (this.VOEmitter != null && node.VoiceLine != EVOLineType.None)
			{
				this.VOEmitter.Play(node.VoiceLine);
			}
			this.canvas.DisplayDialogueNode(this, DialogueHandler.activeDialogueNode, dialogueText, list);
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000CBA24 File Offset: 0x000C9C24
		private void EvaluateBranch(BranchNodeData node)
		{
			int num = this.CheckBranch(node.BranchLabel);
			if (node.options.Length > num)
			{
				NodeLinkData link = this.GetLink(node.options[num].Guid);
				if (link != null)
				{
					if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
					}
					else if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
					}
				}
				else
				{
					this.EndDialogue();
				}
			}
			else
			{
				Console.LogWarning("EvaluateBranch: optionIndex is out of range", null);
				this.EndDialogue();
			}
			this.TempLinks.Clear();
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000CBAD8 File Offset: 0x000C9CD8
		public void ChoiceSelected(int choiceIndex)
		{
			DialogueNodeData dialogueNodeData = DialogueHandler.activeDialogueNode;
			this.ChoiceCallback(this.CurrentChoices[choiceIndex].ChoiceLabel);
			if (DialogueHandler.activeDialogueNode == dialogueNodeData && DialogueHandler.activeDialogueNode != null)
			{
				NodeLinkData link = this.GetLink(this.CurrentChoices[choiceIndex].Guid);
				if (link != null)
				{
					if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
						return;
					}
					if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
					{
						this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
						return;
					}
				}
				else
				{
					this.EndDialogue();
				}
			}
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000CBB84 File Offset: 0x000C9D84
		public void ContinueSubmitted()
		{
			if (DialogueHandler.activeDialogueNode.choices.Length == 0)
			{
				this.EndDialogue();
				return;
			}
			NodeLinkData link = this.GetLink(DialogueHandler.activeDialogueNode.choices[0].Guid);
			if (link != null)
			{
				if (DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid) != null)
				{
					this.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByGUID(link.TargetNodeGuid));
					return;
				}
				if (DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid) != null)
				{
					this.EvaluateBranch(DialogueHandler.activeDialogue.GetBranchNodeByGUID(link.TargetNodeGuid));
					return;
				}
			}
			else
			{
				this.EndDialogue();
			}
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000CBC18 File Offset: 0x000C9E18
		public virtual bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CHOICE_TEST")
			{
				invalidReason = "IT JUST CAN'T BE DONE";
				return false;
			}
			invalidReason = string.Empty;
			return true;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool ShouldChoiceBeShown(string choiceLabel)
		{
			return true;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000CBC38 File Offset: 0x000C9E38
		protected virtual int CheckBranch(string branchLabel)
		{
			if (branchLabel == "BRANCH_REJECTION")
			{
				return UnityEngine.Random.Range(0, 2);
			}
			if (!(branchLabel == "BRANCH_CHECKPASS"))
			{
				if (branchLabel != string.Empty)
				{
					Console.LogWarning("CheckBranch: branch label '" + branchLabel + "' not accounted for!", null);
				}
				return 0;
			}
			if (this.passChecked)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000C9D9B File Offset: 0x000C7F9B
		protected virtual string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			return dialogueText;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000C9D9B File Offset: 0x000C7F9B
		protected virtual string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			return choiceText;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000CBC97 File Offset: 0x000C9E97
		protected virtual void ChoiceCallback(string choiceLabel)
		{
			if (this.onDialogueChoiceChosen != null)
			{
				this.onDialogueChoiceChosen.Invoke(choiceLabel);
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000CBCB0 File Offset: 0x000C9EB0
		protected virtual void DialogueCallback(string dialogueLabel)
		{
			if (this.onDialogueNodeDisplayed != null)
			{
				this.onDialogueNodeDisplayed.Invoke(dialogueLabel);
			}
			foreach (DialogueEvent dialogueEvent in this.DialogueEvents)
			{
				if (!(dialogueEvent.Dialogue != DialogueHandler.activeDialogue))
				{
					foreach (DialogueNodeEvent dialogueNodeEvent in dialogueEvent.NodeEvents)
					{
						if (dialogueNodeEvent.NodeLabel == dialogueLabel)
						{
							dialogueNodeEvent.onNodeDisplayed.Invoke();
						}
					}
				}
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000CBD38 File Offset: 0x000C9F38
		protected void CreateTempLink(string baseNodeGUID, string baseOptionGUID, string targetNodeGUID)
		{
			NodeLinkData nodeLinkData = new NodeLinkData();
			nodeLinkData.BaseDialogueOrBranchNodeGuid = baseNodeGUID;
			nodeLinkData.BaseChoiceOrOptionGUID = baseOptionGUID;
			nodeLinkData.TargetNodeGuid = targetNodeGUID;
			this.TempLinks.Add(nodeLinkData);
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000CBD6C File Offset: 0x000C9F6C
		private NodeLinkData GetLink(string baseChoiceOrOptionGUID)
		{
			NodeLinkData nodeLinkData = DialogueHandler.activeDialogue.GetLink(baseChoiceOrOptionGUID);
			if (nodeLinkData == null)
			{
				nodeLinkData = this.TempLinks.Find((NodeLinkData x) => x.BaseChoiceOrOptionGUID == baseChoiceOrOptionGUID);
			}
			return nodeLinkData;
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Hovered()
		{
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Interacted()
		{
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000CBDB3 File Offset: 0x000C9FB3
		public virtual void PlayReaction_Local(string key)
		{
			this.PlayReaction(key, -1f, false);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000CBDC2 File Offset: 0x000C9FC2
		public virtual void PlayReaction_Networked(string key)
		{
			this.PlayReaction(key, -1f, true);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000CBDD4 File Offset: 0x000C9FD4
		public virtual void PlayReaction(string key, float duration, bool network)
		{
			if (!this.NPC.IsConscious)
			{
				return;
			}
			if (network)
			{
				this.NPC.SendWorldspaceDialogueKey(key, duration);
				return;
			}
			if (key == string.Empty)
			{
				this.HideWorldspaceDialogue();
				return;
			}
			string line = this.Database.GetLine(EDialogueModule.Reactions, key);
			if (duration == -1f)
			{
				duration = Mathf.Clamp((float)line.Length * 0.2f, 1.5f, 5f);
			}
			this.WorldspaceRend.ShowText(line, duration);
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000CBE55 File Offset: 0x000CA055
		public virtual void HideWorldspaceDialogue()
		{
			this.WorldspaceRend.HideText();
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000CBE62 File Offset: 0x000CA062
		public virtual void ShowWorldspaceDialogue(string text, float duration)
		{
			if (!this.NPC.IsConscious)
			{
				return;
			}
			this.WorldspaceRend.ShowText(text, duration);
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000CBE7F File Offset: 0x000CA07F
		public virtual void ShowWorldspaceDialogue_5s(string text)
		{
			this.ShowWorldspaceDialogue(text, 5f);
		}

		// Token: 0x0400221A RID: 8730
		public const float TimePerChar = 0.2f;

		// Token: 0x0400221B RID: 8731
		public const float WorldspaceDialogueMinDuration = 1.5f;

		// Token: 0x0400221C RID: 8732
		public const float WorldspaceDialogueMaxDuration = 5f;

		// Token: 0x0400221D RID: 8733
		public static DialogueContainer activeDialogue;

		// Token: 0x0400221E RID: 8734
		public static DialogueNodeData activeDialogueNode;

		// Token: 0x04002220 RID: 8736
		public DialogueDatabase Database;

		// Token: 0x04002221 RID: 8737
		[Header("References")]
		public Transform LookPosition;

		// Token: 0x04002222 RID: 8738
		public WorldspaceDialogueRenderer WorldspaceRend;

		// Token: 0x04002224 RID: 8740
		public VOEmitter VOEmitter;

		// Token: 0x04002225 RID: 8741
		[HideInInspector]
		public List<DialogueChoiceData> CurrentChoices = new List<DialogueChoiceData>();

		// Token: 0x04002226 RID: 8742
		[Header("Events")]
		public DialogueEvent[] DialogueEvents;

		// Token: 0x04002227 RID: 8743
		public UnityEvent onConversationStart;

		// Token: 0x04002228 RID: 8744
		public UnityEvent<string> onDialogueNodeDisplayed;

		// Token: 0x04002229 RID: 8745
		public UnityEvent<string> onDialogueChoiceChosen;

		// Token: 0x0400222A RID: 8746
		protected string overrideText = string.Empty;

		// Token: 0x0400222B RID: 8747
		[SerializeField]
		private List<DialogueContainer> dialogueContainers = new List<DialogueContainer>();

		// Token: 0x0400222C RID: 8748
		private List<NodeLinkData> TempLinks = new List<NodeLinkData>();

		// Token: 0x0400222D RID: 8749
		private bool skipNextDialogueBehaviourEnd;

		// Token: 0x0400222F RID: 8751
		private bool passChecked;
	}
}
