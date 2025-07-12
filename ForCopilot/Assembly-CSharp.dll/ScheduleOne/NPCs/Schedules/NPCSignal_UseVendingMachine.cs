using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004BE RID: 1214
	public class NPCSignal_UseVendingMachine : NPCSignal
	{
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x0007309D File Offset: 0x0007129D
		public new string ActionName
		{
			get
			{
				return "Use Vending Machine";
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000730A4 File Offset: 0x000712A4
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x000730AC File Offset: 0x000712AC
		public override void Started()
		{
			base.Started();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.TargetMachine = this.GetTargetMachine();
			if (this.TargetMachine == null)
			{
				Debug.LogWarning("No vending machine found for NPC to use");
				this.End();
				return;
			}
			base.SetDestination(this.TargetMachine.AccessPoint.position, true);
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0007310C File Offset: 0x0007130C
		public override void MinPassed()
		{
			base.MinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.TargetMachine == null)
				{
					this.TargetMachine = this.GetTargetMachine();
				}
				if (this.TargetMachine == null)
				{
					Debug.LogWarning("No vending machine found for NPC to use");
					this.End();
					return;
				}
				if (this.TargetMachine.AccessPoint == null)
				{
					Debug.LogWarning("Vending machine has no access point");
					this.End();
					return;
				}
				if (this.IsAtDestination())
				{
					if (this.purchaseCoroutine == null)
					{
						this.Purchase();
						return;
					}
				}
				else
				{
					if (this.npc.Movement.CanGetTo(this.TargetMachine.AccessPoint.position, 1f))
					{
						base.SetDestination(this.TargetMachine.AccessPoint.position, true);
						return;
					}
					Debug.LogWarning("Unable to reach vending machine");
					this.End();
					return;
				}
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00072D48 File Offset: 0x00070F48
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00073204 File Offset: 0x00071404
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
				this.purchaseCoroutine = null;
			}
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00072DA4 File Offset: 0x00070FA4
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00071594 File Offset: 0x0006F794
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00073258 File Offset: 0x00071458
		private bool IsAtDestination()
		{
			return !(this.TargetMachine == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.TargetMachine.AccessPoint.position) < 1f;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00073298 File Offset: 0x00071498
		private VendingMachine GetTargetMachine()
		{
			if (this.MachineOverride != null && base.movement.CanGetTo(this.MachineOverride.AccessPoint.position, 1f))
			{
				return this.MachineOverride;
			}
			VendingMachine result = null;
			float num = float.MaxValue;
			foreach (VendingMachine vendingMachine in VendingMachine.AllMachines)
			{
				if (base.movement.CanGetTo(vendingMachine.AccessPoint.position, 1f))
				{
					float num2 = Vector3.Distance(this.npc.Movement.FootPosition, vendingMachine.AccessPoint.position);
					if (num2 < num)
					{
						result = vendingMachine;
						num = num2;
					}
				}
			}
			return result;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0007336C File Offset: 0x0007156C
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			this.Purchase();
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0007338C File Offset: 0x0007158C
		[ObserversRpc(RunLocally = true)]
		public void Purchase()
		{
			this.RpcWriter___Observers_Purchase_2166136261();
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000733A5 File Offset: 0x000715A5
		private bool CheckItem()
		{
			if (this.TargetMachine.lastDroppedItem == null || this.TargetMachine.lastDroppedItem.gameObject == null)
			{
				this.ItemWasStolen();
				this.End();
				return false;
			}
			return true;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000733E1 File Offset: 0x000715E1
		private void ItemWasStolen()
		{
			this.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "drinkstolen", 20f, 0);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00073408 File Offset: 0x00071608
		[CompilerGenerated]
		private IEnumerator <Purchase>g__Purchase|16_0()
		{
			yield return new WaitForSeconds(1f);
			if (this.TargetMachine == null || this.TargetMachine.IsBroken)
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.TargetMachine.PurchaseRoutine();
			yield return new WaitForSeconds(1f);
			if (!this.CheckItem())
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.npc.SetAnimationTrigger_Networked(null, "GrabItem");
			yield return new WaitForSeconds(0.4f);
			if (!this.CheckItem())
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.TargetMachine.RemoveLastDropped();
			yield return new WaitForSeconds(0.5f);
			this.End();
			this.purchaseCoroutine = null;
			this.npc.Avatar.EmotionManager.AddEmotionOverride("Cheery", "energydrink", 5f, 0);
			yield break;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00073417 File Offset: 0x00071617
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Purchase_2166136261));
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00073447 File Offset: 0x00071647
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00073460 File Offset: 0x00071660
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00073470 File Offset: 0x00071670
		private void RpcWriter___Observers_Purchase_2166136261()
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
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0007351C File Offset: 0x0007171C
		public void RpcLogic___Purchase_2166136261()
		{
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
			}
			if (this.TargetMachine == null)
			{
				this.TargetMachine = this.GetTargetMachine();
			}
			if (this.TargetMachine != null)
			{
				this.npc.Movement.FaceDirection(this.TargetMachine.AccessPoint.forward, 0.5f);
			}
			this.purchaseCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Purchase>g__Purchase|16_0());
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000735A4 File Offset: 0x000717A4
		private void RpcReader___Observers_Purchase_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000735CE File Offset: 0x000717CE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001697 RID: 5783
		private const float destinationThreshold = 1f;

		// Token: 0x04001698 RID: 5784
		public VendingMachine MachineOverride;

		// Token: 0x04001699 RID: 5785
		private VendingMachine TargetMachine;

		// Token: 0x0400169A RID: 5786
		private Coroutine purchaseCoroutine;

		// Token: 0x0400169B RID: 5787
		private bool dll_Excuted;

		// Token: 0x0400169C RID: 5788
		private bool dll_Excuted;
	}
}
