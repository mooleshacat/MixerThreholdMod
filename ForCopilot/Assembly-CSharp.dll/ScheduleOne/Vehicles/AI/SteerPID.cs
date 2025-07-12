using System;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x0200083A RID: 2106
	public class SteerPID
	{
		// Token: 0x060038F0 RID: 14576 RVA: 0x000F065C File Offset: 0x000EE85C
		public float GetNewValue(float error, PID_Parameters pid_parameters)
		{
			float num = -pid_parameters.P * error;
			this.error_sum = SteerPID.AddValueToAverage(this.error_sum, Time.deltaTime * error, 1000f);
			float num2 = num - pid_parameters.I * this.error_sum;
			float num3 = (error - this.error_old) / Time.deltaTime;
			float result = num2 - pid_parameters.D * num3;
			this.error_old = error;
			return result;
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x000F06BC File Offset: 0x000EE8BC
		public static float AddValueToAverage(float oldAverage, float valueToAdd, float count)
		{
			return (oldAverage * count + valueToAdd) / (count + 1f);
		}

		// Token: 0x040028E9 RID: 10473
		private float error_old;

		// Token: 0x040028EA RID: 10474
		private float error_sum;
	}
}
