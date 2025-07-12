using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000885 RID: 2181
	[Serializable]
	public class ColorSmoother
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x000FC750 File Offset: 0x000FA950
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x000FC758 File Offset: 0x000FA958
		public Color CurrentValue { get; private set; } = Color.white;

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x000FC761 File Offset: 0x000FA961
		// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x000FC769 File Offset: 0x000FA969
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x000FC772 File Offset: 0x000FA972
		public Color Default
		{
			get
			{
				return this.DefaultValue;
			}
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x000FC77A File Offset: 0x000FA97A
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000FC7B5 File Offset: 0x000FA9B5
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000FC7E4 File Offset: 0x000FA9E4
		public void SetDefault(Color value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000FC7FA File Offset: 0x000FA9FA
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000FC804 File Offset: 0x000FAA04
		public void AddOverride(Color value, int priority, string label)
		{
			ColorSmoother.Override @override = this.overrides.Find((ColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new ColorSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((ColorSmoother.Override x, ColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000FC890 File Offset: 0x000FAA90
		public void RemoveOverride(string label)
		{
			ColorSmoother.Override @override = this.overrides.Find((ColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((ColorSmoother.Override x, ColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000FC8FC File Offset: 0x000FAAFC
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			ColorSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Color.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x04002A98 RID: 10904
		[SerializeField]
		private Color DefaultValue = Color.white;

		// Token: 0x04002A99 RID: 10905
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x04002A9A RID: 10906
		[SerializeField]
		private List<ColorSmoother.Override> overrides = new List<ColorSmoother.Override>();

		// Token: 0x02000886 RID: 2182
		[Serializable]
		public class Override
		{
			// Token: 0x04002A9B RID: 10907
			public Color Value;

			// Token: 0x04002A9C RID: 10908
			public int Priority;

			// Token: 0x04002A9D RID: 10909
			public string Label;
		}
	}
}
