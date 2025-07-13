using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C6A RID: 3178
	public class AutoshopAccessZone : NPCPresenceAccessZone
	{
		// Token: 0x06005965 RID: 22885 RVA: 0x00179C3A File Offset: 0x00177E3A
		public override void SetIsOpen(bool open)
		{
			base.SetIsOpen(open);
			if (this.rollerDoorOpen != open)
			{
				this.rollerDoorOpen = open;
				this.RollerDoorAnim.Play(this.rollerDoorOpen ? "Roller door open" : "Roller door close");
			}
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x00179C74 File Offset: 0x00177E74
		protected override void MinPass()
		{
			if (this.TargetNPC == null)
			{
				return;
			}
			this.SetIsOpen(this.DetectionZone.bounds.Contains(this.TargetNPC.Avatar.CenterPoint) || this.VehicleDetection.closestVehicle != null);
		}

		// Token: 0x04004190 RID: 16784
		public Animation RollerDoorAnim;

		// Token: 0x04004191 RID: 16785
		public VehicleDetector VehicleDetection;

		// Token: 0x04004192 RID: 16786
		private bool rollerDoorOpen = true;
	}
}
