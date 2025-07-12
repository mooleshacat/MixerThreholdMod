using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F4 RID: 2036
	public class SFXManager : Singleton<SFXManager>
	{
		// Token: 0x060036E5 RID: 14053 RVA: 0x000E7084 File Offset: 0x000E5284
		public void PlayImpactSound(ImpactSoundEntity.EMaterial material, Vector3 position, float momentum)
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.Distance(position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 40f)
			{
				Console.LogWarning("Impact sound too far away", null);
				return;
			}
			SFXManager.ImpactType impactType = this.ImpactTypes.Find((SFXManager.ImpactType x) => x.Material == material);
			if (impactType == null)
			{
				Console.LogWarning("No impact type found for material: " + material.ToString(), null);
				return;
			}
			AudioSourceController source = this.GetSource();
			if (source == null)
			{
				Console.LogWarning("No source available", null);
				return;
			}
			source.transform.position = position;
			float num = Mathf.Clamp01(momentum / 100f);
			source.PitchMultiplier = Mathf.Lerp(impactType.MaxPitch, impactType.MinPitch, num);
			source.VolumeMultiplier = Mathf.Lerp(impactType.MinVolume, impactType.MaxVolume, Mathf.Sqrt(num));
			source.AudioSource.clip = impactType.Clips[UnityEngine.Random.Range(0, impactType.Clips.Length)];
			source.Play();
			this.soundsInUse.Add(source);
			this.soundPool.Remove(source);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000E71B4 File Offset: 0x000E53B4
		private void FixedUpdate()
		{
			for (int i = this.soundsInUse.Count - 1; i >= 0; i--)
			{
				if (!this.soundsInUse[i].isPlaying)
				{
					this.soundPool.Add(this.soundsInUse[i]);
					this.soundsInUse.RemoveAt(i);
				}
			}
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000E720F File Offset: 0x000E540F
		private AudioSourceController GetSource()
		{
			if (this.soundPool.Count == 0)
			{
				Console.Log("No more sources available", null);
				return null;
			}
			return this.soundPool[0];
		}

		// Token: 0x04002724 RID: 10020
		public const float MAX_PLAYER_DISTANCE = 40f;

		// Token: 0x04002725 RID: 10021
		public const float SQR_MAX_PLAYER_DISTANCE = 1600f;

		// Token: 0x04002726 RID: 10022
		public List<SFXManager.ImpactType> ImpactTypes = new List<SFXManager.ImpactType>();

		// Token: 0x04002727 RID: 10023
		[SerializeField]
		private List<AudioSourceController> soundPool = new List<AudioSourceController>();

		// Token: 0x04002728 RID: 10024
		private List<AudioSourceController> soundsInUse = new List<AudioSourceController>();

		// Token: 0x020007F5 RID: 2037
		[Serializable]
		public class ImpactType
		{
			// Token: 0x04002729 RID: 10025
			public ImpactSoundEntity.EMaterial Material;

			// Token: 0x0400272A RID: 10026
			public float MinVolume;

			// Token: 0x0400272B RID: 10027
			public float MaxVolume;

			// Token: 0x0400272C RID: 10028
			public float MinPitch;

			// Token: 0x0400272D RID: 10029
			public float MaxPitch;

			// Token: 0x0400272E RID: 10030
			public AudioClip[] Clips;
		}
	}
}
