using System;
using ScheduleOne.ConstructableScripts;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200076E RID: 1902
	public class ConstructUpdate_Base : MonoBehaviour
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x000D44BB File Offset: 0x000D26BB
		public bool isMoving
		{
			get
			{
				return this.MovedConstructable != null;
			}
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void LateUpdate()
		{
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000D44C9 File Offset: 0x000D26C9
		public virtual void ConstructionStop()
		{
			if (this.MovedConstructable != null)
			{
				this.MovedConstructable.RestoreVisibility();
			}
		}

		// Token: 0x04002418 RID: 9240
		public Constructable_GridBased MovedConstructable;
	}
}
