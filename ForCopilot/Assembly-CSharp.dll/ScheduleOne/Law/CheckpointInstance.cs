using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005ED RID: 1517
	[Serializable]
	public class CheckpointInstance
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x00097B22 File Offset: 0x00095D22
		// (set) Token: 0x0600254E RID: 9550 RVA: 0x00097B2A File Offset: 0x00095D2A
		public RoadCheckpoint activeCheckpoint { get; protected set; }

		// Token: 0x0600254F RID: 9551 RVA: 0x00097B34 File Offset: 0x00095D34
		public void Evaluate()
		{
			if (this.checkPoint == null)
			{
				this.checkPoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location);
			}
			if (this.activeCheckpoint != null)
			{
				return;
			}
			if (this.checkPoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && (!this.OnlyIfCurfewEnabled || NetworkSingleton<CurfewManager>.Instance.IsEnabled) && this.DistanceRequirementsMet())
			{
				this.EnableCheckpoint();
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00097BD0 File Offset: 0x00095DD0
		public void EnableCheckpoint()
		{
			if (this.activeCheckpoint != null)
			{
				Console.LogWarning("StartPatrol called but patrol is already active.", null);
				return;
			}
			if (PoliceStation.GetClosestPoliceStation(Vector3.zero).OfficerPool.Count == 0)
			{
				return;
			}
			this.activeCheckpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location);
			NetworkSingleton<CheckpointManager>.Instance.SetCheckpointEnabled(this.Location, true, this.Members);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x00097C64 File Offset: 0x00095E64
		private bool DistanceRequirementsMet()
		{
			float num;
			Player closestPlayer = Player.GetClosestPlayer(NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.Location).transform.position, out num, null);
			return NetworkSingleton<TimeManager>.Instance.SleepInProgress || closestPlayer == null || num >= 50f;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00097CB4 File Offset: 0x00095EB4
		private void MinPass()
		{
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime, this.EndTime) && this.DistanceRequirementsMet())
			{
				this.DisableCheckpoint();
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x00097CDC File Offset: 0x00095EDC
		public void DisableCheckpoint()
		{
			if (this.activeCheckpoint == null)
			{
				return;
			}
			NetworkSingleton<CheckpointManager>.Instance.SetCheckpointEnabled(this.Location, false, this.Members);
			this.activeCheckpoint = null;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x04001B90 RID: 7056
		public const float MIN_ACTIVATION_DISTANCE = 50f;

		// Token: 0x04001B91 RID: 7057
		public CheckpointManager.ECheckpointLocation Location;

		// Token: 0x04001B92 RID: 7058
		public int Members = 2;

		// Token: 0x04001B93 RID: 7059
		public int StartTime = 800;

		// Token: 0x04001B94 RID: 7060
		public int EndTime = 2000;

		// Token: 0x04001B95 RID: 7061
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001B96 RID: 7062
		public bool OnlyIfCurfewEnabled;

		// Token: 0x04001B97 RID: 7063
		private RoadCheckpoint checkPoint;
	}
}
