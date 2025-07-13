using System;
using System.Collections.Generic;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Property.Utilities.Power
{
	// Token: 0x02000862 RID: 2146
	public class PowerManager : Singleton<PowerManager>
	{
		// Token: 0x06003A75 RID: 14965 RVA: 0x000F78D4 File Offset: 0x000F5AD4
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000F7933 File Offset: 0x000F5B33
		private void MinPass()
		{
			this.usageThisMinute = 0f;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x000F7940 File Offset: 0x000F5B40
		private void DayPass()
		{
			this.usageAtTime.Clear();
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x000F7950 File Offset: 0x000F5B50
		public float GetTotalUsage()
		{
			float num = 0f;
			foreach (int key in this.usageAtTime.Keys)
			{
				num += this.usageAtTime[key];
			}
			return num;
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x000F79B8 File Offset: 0x000F5BB8
		public void ConsumePower(float kwh)
		{
			this.usageThisMinute += kwh;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x000F79C8 File Offset: 0x000F5BC8
		public PowerLine CreatePowerLine(PowerNode nodeA, PowerNode nodeB, Property p)
		{
			if (!PowerLine.CanNodesBeConnected(nodeA, nodeB))
			{
				Console.LogWarning("Nodes can't be connected!", null);
				return null;
			}
			PowerLine component = Singleton<ConstructionManager>.Instance.CreateConstructable("Utilities/PowerLine/PowerLine").GetComponent<PowerLine>();
			component.transform.SetParent(p.Container.transform);
			component.InitializePowerLine(nodeA, nodeB);
			return component;
		}

		// Token: 0x04002A00 RID: 10752
		[Header("Prefabs")]
		public GameObject powerLineSegmentPrefab;

		// Token: 0x04002A01 RID: 10753
		public static float pricePerkWh = 0.25f;

		// Token: 0x04002A02 RID: 10754
		private Dictionary<int, float> usageAtTime = new Dictionary<int, float>();

		// Token: 0x04002A03 RID: 10755
		private float usageThisMinute;
	}
}
