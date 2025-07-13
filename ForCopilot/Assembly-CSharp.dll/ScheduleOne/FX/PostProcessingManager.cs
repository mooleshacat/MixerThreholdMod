using System;
using Beautify.Universal;
using CorgiGodRays;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.FX
{
	// Token: 0x0200065D RID: 1629
	public class PostProcessingManager : Singleton<PostProcessingManager>
	{
		// Token: 0x06002A3F RID: 10815 RVA: 0x000AED1C File Offset: 0x000ACF1C
		protected override void Awake()
		{
			base.Awake();
			this.GlobalVolume.enabled = true;
			this.GlobalVolume.sharedProfile.TryGet<Vignette>(ref this.vig);
			this.ResetVignette();
			this.GlobalVolume.sharedProfile.TryGet<DepthOfField>(ref this.DoF);
			this.DoF.active = false;
			this.GlobalVolume.sharedProfile.TryGet<GodRaysVolume>(ref this.GodRays);
			this.GlobalVolume.sharedProfile.TryGet<ColorAdjustments>(ref this.ColorAdjustments);
			this.GlobalVolume.sharedProfile.TryGet<Beautify>(ref this.beautifySettings);
			this.GlobalVolume.sharedProfile.TryGet<Bloom>(ref this.bloom);
			this.GlobalVolume.sharedProfile.TryGet<ChromaticAberration>(ref this.chromaticAberration);
			this.GlobalVolume.sharedProfile.TryGet<ColorAdjustments>(ref this.colorAdjustments);
			this.ChromaticAberrationController.Initialize();
			this.SaturationController.Initialize();
			this.BloomController.Initialize();
			this.ColorFilterController.Initialize();
			this.SetBlur(0f);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000AEE3C File Offset: 0x000AD03C
		public void Update()
		{
			this.UpdateEffects();
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000AEE44 File Offset: 0x000AD044
		private void UpdateEffects()
		{
			float num = Mathf.Lerp(1f, 12f, PlayerSingleton<PlayerCamera>.InstanceExists ? PlayerSingleton<PlayerCamera>.Instance.FovJitter : 0f);
			this.chromaticAberration.intensity.value = this.ChromaticAberrationController.CurrentValue * num;
			this.ColorAdjustments.saturation.value = this.SaturationController.CurrentValue;
			this.ColorAdjustments.postExposure.value = 0.1f * num;
			this.bloom.intensity.value = this.BloomController.CurrentValue * num;
			this.colorAdjustments.colorFilter.value = this.ColorFilterController.CurrentValue;
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000AEF00 File Offset: 0x000AD100
		public void OverrideVignette(float intensity, float smoothness)
		{
			this.vig.intensity.value = intensity;
			this.vig.smoothness.value = smoothness;
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000AEF24 File Offset: 0x000AD124
		public void ResetVignette()
		{
			this.vig.intensity.value = this.Vig_DefaultIntensity;
			this.vig.smoothness.value = this.Vig_DefaultSmoothness;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000AEF52 File Offset: 0x000AD152
		public void SetGodRayIntensity(float intensity)
		{
			this.GodRays.MainLightIntensity.value = intensity;
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000AEF65 File Offset: 0x000AD165
		public void SetContrast(float value)
		{
			this.ColorAdjustments.contrast.value = value;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000AEF78 File Offset: 0x000AD178
		public void SetSaturation(float value)
		{
			this.SaturationController.SetDefault(value);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000AEF86 File Offset: 0x000AD186
		public void SetBloomThreshold(float threshold)
		{
			this.bloom.threshold.value = threshold;
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000AEF99 File Offset: 0x000AD199
		public void SetBlur(float blurLevel)
		{
			this.beautifySettings.blurIntensity.value = Mathf.Lerp(this.MinBlur, this.MaxBlur, blurLevel);
		}

		// Token: 0x04001EEB RID: 7915
		[Header("References")]
		public Volume GlobalVolume;

		// Token: 0x04001EEC RID: 7916
		[Header("Vignette")]
		public float Vig_DefaultIntensity = 0.25f;

		// Token: 0x04001EED RID: 7917
		public float Vig_DefaultSmoothness = 0.3f;

		// Token: 0x04001EEE RID: 7918
		[Header("Blur")]
		public float MinBlur;

		// Token: 0x04001EEF RID: 7919
		public float MaxBlur = 1f;

		// Token: 0x04001EF0 RID: 7920
		[Header("Smoothers")]
		public FloatSmoother ChromaticAberrationController;

		// Token: 0x04001EF1 RID: 7921
		public FloatSmoother SaturationController;

		// Token: 0x04001EF2 RID: 7922
		public FloatSmoother BloomController;

		// Token: 0x04001EF3 RID: 7923
		public HDRColorSmoother ColorFilterController;

		// Token: 0x04001EF4 RID: 7924
		private Vignette vig;

		// Token: 0x04001EF5 RID: 7925
		private DepthOfField DoF;

		// Token: 0x04001EF6 RID: 7926
		private GodRaysVolume GodRays;

		// Token: 0x04001EF7 RID: 7927
		private ColorAdjustments ColorAdjustments;

		// Token: 0x04001EF8 RID: 7928
		private Beautify beautifySettings;

		// Token: 0x04001EF9 RID: 7929
		private Bloom bloom;

		// Token: 0x04001EFA RID: 7930
		private ChromaticAberration chromaticAberration;

		// Token: 0x04001EFB RID: 7931
		private ColorAdjustments colorAdjustments;
	}
}
