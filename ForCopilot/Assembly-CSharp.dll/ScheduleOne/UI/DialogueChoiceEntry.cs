using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A19 RID: 2585
	[Serializable]
	public class DialogueChoiceEntry
	{
		// Token: 0x04003251 RID: 12881
		public GameObject gameObject;

		// Token: 0x04003252 RID: 12882
		public TextMeshProUGUI text;

		// Token: 0x04003253 RID: 12883
		public Button button;

		// Token: 0x04003254 RID: 12884
		public GameObject notPossibleGameObject;

		// Token: 0x04003255 RID: 12885
		public TextMeshProUGUI notPossibleText;

		// Token: 0x04003256 RID: 12886
		public CanvasGroup canvasGroup;
	}
}
