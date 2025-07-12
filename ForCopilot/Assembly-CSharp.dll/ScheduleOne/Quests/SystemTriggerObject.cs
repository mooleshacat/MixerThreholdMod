using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x0200030F RID: 783
	public class SystemTriggerObject : MonoBehaviour
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x0004D237 File Offset: 0x0004B437
		[Button]
		public void Trigger()
		{
			this.SystemTrigger.Trigger();
		}

		// Token: 0x04001159 RID: 4441
		public SystemTrigger SystemTrigger;
	}
}
