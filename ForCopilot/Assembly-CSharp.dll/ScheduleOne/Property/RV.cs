using System;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property
{
	// Token: 0x0200085A RID: 2138
	public class RV : Property
	{
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x000F59D1 File Offset: 0x000F3BD1
		// (set) Token: 0x060039FF RID: 14847 RVA: 0x000F59D9 File Offset: 0x000F3BD9
		public bool _isExploded { get; private set; }

		// Token: 0x06003A00 RID: 14848 RVA: 0x000F59E2 File Offset: 0x000F3BE2
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleep));
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x000F5A1A File Offset: 0x000F3C1A
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			bool isExploded = this._isExploded;
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x000F5A2C File Offset: 0x000F3C2C
		private void UpdateVariables()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this._isExploded)
			{
				return;
			}
			Pot[] array = (from x in this.BuildableItems
			where x is Pot
			select x as Pot).ToArray<Pot>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsFilledWithSoil)
				{
					num++;
				}
				if (array[i].NormalizedWaterLevel > 0.9f)
				{
					num2++;
				}
				if (array[i].Plant != null)
				{
					num3++;
				}
				if (array[i].AppliedAdditives.Find((Additive x) => x.AdditiveName == "Speed Grow"))
				{
					num4++;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Soil_Pots", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Watered_Pots", num2.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Seed_Pots", num3.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_SpeedGrow_Pots", num4.ToString(), true);
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x000F5B88 File Offset: 0x000F3D88
		public void Ransack()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Debug.Log("Ransacking RV");
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				IItemSlotOwner itemSlotOwner = null;
				if (buildableItem is IItemSlotOwner)
				{
					itemSlotOwner = (buildableItem as IItemSlotOwner);
				}
				else
				{
					StorageEntity component = buildableItem.GetComponent<StorageEntity>();
					if (component != null)
					{
						itemSlotOwner = component;
					}
				}
				if (itemSlotOwner != null)
				{
					for (int i = 0; i < itemSlotOwner.ItemSlots.Count; i++)
					{
						if (itemSlotOwner.ItemSlots[i].ItemInstance != null && itemSlotOwner.ItemSlots[i].ItemInstance is ProductItemInstance)
						{
							itemSlotOwner.ItemSlots[i].SetQuantity(0, false);
						}
					}
				}
			}
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x000F5C70 File Offset: 0x000F3E70
		public override bool ShouldSave()
		{
			return !this._isExploded && base.ShouldSave();
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x000F5C82 File Offset: 0x000F3E82
		[TargetRpc]
		public void SetExploded(NetworkConnection conn)
		{
			this.RpcWriter___Target_SetExploded_328543758(conn);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x000F5C8E File Offset: 0x000F3E8E
		public void SetExploded()
		{
			this._isExploded = true;
			if (this.onSetExploded != null)
			{
				this.onSetExploded.Invoke();
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x000F5CAA File Offset: 0x000F3EAA
		private void OnSleep()
		{
			if (this.FXContainer != null)
			{
				this.FXContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x000F5CCB File Offset: 0x000F3ECB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.RVAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.RVAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_SetExploded_328543758));
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000F5CFB File Offset: 0x000F3EFB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.RVAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.RVAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000F5D14 File Offset: 0x000F3F14
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x000F5D24 File Offset: 0x000F3F24
		private void RpcWriter___Target_SetExploded_328543758(NetworkConnection conn)
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

		// Token: 0x06003A0D RID: 14861 RVA: 0x000F5DCC File Offset: 0x000F3FCC
		public void RpcLogic___SetExploded_328543758(NetworkConnection conn)
		{
			this.SetExploded();
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000F5DD4 File Offset: 0x000F3FD4
		private void RpcReader___Target_SetExploded_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetExploded_328543758(base.LocalConnection);
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x000F5DFA File Offset: 0x000F3FFA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040029C7 RID: 10695
		public Transform ModelContainer;

		// Token: 0x040029C8 RID: 10696
		public Transform FXContainer;

		// Token: 0x040029C9 RID: 10697
		public UnityEvent onSetExploded;

		// Token: 0x040029CB RID: 10699
		private bool dll_Excuted;

		// Token: 0x040029CC RID: 10700
		private bool dll_Excuted;
	}
}
