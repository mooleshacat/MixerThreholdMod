using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A1 RID: 2209
	public class IntervalEvent : MonoBehaviour
	{
		// Token: 0x06003C05 RID: 15365 RVA: 0x000FD3DC File Offset: 0x000FB5DC
		public void Start()
		{
			base.InvokeRepeating("Execute", this.Interval, this.Interval);
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000FD3F5 File Offset: 0x000FB5F5
		private void Execute()
		{
			if (this.Event != null)
			{
				this.Event.Invoke();
			}
		}

		// Token: 0x04002ADB RID: 10971
		public float Interval = 1f;

		// Token: 0x04002ADC RID: 10972
		public UnityEvent Event;
	}
}
