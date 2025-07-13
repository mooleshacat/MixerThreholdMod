using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002B6 RID: 694
	public class AnalogueClock : MonoBehaviour
	{
		// Token: 0x06000E92 RID: 3730 RVA: 0x000408D0 File Offset: 0x0003EAD0
		public void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			this.MinPass();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0004092F File Offset: 0x0003EB2F
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00040960 File Offset: 0x0003EB60
		public void MinPass()
		{
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(currentTime);
			float num = (float)(minSumFrom24HourTime % 60);
			float num2 = (float)(minSumFrom24HourTime / 60);
			float num3 = num / 60f * 360f;
			float d = num2 / 12f * 360f + num3 / 12f;
			if (currentTime == 1200 && this.onNoon != null)
			{
				this.onNoon.Invoke();
			}
			if (currentTime == 0 && this.onMidnight != null)
			{
				this.onMidnight.Invoke();
			}
			this.MinHand.localEulerAngles = this.RotationAxis * num3;
			this.HourHand.localEulerAngles = this.RotationAxis * d;
		}

		// Token: 0x04000F0F RID: 3855
		public Transform MinHand;

		// Token: 0x04000F10 RID: 3856
		public Transform HourHand;

		// Token: 0x04000F11 RID: 3857
		public Vector3 RotationAxis = Vector3.forward;

		// Token: 0x04000F12 RID: 3858
		public UnityEvent onNoon;

		// Token: 0x04000F13 RID: 3859
		public UnityEvent onMidnight;
	}
}
