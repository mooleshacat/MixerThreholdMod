using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002EC RID: 748
	[Serializable]
	public class QuestEntry : MonoBehaviour
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x00048FFD File Offset: 0x000471FD
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x00049005 File Offset: 0x00047205
		[CodegenExclude]
		public Quest ParentQuest { get; private set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004900E File Offset: 0x0004720E
		[CodegenExclude]
		public string Title
		{
			get
			{
				return this.EntryTitle;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x00049016 File Offset: 0x00047216
		[CodegenExclude]
		public EQuestState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00049020 File Offset: 0x00047220
		protected virtual void Awake()
		{
			this.ParentQuest = base.GetComponentInParent<Quest>();
			this.ParentQuest.onQuestEnd.AddListener(delegate(EQuestState <p0>)
			{
				this.DestroyPoI();
			});
			this.ParentQuest.onTrackChange.AddListener(delegate(bool b)
			{
				this.UpdatePoI();
			});
			if (this.AutoComplete)
			{
				StateMachine.OnStateChange = (Action)Delegate.Combine(StateMachine.OnStateChange, new Action(this.EvaluateConditions));
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0004909C File Offset: 0x0004729C
		protected virtual void Start()
		{
			if (this.AutoCreatePoI && this.PoI == null)
			{
				this.CreatePoI();
			}
			if (!this.ParentQuest.Entries.Contains(this))
			{
				Console.LogError(string.Concat(new string[]
				{
					"Parent quest '",
					this.ParentQuest.GetQuestTitle(),
					"' does not contain entry '",
					this.EntryTitle,
					"'."
				}), null);
			}
			if (this.ParentQuest.hudUIExists)
			{
				this.CreateEntryUI();
			}
			else
			{
				Quest parentQuest = this.ParentQuest;
				parentQuest.onHudUICreated = (Action)Delegate.Combine(parentQuest.onHudUICreated, new Action(this.CreateEntryUI));
			}
			this.CreateCompassElement();
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0004915B File Offset: 0x0004735B
		private void OnValidate()
		{
			this.UpdateName();
			if (this.EntryAddedIn == null || this.EntryAddedIn == string.Empty)
			{
				this.EntryAddedIn = Application.version;
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00049188 File Offset: 0x00047388
		public virtual void MinPass()
		{
			if (this.AutoUpdatePoILocation && this.PoI != null)
			{
				this.PoI.transform.position = this.PoILocation.position;
				this.PoI.UpdatePosition();
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000491C6 File Offset: 0x000473C6
		public void SetData(QuestEntryData data)
		{
			this.EntryTitle = data.Name;
			this.SetState(data.State, false);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x000491E1 File Offset: 0x000473E1
		public void Begin()
		{
			this.SetState(EQuestState.Active, true);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000491EB File Offset: 0x000473EB
		public void Complete()
		{
			this.SetState(EQuestState.Completed, true);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000491F5 File Offset: 0x000473F5
		public void SetActive(bool network = true)
		{
			this.SetState(EQuestState.Active, network);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00049200 File Offset: 0x00047400
		public virtual void SetState(EQuestState newState, bool network = true)
		{
			EQuestState equestState = this.state;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			this.state = newState;
			if (newState == EQuestState.Active && equestState != EQuestState.Active)
			{
				if (this.onStart != null)
				{
					this.onStart.Invoke();
				}
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
			if (newState != EQuestState.Active && equestState == EQuestState.Active && this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
			if (newState == EQuestState.Completed && equestState != EQuestState.Completed)
			{
				if (this.onComplete != null)
				{
					this.onComplete.Invoke();
				}
				if (equestState == EQuestState.Active)
				{
					if (this.onInitialComplete != null)
					{
						this.onInitialComplete.Invoke();
					}
					NetworkSingleton<QuestManager>.Instance.PlayCompleteQuestEntrySound();
				}
				if (this.CompleteParentQuest)
				{
					this.ParentQuest.Complete(network);
				}
			}
			if (this.PoI != null)
			{
				this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			}
			this.ParentQuest.UpdateHUDUI();
			this.UpdateCompassElement();
			if (network)
			{
				int entryIndex = this.ParentQuest.Entries.ToList<QuestEntry>().IndexOf(this);
				NetworkSingleton<QuestManager>.Instance.SendQuestEntryState(this.ParentQuest.GUID.ToString(), entryIndex, newState);
			}
			this.UpdateName();
			StateMachine.ChangeState();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0004936E File Offset: 0x0004756E
		protected virtual bool ShouldShowPoI()
		{
			return this.State == EQuestState.Active && this.ParentQuest.IsTracked;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00049386 File Offset: 0x00047586
		protected virtual void UpdatePoI()
		{
			if (this.PoI != null)
			{
				this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x000493AC File Offset: 0x000475AC
		public void SetPoILocation(Vector3 location)
		{
			this.PoILocation.position = location;
			if (this.PoI != null)
			{
				this.PoI.transform.position = location;
				this.PoI.UpdatePosition();
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x000493E4 File Offset: 0x000475E4
		public void CreatePoI()
		{
			if (this.PoI != null)
			{
				Console.LogWarning("PoI already exists for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.ParentQuest == null)
			{
				Console.LogWarning("Parent quest is null for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.PoILocation == null)
			{
				Console.LogWarning("PoI location is null for quest entry " + this.EntryTitle, null);
				return;
			}
			this.PoI = UnityEngine.Object.Instantiate<GameObject>(this.ParentQuest.PoIPrefab, base.transform).GetComponent<POI>();
			this.PoI.transform.position = this.PoILocation.position;
			this.PoI.SetMainText(this.Title);
			this.PoI.UpdatePosition();
			this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			if (this.PoI.IconContainer != null)
			{
				this.<CreatePoI>g__CreateUI|36_0();
				return;
			}
			this.PoI.onUICreated.AddListener(new UnityAction(this.<CreatePoI>g__CreateUI|36_0));
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00049504 File Offset: 0x00047704
		public void DestroyPoI()
		{
			if (this.PoI != null)
			{
				UnityEngine.Object.Destroy(this.PoI.gameObject);
				this.PoI = null;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0004952C File Offset: 0x0004772C
		public void CreateCompassElement()
		{
			if (this.compassElement != null)
			{
				Console.LogWarning("Compass element already exists for quest: " + this.Title, null);
				return;
			}
			this.compassElement = Singleton<CompassManager>.Instance.AddElement(this.PoILocation, this.ParentQuest.IconPrefab, this.state == EQuestState.Active);
			this.UpdateCompassElement();
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00049588 File Offset: 0x00047788
		public void UpdateCompassElement()
		{
			if (this.compassElement == null)
			{
				return;
			}
			this.compassElement.Transform = this.PoILocation;
			this.compassElement.Visible = (this.ParentQuest.QuestState == EQuestState.Active && this.ParentQuest.IsTracked && this.state == EQuestState.Active && this.PoILocation != null);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x000495ED File Offset: 0x000477ED
		public QuestEntryData GetSaveData()
		{
			return new QuestEntryData(this.EntryTitle, this.state);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00049600 File Offset: 0x00047800
		private void UpdateName()
		{
			base.name = this.EntryTitle + " (" + this.state.ToString() + ")";
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004962E File Offset: 0x0004782E
		private void EvaluateConditions()
		{
			if (this.State != EQuestState.Active)
			{
				return;
			}
			if (this.AutoCompleteConditions.Evaluate())
			{
				this.SetState(EQuestState.Completed, true);
			}
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0004964F File Offset: 0x0004784F
		public void SetEntryTitle(string newTitle)
		{
			this.EntryTitle = newTitle;
			this.ParentQuest.UpdateHUDUI();
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00049664 File Offset: 0x00047864
		protected virtual void CreateEntryUI()
		{
			if (!this.ParentQuest.hudUIExists)
			{
				Console.LogWarning("Quest HUD UI does not exist for quest " + this.ParentQuest.GetQuestTitle(), null);
				return;
			}
			this.entryUI = UnityEngine.Object.Instantiate<QuestEntryHUDUI>(PlayerSingleton<JournalApp>.Instance.QuestEntryHUDUIPrefab, this.ParentQuest.hudUI.EntryContainer).GetComponent<QuestEntryHUDUI>();
			this.entryUI.Initialize(this);
			this.UpdateEntryUI();
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x000496D6 File Offset: 0x000478D6
		public virtual void UpdateEntryUI()
		{
			this.entryUI.UpdateUI();
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00049750 File Offset: 0x00047950
		[CompilerGenerated]
		private void <CreatePoI>g__CreateUI|36_0()
		{
			if (this.PoI != null)
			{
				Console.LogWarning("PoI already exists for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.ParentQuest == null)
			{
				Console.LogWarning("Parent quest is null for quest entry " + this.EntryTitle, null);
				return;
			}
			UnityEngine.Object.Instantiate<GameObject>(this.ParentQuest.IconPrefab.gameObject, this.PoI.IconContainer).GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
		}

		// Token: 0x040010D7 RID: 4311
		[Header("Naming")]
		[SerializeField]
		protected string EntryTitle = string.Empty;

		// Token: 0x040010D8 RID: 4312
		[SerializeField]
		protected EQuestState state;

		// Token: 0x040010D9 RID: 4313
		[Header("Settings")]
		public bool AutoComplete;

		// Token: 0x040010DA RID: 4314
		public Conditions AutoCompleteConditions;

		// Token: 0x040010DB RID: 4315
		public bool CompleteParentQuest;

		// Token: 0x040010DC RID: 4316
		public string EntryAddedIn = "0.0.1";

		// Token: 0x040010DD RID: 4317
		[Header("PoI Settings")]
		public bool AutoCreatePoI = true;

		// Token: 0x040010DE RID: 4318
		public Transform PoILocation;

		// Token: 0x040010DF RID: 4319
		public bool AutoUpdatePoILocation;

		// Token: 0x040010E0 RID: 4320
		public POI PoI;

		// Token: 0x040010E1 RID: 4321
		public UnityEvent onStart = new UnityEvent();

		// Token: 0x040010E2 RID: 4322
		public UnityEvent onEnd = new UnityEvent();

		// Token: 0x040010E3 RID: 4323
		public UnityEvent onComplete = new UnityEvent();

		// Token: 0x040010E4 RID: 4324
		public UnityEvent onInitialComplete = new UnityEvent();

		// Token: 0x040010E5 RID: 4325
		private CompassManager.Element compassElement;

		// Token: 0x040010E6 RID: 4326
		private QuestEntryHUDUI entryUI;
	}
}
