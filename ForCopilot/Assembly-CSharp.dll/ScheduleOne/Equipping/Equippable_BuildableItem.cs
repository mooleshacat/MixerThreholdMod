using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200095C RID: 2396
	public class Equippable_BuildableItem : Equippable_StorableItem
	{
		// Token: 0x0600409B RID: 16539 RVA: 0x00111168 File Offset: 0x0010F368
		protected override void Update()
		{
			base.CheckLookingAtStorageObject();
			if (this.lookingAtStorageObject && this.isBuilding)
			{
				this.isBuilding = false;
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>() != null)
				{
					this.rotation = Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>().CurrentRotation;
				}
			}
			base.Update();
			if (!this.lookingAtStorageObject && !this.isBuilding)
			{
				this.isBuilding = true;
				Singleton<BuildManager>.Instance.StartBuilding(this.itemInstance);
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>() != null)
				{
					Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Grid>().CurrentRotation = this.rotation;
				}
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x00111221 File Offset: 0x0010F421
		public override void Unequip()
		{
			if (this.isBuilding)
			{
				Singleton<BuildManager>.Instance.StopBuilding();
			}
			base.Unequip();
		}

		// Token: 0x04002E03 RID: 11779
		protected bool isBuilding;
	}
}
