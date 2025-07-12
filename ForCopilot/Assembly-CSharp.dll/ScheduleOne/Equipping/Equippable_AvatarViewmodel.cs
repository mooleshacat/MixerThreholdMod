using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200095B RID: 2395
	public class Equippable_AvatarViewmodel : Equippable_Viewmodel
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x0011106B File Offset: 0x0010F26B
		protected bool equipAnimDone
		{
			get
			{
				return this.timeEquipped >= this.EquipTime;
			}
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x00111080 File Offset: 0x0010F280
		public override void Equip(ItemInstance item)
		{
			base.transform.SetParent(Singleton<ViewmodelAvatar>.Instance.RightHandContainer);
			if (this.AnimatorController != null)
			{
				Singleton<ViewmodelAvatar>.Instance.SetAnimatorController(this.AnimatorController);
				Singleton<ViewmodelAvatar>.Instance.SetVisibility(true);
				Singleton<ViewmodelAvatar>.Instance.SetOffset(this.ViewmodelAvatarOffset);
			}
			base.Equip(item);
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x001110E2 File Offset: 0x0010F2E2
		public override void Unequip()
		{
			base.Unequip();
			Singleton<ViewmodelAvatar>.Instance.SetVisibility(false);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x001110F5 File Offset: 0x0010F2F5
		protected override void PlayEquipAnimation()
		{
			base.PlayEquipAnimation();
			if (this.EquipTrigger != string.Empty)
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.EquipTrigger);
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00111124 File Offset: 0x0010F324
		protected override void Update()
		{
			base.Update();
			this.timeEquipped += Time.deltaTime;
		}

		// Token: 0x04002DFE RID: 11774
		public RuntimeAnimatorController AnimatorController;

		// Token: 0x04002DFF RID: 11775
		public Vector3 ViewmodelAvatarOffset = Vector3.zero;

		// Token: 0x04002E00 RID: 11776
		[Header("Equipping")]
		public float EquipTime = 0.4f;

		// Token: 0x04002E01 RID: 11777
		public string EquipTrigger = "Equip";

		// Token: 0x04002E02 RID: 11778
		protected float timeEquipped;
	}
}
