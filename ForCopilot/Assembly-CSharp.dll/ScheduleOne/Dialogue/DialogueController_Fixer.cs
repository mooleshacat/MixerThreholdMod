using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Money;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.Property;
using ScheduleOne.Variables;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006DD RID: 1757
	public class DialogueController_Fixer : DialogueController
	{
		// Token: 0x0600304A RID: 12362 RVA: 0x000CA8D8 File Offset: 0x000C8AD8
		public override void ChoiceCallback(string choiceLabel)
		{
			base.ChoiceCallback(choiceLabel);
			if (choiceLabel == "CONFIRM" && this.selectedProperty != null)
			{
				this.Confirm();
			}
			if (!(choiceLabel == "Botanist"))
			{
				if (!(choiceLabel == "Packager"))
				{
					if (!(choiceLabel == "Chemist"))
					{
						if (choiceLabel == "Cleaner")
						{
							this.selectedEmployeeType = EEmployeeType.Cleaner;
						}
					}
					else
					{
						this.selectedEmployeeType = EEmployeeType.Chemist;
					}
				}
				else
				{
					this.selectedEmployeeType = EEmployeeType.Handler;
				}
			}
			else
			{
				this.selectedEmployeeType = EEmployeeType.Botanist;
			}
			foreach (Property property in Property.OwnedProperties)
			{
				if (!(property == null) && choiceLabel == property.PropertyCode)
				{
					this.selectedProperty = property;
					this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
					break;
				}
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000CA9E0 File Offset: 0x000C8BE0
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "SELECT_LOCATION")
			{
				foreach (Property property in Property.OwnedProperties)
				{
					if (!(property.PropertyCode == "rv") && !(property.PropertyCode == "motelroom"))
					{
						int count = property.GetUnassignedBeds().Count;
						string propertyName = property.PropertyName;
						existingChoices.Add(new DialogueChoiceData
						{
							ChoiceText = propertyName,
							ChoiceLabel = property.PropertyCode
						});
					}
				}
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000CAA9C File Offset: 0x000C8C9C
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CONFIRM")
			{
				Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee())
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			foreach (Property property in Property.OwnedProperties)
			{
				if (choiceLabel == property.PropertyCode && property.Employees.Count >= property.EmployeeCapacity)
				{
					invalidReason = string.Concat(new string[]
					{
						"Employee limit reached (",
						property.Employees.Count.ToString(),
						"/",
						property.EmployeeCapacity.ToString(),
						")"
					});
					return false;
				}
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000CABA0 File Offset: 0x000C8DA0
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE")
			{
				Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
				dialogueText = dialogueText.Replace("<SIGN_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee(), false, false) + "</color>");
				dialogueText = dialogueText.Replace("<DAILY_WAGE>", "<color=#54E717>" + MoneyManager.FormatAmount(employeePrefab.DailyWage, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000CAC2B File Offset: 0x000C8E2B
		public override bool DecideBranch(string branchLabel, out int index)
		{
			if (branchLabel == "IS_FIRST_WORKER")
			{
				if (this.lastConfirmationWasInitial)
				{
					index = 1;
				}
				else
				{
					index = 0;
				}
				return true;
			}
			return base.DecideBranch(branchLabel, out index);
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000CAC54 File Offset: 0x000C8E54
		private void Confirm()
		{
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("ClipboardAcquired"))
			{
				this.lastConfirmationWasInitial = true;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ClipboardAcquired", true.ToString(), true);
			}
			else
			{
				this.lastConfirmationWasInitial = false;
			}
			Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-(employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee()), true, false);
			NetworkSingleton<EmployeeManager>.Instance.CreateNewEmployee(this.selectedProperty, this.selectedEmployeeType);
		}

		// Token: 0x040021F4 RID: 8692
		private EEmployeeType selectedEmployeeType;

		// Token: 0x040021F5 RID: 8693
		private Property selectedProperty;

		// Token: 0x040021F6 RID: 8694
		private bool lastConfirmationWasInitial;
	}
}
