using System;
using ScheduleOne.UI.Shop;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000513 RID: 1299
	public class Steve : NPC
	{
		// Token: 0x06001C93 RID: 7315 RVA: 0x0007746D File Offset: 0x0007566D
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00077491 File Offset: 0x00075691
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000774BF File Offset: 0x000756BF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000774D8 File Offset: 0x000756D8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SteveAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000774F1 File Offset: 0x000756F1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000774FF File Offset: 0x000756FF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001794 RID: 6036
		public ShopInterface ShopInterface;

		// Token: 0x04001795 RID: 6037
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x04001796 RID: 6038
		private bool dll_Excuted;

		// Token: 0x04001797 RID: 6039
		private bool dll_Excuted;
	}
}
