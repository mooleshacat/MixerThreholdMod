using System;
using LiquidVolumeFX;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000906 RID: 2310
	public class LiquidContainer : MonoBehaviour
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003E8E RID: 16014 RVA: 0x00107348 File Offset: 0x00105548
		// (set) Token: 0x06003E8F RID: 16015 RVA: 0x00107350 File Offset: 0x00105550
		public float CurrentLiquidLevel { get; private set; }

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x00107359 File Offset: 0x00105559
		// (set) Token: 0x06003E91 RID: 16017 RVA: 0x00107361 File Offset: 0x00105561
		public Color LiquidColor { get; private set; } = Color.white;

		// Token: 0x06003E92 RID: 16018 RVA: 0x0010736A File Offset: 0x0010556A
		private void Awake()
		{
			this.liquidMesh = this.LiquidVolume.GetComponent<MeshRenderer>();
			this.SetLiquidColor(this.LiquidVolume.liquidColor1, true, true);
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00107390 File Offset: 0x00105590
		private void Start()
		{
			this.LiquidVolume.directionalLight = Singleton<EnvironmentFX>.Instance.SunLight;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x001073CD File Offset: 0x001055CD
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x001073FC File Offset: 0x001055FC
		private void MinPass()
		{
			this.UpdateLighting();
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x00107404 File Offset: 0x00105604
		private void UpdateLighting()
		{
			if (this.AdjustMurkiness)
			{
				float t = Mathf.Abs((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f - 0.5f) / 0.5f;
				float b = Mathf.Lerp(1f, 0.75f, t);
				this.SetLiquidColor(this.LiquidColor * b, false, false);
			}
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x00107464 File Offset: 0x00105664
		public void SetLiquidLevel(float level, bool debug = false)
		{
			if (debug)
			{
				Console.Log("setting liquid level to: " + level.ToString(), null);
			}
			this.CurrentLiquidLevel = Mathf.Clamp01(level);
			this.LiquidVolume.level = Mathf.Lerp(0f, this.MaxLevel, this.CurrentLiquidLevel);
			if (this.liquidMesh != null)
			{
				this.liquidMesh.enabled = (this.CurrentLiquidLevel > 0.01f);
			}
			if (this.Collider != null && this.ColliderTransform_Min != null && this.ColliderTransform_Max != null)
			{
				this.Collider.transform.localPosition = Vector3.Lerp(this.ColliderTransform_Min.localPosition, this.ColliderTransform_Max.localPosition, this.CurrentLiquidLevel);
				this.Collider.transform.localScale = Vector3.Lerp(this.ColliderTransform_Min.localScale, this.ColliderTransform_Max.localScale, this.CurrentLiquidLevel);
			}
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0010756A File Offset: 0x0010576A
		public void SetLiquidColor(Color color, bool setColorVariable = true, bool updateLigting = true)
		{
			if (setColorVariable)
			{
				this.LiquidColor = color;
			}
			this.LiquidVolume.liquidColor1 = color;
			this.LiquidVolume.liquidColor2 = color;
			if (updateLigting)
			{
				this.UpdateLighting();
			}
		}

		// Token: 0x04002C9B RID: 11419
		[Header("Settings")]
		[Range(0f, 1f)]
		public float Viscosity = 0.4f;

		// Token: 0x04002C9C RID: 11420
		public bool AdjustMurkiness = true;

		// Token: 0x04002C9D RID: 11421
		[Header("References")]
		public LiquidVolume LiquidVolume;

		// Token: 0x04002C9E RID: 11422
		public LiquidVolumeCollider Collider;

		// Token: 0x04002C9F RID: 11423
		public Transform ColliderTransform_Min;

		// Token: 0x04002CA0 RID: 11424
		public Transform ColliderTransform_Max;

		// Token: 0x04002CA1 RID: 11425
		[Header("Visuals Settings")]
		public float MaxLevel = 1f;

		// Token: 0x04002CA2 RID: 11426
		private MeshRenderer liquidMesh;
	}
}
