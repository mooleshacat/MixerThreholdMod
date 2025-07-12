using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x02000849 RID: 2121
	public class Business : Property, ISaveable
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x000F351F File Offset: 0x000F171F
		public float currentLaunderTotal
		{
			get
			{
				return this.LaunderingOperations.Sum((LaunderingOperation x) => x.amount);
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06003946 RID: 14662 RVA: 0x000F354B File Offset: 0x000F174B
		public float appliedLaunderLimit
		{
			get
			{
				return this.LaunderCapacity - this.currentLaunderTotal;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x000F355A File Offset: 0x000F175A
		public new Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x000F3562 File Offset: 0x000F1762
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Business_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x000F3578 File Offset: 0x000F1778
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x000F35D8 File Offset: 0x000F17D8
		protected override void OnDestroy()
		{
			Business.Businesses.Remove(this);
			Business.UnownedBusinesses.Remove(this);
			Business.OwnedBusinesses.Remove(this);
			base.OnDestroy();
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x000F3604 File Offset: 0x000F1804
		protected override void GetNetworth(MoneyManager.FloatContainer container)
		{
			base.GetNetworth(container);
			container.ChangeValue(this.currentLaunderTotal);
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x000F361C File Offset: 0x000F181C
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			for (int i = 0; i < this.LaunderingOperations.Count; i++)
			{
				this.ReceiveLaunderingOperation(connection, this.LaunderingOperations[i].amount, this.LaunderingOperations[i].minutesSinceStarted);
			}
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x000F366F File Offset: 0x000F186F
		protected virtual void MinPass()
		{
			this.MinsPass(1);
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x000F3678 File Offset: 0x000F1878
		protected virtual void MinsPass(int mins)
		{
			for (int i = 0; i < this.LaunderingOperations.Count; i++)
			{
				this.LaunderingOperations[i].minutesSinceStarted += mins;
				if (this.LaunderingOperations[i].minutesSinceStarted >= this.LaunderingOperations[i].completionTime_Minutes)
				{
					this.CompleteOperation(this.LaunderingOperations[i]);
					i--;
				}
			}
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x000F36EE File Offset: 0x000F18EE
		private void TimeSkipped(int minsPassed)
		{
			this.MinsPass(minsPassed);
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x000F36F8 File Offset: 0x000F18F8
		public override string GetSaveString()
		{
			bool[] array = new bool[this.Switches.Count];
			for (int i = 0; i < this.Switches.Count; i++)
			{
				array[i] = this.Switches[i].isOn;
			}
			LaunderOperationData[] array2 = new LaunderOperationData[this.LaunderingOperations.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new LaunderOperationData(this.LaunderingOperations[j].amount, this.LaunderingOperations[j].minutesSinceStarted);
			}
			bool[] array3 = new bool[this.Toggleables.Count];
			for (int k = 0; k < this.Toggleables.Count; k++)
			{
				array3[k] = this.Toggleables[k].IsActivated;
			}
			return new BusinessData(this.propertyCode, base.IsOwned, array, array3, base.GetEmployeeSaveDatas().ToArray(), base.GetObjectSaveDatas().ToArray(), array2).GetJson(true);
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x000F3804 File Offset: 0x000F1A04
		public override void Load(PropertyData propertyData)
		{
			base.Load(propertyData);
			Console.Log("Loading business: " + propertyData.PropertyCode, null);
			BusinessData businessData = propertyData as BusinessData;
			if (businessData != null)
			{
				for (int i = 0; i < businessData.LaunderingOperations.Length; i++)
				{
					this.StartLaunderingOperation(businessData.LaunderingOperations[i].Amount, businessData.LaunderingOperations[i].MinutesSinceStarted);
				}
			}
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x000F386B File Offset: 0x000F1A6B
		protected override void RecieveOwned()
		{
			base.RecieveOwned();
			Business.UnownedBusinesses.Remove(this);
			if (!Business.OwnedBusinesses.Contains(this))
			{
				Business.OwnedBusinesses.Add(this);
			}
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x000F3897 File Offset: 0x000F1A97
		[ServerRpc(RequireOwnership = false)]
		public void StartLaunderingOperation(float amount, int minutesSinceStarted = 0)
		{
			this.RpcWriter___Server_StartLaunderingOperation_1481775633(amount, minutesSinceStarted);
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x000F38A8 File Offset: 0x000F1AA8
		[TargetRpc]
		[ObserversRpc]
		private void ReceiveLaunderingOperation(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveLaunderingOperation_1001022388(conn, amount, minutesSinceStarted);
			}
			else
			{
				this.RpcWriter___Target_ReceiveLaunderingOperation_1001022388(conn, amount, minutesSinceStarted);
			}
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x000F38E4 File Offset: 0x000F1AE4
		protected void CompleteOperation(LaunderingOperation op)
		{
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Money laundering (" + this.propertyName + ")", op.amount, 1f, string.Empty);
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LaunderingOperationsCompleted");
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LaunderingOperationsCompleted", (value + 1f).ToString(), true);
			}
			Singleton<NotificationsManager>.Instance.SendNotification(this.propertyName, "<color=#16F01C>" + MoneyManager.FormatAmount(op.amount, false, false) + "</color> Laundered", NetworkSingleton<MoneyManager>.Instance.LaunderingNotificationIcon, 5f, true);
			this.LaunderingOperations.Remove(op);
			base.HasChanged = true;
			if (Business.onOperationFinished != null)
			{
				Business.onOperationFinished(op);
			}
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000F3A04 File Offset: 0x000F1C04
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_StartLaunderingOperation_1481775633));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveLaunderingOperation_1001022388));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveLaunderingOperation_1001022388));
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x000F3A6D File Offset: 0x000F1C6D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x000F3A86 File Offset: 0x000F1C86
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x000F3A94 File Offset: 0x000F1C94
		private void RpcWriter___Server_StartLaunderingOperation_1481775633(float amount, int minutesSinceStarted = 0)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(amount, 0);
			writer.WriteInt32(minutesSinceStarted, 1);
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x000F3B52 File Offset: 0x000F1D52
		public void RpcLogic___StartLaunderingOperation_1481775633(float amount, int minutesSinceStarted = 0)
		{
			this.ReceiveLaunderingOperation(null, amount, minutesSinceStarted);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x000F3B60 File Offset: 0x000F1D60
		private void RpcReader___Server_StartLaunderingOperation_1481775633(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float amount = PooledReader0.ReadSingle(0);
			int minutesSinceStarted = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___StartLaunderingOperation_1481775633(amount, minutesSinceStarted);
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x000F3BAC File Offset: 0x000F1DAC
		private void RpcWriter___Target_ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(amount, 0);
			writer.WriteInt32(minutesSinceStarted, 1);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x000F3C78 File Offset: 0x000F1E78
		private void RpcLogic___ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			Console.Log(string.Concat(new string[]
			{
				"Received laundering operation: ",
				amount.ToString(),
				" for ",
				minutesSinceStarted.ToString(),
				" minutes"
			}), null);
			LaunderingOperation launderingOperation = new LaunderingOperation(this, amount, minutesSinceStarted);
			this.LaunderingOperations.Add(launderingOperation);
			base.HasChanged = true;
			if (Business.onOperationStarted != null)
			{
				Business.onOperationStarted(launderingOperation);
			}
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x000F3CF0 File Offset: 0x000F1EF0
		private void RpcReader___Target_ReceiveLaunderingOperation_1001022388(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(0);
			int minutesSinceStarted = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveLaunderingOperation_1001022388(base.LocalConnection, amount, minutesSinceStarted);
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x000F3D44 File Offset: 0x000F1F44
		private void RpcWriter___Observers_ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(amount, 0);
			writer.WriteInt32(minutesSinceStarted, 1);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x000F3E14 File Offset: 0x000F2014
		private void RpcReader___Observers_ReceiveLaunderingOperation_1001022388(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(0);
			int minutesSinceStarted = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveLaunderingOperation_1001022388(null, amount, minutesSinceStarted);
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x000F3E61 File Offset: 0x000F2061
		protected override void dll()
		{
			base.Awake();
			Business.Businesses.Add(this);
			Business.UnownedBusinesses.Remove(this);
			Business.UnownedBusinesses.Add(this);
		}

		// Token: 0x04002970 RID: 10608
		public static List<Business> Businesses = new List<Business>();

		// Token: 0x04002971 RID: 10609
		public static List<Business> UnownedBusinesses = new List<Business>();

		// Token: 0x04002972 RID: 10610
		public static List<Business> OwnedBusinesses = new List<Business>();

		// Token: 0x04002973 RID: 10611
		[Header("Settings")]
		public float LaunderCapacity = 1000f;

		// Token: 0x04002974 RID: 10612
		public List<LaunderingOperation> LaunderingOperations = new List<LaunderingOperation>();

		// Token: 0x04002975 RID: 10613
		public static Action<LaunderingOperation> onOperationStarted;

		// Token: 0x04002976 RID: 10614
		public static Action<LaunderingOperation> onOperationFinished;

		// Token: 0x04002977 RID: 10615
		private BusinessLoader loader = new BusinessLoader();

		// Token: 0x04002978 RID: 10616
		private bool dll_Excuted;

		// Token: 0x04002979 RID: 10617
		private bool dll_Excuted;
	}
}
