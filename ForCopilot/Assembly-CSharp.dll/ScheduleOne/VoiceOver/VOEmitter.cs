using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000287 RID: 647
	[RequireComponent(typeof(AudioSourceController))]
	public class VOEmitter : MonoBehaviour
	{
		// Token: 0x06000D84 RID: 3460 RVA: 0x0003B900 File Offset: 0x00039B00
		protected virtual void Awake()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003B910 File Offset: 0x00039B10
		public virtual void Play(EVOLineType lineType)
		{
			if (!this.audioSourceController.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.Database == null)
			{
				Console.LogError("Database is not set on VOEmitter.", null);
				return;
			}
			AudioClip randomClip = this.Database.GetRandomClip(lineType);
			if (randomClip == null)
			{
				Console.LogError("No clip found for line type: " + lineType.ToString(), null);
				return;
			}
			this.audioSourceController.Stop();
			this.audioSourceController.AudioSource.clip = randomClip;
			this.audioSourceController.VolumeMultiplier = this.Database.VolumeMultiplier * this.Database.GetEntry(lineType).VolumeMultiplier;
			this.audioSourceController.PitchMultiplier = (this.PitchMultiplier + UnityEngine.Random.Range(-0.05f, 0.05f)) * this.runtimePitchMultiplier;
			this.audioSourceController.Play();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0003B9F5 File Offset: 0x00039BF5
		public void SetRuntimePitchMultiplier(float pitchMultiplier)
		{
			this.runtimePitchMultiplier = pitchMultiplier;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0003B9FE File Offset: 0x00039BFE
		public void SetDatabase(VODatabase database, bool writeDefault = true)
		{
			this.Database = database;
			if (writeDefault)
			{
				this.defaultVODatabase = database;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0003BA11 File Offset: 0x00039C11
		public void ResetDatabase()
		{
			this.SetDatabase(this.defaultVODatabase, false);
		}

		// Token: 0x04000DEC RID: 3564
		public const float PitchVariation = 0.05f;

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		private VODatabase Database;

		// Token: 0x04000DEE RID: 3566
		[Range(0.5f, 2f)]
		public float PitchMultiplier = 1f;

		// Token: 0x04000DEF RID: 3567
		private float runtimePitchMultiplier = 1f;

		// Token: 0x04000DF0 RID: 3568
		protected AudioSourceController audioSourceController;

		// Token: 0x04000DF1 RID: 3569
		private VODatabase defaultVODatabase;
	}
}
