using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000738 RID: 1848
	public class ValueTracker
	{
		// Token: 0x060031EA RID: 12778 RVA: 0x000D03CE File Offset: 0x000CE5CE
		public ValueTracker(float HistoryDuration)
		{
			this.historyDuration = HistoryDuration;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onUpdate = (Action)Delegate.Combine(instance.onUpdate, new Action(this.Update));
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000D040E File Offset: 0x000CE60E
		public void Destroy()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onUpdate = (Action)Delegate.Remove(instance.onUpdate, new Action(this.Update));
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000D0438 File Offset: 0x000CE638
		public void Update()
		{
			int num = 0;
			while (num < this.valueHistory.Count && Time.timeSinceLevelLoad - this.valueHistory[num].time > this.historyDuration)
			{
				this.valueHistory.RemoveAt(num);
				num--;
				num++;
			}
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000D048A File Offset: 0x000CE68A
		public void SubmitValue(float value)
		{
			this.valueHistory.Add(new ValueTracker.Value(value, Time.timeSinceLevelLoad));
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x000D04A2 File Offset: 0x000CE6A2
		public float RecordedHistoryLength()
		{
			if (this.valueHistory.Count == 0)
			{
				return 0f;
			}
			return Time.timeSinceLevelLoad - this.valueHistory[0].time;
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x000D04D0 File Offset: 0x000CE6D0
		public float GetLowestValue()
		{
			ValueTracker.Value value = (from x in this.valueHistory
			orderby x.val
			select x).FirstOrDefault<ValueTracker.Value>();
			if (value != null)
			{
				return value.val;
			}
			return 0f;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000D051C File Offset: 0x000CE71C
		public float GetAverageValue()
		{
			if (this.valueHistory.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			foreach (ValueTracker.Value value in this.valueHistory)
			{
				num += value.val;
			}
			num /= (float)this.valueHistory.Count;
			return num;
		}

		// Token: 0x04002328 RID: 9000
		private float historyDuration;

		// Token: 0x04002329 RID: 9001
		private List<ValueTracker.Value> valueHistory = new List<ValueTracker.Value>();

		// Token: 0x02000739 RID: 1849
		public class Value
		{
			// Token: 0x060031F1 RID: 12785 RVA: 0x000D059C File Offset: 0x000CE79C
			public Value(float val, float time)
			{
				this.val = val;
				this.time = time;
			}

			// Token: 0x0400232A RID: 9002
			public float val;

			// Token: 0x0400232B RID: 9003
			public float time;
		}
	}
}
