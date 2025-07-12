using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005E3 RID: 1507
	[RequireComponent(typeof(ReflectionProbe))]
	public class ReflectionProbeUpdater : MonoBehaviour
	{
		// Token: 0x060024F5 RID: 9461 RVA: 0x0009683D File Offset: 0x00094A3D
		private void OnValidate()
		{
			if (this.Probe == null)
			{
				this.Probe = base.GetComponent<ReflectionProbe>();
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0009685C File Offset: 0x00094A5C
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.UpdateProbe));
			this.UpdateProbe();
			if (ReflectionProbeUpdater.RenderRoutine == null)
			{
				ReflectionProbeUpdater.RenderRoutine = base.StartCoroutine(this.ProcessQueue());
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000968AD File Offset: 0x00094AAD
		private void UpdateProbe()
		{
			if (!ReflectionProbeUpdater.renderQueue.Contains(this.Probe))
			{
				ReflectionProbeUpdater.renderQueue.Add(this.Probe);
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000968D1 File Offset: 0x00094AD1
		private IEnumerator ProcessQueue()
		{
			int renderDuration_Frames = 14;
			for (;;)
			{
				if (ReflectionProbeUpdater.renderQueue.Count > 0)
				{
					ReflectionProbeUpdater.renderQueue[0].RenderProbe();
					ReflectionProbeUpdater.renderQueue.RemoveAt(0);
				}
				int num;
				for (int i = 0; i < renderDuration_Frames; i = num + 1)
				{
					yield return new WaitForEndOfFrame();
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04001B5B RID: 7003
		public ReflectionProbe Probe;

		// Token: 0x04001B5C RID: 7004
		private static List<ReflectionProbe> renderQueue = new List<ReflectionProbe>();

		// Token: 0x04001B5D RID: 7005
		private static Coroutine RenderRoutine = null;
	}
}
