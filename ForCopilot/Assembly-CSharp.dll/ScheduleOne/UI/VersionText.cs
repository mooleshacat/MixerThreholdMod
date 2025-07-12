using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A9A RID: 2714
	public class VersionText : MonoBehaviour
	{
		// Token: 0x060048E6 RID: 18662 RVA: 0x00131AEC File Offset: 0x0012FCEC
		private void Awake()
		{
			base.GetComponent<TextMeshProUGUI>().text = "v" + Application.version;
		}
	}
}
