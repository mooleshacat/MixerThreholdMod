using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C8C RID: 3212
	public class TimedAccessZone : AccessZone
	{
		// Token: 0x06005A1D RID: 23069 RVA: 0x0017BF75 File Offset: 0x0017A175
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x0017BF9E File Offset: 0x0017A19E
		protected virtual void MinPass()
		{
			this.SetIsOpen(this.GetIsOpen());
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x0017BFAC File Offset: 0x0017A1AC
		protected virtual bool GetIsOpen()
		{
			return NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.OpenTime, this.CloseTime);
		}

		// Token: 0x0400422F RID: 16943
		[Header("Timing Settings")]
		public int OpenTime = 600;

		// Token: 0x04004230 RID: 16944
		public int CloseTime = 1800;
	}
}
