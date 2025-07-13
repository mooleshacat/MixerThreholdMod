using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Water
{
	// Token: 0x02000860 RID: 2144
	public class WaterManager : Singleton<WaterManager>
	{
		// Token: 0x06003A5B RID: 14939 RVA: 0x000F6EC0 File Offset: 0x000F50C0
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x000F6F1F File Offset: 0x000F511F
		private void MinPass()
		{
			this.usageThisMinute = 0f;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x000F6F2C File Offset: 0x000F512C
		private void DayPass()
		{
			this.usageAtTime.Clear();
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x000F6F3C File Offset: 0x000F513C
		public float GetTotalUsage()
		{
			float num = 0f;
			foreach (int key in this.usageAtTime.Keys)
			{
				num += this.usageAtTime[key];
			}
			return num;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000F6FA4 File Offset: 0x000F51A4
		public void ConsumeWater(float litres)
		{
			this.usageThisMinute += litres;
		}

		// Token: 0x040029F0 RID: 10736
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject waterPipePrefab;

		// Token: 0x040029F1 RID: 10737
		public static float pricePerL = 0.1f;

		// Token: 0x040029F2 RID: 10738
		private Dictionary<int, float> usageAtTime = new Dictionary<int, float>();

		// Token: 0x040029F3 RID: 10739
		private float usageThisMinute;
	}
}
