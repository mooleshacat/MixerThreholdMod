using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200073D RID: 1853
	public class GUIDUtility : MonoBehaviour
	{
		// Token: 0x0600320D RID: 12813 RVA: 0x000D0CAC File Offset: 0x000CEEAC
		[Button]
		public void GenerateGUID()
		{
			Console.Log(Guid.NewGuid().ToString(), null);
		}
	}
}
