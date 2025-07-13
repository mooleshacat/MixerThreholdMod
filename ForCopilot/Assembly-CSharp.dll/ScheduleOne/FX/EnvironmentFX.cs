using System;
using Funly.SkyStudio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Tools;
using UnityEngine;
using VolumetricFogAndMist2;

namespace ScheduleOne.FX
{
	// Token: 0x02000657 RID: 1623
	[ExecuteInEditMode]
	public class EnvironmentFX : Singleton<EnvironmentFX>
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000AE47E File Offset: 0x000AC67E
		public float normalizedEnvironmentalBrightness
		{
			get
			{
				return this.environmentalBrightnessCurve.Evaluate(((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal + NetworkSingleton<TimeManager>.Instance.TimeOnCurrentMinute / 1f) / 1440f);
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000AE4B0 File Offset: 0x000AC6B0
		protected override void Start()
		{
			base.Start();
			this.UpdateVisuals();
			this.FogEndDistanceController = new FloatSmoother();
			this.FogEndDistanceController.Initialize();
			this.FogEndDistanceController.SetSmoothingSpeed(0.2f);
			this.FogEndDistanceController.SetDefault(1f);
			if (Application.isPlaying && !this.started)
			{
				this.started = true;
				base.InvokeRepeating("UpdateVisuals", 0f, 0.1f);
			}
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000AE52C File Offset: 0x000AC72C
		private void Update()
		{
			if (Application.isEditor)
			{
				byte b = (byte)this.distanceTreeColorCurve.Evaluate(this.timeOfDayController.skyTime);
				this.distanceTreeMat.SetColor("_TintColor", new Color32(b, b, b, byte.MaxValue));
				this.grassMat.color = this.grassColorGradient.Evaluate(this.timeOfDayController.skyTime);
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000AE59C File Offset: 0x000AC79C
		private void UpdateVisuals()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			float num = (float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal + NetworkSingleton<TimeManager>.Instance.TimeOnCurrentMinute / 1f;
			this.timeOfDayController.skyTime = num / 1440f;
			RenderSettings.fogColor = this.fogColorGradient.Evaluate(this.timeOfDayController.skyTime);
			RenderSettings.fogEndDistance = this.fogEndDistanceCurve.Evaluate(this.timeOfDayController.skyTime) * this.fogEndDistanceMultiplier * this.FogEndDistanceController.CurrentValue;
			Color albedo = this.VolumetricFogColor.Evaluate(this.timeOfDayController.skyTime);
			albedo.a = this.VolumetricFogIntensityCurve.Evaluate(this.timeOfDayController.skyTime) * this.VolumetricFogIntensityMultiplier;
			this.VolumetricFog.profile.albedo = albedo;
			byte b = (byte)this.distanceTreeColorCurve.Evaluate(num / 1440f);
			this.distanceTreeMat.SetColor("_TintColor", new Color32(b, b, b, byte.MaxValue));
			this.grassMat.color = this.grassColorGradient.Evaluate(this.timeOfDayController.skyTime);
			Singleton<PostProcessingManager>.Instance.SetGodRayIntensity(this.godRayIntensityCurve.Evaluate(this.timeOfDayController.skyTime));
			Singleton<PostProcessingManager>.Instance.SetContrast(this.contrastCurve.Evaluate(this.timeOfDayController.skyTime) * this.contractMultiplier);
			Singleton<PostProcessingManager>.Instance.SetSaturation(this.saturationCurve.Evaluate(this.timeOfDayController.skyTime) * this.saturationMultiplier);
			Singleton<PostProcessingManager>.Instance.SetBloomThreshold(this.bloomThreshholdCurve.Evaluate(this.timeOfDayController.skyTime));
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000AE75A File Offset: 0x000AC95A
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x04001EB7 RID: 7863
		[Header("References")]
		[SerializeField]
		protected WindZone windZone;

		// Token: 0x04001EB8 RID: 7864
		[SerializeField]
		protected TimeOfDayController timeOfDayController;

		// Token: 0x04001EB9 RID: 7865
		public VolumetricFog VolumetricFog;

		// Token: 0x04001EBA RID: 7866
		public Light SunLight;

		// Token: 0x04001EBB RID: 7867
		public Light MoonLight;

		// Token: 0x04001EBC RID: 7868
		[Header("Fog")]
		[SerializeField]
		protected Gradient fogColorGradient;

		// Token: 0x04001EBD RID: 7869
		[SerializeField]
		protected AnimationCurve fogEndDistanceCurve;

		// Token: 0x04001EBE RID: 7870
		[SerializeField]
		protected float fogEndDistanceMultiplier = 0.01f;

		// Token: 0x04001EBF RID: 7871
		[Header("Height Fog")]
		[SerializeField]
		protected Gradient HeightFogColor;

		// Token: 0x04001EC0 RID: 7872
		[SerializeField]
		protected AnimationCurve HeightFogIntensityCurve;

		// Token: 0x04001EC1 RID: 7873
		[SerializeField]
		protected float HeightFogIntensityMultiplier = 0.5f;

		// Token: 0x04001EC2 RID: 7874
		[SerializeField]
		protected AnimationCurve HeightFogDirectionalIntensityCurve;

		// Token: 0x04001EC3 RID: 7875
		[Header("Volumetric Fog")]
		[SerializeField]
		protected Gradient VolumetricFogColor;

		// Token: 0x04001EC4 RID: 7876
		[SerializeField]
		protected AnimationCurve VolumetricFogIntensityCurve;

		// Token: 0x04001EC5 RID: 7877
		[SerializeField]
		protected float VolumetricFogIntensityMultiplier = 0.5f;

		// Token: 0x04001EC6 RID: 7878
		[Header("God rays")]
		[SerializeField]
		protected AnimationCurve godRayIntensityCurve;

		// Token: 0x04001EC7 RID: 7879
		[Header("Contrast")]
		[SerializeField]
		protected AnimationCurve contrastCurve;

		// Token: 0x04001EC8 RID: 7880
		[SerializeField]
		protected float contractMultiplier = 1f;

		// Token: 0x04001EC9 RID: 7881
		[Header("Saturation")]
		[SerializeField]
		protected AnimationCurve saturationCurve;

		// Token: 0x04001ECA RID: 7882
		[SerializeField]
		protected float saturationMultiplier = 1f;

		// Token: 0x04001ECB RID: 7883
		[Header("Grass")]
		[SerializeField]
		protected Material grassMat;

		// Token: 0x04001ECC RID: 7884
		[SerializeField]
		protected Gradient grassColorGradient;

		// Token: 0x04001ECD RID: 7885
		[Header("Trees")]
		public Material distanceTreeMat;

		// Token: 0x04001ECE RID: 7886
		public AnimationCurve distanceTreeColorCurve;

		// Token: 0x04001ECF RID: 7887
		[Header("Stealth settings")]
		public AnimationCurve environmentalBrightnessCurve;

		// Token: 0x04001ED0 RID: 7888
		[Header("Bloom")]
		public AnimationCurve bloomThreshholdCurve;

		// Token: 0x04001ED1 RID: 7889
		private bool started;

		// Token: 0x04001ED2 RID: 7890
		public FloatSmoother FogEndDistanceController;
	}
}
