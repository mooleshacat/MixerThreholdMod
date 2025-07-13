using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040B RID: 1035
	[Serializable]
	public class GenericSaveData : SaveData
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x00063924 File Offset: 0x00061B24
		public GenericSaveData(string guid)
		{
			this.GUID = guid;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00063975 File Offset: 0x00061B75
		public void Add(string key, bool value)
		{
			this.boolValues.Add(new GenericSaveData.BoolValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00063995 File Offset: 0x00061B95
		public void Add(string key, float value)
		{
			this.floatValues.Add(new GenericSaveData.FloatValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000639B5 File Offset: 0x00061BB5
		public void Add(string key, int value)
		{
			this.intValues.Add(new GenericSaveData.IntValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000639D5 File Offset: 0x00061BD5
		public void Add(string key, string value)
		{
			this.stringValues.Add(new GenericSaveData.StringValue
			{
				key = key,
				value = value
			});
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000639F8 File Offset: 0x00061BF8
		public bool GetBool(string key, bool defaultValue = false)
		{
			GenericSaveData.BoolValue boolValue = this.boolValues.Find((GenericSaveData.BoolValue x) => x.key == key);
			if (boolValue != null)
			{
				return boolValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00063A38 File Offset: 0x00061C38
		public float GetFloat(string key, float defaultValue = 0f)
		{
			GenericSaveData.FloatValue floatValue = this.floatValues.Find((GenericSaveData.FloatValue x) => x.key == key);
			if (floatValue != null)
			{
				return floatValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00063A78 File Offset: 0x00061C78
		public int GetInt(string key, int defaultValue = 0)
		{
			GenericSaveData.IntValue intValue = this.intValues.Find((GenericSaveData.IntValue x) => x.key == key);
			if (intValue != null)
			{
				return intValue.value;
			}
			return defaultValue;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00063AB8 File Offset: 0x00061CB8
		public string GetString(string key, string defaultValue = "")
		{
			GenericSaveData.StringValue stringValue = this.stringValues.Find((GenericSaveData.StringValue x) => x.key == key);
			if (stringValue != null)
			{
				return stringValue.value;
			}
			return defaultValue;
		}

		// Token: 0x04001402 RID: 5122
		public string GUID = string.Empty;

		// Token: 0x04001403 RID: 5123
		public List<GenericSaveData.BoolValue> boolValues = new List<GenericSaveData.BoolValue>();

		// Token: 0x04001404 RID: 5124
		public List<GenericSaveData.FloatValue> floatValues = new List<GenericSaveData.FloatValue>();

		// Token: 0x04001405 RID: 5125
		public List<GenericSaveData.IntValue> intValues = new List<GenericSaveData.IntValue>();

		// Token: 0x04001406 RID: 5126
		public List<GenericSaveData.StringValue> stringValues = new List<GenericSaveData.StringValue>();

		// Token: 0x0200040C RID: 1036
		[Serializable]
		public class BoolValue
		{
			// Token: 0x04001407 RID: 5127
			public string key;

			// Token: 0x04001408 RID: 5128
			public bool value;
		}

		// Token: 0x0200040D RID: 1037
		[Serializable]
		public class FloatValue
		{
			// Token: 0x04001409 RID: 5129
			public string key;

			// Token: 0x0400140A RID: 5130
			public float value;
		}

		// Token: 0x0200040E RID: 1038
		[Serializable]
		public class IntValue
		{
			// Token: 0x0400140B RID: 5131
			public string key;

			// Token: 0x0400140C RID: 5132
			public int value;
		}

		// Token: 0x0200040F RID: 1039
		[Serializable]
		public class StringValue
		{
			// Token: 0x0400140D RID: 5133
			public string key;

			// Token: 0x0400140E RID: 5134
			public string value;
		}
	}
}
