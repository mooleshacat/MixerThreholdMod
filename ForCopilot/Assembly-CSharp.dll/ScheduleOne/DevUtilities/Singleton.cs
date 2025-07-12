using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000736 RID: 1846
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000D0325 File Offset: 0x000CE525
		public static bool InstanceExists
		{
			get
			{
				return Singleton<T>.instance != null;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060031E2 RID: 12770 RVA: 0x000D0337 File Offset: 0x000CE537
		// (set) Token: 0x060031E3 RID: 12771 RVA: 0x000D033E File Offset: 0x000CE53E
		public static T Instance
		{
			get
			{
				return Singleton<T>.instance;
			}
			protected set
			{
				Singleton<T>.instance = value;
			}
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000D0348 File Offset: 0x000CE548
		protected virtual void Awake()
		{
			if (Singleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Destroying this instance.", null);
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Singleton<T>.instance = (T)((object)this);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000D0399 File Offset: 0x000CE599
		protected virtual void OnDestroy()
		{
			if (Singleton<T>.instance == this)
			{
				Singleton<T>.instance = default(T);
			}
		}

		// Token: 0x04002324 RID: 8996
		private static T instance;

		// Token: 0x04002325 RID: 8997
		protected bool Destroyed;
	}
}
