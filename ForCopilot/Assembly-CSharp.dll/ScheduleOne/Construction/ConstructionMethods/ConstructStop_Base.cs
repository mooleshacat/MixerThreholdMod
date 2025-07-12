using System;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200076D RID: 1901
	public class ConstructStop_Base : MonoBehaviour
	{
		// Token: 0x0600332C RID: 13100 RVA: 0x000D44A3 File Offset: 0x000D26A3
		public virtual void StopConstruction()
		{
			base.GetComponent<ConstructUpdate_Base>().ConstructionStop();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
