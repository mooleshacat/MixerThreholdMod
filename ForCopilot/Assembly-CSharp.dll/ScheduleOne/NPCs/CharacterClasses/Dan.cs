using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DB RID: 1243
	public class Dan : NPC
	{
		// Token: 0x06001B4B RID: 6987 RVA: 0x0007568F File Offset: 0x0007388F
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000756B4 File Offset: 0x000738B4
		private void OrderCompleted()
		{
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("Dan_Greeting_Done"))
			{
				base.PlayVO(EVOLineType.Thanks);
				this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
				return;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Dan_Greeting_Done", true.ToString(), true);
			base.PlayVO(EVOLineType.Question);
			if (this.onGreeting != null)
			{
				this.onGreeting.Invoke();
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00075732 File Offset: 0x00073932
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0007574B File Offset: 0x0007394B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00075764 File Offset: 0x00073964
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00075772 File Offset: 0x00073972
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016FE RID: 5886
		public ShopInterface ShopInterface;

		// Token: 0x040016FF RID: 5887
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x04001700 RID: 5888
		public UnityEvent onGreeting;

		// Token: 0x04001701 RID: 5889
		private bool dll_Excuted;

		// Token: 0x04001702 RID: 5890
		private bool dll_Excuted;
	}
}
