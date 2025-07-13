using System;
using FishNet.Connection;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005EE RID: 1518
	public class CheckpointManager : NetworkSingleton<CheckpointManager>
	{
		// Token: 0x06002555 RID: 9557 RVA: 0x00097D68 File Offset: 0x00095F68
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.WesternCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.WesternCheckpoint.Enable(connection);
			}
			if (this.DocksCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.DocksCheckpoint.Enable(connection);
			}
			if (this.NorthResidentialCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.NorthResidentialCheckpoint.Enable(connection);
			}
			if (this.WestResidentialCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.WestResidentialCheckpoint.Enable(connection);
			}
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x00097DE4 File Offset: 0x00095FE4
		public void SetCheckpointEnabled(CheckpointManager.ECheckpointLocation checkpoint, bool enabled, int requestedOfficers)
		{
			if (enabled)
			{
				this.GetCheckpoint(checkpoint).Enable(null);
				for (int i = 0; i < requestedOfficers; i++)
				{
					if (Singleton<Map>.Instance.PoliceStation.OfficerPool.Count <= 0)
					{
						return;
					}
					Singleton<Map>.Instance.PoliceStation.PullOfficer().AssignToCheckpoint(checkpoint);
				}
				return;
			}
			this.GetCheckpoint(checkpoint).Disable();
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00097E46 File Offset: 0x00096046
		public RoadCheckpoint GetCheckpoint(CheckpointManager.ECheckpointLocation loc)
		{
			switch (loc)
			{
			case CheckpointManager.ECheckpointLocation.Western:
				return this.WesternCheckpoint;
			case CheckpointManager.ECheckpointLocation.Docks:
				return this.DocksCheckpoint;
			case CheckpointManager.ECheckpointLocation.NorthResidential:
				return this.NorthResidentialCheckpoint;
			case CheckpointManager.ECheckpointLocation.WestResidential:
				return this.WestResidentialCheckpoint;
			default:
				return null;
			}
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00097E85 File Offset: 0x00096085
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x00097E9E File Offset: 0x0009609E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00097EB7 File Offset: 0x000960B7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00097EC5 File Offset: 0x000960C5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001B99 RID: 7065
		[Header("References")]
		public RoadCheckpoint WesternCheckpoint;

		// Token: 0x04001B9A RID: 7066
		public RoadCheckpoint DocksCheckpoint;

		// Token: 0x04001B9B RID: 7067
		public RoadCheckpoint NorthResidentialCheckpoint;

		// Token: 0x04001B9C RID: 7068
		public RoadCheckpoint WestResidentialCheckpoint;

		// Token: 0x04001B9D RID: 7069
		private bool dll_Excuted;

		// Token: 0x04001B9E RID: 7070
		private bool dll_Excuted;

		// Token: 0x020005EF RID: 1519
		public enum ECheckpointLocation
		{
			// Token: 0x04001BA0 RID: 7072
			Western,
			// Token: 0x04001BA1 RID: 7073
			Docks,
			// Token: 0x04001BA2 RID: 7074
			NorthResidential,
			// Token: 0x04001BA3 RID: 7075
			WestResidential
		}
	}
}
