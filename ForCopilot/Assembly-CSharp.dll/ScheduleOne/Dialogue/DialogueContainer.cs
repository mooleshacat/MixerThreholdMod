using System;
using System.Collections.Generic;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000704 RID: 1796
	[Serializable]
	public class DialogueContainer : ScriptableObject
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000CCAC9 File Offset: 0x000CACC9
		// (set) Token: 0x060030EA RID: 12522 RVA: 0x000CCAD1 File Offset: 0x000CACD1
		public bool allowExit { get; private set; } = true;

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x000CCADA File Offset: 0x000CACDA
		public bool AllowExit
		{
			get
			{
				return this.allowExit || Player.Local.IsArrested || !Player.Local.Health.IsAlive;
			}
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000CCB04 File Offset: 0x000CAD04
		public DialogueNodeData GetDialogueNodeByLabel(string dialogueNodeLabel)
		{
			return this.DialogueNodeData.Find((DialogueNodeData x) => x.DialogueNodeLabel == dialogueNodeLabel);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000CCB38 File Offset: 0x000CAD38
		public BranchNodeData GetBranchNodeByLabel(string branchLabel)
		{
			return this.BranchNodeData.Find((BranchNodeData x) => x.BranchLabel == branchLabel);
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000CCB6C File Offset: 0x000CAD6C
		public DialogueNodeData GetDialogueNodeByGUID(string dialogueNodeGUID)
		{
			return this.DialogueNodeData.Find((DialogueNodeData x) => x.Guid == dialogueNodeGUID);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000CCBA0 File Offset: 0x000CADA0
		public BranchNodeData GetBranchNodeByGUID(string branchGUID)
		{
			return this.BranchNodeData.Find((BranchNodeData x) => x.Guid == branchGUID);
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000CCBD4 File Offset: 0x000CADD4
		public NodeLinkData GetLink(string baseChoiceOrOptionGUID)
		{
			return this.NodeLinks.Find((NodeLinkData x) => x.BaseChoiceOrOptionGUID == baseChoiceOrOptionGUID);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000CCC05 File Offset: 0x000CAE05
		public void SetAllowExit(bool allowed)
		{
			this.allowExit = allowed;
		}

		// Token: 0x0400225C RID: 8796
		public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();

		// Token: 0x0400225D RID: 8797
		public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();

		// Token: 0x0400225E RID: 8798
		public List<BranchNodeData> BranchNodeData = new List<BranchNodeData>();
	}
}
