using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A2 RID: 2210
	public class MonoBehaviourEvents : MonoBehaviour
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x000FD41D File Offset: 0x000FB61D
		private void Awake()
		{
			UnityEvent unityEvent = this.onAwake;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000FD42F File Offset: 0x000FB62F
		private void Start()
		{
			UnityEvent unityEvent = this.onStart;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x000FD441 File Offset: 0x000FB641
		private void Update()
		{
			UnityEvent unityEvent = this.onUpdate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04002ADD RID: 10973
		public UnityEvent onAwake;

		// Token: 0x04002ADE RID: 10974
		public UnityEvent onStart;

		// Token: 0x04002ADF RID: 10975
		public UnityEvent onUpdate;
	}
}
