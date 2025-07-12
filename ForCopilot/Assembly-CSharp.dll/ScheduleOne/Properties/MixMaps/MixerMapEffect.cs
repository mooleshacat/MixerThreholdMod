using System;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200033C RID: 828
	[Serializable]
	public class MixerMapEffect
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x0004EF1E File Offset: 0x0004D11E
		public bool IsPointInEffect(Vector2 point)
		{
			return Vector2.Distance(point, this.Position) < this.Radius;
		}

		// Token: 0x0400118F RID: 4495
		public Vector2 Position;

		// Token: 0x04001190 RID: 4496
		public float Radius;

		// Token: 0x04001191 RID: 4497
		public Property Property;
	}
}
