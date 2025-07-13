using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039F RID: 927
	public class GameDataLoader : Loader
	{
		// Token: 0x06001507 RID: 5383 RVA: 0x0005D428 File Offset: 0x0005B628
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				GameData gameData = JsonUtility.FromJson<GameData>(text);
				if (gameData != null)
				{
					NetworkSingleton<GameManager>.Instance.Load(gameData, mainPath);
				}
			}
		}
	}
}
