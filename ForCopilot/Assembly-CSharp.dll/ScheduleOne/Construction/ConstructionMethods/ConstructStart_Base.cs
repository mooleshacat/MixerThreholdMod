using System;
using ScheduleOne.ConstructableScripts;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200076A RID: 1898
	public abstract class ConstructStart_Base : MonoBehaviour
	{
		// Token: 0x06003326 RID: 13094 RVA: 0x000D4327 File Offset: 0x000D2527
		public virtual void StartConstruction(string constructableID, Constructable_GridBased movedConstructable = null)
		{
			if (movedConstructable != null)
			{
				base.gameObject.GetComponent<ConstructUpdate_Base>().MovedConstructable = movedConstructable;
				movedConstructable.SetInvisible();
			}
		}
	}
}
