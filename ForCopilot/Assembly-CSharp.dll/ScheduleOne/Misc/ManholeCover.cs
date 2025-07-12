using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C5F RID: 3167
	public class ManholeCover : MonoBehaviour
	{
		// Token: 0x0600592B RID: 22827 RVA: 0x00178AD4 File Offset: 0x00176CD4
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x00178AFC File Offset: 0x00176CFC
		private void MinPass()
		{
			Color startColor = this.SteamColor.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			startColor.a = this.SteamAlpha.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.SteamParticles.startColor = startColor;
		}

		// Token: 0x04004151 RID: 16721
		public ParticleSystem SteamParticles;

		// Token: 0x04004152 RID: 16722
		public Gradient SteamColor;

		// Token: 0x04004153 RID: 16723
		public AnimationCurve SteamAlpha;
	}
}
