using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006DE RID: 1758
	public class DialogueController_Ming : DialogueController
	{
		// Token: 0x06003051 RID: 12369 RVA: 0x000CACDC File Offset: 0x000C8EDC
		protected override void Start()
		{
			base.Start();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = this.BuyText;
			dialogueChoice.Conversation = this.BuyDialogue;
			dialogueChoice.Enabled = true;
			dialogueChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.CanBuyRoom);
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = this.RemindText;
			dialogueChoice2.Conversation = this.RemindLocationDialogue;
			dialogueChoice2.Enabled = true;
			dialogueChoice2.shouldShowCheck = ((bool enabled) => this.Property.IsOwned);
			this.AddDialogueChoice(dialogueChoice, 0);
			this.AddDialogueChoice(dialogueChoice2, 0);
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000CAD70 File Offset: 0x000C8F70
		private bool CanBuyRoom(bool enabled)
		{
			if (this.Property.IsOwned)
			{
				return false;
			}
			if (this.PurchaseRoomQuests.Length != 0)
			{
				return this.PurchaseRoomQuests.FirstOrDefault((QuestEntry q) => q.State == EQuestState.Active) != null;
			}
			return true;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000CADC7 File Offset: 0x000C8FC7
		public override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			if (choiceLabel == "CHOICE_CONFIRM")
			{
				choiceText = choiceText.Replace("<PRICE>", "<color=#54E717>(" + MoneyManager.FormatAmount(this.Price, false, false) + ")</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000CAE07 File Offset: 0x000C9007
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "ENTRY")
			{
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000CAE47 File Offset: 0x000C9047
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CHOICE_CONFIRM" && NetworkSingleton<MoneyManager>.Instance.cashBalance < this.Price)
			{
				invalidReason = "Insufficient cash";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000CAE7C File Offset: 0x000C907C
		public override void ChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "CHOICE_CONFIRM")
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.Price, true, false);
				this.npc.Inventory.InsertItem(NetworkSingleton<MoneyManager>.Instance.GetCashInstance(this.Price), true);
				this.Property.SetOwned();
				if (this.onPurchase != null)
				{
					this.onPurchase.Invoke();
				}
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x040021F7 RID: 8695
		public Property Property;

		// Token: 0x040021F8 RID: 8696
		public float Price = 500f;

		// Token: 0x040021F9 RID: 8697
		public DialogueContainer BuyDialogue;

		// Token: 0x040021FA RID: 8698
		public string BuyText = "I'd like to buy the room";

		// Token: 0x040021FB RID: 8699
		public string RemindText = "Where is my room?";

		// Token: 0x040021FC RID: 8700
		public DialogueContainer RemindLocationDialogue;

		// Token: 0x040021FD RID: 8701
		public QuestEntry[] PurchaseRoomQuests;

		// Token: 0x040021FE RID: 8702
		public UnityEvent onPurchase;
	}
}
