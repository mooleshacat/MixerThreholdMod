using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200089B RID: 2203
	[Serializable]
	public class HDRColorSmoother
	{
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003BEE RID: 15342 RVA: 0x000FD11D File Offset: 0x000FB31D
		// (set) Token: 0x06003BEF RID: 15343 RVA: 0x000FD125 File Offset: 0x000FB325
		public Color CurrentValue { get; private set; } = Color.white;

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x000FD12E File Offset: 0x000FB32E
		// (set) Token: 0x06003BF1 RID: 15345 RVA: 0x000FD136 File Offset: 0x000FB336
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x06003BF2 RID: 15346 RVA: 0x000FD13F File Offset: 0x000FB33F
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x000FD17A File Offset: 0x000FB37A
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x000FD1A9 File Offset: 0x000FB3A9
		public void SetDefault(Color value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x000FD1BF File Offset: 0x000FB3BF
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x000FD1C8 File Offset: 0x000FB3C8
		public void AddOverride(Color value, int priority, string label)
		{
			HDRColorSmoother.Override @override = this.overrides.Find((HDRColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new HDRColorSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((HDRColorSmoother.Override x, HDRColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x000FD254 File Offset: 0x000FB454
		public void RemoveOverride(string label)
		{
			HDRColorSmoother.Override @override = this.overrides.Find((HDRColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((HDRColorSmoother.Override x, HDRColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x000FD2C0 File Offset: 0x000FB4C0
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			HDRColorSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Color.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x04002ACE RID: 10958
		[ColorUsage(true, true)]
		[SerializeField]
		private Color DefaultValue = Color.white;

		// Token: 0x04002ACF RID: 10959
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x04002AD0 RID: 10960
		[SerializeField]
		private List<HDRColorSmoother.Override> overrides = new List<HDRColorSmoother.Override>();

		// Token: 0x0200089C RID: 2204
		[Serializable]
		public class Override
		{
			// Token: 0x04002AD1 RID: 10961
			public Color Value;

			// Token: 0x04002AD2 RID: 10962
			public int Priority;

			// Token: 0x04002AD3 RID: 10963
			public string Label;
		}
	}
}
