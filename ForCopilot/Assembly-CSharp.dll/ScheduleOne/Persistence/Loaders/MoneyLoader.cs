using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A5 RID: 933
	public class MoneyLoader : Loader
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x0005D7D8 File Offset: 0x0005B9D8
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				MoneyData moneyData = null;
				try
				{
					moneyData = JsonUtility.FromJson<MoneyData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (moneyData != null)
				{
					NetworkSingleton<MoneyManager>.Instance.Load(moneyData);
				}
			}
		}
	}
}
