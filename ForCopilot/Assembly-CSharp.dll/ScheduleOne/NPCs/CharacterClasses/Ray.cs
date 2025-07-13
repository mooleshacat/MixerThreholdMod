using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000510 RID: 1296
	public class Ray : NPC
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x000770EE File Offset: 0x000752EE
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00077111 File Offset: 0x00075311
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00077146 File Offset: 0x00075346
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0007717C File Offset: 0x0007537C
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00077201 File Offset: 0x00075401
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0007721A File Offset: 0x0007541A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RayAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00077233 File Offset: 0x00075433
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00077241 File Offset: 0x00075441
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001786 RID: 6022
		public DialogueContainer GreetingDialogue;

		// Token: 0x04001787 RID: 6023
		public string GreetedVariable = "RayGreeted";

		// Token: 0x04001788 RID: 6024
		public string IntroductionMessage;

		// Token: 0x04001789 RID: 6025
		public string IntroSentVariable = "RayIntroSent";

		// Token: 0x0400178A RID: 6026
		[Header("Intro message conditions")]
		public FullRank IntroRank;

		// Token: 0x0400178B RID: 6027
		public int IntroDaysPlayed = 21;

		// Token: 0x0400178C RID: 6028
		public float IntroNetworth = 15000f;

		// Token: 0x0400178D RID: 6029
		private bool dll_Excuted;

		// Token: 0x0400178E RID: 6030
		private bool dll_Excuted;
	}
}
