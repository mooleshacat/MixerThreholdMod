using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200098A RID: 2442
	[RequireComponent(typeof(InteractableObject))]
	public class ItemPickup : NetworkBehaviour
	{
		// Token: 0x060041CB RID: 16843 RVA: 0x00115214 File Offset: 0x00113414
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ItemFramework.ItemPickup_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x00115233 File Offset: 0x00113433
		private void Start()
		{
			if (Player.Local != null)
			{
				this.Init();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Init));
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x00115269 File Offset: 0x00113469
		private void Init()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Init));
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Init>g__Wait|9_0());
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0011529C File Offset: 0x0011349C
		protected virtual void Hovered()
		{
			if (this.CanPickup())
			{
				this.IntObj.SetMessage("Pick up " + this.ItemToGive.Name);
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Inventory Full");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x001152FA File Offset: 0x001134FA
		private void Interacted()
		{
			if (this.CanPickup())
			{
				this.Pickup();
			}
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x0011530A File Offset: 0x0011350A
		protected virtual bool CanPickup()
		{
			return this.ItemToGive != null && PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.ItemToGive.GetDefaultInstance(1), 1);
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x00115334 File Offset: 0x00113534
		protected virtual void Pickup()
		{
			if (this.ItemToGive != null)
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemToGive.GetDefaultInstance(1));
			}
			if (this.onPickup != null)
			{
				this.onPickup.Invoke();
			}
			if (this.DestroyOnPickup)
			{
				if (this.Networked)
				{
					this.Destroy();
					return;
				}
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0011539C File Offset: 0x0011359C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void Destroy()
		{
			this.RpcWriter___Server_Destroy_2166136261();
			this.RpcLogic___Destroy_2166136261();
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x001153CB File Offset: 0x001135CB
		[CompilerGenerated]
		private IEnumerator <Init>g__Wait|9_0()
		{
			yield return new WaitUntil(() => Player.Local.playerDataRetrieveReturned);
			if (this.ConditionallyActive && this.ActiveCondition != null)
			{
				base.gameObject.SetActive(this.ActiveCondition.Evaluate());
			}
			yield break;
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x001153DA File Offset: 0x001135DA
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_2166136261));
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00115404 File Offset: 0x00113604
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ItemFramework.ItemPickupAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x00115417 File Offset: 0x00113617
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x00115428 File Offset: 0x00113628
		private void RpcWriter___Server_Destroy_2166136261()
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

		// Token: 0x060041D9 RID: 16857 RVA: 0x001154C4 File Offset: 0x001136C4
		public void RpcLogic___Destroy_2166136261()
		{
			if (base.IsServer)
			{
				base.NetworkObject.Despawn(null);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x001154F8 File Offset: 0x001136F8
		private void RpcReader___Server_Destroy_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___Destroy_2166136261();
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x00115528 File Offset: 0x00113728
		protected virtual void dll()
		{
			if (this.ItemToGive != null)
			{
				this.IntObj.SetMessage("Pick up " + this.ItemToGive.Name);
			}
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x04002EE8 RID: 12008
		public ItemDefinition ItemToGive;

		// Token: 0x04002EE9 RID: 12009
		public bool DestroyOnPickup = true;

		// Token: 0x04002EEA RID: 12010
		public bool ConditionallyActive;

		// Token: 0x04002EEB RID: 12011
		public Condition ActiveCondition;

		// Token: 0x04002EEC RID: 12012
		public bool Networked = true;

		// Token: 0x04002EED RID: 12013
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04002EEE RID: 12014
		public UnityEvent onPickup;

		// Token: 0x04002EEF RID: 12015
		private bool dll_Excuted;

		// Token: 0x04002EF0 RID: 12016
		private bool dll_Excuted;
	}
}
