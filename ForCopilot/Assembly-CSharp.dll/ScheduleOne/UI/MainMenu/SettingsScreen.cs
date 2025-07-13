using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B6D RID: 2925
	public class SettingsScreen : MainMenuScreen
	{
		// Token: 0x06004DB8 RID: 19896 RVA: 0x0014720F File Offset: 0x0014540F
		protected override void Awake()
		{
			base.Awake();
			this.ApplyDisplayButton.onClick.AddListener(new UnityAction(this.ApplyDisplaySettings));
			this.ApplyDisplayButton.gameObject.SetActive(false);
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x00147244 File Offset: 0x00145444
		protected void Start()
		{
			for (int i = 0; i < this.Categories.Length; i++)
			{
				int index = i;
				this.Categories[i].Button.onClick.AddListener(delegate()
				{
					this.ShowCategory(index);
				});
			}
			this.ShowCategory(0);
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x001472A4 File Offset: 0x001454A4
		public void ShowCategory(int index)
		{
			for (int i = 0; i < this.Categories.Length; i++)
			{
				this.Categories[i].Button.interactable = (i != index);
				this.Categories[i].Panel.SetActive(i == index);
			}
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x001472F3 File Offset: 0x001454F3
		public void DisplayChanged()
		{
			this.ApplyDisplayButton.gameObject.SetActive(true);
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x00147308 File Offset: 0x00145508
		private void ApplyDisplaySettings()
		{
			this.ApplyDisplayButton.gameObject.SetActive(false);
			DisplaySettings displaySettings = Singleton<Settings>.Instance.DisplaySettings;
			DisplaySettings unappliedDisplaySettings = Singleton<Settings>.Instance.UnappliedDisplaySettings;
			Singleton<Settings>.Instance.ApplyDisplaySettings(unappliedDisplaySettings);
			this.ConfirmDisplaySettings.Open(displaySettings, unappliedDisplaySettings);
		}

		// Token: 0x040039EE RID: 14830
		public SettingsScreen.SettingsCategory[] Categories;

		// Token: 0x040039EF RID: 14831
		public Button ApplyDisplayButton;

		// Token: 0x040039F0 RID: 14832
		public ConfirmDisplaySettings ConfirmDisplaySettings;

		// Token: 0x02000B6E RID: 2926
		[Serializable]
		public class SettingsCategory
		{
			// Token: 0x040039F1 RID: 14833
			public Button Button;

			// Token: 0x040039F2 RID: 14834
			public GameObject Panel;
		}
	}
}
