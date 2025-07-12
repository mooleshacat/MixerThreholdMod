using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000765 RID: 1893
	public class GenericOption : MonoBehaviour
	{
		// Token: 0x060032FC RID: 13052 RVA: 0x000D3C8E File Offset: 0x000D1E8E
		public virtual void Install()
		{
			if (this.onInstalled != null)
			{
				this.onInstalled.Invoke();
			}
			this.SetVisible();
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x000D3CA9 File Offset: 0x000D1EA9
		public virtual void Uninstall()
		{
			if (this.onUninstalled != null)
			{
				this.onUninstalled.Invoke();
			}
			this.SetInvisible();
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x000D3CC4 File Offset: 0x000D1EC4
		public virtual void SetVisible()
		{
			if (this.onSetVisible != null)
			{
				this.onSetVisible.Invoke();
			}
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x000D3CD9 File Offset: 0x000D1ED9
		public virtual void SetInvisible()
		{
			if (this.onSetInvisible != null)
			{
				this.onSetInvisible.Invoke();
			}
		}

		// Token: 0x040023FD RID: 9213
		[Header("Interface settings")]
		public string optionName;

		// Token: 0x040023FE RID: 9214
		public Color optionButtonColor;

		// Token: 0x040023FF RID: 9215
		public float optionPrice;

		// Token: 0x04002400 RID: 9216
		[Header("Events")]
		public UnityEvent onInstalled;

		// Token: 0x04002401 RID: 9217
		public UnityEvent onUninstalled;

		// Token: 0x04002402 RID: 9218
		public UnityEvent onSetVisible;

		// Token: 0x04002403 RID: 9219
		public UnityEvent onSetInvisible;
	}
}
