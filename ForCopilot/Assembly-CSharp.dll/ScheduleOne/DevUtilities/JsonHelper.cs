using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200071E RID: 1822
	public static class JsonHelper
	{
		// Token: 0x0600315A RID: 12634 RVA: 0x000CE20E File Offset: 0x000CC40E
		public static T[] FromJson<T>(string json)
		{
			return JsonUtility.FromJson<JsonHelper.Wrapper<T>>(json).Items;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000CE21B File Offset: 0x000CC41B
		public static string ToJson<T>(T[] array)
		{
			return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
			{
				Items = array
			});
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000CE22E File Offset: 0x000CC42E
		public static string ToJson<T>(T[] array, bool prettyPrint)
		{
			return JsonUtility.ToJson(new JsonHelper.Wrapper<T>
			{
				Items = array
			}, prettyPrint);
		}

		// Token: 0x0200071F RID: 1823
		[Serializable]
		private class Wrapper<T>
		{
			// Token: 0x040022C4 RID: 8900
			public T[] Items;
		}
	}
}
