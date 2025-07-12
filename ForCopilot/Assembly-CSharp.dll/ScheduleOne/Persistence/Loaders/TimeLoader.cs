using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B6 RID: 950
	public class TimeLoader : Loader
	{
		// Token: 0x06001543 RID: 5443 RVA: 0x0005EE00 File Offset: 0x0005D000
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				TimeData timeData = JsonUtility.FromJson<TimeData>(text);
				if (timeData != null)
				{
					NetworkSingleton<TimeManager>.Instance.SetTime(timeData.TimeOfDay, false);
					NetworkSingleton<TimeManager>.Instance.SetElapsedDays(timeData.ElapsedDays);
					NetworkSingleton<TimeManager>.Instance.SetPlaytime((float)timeData.Playtime);
				}
			}
		}
	}
}
