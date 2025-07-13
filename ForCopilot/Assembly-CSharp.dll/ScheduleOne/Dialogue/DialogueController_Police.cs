using System;
using ScheduleOne.Police;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006E0 RID: 1760
	public class DialogueController_Police : DialogueController
	{
		// Token: 0x0600305C RID: 12380 RVA: 0x000CAF31 File Offset: 0x000C9131
		protected override void Start()
		{
			base.Start();
			this.officer = (this.npc as PoliceOfficer);
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000CAF4C File Offset: 0x000C914C
		public override bool CanStartDialogue()
		{
			return !this.officer.PursuitBehaviour.Active && !this.officer.VehiclePursuitBehaviour.Active && !this.officer.BodySearchBehaviour.Active && (!this.officer.CheckpointBehaviour.Active || !this.officer.CheckpointBehaviour.IsSearching) && base.CanStartDialogue();
		}

		// Token: 0x04002201 RID: 8705
		private PoliceOfficer officer;
	}
}
