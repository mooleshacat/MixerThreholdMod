using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007D9 RID: 2009
	public class AmbientOneShot : MonoBehaviour
	{
		// Token: 0x06003657 RID: 13911 RVA: 0x000E4ECE File Offset: 0x000E30CE
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000E4EF8 File Offset: 0x000E30F8
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position, this.MinDistance);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.MaxDistance);
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000E4F48 File Offset: 0x000E3148
		private void MinPass()
		{
			this.timeSinceLastPlay++;
			if (this.timeSinceLastPlay < this.CooldownTime)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
			{
				return;
			}
			if (this.PlayTime == AmbientOneShot.EPlayTime.Day && NetworkSingleton<TimeManager>.Instance.IsNight)
			{
				return;
			}
			if (this.PlayTime == AmbientOneShot.EPlayTime.Night && !NetworkSingleton<TimeManager>.Instance.IsNight)
			{
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.MinDistance)
			{
				return;
			}
			if (num > this.MaxDistance)
			{
				return;
			}
			if (UnityEngine.Random.value < this.ChancePerHour / 60f)
			{
				this.Play();
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000E4FF8 File Offset: 0x000E31F8
		private void Play()
		{
			this.timeSinceLastPlay = 0;
			this.Audio.SetVolume(this.Volume);
			this.Audio.Play();
		}

		// Token: 0x04002689 RID: 9865
		public AudioSourceController Audio;

		// Token: 0x0400268A RID: 9866
		[Header("Settings")]
		[Range(0f, 1f)]
		public float Volume = 0.2f;

		// Token: 0x0400268B RID: 9867
		[Range(0f, 1f)]
		public float ChancePerHour = 0.2f;

		// Token: 0x0400268C RID: 9868
		public int CooldownTime = 60;

		// Token: 0x0400268D RID: 9869
		public AmbientOneShot.EPlayTime PlayTime;

		// Token: 0x0400268E RID: 9870
		public float MinDistance = 20f;

		// Token: 0x0400268F RID: 9871
		public float MaxDistance = 100f;

		// Token: 0x04002690 RID: 9872
		private int timeSinceLastPlay;

		// Token: 0x020007DA RID: 2010
		public enum EPlayTime
		{
			// Token: 0x04002692 RID: 9874
			All,
			// Token: 0x04002693 RID: 9875
			Day,
			// Token: 0x04002694 RID: 9876
			Night
		}
	}
}
