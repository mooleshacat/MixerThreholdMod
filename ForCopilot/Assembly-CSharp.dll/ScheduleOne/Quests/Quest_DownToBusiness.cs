using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Variables;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F7 RID: 759
	public class Quest_DownToBusiness : Quest
	{
		// Token: 0x06001107 RID: 4359 RVA: 0x0004BE60 File Offset: 0x0004A060
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0004BE68 File Offset: 0x0004A068
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onDayPass = (Action)Delegate.Combine(instance.onDayPass, new Action(this.DayPass));
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0004BE98 File Offset: 0x0004A098
		private void DayPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Completed)
			{
				float num = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Days_Since_Tutorial_Completed");
				num += 1f;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Days_Since_Tutorial_Completed", num.ToString(), true);
			}
		}
	}
}
