using System;
using System.Collections;
using ScheduleOne.Misc;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005DC RID: 1500
	[RequireComponent(typeof(ToggleableLight))]
	public class BlinkingLight : MonoBehaviour
	{
		// Token: 0x060024D2 RID: 9426 RVA: 0x000960E1 File Offset: 0x000942E1
		private void Awake()
		{
			this.light = base.GetComponent<ToggleableLight>();
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000960EF File Offset: 0x000942EF
		private void Update()
		{
			if (this.IsOn && this.blinkRoutine == null)
			{
				this.blinkRoutine = base.StartCoroutine(this.Blink());
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00096113 File Offset: 0x00094313
		private IEnumerator Blink()
		{
			while (this.IsOn)
			{
				this.light.isOn = true;
				yield return new WaitForSeconds(this.OnTime);
				this.light.isOn = false;
				yield return new WaitForSeconds(this.OffTime);
			}
			this.blinkRoutine = null;
			yield break;
		}

		// Token: 0x04001B31 RID: 6961
		public bool IsOn;

		// Token: 0x04001B32 RID: 6962
		public float OnTime = 0.5f;

		// Token: 0x04001B33 RID: 6963
		public float OffTime = 0.5f;

		// Token: 0x04001B34 RID: 6964
		private ToggleableLight light;

		// Token: 0x04001B35 RID: 6965
		private Coroutine blinkRoutine;
	}
}
