using System;
using FishNet.Object;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000726 RID: 1830
	public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x000CE95D File Offset: 0x000CCB5D
		public static bool InstanceExists
		{
			get
			{
				return NetworkSingleton<T>.instance != null;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06003188 RID: 12680 RVA: 0x000CE96F File Offset: 0x000CCB6F
		// (set) Token: 0x06003189 RID: 12681 RVA: 0x000CE976 File Offset: 0x000CCB76
		public static T Instance
		{
			get
			{
				return NetworkSingleton<T>.instance;
			}
			protected set
			{
				NetworkSingleton<T>.instance = value;
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000CE97E File Offset: 0x000CCB7E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.DevUtilities.NetworkSingleton`1_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000CE992 File Offset: 0x000CCB92
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<T>.instance == this)
			{
				NetworkSingleton<T>.instance = default(T);
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000CE9B1 File Offset: 0x000CCBB1
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000CE9C4 File Offset: 0x000CCBC4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.DevUtilities.NetworkSingleton`1Assembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000CE9D7 File Offset: 0x000CCBD7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000CE9E5 File Offset: 0x000CCBE5
		protected virtual void NetworkSingleton()
		{
			if (NetworkSingleton<T>.instance != null)
			{
				Console.LogWarning("Multiple instances of " + base.name + " exist. Keeping prior instance reference.", null);
				return;
			}
			NetworkSingleton<T>.instance = (T)((object)this);
		}

		// Token: 0x040022D9 RID: 8921
		private static T instance;

		// Token: 0x040022DA RID: 8922
		protected bool Destroyed;

		// Token: 0x040022DB RID: 8923
		private bool NetworkSingleton;

		// Token: 0x040022DC RID: 8924
		private bool NetworkSingleton;
	}
}
