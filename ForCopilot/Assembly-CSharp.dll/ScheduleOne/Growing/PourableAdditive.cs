using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008C1 RID: 2241
	public class PourableAdditive : Pourable
	{
		// Token: 0x06003C82 RID: 15490 RVA: 0x000FF080 File Offset: 0x000FD280
		protected override void PourAmount(float amount)
		{
			base.PourAmount(amount);
		}

		// Token: 0x04002B52 RID: 11090
		public const float NormalizedAmountForSuccess = 0.8f;

		// Token: 0x04002B53 RID: 11091
		public AdditiveDefinition AdditiveDefinition;

		// Token: 0x04002B54 RID: 11092
		public Color LiquidColor;

		// Token: 0x04002B55 RID: 11093
		private float pouredAmount;
	}
}
