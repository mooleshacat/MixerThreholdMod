using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.FX
{
	// Token: 0x02000655 RID: 1621
	public class CountdownExplosion : MonoBehaviour
	{
		// Token: 0x06002A15 RID: 10773 RVA: 0x000AE2E1 File Offset: 0x000AC4E1
		public void Trigger()
		{
			this.countdownRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Trigger>g__Routine|5_0());
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000AE2F9 File Offset: 0x000AC4F9
		public void StopCountdown()
		{
			if (this.countdownRoutine != null)
			{
				base.StopCoroutine(this.countdownRoutine);
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000AE30F File Offset: 0x000AC50F
		[CompilerGenerated]
		private IEnumerator <Trigger>g__Routine|5_0()
		{
			float timeUntilNextTick = 1f;
			for (float i = 0f; i < 30f; i += Time.deltaTime)
			{
				timeUntilNextTick -= Time.deltaTime;
				if (timeUntilNextTick <= 0f)
				{
					timeUntilNextTick = Mathf.Lerp(1f, 0.1f, i / 30f);
					this.TickSound.PitchMultiplier = Mathf.Lerp(1f, 1.1f, i / 30f);
					this.TickSound.VolumeMultiplier = Mathf.Lerp(0.6f, 1f, i / 30f);
					this.TickSound.Play();
				}
				yield return new WaitForEndOfFrame();
			}
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<CombatManager>.Instance.CreateExplosion(base.transform.position, ExplosionData.DefaultSmall);
			}
			this.countdownRoutine = null;
			yield break;
		}

		// Token: 0x04001EAD RID: 7853
		public const float COUNTDOWN = 30f;

		// Token: 0x04001EAE RID: 7854
		public const float TICK_SPACING_MAX = 1f;

		// Token: 0x04001EAF RID: 7855
		public const float TICK_SPACING_MIN = 0.1f;

		// Token: 0x04001EB0 RID: 7856
		public AudioSourceController TickSound;

		// Token: 0x04001EB1 RID: 7857
		private Coroutine countdownRoutine;
	}
}
