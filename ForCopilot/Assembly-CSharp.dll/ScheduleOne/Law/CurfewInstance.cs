using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x02000602 RID: 1538
	[Serializable]
	public class CurfewInstance
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002593 RID: 9619 RVA: 0x00098161 File Offset: 0x00096361
		// (set) Token: 0x06002594 RID: 9620 RVA: 0x00098169 File Offset: 0x00096369
		public bool Enabled { get; protected set; }

		// Token: 0x06002595 RID: 9621 RVA: 0x00098172 File Offset: 0x00096372
		public void Evaluate(bool ignoreSleepReq = false)
		{
			if (this.Enabled)
			{
				return;
			}
			if (Singleton<LawController>.Instance.LE_Intensity >= this.IntensityRequirement && (NetworkSingleton<TimeManager>.Instance.SleepInProgress || ignoreSleepReq))
			{
				this.Enable();
			}
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000981A3 File Offset: 0x000963A3
		private void MinPass()
		{
			if (this.Enabled)
			{
				if (Singleton<LawController>.Instance.LE_Intensity < this.IntensityRequirement)
				{
					this.shouldDisable = true;
				}
				if (this.shouldDisable && NetworkSingleton<TimeManager>.Instance.SleepInProgress)
				{
					this.Disable();
				}
			}
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000981E0 File Offset: 0x000963E0
		public void Enable()
		{
			CurfewInstance.ActiveInstance = this;
			this.Enabled = true;
			this.shouldDisable = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			NetworkSingleton<CurfewManager>.Instance.Enable(null);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00098234 File Offset: 0x00096434
		public void Disable()
		{
			this.Enabled = false;
			this.shouldDisable = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			if (CurfewInstance.ActiveInstance == this)
			{
				NetworkSingleton<CurfewManager>.Instance.Disable();
			}
		}

		// Token: 0x04001BB6 RID: 7094
		public static CurfewInstance ActiveInstance;

		// Token: 0x04001BB7 RID: 7095
		[Range(1f, 10f)]
		public int IntensityRequirement = 5;

		// Token: 0x04001BB9 RID: 7097
		[HideInInspector]
		public bool shouldDisable;
	}
}
