using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Law
{
	// Token: 0x02000605 RID: 1541
	public class LawController : Singleton<LawController>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x000989F8 File Offset: 0x00096BF8
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x00098A00 File Offset: 0x00096C00
		public bool OverrideSettings { get; protected set; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x00098A09 File Offset: 0x00096C09
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x00098A11 File Offset: 0x00096C11
		public LawActivitySettings OverriddenSettings { get; protected set; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x00098A1A File Offset: 0x00096C1A
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x00098A22 File Offset: 0x00096C22
		public LawActivitySettings CurrentSettings { get; protected set; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x00098A2B File Offset: 0x00096C2B
		public string SaveFolderName
		{
			get
			{
				return "Law";
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x00098A2B File Offset: 0x00096C2B
		public string SaveFileName
		{
			get
			{
				return "Law";
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x00098A32 File Offset: 0x00096C32
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x00098A3A File Offset: 0x00096C3A
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x00098A42 File Offset: 0x00096C42
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x00098A4B File Offset: 0x00096C4B
		// (set) Token: 0x060025C2 RID: 9666 RVA: 0x00098A53 File Offset: 0x00096C53
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x00098A5C File Offset: 0x00096C5C
		// (set) Token: 0x060025C4 RID: 9668 RVA: 0x00098A64 File Offset: 0x00096C64
		public bool HasChanged { get; set; }

		// Token: 0x060025C5 RID: 9669 RVA: 0x00098A6D File Offset: 0x00096C6D
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00098A7C File Offset: 0x00096C7C
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onHourPass = (Action)Delegate.Combine(instance2.onHourPass, new Action(this.HourPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onDayPass = (Action)Delegate.Combine(instance3.onDayPass, new Action(this.DayPass));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoadComplete));
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00098B1C File Offset: 0x00096D1C
		protected override void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onHourPass = (Action)Delegate.Remove(instance2.onHourPass, new Action(this.HourPass));
				TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
				instance3.onDayPass = (Action)Delegate.Remove(instance3.onDayPass, new Action(this.DayPass));
			}
			base.OnDestroy();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00098BA8 File Offset: 0x00096DA8
		private void OnLoadComplete()
		{
			this.GetSettings().OnLoaded();
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00098BB8 File Offset: 0x00096DB8
		private void MinPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			LawActivitySettings settings = this.GetSettings();
			if (settings != this.CurrentSettings)
			{
				if (this.CurrentSettings != null)
				{
					this.CurrentSettings.End();
				}
				this.CurrentSettings = settings;
			}
			this.CurrentSettings.Evaluate();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00098C02 File Offset: 0x00096E02
		private void HourPass()
		{
			bool isServer = InstanceFinder.IsServer;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00098C0A File Offset: 0x00096E0A
		private void DayPass()
		{
			if (InstanceFinder.IsServer)
			{
				this.ChangeInternalIntensity(this.IntensityIncreasePerDay / 10f);
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00098C25 File Offset: 0x00096E25
		public LawActivitySettings GetSettings()
		{
			if (this.OverrideSettings && this.OverriddenSettings != null)
			{
				return this.OverriddenSettings;
			}
			return this.GetSettings(NetworkSingleton<TimeManager>.Instance.CurrentDay);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00098C50 File Offset: 0x00096E50
		public LawActivitySettings GetSettings(EDay day)
		{
			switch (day)
			{
			case EDay.Monday:
				return this.MondaySettings;
			case EDay.Tuesday:
				return this.TuesdaySettings;
			case EDay.Wednesday:
				return this.WednesdaySettings;
			case EDay.Thursday:
				return this.ThursdaySettings;
			case EDay.Friday:
				return this.FridaySettings;
			case EDay.Saturday:
				return this.SaturdaySettings;
			case EDay.Sunday:
				return this.SundaySettings;
			default:
				return null;
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00098CB3 File Offset: 0x00096EB3
		public void OverrideSetings(LawActivitySettings settings)
		{
			this.OverrideSettings = true;
			this.OverriddenSettings = settings;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00098CC3 File Offset: 0x00096EC3
		public void EndOverride()
		{
			this.OverrideSettings = false;
			this.OverriddenSettings = null;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00098CD3 File Offset: 0x00096ED3
		public void ChangeInternalIntensity(float change)
		{
			this.internalLawIntensity = Mathf.Clamp01(this.internalLawIntensity + change);
			this.LE_Intensity = Mathf.RoundToInt(Mathf.Lerp(1f, 10f, this.internalLawIntensity));
			this.HasChanged = true;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00098D0F File Offset: 0x00096F0F
		public void SetInternalIntensity(float intensity)
		{
			this.internalLawIntensity = Mathf.Clamp01(intensity);
			this.LE_Intensity = Mathf.RoundToInt(Mathf.Lerp(1f, 10f, this.internalLawIntensity));
			this.HasChanged = true;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00098D44 File Offset: 0x00096F44
		public virtual string GetSaveString()
		{
			return new LawData(this.internalLawIntensity).GetJson(true);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00098D57 File Offset: 0x00096F57
		public void Load(LawData data)
		{
			this.SetInternalIntensity(data.InternalLawIntensity);
		}

		// Token: 0x04001BCE RID: 7118
		public const float DAILY_INTENSITY_DRAIN = 0.05f;

		// Token: 0x04001BCF RID: 7119
		[Range(1f, 10f)]
		public int LE_Intensity = 1;

		// Token: 0x04001BD0 RID: 7120
		private float internalLawIntensity;

		// Token: 0x04001BD1 RID: 7121
		[Header("Settings")]
		public LawActivitySettings MondaySettings;

		// Token: 0x04001BD2 RID: 7122
		public LawActivitySettings TuesdaySettings;

		// Token: 0x04001BD3 RID: 7123
		public LawActivitySettings WednesdaySettings;

		// Token: 0x04001BD4 RID: 7124
		public LawActivitySettings ThursdaySettings;

		// Token: 0x04001BD5 RID: 7125
		public LawActivitySettings FridaySettings;

		// Token: 0x04001BD6 RID: 7126
		public LawActivitySettings SaturdaySettings;

		// Token: 0x04001BD7 RID: 7127
		public LawActivitySettings SundaySettings;

		// Token: 0x04001BD8 RID: 7128
		[Header("Demo Settings")]
		public float IntensityIncreasePerDay = 1.5f;

		// Token: 0x04001BDC RID: 7132
		private LawLoader loader = new LawLoader();
	}
}
