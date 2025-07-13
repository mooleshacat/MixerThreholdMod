using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200088C RID: 2188
	public class DelayedUnityEvent : MonoBehaviour
	{
		// Token: 0x06003BBD RID: 15293 RVA: 0x000FCC6C File Offset: 0x000FAE6C
		[Button]
		public void Execute()
		{
			base.StartCoroutine(this.<Execute>g__Wait|3_0());
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000FCC8E File Offset: 0x000FAE8E
		[CompilerGenerated]
		private IEnumerator <Execute>g__Wait|3_0()
		{
			if (this.onDelayStart != null)
			{
				this.onDelayStart.Invoke();
			}
			yield return new WaitForSeconds(this.Delay);
			if (this.onDelayedExecute != null)
			{
				this.onDelayedExecute.Invoke();
			}
			yield break;
		}

		// Token: 0x04002AB0 RID: 10928
		public float Delay = 1f;

		// Token: 0x04002AB1 RID: 10929
		public UnityEvent onDelayStart;

		// Token: 0x04002AB2 RID: 10930
		public UnityEvent onDelayedExecute;
	}
}
