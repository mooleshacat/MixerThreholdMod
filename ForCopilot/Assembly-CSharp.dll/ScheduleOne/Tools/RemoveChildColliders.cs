using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AC RID: 2220
	public class RemoveChildColliders : MonoBehaviour
	{
		// Token: 0x06003C38 RID: 15416 RVA: 0x000FDCCC File Offset: 0x000FBECC
		private void Start()
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}
	}
}
