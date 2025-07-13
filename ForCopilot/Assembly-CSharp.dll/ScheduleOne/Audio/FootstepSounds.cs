using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Materials;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007E4 RID: 2020
	public class FootstepSounds : MonoBehaviour
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060036A8 RID: 13992 RVA: 0x000E63F4 File Offset: 0x000E45F4
		// (set) Token: 0x060036A9 RID: 13993 RVA: 0x000E63FC File Offset: 0x000E45FC
		public float VolumeMultiplier { get; set; } = 1f;

		// Token: 0x060036AA RID: 13994 RVA: 0x000E6408 File Offset: 0x000E4608
		private void Start()
		{
			foreach (FootstepSounds.FootstepSoundGroup footstepSoundGroup in this.soundGroups)
			{
				foreach (FootstepSounds.FootstepSoundGroup.MaterialType materialType in footstepSoundGroup.appliesTo)
				{
					if (!this.materialFootstepSounds.ContainsKey(materialType.type))
					{
						this.materialFootstepSounds.Add(materialType.type, footstepSoundGroup);
					}
				}
			}
			foreach (object obj in Enum.GetValues(typeof(EMaterialType)))
			{
				EMaterialType key = (EMaterialType)obj;
				if (!this.materialFootstepSounds.ContainsKey(key))
				{
					Console.Log("No footstep sounds for material type: " + key.ToString() + "\n Assigning to default group.", null);
					this.materialFootstepSounds.Add(key, this.soundGroups[0]);
				}
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				this.sources[i].AudioSource.enabled = false;
				this.sources[i].enabled = false;
			}
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000E6598 File Offset: 0x000E4798
		private void Update()
		{
			this.lastStepTime += Time.deltaTime;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000E65AC File Offset: 0x000E47AC
		public void Step(EMaterialType materialType, float hardness)
		{
			FootstepSounds.<>c__DisplayClass12_0 CS$<>8__locals1 = new FootstepSounds.<>c__DisplayClass12_0();
			if (this.lastStepTime < 0.15f)
			{
				return;
			}
			this.lastStepTime = 0f;
			CS$<>8__locals1.source = this.GetFreeSource();
			if (CS$<>8__locals1.source == null)
			{
				Console.LogWarning("No free audio sources available for footstep sound.", null);
				return;
			}
			FootstepSounds.FootstepSoundGroup footstepSoundGroup = this.materialFootstepSounds[materialType];
			CS$<>8__locals1.source.AudioSource.clip = footstepSoundGroup.clips[UnityEngine.Random.Range(0, footstepSoundGroup.clips.Count)];
			CS$<>8__locals1.source.AudioSource.pitch = UnityEngine.Random.Range(footstepSoundGroup.PitchMin, footstepSoundGroup.PitchMax);
			CS$<>8__locals1.source.SetVolume(footstepSoundGroup.Volume * hardness * this.VolumeMultiplier);
			CS$<>8__locals1.source.AudioSource.enabled = true;
			CS$<>8__locals1.source.enabled = true;
			CS$<>8__locals1.source.Play();
			base.StartCoroutine(CS$<>8__locals1.<Step>g__DisableSource|0());
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000E66A7 File Offset: 0x000E48A7
		public AudioSourceController GetFreeSource()
		{
			return this.sources.FirstOrDefault((AudioSourceController source) => !source.enabled);
		}

		// Token: 0x040026EA RID: 9962
		public const float COOLDOWN_TIME = 0.15f;

		// Token: 0x040026EC RID: 9964
		public List<AudioSourceController> sources = new List<AudioSourceController>();

		// Token: 0x040026ED RID: 9965
		public List<FootstepSounds.FootstepSoundGroup> soundGroups = new List<FootstepSounds.FootstepSoundGroup>();

		// Token: 0x040026EE RID: 9966
		private Dictionary<EMaterialType, FootstepSounds.FootstepSoundGroup> materialFootstepSounds = new Dictionary<EMaterialType, FootstepSounds.FootstepSoundGroup>();

		// Token: 0x040026EF RID: 9967
		private float lastStepTime;

		// Token: 0x020007E5 RID: 2021
		[Serializable]
		public class FootstepSoundGroup
		{
			// Token: 0x040026F0 RID: 9968
			public string name;

			// Token: 0x040026F1 RID: 9969
			public List<AudioClip> clips = new List<AudioClip>();

			// Token: 0x040026F2 RID: 9970
			public List<FootstepSounds.FootstepSoundGroup.MaterialType> appliesTo = new List<FootstepSounds.FootstepSoundGroup.MaterialType>();

			// Token: 0x040026F3 RID: 9971
			public float PitchMin = 0.9f;

			// Token: 0x040026F4 RID: 9972
			public float PitchMax = 1.1f;

			// Token: 0x040026F5 RID: 9973
			public float Volume = 0.5f;

			// Token: 0x020007E6 RID: 2022
			[Serializable]
			public class MaterialType
			{
				// Token: 0x040026F6 RID: 9974
				public EMaterialType type;
			}
		}
	}
}
