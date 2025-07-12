using System;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x0200090A RID: 2314
	public class PourableAngleLimit : MonoBehaviour
	{
		// Token: 0x06003EA0 RID: 16032 RVA: 0x001076DA File Offset: 0x001058DA
		private void Awake()
		{
			this.Constraint.ClampUpDirection = true;
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x001076E8 File Offset: 0x001058E8
		public void FixedUpdate()
		{
			float upDirectionMaxDifference = Mathf.Lerp(this.AngleAtMinFill, this.AngleAtMaxFill, this.Pourable.NormalizedLiquidLevel);
			this.Constraint.UpDirectionMaxDifference = upDirectionMaxDifference;
			float angleFromUpToPour = Mathf.Lerp(this.PourAngleMinFill, this.PourAngleMaxFill, this.Pourable.NormalizedLiquidLevel);
			this.Pourable.AngleFromUpToPour = angleFromUpToPour;
		}

		// Token: 0x04002CA9 RID: 11433
		public PourableModule Pourable;

		// Token: 0x04002CAA RID: 11434
		public DraggableConstraint Constraint;

		// Token: 0x04002CAB RID: 11435
		[Header("Settings")]
		public float AngleAtMaxFill = 15f;

		// Token: 0x04002CAC RID: 11436
		public float AngleAtMinFill = 90f;

		// Token: 0x04002CAD RID: 11437
		public float PourAngleMaxFill = 15f;

		// Token: 0x04002CAE RID: 11438
		public float PourAngleMinFill = 90f;
	}
}
