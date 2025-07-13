using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000966 RID: 2406
	public class Equippable_StorableItem : Equippable
	{
		// Token: 0x060040F0 RID: 16624 RVA: 0x00112AE6 File Offset: 0x00110CE6
		protected virtual void Update()
		{
			this.CheckLookingAtStorageObject();
			if (this.lookingAtStorageObject)
			{
				if (!this.isBuildingStoredItem)
				{
					this.StartBuildingStoredItem();
					return;
				}
			}
			else if (this.isBuildingStoredItem)
			{
				this.StopBuildingStoredItem();
			}
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00112B14 File Offset: 0x00110D14
		protected void CheckLookingAtStorageObject()
		{
			this.lookingAtStorageObject = false;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00112B28 File Offset: 0x00110D28
		public override void Unequip()
		{
			if (this.lookingAtStorageObject)
			{
				Singleton<BuildManager>.Instance.StopBuilding();
			}
			base.Unequip();
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00112B42 File Offset: 0x00110D42
		protected virtual void StartBuildingStoredItem()
		{
			this.isBuildingStoredItem = true;
			Singleton<BuildManager>.Instance.StartBuildingStoredItem(this.itemInstance);
			Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_StoredItem>().currentRotation = this.rotation;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00112B75 File Offset: 0x00110D75
		protected virtual void StopBuildingStoredItem()
		{
			this.isBuildingStoredItem = false;
			this.rotation = Singleton<BuildManager>.Instance.currentBuildHandler.GetComponent<BuildUpdate_StoredItem>().currentRotation;
			Singleton<BuildManager>.Instance.StopBuilding();
		}

		// Token: 0x04002E5E RID: 11870
		protected bool isBuildingStoredItem;

		// Token: 0x04002E5F RID: 11871
		protected bool lookingAtStorageObject;

		// Token: 0x04002E60 RID: 11872
		protected float rotation;
	}
}
