using System;
using System.Collections.Generic;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006CF RID: 1743
	public class ControlledDialogueHandler : DialogueHandler
	{
		// Token: 0x06002FFC RID: 12284 RVA: 0x000C97B4 File Offset: 0x000C79B4
		protected override void Awake()
		{
			base.Awake();
			this.controller = base.GetComponent<DialogueController>();
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000C97C8 File Offset: 0x000C79C8
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			dialogueText = this.controller.ModifyDialogueText(dialogueLabel, dialogueText);
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000C97E1 File Offset: 0x000C79E1
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			choiceText = this.controller.ModifyChoiceText(choiceLabel, choiceText);
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000C97FA File Offset: 0x000C79FA
		protected override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			this.controller.ModifyChoiceList(dialogueLabel, ref existingChoices);
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000C9811 File Offset: 0x000C7A11
		protected override void ChoiceCallback(string choiceLabel)
		{
			this.controller.ChoiceCallback(choiceLabel);
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000C9828 File Offset: 0x000C7A28
		protected override int CheckBranch(string branchLabel)
		{
			int result;
			if (this.controller.DecideBranch(branchLabel, out result))
			{
				return result;
			}
			return base.CheckBranch(branchLabel);
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000C984E File Offset: 0x000C7A4E
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			return this.controller.CheckChoice(choiceLabel, out invalidReason) && base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x040021C9 RID: 8649
		private DialogueController controller;
	}
}
