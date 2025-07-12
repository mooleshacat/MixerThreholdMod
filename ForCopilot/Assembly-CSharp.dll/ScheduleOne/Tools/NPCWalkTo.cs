using System;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A3 RID: 2211
	[RequireComponent(typeof(NPCMovement))]
	public class NPCWalkTo : MonoBehaviour
	{
		// Token: 0x06003C0C RID: 15372 RVA: 0x000FD454 File Offset: 0x000FB654
		private void Update()
		{
			this.timeSinceLastPath += Time.deltaTime;
			if (this.timeSinceLastPath >= this.RepathRate)
			{
				this.timeSinceLastPath = 0f;
				base.GetComponent<NPCMovement>().SetDestination(this.Target.position);
			}
		}

		// Token: 0x04002AE0 RID: 10976
		public Transform Target;

		// Token: 0x04002AE1 RID: 10977
		public float RepathRate = 0.5f;

		// Token: 0x04002AE2 RID: 10978
		private float timeSinceLastPath;
	}
}
