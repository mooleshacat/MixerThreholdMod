using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x0200060C RID: 1548
	[Serializable]
	public class VehiclePatrolInstance
	{
		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x000995B3 File Offset: 0x000977B3
		private PoliceStation nearestStation
		{
			get
			{
				return PoliceStation.GetClosestPoliceStation(Vector3.zero);
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000995C0 File Offset: 0x000977C0
		public void Evaluate()
		{
			if (this.activeOfficer != null)
			{
				this.CheckEnd();
				return;
			}
			if (this.nearestStation.OfficerPool.Count == 0)
			{
				return;
			}
			this.latestStartTime = TimeManager.AddMinutesTo24HourTime(this.StartTime, 30);
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.latestStartTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled))
			{
				if (!this.startedThisCycle && Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement)
				{
					this.StartPatrol();
					return;
				}
			}
			else
			{
				this.startedThisCycle = false;
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x0009965C File Offset: 0x0009785C
		private void CheckEnd()
		{
			if (this.activeOfficer != null && !this.activeOfficer.VehiclePatrolBehaviour.Enabled)
			{
				this.activeOfficer = null;
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00099688 File Offset: 0x00097888
		public void StartPatrol()
		{
			if (this.activeOfficer != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			this.startedThisCycle = true;
			if (this.nearestStation.OfficerPool.Count == 0)
			{
				return;
			}
			this.activeOfficer = Singleton<LawManager>.Instance.StartVehiclePatrol(this.Route);
		}

		// Token: 0x04001C02 RID: 7170
		public VehiclePatrolRoute Route;

		// Token: 0x04001C03 RID: 7171
		public int StartTime = 2000;

		// Token: 0x04001C04 RID: 7172
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001C05 RID: 7173
		public bool OnlyIfCurfewEnabled;

		// Token: 0x04001C06 RID: 7174
		private PoliceOfficer activeOfficer;

		// Token: 0x04001C07 RID: 7175
		private int latestStartTime;

		// Token: 0x04001C08 RID: 7176
		private bool startedThisCycle;
	}
}
