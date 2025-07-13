using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BE4 RID: 3044
	public class VMSBoard : MonoBehaviour
	{
		// Token: 0x06005107 RID: 20743 RVA: 0x001568F4 File Offset: 0x00154AF4
		public void SetText(string text, Color col)
		{
			this.Label.text = text;
			this.Label.color = col;
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x0015690E File Offset: 0x00154B0E
		public void SetText(string text)
		{
			this.SetText(text, new Color32(byte.MaxValue, 215, 50, byte.MaxValue));
		}

		// Token: 0x04003CCF RID: 15567
		[Header("References")]
		public TextMeshProUGUI Label;
	}
}
