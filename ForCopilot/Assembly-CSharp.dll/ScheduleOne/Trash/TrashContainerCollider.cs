using System;
using UnityEngine;

namespace ScheduleOne.Trash
{
	// Token: 0x02000868 RID: 2152
	[RequireComponent(typeof(Rigidbody))]
	public class TrashContainerCollider : MonoBehaviour
	{
		// Token: 0x06003ABD RID: 15037 RVA: 0x000F8C3B File Offset: 0x000F6E3B
		public void OnTriggerEnter(Collider other)
		{
			this.Container.TriggerEnter(other);
		}

		// Token: 0x04002A1F RID: 10783
		public TrashContainer Container;
	}
}
