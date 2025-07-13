using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200048B RID: 1163
	public class Stan : NPC
	{
		// Token: 0x060016F9 RID: 5881 RVA: 0x00065357 File Offset: 0x00063557
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x0006537A File Offset: 0x0006357A
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x000653AF File Offset: 0x000635AF
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x000653E4 File Offset: 0x000635E4
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x0006544B File Offset: 0x0006364B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00065464 File Offset: 0x00063664
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.StanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0006547D File Offset: 0x0006367D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0006548B File Offset: 0x0006368B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400151C RID: 5404
		public DialogueContainer GreetingDialogue;

		// Token: 0x0400151D RID: 5405
		public string GreetedVariable = "StanGreeted";

		// Token: 0x0400151E RID: 5406
		private bool dll_Excuted;

		// Token: 0x0400151F RID: 5407
		private bool dll_Excuted;
	}
}
