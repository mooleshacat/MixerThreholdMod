using System;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006F8 RID: 1784
	public class DialogueHandler_Police : ControlledDialogueHandler
	{
		// Token: 0x060030CA RID: 12490 RVA: 0x000CC4CF File Offset: 0x000CA6CF
		protected override void Awake()
		{
			base.Awake();
			this.officer = (base.NPC as PoliceOfficer);
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000CC4E8 File Offset: 0x000CA6E8
		public override void Hovered()
		{
			base.Hovered();
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000CC4F0 File Offset: 0x000CA6F0
		public override void Interacted()
		{
			base.Interacted();
			if (this.CanTalk_Checkpoint())
			{
				this.officer.PlayVO(EVOLineType.Question);
				base.InitializeDialogue(this.CheckpointRequestDialogue.name, true, "ENTRY");
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000CC524 File Offset: 0x000CA724
		private bool CanTalk_Checkpoint()
		{
			return this.officer.behaviour.activeBehaviour != null && this.officer.behaviour.activeBehaviour is CheckpointBehaviour && !(this.officer.behaviour.activeBehaviour as CheckpointBehaviour).IsSearching;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000CC580 File Offset: 0x000CA780
		protected override int CheckBranch(string branchLabel)
		{
			if (!(branchLabel == "BRANCH_VEHICLE_EXISTS"))
			{
				return base.CheckBranch(branchLabel);
			}
			LandVehicle lastDrivenVehicle = Player.Local.LastDrivenVehicle;
			CheckpointBehaviour checkpointBehaviour = this.officer.CheckpointBehaviour;
			if (lastDrivenVehicle != null && (checkpointBehaviour.Checkpoint.SearchArea1.vehicles.Contains(lastDrivenVehicle) || checkpointBehaviour.Checkpoint.SearchArea2.vehicles.Contains(lastDrivenVehicle)))
			{
				checkpointBehaviour.StartSearch(lastDrivenVehicle.NetworkObject, Player.Local.NetworkObject);
				return 1;
			}
			return 0;
		}

		// Token: 0x0400223F RID: 8767
		[Header("References")]
		public DialogueContainer CheckpointRequestDialogue;

		// Token: 0x04002240 RID: 8768
		private PoliceOfficer officer;
	}
}
