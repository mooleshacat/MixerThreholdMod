using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Misc;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005E0 RID: 1504
	[RequireComponent(typeof(ToggleableLight))]
	public class LightTimer : MonoBehaviour
	{
		// Token: 0x060024E5 RID: 9445 RVA: 0x00096489 File Offset: 0x00094689
		protected virtual void Awake()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateState));
			this.toggleableLight = base.GetComponent<ToggleableLight>();
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000964BE File Offset: 0x000946BE
		private void Start()
		{
			this.UpdateState();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000964C6 File Offset: 0x000946C6
		protected virtual void UpdateState()
		{
			this.SetState(NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.StartTime + this.StartTimeOffset, this.EndTime));
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000964EB File Offset: 0x000946EB
		private void SetState(bool on)
		{
			this.toggleableLight.isOn = on;
		}

		// Token: 0x04001B44 RID: 6980
		[Header("Timing")]
		public int StartTime = 600;

		// Token: 0x04001B45 RID: 6981
		public int EndTime = 1800;

		// Token: 0x04001B46 RID: 6982
		public int StartTimeOffset;

		// Token: 0x04001B47 RID: 6983
		private ToggleableLight toggleableLight;
	}
}
