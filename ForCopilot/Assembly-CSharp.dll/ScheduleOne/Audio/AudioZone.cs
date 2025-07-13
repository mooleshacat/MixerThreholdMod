using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007DF RID: 2015
	public class AudioZone : Zone
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x000E5C74 File Offset: 0x000E3E74
		// (set) Token: 0x06003690 RID: 13968 RVA: 0x000E5C7C File Offset: 0x000E3E7C
		public float VolumeModifier { get; set; }

		// Token: 0x06003691 RID: 13969 RVA: 0x000E5C88 File Offset: 0x000E3E88
		private void Start()
		{
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.Init();
			}
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000E5D00 File Offset: 0x000E3F00
		private void Update()
		{
			this.VolumeModifier = Mathf.MoveTowards(this.VolumeModifier, this.GetFalloffFactor(base.LocalPlayerDistance), 1f * Time.deltaTime);
			this.CurrentVolumeMultiplier = Mathf.MoveTowards(this.CurrentVolumeMultiplier, this.GetTotalVolumeMultiplier(), 1f * Time.deltaTime);
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.Update(this.VolumeModifier * this.CurrentVolumeMultiplier);
			}
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000E5DA8 File Offset: 0x000E3FA8
		private float GetTotalVolumeMultiplier()
		{
			float num = 1f;
			foreach (KeyValuePair<AudioZoneModifierVolume, float> keyValuePair in this.Modifiers)
			{
				num *= keyValuePair.Value;
			}
			return num;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000E5E08 File Offset: 0x000E4008
		private void MinPass()
		{
			foreach (AudioZone.Track track in this.Tracks)
			{
				track.UpdateTimeMultiplier(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			}
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000E5E64 File Offset: 0x000E4064
		public void AddModifier(AudioZoneModifierVolume modifier, float value)
		{
			if (!this.Modifiers.ContainsKey(modifier))
			{
				this.Modifiers.Add(modifier, value);
			}
			this.Modifiers[modifier] = value;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000E5E8E File Offset: 0x000E408E
		public void RemoveModifier(AudioZoneModifierVolume modifier)
		{
			if (this.Modifiers.ContainsKey(modifier))
			{
				this.Modifiers.Remove(modifier);
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000E5EAB File Offset: 0x000E40AB
		private float GetFalloffFactor(float distance)
		{
			if (distance > this.MaxDistance)
			{
				return 0f;
			}
			return 1f / (1f + 0.5f * distance);
		}

		// Token: 0x040026C9 RID: 9929
		public const float VOLUME_CHANGE_RATE = 1f;

		// Token: 0x040026CA RID: 9930
		public const float ROLLOFF_SCALE = 0.5f;

		// Token: 0x040026CB RID: 9931
		[Header("Settings")]
		[Range(1f, 200f)]
		public float MaxDistance = 100f;

		// Token: 0x040026CC RID: 9932
		public List<AudioZone.Track> Tracks = new List<AudioZone.Track>();

		// Token: 0x040026CD RID: 9933
		public Dictionary<AudioZoneModifierVolume, float> Modifiers = new Dictionary<AudioZoneModifierVolume, float>();

		// Token: 0x040026CF RID: 9935
		protected float CurrentVolumeMultiplier = 1f;

		// Token: 0x020007E0 RID: 2016
		[Serializable]
		public class Track
		{
			// Token: 0x06003699 RID: 13977 RVA: 0x000E5F04 File Offset: 0x000E4104
			public void Init()
			{
				this.fadeInStart = TimeManager.AddMinutesTo24HourTime(this.StartTime, -this.FadeTime / 2);
				this.fadeInEnd = TimeManager.AddMinutesTo24HourTime(this.StartTime, this.FadeTime / 2);
				this.fadeOutStart = TimeManager.AddMinutesTo24HourTime(this.EndTime, -this.FadeTime / 2);
				this.fadeOutEnd = TimeManager.AddMinutesTo24HourTime(this.EndTime, this.FadeTime / 2);
				this.fadeInStartMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeInStart);
				this.fadeInEndMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeInEnd);
				this.fadeOutStartMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeOutStart);
				this.fadeOutEndMinSum = TimeManager.GetMinSumFrom24HourTime(this.fadeOutEnd);
			}

			// Token: 0x0600369A RID: 13978 RVA: 0x000E5FBC File Offset: 0x000E41BC
			public void Update(float multiplier)
			{
				float num = this.Volume * multiplier * this.timeVolMultiplier;
				this.Source.SetVolume(num);
				if (num > 0f)
				{
					if (!this.Source.isPlaying)
					{
						this.Source.Play();
						return;
					}
				}
				else if (this.Source.isPlaying)
				{
					this.Source.Stop();
				}
			}

			// Token: 0x0600369B RID: 13979 RVA: 0x000E6020 File Offset: 0x000E4220
			public void UpdateTimeMultiplier(int time)
			{
				int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(time);
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeInEnd, this.fadeOutStart))
				{
					this.timeVolMultiplier = 1f;
					return;
				}
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeInStart, this.fadeInEnd))
				{
					this.timeVolMultiplier = (float)(minSumFrom24HourTime - this.fadeInStartMinSum) / (float)(this.fadeInEndMinSum - this.fadeInStartMinSum);
					return;
				}
				if (TimeManager.IsGivenTimeWithinRange(time, this.fadeOutStart, this.fadeOutEnd))
				{
					this.timeVolMultiplier = 1f - (float)(minSumFrom24HourTime - this.fadeOutStartMinSum) / (float)(this.fadeOutEndMinSum - this.fadeOutStartMinSum);
					return;
				}
				this.timeVolMultiplier = 0f;
			}

			// Token: 0x040026D0 RID: 9936
			public AudioSourceController Source;

			// Token: 0x040026D1 RID: 9937
			[Range(0.01f, 2f)]
			public float Volume = 1f;

			// Token: 0x040026D2 RID: 9938
			public int StartTime;

			// Token: 0x040026D3 RID: 9939
			public int EndTime;

			// Token: 0x040026D4 RID: 9940
			public int FadeTime = 60;

			// Token: 0x040026D5 RID: 9941
			private float timeVolMultiplier;

			// Token: 0x040026D6 RID: 9942
			private int fadeInStart;

			// Token: 0x040026D7 RID: 9943
			private int fadeInEnd;

			// Token: 0x040026D8 RID: 9944
			private int fadeOutStart;

			// Token: 0x040026D9 RID: 9945
			private int fadeOutEnd;

			// Token: 0x040026DA RID: 9946
			private int fadeInStartMinSum;

			// Token: 0x040026DB RID: 9947
			private int fadeInEndMinSum;

			// Token: 0x040026DC RID: 9948
			private int fadeOutStartMinSum;

			// Token: 0x040026DD RID: 9949
			private int fadeOutEndMinSum;
		}
	}
}
