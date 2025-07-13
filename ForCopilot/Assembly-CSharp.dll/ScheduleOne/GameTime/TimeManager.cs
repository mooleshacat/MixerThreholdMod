using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002B9 RID: 697
	public class TimeManager : NetworkSingleton<TimeManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00040AFA File Offset: 0x0003ECFA
		public bool IsEndOfDay
		{
			get
			{
				return this.CurrentTime == 400;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00040B09 File Offset: 0x0003ED09
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x00040B11 File Offset: 0x0003ED11
		public bool SleepInProgress { get; protected set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00040B1A File Offset: 0x0003ED1A
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x00040B22 File Offset: 0x0003ED22
		public int ElapsedDays { get; protected set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00040B2B File Offset: 0x0003ED2B
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00040B33 File Offset: 0x0003ED33
		public int CurrentTime { get; protected set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00040B3C File Offset: 0x0003ED3C
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x00040B44 File Offset: 0x0003ED44
		public float TimeOnCurrentMinute { get; protected set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00040B4D File Offset: 0x0003ED4D
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x00040B55 File Offset: 0x0003ED55
		public int DailyMinTotal { get; protected set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00040B5E File Offset: 0x0003ED5E
		public bool IsNight
		{
			get
			{
				return this.CurrentTime < 600 || this.CurrentTime >= 1800;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00040B7F File Offset: 0x0003ED7F
		public int DayIndex
		{
			get
			{
				return this.ElapsedDays % 7;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00040B89 File Offset: 0x0003ED89
		public float NormalizedTime
		{
			get
			{
				return (float)this.DailyMinTotal / 1440f;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00040B98 File Offset: 0x0003ED98
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x00040BA0 File Offset: 0x0003EDA0
		public float Playtime { get; protected set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x00040BA9 File Offset: 0x0003EDA9
		public EDay CurrentDay
		{
			get
			{
				return (EDay)this.DayIndex;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00040BB1 File Offset: 0x0003EDB1
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00040BB9 File Offset: 0x0003EDB9
		public bool TimeOverridden { get; protected set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00040BC2 File Offset: 0x0003EDC2
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x00040BCA File Offset: 0x0003EDCA
		public bool HostDailySummaryDone { get; private set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00040BD3 File Offset: 0x0003EDD3
		public string SaveFolderName
		{
			get
			{
				return "Time";
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00040BD3 File Offset: 0x0003EDD3
		public string SaveFileName
		{
			get
			{
				return "Time";
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00040BDA File Offset: 0x0003EDDA
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00040BE2 File Offset: 0x0003EDE2
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00040BEA File Offset: 0x0003EDEA
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00040BF3 File Offset: 0x0003EDF3
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00040BFB File Offset: 0x0003EDFB
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00040C04 File Offset: 0x0003EE04
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x00040C0C File Offset: 0x0003EE0C
		public bool HasChanged { get; set; }

		// Token: 0x06000EBE RID: 3774 RVA: 0x00040C18 File Offset: 0x0003EE18
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.GameTime.TimeManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00040C37 File Offset: 0x0003EE37
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!connection.IsHost)
			{
				this.SendTimeData(connection);
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00040C4F File Offset: 0x0003EE4F
		public override void OnStartClient()
		{
			base.OnStartClient();
			base.StartCoroutine(this.TimeLoop());
			base.StartCoroutine(this.TickLoop());
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00040C71 File Offset: 0x0003EE71
		private void Clean()
		{
			TimeManager.onSleepStart = null;
			TimeManager.onSleepEnd = null;
			TimeManager.onSleepStart = null;
			TimeManager.onSleepEnd = null;
			this.onMinutePass = null;
			this.onHourPass = null;
			this.onDayPass = null;
			this.onTimeChanged = null;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00040CA8 File Offset: 0x0003EEA8
		public void SendTimeData(NetworkConnection connection)
		{
			TimeManager.<>c__DisplayClass94_0 CS$<>8__locals1 = new TimeManager.<>c__DisplayClass94_0();
			CS$<>8__locals1.connection = connection;
			CS$<>8__locals1.<>4__this = this;
			base.StartCoroutine(CS$<>8__locals1.<SendTimeData>g__WaitForPlayerReady|0());
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00040CD8 File Offset: 0x0003EED8
		[ObserversRpc(RunLocally = true, ExcludeServer = true)]
		[TargetRpc]
		private void SetData(NetworkConnection conn, int _elapsedDays, int _time, float sendTime)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetData_2661156041(conn, _elapsedDays, _time, sendTime);
				this.RpcLogic___SetData_2661156041(conn, _elapsedDays, _time, sendTime);
			}
			else
			{
				this.RpcWriter___Target_SetData_2661156041(conn, _elapsedDays, _time, sendTime);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00040D34 File Offset: 0x0003EF34
		protected virtual void Update()
		{
			if (this.CurrentTime != 400)
			{
				this.TimeOnCurrentMinute += Time.deltaTime * this.TimeProgressionMultiplier;
			}
			this.Playtime += Time.unscaledDeltaTime;
			if (Time.timeScale >= 1f)
			{
				Time.fixedDeltaTime = this.defaultFixedTimeScale * Time.timeScale;
			}
			else
			{
				Time.fixedDeltaTime = this.defaultFixedTimeScale;
			}
			if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.RightArrow) && InstanceFinder.IsServer && (Application.isEditor || Debug.isDebugBuild))
			{
				for (int i = 0; i < 60; i++)
				{
					this.Tick();
				}
				this.SetData(null, this.ElapsedDays, this.CurrentTime, (float)(DateTime.UtcNow.Ticks / 10000000L));
			}
			if (InstanceFinder.IsHost)
			{
				if (this.SleepInProgress)
				{
					if (this.IsCurrentTimeWithinRange(this.sleepEndTime, TimeManager.AddMinutesTo24HourTime(this.sleepEndTime, 60)))
					{
						this.EndSleep();
					}
				}
				else if (Player.AreAllPlayersReadyToSleep())
				{
					this.StartSleep();
				}
			}
			if (this.onUpdate != null)
			{
				this.onUpdate();
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00040E5D File Offset: 0x0003F05D
		protected virtual void FixedUpdate()
		{
			if (this.onFixedUpdate != null)
			{
				this.onFixedUpdate();
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00040E72 File Offset: 0x0003F072
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void ResetHostSleepDone()
		{
			this.RpcWriter___Server_ResetHostSleepDone_2166136261();
			this.RpcLogic___ResetHostSleepDone_2166136261();
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00040E80 File Offset: 0x0003F080
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void MarkHostSleepDone()
		{
			this.RpcWriter___Server_MarkHostSleepDone_2166136261();
			this.RpcLogic___MarkHostSleepDone_2166136261();
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00040E8E File Offset: 0x0003F08E
		[ObserversRpc(RunLocally = true)]
		private void SetHostSleepDone(bool done)
		{
			this.RpcWriter___Observers_SetHostSleepDone_1140765316(done);
			this.RpcLogic___SetHostSleepDone_1140765316(done);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00040EA4 File Offset: 0x0003F0A4
		private IEnumerator TickLoop()
		{
			float lastWaitExcess = 0f;
			while (base.gameObject != null)
			{
				if (Time.timeScale == 0f)
				{
					yield return new WaitUntil(() => Time.timeScale > 0f);
				}
				float timeToWait = 1f / Time.timeScale - lastWaitExcess;
				if (timeToWait > 0f)
				{
					float timeOnWaitStart = Time.realtimeSinceStartup;
					yield return new WaitForSecondsRealtime(timeToWait);
					float num = Time.realtimeSinceStartup - timeOnWaitStart;
					lastWaitExcess = Mathf.Max(num - timeToWait, 0f);
				}
				else
				{
					lastWaitExcess -= 1f;
				}
				try
				{
					if (this.onTick != null)
					{
						this.onTick();
					}
				}
				catch (Exception ex)
				{
					Console.LogError("Error invoking onTick: " + ex.Message + "\nSite:" + ex.StackTrace, null);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00040EB3 File Offset: 0x0003F0B3
		private IEnumerator TimeLoop()
		{
			float lastWaitExcess = 0f;
			while (base.gameObject != null)
			{
				if (this.TimeProgressionMultiplier <= 0f)
				{
					yield return new WaitUntil(() => this.TimeProgressionMultiplier > 0f);
				}
				if (Time.timeScale == 0f)
				{
					yield return new WaitUntil(() => Time.timeScale > 0f);
				}
				float timeToWait = 1f / (this.TimeProgressionMultiplier * Time.timeScale) - lastWaitExcess;
				if (timeToWait > 0f)
				{
					float timeOnWaitStart = Time.realtimeSinceStartup;
					yield return new WaitForSecondsRealtime(timeToWait);
					float num = Time.realtimeSinceStartup - timeOnWaitStart;
					lastWaitExcess = Mathf.Max(num - timeToWait, 0f);
				}
				else
				{
					lastWaitExcess -= 1f / this.TimeProgressionMultiplier;
				}
				this.Tick();
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00040EC2 File Offset: 0x0003F0C2
		private IEnumerator StaggeredMinPass(float staggerTime)
		{
			if (this.onMinutePass == null)
			{
				yield break;
			}
			Delegate[] listeners = this.onMinutePass.GetInvocationList();
			float perDelay = staggerTime / (float)listeners.Length;
			float startTime = Time.timeSinceLevelLoad;
			float waitOverflow = 0f;
			float timeOnWaitStart = Time.timeSinceLevelLoad;
			int loopsSinceLastWait = 0;
			int num;
			for (int i = 0; i < listeners.Length; i = num + 1)
			{
				num = loopsSinceLastWait;
				loopsSinceLastWait = num + 1;
				float num2 = perDelay - waitOverflow;
				timeOnWaitStart = Time.timeSinceLevelLoad;
				if (num2 > 0f)
				{
					loopsSinceLastWait = 0;
					yield return new WaitForSeconds(num2);
				}
				float num3 = Time.timeSinceLevelLoad - timeOnWaitStart - perDelay;
				waitOverflow += num3;
				if (listeners[i] != null)
				{
					try
					{
						listeners[i].DynamicInvoke(Array.Empty<object>());
					}
					catch (Exception ex)
					{
						Console.LogError("Error invoking onMinutePass: " + ex.Message + "\nSite:" + ex.StackTrace, null);
					}
				}
				num = i;
			}
			float timeSinceLevelLoad = Time.timeSinceLevelLoad;
			yield break;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00040ED8 File Offset: 0x0003F0D8
		private void Tick()
		{
			if (Player.Local == null)
			{
				Console.LogWarning("Local player does not exist. Waiting for player to spawn.", null);
				return;
			}
			this.TimeOnCurrentMinute = 0f;
			try
			{
				base.StartCoroutine(this.StaggeredMinPass(1f / (this.TimeProgressionMultiplier * Time.timeScale)));
			}
			catch (Exception ex)
			{
				string[] array = new string[8];
				array[0] = "Error invoking onMinutePass: ";
				array[1] = ex.Message;
				array[2] = "\nStack Trace: ";
				array[3] = ex.StackTrace;
				array[4] = "\nSource: ";
				array[5] = ex.Source;
				array[6] = "\nTarget Site: ";
				int num = 7;
				MethodBase targetSite = ex.TargetSite;
				array[num] = ((targetSite != null) ? targetSite.ToString() : null);
				Console.LogError(string.Concat(array), null);
			}
			if (this.CurrentTime == 400 || (this.IsCurrentTimeWithinRange(400, 600) && !GameManager.IS_TUTORIAL))
			{
				return;
			}
			if (this.CurrentTime == 2359)
			{
				int num2 = this.ElapsedDays;
				this.ElapsedDays = num2 + 1;
				this.CurrentTime = 0;
				this.DailyMinTotal = 0;
				if (this.onDayPass != null)
				{
					this.onDayPass();
				}
				if (this.onHourPass != null)
				{
					this.onHourPass();
				}
				if (this.CurrentDay == EDay.Monday && this.onWeekPass != null)
				{
					this.onWeekPass();
				}
			}
			else if (this.CurrentTime % 100 >= 59)
			{
				this.CurrentTime += 41;
				if (this.onHourPass != null)
				{
					this.onHourPass();
				}
			}
			else
			{
				int num2 = this.CurrentTime;
				this.CurrentTime = num2 + 1;
			}
			this.DailyMinTotal = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
			this.HasChanged = true;
			if (this.ElapsedDays == 0 && this.CurrentTime == 2000 && this.onFirstNight != null)
			{
				this.onFirstNight.Invoke();
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000410B4 File Offset: 0x0003F2B4
		public void SetTime(int _time, bool local = false)
		{
			if (!InstanceFinder.IsHost && InstanceFinder.NetworkManager != null && !local)
			{
				Console.LogWarning("SetTime can only be called by host", null);
				return;
			}
			Console.Log("Setting time to: " + _time.ToString(), null);
			this.CurrentTime = _time;
			this.TimeOnCurrentMinute = 0f;
			this.DailyMinTotal = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
			this.SetData(null, this.ElapsedDays, this.CurrentTime, (float)(DateTime.UtcNow.Ticks / 10000000L));
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00041148 File Offset: 0x0003F348
		public void SetElapsedDays(int days)
		{
			if (!InstanceFinder.IsHost && InstanceFinder.NetworkManager != null)
			{
				Console.LogWarning("SetElapsedDays can only be called by host", null);
				return;
			}
			this.ElapsedDays = days;
			this.SetData(null, this.ElapsedDays, this.CurrentTime, (float)(DateTime.UtcNow.Ticks / 10000000L));
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000411A4 File Offset: 0x0003F3A4
		public static string Get12HourTime(float _time, bool appendDesignator = true)
		{
			string text = _time.ToString();
			while (text.Length < 4)
			{
				text = "0" + text;
			}
			int num = Convert.ToInt32(text.Substring(0, 2));
			int num2 = Convert.ToInt32(text.Substring(2, 2));
			string str = "AM";
			if (num == 0)
			{
				num = 12;
			}
			else if (num == 12)
			{
				str = "PM";
			}
			else if (num > 12)
			{
				num -= 12;
				str = "PM";
			}
			string text2 = string.Format("{0}:{1:00}", num, num2);
			if (appendDesignator)
			{
				text2 = text2 + " " + str;
			}
			return text2;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00041244 File Offset: 0x0003F444
		public static int Get24HourTimeFromMinSum(int minSum)
		{
			if (minSum < 0)
			{
				minSum = 1440 - minSum;
			}
			minSum %= 1440;
			int num = (int)((float)minSum / 60f);
			int num2 = minSum - 60 * num;
			return num * 100 + num2;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00041280 File Offset: 0x0003F480
		public static int GetMinSumFrom24HourTime(int _time)
		{
			int num = (int)((float)_time / 100f);
			int num2 = _time - num * 100;
			return num * 60 + num2;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000412A4 File Offset: 0x0003F4A4
		public bool IsCurrentTimeWithinRange(int min, int max)
		{
			return TimeManager.IsGivenTimeWithinRange(this.CurrentTime, min, max);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000412B3 File Offset: 0x0003F4B3
		public static bool IsGivenTimeWithinRange(int givenTime, int min, int max)
		{
			if (max > min)
			{
				return givenTime >= min && givenTime <= max;
			}
			return givenTime >= min || givenTime <= max;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000412D4 File Offset: 0x0003F4D4
		public static bool IsValid24HourTime(string input)
		{
			string pattern = "^([01]?[0-9]|2[0-3])[0-5][0-9]$";
			return Regex.IsMatch(input, pattern);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x000412F0 File Offset: 0x0003F4F0
		public bool IsCurrentDateWithinRange(GameDateTime start, GameDateTime end)
		{
			int totalMinSum = this.GetTotalMinSum();
			return totalMinSum >= start.GetMinSum() && totalMinSum <= end.GetMinSum();
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0004131D File Offset: 0x0003F51D
		[ObserversRpc]
		private void InvokeDayPassClientSide()
		{
			this.RpcWriter___Observers_InvokeDayPassClientSide_2166136261();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00041325 File Offset: 0x0003F525
		[ObserversRpc]
		private void InvokeWeekPassClientSide()
		{
			this.RpcWriter___Observers_InvokeWeekPassClientSide_2166136261();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00041330 File Offset: 0x0003F530
		public void FastForwardToWakeTime()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Console.Log("Fast forwarding to wake time: " + 700.ToString(), null);
			if (this.CurrentTime > 1200)
			{
				int elapsedDays = this.ElapsedDays;
				this.ElapsedDays = elapsedDays + 1;
				this.DailyMinTotal = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
				this.HasChanged = true;
				if (this.onDayPass != null)
				{
					this.onDayPass();
				}
				this.InvokeDayPassClientSide();
				if (this.CurrentDay == EDay.Monday)
				{
					if (this.onWeekPass != null)
					{
						this.onWeekPass();
					}
					this.InvokeWeekPassClientSide();
				}
			}
			int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
			int obj = Mathf.Abs(TimeManager.GetMinSumFrom24HourTime(700) - minSumFrom24HourTime);
			int time = 700;
			if (GameManager.IS_TUTORIAL)
			{
				time = 300;
			}
			this.SetTime(time, false);
			try
			{
				if (this.onTimeSkip != null)
				{
					this.onTimeSkip(obj);
				}
			}
			catch (Exception ex)
			{
				Console.LogError("Error invoking onTimeSkip: " + ex.Message + "\nSite:" + ex.StackTrace, null);
			}
			try
			{
				if (TimeManager.onSleepEnd != null)
				{
					TimeManager.onSleepEnd(obj);
				}
			}
			catch (Exception ex2)
			{
				Console.LogError("Error invoking onSleepEnd: " + ex2.Message + "\nSite:" + ex2.StackTrace, null);
			}
			try
			{
				if (this._onSleepEnd != null)
				{
					this._onSleepEnd.Invoke();
				}
			}
			catch (Exception ex3)
			{
				Console.LogError("Error invoking _onSleepEnd: " + ex3.Message + "\nSite:" + ex3.StackTrace, null);
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000414E8 File Offset: 0x0003F6E8
		public GameDateTime GetDateTime()
		{
			return new GameDateTime(this.ElapsedDays, this.CurrentTime);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000414FB File Offset: 0x0003F6FB
		public int GetTotalMinSum()
		{
			return this.ElapsedDays * 1440 + this.DailyMinTotal;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00041510 File Offset: 0x0003F710
		public static int AddMinutesTo24HourTime(int time, int minsToAdd)
		{
			int num = TimeManager.GetMinSumFrom24HourTime(time) + minsToAdd;
			if (num < 0)
			{
				num = 1440 + num;
			}
			return TimeManager.Get24HourTimeFromMinSum(num);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00041538 File Offset: 0x0003F738
		public static List<int> GetAllTimeInRange(int min, int max)
		{
			List<int> list = new List<int>();
			int num = min;
			while (num != max)
			{
				list.Add(num);
				num++;
				if (num >= 2360)
				{
					num = 0;
				}
				else if (num % 100 >= 60)
				{
					num += 40;
				}
			}
			list.Add(max);
			return list;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0004157F File Offset: 0x0003F77F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetWakeTime(int amount)
		{
			this.RpcWriter___Server_SetWakeTime_3316948804(amount);
			this.RpcLogic___SetWakeTime_3316948804(amount);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00041598 File Offset: 0x0003F798
		[ObserversRpc(RunLocally = true)]
		private void StartSleep()
		{
			this.RpcWriter___Observers_StartSleep_2166136261();
			this.RpcLogic___StartSleep_2166136261();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000415B4 File Offset: 0x0003F7B4
		[ObserversRpc(RunLocally = true)]
		private void EndSleep()
		{
			this.RpcWriter___Observers_EndSleep_2166136261();
			this.RpcLogic___EndSleep_2166136261();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000415CD File Offset: 0x0003F7CD
		public virtual string GetSaveString()
		{
			return new TimeData(this.CurrentTime, this.ElapsedDays, Mathf.RoundToInt(this.Playtime)).GetJson(true);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x000415F1 File Offset: 0x0003F7F1
		public void SetPlaytime(float time)
		{
			this.Playtime = time;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000415FC File Offset: 0x0003F7FC
		public void SetTimeOverridden(bool overridden, int time = 1200)
		{
			if (overridden && this.TimeOverridden)
			{
				Console.LogWarning("Time already overridden.", null);
				return;
			}
			this.TimeOverridden = overridden;
			if (overridden)
			{
				this.savedTime = this.CurrentTime;
				this.SetTime(time, false);
			}
			else
			{
				this.SetTime(this.savedTime, false);
			}
			if (this.onMinutePass != null)
			{
				this.onMinutePass();
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00041660 File Offset: 0x0003F860
		private void SetRandomTime()
		{
			int minSum = UnityEngine.Random.Range(0, 1440);
			this.SetTime(TimeManager.Get24HourTimeFromMinSum(minSum), false);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000416D4 File Offset: 0x0003F8D4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.GameTime.TimeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.GameTime.TimeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetData_2661156041));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_SetData_2661156041));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_ResetHostSleepDone_2166136261));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_MarkHostSleepDone_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_SetHostSleepDone_1140765316));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_InvokeDayPassClientSide_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_InvokeWeekPassClientSide_2166136261));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SetWakeTime_3316948804));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_StartSleep_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_EndSleep_2166136261));
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x000417DE File Offset: 0x0003F9DE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.GameTime.TimeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.GameTime.TimeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000417F7 File Offset: 0x0003F9F7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00041808 File Offset: 0x0003FA08
		private void RpcWriter___Observers_SetData_2661156041(NetworkConnection conn, int _elapsedDays, int _time, float sendTime)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(_elapsedDays, 1);
			writer.WriteInt32(_time, 1);
			writer.WriteSingle(sendTime, 0);
			base.SendObserversRpc(0U, writer, channel, 0, false, true, false);
			writer.Store();
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000418E8 File Offset: 0x0003FAE8
		private void RpcLogic___SetData_2661156041(NetworkConnection conn, int _elapsedDays, int _time, float sendTime)
		{
			this.ElapsedDays = _elapsedDays;
			this.CurrentTime = _time;
			this.DailyMinTotal = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
			this.HasChanged = true;
			try
			{
				if (this.onTimeChanged != null)
				{
					this.onTimeChanged();
				}
			}
			catch (Exception ex)
			{
				Console.LogError("Error invoking onTimeChanged: " + ex.Message + "\nSite:" + ex.StackTrace, null);
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00041964 File Offset: 0x0003FB64
		private void RpcReader___Observers_SetData_2661156041(PooledReader PooledReader0, Channel channel)
		{
			int elapsedDays = PooledReader0.ReadInt32(1);
			int time = PooledReader0.ReadInt32(1);
			float sendTime = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetData_2661156041(null, elapsedDays, time, sendTime);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000419D4 File Offset: 0x0003FBD4
		private void RpcWriter___Target_SetData_2661156041(NetworkConnection conn, int _elapsedDays, int _time, float sendTime)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(_elapsedDays, 1);
			writer.WriteInt32(_time, 1);
			writer.WriteSingle(sendTime, 0);
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00041AB4 File Offset: 0x0003FCB4
		private void RpcReader___Target_SetData_2661156041(PooledReader PooledReader0, Channel channel)
		{
			int elapsedDays = PooledReader0.ReadInt32(1);
			int time = PooledReader0.ReadInt32(1);
			float sendTime = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetData_2661156041(base.LocalConnection, elapsedDays, time, sendTime);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00041B1C File Offset: 0x0003FD1C
		private void RpcWriter___Server_ResetHostSleepDone_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00041BB6 File Offset: 0x0003FDB6
		public void RpcLogic___ResetHostSleepDone_2166136261()
		{
			this.SetHostSleepDone(false);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00041BC0 File Offset: 0x0003FDC0
		private void RpcReader___Server_ResetHostSleepDone_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ResetHostSleepDone_2166136261();
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00041BF0 File Offset: 0x0003FDF0
		private void RpcWriter___Server_MarkHostSleepDone_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00041C8A File Offset: 0x0003FE8A
		public void RpcLogic___MarkHostSleepDone_2166136261()
		{
			this.SetHostSleepDone(true);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00041C94 File Offset: 0x0003FE94
		private void RpcReader___Server_MarkHostSleepDone_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkHostSleepDone_2166136261();
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00041CC4 File Offset: 0x0003FEC4
		private void RpcWriter___Observers_SetHostSleepDone_1140765316(bool done)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(done);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00041D7A File Offset: 0x0003FF7A
		private void RpcLogic___SetHostSleepDone_1140765316(bool done)
		{
			this.HostDailySummaryDone = done;
			Console.Log("Host daily summary done: " + done.ToString(), null);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00041D9C File Offset: 0x0003FF9C
		private void RpcReader___Observers_SetHostSleepDone_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool done = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetHostSleepDone_1140765316(done);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00041DD8 File Offset: 0x0003FFD8
		private void RpcWriter___Observers_InvokeDayPassClientSide_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00041E81 File Offset: 0x00040081
		private void RpcLogic___InvokeDayPassClientSide_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				return;
			}
			if (this.onDayPass != null)
			{
				this.onDayPass();
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00041EA0 File Offset: 0x000400A0
		private void RpcReader___Observers_InvokeDayPassClientSide_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InvokeDayPassClientSide_2166136261();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00041EC0 File Offset: 0x000400C0
		private void RpcWriter___Observers_InvokeWeekPassClientSide_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00041F69 File Offset: 0x00040169
		private void RpcLogic___InvokeWeekPassClientSide_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				return;
			}
			if (this.onWeekPass != null)
			{
				this.onWeekPass();
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00041F88 File Offset: 0x00040188
		private void RpcReader___Observers_InvokeWeekPassClientSide_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___InvokeWeekPassClientSide_2166136261();
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00041FA8 File Offset: 0x000401A8
		private void RpcWriter___Server_SetWakeTime_3316948804(int amount)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(amount, 1);
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000045B1 File Offset: 0x000027B1
		public void RpcLogic___SetWakeTime_3316948804(int amount)
		{
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00042054 File Offset: 0x00040254
		private void RpcReader___Server_SetWakeTime_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int amount = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetWakeTime_3316948804(amount);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00042098 File Offset: 0x00040298
		private void RpcWriter___Observers_StartSleep_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00042144 File Offset: 0x00040344
		private void RpcLogic___StartSleep_2166136261()
		{
			if (this.SleepInProgress)
			{
				return;
			}
			Debug.Log("Start sleep");
			this.sleepStartTime = this.GetDateTime();
			this.sleepEndTime = 700;
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.sleepEndTime = 100;
			}
			this.SleepInProgress = true;
			Time.timeScale = 1f;
			if (TimeManager.onSleepStart != null)
			{
				TimeManager.onSleepStart();
			}
			if (this._onSleepStart != null)
			{
				this._onSleepStart.Invoke();
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000421C4 File Offset: 0x000403C4
		private void RpcReader___Observers_StartSleep_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartSleep_2166136261();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000421F0 File Offset: 0x000403F0
		private void RpcWriter___Observers_EndSleep_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0004229C File Offset: 0x0004049C
		private void RpcLogic___EndSleep_2166136261()
		{
			if (!this.SleepInProgress)
			{
				return;
			}
			this.SleepInProgress = false;
			Time.timeScale = 1f;
			if (NetworkSingleton<TimeManager>.Instance.IsHost)
			{
				this.SendTimeData(null);
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sleep_Count", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Sleep_Count") + 1f).ToString(), false);
			if (TimeManager.onSleepEnd != null)
			{
				TimeManager.onSleepEnd(this.GetDateTime().GetMinSum() - this.sleepStartTime.GetMinSum());
			}
			if (this._onSleepEnd != null)
			{
				this._onSleepEnd.Invoke();
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00042344 File Offset: 0x00040544
		private void RpcReader___Observers_EndSleep_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndSleep_2166136261();
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00042370 File Offset: 0x00040570
		protected override void dll()
		{
			base.Awake();
			this.defaultFixedTimeScale = Time.fixedDeltaTime;
			if (!Singleton<Lobby>.InstanceExists || !Singleton<Lobby>.Instance.IsInLobby || Singleton<Lobby>.Instance.IsHost || GameManager.IS_TUTORIAL)
			{
				this.SetTime(this.DefaultTime, true);
				this.ElapsedDays = (int)this.DefaultDay;
				this.DailyMinTotal = TimeManager.GetMinSumFrom24HourTime(this.CurrentTime);
			}
			this.InitializeSaveable();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			this.SetWakeTime(700);
		}

		// Token: 0x04000F1E RID: 3870
		public const float CYCLE_DURATION_MINS = 24f;

		// Token: 0x04000F1F RID: 3871
		public const float MINUTE_TIME = 1f;

		// Token: 0x04000F20 RID: 3872
		public const int DEFAULT_WAKE_TIME = 700;

		// Token: 0x04000F21 RID: 3873
		public const int END_OF_DAY = 400;

		// Token: 0x04000F26 RID: 3878
		public int DefaultTime = 900;

		// Token: 0x04000F27 RID: 3879
		public EDay DefaultDay;

		// Token: 0x04000F28 RID: 3880
		public float TimeProgressionMultiplier = 1f;

		// Token: 0x04000F2B RID: 3883
		private int savedTime;

		// Token: 0x04000F2D RID: 3885
		public Action onMinutePass;

		// Token: 0x04000F2E RID: 3886
		public Action onHourPass;

		// Token: 0x04000F2F RID: 3887
		public Action onDayPass;

		// Token: 0x04000F30 RID: 3888
		public Action onWeekPass;

		// Token: 0x04000F31 RID: 3889
		public Action onUpdate;

		// Token: 0x04000F32 RID: 3890
		public Action onFixedUpdate;

		// Token: 0x04000F33 RID: 3891
		public Action<int> onTimeSkip;

		// Token: 0x04000F34 RID: 3892
		public Action onTick;

		// Token: 0x04000F35 RID: 3893
		public static Action onSleepStart;

		// Token: 0x04000F36 RID: 3894
		public UnityEvent _onSleepStart;

		// Token: 0x04000F37 RID: 3895
		public static Action<int> onSleepEnd;

		// Token: 0x04000F38 RID: 3896
		public UnityEvent _onSleepEnd;

		// Token: 0x04000F39 RID: 3897
		public UnityEvent onFirstNight;

		// Token: 0x04000F3A RID: 3898
		public Action onTimeChanged;

		// Token: 0x04000F3B RID: 3899
		public const int SelectedWakeTime = 700;

		// Token: 0x04000F3C RID: 3900
		private GameDateTime sleepStartTime;

		// Token: 0x04000F3D RID: 3901
		private int sleepEndTime;

		// Token: 0x04000F3F RID: 3903
		private float defaultFixedTimeScale;

		// Token: 0x04000F40 RID: 3904
		private TimeLoader loader = new TimeLoader();

		// Token: 0x04000F44 RID: 3908
		private bool dll_Excuted;

		// Token: 0x04000F45 RID: 3909
		private bool dll_Excuted;
	}
}
