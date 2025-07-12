using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B62 RID: 2914
	public class MainMenuPopup : Singleton<MainMenuPopup>
	{
		// Token: 0x06004D82 RID: 19842 RVA: 0x00146461 File Offset: 0x00144661
		public void Open(MainMenuPopup.Data data)
		{
			this.Open(data.Title, data.Description, data.IsBad);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0014647C File Offset: 0x0014467C
		public void Open(string title, string description, bool isBad)
		{
			this.Title.color = (isBad ? new Color32(byte.MaxValue, 115, 115, byte.MaxValue) : Color.white);
			this.Title.text = title;
			this.Description.text = description;
			this.Screen.Open(false);
		}

		// Token: 0x040039C3 RID: 14787
		public MainMenuScreen Screen;

		// Token: 0x040039C4 RID: 14788
		public TextMeshProUGUI Title;

		// Token: 0x040039C5 RID: 14789
		public TextMeshProUGUI Description;

		// Token: 0x02000B63 RID: 2915
		public class Data
		{
			// Token: 0x06004D85 RID: 19845 RVA: 0x001464E2 File Offset: 0x001446E2
			public Data(string title, string description, bool isBad)
			{
				this.Title = title;
				this.Description = description;
				this.IsBad = isBad;
			}

			// Token: 0x040039C6 RID: 14790
			public string Title;

			// Token: 0x040039C7 RID: 14791
			public string Description;

			// Token: 0x040039C8 RID: 14792
			public bool IsBad;
		}
	}
}
