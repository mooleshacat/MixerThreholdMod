using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000AA1 RID: 2721
	public class WorldspacePopupUI : MonoBehaviour
	{
		// Token: 0x06004911 RID: 18705 RVA: 0x001327F7 File Offset: 0x001309F7
		public void SetFill(float fill)
		{
			this.FillImage.fillAmount = fill;
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x00132805 File Offset: 0x00130A05
		public void Destroy()
		{
			if (this.onDestroyed != null)
			{
				this.onDestroyed.Invoke();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x040035B4 RID: 13748
		[HideInInspector]
		public WorldspacePopup Popup;

		// Token: 0x040035B5 RID: 13749
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x040035B6 RID: 13750
		public Image FillImage;

		// Token: 0x040035B7 RID: 13751
		public UnityEvent onDestroyed;
	}
}
