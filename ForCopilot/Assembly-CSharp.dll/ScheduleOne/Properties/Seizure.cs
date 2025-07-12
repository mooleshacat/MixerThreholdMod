using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000329 RID: 809
	[CreateAssetMenu(fileName = "Seizure", menuName = "Properties/Seizure Property")]
	public class Seizure : Property
	{
		// Token: 0x060011E7 RID: 4583 RVA: 0x0004E47C File Offset: 0x0004C67C
		public override void ApplyToNPC(NPC npc)
		{
			Seizure.<>c__DisplayClass3_0 CS$<>8__locals1 = new Seizure.<>c__DisplayClass3_0();
			CS$<>8__locals1.npc = npc;
			CS$<>8__locals1.npc.PlayVO(EVOLineType.Hurt);
			CS$<>8__locals1.npc.behaviour.RagdollBehaviour.Seizure = true;
			CS$<>8__locals1.npc.Movement.ActivateRagdoll_Server();
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ApplyToNPC>g__Wait|0());
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004E4DC File Offset: 0x0004C6DC
		public override void ApplyToPlayer(Player player)
		{
			Seizure.<>c__DisplayClass4_0 CS$<>8__locals1 = new Seizure.<>c__DisplayClass4_0();
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.player.Seizure = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ApplyToPlayer>g__Wait|0());
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004E513 File Offset: 0x0004C713
		public override void ClearFromNPC(NPC npc)
		{
			npc.behaviour.RagdollBehaviour.Seizure = false;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004E526 File Offset: 0x0004C726
		public override void ClearFromPlayer(Player player)
		{
			player.Seizure = false;
		}

		// Token: 0x04001167 RID: 4455
		public const float CAMERA_JITTER_INTENSITY = 1f;

		// Token: 0x04001168 RID: 4456
		public const float DURATION_NPC = 60f;

		// Token: 0x04001169 RID: 4457
		public const float DURATION_PLAYER = 30f;
	}
}
