using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000C5B RID: 3163
	public class Equippable_Cash : Equippable_Viewmodel
	{
		// Token: 0x06005916 RID: 22806 RVA: 0x001785C1 File Offset: 0x001767C1
		protected override void Update()
		{
			base.Update();
			if (!this.lookingAtStorageObject && GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.amountIndex++;
			}
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x001785E7 File Offset: 0x001767E7
		protected override void StartBuildingStoredItem()
		{
			this.isBuildingStoredItem = true;
			Singleton<BuildManager>.Instance.StartPlacingCash(this.itemInstance);
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x00178600 File Offset: 0x00176800
		protected override void StopBuildingStoredItem()
		{
			this.isBuildingStoredItem = false;
			Singleton<BuildManager>.Instance.StopBuilding();
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x00178613 File Offset: 0x00176813
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			item.onDataChanged = (Action)Delegate.Combine(item.onDataChanged, new Action(this.UpdateCashVisuals));
			this.UpdateCashVisuals();
		}

		// Token: 0x0600591A RID: 22810 RVA: 0x00178644 File Offset: 0x00176844
		public override void Unequip()
		{
			base.Unequip();
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateCashVisuals));
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x00178674 File Offset: 0x00176874
		private void UpdateCashVisuals()
		{
			CashInstance cashInstance = this.itemInstance as CashInstance;
			if (cashInstance == null)
			{
				this.Container_100_300.gameObject.SetActive(false);
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_Under100.gameObject.SetActive(false);
				return;
			}
			float num = cashInstance.Balance;
			if (num < 100f)
			{
				num = Mathf.Round(num / 10f) * 10f;
				int num2 = Mathf.Clamp(Mathf.RoundToInt(num / 10f), 0, 10);
				if (num > 0f)
				{
					num2 = Mathf.Max(1, num2);
				}
				this.Container_100_300.gameObject.SetActive(false);
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_Under100.gameObject.SetActive(true);
				for (int i = 0; i < this.SingleNotes.Count; i++)
				{
					if (i < num2)
					{
						this.SingleNotes[i].gameObject.SetActive(true);
					}
					else
					{
						this.SingleNotes[i].gameObject.SetActive(false);
					}
				}
				return;
			}
			num = Mathf.Floor(num / 100f) * 100f;
			this.Container_Under100.gameObject.SetActive(false);
			if (num < 400f)
			{
				this.Container_300Plus.gameObject.SetActive(false);
				this.Container_100_300.gameObject.SetActive(true);
				for (int j = 0; j < this.Under300Stacks.Count; j++)
				{
					if ((float)j < num / 100f)
					{
						this.Under300Stacks[j].gameObject.SetActive(true);
					}
					else
					{
						this.Under300Stacks[j].gameObject.SetActive(false);
					}
				}
				return;
			}
			this.Container_100_300.gameObject.SetActive(false);
			this.Container_300Plus.gameObject.SetActive(true);
			for (int k = 0; k < this.PlusStacks.Count; k++)
			{
				if ((float)k < num / 100f)
				{
					this.PlusStacks[k].gameObject.SetActive(true);
				}
				else
				{
					this.PlusStacks[k].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0400413C RID: 16700
		private int amountIndex;

		// Token: 0x0400413D RID: 16701
		[Header("References")]
		public Transform Container_Under100;

		// Token: 0x0400413E RID: 16702
		public List<Transform> SingleNotes;

		// Token: 0x0400413F RID: 16703
		public Transform Container_100_300;

		// Token: 0x04004140 RID: 16704
		public List<Transform> Under300Stacks;

		// Token: 0x04004141 RID: 16705
		public Transform Container_300Plus;

		// Token: 0x04004142 RID: 16706
		public List<Transform> PlusStacks;
	}
}
