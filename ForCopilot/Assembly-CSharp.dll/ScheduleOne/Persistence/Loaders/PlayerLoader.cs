using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A7 RID: 935
	public class PlayerLoader : Loader
	{
		// Token: 0x0600151D RID: 5405 RVA: 0x0005D950 File Offset: 0x0005BB50
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "Player", out text))
			{
				PlayerData playerData = null;
				try
				{
					playerData = JsonUtility.FromJson<PlayerData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (playerData != null)
				{
					Singleton<PlayerManager>.Instance.LoadPlayer(playerData, mainPath);
				}
			}
		}
	}
}
