using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006D6 RID: 1750
	public class DialogueController_ArmsDealer : DialogueController
	{
		// Token: 0x0600302A RID: 12330 RVA: 0x000CA01A File Offset: 0x000C821A
		private void Awake()
		{
			this.allWeapons = new List<DialogueController_ArmsDealer.WeaponOption>();
			this.allWeapons.AddRange(this.MeleeWeapons);
			this.allWeapons.AddRange(this.RangedWeapons);
			this.allWeapons.AddRange(this.Ammo);
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000CA05C File Offset: 0x000C825C
		public override void ChoiceCallback(string choiceLabel)
		{
			DialogueController_ArmsDealer.WeaponOption weaponOption = this.allWeapons.Find((DialogueController_ArmsDealer.WeaponOption x) => x.Name == choiceLabel);
			if (weaponOption != null)
			{
				this.chosenWeapon = weaponOption;
				this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
			}
			if (choiceLabel == "CONFIRM" && this.chosenWeapon != null)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.chosenWeapon.Price, true, false);
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.chosenWeapon.Item.GetDefaultInstance(1));
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000CA10C File Offset: 0x000C830C
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "MELEE_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.MeleeWeapons));
			}
			if (dialogueLabel == "RANGED_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.RangedWeapons));
			}
			if (dialogueLabel == "AMMO_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.Ammo));
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000CA184 File Offset: 0x000C8384
		private List<DialogueChoiceData> GetWeaponChoices(List<DialogueController_ArmsDealer.WeaponOption> options)
		{
			List<DialogueChoiceData> list = new List<DialogueChoiceData>();
			foreach (DialogueController_ArmsDealer.WeaponOption weaponOption in options)
			{
				list.Add(new DialogueChoiceData
				{
					ChoiceText = weaponOption.Name + "<color=#54E717> (" + MoneyManager.FormatAmount(weaponOption.Price, false, false) + ")</color>",
					ChoiceLabel = weaponOption.Name
				});
			}
			return list;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000CA214 File Offset: 0x000C8414
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			DialogueController_ArmsDealer.WeaponOption weaponOption = this.allWeapons.Find((DialogueController_ArmsDealer.WeaponOption x) => x.Name == choiceLabel);
			if (weaponOption != null)
			{
				if (!weaponOption.IsAvailable)
				{
					invalidReason = weaponOption.NotAvailableReason;
					return false;
				}
				if (weaponOption.Item.RequiresLevelToPurchase && NetworkSingleton<LevelManager>.Instance.GetFullRank() < weaponOption.Item.RequiredRank)
				{
					string str = "Available at ";
					FullRank requiredRank = weaponOption.Item.RequiredRank;
					invalidReason = str + requiredRank.ToString();
					return false;
				}
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < weaponOption.Price)
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

		// Token: 0x0600302F RID: 12335 RVA: 0x000CA30C File Offset: 0x000C850C
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE" && this.chosenWeapon != null)
			{
				dialogueText = dialogueText.Replace("<WEAPON>", this.chosenWeapon.Name);
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.chosenWeapon.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x040021E5 RID: 8677
		public List<DialogueController_ArmsDealer.WeaponOption> MeleeWeapons;

		// Token: 0x040021E6 RID: 8678
		public List<DialogueController_ArmsDealer.WeaponOption> RangedWeapons;

		// Token: 0x040021E7 RID: 8679
		public List<DialogueController_ArmsDealer.WeaponOption> Ammo;

		// Token: 0x040021E8 RID: 8680
		private List<DialogueController_ArmsDealer.WeaponOption> allWeapons;

		// Token: 0x040021E9 RID: 8681
		private DialogueController_ArmsDealer.WeaponOption chosenWeapon;

		// Token: 0x020006D7 RID: 1751
		[Serializable]
		public class WeaponOption
		{
			// Token: 0x040021EA RID: 8682
			public string Name;

			// Token: 0x040021EB RID: 8683
			public float Price;

			// Token: 0x040021EC RID: 8684
			public bool IsAvailable;

			// Token: 0x040021ED RID: 8685
			public string NotAvailableReason;

			// Token: 0x040021EE RID: 8686
			public StorableItemDefinition Item;
		}
	}
}
