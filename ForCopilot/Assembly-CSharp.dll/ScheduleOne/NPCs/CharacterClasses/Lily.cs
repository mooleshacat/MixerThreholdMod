using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004FC RID: 1276
	public class Lily : NPC
	{
		// Token: 0x06001C00 RID: 7168 RVA: 0x0007656B File Offset: 0x0007476B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Lily_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0007657F File Offset: 0x0007477F
		private void Unlocked(NPCRelationData.EUnlockType type, bool b)
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Lily_Unlocked", "true", true);
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00076596 File Offset: 0x00074796
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0007659E File Offset: 0x0007479E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000765B7 File Offset: 0x000747B7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000765D0 File Offset: 0x000747D0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000765DE File Offset: 0x000747DE
		protected override void dll()
		{
			base.Awake();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.Unlocked));
		}

		// Token: 0x04001750 RID: 5968
		[Header("References")]
		public Transform TutorialScheduleGroup;

		// Token: 0x04001751 RID: 5969
		public Transform RegularScheduleGroup;

		// Token: 0x04001752 RID: 5970
		public Conditions TutorialConditions;

		// Token: 0x04001753 RID: 5971
		private bool dll_Excuted;

		// Token: 0x04001754 RID: 5972
		private bool dll_Excuted;
	}
}
