using System;
using FishNet;
using ScheduleOne.Combat;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Employees
{
	// Token: 0x0200068B RID: 1675
	public class NPCResponses_Employee : NPCResponses
	{
		// Token: 0x06002CFD RID: 11517 RVA: 0x000B96BE File Offset: 0x000B78BE
		protected override void RespondToFirstNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToFirstNonLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000B96CF File Offset: 0x000B78CF
		protected override void RespondToLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000B96E0 File Offset: 0x000B78E0
		protected override void RespondToRepeatedNonLethalAttack(Player perpetrator, Impact impact)
		{
			base.RespondToRepeatedNonLethalAttack(perpetrator, impact);
			this.Ow(perpetrator);
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000B96F4 File Offset: 0x000B78F4
		private void Ow(Player perpetrator)
		{
			base.npc.dialogueHandler.PlayReaction("hurt", 2.5f, false);
			base.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "hurt", 20f, 3);
			if (InstanceFinder.IsServer)
			{
				base.npc.behaviour.FacePlayerBehaviour.SetTarget(perpetrator.NetworkObject, 5f);
				base.npc.behaviour.FacePlayerBehaviour.Enable_Networked(null);
			}
		}
	}
}
