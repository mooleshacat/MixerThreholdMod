using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009F9 RID: 2553
	public class QualitySetter : MonoBehaviour
	{
		// Token: 0x060044B1 RID: 17585 RVA: 0x00120771 File Offset: 0x0011E971
		private void Awake()
		{
			base.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate(int x)
			{
				this.SetQuality(x);
			});
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0012078F File Offset: 0x0011E98F
		private void SetQuality(int quality)
		{
			Console.Log("Setting quality to " + quality.ToString(), null);
			QualitySettings.SetQualityLevel(quality);
		}
	}
}
