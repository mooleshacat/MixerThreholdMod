using System;
using Beautify.Universal;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ScheduleOne.FX
{
	// Token: 0x0200065C RID: 1628
	public class PlayerHealthVisuals : MonoBehaviour
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x000AEAC8 File Offset: 0x000ACCC8
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Spawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Spawned));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			this.GlobalVolume.sharedProfile.TryGet<Beautify>(ref this._beautifySettings);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000AEB78 File Offset: 0x000ACD78
		private void Spawned()
		{
			if (!Player.Local.Owner.IsLocalClient)
			{
				return;
			}
			this.UpdateEffects(Player.Local.Health.CurrentHealth);
			Player.Local.Health.onHealthChanged.AddListener(new UnityAction<float>(this.UpdateEffects));
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000AEBCC File Offset: 0x000ACDCC
		private void MinPass()
		{
			this._beautifySettings.vignettingOuterRing.value = this.OuterRingCurve.Evaluate(NetworkSingleton<TimeManager>.Instance.NormalizedTime);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000AEBF4 File Offset: 0x000ACDF4
		private void UpdateEffects(float newHealth)
		{
			this._beautifySettings.vignettingColor.value = new Color(this._beautifySettings.vignettingColor.value.r, this._beautifySettings.vignettingColor.value.g, this._beautifySettings.vignettingColor.value.b, Mathf.Lerp(this.VignetteAlpha_MinHealth, this.VignetteAlpha_MaxHealth, newHealth / 100f));
			this._beautifySettings.saturate.value = Mathf.Lerp(this.Saturation_MinHealth, this.Saturation_MaxHealth, newHealth / 100f);
			this._beautifySettings.chromaticAberrationIntensity.value = Mathf.Lerp(this.ChromAb_MinHealth, this.ChromAb_MaxHealth, newHealth / 100f);
			this._beautifySettings.lensDirtIntensity.value = Mathf.Lerp(this.LensDirt_MinHealth, this.LensDirt_MaxHealth, newHealth / 100f);
		}

		// Token: 0x04001EE0 RID: 7904
		[Header("References")]
		public Volume GlobalVolume;

		// Token: 0x04001EE1 RID: 7905
		[Header("Vignette")]
		public float VignetteAlpha_MaxHealth;

		// Token: 0x04001EE2 RID: 7906
		public float VignetteAlpha_MinHealth;

		// Token: 0x04001EE3 RID: 7907
		public AnimationCurve OuterRingCurve;

		// Token: 0x04001EE4 RID: 7908
		[Header("Saturation")]
		public float Saturation_MaxHealth = 0.5f;

		// Token: 0x04001EE5 RID: 7909
		public float Saturation_MinHealth = -2f;

		// Token: 0x04001EE6 RID: 7910
		[Header("Chromatic Abberation")]
		public float ChromAb_MaxHealth;

		// Token: 0x04001EE7 RID: 7911
		public float ChromAb_MinHealth = 0.02f;

		// Token: 0x04001EE8 RID: 7912
		[Header("Lens Dirt")]
		public float LensDirt_MaxHealth;

		// Token: 0x04001EE9 RID: 7913
		public float LensDirt_MinHealth = 1f;

		// Token: 0x04001EEA RID: 7914
		private Beautify _beautifySettings;
	}
}
