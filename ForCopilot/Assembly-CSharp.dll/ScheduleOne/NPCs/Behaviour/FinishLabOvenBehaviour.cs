using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000533 RID: 1331
	public class FinishLabOvenBehaviour : Behaviour
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x0007DCB8 File Offset: 0x0007BEB8
		// (set) Token: 0x06001E77 RID: 7799 RVA: 0x0007DCC0 File Offset: 0x0007BEC0
		public LabOven targetOven { get; private set; }

		// Token: 0x06001E78 RID: 7800 RVA: 0x0007DCC9 File Offset: 0x0007BEC9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0007DCDD File Offset: 0x0007BEDD
		public void SetTargetOven(LabOven oven)
		{
			this.targetOven = oven;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0007DCE8 File Offset: 0x0007BEE8
		protected override void End()
		{
			base.End();
			if (this.targetOven != null)
			{
				this.targetOven.Door.SetPosition(0f);
				this.targetOven.ClearShards();
				this.targetOven.RemoveTrayAnimation.Stop();
				this.targetOven.ResetSquareTray();
			}
			this.Disable();
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0007DD4C File Offset: 0x0007BF4C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.actionRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetOven.UIPoint.position, 5, false);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (this.IsAtStation())
				{
					this.StartAction();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0007DDC5 File Offset: 0x0007BFC5
		[ObserversRpc(RunLocally = true)]
		private void StartAction()
		{
			this.RpcWriter___Observers_StartAction_2166136261();
			this.RpcLogic___StartAction_2166136261();
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0007DDD4 File Offset: 0x0007BFD4
		private bool CanActionStart()
		{
			return !(this.targetOven == null) && (!((IUsable)this.targetOven).IsInUse || !(((IUsable)this.targetOven).NPCUserObject != base.Npc.NetworkObject)) && this.targetOven.CurrentOperation != null && this.targetOven.CurrentOperation.IsReady() && this.targetOven.CanOutputSpaceFitCurrentOperation();
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0007DE50 File Offset: 0x0007C050
		private void StopAction()
		{
			this.targetOven.SetNPCUser(null);
			base.Npc.SetEquippable_Networked(null, string.Empty);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", false);
			if (this.actionRoutine != null)
			{
				base.StopCoroutine(this.actionRoutine);
				this.actionRoutine = null;
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0007DEA7 File Offset: 0x0007C0A7
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetOven == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetOven).AccessPoints[0].position;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0007DEDA File Offset: 0x0007C0DA
		private bool IsAtStation()
		{
			return !(this.targetOven == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0007DF0E File Offset: 0x0007C10E
		[CompilerGenerated]
		private IEnumerator <StartAction>g__ActionRoutine|11_0()
		{
			this.targetOven.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.Movement.FacePoint(this.targetOven.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanActionStart())
			{
				this.StopAction();
				base.End_Networked(null);
				yield break;
			}
			base.Npc.SetEquippable_Networked(null, "Avatar/Equippables/Hammer");
			this.targetOven.Door.SetPosition(1f);
			this.targetOven.WireTray.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.targetOven.SquareTray.SetParent(this.targetOven.transform);
			this.targetOven.RemoveTrayAnimation.Play();
			yield return new WaitForSeconds(0.1f);
			this.targetOven.Door.SetPosition(0f);
			yield return new WaitForSeconds(1f);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", true);
			yield return new WaitForSeconds(10f);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", false);
			this.targetOven.Shatter(this.targetOven.CurrentOperation.Cookable.ProductQuantity, this.targetOven.CurrentOperation.Cookable.ProductShardPrefab.gameObject);
			yield return new WaitForSeconds(1f);
			ItemInstance productItem = this.targetOven.CurrentOperation.GetProductItem(this.targetOven.CurrentOperation.Cookable.ProductQuantity * this.targetOven.CurrentOperation.IngredientQuantity);
			this.targetOven.OutputSlot.AddItem(productItem, false);
			this.targetOven.SendCookOperation(null);
			this.StopAction();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0007DF1D File Offset: 0x0007C11D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_2166136261));
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0007DF4D File Offset: 0x0007C14D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0007DF66 File Offset: 0x0007C166
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0007DF74 File Offset: 0x0007C174
		private void RpcWriter___Observers_StartAction_2166136261()
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
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0007E01D File Offset: 0x0007C21D
		private void RpcLogic___StartAction_2166136261()
		{
			if (this.actionRoutine != null)
			{
				return;
			}
			if (this.targetOven == null)
			{
				return;
			}
			this.actionRoutine = base.StartCoroutine(this.<StartAction>g__ActionRoutine|11_0());
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0007E04C File Offset: 0x0007C24C
		private void RpcReader___Observers_StartAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_2166136261();
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0007E076 File Offset: 0x0007C276
		protected override void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x0400183B RID: 6203
		public const float HARVEST_TIME = 10f;

		// Token: 0x0400183D RID: 6205
		private Chemist chemist;

		// Token: 0x0400183E RID: 6206
		private Coroutine actionRoutine;

		// Token: 0x0400183F RID: 6207
		private bool dll_Excuted;

		// Token: 0x04001840 RID: 6208
		private bool dll_Excuted;
	}
}
