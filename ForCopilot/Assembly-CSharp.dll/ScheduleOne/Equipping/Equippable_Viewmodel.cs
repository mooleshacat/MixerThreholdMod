using System;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000968 RID: 2408
	public class Equippable_Viewmodel : Equippable_StorableItem
	{
		// Token: 0x060040F9 RID: 16633 RVA: 0x00112C80 File Offset: 0x00110E80
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			base.transform.localPosition = this.localPosition;
			base.transform.localEulerAngles = this.localEulerAngles;
			base.transform.localScale = this.localScale;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Viewmodel"));
			foreach (MeshRenderer meshRenderer in base.gameObject.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					meshRenderer.enabled = false;
				}
				else
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			this.PlayEquipAnimation();
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00112D19 File Offset: 0x00110F19
		public override void Unequip()
		{
			base.Unequip();
			this.PlayUnequipAnimation();
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00112D27 File Offset: 0x00110F27
		protected virtual void PlayEquipAnimation()
		{
			if (this.AvatarEquippable != null)
			{
				Player.Local.SendEquippable_Networked(this.AvatarEquippable.AssetPath);
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00112D4C File Offset: 0x00110F4C
		protected virtual void PlayUnequipAnimation()
		{
			if (this.AvatarEquippable != null)
			{
				Player.Local.SendEquippable_Networked(string.Empty);
			}
		}

		// Token: 0x04002E62 RID: 11874
		[Header("Viewmodel settings")]
		public Vector3 localPosition;

		// Token: 0x04002E63 RID: 11875
		public Vector3 localEulerAngles;

		// Token: 0x04002E64 RID: 11876
		public Vector3 localScale = Vector3.one;

		// Token: 0x04002E65 RID: 11877
		[Header("Third person animation settings")]
		public AvatarEquippable AvatarEquippable;
	}
}
