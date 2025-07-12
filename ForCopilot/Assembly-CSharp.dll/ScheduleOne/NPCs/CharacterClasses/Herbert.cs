using System;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Shop;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E8 RID: 1256
	public class Herbert : NPC
	{
		// Token: 0x06001B98 RID: 7064 RVA: 0x00075D74 File Offset: 0x00073F74
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x00075D98 File Offset: 0x00073F98
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			string text = this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)];
			text = text.Replace("formal_address", Player.Local.Avatar.GetFormalAddress(true));
			this.dialogueHandler.ShowWorldspaceDialogue(text, 5f);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00075DEF File Offset: 0x00073FEF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00075E08 File Offset: 0x00074008
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00075E21 File Offset: 0x00074021
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00075E2F File Offset: 0x0007402F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001723 RID: 5923
		public ShopInterface ShopInterface;

		// Token: 0x04001724 RID: 5924
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x04001725 RID: 5925
		private bool dll_Excuted;

		// Token: 0x04001726 RID: 5926
		private bool dll_Excuted;
	}
}
