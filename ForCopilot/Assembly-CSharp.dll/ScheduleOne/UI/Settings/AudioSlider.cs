using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB4 RID: 2740
	public class AudioSlider : SettingsSlider
	{
		// Token: 0x060049B5 RID: 18869 RVA: 0x001363FC File Offset: 0x001345FC
		protected virtual void Start()
		{
			if (this.Master)
			{
				this.slider.SetValueWithoutNotify(Singleton<AudioManager>.Instance.MasterVolume / 0.01f);
				return;
			}
			this.slider.SetValueWithoutNotify(Singleton<AudioManager>.Instance.GetVolume(this.AudioType, false) / 0.01f);
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00136450 File Offset: 0x00134650
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			if (this.Master)
			{
				Singleton<Settings>.Instance.AudioSettings.MasterVolume = value * 0.01f;
			}
			else
			{
				switch (this.AudioType)
				{
				case EAudioType.Ambient:
					Singleton<Settings>.Instance.AudioSettings.AmbientVolume = value * 0.01f;
					break;
				case EAudioType.Footsteps:
					Singleton<Settings>.Instance.AudioSettings.FootstepsVolume = value * 0.01f;
					break;
				case EAudioType.FX:
					Singleton<Settings>.Instance.AudioSettings.SFXVolume = value * 0.01f;
					break;
				case EAudioType.UI:
					Singleton<Settings>.Instance.AudioSettings.UIVolume = value * 0.01f;
					break;
				case EAudioType.Music:
					Singleton<Settings>.Instance.AudioSettings.MusicVolume = value * 0.01f;
					break;
				case EAudioType.Voice:
					Singleton<Settings>.Instance.AudioSettings.DialogueVolume = value * 0.01f;
					break;
				}
			}
			Singleton<Settings>.Instance.ReloadAudioSettings();
			Singleton<Settings>.Instance.WriteAudioSettings(Singleton<Settings>.Instance.AudioSettings);
		}

		// Token: 0x0400364B RID: 13899
		public const float MULTIPLIER = 0.01f;

		// Token: 0x0400364C RID: 13900
		public bool Master;

		// Token: 0x0400364D RID: 13901
		public EAudioType AudioType = EAudioType.FX;
	}
}
