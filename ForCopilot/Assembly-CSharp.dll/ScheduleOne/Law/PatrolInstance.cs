using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x02000608 RID: 1544
	[Serializable]
	public class PatrolInstance
	{
		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x00098F6E File Offset: 0x0009716E
		// (set) Token: 0x060025E0 RID: 9696 RVA: 0x00098F76 File Offset: 0x00097176
		public PatrolGroup ActiveGroup { get; protected set; }

		// Token: 0x060025E1 RID: 9697 RVA: 0x00098F80 File Offset: 0x00097180
		public void Evaluate()
		{
			if (this.ActiveGroup != null)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled))
			{
				this.StartPatrol();
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00098FDC File Offset: 0x000971DC
		public void StartPatrol()
		{
			if (this.ActiveGroup != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			if (PoliceStation.GetClosestPoliceStation(Vector3.zero).OfficerPool.Count == 0)
			{
				return;
			}
			this.ActiveGroup = Singleton<LawManager>.Instance.StartFootpatrol(this.Route, this.Members);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00099056 File Offset: 0x00097256
		private void MinPass()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime))
			{
				this.EndPatrol();
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00099078 File Offset: 0x00097278
		public void EndPatrol()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (this.ActiveGroup == null)
			{
				return;
			}
			this.ActiveGroup.DisbandGroup();
			this.ActiveGroup = null;
		}

		// Token: 0x04001BE4 RID: 7140
		public FootPatrolRoute Route;

		// Token: 0x04001BE5 RID: 7141
		public int Members = 2;

		// Token: 0x04001BE6 RID: 7142
		public int StartTime = 2000;

		// Token: 0x04001BE7 RID: 7143
		public int EndTime = 100;

		// Token: 0x04001BE8 RID: 7144
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001BE9 RID: 7145
		public bool OnlyIfCurfewEnabled;
	}
}
