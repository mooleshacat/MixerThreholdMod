using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C8A RID: 3210
	public class ScheduledMaterialChange : MonoBehaviour
	{
		// Token: 0x06005A19 RID: 23065 RVA: 0x0017BDB4 File Offset: 0x00179FB4
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.Tick));
			this.SetMaterial(false);
			this.appliedInsideTimeRange = false;
			this.randomShift = UnityEngine.Random.Range(-this.TimeRangeRandomization, this.TimeRangeRandomization);
			this.Tick();
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x0017BE14 File Offset: 0x0017A014
		protected virtual void Tick()
		{
			if (!this.Enabled && this.appliedInsideTimeRange)
			{
				this.SetMaterial(false);
			}
			int min = TimeManager.AddMinutesTo24HourTime(this.TimeRangeMin, this.TimeRangeShift + this.randomShift);
			int max = TimeManager.AddMinutesTo24HourTime(this.TimeRangeMax, this.TimeRangeShift + this.randomShift);
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, max))
			{
				if (this.onState == ScheduledMaterialChange.EOnState.Undecided)
				{
					this.onState = ((UnityEngine.Random.Range(0f, 1f) > this.TurnOnChance) ? ScheduledMaterialChange.EOnState.Off : ScheduledMaterialChange.EOnState.On);
				}
			}
			else
			{
				this.onState = ScheduledMaterialChange.EOnState.Undecided;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, max) && this.onState == ScheduledMaterialChange.EOnState.On)
			{
				if (!this.appliedInsideTimeRange)
				{
					this.SetMaterial(true);
					return;
				}
			}
			else if (this.appliedInsideTimeRange)
			{
				this.SetMaterial(false);
			}
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x0017BEE0 File Offset: 0x0017A0E0
		private void SetMaterial(bool insideTimeRange)
		{
			if (this.Renderers == null || this.Renderers.Length == 0)
			{
				return;
			}
			this.appliedInsideTimeRange = insideTimeRange;
			Material material = this.Renderers[0].materials[this.MaterialIndex];
			material = (insideTimeRange ? this.InsideTimeRangeMaterial : this.OutsideTimeRangeMaterial);
			foreach (MeshRenderer meshRenderer in this.Renderers)
			{
				Material[] materials = meshRenderer.materials;
				materials[this.MaterialIndex] = material;
				meshRenderer.materials = materials;
			}
		}

		// Token: 0x0400421E RID: 16926
		public MeshRenderer[] Renderers;

		// Token: 0x0400421F RID: 16927
		public int MaterialIndex;

		// Token: 0x04004220 RID: 16928
		[Header("Settings")]
		public bool Enabled = true;

		// Token: 0x04004221 RID: 16929
		public Material OutsideTimeRangeMaterial;

		// Token: 0x04004222 RID: 16930
		public Material InsideTimeRangeMaterial;

		// Token: 0x04004223 RID: 16931
		public int TimeRangeMin;

		// Token: 0x04004224 RID: 16932
		public int TimeRangeMax;

		// Token: 0x04004225 RID: 16933
		public int TimeRangeShift;

		// Token: 0x04004226 RID: 16934
		public int TimeRangeRandomization;

		// Token: 0x04004227 RID: 16935
		[Range(0f, 1f)]
		public float TurnOnChance = 1f;

		// Token: 0x04004228 RID: 16936
		private bool appliedInsideTimeRange;

		// Token: 0x04004229 RID: 16937
		private ScheduledMaterialChange.EOnState onState;

		// Token: 0x0400422A RID: 16938
		private int randomShift;

		// Token: 0x02000C8B RID: 3211
		private enum EOnState
		{
			// Token: 0x0400422C RID: 16940
			Undecided,
			// Token: 0x0400422D RID: 16941
			On,
			// Token: 0x0400422E RID: 16942
			Off
		}
	}
}
