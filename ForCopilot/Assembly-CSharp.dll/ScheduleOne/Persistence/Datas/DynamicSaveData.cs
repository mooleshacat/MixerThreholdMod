using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000405 RID: 1029
	[Serializable]
	public class DynamicSaveData : SaveData
	{
		// Token: 0x06001624 RID: 5668 RVA: 0x00063608 File Offset: 0x00061808
		public DynamicSaveData(SaveData baseData)
		{
			this.DataType = baseData.DataType;
			this.GameVersion = baseData.GameVersion;
			this.BaseData = baseData.GetJson(false);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00063658 File Offset: 0x00061858
		public void AddData(string name, string contents)
		{
			if (this.GetData(name) != string.Empty)
			{
				Console.LogWarning("DynamicSaveData already has data with the name " + name + ". Replacing original data.", null);
				for (int i = 0; i < this.AdditionalDatas.Count; i++)
				{
					if (this.AdditionalDatas[i].Name == name)
					{
						this.AdditionalDatas[i].Contents = contents;
						return;
					}
				}
				return;
			}
			this.AdditionalDatas.Add(new DynamicSaveData.AdditionalData
			{
				Name = name,
				Contents = contents
			});
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x000636EF File Offset: 0x000618EF
		public void AddData(string name, SaveData data)
		{
			this.AddData(name, data.GetJson(false));
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00063700 File Offset: 0x00061900
		public string GetData(string name)
		{
			foreach (DynamicSaveData.AdditionalData additionalData in this.AdditionalDatas)
			{
				if (additionalData.Name == name)
				{
					return additionalData.Contents;
				}
			}
			return string.Empty;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0006376C File Offset: 0x0006196C
		public bool TryGetData(string name, out string data)
		{
			data = this.GetData(name);
			if (data != string.Empty)
			{
				return true;
			}
			Console.LogWarning("DynamicSaveData does not contain data with the name " + name + ".", null);
			return false;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000637A0 File Offset: 0x000619A0
		public T GetData<T>(string name) where T : SaveData
		{
			string data = this.GetData(name);
			if (data != string.Empty)
			{
				return JsonUtility.FromJson<T>(data);
			}
			Console.LogWarning("DynamicSaveData does not contain data with the name " + name + ".", null);
			return default(T);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000637E8 File Offset: 0x000619E8
		public bool TryGetData<T>(string name, out T data) where T : SaveData
		{
			data = this.GetData<T>(name);
			if (data != null)
			{
				return true;
			}
			Console.LogWarning("DynamicSaveData does not contain data with the name " + name + ".", null);
			return false;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00063820 File Offset: 0x00061A20
		public T ExtractBaseData<T>() where T : SaveData
		{
			if (this.BaseData != string.Empty)
			{
				return JsonUtility.FromJson<T>(this.BaseData);
			}
			Console.LogWarning("DynamicSaveData does not contain base data.", null);
			return default(T);
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0006385F File Offset: 0x00061A5F
		public bool TryExtractBaseData<T>(out T data) where T : SaveData
		{
			data = this.ExtractBaseData<T>();
			if (data != null)
			{
				return true;
			}
			Console.LogWarning("DynamicSaveData does not contain base data.", null);
			return false;
		}

		// Token: 0x040013F5 RID: 5109
		public string BaseData = string.Empty;

		// Token: 0x040013F6 RID: 5110
		public List<DynamicSaveData.AdditionalData> AdditionalDatas = new List<DynamicSaveData.AdditionalData>();

		// Token: 0x02000406 RID: 1030
		[Serializable]
		public class AdditionalData
		{
			// Token: 0x040013F7 RID: 5111
			public string Name;

			// Token: 0x040013F8 RID: 5112
			public string Contents;
		}
	}
}
