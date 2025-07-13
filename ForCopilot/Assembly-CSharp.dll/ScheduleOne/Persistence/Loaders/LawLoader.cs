using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A1 RID: 929
	public class LawLoader : Loader
	{
		// Token: 0x0600150B RID: 5387 RVA: 0x0005D534 File Offset: 0x0005B734
		public override void Load(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, out text, true))
			{
				LawData lawData = null;
				try
				{
					lawData = JsonUtility.FromJson<LawData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (lawData != null)
				{
					Singleton<LawController>.Instance.Load(lawData);
				}
			}
		}
	}
}
