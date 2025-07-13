using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.NPCs.CharacterClasses;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006F9 RID: 1785
	public class DialogueHandler_VehicleSalesman : ControlledDialogueHandler
	{
		// Token: 0x060030D0 RID: 12496 RVA: 0x000CC60C File Offset: 0x000CA80C
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			Jeremy.DealershipListing dealershipListing = this.Salesman.Listings.Find((Jeremy.DealershipListing x) => x.vehicleCode.ToLower() == choiceLabel.ToLower());
			if (choiceLabel == "BUY_CASH")
			{
				if (this.selectedVehicle != null)
				{
					choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedVehicle.price, false, false) + ")</color>");
				}
			}
			else if (choiceLabel == "BUY_ONLINE")
			{
				if (this.selectedVehicle != null)
				{
					choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedVehicle.price, false, false) + ")</color>");
				}
			}
			else if (dealershipListing != null)
			{
				choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(dealershipListing.price, false, false) + ")</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000CC714 File Offset: 0x000CA914
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "BUY_CASH")
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < this.selectedVehicle.price)
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			else if (choiceLabel == "BUY_ONLINE" && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.selectedVehicle.price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000CC784 File Offset: 0x000CA984
		protected override void ChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "BUY_CASH")
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.selectedVehicle.price)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.selectedVehicle.price, true, false);
					this.Salesman.Dealership.SpawnVehicle(this.selectedVehicle.vehicleCode);
					return;
				}
			}
			else if (choiceLabel == "BUY_ONLINE")
			{
				if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.selectedVehicle.price)
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedVehicle.vehicleCode + " purchase", -this.selectedVehicle.price, 1f, string.Empty);
					this.Salesman.Dealership.SpawnVehicle(this.selectedVehicle.vehicleCode);
					return;
				}
			}
			else
			{
				Jeremy.DealershipListing dealershipListing = this.Salesman.Listings.Find((Jeremy.DealershipListing x) => x.vehicleCode.ToLower() == choiceLabel.ToLower());
				if (dealershipListing != null)
				{
					this.selectedVehicle = dealershipListing;
				}
				base.ChoiceCallback(choiceLabel);
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000CC8AE File Offset: 0x000CAAAE
		protected override int CheckBranch(string branchLabel)
		{
			if (!(branchLabel == "BRANCH_CAN_AFFORD"))
			{
				return base.CheckBranch(branchLabel);
			}
			if (this.selectedVehicle == null)
			{
				return 0;
			}
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.selectedVehicle.price)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000CC8E9 File Offset: 0x000CAAE9
		protected override void DialogueCallback(string choiceLabel)
		{
			base.DialogueCallback(choiceLabel);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000CC8F2 File Offset: 0x000CAAF2
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "CONFIRM")
			{
				return dialogueText.Replace("<VEHICLE>", this.selectedVehicle.vehicleName);
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x04002241 RID: 8769
		public Jeremy Salesman;

		// Token: 0x04002242 RID: 8770
		public Jeremy.DealershipListing selectedVehicle;
	}
}
