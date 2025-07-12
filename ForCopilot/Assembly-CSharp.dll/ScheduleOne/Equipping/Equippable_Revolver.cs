using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000965 RID: 2405
	public class Equippable_Revolver : Equippable_RangedWeapon
	{
		// Token: 0x060040EA RID: 16618 RVA: 0x00112A43 File Offset: 0x00110C43
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00112A5D File Offset: 0x00110C5D
		public override void Fire()
		{
			base.Fire();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00112A76 File Offset: 0x00110C76
		public override void Reload()
		{
			base.Reload();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00112A8F File Offset: 0x00110C8F
		protected override void NotifyIncrementalReload()
		{
			base.NotifyIncrementalReload();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00112AA8 File Offset: 0x00110CA8
		private void SetDisplayedBullets(int count)
		{
			for (int i = 0; i < this.Bullets.Length; i++)
			{
				this.Bullets[i].gameObject.SetActive(i < count);
			}
		}

		// Token: 0x04002E5D RID: 11869
		public Transform[] Bullets;
	}
}
