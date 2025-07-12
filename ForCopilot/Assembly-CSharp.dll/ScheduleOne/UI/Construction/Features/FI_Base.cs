using System;
using ScheduleOne.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Construction.Features
{
	// Token: 0x02000BD6 RID: 3030
	public class FI_Base : MonoBehaviour
	{
		// Token: 0x06005071 RID: 20593 RVA: 0x001546A1 File Offset: 0x001528A1
		public virtual void Initialize(Feature _feature)
		{
			this.feature = _feature;
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001546AA File Offset: 0x001528AA
		public virtual void Close()
		{
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04003C69 RID: 15465
		protected Feature feature;

		// Token: 0x04003C6A RID: 15466
		public UnityEvent onClose;
	}
}
