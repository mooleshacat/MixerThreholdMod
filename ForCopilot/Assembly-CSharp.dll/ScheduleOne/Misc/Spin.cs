using System;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C66 RID: 3174
	public class Spin : MonoBehaviour
	{
		// Token: 0x0600595B RID: 22875 RVA: 0x001799AB File Offset: 0x00177BAB
		private void Update()
		{
			base.transform.Rotate(this.Axis, this.Speed * Time.deltaTime, Space.Self);
		}

		// Token: 0x04004180 RID: 16768
		public Vector3 Axis;

		// Token: 0x04004181 RID: 16769
		public float Speed;
	}
}
