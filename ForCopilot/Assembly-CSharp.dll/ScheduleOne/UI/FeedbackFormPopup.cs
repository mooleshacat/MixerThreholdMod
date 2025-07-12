using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A22 RID: 2594
	public class FeedbackFormPopup : MonoBehaviour
	{
		// Token: 0x060045D8 RID: 17880 RVA: 0x0012556C File Offset: 0x0012376C
		public void Open(string text)
		{
			if (this.Label != null)
			{
				this.Label.text = text;
			}
			base.gameObject.SetActive(true);
			this.closeTime = Time.unscaledTime + 4f;
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x001255A5 File Offset: 0x001237A5
		private void Update()
		{
			if (this.AutoClose && Time.unscaledTime > this.closeTime)
			{
				this.Close();
			}
		}

		// Token: 0x04003288 RID: 12936
		public TextMeshProUGUI Label;

		// Token: 0x04003289 RID: 12937
		public bool AutoClose = true;

		// Token: 0x0400328A RID: 12938
		private float closeTime;
	}
}
