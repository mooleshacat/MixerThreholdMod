using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.Management;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Management;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property
{
	// Token: 0x02000850 RID: 2128
	public class Property : NetworkBehaviour, ISaveable
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x000F427B File Offset: 0x000F247B
		// (set) Token: 0x06003987 RID: 14727 RVA: 0x000F4283 File Offset: 0x000F2483
		public bool IsOwned { get; protected set; }

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06003988 RID: 14728 RVA: 0x000F428C File Offset: 0x000F248C
		// (set) Token: 0x06003989 RID: 14729 RVA: 0x000F4294 File Offset: 0x000F2494
		public List<Employee> Employees { get; protected set; } = new List<Employee>();

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x000F429D File Offset: 0x000F249D
		// (set) Token: 0x0600398B RID: 14731 RVA: 0x000F42A5 File Offset: 0x000F24A5
		public RectTransform WorldspaceUIContainer { get; protected set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x000F42AE File Offset: 0x000F24AE
		// (set) Token: 0x0600398D RID: 14733 RVA: 0x000F42B6 File Offset: 0x000F24B6
		public bool IsContentCulled { get; set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x000F42BF File Offset: 0x000F24BF
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x000F42C7 File Offset: 0x000F24C7
		public string PropertyCode
		{
			get
			{
				return this.propertyCode;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003990 RID: 14736 RVA: 0x000F42CF File Offset: 0x000F24CF
		public int LoadingDockCount
		{
			get
			{
				return this.LoadingDocks.Length;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003991 RID: 14737 RVA: 0x000F42BF File Offset: 0x000F24BF
		public string SaveFolderName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003992 RID: 14738 RVA: 0x000F42D9 File Offset: 0x000F24D9
		public string SaveFileName
		{
			get
			{
				return SaveManager.MakeFileSafe(this.propertyName);
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x000F42E6 File Offset: 0x000F24E6
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003994 RID: 14740 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000F42EE File Offset: 0x000F24EE
		// (set) Token: 0x06003996 RID: 14742 RVA: 0x000F42F6 File Offset: 0x000F24F6
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x000F42FF File Offset: 0x000F24FF
		// (set) Token: 0x06003998 RID: 14744 RVA: 0x000F4307 File Offset: 0x000F2507
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x000F4310 File Offset: 0x000F2510
		// (set) Token: 0x0600399A RID: 14746 RVA: 0x000F4318 File Offset: 0x000F2518
		public bool HasChanged { get; set; } = true;

		// Token: 0x0600399B RID: 14747 RVA: 0x000F4324 File Offset: 0x000F2524
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Property_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x000F4343 File Offset: 0x000F2543
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000F436C File Offset: 0x000F256C
		protected virtual void FixedUpdate()
		{
			this.UpdateCulling();
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000F4374 File Offset: 0x000F2574
		public void AddConfigurable(IConfigurable configurable)
		{
			if (this.Configurables.Contains(configurable))
			{
				return;
			}
			this.Configurables.Add(configurable);
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000F4391 File Offset: 0x000F2591
		public void RemoveConfigurable(IConfigurable configurable)
		{
			if (!this.Configurables.Contains(configurable))
			{
				return;
			}
			this.Configurables.Remove(configurable);
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000F43B0 File Offset: 0x000F25B0
		private void UpdateCulling()
		{
			if (!Singleton<LoadManager>.InstanceExists || Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.ContentCullingEnabled)
			{
				this.SetContentCulled(false);
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (num < this.MinimumCullingDistance)
			{
				this.SetContentCulled(false);
				return;
			}
			if (num > this.MinimumCullingDistance + 5f)
			{
				this.SetContentCulled(true);
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x000F4430 File Offset: 0x000F2630
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			for (int i = 0; i < this.Toggleables.Count; i++)
			{
				if (this.Toggleables[i].IsActivated)
				{
					this.SetToggleableState(connection, i, this.Toggleables[i].IsActivated);
				}
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000F4488 File Offset: 0x000F2688
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			Property.Properties.Remove(this);
			Property.UnownedProperties.Remove(this);
			Property.OwnedProperties.Remove(this);
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x000F44E7 File Offset: 0x000F26E7
		protected virtual void GetNetworth(MoneyManager.FloatContainer container)
		{
			if (this.IsOwned)
			{
				container.ChangeValue(this.Price);
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x000F4500 File Offset: 0x000F2700
		public override void OnStartServer()
		{
			base.OnStartServer();
			if ((Application.isEditor || Debug.isDebugBuild) && this.DEBUG_SET_OWNED)
			{
				this.SetOwned_Server();
			}
			else if (this.OwnedByDefault)
			{
				this.SetOwned_Server();
			}
			if (base.NetworkObject.GetInitializeOrder() == 0)
			{
				Console.LogError("Property " + this.PropertyName + " has an initialize order of 0. This will cause issues.", null);
			}
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x000F4567 File Offset: 0x000F2767
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected void SetOwned_Server()
		{
			this.RpcWriter___Server_SetOwned_Server_2166136261();
			this.RpcLogic___SetOwned_Server_2166136261();
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000F4575 File Offset: 0x000F2775
		[ObserversRpc(RunLocally = true, BufferLast = true)]
		private void ReceiveOwned_Networked()
		{
			this.RpcWriter___Observers_ReceiveOwned_Networked_2166136261();
			this.RpcLogic___ReceiveOwned_Networked_2166136261();
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x000F4584 File Offset: 0x000F2784
		protected virtual void RecieveOwned()
		{
			if (this.IsOwned)
			{
				return;
			}
			this.IsOwned = true;
			this.HasChanged = true;
			if (this.IsOwnedVariable != string.Empty && NetworkSingleton<VariableDatabase>.InstanceExists && InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.IsOwnedVariable, "true", true);
			}
			if (Property.UnownedProperties.Contains(this))
			{
				Property.UnownedProperties.Remove(this);
				Property.OwnedProperties.Add(this);
			}
			if (Property.onPropertyAcquired != null)
			{
				Property.onPropertyAcquired(this);
			}
			if (this.onThisPropertyAcquired != null)
			{
				this.onThisPropertyAcquired.Invoke();
			}
			this.ForSaleSign.gameObject.SetActive(false);
			if (this.ListingPoster != null)
			{
				this.ListingPoster.gameObject.SetActive(false);
			}
			this.PoI.gameObject.SetActive(true);
			this.PoI.SetMainText(this.propertyName + " (Owned)");
			base.StartCoroutine(this.<RecieveOwned>g__Wait|93_0());
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x000F4690 File Offset: 0x000F2890
		public virtual bool ShouldSave()
		{
			return this.IsOwned || this.Container.transform.childCount > 0;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x000F46AF File Offset: 0x000F28AF
		public void SetOwned()
		{
			this.SetOwned_Server();
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetBoundsVisible(bool vis)
		{
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000F46B8 File Offset: 0x000F28B8
		public virtual void SetContentCulled(bool culled)
		{
			if (this.IsContentCulled == culled)
			{
				return;
			}
			this.IsContentCulled = culled;
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				if (!(buildableItem == null))
				{
					buildableItem.SetCulled(culled);
				}
			}
			foreach (GameObject gameObject in this.ObjectsToCull)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(!culled);
				}
			}
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x000F4754 File Offset: 0x000F2954
		public int RegisterEmployee(Employee emp)
		{
			this.Employees.Add(emp);
			return this.Employees.IndexOf(emp);
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x000F476E File Offset: 0x000F296E
		public void DeregisterEmployee(Employee emp)
		{
			this.Employees.Remove(emp);
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x000F477D File Offset: 0x000F297D
		private void ToggleableActioned(InteractableToggleable toggleable)
		{
			this.HasChanged = true;
			this.SendToggleableState(this.Toggleables.IndexOf(toggleable), toggleable.IsActivated);
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x000F479E File Offset: 0x000F299E
		[ServerRpc(RequireOwnership = false)]
		public void SendToggleableState(int index, bool state)
		{
			this.RpcWriter___Server_SendToggleableState_3658436649(index, state);
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000F47AE File Offset: 0x000F29AE
		[ObserversRpc]
		[TargetRpc]
		public void SetToggleableState(NetworkConnection conn, int index, bool state)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetToggleableState_338960014(conn, index, state);
			}
			else
			{
				this.RpcWriter___Target_SetToggleableState_338960014(conn, index, state);
			}
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000F47E0 File Offset: 0x000F29E0
		public virtual string GetSaveString()
		{
			bool[] array = new bool[this.Switches.Count];
			for (int i = 0; i < this.Switches.Count; i++)
			{
				if (!(this.Switches[i] == null))
				{
					array[i] = this.Switches[i].isOn;
				}
			}
			bool[] array2 = new bool[this.Toggleables.Count];
			for (int j = 0; j < this.Toggleables.Count; j++)
			{
				if (!(this.Toggleables[j] == null))
				{
					array2[j] = this.Toggleables[j].IsActivated;
				}
			}
			return new PropertyData(this.propertyCode, this.IsOwned, array, array2, this.GetEmployeeSaveDatas().ToArray(), this.GetObjectSaveDatas().ToArray()).GetJson(true);
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x000F48BC File Offset: 0x000F2ABC
		protected List<DynamicSaveData> GetEmployeeSaveDatas()
		{
			List<DynamicSaveData> list = new List<DynamicSaveData>();
			for (int i = 0; i < this.Employees.Count; i++)
			{
				if (!(this.Employees[i] == null))
				{
					list.Add(this.Employees[i].GetSaveData());
				}
			}
			return list;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x000F4914 File Offset: 0x000F2B14
		protected List<DynamicSaveData> GetObjectSaveDatas()
		{
			List<DynamicSaveData> list = new List<DynamicSaveData>();
			for (int i = 0; i < this.BuildableItems.Count; i++)
			{
				if (!(this.BuildableItems[i] == null))
				{
					list.Add(this.BuildableItems[i].GetSaveData());
				}
			}
			return list;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x000F4969 File Offset: 0x000F2B69
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			this.savedObjectPaths.Clear();
			this.savedEmployeePaths.Clear();
			return result;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void DeleteUnapprovedFiles(string parentFolderPath)
		{
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x000F4988 File Offset: 0x000F2B88
		public virtual void Load(PropertyData propertyData)
		{
			if (propertyData.IsOwned)
			{
				this.SetOwned();
			}
			int num = 0;
			while (num < propertyData.SwitchStates.Length && num < this.Switches.Count)
			{
				if (propertyData.SwitchStates[num] && this.Switches.Count > num)
				{
					this.Switches[num].SwitchOn();
				}
				num++;
			}
			if (propertyData.ToggleableStates != null)
			{
				int num2 = 0;
				while (num2 < propertyData.ToggleableStates.Length && num2 < this.Toggleables.Count)
				{
					if (propertyData.ToggleableStates[num2] && this.Toggleables.Count > num2)
					{
						this.Toggleables[num2].Toggle();
					}
					num2++;
				}
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x000F4A40 File Offset: 0x000F2C40
		public bool DoBoundsContainPoint(Vector3 point)
		{
			foreach (BoxCollider boxCollider in this.propertyBoundsColliders)
			{
				if (!(boxCollider == null) && this.IsPointInsideBox(point, boxCollider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x000F4A7C File Offset: 0x000F2C7C
		private bool IsPointInsideBox(Vector3 worldPoint, BoxCollider box)
		{
			if (box == null)
			{
				Console.LogWarning("BoxCollider is null.", null);
				return false;
			}
			Vector3 vector = box.transform.InverseTransformPoint(worldPoint);
			vector -= box.center;
			Vector3 vector2 = box.size * 0.5f;
			return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.y) <= vector2.y && Mathf.Abs(vector.z) <= vector2.z;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x000F4B08 File Offset: 0x000F2D08
		public List<Bed> GetUnassignedBeds()
		{
			return (from x in this.Container.GetComponentsInChildren<Bed>()
			where x.AssignedEmployee == null
			select x).ToList<Bed>();
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x000F4B40 File Offset: 0x000F2D40
		public List<T> GetBuildablesOfType<T>() where T : BuildableItem
		{
			List<T> list = new List<T>();
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				if (!(buildableItem == null) && buildableItem is T)
				{
					list.Add((T)((object)buildableItem));
				}
			}
			return list;
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x000F4C98 File Offset: 0x000F2E98
		[CompilerGenerated]
		private IEnumerator <RecieveOwned>g__Wait|93_0()
		{
			yield return new WaitUntil(() => this.PoI.UISetup);
			this.PoI.IconContainer.Find("Unowned").gameObject.SetActive(false);
			this.PoI.IconContainer.Find("Owned").gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x000F4CB4 File Offset: 0x000F2EB4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetOwned_Server_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveOwned_Networked_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendToggleableState_3658436649));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetToggleableState_338960014));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetToggleableState_338960014));
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x000F4D45 File Offset: 0x000F2F45
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x000F4D58 File Offset: 0x000F2F58
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x000F4D68 File Offset: 0x000F2F68
		private void RpcWriter___Server_SetOwned_Server_2166136261()
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x000F4E02 File Offset: 0x000F3002
		protected void RpcLogic___SetOwned_Server_2166136261()
		{
			this.ReceiveOwned_Networked();
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x000F4E0C File Offset: 0x000F300C
		private void RpcReader___Server_SetOwned_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetOwned_Server_2166136261();
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x000F4E3C File Offset: 0x000F303C
		private void RpcWriter___Observers_ReceiveOwned_Networked_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, 0, true, false, false);
			writer.Store();
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x000F4EE5 File Offset: 0x000F30E5
		private void RpcLogic___ReceiveOwned_Networked_2166136261()
		{
			this.RecieveOwned();
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x000F4EF0 File Offset: 0x000F30F0
		private void RpcReader___Observers_ReceiveOwned_Networked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveOwned_Networked_2166136261();
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x000F4F1C File Offset: 0x000F311C
		private void RpcWriter___Server_SendToggleableState_3658436649(int index, bool state)
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
			writer.WriteInt32(index, 1);
			writer.WriteBoolean(state);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x000F4FD5 File Offset: 0x000F31D5
		public void RpcLogic___SendToggleableState_3658436649(int index, bool state)
		{
			this.SetToggleableState(null, index, state);
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x000F4FE0 File Offset: 0x000F31E0
		private void RpcReader___Server_SendToggleableState_3658436649(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(1);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendToggleableState_3658436649(index, state);
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x000F5028 File Offset: 0x000F3228
		private void RpcWriter___Observers_SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
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
			writer.WriteInt32(index, 1);
			writer.WriteBoolean(state);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x000F50F0 File Offset: 0x000F32F0
		public void RpcLogic___SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
		{
			this.Toggleables[index].SetState(state);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x000F5104 File Offset: 0x000F3304
		private void RpcReader___Observers_SetToggleableState_338960014(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(1);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetToggleableState_338960014(null, index, state);
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x000F514C File Offset: 0x000F334C
		private void RpcWriter___Target_SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
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
			writer.WriteInt32(index, 1);
			writer.WriteBoolean(state);
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x000F5214 File Offset: 0x000F3414
		private void RpcReader___Target_SetToggleableState_338960014(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(1);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetToggleableState_338960014(base.LocalConnection, index, state);
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000F5264 File Offset: 0x000F3464
		protected virtual void dll()
		{
			this.propertyBoundsColliders = this.BoundingBox.GetComponentsInChildren<BoxCollider>();
			foreach (BoxCollider boxCollider in this.propertyBoundsColliders)
			{
				boxCollider.isTrigger = true;
				boxCollider.gameObject.layer = LayerMask.NameToLayer("Invisible");
			}
			if (!(this is Business))
			{
				Property.Properties.Add(this);
				Property.UnownedProperties.Remove(this);
				Property.UnownedProperties.Add(this);
			}
			this.Container.Property = this;
			this.PoI.SetMainText(this.propertyName + " (Unowned)");
			this.SetBoundsVisible(false);
			this.ForSaleSign.transform.Find("Name").GetComponent<TextMeshPro>().text = this.propertyName;
			this.ForSaleSign.transform.Find("Price").GetComponent<TextMeshPro>().text = MoneyManager.FormatAmount(this.Price, false, false);
			if (this.DisposalArea == null)
			{
				Console.LogWarning("Property " + this.PropertyName + " has no disposal area.", null);
			}
			if (this.EmployeeIdlePoints.Length < this.EmployeeCapacity)
			{
				Debug.LogWarning("Property " + this.PropertyName + " has less idle points than employee capacity.");
			}
			if (!GameManager.IS_TUTORIAL)
			{
				this.WorldspaceUIContainer = new GameObject(this.propertyName + " Worldspace UI Container").AddComponent<RectTransform>();
				this.WorldspaceUIContainer.SetParent(Singleton<ManagementWorldspaceCanvas>.Instance.Canvas.transform);
				this.WorldspaceUIContainer.gameObject.SetActive(false);
			}
			if (this.ListingPoster != null)
			{
				this.ListingPoster.Find("Title").GetComponent<TextMeshPro>().text = this.propertyName;
				this.ListingPoster.Find("Price").GetComponent<TextMeshPro>().text = MoneyManager.FormatAmount(this.Price, false, false);
				this.ListingPoster.Find("Parking/Text").GetComponent<TextMeshPro>().text = this.LoadingDockCount.ToString();
				this.ListingPoster.Find("Employee/Text").GetComponent<TextMeshPro>().text = this.EmployeeCapacity.ToString();
			}
			this.PoI.gameObject.SetActive(false);
			foreach (ModularSwitch modularSwitch in this.Switches)
			{
				if (!(modularSwitch == null))
				{
					ModularSwitch modularSwitch2 = modularSwitch;
					modularSwitch2.onToggled = (ModularSwitch.ButtonChange)Delegate.Combine(modularSwitch2.onToggled, new ModularSwitch.ButtonChange(delegate(bool <p0>)
					{
						this.HasChanged = true;
					}));
				}
			}
			foreach (InteractableToggleable interactableToggleable in this.Toggleables)
			{
				if (!(interactableToggleable == null))
				{
					InteractableToggleable toggleable1 = interactableToggleable;
					interactableToggleable.onToggle.AddListener(delegate()
					{
						this.ToggleableActioned(toggleable1);
					});
				}
			}
			this.InitializeSaveable();
		}

		// Token: 0x04002989 RID: 10633
		public static List<Property> Properties = new List<Property>();

		// Token: 0x0400298A RID: 10634
		public static List<Property> UnownedProperties = new List<Property>();

		// Token: 0x0400298B RID: 10635
		public static List<Property> OwnedProperties = new List<Property>();

		// Token: 0x0400298C RID: 10636
		public static Property.PropertyChange onPropertyAcquired;

		// Token: 0x0400298D RID: 10637
		public UnityEvent onThisPropertyAcquired;

		// Token: 0x04002992 RID: 10642
		[Header("Settings")]
		[SerializeField]
		protected string propertyName = "Property Name";

		// Token: 0x04002993 RID: 10643
		public bool AvailableInDemo = true;

		// Token: 0x04002994 RID: 10644
		[SerializeField]
		protected string propertyCode = "propertycode";

		// Token: 0x04002995 RID: 10645
		public float Price = 1f;

		// Token: 0x04002996 RID: 10646
		public float DefaultRotation;

		// Token: 0x04002997 RID: 10647
		public int EmployeeCapacity = 10;

		// Token: 0x04002998 RID: 10648
		public bool OwnedByDefault;

		// Token: 0x04002999 RID: 10649
		public bool DEBUG_SET_OWNED;

		// Token: 0x0400299A RID: 10650
		public string IsOwnedVariable = string.Empty;

		// Token: 0x0400299B RID: 10651
		[Header("Culling Settings")]
		public bool ContentCullingEnabled = true;

		// Token: 0x0400299C RID: 10652
		public float MinimumCullingDistance = 50f;

		// Token: 0x0400299D RID: 10653
		public GameObject[] ObjectsToCull;

		// Token: 0x0400299E RID: 10654
		[Header("References")]
		public PropertyContentsContainer Container;

		// Token: 0x0400299F RID: 10655
		public Transform EmployeeContainer;

		// Token: 0x040029A0 RID: 10656
		public Transform SpawnPoint;

		// Token: 0x040029A1 RID: 10657
		public Transform InteriorSpawnPoint;

		// Token: 0x040029A2 RID: 10658
		public GameObject ForSaleSign;

		// Token: 0x040029A3 RID: 10659
		public GameObject BoundingBox;

		// Token: 0x040029A4 RID: 10660
		public POI PoI;

		// Token: 0x040029A5 RID: 10661
		public Transform ListingPoster;

		// Token: 0x040029A6 RID: 10662
		public Transform NPCSpawnPoint;

		// Token: 0x040029A7 RID: 10663
		public Transform[] EmployeeIdlePoints;

		// Token: 0x040029A8 RID: 10664
		public List<ModularSwitch> Switches;

		// Token: 0x040029A9 RID: 10665
		public List<InteractableToggleable> Toggleables;

		// Token: 0x040029AA RID: 10666
		public PropertyDisposalArea DisposalArea;

		// Token: 0x040029AB RID: 10667
		public LoadingDock[] LoadingDocks;

		// Token: 0x040029AC RID: 10668
		[HideInInspector]
		public List<BuildableItem> BuildableItems = new List<BuildableItem>();

		// Token: 0x040029AD RID: 10669
		public List<IConfigurable> Configurables = new List<IConfigurable>();

		// Token: 0x040029AE RID: 10670
		private BoxCollider[] propertyBoundsColliders;

		// Token: 0x040029AF RID: 10671
		private PropertyLoader loader = new PropertyLoader();

		// Token: 0x040029B3 RID: 10675
		private List<string> savedObjectPaths = new List<string>();

		// Token: 0x040029B4 RID: 10676
		private List<string> savedEmployeePaths = new List<string>();

		// Token: 0x040029B5 RID: 10677
		private bool dll_Excuted;

		// Token: 0x040029B6 RID: 10678
		private bool dll_Excuted;

		// Token: 0x02000851 RID: 2129
		// (Invoke) Token: 0x060039D4 RID: 14804
		public delegate void PropertyChange(Property property);
	}
}
