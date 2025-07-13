using System;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039E RID: 926
	public class DynamicLoader
	{
		// Token: 0x06001501 RID: 5377 RVA: 0x0005D2C8 File Offset: 0x0005B4C8
		public void Load(string serializedDynamicSaveData)
		{
			if (string.IsNullOrEmpty(serializedDynamicSaveData))
			{
				Console.LogError("DynamicLoader: No data to load.", null);
				return;
			}
			DynamicSaveData dynamicSaveData = null;
			try
			{
				dynamicSaveData = JsonUtility.FromJson<DynamicSaveData>(serializedDynamicSaveData);
			}
			catch (Exception ex)
			{
				Type type = base.GetType();
				string str = (type != null) ? type.ToString() : null;
				string str2 = " error reading data: ";
				Exception ex2 = ex;
				Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
			}
			if (dynamicSaveData != null)
			{
				this.Load(dynamicSaveData);
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(DynamicSaveData saveData)
		{
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0005D340 File Offset: 0x0005B540
		public static T ExtractBaseData<T>(DynamicSaveData saveData) where T : SaveData
		{
			if (saveData == null)
			{
				Console.LogError("DynamicLoader: No data to extract.", null);
				return default(T);
			}
			T result = default(T);
			try
			{
				result = JsonUtility.FromJson<T>(saveData.BaseData);
			}
			catch (Exception ex)
			{
				string str = "DynamicLoader: Error extracting base data: ";
				Exception ex2 = ex;
				Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null), null);
			}
			return result;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0005D3AC File Offset: 0x0005B5AC
		public static bool TryExtractBaseData<T>(DynamicSaveData saveData, out T baseData) where T : SaveData
		{
			baseData = default(T);
			if (saveData == null)
			{
				Console.LogError("DynamicLoader: No data to extract.", null);
				return false;
			}
			try
			{
				baseData = JsonUtility.FromJson<T>(saveData.BaseData);
			}
			catch (Exception ex)
			{
				string str = "DynamicLoader: Error extracting base data: ";
				Exception ex2 = ex;
				Console.LogError(str + ((ex2 != null) ? ex2.ToString() : null), null);
				return false;
			}
			return baseData != null;
		}
	}
}
