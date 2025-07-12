using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000905 RID: 2309
	public abstract class ItemModule : MonoBehaviour
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003E88 RID: 16008 RVA: 0x00107316 File Offset: 0x00105516
		// (set) Token: 0x06003E89 RID: 16009 RVA: 0x0010731E File Offset: 0x0010551E
		public StationItem Item { get; protected set; }

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x00107327 File Offset: 0x00105527
		// (set) Token: 0x06003E8B RID: 16011 RVA: 0x0010732F File Offset: 0x0010552F
		public bool IsModuleActive { get; protected set; }

		// Token: 0x06003E8C RID: 16012 RVA: 0x00107338 File Offset: 0x00105538
		public virtual void ActivateModule(StationItem item)
		{
			this.IsModuleActive = true;
			this.Item = item;
		}
	}
}
