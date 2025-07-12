using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B0 RID: 944
	public class RankLoader : Loader
	{
		// Token: 0x06001537 RID: 5431 RVA: 0x0005EA28 File Offset: 0x0005CC28
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				RankData rankData = null;
				try
				{
					rankData = JsonUtility.FromJson<RankData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to load rank data: " + ex.Message);
				}
				if (rankData != null)
				{
					NetworkSingleton<LevelManager>.Instance.SetData(null, (ERank)rankData.Rank, rankData.Tier, rankData.XP, rankData.TotalXP);
				}
			}
		}
	}
}
