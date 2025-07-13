using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000720 RID: 1824
	public static class LayerUtility
	{
		// Token: 0x0600315E RID: 12638 RVA: 0x000CE244 File Offset: 0x000CC444
		public static void SetLayerRecursively(GameObject go, int layerNumber)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layerNumber;
			}
		}
	}
}
