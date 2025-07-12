using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000732 RID: 1842
	public abstract class PlayerSingleton<T> : MonoBehaviour where T : PlayerSingleton<T>
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x000D0032 File Offset: 0x000CE232
		public static bool InstanceExists
		{
			get
			{
				return PlayerSingleton<T>.instance != null;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x000D0044 File Offset: 0x000CE244
		// (set) Token: 0x060031CD RID: 12749 RVA: 0x000D004B File Offset: 0x000CE24B
		public static T Instance
		{
			get
			{
				return PlayerSingleton<T>.instance;
			}
			protected set
			{
				PlayerSingleton<T>.instance = value;
			}
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000D0053 File Offset: 0x000CE253
		protected virtual void Awake()
		{
			this.OnStartClient(true);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000D005C File Offset: 0x000CE25C
		public virtual void OnStartClient(bool IsOwner)
		{
			if (!IsOwner)
			{
				Console.Log("Destroying non-local player singleton: " + base.name, null);
				UnityEngine.Object.Destroy(this);
				return;
			}
			if (PlayerSingleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Keeping prior instance reference.", null);
				return;
			}
			PlayerSingleton<T>.instance = (T)((object)this);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000D00C2 File Offset: 0x000CE2C2
		protected virtual void OnDestroy()
		{
			if (PlayerSingleton<T>.instance == this)
			{
				PlayerSingleton<T>.instance = default(T);
			}
		}

		// Token: 0x04002316 RID: 8982
		private static T instance;
	}
}
