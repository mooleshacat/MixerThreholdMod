using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009B5 RID: 2485
	public class AvatarEquippable : MonoBehaviour
	{
		// Token: 0x06004362 RID: 17250 RVA: 0x0011B2CC File Offset: 0x001194CC
		[Button]
		public void RecalculateAssetPath()
		{
			this.AssetPath = AssetPathUtility.GetResourcesPath(base.gameObject);
			string[] array = this.AssetPath.Split('/', StringSplitOptions.None);
			array[array.Length - 1] = base.gameObject.name;
			this.AssetPath = string.Join("/", array);
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0011B31C File Offset: 0x0011951C
		protected virtual void Awake()
		{
			if (this.AssetPath == string.Empty)
			{
				Console.LogWarning(base.gameObject.name + " does not have an assetpath!", null);
			}
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0011B34C File Offset: 0x0011954C
		public virtual void Equip(Avatar _avatar)
		{
			this.avatar = _avatar;
			if (this.Hand == AvatarEquippable.EHand.Right)
			{
				base.transform.SetParent(this.avatar.Anim.RightHandContainer);
			}
			else
			{
				base.transform.SetParent(this.avatar.Anim.LeftHandContainer);
			}
			this.PositionAnimationModel();
			this.InitializeAnimation();
			Player componentInParent = this.avatar.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner && !componentInParent.avatarVisibleToLocalPlayer)
			{
				LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
			}
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].isTrigger = true;
			}
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0011B405 File Offset: 0x00119605
		public virtual void InitializeAnimation()
		{
			if (this.TriggerType == AvatarEquippable.ETriggerType.Trigger)
			{
				this.SetTrigger(this.AnimationTrigger);
				return;
			}
			this.SetBool(this.AnimationTrigger, true);
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0011B429 File Offset: 0x00119629
		public virtual void Unequip()
		{
			if (this.TriggerType == AvatarEquippable.ETriggerType.Trigger)
			{
				this.SetTrigger("EndAction");
			}
			else
			{
				this.SetBool(this.AnimationTrigger, false);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0011B458 File Offset: 0x00119658
		private void PositionAnimationModel()
		{
			Transform transform = (this.Hand == AvatarEquippable.EHand.Right) ? this.avatar.Anim.RightHandAlignmentPoint : this.avatar.Anim.LeftHandAlignmentPoint;
			base.transform.rotation = transform.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			base.transform.position = transform.position + (base.transform.position - this.AlignmentPoint.position);
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0011B4F8 File Offset: 0x001196F8
		protected void SetTrigger(string anim)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().SetAnimationTrigger(anim);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().SetAnimationTrigger(anim);
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0011B550 File Offset: 0x00119750
		protected void SetBool(string anim, bool val)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().SetAnimationBool(anim, val);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().SetAnimationBool(anim, val);
			}
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0011B5A8 File Offset: 0x001197A8
		protected void ResetTrigger(string anim)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().ResetAnimationTrigger(anim);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().ResetAnimationTrigger(anim);
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ReceiveMessage(string message, object parameter)
		{
		}

		// Token: 0x0400301E RID: 12318
		[Header("Settings")]
		public Transform AlignmentPoint;

		// Token: 0x0400301F RID: 12319
		[Range(0f, 1f)]
		public float Suspiciousness;

		// Token: 0x04003020 RID: 12320
		public AvatarEquippable.EHand Hand = AvatarEquippable.EHand.Right;

		// Token: 0x04003021 RID: 12321
		public AvatarEquippable.ETriggerType TriggerType;

		// Token: 0x04003022 RID: 12322
		public string AnimationTrigger = "RightArm_Hold_ClosedHand";

		// Token: 0x04003023 RID: 12323
		public string AssetPath = string.Empty;

		// Token: 0x04003024 RID: 12324
		protected Avatar avatar;

		// Token: 0x020009B6 RID: 2486
		public enum ETriggerType
		{
			// Token: 0x04003026 RID: 12326
			Trigger,
			// Token: 0x04003027 RID: 12327
			Bool
		}

		// Token: 0x020009B7 RID: 2487
		public enum EHand
		{
			// Token: 0x04003029 RID: 12329
			Left,
			// Token: 0x0400302A RID: 12330
			Right
		}
	}
}
