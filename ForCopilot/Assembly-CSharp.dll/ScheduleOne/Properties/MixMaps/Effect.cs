using System;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200033A RID: 826
	public class Effect : MonoBehaviour
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x0004EE1B File Offset: 0x0004D01B
		public Vector2 Position
		{
			get
			{
				return new Vector2(base.transform.position.x, base.transform.position.z);
			}
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0004EE42 File Offset: 0x0004D042
		public void OnValidate()
		{
			if (this.Property == null)
			{
				return;
			}
			base.gameObject.name = this.Property.Name;
		}

		// Token: 0x0400118B RID: 4491
		public Property Property;

		// Token: 0x0400118C RID: 4492
		[Range(0.05f, 3f)]
		public float Radius = 0.5f;
	}
}
