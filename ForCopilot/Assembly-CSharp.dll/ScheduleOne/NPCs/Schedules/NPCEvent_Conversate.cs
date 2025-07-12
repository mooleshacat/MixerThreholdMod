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
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004AD RID: 1197
	public class NPCEvent_Conversate : NPCEvent
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x0006EF8B File Offset: 0x0006D18B
		public new string ActionName
		{
			get
			{
				return "Conversate";
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x0006EF92 File Offset: 0x0006D192
		private Transform StandPoint
		{
			get
			{
				return this.Location.GetStandPoint(this.npc);
			}
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0006EFA8 File Offset: 0x0006D1A8
		public override string GetName()
		{
			if (this.Location == null)
			{
				return this.ActionName + " (No destination set)";
			}
			return this.ActionName + " (" + this.Location.gameObject.name + ")";
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0006EFF9 File Offset: 0x0006D1F9
		protected override void Start()
		{
			base.Start();
			this.Location.NPCs.Add(this.npc);
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0006F017 File Offset: 0x0006D217
		public override void Started()
		{
			base.Started();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0006F044 File Offset: 0x0006D244
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
			if (this.npc.Movement.IsMoving)
			{
				this.Location.SetNPCReady(this.npc, false);
				this.timeAtDestination = 0f;
				return;
			}
			if (this.IsAtDestination())
			{
				this.Location.SetNPCReady(this.npc, true);
				this.timeAtDestination += Time.deltaTime;
				return;
			}
			this.Location.SetNPCReady(this.npc, false);
			this.timeAtDestination = 0f;
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
		public override void MinPassed()
		{
			base.MinPassed();
			if (InstanceFinder.IsServer)
			{
				if (!this.IsConversating && this.timeAtDestination >= 0.1f && this.CanConversationStart())
				{
					this.StartConversate();
				}
				if (!this.IsConversating && !this.IsWaiting && this.timeAtDestination >= 3f && !this.CanConversationStart())
				{
					this.StartWait();
				}
				if (this.IsConversating && !this.CanConversationStart())
				{
					this.EndConversate();
				}
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0006F162 File Offset: 0x0006D362
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0006F18C File Offset: 0x0006D38C
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.StandPoint.position);
				}
				this.npc.Movement.FaceDirection(this.StandPoint.forward, 0.5f);
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0006F20B File Offset: 0x0006D40B
		public override void End()
		{
			base.End();
			this.Location.SetNPCReady(this.npc, false);
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			if (this.IsConversating)
			{
				this.EndConversate();
			}
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0006F244 File Offset: 0x0006D444
		public override void Interrupt()
		{
			base.Interrupt();
			this.Location.SetNPCReady(this.npc, false);
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			if (this.IsConversating)
			{
				this.EndConversate();
			}
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0006F2A7 File Offset: 0x0006D4A7
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.StandPoint.position, true);
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0006F2D1 File Offset: 0x0006D4D1
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.StandPoint.position) < 1f;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0006F2FA File Offset: 0x0006D4FA
		private bool CanConversationStart()
		{
			return this.Location.NPCsReady;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0006F307 File Offset: 0x0006D507
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
			this.npc.Movement.FaceDirection(this.StandPoint.forward, 0.5f);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0006F33E File Offset: 0x0006D53E
		[ObserversRpc(RunLocally = true)]
		protected virtual void StartWait()
		{
			this.RpcWriter___Observers_StartWait_2166136261();
			this.RpcLogic___StartWait_2166136261();
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0006F34C File Offset: 0x0006D54C
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndWait()
		{
			this.RpcWriter___Observers_EndWait_2166136261();
			this.RpcLogic___EndWait_2166136261();
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0006F35A File Offset: 0x0006D55A
		[ObserversRpc(RunLocally = true)]
		protected virtual void StartConversate()
		{
			this.RpcWriter___Observers_StartConversate_2166136261();
			this.RpcLogic___StartConversate_2166136261();
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0006F368 File Offset: 0x0006D568
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndConversate()
		{
			this.RpcWriter___Observers_EndConversate_2166136261();
			this.RpcLogic___EndConversate_2166136261();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0006F3CE File Offset: 0x0006D5CE
		[CompilerGenerated]
		private IEnumerator <StartConversate>g__Routine|30_0()
		{
			while (this.IsConversating)
			{
				UnityEngine.Random.InitState(this.npc.fullName.GetHashCode() + (int)Time.time);
				float wait = UnityEngine.Random.Range(2f, 8f);
				NPC otherNPC = this.Location.GetOtherNPC(this.npc);
				for (float t = 0f; t < wait; t += Time.deltaTime)
				{
					if (!this.IsConversating)
					{
						yield break;
					}
					this.npc.Avatar.LookController.OverrideLookTarget(otherNPC.Avatar.LookController.HeadBone.position, 1, false);
					yield return new WaitForEndOfFrame();
				}
				this.npc.VoiceOverEmitter.Play(this.ConversationLines[UnityEngine.Random.Range(0, this.ConversationLines.Length)]);
				this.npc.Avatar.Anim.SetTrigger(this.AnimationTriggers[UnityEngine.Random.Range(0, this.AnimationTriggers.Length)]);
				otherNPC = null;
			}
			yield break;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0006F3E0 File Offset: 0x0006D5E0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartWait_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_EndWait_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_StartConversate_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_EndConversate_2166136261));
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0006F460 File Offset: 0x0006D660
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_ConversateAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0006F479 File Offset: 0x0006D679
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0006F488 File Offset: 0x0006D688
		private void RpcWriter___Observers_StartWait_2166136261()
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

		// Token: 0x06001948 RID: 6472 RVA: 0x0006F531 File Offset: 0x0006D731
		protected virtual void RpcLogic___StartWait_2166136261()
		{
			if (this.IsWaiting)
			{
				return;
			}
			this.IsWaiting = true;
			if (this.OnWaitStart != null)
			{
				this.OnWaitStart.Invoke();
			}
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0006F558 File Offset: 0x0006D758
		private void RpcReader___Observers_StartWait_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartWait_2166136261();
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0006F584 File Offset: 0x0006D784
		private void RpcWriter___Observers_EndWait_2166136261()
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

		// Token: 0x0600194B RID: 6475 RVA: 0x0006F62D File Offset: 0x0006D82D
		protected virtual void RpcLogic___EndWait_2166136261()
		{
			if (!this.IsWaiting)
			{
				return;
			}
			this.IsWaiting = false;
			if (this.OnWaitEnd != null)
			{
				this.OnWaitEnd.Invoke();
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0006F654 File Offset: 0x0006D854
		private void RpcReader___Observers_EndWait_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndWait_2166136261();
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0006F680 File Offset: 0x0006D880
		private void RpcWriter___Observers_StartConversate_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0006F729 File Offset: 0x0006D929
		protected virtual void RpcLogic___StartConversate_2166136261()
		{
			if (this.IsConversating)
			{
				return;
			}
			if (this.IsWaiting)
			{
				this.EndWait();
			}
			this.IsConversating = true;
			this.conversateRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<StartConversate>g__Routine|30_0());
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0006F760 File Offset: 0x0006D960
		private void RpcReader___Observers_StartConversate_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartConversate_2166136261();
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0006F78C File Offset: 0x0006D98C
		private void RpcWriter___Observers_EndConversate_2166136261()
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
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0006F835 File Offset: 0x0006DA35
		protected virtual void RpcLogic___EndConversate_2166136261()
		{
			if (!this.IsConversating)
			{
				return;
			}
			this.IsConversating = false;
			this.timeAtDestination = 0f;
			if (this.conversateRoutine != null)
			{
				base.StopCoroutine(this.conversateRoutine);
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0006F868 File Offset: 0x0006DA68
		private void RpcReader___Observers_EndConversate_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndConversate_2166136261();
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0006F892 File Offset: 0x0006DA92
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400162B RID: 5675
		private EVOLineType[] ConversationLines = new EVOLineType[]
		{
			EVOLineType.Greeting,
			EVOLineType.Question,
			EVOLineType.Surprised,
			EVOLineType.Alerted,
			EVOLineType.Annoyed,
			EVOLineType.Acknowledge,
			EVOLineType.Think,
			EVOLineType.No
		};

		// Token: 0x0400162C RID: 5676
		private string[] AnimationTriggers = new string[]
		{
			"ThumbsUp",
			"DisagreeWave",
			"Nod",
			"ConversationGesture1"
		};

		// Token: 0x0400162D RID: 5677
		public const float DESTINATION_THRESHOLD = 1f;

		// Token: 0x0400162E RID: 5678
		public const float TIME_BEFORE_WAIT_START = 3f;

		// Token: 0x0400162F RID: 5679
		public ConversationLocation Location;

		// Token: 0x04001630 RID: 5680
		private bool IsConversating;

		// Token: 0x04001631 RID: 5681
		private Coroutine conversateRoutine;

		// Token: 0x04001632 RID: 5682
		private bool IsWaiting;

		// Token: 0x04001633 RID: 5683
		public UnityEvent OnWaitStart;

		// Token: 0x04001634 RID: 5684
		public UnityEvent OnWaitEnd;

		// Token: 0x04001635 RID: 5685
		private float timeAtDestination;

		// Token: 0x04001636 RID: 5686
		private bool dll_Excuted;

		// Token: 0x04001637 RID: 5687
		private bool dll_Excuted;
	}
}
