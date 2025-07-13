using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000967 RID: 2407
	public class Equippable_SurfaceItem : Equippable_StorableItem
	{
		// Token: 0x060040F6 RID: 16630 RVA: 0x00112BAC File Offset: 0x00110DAC
		protected override void Update()
		{
			base.CheckLookingAtStorageObject();
			if (this.lookingAtStorageObject && this.isBuilding)
			{
				this.isBuilding = false;
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Surface>() != null)
				{
					this.rotation = Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Surface>().CurrentRotation;
				}
			}
			base.Update();
			if (!this.lookingAtStorageObject && !this.isBuilding)
			{
				this.isBuilding = true;
				Singleton<BuildManager>.Instance.StartBuilding(this.itemInstance);
				if (Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Surface>() != null)
				{
					Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_Surface>().CurrentRotation = this.rotation;
				}
			}
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00112C65 File Offset: 0x00110E65
		public override void Unequip()
		{
			if (this.isBuilding)
			{
				Singleton<BuildManager>.Instance.StopBuilding();
			}
			base.Unequip();
		}

		// Token: 0x04002E61 RID: 11873
		protected bool isBuilding;
	}
}
