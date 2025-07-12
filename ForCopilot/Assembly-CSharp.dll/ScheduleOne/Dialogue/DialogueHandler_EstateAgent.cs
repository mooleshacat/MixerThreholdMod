using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006F3 RID: 1779
	public class DialogueHandler_EstateAgent : ControlledDialogueHandler
	{
		// Token: 0x060030B7 RID: 12471 RVA: 0x000CC06C File Offset: 0x000CA26C
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (property != null && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < property.Price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			if (business != null && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < business.Price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000CC10C File Offset: 0x000CA30C
		public override bool ShouldChoiceBeShown(string choiceLabel)
		{
			Property property = Property.OwnedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.OwnedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			return (!(property != null) || !property.IsOwned) && (!(business != null) || !business.IsOwned) && base.ShouldChoiceBeShown(choiceLabel);
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000CC188 File Offset: 0x000CA388
		protected override void ChoiceCallback(string choiceLabel)
		{
			Property x3 = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business x2 = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (x3 != null)
			{
				this.selectedProperty = x3;
			}
			if (x2 != null)
			{
				this.selectedBusiness = x2;
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000CC1FC File Offset: 0x000CA3FC
		protected override void DialogueCallback(string choiceLabel)
		{
			if (choiceLabel == "CONFIRM_BUY")
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedProperty.PropertyName + " purchase", -this.selectedProperty.Price, 1f, string.Empty);
				this.selectedProperty.SetOwned();
			}
			if (choiceLabel == "CONFIRM_BUY_BUSINESS")
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedBusiness.PropertyName + " purchase", -this.selectedBusiness.Price, 1f, string.Empty);
				this.selectedBusiness.SetOwned();
			}
			base.DialogueCallback(choiceLabel);
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000CC2AC File Offset: 0x000CA4AC
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "CONFIRM")
			{
				return dialogueText.Replace("<PROPERTY>", this.selectedProperty.PropertyName.ToLower());
			}
			if (dialogueLabel == "CONFIRM_BUSINESS")
			{
				return dialogueText.Replace("<BUSINESS>", this.selectedBusiness.PropertyName.ToLower());
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000CC314 File Offset: 0x000CA514
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (property != null)
			{
				return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(property.Price, false, false) + ")</color>");
			}
			if (business != null)
			{
				return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(business.Price, false, false) + ")</color>");
			}
			if (choiceLabel == "CONFIRM_CHOICE")
			{
				if (this.selectedProperty != null)
				{
					return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedProperty.Price, false, false) + ")</color>");
				}
				if (this.selectedBusiness != null)
				{
					return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedBusiness.Price, false, false) + ")</color>");
				}
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x04002239 RID: 8761
		private Property selectedProperty;

		// Token: 0x0400223A RID: 8762
		private Business selectedBusiness;
	}
}
