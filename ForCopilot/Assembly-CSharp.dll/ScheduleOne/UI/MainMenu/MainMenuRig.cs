using System;
using System.IO;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B64 RID: 2916
	public class MainMenuRig : MonoBehaviour
	{
		// Token: 0x06004D86 RID: 19846 RVA: 0x001464FF File Offset: 0x001446FF
		public void Awake()
		{
			Singleton<LoadManager>.Instance.onSaveInfoLoaded.AddListener(new UnityAction(this.LoadStuff));
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0014651C File Offset: 0x0014471C
		private void LoadStuff()
		{
			bool flag = false;
			if (LoadManager.LastPlayedGame != null)
			{
				string text = Path.Combine(Path.Combine(LoadManager.LastPlayedGame.SavePath, "Players", "Player_0"), "Appearance.json");
				if (File.Exists(text))
				{
					string text2 = File.ReadAllText(text);
					BasicAvatarSettings basicAvatarSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
					JsonUtility.FromJsonOverwrite(text2, basicAvatarSettings);
					this.Avatar.LoadAvatarSettings(basicAvatarSettings.GetAvatarSettings());
					flag = true;
					Console.Log("Loaded player appearance from " + text, null);
				}
				float num = LoadManager.LastPlayedGame.Networth;
				for (int i = 0; i < this.CashPiles.Length; i++)
				{
					float displayedAmount = Mathf.Clamp(num, 0f, 100000f);
					this.CashPiles[i].SetDisplayedAmount(displayedAmount);
					num -= 100000f;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				this.Avatar.gameObject.SetActive(false);
			}
		}

		// Token: 0x040039C9 RID: 14793
		public Avatar Avatar;

		// Token: 0x040039CA RID: 14794
		public BasicAvatarSettings DefaultSettings;

		// Token: 0x040039CB RID: 14795
		public CashPile[] CashPiles;
	}
}
