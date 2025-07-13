using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x020002B2 RID: 690
	public class TVHomeScreen : TVApp
	{
		// Token: 0x06000E77 RID: 3703 RVA: 0x000402E8 File Offset: 0x0003E4E8
		protected override void Awake()
		{
			base.Awake();
			TVApp[] apps = this.Apps;
			for (int i = 0; i < apps.Length; i++)
			{
				TVApp app = apps[i];
				app.PreviousScreen = this;
				app.CanvasGroup.alpha = 0f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.AppButtonPrefab, this.AppButtonContainer);
				gameObject.transform.Find("Icon").GetComponent<Image>().sprite = app.Icon;
				gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = app.AppName;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.AppSelected(app);
				});
				app.Close();
			}
			this.Interface.onPlayerAdded.AddListener(new UnityAction<Player>(this.PlayerChange));
			this.Interface.onPlayerRemoved.AddListener(new UnityAction<Player>(this.PlayerChange));
			this.Close();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0004040C File Offset: 0x0003E60C
		public override void Open()
		{
			base.Open();
			this.UpdateTimeLabel();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0004041A File Offset: 0x0003E61A
		public override void Close()
		{
			base.Close();
			if (this.skipExit)
			{
				this.skipExit = false;
				return;
			}
			this.Interface.Close();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0004043D File Offset: 0x0003E63D
		protected override void ActiveMinPass()
		{
			base.ActiveMinPass();
			this.UpdateTimeLabel();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0004044B File Offset: 0x0003E64B
		private void UpdateTimeLabel()
		{
			this.TimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00040469 File Offset: 0x0003E669
		private void AppSelected(TVApp app)
		{
			this.skipExit = true;
			this.Close();
			app.Open();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00040480 File Offset: 0x0003E680
		private void PlayerChange(Player player)
		{
			for (int i = 0; i < this.PlayerDisplays.Length; i++)
			{
				if (this.Interface.Players.Count > i)
				{
					this.PlayerDisplays[i].Find("Name").GetComponent<TextMeshProUGUI>().text = this.Interface.Players[i].PlayerName;
					this.PlayerDisplays[i].gameObject.SetActive(true);
				}
				else
				{
					this.PlayerDisplays[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04000EF9 RID: 3833
		[Header("References")]
		public TVInterface Interface;

		// Token: 0x04000EFA RID: 3834
		public TVApp[] Apps;

		// Token: 0x04000EFB RID: 3835
		public RectTransform AppButtonContainer;

		// Token: 0x04000EFC RID: 3836
		public RectTransform[] PlayerDisplays;

		// Token: 0x04000EFD RID: 3837
		public TextMeshProUGUI TimeLabel;

		// Token: 0x04000EFE RID: 3838
		[Header("Prefabs")]
		public GameObject AppButtonPrefab;

		// Token: 0x04000EFF RID: 3839
		private bool skipExit;
	}
}
