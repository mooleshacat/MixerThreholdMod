using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x020004A7 RID: 1191
	public class NPCScheduleManager : MonoBehaviour
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x0006E3BE File Offset: 0x0006C5BE
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x0006E3C6 File Offset: 0x0006C5C6
		public bool ScheduleEnabled { get; protected set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x0006E3CF File Offset: 0x0006C5CF
		// (set) Token: 0x06001900 RID: 6400 RVA: 0x0006E3D7 File Offset: 0x0006C5D7
		public bool CurfewModeEnabled { get; protected set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x0006E3E0 File Offset: 0x0006C5E0
		// (set) Token: 0x06001902 RID: 6402 RVA: 0x0006E3E8 File Offset: 0x0006C5E8
		public NPCAction ActiveAction { get; set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x0006E3F1 File Offset: 0x0006C5F1
		// (set) Token: 0x06001904 RID: 6404 RVA: 0x0006E3F9 File Offset: 0x0006C5F9
		public List<NPCAction> PendingActions { get; set; } = new List<NPCAction>();

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x0006E402 File Offset: 0x0006C602
		// (set) Token: 0x06001906 RID: 6406 RVA: 0x0006E40A File Offset: 0x0006C60A
		public NPC Npc { get; protected set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x0006E413 File Offset: 0x0006C613
		// (set) Token: 0x06001908 RID: 6408 RVA: 0x0006E41B File Offset: 0x0006C61B
		protected List<NPCAction> ActionsAwaitingStart { get; set; } = new List<NPCAction>();

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x0006E424 File Offset: 0x0006C624
		protected TimeManager Time
		{
			get
			{
				return NetworkSingleton<TimeManager>.Instance;
			}
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0006E42B File Offset: 0x0006C62B
		protected virtual void Awake()
		{
			this.Npc = base.GetComponentInParent<NPC>();
			this.SetCurfewModeEnabled(false);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0006E440 File Offset: 0x0006C640
		protected virtual void Start()
		{
			this.InitializeActions();
			TimeManager time = this.Time;
			time.onTimeChanged = (Action)Delegate.Remove(time.onTimeChanged, new Action(this.EnforceState));
			TimeManager time2 = this.Time;
			time2.onTimeChanged = (Action)Delegate.Combine(time2.onTimeChanged, new Action(this.EnforceState));
			TimeManager time3 = this.Time;
			time3.onMinutePass = (Action)Delegate.Remove(time3.onMinutePass, new Action(this.MinPass));
			TimeManager time4 = this.Time;
			time4.onMinutePass = (Action)Delegate.Combine(time4.onMinutePass, new Action(this.MinPass));
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.LocalPlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.LocalPlayerSpawned));
			NetworkSingleton<CurfewManager>.Instance.onCurfewEnabled.AddListener(new UnityAction(this.CurfewEnabled));
			NetworkSingleton<CurfewManager>.Instance.onCurfewDisabled.AddListener(new UnityAction(this.CurfewDisabled));
			if (this.DEBUG_MODE)
			{
				int min = 1250;
				int max = 930;
				this.GetActionsTotallyOccurringWithinRange(min, max, true);
			}
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0006E587 File Offset: 0x0006C787
		private void LocalPlayerSpawned()
		{
			if (InstanceFinder.IsServer)
			{
				this.EnforceState(true);
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0003488C File Offset: 0x00032A8C
		private void OnValidate()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0006E597 File Offset: 0x0006C797
		protected virtual void Update()
		{
			if (this.ActiveAction != null)
			{
				this.ActiveAction.ActiveUpdate();
			}
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0006E5B2 File Offset: 0x0006C7B2
		public void EnableSchedule()
		{
			this.ScheduleEnabled = true;
			this.MinPass();
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0006E5C1 File Offset: 0x0006C7C1
		public void DisableSchedule()
		{
			this.ScheduleEnabled = false;
			this.MinPass();
			if (this.Npc.Movement.IsMoving)
			{
				this.Npc.Movement.Stop();
			}
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0006E5F4 File Offset: 0x0006C7F4
		[Button]
		public void InitializeActions()
		{
			List<NPCAction> list = base.gameObject.GetComponentsInChildren<NPCAction>(true).ToList<NPCAction>();
			list.Sort(delegate(NPCAction a, NPCAction b)
			{
				float num = (float)a.StartTime;
				float value = (float)b.StartTime;
				int num2 = num.CompareTo(value);
				if (num2 != 0)
				{
					return num2;
				}
				if (a.IsSignal)
				{
					return -1;
				}
				return 1;
			});
			if (!Application.isPlaying)
			{
				foreach (NPCAction npcaction in list)
				{
					npcaction.transform.name = npcaction.GetName() + " (" + npcaction.GetTimeDescription() + ")";
					npcaction.transform.SetAsLastSibling();
				}
			}
			this.ActionList = list;
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0006E6B4 File Offset: 0x0006C8B4
		protected virtual void MinPass()
		{
			if (!this.Npc.IsSpawned)
			{
				return;
			}
			if (!this.ScheduleEnabled)
			{
				if (this.ActiveAction != null)
				{
					this.ActiveAction.Interrupt();
				}
				return;
			}
			if (this.ActiveAction != null)
			{
				this.ActiveAction.ActiveMinPassed();
			}
			if (this.ActiveAction != null && !this.ActiveAction.gameObject.activeInHierarchy)
			{
				this.ActiveAction.End();
			}
			List<NPCAction> actionsOccurringAt = this.GetActionsOccurringAt(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			bool debug_MODE = this.DEBUG_MODE;
			if (actionsOccurringAt.Count > 0)
			{
				NPCAction npcaction = actionsOccurringAt[0];
				if (this.ActiveAction != npcaction)
				{
					if (this.ActiveAction != null && npcaction.Priority > this.ActiveAction.Priority)
					{
						if (this.DEBUG_MODE)
						{
							Debug.Log("New active action: " + npcaction.GetName());
						}
						this.ActiveAction.Interrupt();
					}
					if (this.ActiveAction == null)
					{
						this.StartAction(npcaction);
					}
				}
			}
			foreach (NPCAction npcaction2 in actionsOccurringAt)
			{
				if (!npcaction2.HasStarted && !this.ActionsAwaitingStart.Contains(npcaction2))
				{
					this.ActionsAwaitingStart.Add(npcaction2);
				}
			}
			foreach (NPCAction npcaction3 in this.ActionsAwaitingStart.ToList<NPCAction>())
			{
				if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(npcaction3.StartTime, npcaction3.GetEndTime()))
				{
					npcaction3.Skipped();
					this.ActionsAwaitingStart.Remove(npcaction3);
				}
			}
			this.lastProcessedTime = this.Time.CurrentTime;
			if (this.DEBUG_MODE)
			{
				Console.Log("Active action: " + ((this.ActiveAction != null) ? this.ActiveAction.GetName() : "None"), null);
			}
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0006E8E0 File Offset: 0x0006CAE0
		private List<NPCAction> GetActionsOccurringAt(int time)
		{
			List<NPCAction> list = new List<NPCAction>();
			foreach (NPCAction npcaction in this.ActionList)
			{
				if (!(npcaction == null) && npcaction.ShouldStart() && TimeManager.IsGivenTimeWithinRange(time, npcaction.StartTime, TimeManager.AddMinutesTo24HourTime(npcaction.GetEndTime(), -1)))
				{
					list.Add(npcaction);
				}
			}
			list = (from x in list
			orderby x.Priority descending
			select x).ToList<NPCAction>();
			return list;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0006E990 File Offset: 0x0006CB90
		private List<NPCAction> GetActionsTotallyOccurringWithinRange(int min, int max, bool checkShouldStart)
		{
			List<NPCAction> list = new List<NPCAction>();
			foreach (NPCAction npcaction in this.ActionList)
			{
				if ((!checkShouldStart || npcaction.ShouldStart()) && TimeManager.IsGivenTimeWithinRange(npcaction.StartTime, min, max) && TimeManager.IsGivenTimeWithinRange(npcaction.GetEndTime(), min, max))
				{
					list.Add(npcaction);
				}
			}
			list = (from x in list
			orderby x.Priority descending
			select x).ToList<NPCAction>();
			bool debug_MODE = this.DEBUG_MODE;
			return list;
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0006EA48 File Offset: 0x0006CC48
		private void StartAction(NPCAction action)
		{
			if (this.ActiveAction != null)
			{
				Console.LogWarning("JumpToAction called but there is already an active action! Existing action should first be ended or interrupted!", null);
			}
			if (this.ActionsAwaitingStart.Contains(action))
			{
				this.ActionsAwaitingStart.Remove(action);
			}
			if (NetworkSingleton<TimeManager>.Instance.CurrentTime == action.StartTime)
			{
				action.Started();
				return;
			}
			if (action.HasStarted)
			{
				action.Resume();
				return;
			}
			action.LateStarted();
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0006EAB7 File Offset: 0x0006CCB7
		private void EnforceState()
		{
			this.EnforceState(Singleton<LoadManager>.Instance.IsLoading);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0006EACC File Offset: 0x0006CCCC
		public void EnforceState(bool initial = false)
		{
			this.ActionsAwaitingStart.Clear();
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(currentTime);
			if (this.DEBUG_MODE)
			{
				Debug.Log("Enforcing state. Last processed time: " + this.lastProcessedTime.ToString() + ", Current time: " + NetworkSingleton<TimeManager>.Instance.CurrentTime.ToString());
			}
			List<NPCAction> list = this.GetActionsTotallyOccurringWithinRange(this.lastProcessedTime, NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			List<NPCAction> actionsOccurringThisFrame = this.GetActionsOccurringAt(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			list.RemoveAll((NPCAction x) => x.IsActive || actionsOccurringThisFrame.Contains(x));
			NPCAction npcaction = null;
			if (actionsOccurringThisFrame.Count > 0)
			{
				npcaction = actionsOccurringThisFrame[0];
			}
			if (this.ActiveAction != null && this.ActiveAction != npcaction)
			{
				this.ActiveAction.Interrupt();
			}
			Dictionary<NPCAction, float> skippedActionOrder = new Dictionary<NPCAction, float>();
			for (int i = 0; i < list.Count; i++)
			{
				float num;
				if (list[i].StartTime >= currentTime)
				{
					num = (float)(TimeManager.GetMinSumFrom24HourTime(list[i].StartTime) - minSumFrom24HourTime);
				}
				else
				{
					num = 1440f - (float)minSumFrom24HourTime + (float)TimeManager.GetMinSumFrom24HourTime(list[i].StartTime);
				}
				num -= 0.01f * (float)list[i].Priority;
				skippedActionOrder.Add(list[i], num);
			}
			list = (from x in list
			orderby skippedActionOrder[x]
			select x).ToList<NPCAction>();
			if (this.DEBUG_MODE)
			{
				Debug.Log("Ordered skipped actions: " + list.Count.ToString());
			}
			if (!initial)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].Skipped();
				}
			}
			if (npcaction != null)
			{
				npcaction.JumpTo();
			}
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0006ECD4 File Offset: 0x0006CED4
		protected virtual void CurfewEnabled()
		{
			this.SetCurfewModeEnabled(true);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0006ECDD File Offset: 0x0006CEDD
		protected virtual void CurfewDisabled()
		{
			this.SetCurfewModeEnabled(false);
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0006ECE8 File Offset: 0x0006CEE8
		public void SetCurfewModeEnabled(bool enabled)
		{
			for (int i = 0; i < this.EnabledDuringCurfew.Length; i++)
			{
				this.EnabledDuringCurfew[i].gameObject.SetActive(enabled);
			}
			for (int j = 0; j < this.EnabledDuringNoCurfew.Length; j++)
			{
				this.EnabledDuringNoCurfew[j].gameObject.SetActive(!enabled);
			}
		}

		// Token: 0x04001616 RID: 5654
		public bool DEBUG_MODE;

		// Token: 0x0400161A RID: 5658
		[Header("References")]
		public GameObject[] EnabledDuringCurfew;

		// Token: 0x0400161B RID: 5659
		public GameObject[] EnabledDuringNoCurfew;

		// Token: 0x0400161C RID: 5660
		public List<NPCAction> ActionList = new List<NPCAction>();

		// Token: 0x0400161E RID: 5662
		protected int lastProcessedTime;
	}
}
