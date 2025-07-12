using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200033B RID: 827
	[Serializable]
	public class MixerMap : ScriptableObject
	{
		// Token: 0x06001235 RID: 4661 RVA: 0x0004EE7C File Offset: 0x0004D07C
		public MixerMapEffect GetEffectAtPoint(Vector2 point)
		{
			if (point.magnitude > this.MapRadius)
			{
				return null;
			}
			for (int i = 0; i < this.Effects.Count; i++)
			{
				if (this.Effects[i].IsPointInEffect(point))
				{
					return this.Effects[i];
				}
			}
			return null;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0004EED4 File Offset: 0x0004D0D4
		public MixerMapEffect GetEffect(Property property)
		{
			for (int i = 0; i < this.Effects.Count; i++)
			{
				if (this.Effects[i].Property == property)
				{
					return this.Effects[i];
				}
			}
			return null;
		}

		// Token: 0x0400118D RID: 4493
		public float MapRadius;

		// Token: 0x0400118E RID: 4494
		public List<MixerMapEffect> Effects;
	}
}
