using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Variables;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000505 RID: 1285
	public class Mick : NPC
	{
		// Token: 0x06001C3B RID: 7227 RVA: 0x00076BA3 File Offset: 0x00074DA3
		protected override void Start()
		{
			base.Start();
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.CanPawn);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00076BD2 File Offset: 0x00074DD2
		private bool CanPawn(out string reason)
		{
			reason = string.Empty;
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("PawnShopAngeredToday"))
			{
				reason = "Mick doesn't want to do business with you right now.";
				return false;
			}
			return true;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00076BF6 File Offset: 0x00074DF6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00076C0F File Offset: 0x00074E0F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00076C28 File Offset: 0x00074E28
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00076C36 File Offset: 0x00074E36
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400176A RID: 5994
		private bool dll_Excuted;

		// Token: 0x0400176B RID: 5995
		private bool dll_Excuted;
	}
}
