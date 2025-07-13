using System;
using System.Collections;
using ScheduleOne.Audio;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009BD RID: 2493
	public class AvatarRangedWeapon : AvatarWeapon
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x0600437C RID: 17276 RVA: 0x0011B9C8 File Offset: 0x00119BC8
		// (set) Token: 0x0600437D RID: 17277 RVA: 0x0011B9D0 File Offset: 0x00119BD0
		public bool IsRaised { get; protected set; }

		// Token: 0x0600437E RID: 17278 RVA: 0x0011B9D9 File Offset: 0x00119BD9
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			if (this.MagazineSize != -1)
			{
				this.currentAmmo = this.MagazineSize;
			}
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0011B9F8 File Offset: 0x00119BF8
		public virtual void SetIsRaised(bool raised)
		{
			if (this.IsRaised == raised)
			{
				return;
			}
			this.IsRaised = raised;
			this.timeRaised = 0f;
			if (this.IsRaised)
			{
				base.ResetTrigger(this.LoweredAnimationTrigger);
				base.SetTrigger(this.RaisedAnimationTrigger);
				return;
			}
			base.ResetTrigger(this.RaisedAnimationTrigger);
			base.SetTrigger(this.LoweredAnimationTrigger);
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0011BA5A File Offset: 0x00119C5A
		private void Update()
		{
			this.timeEquipped += Time.deltaTime;
			this.timeSinceLastShot += Time.deltaTime;
			if (this.IsRaised)
			{
				this.timeRaised += Time.deltaTime;
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0011BA9C File Offset: 0x00119C9C
		public override void ReceiveMessage(string message, object data)
		{
			base.ReceiveMessage(message, data);
			if (message == "Shoot")
			{
				this.Shoot((Vector3)data);
			}
			if (message == "Lower")
			{
				this.SetIsRaised(false);
			}
			if (message == "Raise")
			{
				this.SetIsRaised(true);
			}
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0011BAF4 File Offset: 0x00119CF4
		public bool CanShoot()
		{
			return (this.currentAmmo > 0 || this.MagazineSize == -1) && this.timeEquipped > this.EquipTime && !this.isReloading && this.timeSinceLastShot > this.MaxFireRate && this.timeRaised > this.RaiseTime;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0011BB48 File Offset: 0x00119D48
		public virtual void Shoot(Vector3 endPoint)
		{
			this.timeSinceLastShot = 0f;
			if (this.RecoilAnimationTrigger != string.Empty)
			{
				base.ResetTrigger(this.RecoilAnimationTrigger);
				base.SetTrigger(this.RecoilAnimationTrigger);
			}
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				return;
			}
			this.currentAmmo--;
			this.FireSound.PlayOneShot(true);
			if (this.currentAmmo <= 0 && this.MagazineSize != -1)
			{
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0011BBDC File Offset: 0x00119DDC
		private IEnumerator Reload()
		{
			this.isReloading = true;
			yield return new WaitForSeconds(this.ReloadTime);
			this.currentAmmo = this.MagazineSize;
			this.isReloading = false;
			yield break;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0011BBEC File Offset: 0x00119DEC
		public bool IsPlayerInLoS(Player target)
		{
			LayerMask mask = LayerMask.GetMask(AvatarRangedWeapon.RaycastLayers);
			RaycastHit raycastHit;
			return !Physics.Raycast(this.MuzzlePoint.position, (target.Avatar.CenterPoint - this.MuzzlePoint.position).normalized, ref raycastHit, Vector3.Distance(this.MuzzlePoint.position, target.Avatar.CenterPoint), mask) || !raycastHit.collider.GetComponentInParent<Player>() || raycastHit.collider.GetComponentInParent<Player>() == target || (raycastHit.collider.GetComponentInParent<LandVehicle>() != null && raycastHit.collider.GetComponentInParent<LandVehicle>().DriverPlayer == target);
		}

		// Token: 0x04003042 RID: 12354
		public static string[] RaycastLayers = new string[]
		{
			"Default",
			"Vehicle",
			"Door",
			"Terrain",
			"Player"
		};

		// Token: 0x04003043 RID: 12355
		[Header("Weapon Settings")]
		public int MagazineSize = -1;

		// Token: 0x04003044 RID: 12356
		public float ReloadTime = 2f;

		// Token: 0x04003045 RID: 12357
		public float MaxFireRate = 0.5f;

		// Token: 0x04003046 RID: 12358
		public bool CanShootWhileMoving;

		// Token: 0x04003047 RID: 12359
		public float EquipTime = 1f;

		// Token: 0x04003048 RID: 12360
		public float RaiseTime = 1f;

		// Token: 0x04003049 RID: 12361
		public float Damage = 35f;

		// Token: 0x0400304A RID: 12362
		[Header("Accuracy")]
		public float HitChange_MinRange = 0.6f;

		// Token: 0x0400304B RID: 12363
		public float HitChange_MaxRange = 0.1f;

		// Token: 0x0400304C RID: 12364
		[Header("References")]
		public Transform MuzzlePoint;

		// Token: 0x0400304D RID: 12365
		public AudioSourceController FireSound;

		// Token: 0x0400304E RID: 12366
		[Header("Settings")]
		public string LoweredAnimationTrigger;

		// Token: 0x0400304F RID: 12367
		public string RaisedAnimationTrigger;

		// Token: 0x04003050 RID: 12368
		public string RecoilAnimationTrigger;

		// Token: 0x04003052 RID: 12370
		private bool isReloading;

		// Token: 0x04003053 RID: 12371
		private float timeEquipped;

		// Token: 0x04003054 RID: 12372
		private float timeRaised;

		// Token: 0x04003055 RID: 12373
		private float timeSinceLastShot = 1000f;

		// Token: 0x04003056 RID: 12374
		private int currentAmmo;
	}
}
