using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006DA RID: 1754
	public class DialogueController_Dan : DialogueController
	{
		// Token: 0x06003036 RID: 12342 RVA: 0x000CA3AA File Offset: 0x000C85AA
		protected override void Start()
		{
			base.Start();
			if (this.ItemToGive == null)
			{
				Debug.LogWarning("ItemToGive is not set in the inspector.");
			}
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000CA3CA File Offset: 0x000C85CA
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "GIVE_ITEM" && this.ItemToGive != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemToGive.GetDefaultInstance(1));
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x040021F1 RID: 8689
		public ItemDefinition ItemToGive;
	}
}
