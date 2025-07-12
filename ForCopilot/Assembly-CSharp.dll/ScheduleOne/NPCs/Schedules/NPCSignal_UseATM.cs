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
using ScheduleOne.Money;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004BC RID: 1212
	public class NPCSignal_UseATM : NPCSignal
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x00072C58 File Offset: 0x00070E58
		public new string ActionName
		{
			get
			{
				return "Use ATM";
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00072C5F File Offset: 0x00070E5F
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x00072C67 File Offset: 0x00070E67
		public override void Started()
		{
			base.Started();
			if (this.ATM == null)
			{
				Debug.LogWarning("No ATM found for NPC to use");
				this.End();
				return;
			}
			base.SetDestination(this.ATM.AccessPoint.position, true);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00072CA8 File Offset: 0x00070EA8
		public override void ActiveMinPassed()
		{
			base.MinPassed();
			if (this.ATM == null)
			{
				this.End();
				return;
			}
			if (this.purchaseCoroutine != null)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
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
					Debug.DrawLine(this.npc.Movement.FootPosition, this.ATM.AccessPoint.position, Color.red, 1f);
					base.SetDestination(this.ATM.AccessPoint.position, true);
				}
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00072D48 File Offset: 0x00070F48
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00072D50 File Offset: 0x00070F50
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

		// Token: 0x06001A59 RID: 6745 RVA: 0x00072DA4 File Offset: 0x00070FA4
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00071594 File Offset: 0x0006F794
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00072DAC File Offset: 0x00070FAC
		private bool IsAtDestination()
		{
			return !(this.ATM == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.ATM.AccessPoint.position) < 2f;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00072DEA File Offset: 0x00070FEA
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

		// Token: 0x06001A5D RID: 6749 RVA: 0x00072E08 File Offset: 0x00071008
		[ObserversRpc(RunLocally = true)]
		public void Purchase()
		{
			this.RpcWriter___Observers_Purchase_2166136261();
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00072E21 File Offset: 0x00071021
		[CompilerGenerated]
		private IEnumerator <Purchase>g__Purchase|14_0()
		{
			if (this.ATM.IsBroken)
			{
				this.End();
				this.purchaseCoroutine = null;
				yield break;
			}
			yield return new WaitForSeconds(2f);
			this.npc.SetAnimationTrigger_Networked(null, "GrabItem");
			yield return new WaitForSeconds(1f);
			this.End();
			this.purchaseCoroutine = null;
			yield break;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00072E30 File Offset: 0x00071030
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Purchase_2166136261));
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00072E60 File Offset: 0x00071060
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00072E79 File Offset: 0x00071079
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00072E88 File Offset: 0x00071088
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

		// Token: 0x06001A64 RID: 6756 RVA: 0x00072F34 File Offset: 0x00071134
		public void RpcLogic___Purchase_2166136261()
		{
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
			}
			this.npc.Movement.FaceDirection(this.ATM.AccessPoint.forward, 0.5f);
			this.purchaseCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Purchase>g__Purchase|14_0());
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00072F94 File Offset: 0x00071194
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

		// Token: 0x06001A66 RID: 6758 RVA: 0x00072FBE File Offset: 0x000711BE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400168F RID: 5775
		private const float destinationThreshold = 2f;

		// Token: 0x04001690 RID: 5776
		public ATM ATM;

		// Token: 0x04001691 RID: 5777
		private Coroutine purchaseCoroutine;

		// Token: 0x04001692 RID: 5778
		private bool dll_Excuted;

		// Token: 0x04001693 RID: 5779
		private bool dll_Excuted;
	}
}
