using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Levelling;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E9 RID: 745
	[Serializable]
	public class Quest : MonoBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x00047CBD File Offset: 0x00045EBD
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x00047CC5 File Offset: 0x00045EC5
		public EQuestState QuestState { get; protected set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x00047CCE File Offset: 0x00045ECE
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x00047CD6 File Offset: 0x00045ED6
		public Guid GUID { get; protected set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x00047CDF File Offset: 0x00045EDF
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x00047CE7 File Offset: 0x00045EE7
		public bool IsTracked { get; protected set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x00047CF0 File Offset: 0x00045EF0
		public int ActiveEntryCount
		{
			get
			{
				return this.Entries.Count((QuestEntry x) => x.State == EQuestState.Active);
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00047D1C File Offset: 0x00045F1C
		public string Title
		{
			get
			{
				return this.GetQuestTitle();
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x00047D24 File Offset: 0x00045F24
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x00047D2C File Offset: 0x00045F2C
		public bool Expires { get; protected set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x00047D35 File Offset: 0x00045F35
		// (set) Token: 0x06001039 RID: 4153 RVA: 0x00047D3D File Offset: 0x00045F3D
		public GameDateTime Expiry { get; protected set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00047D46 File Offset: 0x00045F46
		public bool hudUIExists
		{
			get
			{
				return this.hudUI != null;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00047D54 File Offset: 0x00045F54
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00047D5C File Offset: 0x00045F5C
		public QuestHUDUI hudUI { get; private set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00047D68 File Offset: 0x00045F68
		public string SaveFolderName
		{
			get
			{
				return "Quest_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x00047D9C File Offset: 0x00045F9C
		public string SaveFileName
		{
			get
			{
				return "Quest_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x00047DD1 File Offset: 0x00045FD1
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x00047DD9 File Offset: 0x00045FD9
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x00047DE2 File Offset: 0x00045FE2
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x00047DEA File Offset: 0x00045FEA
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00047DF3 File Offset: 0x00045FF3
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00047DFB File Offset: 0x00045FFB
		public bool HasChanged { get; set; }

		// Token: 0x06001047 RID: 4167 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00047E04 File Offset: 0x00046004
		protected virtual void Start()
		{
			if (this.autoInitialize)
			{
				if (Player.Local != null)
				{
					this.<Start>g__Initialize|88_0();
				}
				else
				{
					Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Initialize|88_0));
				}
			}
			if (this.AutoCompleteOnAllEntriesComplete)
			{
				for (int i = 0; i < this.Entries.Count; i++)
				{
					this.Entries[i].onComplete.AddListener(new UnityAction(this.CheckAutoComplete));
				}
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00047E90 File Offset: 0x00046090
		public virtual void InitializeQuest(string title, string description, QuestEntryData[] entries, string guid)
		{
			if (guid == string.Empty)
			{
				guid = Guid.NewGuid().ToString();
			}
			if (entries.Length == 0 && this.Entries.Count == 0)
			{
				Console.LogWarning(title + " quest has no entries!", null);
			}
			base.gameObject.name = title;
			for (int i = 0; i < entries.Length; i++)
			{
				GameObject gameObject = new GameObject(entries[i].Name);
				gameObject.transform.SetParent(base.transform);
				QuestEntry questEntry = gameObject.AddComponent<QuestEntry>();
				this.Entries.Add(questEntry);
				questEntry.SetData(entries[i]);
			}
			this.GUID = new Guid(guid);
			GUIDManager.RegisterObject(this);
			this.title = title;
			this.Description = description;
			this.HasChanged = true;
			Quest.Quests.Add(this);
			this.InitializeSaveable();
			this.SetupJournalEntry();
			this.SetupHudUI();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00047FA2 File Offset: 0x000461A2
		public void ConfigureExpiry(bool expires, GameDateTime expiry)
		{
			this.Expires = expires;
			this.Expiry = expiry;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00047FB4 File Offset: 0x000461B4
		public virtual void Begin(bool network = true)
		{
			if (this.QuestState == EQuestState.Active)
			{
				return;
			}
			this.SetQuestState(EQuestState.Active, false);
			if (this.AutoStartFirstEntry && this.Entries.Count > 0)
			{
				this.Entries[0].SetState(EQuestState.Active, network);
			}
			if (this.TrackOnBegin)
			{
				this.SetIsTracked(true);
			}
			this.UpdateHUDUI();
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(this.GUID.ToString(), QuestManager.EQuestAction.Begin);
			}
			if (this.onQuestBegin != null)
			{
				this.onQuestBegin.Invoke();
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00048048 File Offset: 0x00046248
		public virtual void Complete(bool network = true)
		{
			if (this.QuestState == EQuestState.Completed)
			{
				return;
			}
			int num = 2;
			if (this.CompletionXP > 0 && InstanceFinder.IsServer && !Singleton<LoadManager>.Instance.IsLoading)
			{
				Console.Log("Adding XP for quest: " + this.Title, null);
				NetworkSingleton<LevelManager>.Instance.AddXP(this.CompletionXP);
			}
			this.SetQuestState(EQuestState.Completed, false);
			if (this.PlayQuestCompleteSound)
			{
				NetworkSingleton<QuestManager>.Instance.PlayCompleteQuestSound();
			}
			this.End();
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(this.GUID.ToString(), QuestManager.EQuestAction.Success);
			}
			if (this.onComplete != null)
			{
				this.onComplete.Invoke();
			}
			if (num != 2 && !Singleton<LoadManager>.Instance.IsLoading && this.onInitialComplete != null)
			{
				this.onInitialComplete.Invoke();
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0004811C File Offset: 0x0004631C
		public virtual void Fail(bool network = true)
		{
			this.SetQuestState(EQuestState.Failed, false);
			this.End();
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(this.GUID.ToString(), QuestManager.EQuestAction.Fail);
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0004815C File Offset: 0x0004635C
		public virtual void Expire(bool network = true)
		{
			if (this.QuestState == EQuestState.Expired)
			{
				return;
			}
			this.SetQuestState(EQuestState.Expired, false);
			if (this.ShouldSendExpiredNotification)
			{
				this.SendExpiredNotification();
			}
			this.End();
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(this.GUID.ToString(), QuestManager.EQuestAction.Expire);
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000481B4 File Offset: 0x000463B4
		public virtual void Cancel(bool network = true)
		{
			this.SetQuestState(EQuestState.Cancelled, false);
			this.End();
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(this.GUID.ToString(), QuestManager.EQuestAction.Cancel);
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000481F4 File Offset: 0x000463F4
		public virtual void End()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			Quest.ActiveQuests.Remove(this);
			this.DestroyDetailDisplay();
			this.DestroyJournalEntry();
			if (this.onQuestEnd != null)
			{
				this.onQuestEnd.Invoke(this.QuestState);
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0004825C File Offset: 0x0004645C
		public virtual void SetQuestState(EQuestState state, bool network = true)
		{
			this.QuestState = state;
			this.HasChanged = true;
			StateMachine.ChangeState();
			if (this.hudUI != null)
			{
				this.hudUI.gameObject.SetActive(this.IsTracked && (this.QuestState == EQuestState.Active || this.QuestState == EQuestState.Completed));
			}
			if (this.journalEntry != null)
			{
				this.journalEntry.gameObject.SetActive(this.ShouldShowJournalEntry());
			}
			for (int i = 0; i < this.Entries.Count; i++)
			{
				this.Entries[i].UpdateCompassElement();
			}
			if (state == EQuestState.Active && this.onActiveState != null)
			{
				this.onActiveState.Invoke();
			}
			if (network)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestState(this.GUID.ToString(), state);
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004833E File Offset: 0x0004653E
		protected virtual bool ShouldShowJournalEntry()
		{
			return this.QuestState == EQuestState.Active;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004834C File Offset: 0x0004654C
		public virtual void SetQuestEntryState(int entryIndex, EQuestState state, bool network = true)
		{
			if (entryIndex < 0 || entryIndex >= this.Entries.Count)
			{
				Console.LogWarning("Invalid entry index: " + entryIndex.ToString(), null);
				return;
			}
			this.HasChanged = true;
			this.Entries[entryIndex].SetState(state, network);
			if (state == EQuestState.Completed)
			{
				this.BopHUDUI();
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000483A8 File Offset: 0x000465A8
		protected virtual void MinPass()
		{
			if (this.Expires)
			{
				bool flag = this.GetMinsUntilExpiry() <= 120;
				if (this.entryTimeLabel != null)
				{
					this.entryTimeLabel.text = this.GetExpiryText();
				}
				if (this.criticalTimeBackground != null)
				{
					this.criticalTimeBackground.enabled = flag;
				}
				this.UpdateHUDUI();
				this.CheckExpiry();
				if (this.ShouldSendExpiryReminder && flag && !this.expiryReminderSent)
				{
					this.SendExpiryReminder();
					this.expiryReminderSent = true;
				}
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0004842F File Offset: 0x0004662F
		protected virtual void CheckExpiry()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.Expires)
			{
				return;
			}
			if (this.GetMinsUntilExpiry() <= 0 && this.CanExpire())
			{
				this.Expire(true);
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0004845C File Offset: 0x0004665C
		private void CheckAutoComplete()
		{
			bool flag = true;
			for (int i = 0; i < this.Entries.Count; i++)
			{
				if (this.Entries[i].State != EQuestState.Completed)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.Complete(true);
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanExpire()
		{
			return true;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x000484A3 File Offset: 0x000466A3
		protected virtual void SendExpiryReminder()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FFB43C>Quest Expiring Soon</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000484CA File Offset: 0x000466CA
		protected virtual void SendExpiredNotification()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FF6455>Quest Expired</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x000484F1 File Offset: 0x000466F1
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00048500 File Offset: 0x00046700
		public void SetSubtitle(string subtitle)
		{
			this.Subtitle = subtitle;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0004850C File Offset: 0x0004670C
		public virtual void SetIsTracked(bool tracked)
		{
			this.IsTracked = tracked;
			if (this.hudUI != null)
			{
				this.hudUI.gameObject.SetActive(tracked && this.QuestState == EQuestState.Active);
			}
			if (this.journalEntry != null)
			{
				this.trackedRect.gameObject.SetActive(tracked);
				this.journalEntry.GetComponent<Image>().color = (this.IsTracked ? new Color32(75, 75, 75, byte.MaxValue) : new Color32(150, 150, 150, byte.MaxValue));
			}
			this.HasChanged = true;
			for (int i = 0; i < this.Entries.Count; i++)
			{
				this.Entries[i].UpdateCompassElement();
			}
			if (this.onTrackChange != null)
			{
				this.onTrackChange.Invoke(tracked);
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000485F8 File Offset: 0x000467F8
		public virtual void SetupJournalEntry()
		{
			this.journalEntry = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<JournalApp>.Instance.GenericEntry, PlayerSingleton<JournalApp>.Instance.EntryContainer).GetComponent<RectTransform>();
			this.journalEntry.Find("Title").GetComponent<Text>().text = this.title;
			this.entryTitleRect = this.journalEntry.Find("Title").GetComponent<RectTransform>();
			this.trackedRect = this.journalEntry.Find("Tracked").GetComponent<RectTransform>();
			this.SetIsTracked(this.IsTracked);
			this.journalEntry.Find("Expiry").gameObject.SetActive(this.Expires);
			this.entryTimeLabel = this.journalEntry.Find("Expiry/Time").GetComponent<Text>();
			this.criticalTimeBackground = this.journalEntry.Find("Expiry/Critical").GetComponent<Image>();
			this.journalEntry.GetComponent<Button>().onClick.AddListener(new UnityAction(this.JournalEntryClicked));
			EventTrigger component = this.journalEntry.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.JournalEntryHoverStart();
			});
			component.triggers.Add(entry);
			UnityEngine.Object.Instantiate<RectTransform>(this.IconPrefab, this.journalEntry.Find("IconContainer")).GetComponent<RectTransform>().sizeDelta = new Vector2(25f, 25f);
			this.journalEntry.gameObject.SetActive(false);
			if (this.Expires)
			{
				this.entryTimeLabel.text = this.GetExpiryText();
			}
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0004879A File Offset: 0x0004699A
		private void DestroyJournalEntry()
		{
			if (this.journalEntry == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(this.journalEntry.gameObject);
			this.journalEntry = null;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000487C2 File Offset: 0x000469C2
		private void JournalEntryClicked()
		{
			this.SetIsTracked(!this.IsTracked);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000487D3 File Offset: 0x000469D3
		private void JournalEntryHoverStart()
		{
			Quest.HoveredQuest = this;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x000487DC File Offset: 0x000469DC
		public int GetMinsUntilExpiry()
		{
			int totalMinSum = NetworkSingleton<TimeManager>.Instance.GetTotalMinSum();
			int num = this.Expiry.GetMinSum() - totalMinSum;
			if (num > 0)
			{
				return num;
			}
			return 0;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0004880C File Offset: 0x00046A0C
		public string GetExpiryText()
		{
			int minsUntilExpiry = this.GetMinsUntilExpiry();
			if (minsUntilExpiry >= 60)
			{
				return Mathf.RoundToInt((float)minsUntilExpiry / 60f).ToString() + " hrs";
			}
			return minsUntilExpiry.ToString() + " min";
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00048858 File Offset: 0x00046A58
		public virtual QuestHUDUI SetupHudUI()
		{
			if (this.hudUI != null)
			{
				return this.hudUI;
			}
			this.hudUI = UnityEngine.Object.Instantiate<QuestHUDUI>(PlayerSingleton<JournalApp>.Instance.QuestHUDUIPrefab, Singleton<HUD>.Instance.QuestEntryContainer).GetComponent<QuestHUDUI>();
			this.hudUI.Initialize(this);
			if (this.onHudUICreated != null)
			{
				this.onHudUICreated();
			}
			this.hudUI.gameObject.SetActive(this.IsTracked && this.QuestState == EQuestState.Active);
			return this.hudUI;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000488E7 File Offset: 0x00046AE7
		public void UpdateHUDUI()
		{
			QuestHUDUI hudUI = this.hudUI;
			if (hudUI == null)
			{
				return;
			}
			hudUI.UpdateUI();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000488F9 File Offset: 0x00046AF9
		public void BopHUDUI()
		{
			if (this.hudUI == null)
			{
				return;
			}
			this.hudUI.BopIcon();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00048915 File Offset: 0x00046B15
		public virtual string GetQuestTitle()
		{
			return this.title;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00048920 File Offset: 0x00046B20
		public QuestEntry GetFirstActiveEntry()
		{
			for (int i = 0; i < this.Entries.Count; i++)
			{
				if (this.Entries[i].State == EQuestState.Active)
				{
					return this.Entries[i];
				}
			}
			return null;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00048965 File Offset: 0x00046B65
		private void DestroyHudUI()
		{
			if (this.hudUI != null)
			{
				UnityEngine.Object.Destroy(this.hudUI.gameObject);
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00048988 File Offset: 0x00046B88
		public virtual RectTransform CreateDetailDisplay(RectTransform parent)
		{
			if (this.detailPanel != null)
			{
				Console.LogWarning("Detail panel already exists!", null);
				return null;
			}
			if (!PlayerSingleton<JournalApp>.InstanceExists)
			{
				Console.LogWarning("Journal app does not exist!", null);
				return null;
			}
			this.detailPanel = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<JournalApp>.Instance.GenericDetailsPanel, parent).GetComponent<RectTransform>();
			this.detailPanel.Find("Title").GetComponent<Text>().text = this.title;
			this.detailPanel.Find("Description").GetComponent<Text>().text = this.Description;
			float preferredHeight = this.detailPanel.Find("Description").GetComponent<Text>().preferredHeight;
			this.detailPanel.Find("OuterContainer").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -45f - preferredHeight);
			RectTransform component = this.detailPanel.Find("OuterContainer/Entries").GetComponent<RectTransform>();
			int num = 0;
			for (int i = 0; i < this.Entries.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<JournalApp>.Instance.GenericQuestEntry, component).gameObject;
				gameObject.transform.Find("Title").GetComponent<Text>().text = this.Entries[i].Title;
				gameObject.transform.Find("State").GetComponent<Text>().text = this.Entries[i].State.ToString();
				gameObject.transform.Find("State").GetComponent<Text>().color = ((this.Entries[i].State == EQuestState.Active) ? new Color32(50, 50, 50, byte.MaxValue) : new Color32(150, 150, 150, byte.MaxValue));
				gameObject.gameObject.SetActive(this.Entries[i].State > EQuestState.Inactive);
				if (gameObject.gameObject.activeSelf)
				{
					num++;
				}
			}
			this.detailPanel.Find("OuterContainer/Contents").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -40f - (float)num * 35f);
			POI x = null;
			QuestEntry firstActiveEntry = this.GetFirstActiveEntry();
			if (firstActiveEntry != null)
			{
				x = firstActiveEntry.PoI;
			}
			GameObject gameObject2 = this.detailPanel.Find("OuterContainer/Contents/ShowOnMap").gameObject;
			gameObject2.SetActive(x != null && !GameManager.IS_TUTORIAL);
			gameObject2.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<CreateDetailDisplay>g__ShowOnMap|122_0));
			return this.detailPanel;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00048C46 File Offset: 0x00046E46
		public void DestroyDetailDisplay()
		{
			if (this.detailPanel != null)
			{
				UnityEngine.Object.Destroy(this.detailPanel.gameObject);
			}
			this.detailPanel = null;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool ShouldSave()
		{
			return true;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00048C70 File Offset: 0x00046E70
		public virtual SaveData GetSaveData()
		{
			List<QuestEntryData> list = new List<QuestEntryData>();
			for (int i = 0; i < this.Entries.Count; i++)
			{
				list.Add(this.Entries[i].GetSaveData());
			}
			return new QuestData(this.GUID.ToString(), this.QuestState, this.IsTracked, this.title, this.Description, this.Expires, new GameDateTimeData(this.Expiry), list.ToArray());
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00048CF8 File Offset: 0x00046EF8
		public string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00048D08 File Offset: 0x00046F08
		public virtual void Load(QuestData data)
		{
			this.SetQuestState(data.State, true);
			if (data.IsTracked)
			{
				this.SetIsTracked(true);
			}
			for (int i = 0; i < data.Entries.Length; i++)
			{
				int num = i;
				float versionNumber = SaveManager.GetVersionNumber(data.GameVersion);
				if (SaveManager.GetVersionNumber(Application.version) > versionNumber)
				{
					int num2 = i;
					int num3 = 0;
					while (num3 < num2 && num3 < this.Entries.Count)
					{
						if (SaveManager.GetVersionNumber(this.Entries[num3].EntryAddedIn) > versionNumber)
						{
							Console.Log("Increasing index for quest entry: " + this.Entries[num3].Title, null);
							num++;
							num2++;
						}
						num3++;
					}
				}
				this.SetQuestEntryState(num, data.Entries[i].State, true);
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00048DE0 File Offset: 0x00046FE0
		public static Quest GetQuest(string questName)
		{
			return Quest.Quests.FirstOrDefault((Quest x) => x.title.ToLower() == questName.ToLower());
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00048EB0 File Offset: 0x000470B0
		[CompilerGenerated]
		private void <Start>g__Initialize|88_0()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Initialize|88_0));
			if (!GUIDManager.IsGUIDValid(this.StaticGUID))
			{
				Console.LogWarning("Invalid GUID for quest: " + this.title + " Generating random GUID", null);
				this.StaticGUID = GUIDManager.GenerateUniqueGUID().ToString();
			}
			QuestEntryData[] entries = new QuestEntryData[0];
			this.InitializeQuest(this.title, this.Description, entries, this.StaticGUID);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00048F48 File Offset: 0x00047148
		[CompilerGenerated]
		private void <CreateDetailDisplay>g__ShowOnMap|122_0()
		{
			POI poi = null;
			QuestEntry firstActiveEntry = this.GetFirstActiveEntry();
			if (firstActiveEntry != null)
			{
				poi = firstActiveEntry.PoI;
			}
			if (poi != null && poi.UI != null && PlayerSingleton<MapApp>.InstanceExists && PlayerSingleton<JournalApp>.InstanceExists)
			{
				PlayerSingleton<MapApp>.Instance.FocusPosition(poi.UI.anchoredPosition);
				PlayerSingleton<JournalApp>.Instance.SetOpen(false);
				PlayerSingleton<MapApp>.Instance.SkipFocusPlayer = true;
				PlayerSingleton<MapApp>.Instance.SetOpen(true);
			}
		}

		// Token: 0x040010A5 RID: 4261
		public const int MAX_HUD_ENTRY_LABELS = 10;

		// Token: 0x040010A6 RID: 4262
		public const int CriticalExpiryThreshold = 120;

		// Token: 0x040010A7 RID: 4263
		public static List<Quest> Quests = new List<Quest>();

		// Token: 0x040010A8 RID: 4264
		public static Quest HoveredQuest = null;

		// Token: 0x040010A9 RID: 4265
		public static List<Quest> ActiveQuests = new List<Quest>();

		// Token: 0x040010AD RID: 4269
		[Header("Basic Settings")]
		[SerializeField]
		protected string title = string.Empty;

		// Token: 0x040010AE RID: 4270
		public string Subtitle = string.Empty;

		// Token: 0x040010AF RID: 4271
		public Action onSubtitleChanged;

		// Token: 0x040010B0 RID: 4272
		[TextArea(3, 10)]
		public string Description = string.Empty;

		// Token: 0x040010B1 RID: 4273
		public string StaticGUID = string.Empty;

		// Token: 0x040010B2 RID: 4274
		public bool TrackOnBegin;

		// Token: 0x040010B3 RID: 4275
		public EExpiryVisibility ExpiryVisibility;

		// Token: 0x040010B4 RID: 4276
		public bool AutoCompleteOnAllEntriesComplete;

		// Token: 0x040010B5 RID: 4277
		public bool PlayQuestCompleteSound = true;

		// Token: 0x040010B6 RID: 4278
		public int CompletionXP;

		// Token: 0x040010B9 RID: 4281
		[Header("Entries")]
		public bool AutoStartFirstEntry = true;

		// Token: 0x040010BA RID: 4282
		public List<QuestEntry> Entries = new List<QuestEntry>();

		// Token: 0x040010BB RID: 4283
		[Header("UI")]
		public RectTransform IconPrefab;

		// Token: 0x040010BC RID: 4284
		[Header("PoI Settings")]
		public GameObject PoIPrefab;

		// Token: 0x040010BD RID: 4285
		[Header("Events")]
		public UnityEvent onQuestBegin;

		// Token: 0x040010BE RID: 4286
		public UnityEvent<EQuestState> onQuestEnd;

		// Token: 0x040010BF RID: 4287
		public UnityEvent onActiveState;

		// Token: 0x040010C0 RID: 4288
		public UnityEvent<bool> onTrackChange;

		// Token: 0x040010C1 RID: 4289
		public UnityEvent onComplete;

		// Token: 0x040010C2 RID: 4290
		public UnityEvent onInitialComplete;

		// Token: 0x040010C3 RID: 4291
		[Header("Reminders")]
		public bool ShouldSendExpiryReminder = true;

		// Token: 0x040010C4 RID: 4292
		public bool ShouldSendExpiredNotification = true;

		// Token: 0x040010C5 RID: 4293
		protected RectTransform journalEntry;

		// Token: 0x040010C6 RID: 4294
		protected RectTransform entryTitleRect;

		// Token: 0x040010C7 RID: 4295
		protected RectTransform trackedRect;

		// Token: 0x040010C8 RID: 4296
		protected Text entryTimeLabel;

		// Token: 0x040010C9 RID: 4297
		protected Image criticalTimeBackground;

		// Token: 0x040010CA RID: 4298
		protected RectTransform detailPanel;

		// Token: 0x040010CC RID: 4300
		public Action onHudUICreated;

		// Token: 0x040010CD RID: 4301
		private bool expiryReminderSent;

		// Token: 0x040010CE RID: 4302
		private CompassManager.Element compassElement;

		// Token: 0x040010D2 RID: 4306
		protected bool autoInitialize = true;
	}
}
