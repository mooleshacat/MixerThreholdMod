using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Money;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006DB RID: 1755
	public class DialogueController_Dealer : DialogueController
	{
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x000CA405 File Offset: 0x000C8605
		// (set) Token: 0x0600303A RID: 12346 RVA: 0x000CA40D File Offset: 0x000C860D
		public Dealer Dealer { get; private set; }

		// Token: 0x0600303B RID: 12347 RVA: 0x000CA416 File Offset: 0x000C8616
		protected override void Start()
		{
			base.Start();
			this.Dealer = (this.npc as Dealer);
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000CA430 File Offset: 0x000C8630
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && dialogueLabel == "ENTRY")
			{
				dialogueText = dialogueText.Replace("<SIGNING_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.Dealer.SigningFee, false, false) + "</color>");
				dialogueText = dialogueText.Replace("<CUT>", "<color=#54E717>" + Mathf.RoundToInt(this.Dealer.Cut * 100f).ToString() + "%</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000CA4D0 File Offset: 0x000C86D0
		public override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM")
			{
				choiceText = choiceText.Replace("<SIGNING_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.Dealer.SigningFee, false, false) + "</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000CA538 File Offset: 0x000C8738
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM" && NetworkSingleton<MoneyManager>.Instance.cashBalance < this.Dealer.SigningFee)
			{
				invalidReason = "Insufficient cash";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x000CA590 File Offset: 0x000C8790
		public override void ChoiceCallback(string choiceLabel)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM")
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.Dealer.SigningFee, true, false);
				this.Dealer.InitialRecruitment();
			}
			base.ChoiceCallback(choiceLabel);
		}
	}
}
