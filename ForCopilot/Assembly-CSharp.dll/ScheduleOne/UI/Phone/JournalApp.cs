using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AD3 RID: 2771
	public class JournalApp : App<JournalApp>
	{
		// Token: 0x06004A4B RID: 19019 RVA: 0x00138317 File Offset: 0x00136517
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0013831F File Offset: 0x0013651F
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0013834E File Offset: 0x0013654E
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (!open && this.currentDetailsPanel != null)
			{
				this.currentDetailsPanelQuest.DestroyDetailDisplay();
				this.currentDetailsPanel = null;
				this.currentDetailsPanelQuest = null;
			}
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00138384 File Offset: 0x00136584
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				this.RefreshDetailsPanel();
				this.NoTasksLabel.enabled = (Quest.ActiveQuests.Count == 0);
				this.NoDetailsLabel.enabled = (this.currentDetailsPanel == null);
			}
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x001383D4 File Offset: 0x001365D4
		private void RefreshDetailsPanel()
		{
			if (Quest.HoveredQuest != null)
			{
				if (this.currentDetailsPanelQuest != Quest.HoveredQuest)
				{
					if (this.currentDetailsPanel != null)
					{
						this.currentDetailsPanelQuest.DestroyDetailDisplay();
						this.currentDetailsPanel = null;
						this.currentDetailsPanelQuest = null;
					}
					this.currentDetailsPanel = Quest.HoveredQuest.CreateDetailDisplay(this.DetailsPanelContainer);
					this.currentDetailsPanelQuest = Quest.HoveredQuest;
					return;
				}
			}
			else if (this.currentDetailsPanel != null)
			{
				this.currentDetailsPanelQuest.DestroyDetailDisplay();
				this.currentDetailsPanel = null;
				this.currentDetailsPanelQuest = null;
			}
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x00138470 File Offset: 0x00136670
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x001384A6 File Offset: 0x001366A6
		protected virtual void MinPass()
		{
			bool isOpen = base.isOpen;
		}

		// Token: 0x0400369D RID: 13981
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x0400369E RID: 13982
		public Text NoTasksLabel;

		// Token: 0x0400369F RID: 13983
		public Text NoDetailsLabel;

		// Token: 0x040036A0 RID: 13984
		public RectTransform DetailsPanelContainer;

		// Token: 0x040036A1 RID: 13985
		[Header("Entry prefabs")]
		public GameObject GenericEntry;

		// Token: 0x040036A2 RID: 13986
		[Header("Details panel prefabs")]
		public GameObject GenericDetailsPanel;

		// Token: 0x040036A3 RID: 13987
		[Header("Quest Entry prefab")]
		public GameObject GenericQuestEntry;

		// Token: 0x040036A4 RID: 13988
		[Header("HUD entry prefabs")]
		public QuestHUDUI QuestHUDUIPrefab;

		// Token: 0x040036A5 RID: 13989
		public QuestEntryHUDUI QuestEntryHUDUIPrefab;

		// Token: 0x040036A6 RID: 13990
		protected Quest currentDetailsPanelQuest;

		// Token: 0x040036A7 RID: 13991
		protected RectTransform currentDetailsPanel;
	}
}
