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
	// Token: 0x0200054F RID: 1359
	public class StartMixingStationBehaviour : Behaviour
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00083341 File Offset: 0x00081541
		// (set) Token: 0x06001FDF RID: 8159 RVA: 0x00083349 File Offset: 0x00081549
		public MixingStation targetStation { get; private set; }

		// Token: 0x06001FE0 RID: 8160 RVA: 0x00083352 File Offset: 0x00081552
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartMixingStationBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00083366 File Offset: 0x00081566
		public void AssignStation(MixingStation station)
		{
			this.targetStation = station;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0008336F File Offset: 0x0008156F
		protected override void End()
		{
			base.End();
			if (this.startRoutine != null)
			{
				this.StopCook();
			}
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
			this.Disable();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000833A5 File Offset: 0x000815A5
		protected override void Pause()
		{
			base.Pause();
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000833C8 File Offset: 0x000815C8
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.startRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetStation.UIPoint.position, 5, false);
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

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00083441 File Offset: 0x00081641
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00083450 File Offset: 0x00081650
		private bool CanCookStart()
		{
			if (this.targetStation == null)
			{
				return false;
			}
			if (((IUsable)this.targetStation).IsInUse && ((IUsable)this.targetStation).NPCUserObject != base.Npc.NetworkObject)
			{
				return false;
			}
			MixingStationConfiguration mixingStationConfiguration = this.targetStation.Configuration as MixingStationConfiguration;
			return (float)this.targetStation.GetMixQuantity() >= mixingStationConfiguration.StartThrehold.Value;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000834C8 File Offset: 0x000816C8
		private void StopCook()
		{
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", false);
			if (this.startRoutine != null)
			{
				base.StopCoroutine(this.startRoutine);
				this.startRoutine = null;
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0008351C File Offset: 0x0008171C
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetStation == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetStation).AccessPoints[0].position;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0008354F File Offset: 0x0008174F
		private bool IsAtStation()
		{
			return !(this.targetStation == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x00083583 File Offset: 0x00081783
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|12_0()
		{
			base.Npc.Movement.FacePoint(this.targetStation.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetStation.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", true);
			QualityItemInstance product = this.targetStation.ProductSlot.ItemInstance as QualityItemInstance;
			ItemInstance mixer = this.targetStation.MixerSlot.ItemInstance;
			int mixQuantity = this.targetStation.GetMixQuantity();
			int num;
			for (int i = 0; i < mixQuantity; i = num + 1)
			{
				yield return new WaitForSeconds(1f);
				num = i;
			}
			if (InstanceFinder.IsServer)
			{
				this.targetStation.ProductSlot.ChangeQuantity(-mixQuantity, false);
				this.targetStation.MixerSlot.ChangeQuantity(-mixQuantity, false);
				MixOperation operation = new MixOperation(product.ID, product.Quality, mixer.ID, mixQuantity);
				this.targetStation.SendMixingOperation(operation, 0);
			}
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00083592 File Offset: 0x00081792
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000835C2 File Offset: 0x000817C2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000835DB File Offset: 0x000817DB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000835EC File Offset: 0x000817EC
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

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00083695 File Offset: 0x00081895
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.startRoutine != null)
			{
				return;
			}
			if (this.targetStation == null)
			{
				return;
			}
			this.startRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|12_0());
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000836C4 File Offset: 0x000818C4
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

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000836EE File Offset: 0x000818EE
		protected override void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x040018D1 RID: 6353
		public const float INSERT_INGREDIENT_BASE_TIME = 1f;

		// Token: 0x040018D3 RID: 6355
		private Chemist chemist;

		// Token: 0x040018D4 RID: 6356
		private Coroutine startRoutine;

		// Token: 0x040018D5 RID: 6357
		private bool dll_Excuted;

		// Token: 0x040018D6 RID: 6358
		private bool dll_Excuted;
	}
}
