using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000811 RID: 2065
	public class VehicleFX : MonoBehaviour
	{
		// Token: 0x06003831 RID: 14385 RVA: 0x000ED07C File Offset: 0x000EB27C
		public virtual void OnVehicleStart()
		{
			ParticleSystem[] array = this.exhaustFX;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x000ED0A8 File Offset: 0x000EB2A8
		public virtual void OnVehicleStop()
		{
			ParticleSystem[] array = this.exhaustFX;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Stop();
			}
		}

		// Token: 0x04002801 RID: 10241
		public ParticleSystem[] exhaustFX;
	}
}
