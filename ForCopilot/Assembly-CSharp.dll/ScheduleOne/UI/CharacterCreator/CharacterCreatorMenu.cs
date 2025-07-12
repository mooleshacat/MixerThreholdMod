using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B8C RID: 2956
	public class CharacterCreatorMenu : MonoBehaviour
	{
		// Token: 0x06004E6B RID: 20075 RVA: 0x0014B680 File Offset: 0x00149880
		public void Start()
		{
			CharacterCreatorMenu.Window[] windows = this.Windows;
			for (int i = 0; i < windows.Length; i++)
			{
				windows[i].Close();
			}
			this.OpenWindow(0);
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0014B6B4 File Offset: 0x001498B4
		public void OpenWindow(int index)
		{
			if (this.openWindow != null)
			{
				this.openWindow.Close();
			}
			this.openWindowIndex = index;
			this.openWindow = this.Windows[index];
			this.openWindow.Open();
			this.CategoryLabel.text = this.openWindow.Name;
			this.BackButton.interactable = (index > 0);
			this.NextButton.interactable = (index < this.Windows.Length - 1);
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x0014B731 File Offset: 0x00149931
		public void Back()
		{
			this.OpenWindow(this.openWindowIndex - 1);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x0014B741 File Offset: 0x00149941
		public void Next()
		{
			this.OpenWindow(this.openWindowIndex + 1);
		}

		// Token: 0x04003AB9 RID: 15033
		public CharacterCreatorMenu.Window[] Windows;

		// Token: 0x04003ABA RID: 15034
		[Header("References")]
		public TextMeshProUGUI CategoryLabel;

		// Token: 0x04003ABB RID: 15035
		public Button BackButton;

		// Token: 0x04003ABC RID: 15036
		public Button NextButton;

		// Token: 0x04003ABD RID: 15037
		private int openWindowIndex;

		// Token: 0x04003ABE RID: 15038
		private CharacterCreatorMenu.Window openWindow;

		// Token: 0x02000B8D RID: 2957
		[Serializable]
		public class Window
		{
			// Token: 0x06004E70 RID: 20080 RVA: 0x0014B751 File Offset: 0x00149951
			public void Open()
			{
				this.Container.gameObject.SetActive(true);
			}

			// Token: 0x06004E71 RID: 20081 RVA: 0x0014B764 File Offset: 0x00149964
			public void Close()
			{
				this.Container.gameObject.SetActive(false);
			}

			// Token: 0x04003ABF RID: 15039
			public string Name;

			// Token: 0x04003AC0 RID: 15040
			public RectTransform Container;
		}
	}
}
