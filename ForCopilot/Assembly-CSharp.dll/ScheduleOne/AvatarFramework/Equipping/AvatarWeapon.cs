using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009BF RID: 2495
	public class AvatarWeapon : AvatarEquippable
	{
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600438E RID: 17294 RVA: 0x0011BDE3 File Offset: 0x00119FE3
		// (set) Token: 0x0600438F RID: 17295 RVA: 0x0011BDEB File Offset: 0x00119FEB
		public float LastUseTime { get; private set; }

		// Token: 0x06004390 RID: 17296 RVA: 0x0011BDF4 File Offset: 0x00119FF4
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			if (this.EquipClips.Length != 0 && this.EquipSound != null)
			{
				this.EquipSound.AudioSource.clip = this.EquipClips[UnityEngine.Random.Range(0, this.EquipClips.Length)];
				this.EquipSound.Play();
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0011BE4F File Offset: 0x0011A04F
		public virtual void Attack()
		{
			this.LastUseTime = Time.time;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0011BE5C File Offset: 0x0011A05C
		public virtual bool IsReadyToAttack()
		{
			return Time.time - this.LastUseTime > this.CooldownDuration;
		}

		// Token: 0x0400305A RID: 12378
		[Header("Range settings")]
		public float MinUseRange;

		// Token: 0x0400305B RID: 12379
		public float MaxUseRange = 1f;

		// Token: 0x0400305C RID: 12380
		[Header("Cooldown settings")]
		public float CooldownDuration = 1f;

		// Token: 0x0400305D RID: 12381
		[Header("Equipping")]
		public AudioClip[] EquipClips;

		// Token: 0x0400305E RID: 12382
		public AudioSourceController EquipSound;

		// Token: 0x04003060 RID: 12384
		public UnityEvent onSuccessfulHit;
	}
}
