using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000894 RID: 2196
	[Serializable]
	public class FloatSmoother
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x000FCE23 File Offset: 0x000FB023
		// (set) Token: 0x06003BD5 RID: 15317 RVA: 0x000FCE2B File Offset: 0x000FB02B
		public float CurrentValue { get; private set; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x000FCE34 File Offset: 0x000FB034
		// (set) Token: 0x06003BD7 RID: 15319 RVA: 0x000FCE3C File Offset: 0x000FB03C
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000FCE45 File Offset: 0x000FB045
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000FCE80 File Offset: 0x000FB080
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x000FCEAF File Offset: 0x000FB0AF
		public void SetDefault(float value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x000FCEC5 File Offset: 0x000FB0C5
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x000FCECE File Offset: 0x000FB0CE
		public void SetSmoothingSpeed(float value)
		{
			this.SmoothingSpeed = value;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x000FCED8 File Offset: 0x000FB0D8
		public void AddOverride(float value, int priority, string label)
		{
			FloatSmoother.Override @override = this.overrides.Find((FloatSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new FloatSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((FloatSmoother.Override x, FloatSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000FCF64 File Offset: 0x000FB164
		public void RemoveOverride(string label)
		{
			FloatSmoother.Override @override = this.overrides.Find((FloatSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((FloatSmoother.Override x, FloatSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000FCFD0 File Offset: 0x000FB1D0
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			FloatSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Mathf.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x04002ABF RID: 10943
		[SerializeField]
		private float DefaultValue = 1f;

		// Token: 0x04002AC0 RID: 10944
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x04002AC1 RID: 10945
		private List<FloatSmoother.Override> overrides = new List<FloatSmoother.Override>();

		// Token: 0x02000895 RID: 2197
		public class Override
		{
			// Token: 0x04002AC2 RID: 10946
			public float Value;

			// Token: 0x04002AC3 RID: 10947
			public int Priority;

			// Token: 0x04002AC4 RID: 10948
			public string Label;
		}
	}
}
