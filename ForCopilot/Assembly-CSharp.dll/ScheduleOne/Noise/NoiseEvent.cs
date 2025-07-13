using System;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x02000573 RID: 1395
	public class NoiseEvent
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x0008AD8B File Offset: 0x00088F8B
		public NoiseEvent(Vector3 _origin, float _range, ENoiseType _type, GameObject _source = null)
		{
			this.origin = _origin;
			this.range = _range;
			this.type = _type;
			this.source = _source;
		}

		// Token: 0x040019BF RID: 6591
		public Vector3 origin;

		// Token: 0x040019C0 RID: 6592
		public float range;

		// Token: 0x040019C1 RID: 6593
		public ENoiseType type;

		// Token: 0x040019C2 RID: 6594
		public GameObject source;
	}
}
