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
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200054D RID: 1357
	public class StartLabOvenBehaviour : Behaviour
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x00082D80 File Offset: 0x00080F80
		// (set) Token: 0x06001FC5 RID: 8133 RVA: 0x00082D88 File Offset: 0x00080F88
		public LabOven targetOven { get; private set; }

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00082D91 File Offset: 0x00080F91
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartLabOvenBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00082DA5 File Offset: 0x00080FA5
		public void SetTargetOven(LabOven oven)
		{
			this.targetOven = oven;
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00082DAE File Offset: 0x00080FAE
		protected override void End()
		{
			base.End();
			if (this.targetOven != null)
			{
				this.targetOven.Door.SetPosition(0f);
			}
			if (this.cookRoutine != null)
			{
				this.StopCook();
			}
			this.Disable();
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x00082DF0 File Offset: 0x00080FF0
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.cookRoutine != null)
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
					this.StartCook();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x00082E69 File Offset: 0x00081069
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00082E78 File Offset: 0x00081078
		private bool CanCookStart()
		{
			return !(this.targetOven == null) && (!((IUsable)this.targetOven).IsInUse || !(((IUsable)this.targetOven).NPCUserObject != base.Npc.NetworkObject)) && this.targetOven.CurrentOperation == null && this.targetOven.IsIngredientCookable();
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x00082EE0 File Offset: 0x000810E0
		private void StopCook()
		{
			if (this.targetOven != null)
			{
				this.targetOven.SetNPCUser(null);
			}
			if (this.cookRoutine != null)
			{
				base.StopCoroutine(this.cookRoutine);
				this.cookRoutine = null;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x00082F17 File Offset: 0x00081117
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetOven == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetOven).AccessPoints[0].position;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x00082F4A File Offset: 0x0008114A
		private bool IsAtStation()
		{
			return !(this.targetOven == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x00082F7E File Offset: 0x0008117E
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|11_0()
		{
			Console.Log("Starting cook...", null);
			this.targetOven.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.Movement.FacePoint(this.targetOven.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetOven.Door.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.targetOven.WireTray.SetPosition(1f);
			yield return new WaitForSeconds(5f);
			this.targetOven.Door.SetPosition(0f);
			yield return new WaitForSeconds(1f);
			ItemInstance itemInstance = this.targetOven.IngredientSlot.ItemInstance;
			if (itemInstance == null)
			{
				Console.LogWarning("No ingredient in oven!", null);
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			int num = 1;
			if ((itemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Solid)
			{
				num = Mathf.Min(this.targetOven.IngredientSlot.Quantity, 10);
			}
			itemInstance.ChangeQuantity(-num);
			string id = (itemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (itemInstance is QualityItemInstance)
			{
				ingredientQuality = (itemInstance as QualityItemInstance).Quality;
			}
			this.targetOven.SendCookOperation(new OvenCookOperation(itemInstance.ID, ingredientQuality, num, id));
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00082F8D File Offset: 0x0008118D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00082FBD File Offset: 0x000811BD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x00082FD6 File Offset: 0x000811D6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x00082FE4 File Offset: 0x000811E4
		private void RpcWriter___Observers_StartCook_2166136261()
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

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0008308D File Offset: 0x0008128D
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.cookRoutine != null)
			{
				return;
			}
			if (this.targetOven == null)
			{
				return;
			}
			this.cookRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|11_0());
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000830BC File Offset: 0x000812BC
		private void RpcReader___Observers_StartCook_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000830E6 File Offset: 0x000812E6
		protected override void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x040018C8 RID: 6344
		public const float POUR_TIME = 5f;

		// Token: 0x040018CA RID: 6346
		private Chemist chemist;

		// Token: 0x040018CB RID: 6347
		private Coroutine cookRoutine;

		// Token: 0x040018CC RID: 6348
		private bool dll_Excuted;

		// Token: 0x040018CD RID: 6349
		private bool dll_Excuted;
	}
}
