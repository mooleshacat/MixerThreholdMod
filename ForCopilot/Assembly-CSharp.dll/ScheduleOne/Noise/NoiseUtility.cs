using System;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x02000575 RID: 1397
	public static class NoiseUtility
	{
		// Token: 0x060021AE RID: 8622 RVA: 0x0008ADB0 File Offset: 0x00088FB0
		public static void EmitNoise(Vector3 origin, ENoiseType type, float range, GameObject source = null)
		{
			NoiseEvent nEvent = new NoiseEvent(origin, range, type, source);
			for (int i = 0; i < Listener.listeners.Count; i++)
			{
				if (Listener.listeners[i].enabled && Vector3.Magnitude(origin - Listener.listeners[i].HearingOrigin.position) <= Listener.listeners[i].Sensitivity * range)
				{
					Listener.listeners[i].Notify(nEvent);
				}
			}
		}
	}
}
