using System;
using ScheduleOne.Dialogue;
using ScheduleOne.UI.Shop;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E0 RID: 1248
	public class Fiona : NPC
	{
		// Token: 0x06001B68 RID: 7016 RVA: 0x00075908 File Offset: 0x00073B08
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.ShopChoiceValid);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0007595E File Offset: 0x00073B5E
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x00075416 File Offset: 0x00073616
		public bool ShopChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0007598C File Offset: 0x00073B8C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000759A5 File Offset: 0x00073BA5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FionaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000759BE File Offset: 0x00073BBE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000759CC File Offset: 0x00073BCC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400170B RID: 5899
		public ShopInterface ShopInterface;

		// Token: 0x0400170C RID: 5900
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x0400170D RID: 5901
		private bool dll_Excuted;

		// Token: 0x0400170E RID: 5902
		private bool dll_Excuted;
	}
}
