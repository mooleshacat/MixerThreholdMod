using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200072F RID: 1839
	public abstract class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
	{
		// Token: 0x060031C4 RID: 12740 RVA: 0x000CFDC5 File Offset: 0x000CDFC5
		protected override void Awake()
		{
			base.Awake();
			if (this.Destroyed)
			{
				return;
			}
			base.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
