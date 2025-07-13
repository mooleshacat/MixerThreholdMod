using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200088E RID: 2190
	public class DemoBoundary : MonoBehaviour
	{
		// Token: 0x06003BC6 RID: 15302 RVA: 0x000FCD27 File Offset: 0x000FAF27
		private void OnValidate()
		{
			if (this.Collider == null)
			{
				this.Collider = base.GetComponent<Collider>();
			}
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x000FCD43 File Offset: 0x000FAF43
		private void Start()
		{
			base.InvokeRepeating("UpdateBoundary", 0f, 0.25f);
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x000FCD5C File Offset: 0x000FAF5C
		private void UpdateBoundary()
		{
			if (Player.Local == null)
			{
				return;
			}
			Vector3 vector = this.Collider.transform.InverseTransformPoint(Player.Local.transform.position);
			this.Collider.enabled = (vector.x > 0f);
		}

		// Token: 0x04002AB6 RID: 10934
		public Collider Collider;
	}
}
