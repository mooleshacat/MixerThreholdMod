using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0E RID: 2574
	public class CrosshairText : MonoBehaviour
	{
		// Token: 0x0600453F RID: 17727 RVA: 0x00122AAE File Offset: 0x00120CAE
		private void Awake()
		{
			this.Hide();
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x00122AB6 File Offset: 0x00120CB6
		private void LateUpdate()
		{
			if (!this.setThisFrame)
			{
				this.Label.enabled = false;
			}
			this.setThisFrame = false;
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x00122AD4 File Offset: 0x00120CD4
		public void Show(string text, Color col = default(Color))
		{
			this.setThisFrame = true;
			this.Label.color = ((col != default(Color)) ? col : Color.white);
			this.Label.text = text;
			this.Label.enabled = true;
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x00122B24 File Offset: 0x00120D24
		public void Hide()
		{
			this.Label.enabled = false;
		}

		// Token: 0x04003207 RID: 12807
		public TextMeshProUGUI Label;

		// Token: 0x04003208 RID: 12808
		private bool setThisFrame;
	}
}
