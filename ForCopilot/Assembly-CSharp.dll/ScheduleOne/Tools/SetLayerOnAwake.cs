using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B0 RID: 2224
	public class SetLayerOnAwake : MonoBehaviour
	{
		// Token: 0x06003C43 RID: 15427 RVA: 0x000FDEC9 File Offset: 0x000FC0C9
		private void Awake()
		{
			base.gameObject.layer = this.Layer.value;
		}

		// Token: 0x04002B07 RID: 11015
		public LayerMask Layer;
	}
}
