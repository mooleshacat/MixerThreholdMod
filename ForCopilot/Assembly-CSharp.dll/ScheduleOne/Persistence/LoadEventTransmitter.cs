using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037D RID: 893
	public class LoadEventTransmitter : MonoBehaviour
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x0005988C File Offset: 0x00057A8C
		private void Start()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoadComplete));
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x000598A9 File Offset: 0x00057AA9
		private void OnLoadComplete()
		{
			if (this.onLoadComplete != null)
			{
				this.onLoadComplete.Invoke();
			}
		}

		// Token: 0x04001307 RID: 4871
		public UnityEvent onLoadComplete;
	}
}
