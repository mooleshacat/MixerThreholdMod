using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000554 RID: 1364
	public class FleeBehaviour : Behaviour
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000842E7 File Offset: 0x000824E7
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x000842EF File Offset: 0x000824EF
		public NetworkObject EntityToFlee { get; private set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x000842F8 File Offset: 0x000824F8
		public Vector3 PointToFlee
		{
			get
			{
				if (this.FleeMode != FleeBehaviour.EFleeMode.Point)
				{
					return this.EntityToFlee.transform.position;
				}
				return this.FleeOrigin;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x0008431A File Offset: 0x0008251A
		// (set) Token: 0x06002032 RID: 8242 RVA: 0x00084322 File Offset: 0x00082522
		public FleeBehaviour.EFleeMode FleeMode { get; private set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x0008432B File Offset: 0x0008252B
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x00084333 File Offset: 0x00082533
		public Vector3 FleeOrigin { get; private set; } = Vector3.zero;

		// Token: 0x06002035 RID: 8245 RVA: 0x0008433C File Offset: 0x0008253C
		[ObserversRpc(RunLocally = true)]
		public void SetEntityToFlee(NetworkObject entity)
		{
			this.RpcWriter___Observers_SetEntityToFlee_3323014238(entity);
			this.RpcLogic___SetEntityToFlee_3323014238(entity);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00084352 File Offset: 0x00082552
		[ObserversRpc(RunLocally = true)]
		public void SetPointToFlee(Vector3 point)
		{
			this.RpcWriter___Observers_SetPointToFlee_4276783012(point);
			this.RpcLogic___SetPointToFlee_4276783012(point);
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00084368 File Offset: 0x00082568
		protected override void Begin()
		{
			base.Begin();
			this.StartFlee();
			EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Scared : EVOLineType.Concerned;
			base.Npc.PlayVO(lineType);
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0008439C File Offset: 0x0008259C
		protected override void Resume()
		{
			base.Resume();
			this.StartFlee();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000843AA File Offset: 0x000825AA
		protected override void End()
		{
			base.End();
			this.Stop();
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("fleeing");
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000843D2 File Offset: 0x000825D2
		protected override void Pause()
		{
			base.Pause();
			this.Stop();
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000843E0 File Offset: 0x000825E0
		private void StartFlee()
		{
			this.Flee();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Scared", "fleeing", 0f, 0);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("fleeing", 2, 0.7f));
			this.nextVO = Time.time + UnityEngine.Random.Range(5f, 15f);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00084458 File Offset: 0x00082658
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.FleeMode == FleeBehaviour.EFleeMode.Entity && this.EntityToFlee == null)
			{
				this.End();
				return;
			}
			if (!base.Npc.Movement.IsMoving && Vector3.Distance(base.transform.position, this.currentFleeTarget) < 3f)
			{
				base.End_Networked(null);
				base.Disable_Networked(null);
				return;
			}
			Vector3 from = this.PointToFlee - base.transform.position;
			from.y = 0f;
			if (Vector3.Angle(from, base.Npc.Movement.Agent.desiredVelocity) < 30f)
			{
				Console.Log("Fleeing entity is in front, finding new flee position", null);
				this.Flee();
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00084524 File Offset: 0x00082724
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (Time.time > this.nextVO)
			{
				EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Scared : EVOLineType.Concerned;
				base.Npc.PlayVO(lineType);
				this.nextVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			}
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0008457A File Offset: 0x0008277A
		private void Stop()
		{
			base.Npc.Movement.Stop();
			base.Npc.Movement.SpeedController.RemoveSpeedControl("fleeing");
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000845A8 File Offset: 0x000827A8
		private void Flee()
		{
			Vector3 fleePosition = this.GetFleePosition();
			this.currentFleeTarget = fleePosition;
			base.Npc.Movement.SetDestination(fleePosition);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000845D4 File Offset: 0x000827D4
		public Vector3 GetFleePosition()
		{
			int num = 0;
			float num2 = 0f;
			while (this.FleeMode != FleeBehaviour.EFleeMode.Entity || !(this.EntityToFlee == null))
			{
				Vector3 point = base.transform.position - this.PointToFlee;
				point.y = 0f;
				point = Quaternion.AngleAxis(num2, Vector3.up) * point;
				float d = UnityEngine.Random.Range(20f, 40f);
				RaycastHit raycastHit;
				NavMeshHit navMeshHit;
				if (Physics.Raycast(base.transform.position + point.normalized * d + Vector3.up * 10f, Vector3.down, ref raycastHit, 20f, LayerMask.GetMask(new string[]
				{
					"Default"
				})) && NavMeshUtility.SamplePosition(raycastHit.point, out navMeshHit, 2f, -1, true))
				{
					return navMeshHit.position;
				}
				if (num > 10)
				{
					Console.LogWarning("Failed to find a valid flee position, returning current position", null);
					return base.transform.position;
				}
				num2 += 15f;
				num++;
			}
			return Vector3.zero;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0008470C File Offset: 0x0008290C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetEntityToFlee_3323014238));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetPointToFlee_4276783012));
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0008475E File Offset: 0x0008295E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00084777 File Offset: 0x00082977
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00084788 File Offset: 0x00082988
		private void RpcWriter___Observers_SetEntityToFlee_3323014238(NetworkObject entity)
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
			writer.WriteNetworkObject(entity);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0008483E File Offset: 0x00082A3E
		public void RpcLogic___SetEntityToFlee_3323014238(NetworkObject entity)
		{
			this.EntityToFlee = entity;
			this.FleeMode = FleeBehaviour.EFleeMode.Entity;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00084850 File Offset: 0x00082A50
		private void RpcReader___Observers_SetEntityToFlee_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject entity = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEntityToFlee_3323014238(entity);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0008488C File Offset: 0x00082A8C
		private void RpcWriter___Observers_SetPointToFlee_4276783012(Vector3 point)
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
			writer.WriteVector3(point);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00084942 File Offset: 0x00082B42
		public void RpcLogic___SetPointToFlee_4276783012(Vector3 point)
		{
			this.FleeOrigin = point;
			this.FleeMode = FleeBehaviour.EFleeMode.Point;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00084954 File Offset: 0x00082B54
		private void RpcReader___Observers_SetPointToFlee_4276783012(PooledReader PooledReader0, Channel channel)
		{
			Vector3 point = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPointToFlee_4276783012(point);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0008498F File Offset: 0x00082B8F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018EB RID: 6379
		public const float FLEE_DIST_MIN = 20f;

		// Token: 0x040018EC RID: 6380
		public const float FLEE_DIST_MAX = 40f;

		// Token: 0x040018ED RID: 6381
		public const float FLEE_SPEED = 0.7f;

		// Token: 0x040018F1 RID: 6385
		private Vector3 currentFleeTarget = Vector3.zero;

		// Token: 0x040018F2 RID: 6386
		private float nextVO;

		// Token: 0x040018F3 RID: 6387
		private bool dll_Excuted;

		// Token: 0x040018F4 RID: 6388
		private bool dll_Excuted;

		// Token: 0x02000555 RID: 1365
		public enum EFleeMode
		{
			// Token: 0x040018F6 RID: 6390
			Entity,
			// Token: 0x040018F7 RID: 6391
			Point
		}
	}
}
