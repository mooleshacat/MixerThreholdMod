using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A8B RID: 2699
	public class ButtonRequireInputFields : MonoBehaviour
	{
		// Token: 0x0600489B RID: 18587 RVA: 0x00130B20 File Offset: 0x0012ED20
		public void Update()
		{
			this.Button.interactable = true;
			if (this.Dropdown != null && this.Dropdown.value == 0)
			{
				this.Button.interactable = false;
			}
			foreach (ButtonRequireInputFields.Input input in this.Inputs)
			{
				if (string.IsNullOrEmpty(input.InputField.text))
				{
					input.ErrorMessage.gameObject.SetActive(true);
					this.Button.interactable = false;
				}
				else
				{
					input.ErrorMessage.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0400353E RID: 13630
		public List<ButtonRequireInputFields.Input> Inputs;

		// Token: 0x0400353F RID: 13631
		public TMP_Dropdown Dropdown;

		// Token: 0x04003540 RID: 13632
		public Button Button;

		// Token: 0x02000A8C RID: 2700
		[Serializable]
		public class Input
		{
			// Token: 0x04003541 RID: 13633
			public TMP_InputField InputField;

			// Token: 0x04003542 RID: 13634
			public RectTransform ErrorMessage;
		}
	}
}
