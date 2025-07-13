using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004E1 RID: 1249
	public class Fixer : NPC
	{
		// Token: 0x06001B70 RID: 7024 RVA: 0x000759E0 File Offset: 0x00073BE0
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x00075A03 File Offset: 0x00073C03
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x00075A38 File Offset: 0x00073C38
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00075A6C File Offset: 0x00073C6C
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00075AC0 File Offset: 0x00073CC0
		public static float GetAdditionalSigningFee()
		{
			int num = Mathf.RoundToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LifetimeEmployeesRecruited"));
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				if (i <= 5)
				{
					num2 += 100f;
				}
				else
				{
					num2 += 250f;
				}
			}
			return Mathf.Min(num2, 500f);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x00075B28 File Offset: 0x00073D28
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00075B41 File Offset: 0x00073D41
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FixerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x00075B5A File Offset: 0x00073D5A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00075B68 File Offset: 0x00073D68
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400170F RID: 5903
		public const int ADDITIONAL_SIGNING_FEE_1 = 100;

		// Token: 0x04001710 RID: 5904
		public const int ADDITIONAL_SIGNING_FEE_2 = 250;

		// Token: 0x04001711 RID: 5905
		public const int MAX_SIGNING_FEE = 500;

		// Token: 0x04001712 RID: 5906
		public const int ADDITIONAL_FEE_THRESHOLD = 5;

		// Token: 0x04001713 RID: 5907
		public DialogueContainer GreetingDialogue;

		// Token: 0x04001714 RID: 5908
		public string GreetedVariable = "FixerGreeted";

		// Token: 0x04001715 RID: 5909
		private bool dll_Excuted;

		// Token: 0x04001716 RID: 5910
		private bool dll_Excuted;
	}
}
