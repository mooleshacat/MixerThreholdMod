using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004F1 RID: 1265
	public class Jeremy : NPC
	{
		// Token: 0x06001BC7 RID: 7111 RVA: 0x000760E3 File Offset: 0x000742E3
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00076106 File Offset: 0x00074306
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0007613B File Offset: 0x0007433B
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x00076170 File Offset: 0x00074370
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000761E2 File Offset: 0x000743E2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000761FB File Offset: 0x000743FB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeremyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00076214 File Offset: 0x00074414
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00076222 File Offset: 0x00074422
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001737 RID: 5943
		public Dealership Dealership;

		// Token: 0x04001738 RID: 5944
		public List<Jeremy.DealershipListing> Listings = new List<Jeremy.DealershipListing>();

		// Token: 0x04001739 RID: 5945
		public DialogueContainer GreetingDialogue;

		// Token: 0x0400173A RID: 5946
		public string GreetedVariable = "JeremyGreeted";

		// Token: 0x0400173B RID: 5947
		private bool dll_Excuted;

		// Token: 0x0400173C RID: 5948
		private bool dll_Excuted;

		// Token: 0x020004F2 RID: 1266
		[Serializable]
		public class DealershipListing
		{
			// Token: 0x17000481 RID: 1153
			// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x00076236 File Offset: 0x00074436
			public string vehicleName
			{
				get
				{
					return NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(this.vehicleCode).VehicleName;
				}
			}

			// Token: 0x17000482 RID: 1154
			// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x0007624D File Offset: 0x0007444D
			public float price
			{
				get
				{
					return NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(this.vehicleCode).VehiclePrice;
				}
			}

			// Token: 0x0400173D RID: 5949
			public string vehicleCode = string.Empty;
		}
	}
}
