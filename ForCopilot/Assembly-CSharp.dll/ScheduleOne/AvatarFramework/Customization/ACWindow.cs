using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E7 RID: 2535
	public class ACWindow : MonoBehaviour
	{
		// Token: 0x0600445D RID: 17501 RVA: 0x0011F1F0 File Offset: 0x0011D3F0
		private void Start()
		{
			this.TitleText.text = this.WindowTitle;
			if (this.Predecessor != null)
			{
				this.BackButton.onClick.AddListener(new UnityAction(this.Close));
				this.BackButton.gameObject.SetActive(true);
			}
			else
			{
				this.BackButton.gameObject.SetActive(false);
			}
			if (this.Predecessor != null)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x000C6C1B File Offset: 0x000C4E1B
		public void Open()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0011F276 File Offset: 0x0011D476
		public void Close()
		{
			base.gameObject.SetActive(false);
			if (this.Predecessor != null)
			{
				this.Predecessor.Open();
			}
		}

		// Token: 0x04003137 RID: 12599
		[Header("Settings")]
		public string WindowTitle;

		// Token: 0x04003138 RID: 12600
		public ACWindow Predecessor;

		// Token: 0x04003139 RID: 12601
		[Header("References")]
		public TextMeshProUGUI TitleText;

		// Token: 0x0400313A RID: 12602
		public Button BackButton;
	}
}
