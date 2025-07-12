using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000881 RID: 2177
	public class BetaEnabledGameObject : MonoBehaviour
	{
		// Token: 0x06003B95 RID: 15253 RVA: 0x000FC5EB File Offset: 0x000FA7EB
		private void Start()
		{
			if (!GameManager.IS_BETA)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
