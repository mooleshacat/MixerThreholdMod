using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Property;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006DC RID: 1756
	public class DialogueController_Employee : DialogueController
	{
		// Token: 0x06003041 RID: 12353 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000CA5EC File Offset: 0x000C87EC
		public override void ChoiceCallback(string choiceLabel)
		{
			Property propertyByCode = this.GetPropertyByCode(choiceLabel);
			if (propertyByCode != null)
			{
				this.selectedProperty = propertyByCode;
				this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
			}
			if (choiceLabel == "CONFIRM" && this.selectedProperty != null)
			{
				(this.npc as Employee).SendTransfer(this.selectedProperty.PropertyCode);
				this.npc.dialogueHandler.ShowWorldspaceDialogue("Ok boss, I'll head over there shortly.", 5f);
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000CA682 File Offset: 0x000C8882
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "ENTRY" && DialogueHandler.activeDialogue.name == "Employee_Transfer")
			{
				existingChoices.AddRange(this.GetChoices());
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000CA6BC File Offset: 0x000C88BC
		private List<DialogueChoiceData> GetChoices()
		{
			List<DialogueChoiceData> list = new List<DialogueChoiceData>();
			foreach (Property property in Property.OwnedProperties)
			{
				if (property.EmployeeCapacity > 0 && !(property == (this.npc as Employee).AssignedProperty))
				{
					list.Add(new DialogueChoiceData
					{
						ChoiceText = string.Concat(new string[]
						{
							property.PropertyName,
							" (",
							property.Employees.Count.ToString(),
							"/",
							property.EmployeeCapacity.ToString(),
							")"
						}),
						ChoiceLabel = property.PropertyCode
					});
				}
			}
			list.Sort(delegate(DialogueChoiceData x, DialogueChoiceData y)
			{
				Property propertyByCode = this.GetPropertyByCode(x.ChoiceLabel);
				Property propertyByCode2 = this.GetPropertyByCode(y.ChoiceLabel);
				if (propertyByCode == null || propertyByCode2 == null)
				{
					return 0;
				}
				int value = propertyByCode.EmployeeCapacity - propertyByCode.Employees.Count;
				return (propertyByCode2.EmployeeCapacity - propertyByCode2.Employees.Count).CompareTo(value);
			});
			list.Add(new DialogueChoiceData
			{
				ChoiceText = "Nevermind",
				ChoiceLabel = string.Empty
			});
			return list;
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x000CA7DC File Offset: 0x000C89DC
		private Property GetPropertyByCode(string code)
		{
			return Singleton<PropertyManager>.Instance.GetProperty(code);
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x000CA7EC File Offset: 0x000C89EC
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			Property propertyByCode = this.GetPropertyByCode(choiceLabel);
			if (propertyByCode != null && propertyByCode.Employees.Count >= propertyByCode.EmployeeCapacity)
			{
				invalidReason = "ALREADY AT MAX CAPACITY";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000CA82E File Offset: 0x000C8A2E
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE" && this.selectedProperty != null)
			{
				dialogueText = dialogueText.Replace("<LOCATION>", this.selectedProperty.PropertyName);
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x040021F3 RID: 8691
		private Property selectedProperty;
	}
}
