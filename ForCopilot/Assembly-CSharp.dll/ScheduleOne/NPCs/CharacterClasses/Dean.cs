using System;
using ScheduleOne.Dialogue;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004DC RID: 1244
	public class Dean : NPC
	{
		// Token: 0x06001B52 RID: 6994 RVA: 0x00075786 File Offset: 0x00073986
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Dean_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00075416 File Offset: 0x00073616
		public bool TattooChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0007579A File Offset: 0x0007399A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000757B3 File Offset: 0x000739B3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000757CC File Offset: 0x000739CC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000757DA File Offset: 0x000739DA
		protected override void dll()
		{
			base.Awake();
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.TattooChoiceValid);
		}

		// Token: 0x04001703 RID: 5891
		private bool dll_Excuted;

		// Token: 0x04001704 RID: 5892
		private bool dll_Excuted;
	}
}
