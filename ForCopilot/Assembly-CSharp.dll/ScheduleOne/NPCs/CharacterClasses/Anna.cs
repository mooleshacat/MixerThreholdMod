using System;
using ScheduleOne.Dialogue;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D4 RID: 1236
	public class Anna : NPC
	{
		// Token: 0x06001B26 RID: 6950 RVA: 0x00075402 File Offset: 0x00073602
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Anna_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00075416 File Offset: 0x00073616
		public bool HairCutChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00075420 File Offset: 0x00073620
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00075439 File Offset: 0x00073639
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00075452 File Offset: 0x00073652
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00075460 File Offset: 0x00073660
		protected override void dll()
		{
			base.Awake();
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.HairCutChoiceValid);
		}

		// Token: 0x040016F0 RID: 5872
		private bool dll_Excuted;

		// Token: 0x040016F1 RID: 5873
		private bool dll_Excuted;
	}
}
