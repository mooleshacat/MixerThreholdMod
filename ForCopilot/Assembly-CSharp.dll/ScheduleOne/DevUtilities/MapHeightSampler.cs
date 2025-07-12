using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000723 RID: 1827
	public class MapHeightSampler
	{
		// Token: 0x06003169 RID: 12649 RVA: 0x000CE4B0 File Offset: 0x000CC6B0
		public static bool Sample(float x, out float y, float z)
		{
			y = 0f;
			Vector3 vector = new Vector3(x, MapHeightSampler.SampleHeight, z);
			Debug.DrawRay(vector, Vector3.down * MapHeightSampler.SampleDistance, Color.red, 100f);
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, Vector3.down, ref raycastHit, MapHeightSampler.SampleDistance, 1 << LayerMask.NameToLayer("Default"), 1))
			{
				y = raycastHit.point.y;
			}
			return false;
		}

		// Token: 0x040022CB RID: 8907
		private static float SampleHeight = 100f;

		// Token: 0x040022CC RID: 8908
		private static float SampleDistance = 200f;

		// Token: 0x040022CD RID: 8909
		public static Vector3 ResetPosition = new Vector3(-166.5f, 3f, -60f);
	}
}
