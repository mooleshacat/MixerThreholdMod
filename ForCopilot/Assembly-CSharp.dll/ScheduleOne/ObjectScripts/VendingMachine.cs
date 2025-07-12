using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BE8 RID: 3048
	public class VendingMachine : NetworkBehaviour, IGUIDRegisterable, IGenericSaveable
	{
		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x001579BC File Offset: 0x00155BBC
		// (set) Token: 0x0600513F RID: 20799 RVA: 0x001579C4 File Offset: 0x00155BC4
		public bool IsBroken { get; protected set; }

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06005140 RID: 20800 RVA: 0x001579CD File Offset: 0x00155BCD
		// (set) Token: 0x06005141 RID: 20801 RVA: 0x001579D5 File Offset: 0x00155BD5
		public int DaysUntilRepair { get; protected set; }

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x001579DE File Offset: 0x00155BDE
		// (set) Token: 0x06005143 RID: 20803 RVA: 0x001579E6 File Offset: 0x00155BE6
		public ItemPickup lastDroppedItem { get; protected set; }

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x001579EF File Offset: 0x00155BEF
		// (set) Token: 0x06005145 RID: 20805 RVA: 0x001579F7 File Offset: 0x00155BF7
		public Guid GUID { get; protected set; }

		// Token: 0x06005146 RID: 20806 RVA: 0x00157A00 File Offset: 0x00155C00
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00157A28 File Offset: 0x00155C28
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.VendingMachine_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x00157A48 File Offset: 0x00155C48
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.DayPass));
			this.SetLit(false);
			((IGenericSaveable)this).InitializeSaveable();
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x00157ABE File Offset: 0x00155CBE
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsBroken)
			{
				this.Break(connection);
			}
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x00157AD6 File Offset: 0x00155CD6
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x00157AE5 File Offset: 0x00155CE5
		private void OnDestroy()
		{
			if (VendingMachine.AllMachines.Contains(this))
			{
				VendingMachine.AllMachines.Remove(this);
			}
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x00157B24 File Offset: 0x00155D24
		private void MinPass()
		{
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.LitStartTime, this.LitOnEndTime) && !this.IsBroken)
			{
				if (!this.isLit)
				{
					this.SetLit(true);
					return;
				}
			}
			else if (this.isLit)
			{
				this.SetLit(false);
			}
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00157B70 File Offset: 0x00155D70
		public void DayPass()
		{
			if (this.IsBroken)
			{
				int daysUntilRepair = this.DaysUntilRepair;
				this.DaysUntilRepair = daysUntilRepair - 1;
				if (this.DaysUntilRepair <= 0)
				{
					this.Repair();
				}
			}
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x00157BA4 File Offset: 0x00155DA4
		public void Hovered()
		{
			if (this.purchaseInProgress)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.IsBroken)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= 2f)
			{
				this.IntObj.SetMessage("Purchase Cuke");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Not enough cash");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x00157C25 File Offset: 0x00155E25
		public void Interacted()
		{
			if (this.purchaseInProgress)
			{
				return;
			}
			if (this.IsBroken)
			{
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= 2f)
			{
				this.LocalPurchase();
			}
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00157C50 File Offset: 0x00155E50
		private void LocalPurchase()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-2f, true, false);
			this.SendPurchase();
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x00157C69 File Offset: 0x00155E69
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPurchase()
		{
			this.RpcWriter___Server_SendPurchase_2166136261();
			this.RpcLogic___SendPurchase_2166136261();
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x00157C77 File Offset: 0x00155E77
		[ObserversRpc(RunLocally = true)]
		public void PurchaseRoutine()
		{
			this.RpcWriter___Observers_PurchaseRoutine_2166136261();
			this.RpcLogic___PurchaseRoutine_2166136261();
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x00157C88 File Offset: 0x00155E88
		[ServerRpc(RequireOwnership = false)]
		public void DropItem()
		{
			this.RpcWriter___Server_DropItem_2166136261();
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00157C9B File Offset: 0x00155E9B
		public void RemoveLastDropped()
		{
			if (this.lastDroppedItem != null && this.lastDroppedItem.gameObject != null)
			{
				this.lastDroppedItem.Destroy();
				this.lastDroppedItem = null;
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00157CD0 File Offset: 0x00155ED0
		private void Impacted(Impact impact)
		{
			if (impact.ImpactForce < 50f)
			{
				return;
			}
			if (this.IsBroken)
			{
				return;
			}
			if (impact.ImpactForce >= 165f)
			{
				this.SendBreak();
				if (impact.ImpactSource == Player.Local.NetworkObject)
				{
					Player.Local.VisualState.ApplyState("vandalism", PlayerVisualState.EVisualState.Vandalizing, 0f);
					Player.Local.VisualState.RemoveState("vandalism", 2f);
				}
				base.StartCoroutine(this.<Impacted>g__BreakRoutine|64_0());
				return;
			}
			if (UnityEngine.Random.value < 0.33f && Time.time - this.timeOnLastFreeItem > 10f)
			{
				this.timeOnLastFreeItem = Time.time;
				base.StartCoroutine(this.<Impacted>g__Drop|64_1());
			}
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00157D98 File Offset: 0x00155F98
		private void SetLit(bool lit)
		{
			this.isLit = lit;
			if (this.isLit)
			{
				Material[] materials = this.DoorMesh.materials;
				materials[1] = this.DoorOnMat;
				this.DoorMesh.materials = materials;
				Material[] materials2 = this.BodyMesh.materials;
				materials2[1] = this.BodyOnMat;
				this.BodyMesh.materials = materials2;
			}
			else
			{
				Material[] materials3 = this.DoorMesh.materials;
				materials3[1] = this.DoorOffMat;
				this.DoorMesh.materials = materials3;
				Material[] materials4 = this.BodyMesh.materials;
				materials4[1] = this.BodyOffMat;
				this.BodyMesh.materials = materials4;
			}
			for (int i = 0; i < this.Lights.Length; i++)
			{
				this.Lights[i].Enabled = this.isLit;
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00157E65 File Offset: 0x00156065
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBreak()
		{
			this.RpcWriter___Server_SendBreak_2166136261();
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00157E73 File Offset: 0x00156073
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void Break(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Break_328543758(conn);
				this.RpcLogic___Break_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Break_328543758(conn);
			}
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x00157E9D File Offset: 0x0015609D
		[ObserversRpc]
		private void Repair()
		{
			this.RpcWriter___Observers_Repair_2166136261();
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x00157EA8 File Offset: 0x001560A8
		[ServerRpc(RequireOwnership = false)]
		private void DropCash()
		{
			this.RpcWriter___Server_DropCash_2166136261();
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00157EBC File Offset: 0x001560BC
		public void Load(GenericSaveData data)
		{
			bool @bool = data.GetBool("broken", false);
			if (@bool)
			{
				this.Break(null);
			}
			this.IsBroken = @bool;
			this.DaysUntilRepair = data.GetInt("daysUntilRepair", 0);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x00157EFC File Offset: 0x001560FC
		public GenericSaveData GetSaveData()
		{
			GenericSaveData genericSaveData = new GenericSaveData(this.GUID.ToString());
			genericSaveData.Add("broken", this.IsBroken);
			genericSaveData.Add("daysUntilRepair", this.DaysUntilRepair);
			return genericSaveData;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x00157F79 File Offset: 0x00156179
		[CompilerGenerated]
		private IEnumerator <PurchaseRoutine>g__Routine|61_0()
		{
			this.PaySound.Play();
			this.DispenseSound.Play();
			this.Anim.Play();
			yield return new WaitForSeconds(0.65f);
			if (base.IsServer)
			{
				this.DropItem();
			}
			this.purchaseInProgress = false;
			yield break;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00157F88 File Offset: 0x00156188
		[CompilerGenerated]
		private IEnumerator <Impacted>g__BreakRoutine|64_0()
		{
			int cashDrop = UnityEngine.Random.Range(1, 5);
			int num;
			for (int i = 0; i < cashDrop; i = num + 1)
			{
				this.DropCash();
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield break;
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x00157F97 File Offset: 0x00156197
		[CompilerGenerated]
		private IEnumerator <Impacted>g__Drop|64_1()
		{
			this.DispenseSound.Play();
			yield return new WaitForSeconds(0.65f);
			this.DropItem();
			yield break;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x00157FA8 File Offset: 0x001561A8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendPurchase_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_PurchaseRoutine_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_DropItem_2166136261));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendBreak_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Break_328543758));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_Break_328543758));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_Repair_2166136261));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_DropCash_2166136261));
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x0015807E File Offset: 0x0015627E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.VendingMachineAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x00158091 File Offset: 0x00156291
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001580A0 File Offset: 0x001562A0
		private void RpcWriter___Server_SendPurchase_2166136261()
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

		// Token: 0x06005166 RID: 20838 RVA: 0x0015813A File Offset: 0x0015633A
		public void RpcLogic___SendPurchase_2166136261()
		{
			this.PurchaseRoutine();
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x00158144 File Offset: 0x00156344
		private void RpcReader___Server_SendPurchase_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPurchase_2166136261();
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x00158174 File Offset: 0x00156374
		private void RpcWriter___Observers_PurchaseRoutine_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x0015821D File Offset: 0x0015641D
		public void RpcLogic___PurchaseRoutine_2166136261()
		{
			if (this.purchaseInProgress)
			{
				return;
			}
			this.purchaseInProgress = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<PurchaseRoutine>g__Routine|61_0());
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x00158240 File Offset: 0x00156440
		private void RpcReader___Observers_PurchaseRoutine_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PurchaseRoutine_2166136261();
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x0015826C File Offset: 0x0015646C
		private void RpcWriter___Server_DropItem_2166136261()
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
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x00158308 File Offset: 0x00156508
		public void RpcLogic___DropItem_2166136261()
		{
			ItemPickup itemPickup = UnityEngine.Object.Instantiate<ItemPickup>(this.CukePrefab, this.ItemSpawnPoint.position, this.ItemSpawnPoint.rotation);
			base.Spawn(itemPickup.gameObject, null, default(Scene));
			this.lastDroppedItem = itemPickup;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x00158354 File Offset: 0x00156554
		private void RpcReader___Server_DropItem_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropItem_2166136261();
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x00158374 File Offset: 0x00156574
		private void RpcWriter___Server_SendBreak_2166136261()
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
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0015840E File Offset: 0x0015660E
		private void RpcLogic___SendBreak_2166136261()
		{
			this.DaysUntilRepair = 0;
			this.Break(null);
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x00158420 File Offset: 0x00156620
		private void RpcReader___Server_SendBreak_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x00158450 File Offset: 0x00156650
		private void RpcWriter___Observers_Break_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x001584F9 File Offset: 0x001566F9
		private void RpcLogic___Break_328543758(NetworkConnection conn)
		{
			if (this.IsBroken)
			{
				return;
			}
			this.IsBroken = true;
			this.SetLit(false);
			UnityEvent unityEvent = this.onBreak;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x00158524 File Offset: 0x00156724
		private void RpcReader___Observers_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Break_328543758(null);
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x00158550 File Offset: 0x00156750
		private void RpcWriter___Target_Break_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(5U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x001585F8 File Offset: 0x001567F8
		private void RpcReader___Target_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Break_328543758(base.LocalConnection);
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x00158620 File Offset: 0x00156820
		private void RpcWriter___Observers_Repair_2166136261()
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x001586C9 File Offset: 0x001568C9
		private void RpcLogic___Repair_2166136261()
		{
			if (!this.IsBroken)
			{
				return;
			}
			Console.Log("Repairing...", null);
			this.IsBroken = false;
			UnityEvent unityEvent = this.onRepair;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001586F8 File Offset: 0x001568F8
		private void RpcReader___Observers_Repair_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Repair_2166136261();
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x00158718 File Offset: 0x00156918
		private void RpcWriter___Server_DropCash_2166136261()
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
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x001587B4 File Offset: 0x001569B4
		private void RpcLogic___DropCash_2166136261()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CashPrefab.gameObject, this.CashSpawnPoint.position, this.CashSpawnPoint.rotation);
			gameObject.GetComponent<Rigidbody>().AddForce(this.CashSpawnPoint.forward * UnityEngine.Random.Range(1.5f, 2.5f), 2);
			gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, 2);
			base.Spawn(gameObject.gameObject, null, default(Scene));
			this.PaySound.Play();
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x00158850 File Offset: 0x00156A50
		private void RpcReader___Server_DropCash_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropCash_2166136261();
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x00158870 File Offset: 0x00156A70
		private void dll()
		{
			if (!VendingMachine.AllMachines.Contains(this))
			{
				VendingMachine.AllMachines.Add(this);
			}
			PhysicsDamageable damageable = this.Damageable;
			damageable.onImpacted = (Action<Impact>)Delegate.Combine(damageable.onImpacted, new Action<Impact>(this.Impacted));
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x04003CF8 RID: 15608
		public static List<VendingMachine> AllMachines = new List<VendingMachine>();

		// Token: 0x04003CF9 RID: 15609
		public const float COST = 2f;

		// Token: 0x04003CFA RID: 15610
		public const int REPAIR_TIME_DAYS = 0;

		// Token: 0x04003CFB RID: 15611
		public const float IMPACT_THRESHOLD_FREE_ITEM = 50f;

		// Token: 0x04003CFC RID: 15612
		public const float IMPACT_THRESHOLD_FREE_ITEM_CHANCE = 0.33f;

		// Token: 0x04003CFD RID: 15613
		public const float IMPACT_THRESHOLD_BREAK = 165f;

		// Token: 0x04003CFE RID: 15614
		public const int MIN_CASH_DROP = 1;

		// Token: 0x04003CFF RID: 15615
		public const int MAX_CASH_DROP = 4;

		// Token: 0x04003D02 RID: 15618
		[Header("Settings")]
		public int LitStartTime = 1700;

		// Token: 0x04003D03 RID: 15619
		public int LitOnEndTime = 800;

		// Token: 0x04003D04 RID: 15620
		public ItemPickup CukePrefab;

		// Token: 0x04003D05 RID: 15621
		public CashPickup CashPrefab;

		// Token: 0x04003D06 RID: 15622
		[Header("References")]
		public MeshRenderer DoorMesh;

		// Token: 0x04003D07 RID: 15623
		public MeshRenderer BodyMesh;

		// Token: 0x04003D08 RID: 15624
		public Material DoorOffMat;

		// Token: 0x04003D09 RID: 15625
		public Material DoorOnMat;

		// Token: 0x04003D0A RID: 15626
		public Material BodyOffMat;

		// Token: 0x04003D0B RID: 15627
		public Material BodyOnMat;

		// Token: 0x04003D0C RID: 15628
		public OptimizedLight[] Lights;

		// Token: 0x04003D0D RID: 15629
		public AudioSourceController PaySound;

		// Token: 0x04003D0E RID: 15630
		public AudioSourceController DispenseSound;

		// Token: 0x04003D0F RID: 15631
		public Animation Anim;

		// Token: 0x04003D10 RID: 15632
		public Transform ItemSpawnPoint;

		// Token: 0x04003D11 RID: 15633
		public InteractableObject IntObj;

		// Token: 0x04003D12 RID: 15634
		public Transform AccessPoint;

		// Token: 0x04003D13 RID: 15635
		public PhysicsDamageable Damageable;

		// Token: 0x04003D14 RID: 15636
		public Transform CashSpawnPoint;

		// Token: 0x04003D15 RID: 15637
		public UnityEvent onBreak;

		// Token: 0x04003D16 RID: 15638
		public UnityEvent onRepair;

		// Token: 0x04003D18 RID: 15640
		private bool isLit;

		// Token: 0x04003D19 RID: 15641
		private bool purchaseInProgress;

		// Token: 0x04003D1A RID: 15642
		private float timeOnLastFreeItem;

		// Token: 0x04003D1C RID: 15644
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003D1D RID: 15645
		private bool dll_Excuted;

		// Token: 0x04003D1E RID: 15646
		private bool dll_Excuted;
	}
}
