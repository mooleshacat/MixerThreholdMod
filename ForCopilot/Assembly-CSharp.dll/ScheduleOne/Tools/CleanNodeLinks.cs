using System;
using EasyButtons;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000884 RID: 2180
	public class CleanNodeLinks : MonoBehaviour
	{
		// Token: 0x06003B9C RID: 15260 RVA: 0x000FC700 File Offset: 0x000FA900
		[Button]
		public void Clean()
		{
			foreach (NodeLink nodeLink in base.GetComponentsInChildren<NodeLink>())
			{
				if (nodeLink.End == null)
				{
					Console.Log("Destroying link: " + nodeLink.name, null);
					UnityEngine.Object.DestroyImmediate(nodeLink);
				}
			}
		}
	}
}
