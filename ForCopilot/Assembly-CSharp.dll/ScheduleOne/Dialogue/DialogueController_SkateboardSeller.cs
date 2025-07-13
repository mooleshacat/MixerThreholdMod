using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E1 RID: 1761
	public class DialogueController_SkateboardSeller : DialogueController
	{
		// Token: 0x0600305F RID: 12383 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000CAFC4 File Offset: 0x000C91C4
		public override void ChoiceCallback(string choiceLabel)
		{
			DialogueController_SkateboardSeller.Option option = this.Options.Find((DialogueController_SkateboardSeller.Option x) => x.Name == choiceLabel);
			if (option != null)
			{
				this.chosenWeapon = option;
				this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
			}
			if (choiceLabel == "CONFIRM" && this.chosenWeapon != null)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.chosenWeapon.Price, true, false);
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.chosenWeapon.Item.GetDefaultInstance(1));
				this.npc.Inventory.InsertItem(NetworkSingleton<MoneyManager>.Instance.GetCashInstance(this.chosenWeapon.Price), true);
				if (this.chosenWeapon.Item.ID == "goldenskateboard")
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.ROLLING_IN_STYLE);
				}
				if (this.onPurchase != null)
				{
					this.onPurchase.Invoke();
				}
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000CB0D9 File Offset: 0x000C92D9
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "ENTRY" && DialogueHandler.activeDialogue.name == "Skateboard_Sell")
			{
				existingChoices.AddRange(this.GetChoices(this.Options));
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000CB11C File Offset: 0x000C931C
		private List<DialogueChoiceData> GetChoices(List<DialogueController_SkateboardSeller.Option> options)
		{
			List<DialogueChoiceData> list = new List<DialogueChoiceData>();
			foreach (DialogueController_SkateboardSeller.Option option in options)
			{
				list.Add(new DialogueChoiceData
				{
					ChoiceText = option.Name + "<color=#54E717> (" + MoneyManager.FormatAmount(option.Price, false, false) + ")</color>",
					ChoiceLabel = option.Name
				});
			}
			return list;
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000CB1AC File Offset: 0x000C93AC
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			DialogueController_SkateboardSeller.Option option = this.Options.Find((DialogueController_SkateboardSeller.Option x) => x.Name == choiceLabel);
			if (option != null)
			{
				if (!option.IsAvailable)
				{
					invalidReason = option.NotAvailableReason;
					return false;
				}
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < option.Price)
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			if (choiceLabel == "CONFIRM" && !PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.chosenWeapon.Item.GetDefaultInstance(1), 1))
			{
				invalidReason = "Inventory full";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000CB254 File Offset: 0x000C9454
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE" && this.chosenWeapon != null)
			{
				dialogueText = dialogueText.Replace("<NAME>", this.chosenWeapon.Name);
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.chosenWeapon.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x04002202 RID: 8706
		public List<DialogueController_SkateboardSeller.Option> Options = new List<DialogueController_SkateboardSeller.Option>();

		// Token: 0x04002203 RID: 8707
		private DialogueController_SkateboardSeller.Option chosenWeapon;

		// Token: 0x04002204 RID: 8708
		public UnityEvent onPurchase;

		// Token: 0x020006E2 RID: 1762
		[Serializable]
		public class Option
		{
			// Token: 0x04002205 RID: 8709
			public string Name;

			// Token: 0x04002206 RID: 8710
			public float Price;

			// Token: 0x04002207 RID: 8711
			public bool IsAvailable;

			// Token: 0x04002208 RID: 8712
			public string NotAvailableReason;

			// Token: 0x04002209 RID: 8713
			public ItemDefinition Item;
		}
	}
}
