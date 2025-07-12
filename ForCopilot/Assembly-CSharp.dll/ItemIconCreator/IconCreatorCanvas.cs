using System;
using UnityEngine;
using UnityEngine.UI;

namespace ItemIconCreator
{
	// Token: 0x02000230 RID: 560
	public class IconCreatorCanvas : MonoBehaviour
	{
		// Token: 0x06000BF5 RID: 3061 RVA: 0x00037373 File Offset: 0x00035573
		private void Awake()
		{
			IconCreatorCanvas.instance = this;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003737C File Offset: 0x0003557C
		public void SetInfo(int totalItens, int currentItem, string itemName, bool isRecording, KeyCode key)
		{
			this.borders.gameObject.SetActive(isRecording);
			if (!isRecording)
			{
				this.textLabel.text = "Go to your icon builder in hierarchy and press 'Build icons'";
				return;
			}
			this.textLabel.text = string.Concat(new string[]
			{
				currentItem.ToString(),
				" / ",
				totalItens.ToString(),
				" - ",
				itemName,
				"   |   Press <b>",
				key.ToString(),
				"</b> to continue"
			});
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003740E File Offset: 0x0003560E
		public void SetTakingPicture()
		{
			this.textLabel.text = "Generating icon...";
		}

		// Token: 0x04000D38 RID: 3384
		public Text textLabel;

		// Token: 0x04000D39 RID: 3385
		public GameObject borders;

		// Token: 0x04000D3A RID: 3386
		public static IconCreatorCanvas instance;
	}
}
