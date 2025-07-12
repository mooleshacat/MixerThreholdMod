using System;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200070A RID: 1802
	[Serializable]
	public class DialogueNodeData
	{
		// Token: 0x060030FD RID: 12541 RVA: 0x000CCCA0 File Offset: 0x000CAEA0
		public DialogueNodeData GetCopy()
		{
			DialogueNodeData dialogueNodeData = new DialogueNodeData();
			dialogueNodeData.Guid = this.Guid;
			dialogueNodeData.DialogueText = this.DialogueText;
			dialogueNodeData.DialogueNodeLabel = this.DialogueNodeLabel;
			dialogueNodeData.Position = this.Position;
			for (int i = 0; i < this.choices.Length; i++)
			{
				this.choices.CopyTo(dialogueNodeData.choices, 0);
			}
			dialogueNodeData.VoiceLine = this.VoiceLine;
			return dialogueNodeData;
		}

		// Token: 0x04002264 RID: 8804
		public string Guid;

		// Token: 0x04002265 RID: 8805
		public string DialogueText;

		// Token: 0x04002266 RID: 8806
		public string DialogueNodeLabel;

		// Token: 0x04002267 RID: 8807
		public Vector2 Position;

		// Token: 0x04002268 RID: 8808
		public DialogueChoiceData[] choices;

		// Token: 0x04002269 RID: 8809
		public EVOLineType VoiceLine;
	}
}
