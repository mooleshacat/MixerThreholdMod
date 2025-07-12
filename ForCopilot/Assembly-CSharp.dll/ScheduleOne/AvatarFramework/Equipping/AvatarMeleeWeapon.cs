using System;
using ScheduleOne.Audio;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009B9 RID: 2489
	public class AvatarMeleeWeapon : AvatarWeapon
	{
		// Token: 0x06004370 RID: 17264 RVA: 0x0011B687 File Offset: 0x00119887
		public override void Unequip()
		{
			if (this.attackRoutine != null)
			{
				base.StopCoroutine(this.attackRoutine);
				this.attackRoutine = null;
			}
			base.Unequip();
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0011B6AC File Offset: 0x001198AC
		public override void Attack()
		{
			AvatarMeleeWeapon.<>c__DisplayClass10_0 CS$<>8__locals1 = new AvatarMeleeWeapon.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			base.Attack();
			CS$<>8__locals1.attack = this.Attacks[UnityEngine.Random.Range(0, this.Attacks.Length)];
			CS$<>8__locals1.npc = this.avatar.GetComponentInParent<NPC>();
			this.avatar.Anim.ResetTrigger(CS$<>8__locals1.attack.AnimationTrigger);
			this.avatar.Anim.SetTrigger(CS$<>8__locals1.attack.AnimationTrigger);
			this.attackRoutine = base.StartCoroutine(CS$<>8__locals1.<Attack>g__AttackRoutine|0());
		}

		// Token: 0x0400302D RID: 12333
		public const float GruntChance = 0.4f;

		// Token: 0x0400302E RID: 12334
		[Header("References")]
		public AudioSourceController AttackSound;

		// Token: 0x0400302F RID: 12335
		public AudioSourceController HitSound;

		// Token: 0x04003030 RID: 12336
		[Header("Melee Weapon settings")]
		public float AttackRange = 1.5f;

		// Token: 0x04003031 RID: 12337
		public float AttackRadius = 0.25f;

		// Token: 0x04003032 RID: 12338
		public float Damage = 25f;

		// Token: 0x04003033 RID: 12339
		public AvatarMeleeWeapon.MeleeAttack[] Attacks;

		// Token: 0x04003034 RID: 12340
		private Coroutine attackRoutine;

		// Token: 0x020009BA RID: 2490
		[Serializable]
		public class MeleeAttack
		{
			// Token: 0x04003035 RID: 12341
			public float RangeMultiplier = 1f;

			// Token: 0x04003036 RID: 12342
			public float DamageMultiplier = 1f;

			// Token: 0x04003037 RID: 12343
			public string AnimationTrigger = string.Empty;

			// Token: 0x04003038 RID: 12344
			public float DamageDelay = 0.4f;

			// Token: 0x04003039 RID: 12345
			public float AttackSoundDelay;

			// Token: 0x0400303A RID: 12346
			public AudioClip[] AttackClips;

			// Token: 0x0400303B RID: 12347
			public AudioClip[] HitClips;
		}
	}
}
