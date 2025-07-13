using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039B RID: 923
	public class BusinessLoader : PropertyLoader
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x0005CFDC File Offset: 0x0005B1DC
		public override void Load(string mainPath)
		{
			base.Load(mainPath);
			string text;
			if (base.TryLoadFile(mainPath, "Business", out text))
			{
				BusinessData businessData = null;
				try
				{
					businessData = JsonUtility.FromJson<BusinessData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (businessData != null)
				{
					Singleton<BusinessManager>.Instance.LoadBusiness(businessData);
				}
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0005D05C File Offset: 0x0005B25C
		public override void Load(PropertyData propertyData)
		{
			base.Load(propertyData);
		}
	}
}
