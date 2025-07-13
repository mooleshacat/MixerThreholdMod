using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008BA RID: 2234
	public class FunctionalSeed : MonoBehaviour
	{
		// Token: 0x06003C62 RID: 15458 RVA: 0x000FE81E File Offset: 0x000FCA1E
		public void TriggerExit(Collider other)
		{
			if (other == this.SeedCollider && this.onSeedExitVial != null)
			{
				this.onSeedExitVial();
			}
		}

		// Token: 0x04002B2C RID: 11052
		public Action onSeedExitVial;

		// Token: 0x04002B2D RID: 11053
		public Draggable Vial;

		// Token: 0x04002B2E RID: 11054
		public Collider SeedBlocker;

		// Token: 0x04002B2F RID: 11055
		public VialCap Cap;

		// Token: 0x04002B30 RID: 11056
		public Collider SeedCollider;

		// Token: 0x04002B31 RID: 11057
		public Rigidbody SeedRigidbody;

		// Token: 0x04002B32 RID: 11058
		public TrashItem TrashPrefab;
	}
}
