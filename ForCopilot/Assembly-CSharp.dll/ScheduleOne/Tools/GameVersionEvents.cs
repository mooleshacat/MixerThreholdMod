using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200089A RID: 2202
	public class GameVersionEvents : MonoBehaviour
	{
		// Token: 0x06003BEC RID: 15340 RVA: 0x000FD108 File Offset: 0x000FB308
		private void Start()
		{
			if (this.onFullGame != null)
			{
				this.onFullGame.Invoke();
			}
		}

		// Token: 0x04002ACA RID: 10954
		public UnityEvent onFullGame;

		// Token: 0x04002ACB RID: 10955
		public UnityEvent onDemoGame;
	}
}
