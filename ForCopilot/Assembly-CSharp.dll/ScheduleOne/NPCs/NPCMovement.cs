using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Skating;
using ScheduleOne.Tools;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000499 RID: 1177
	public class NPCMovement : NetworkBehaviour
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0006BEC6 File Offset: 0x0006A0C6
		// (set) Token: 0x06001880 RID: 6272 RVA: 0x0006BECE File Offset: 0x0006A0CE
		public bool hasDestination { get; protected set; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0006BED7 File Offset: 0x0006A0D7
		public bool IsMoving
		{
			get
			{
				return ((this.Agent.hasPath || this.Agent.pathPending) && this.Agent.remainingDistance > 0.25f) || this.forceIsMoving;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001882 RID: 6274 RVA: 0x0006BF0D File Offset: 0x0006A10D
		// (set) Token: 0x06001883 RID: 6275 RVA: 0x0006BF15 File Offset: 0x0006A115
		public bool IsPaused { get; protected set; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x0006BF1E File Offset: 0x0006A11E
		public Vector3 FootPosition
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0006BF2B File Offset: 0x0006A12B
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x0006BF33 File Offset: 0x0006A133
		public float GravityMultiplier { get; protected set; } = 1f;

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x0006BF3C File Offset: 0x0006A13C
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x0006BF44 File Offset: 0x0006A144
		public NPCMovement.EStance Stance { get; protected set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x0006BF4D File Offset: 0x0006A14D
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x0006BF55 File Offset: 0x0006A155
		public float timeSinceHitByCar { get; protected set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x0006BF5E File Offset: 0x0006A15E
		public bool FaceDirectionInProgress
		{
			get
			{
				return this.FaceDirectionRoutine != null;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600188C RID: 6284 RVA: 0x0006BF69 File Offset: 0x0006A169
		// (set) Token: 0x0600188D RID: 6285 RVA: 0x0006BF71 File Offset: 0x0006A171
		public Vector3 CurrentDestination { get; protected set; } = Vector3.zero;

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600188E RID: 6286 RVA: 0x0006BF7A File Offset: 0x0006A17A
		// (set) Token: 0x0600188F RID: 6287 RVA: 0x0006BF82 File Offset: 0x0006A182
		public NPCPathCache PathCache { get; private set; } = new NPCPathCache();

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x0006BF8B File Offset: 0x0006A18B
		// (set) Token: 0x06001891 RID: 6289 RVA: 0x0006BF93 File Offset: 0x0006A193
		public bool Disoriented { get; set; }

		// Token: 0x06001892 RID: 6290 RVA: 0x0006BF9C File Offset: 0x0006A19C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCMovement_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0006BFBC File Offset: 0x0006A1BC
		private void Start()
		{
			string text = this.npc.BakedGUID;
			if (text != string.Empty)
			{
				if (text[text.Length - 1] != '1')
				{
					text = text.Substring(0, text.Length - 1) + "1";
				}
				else
				{
					text = text.Substring(0, text.Length - 1) + "2";
				}
				this.RagdollDraggable.SetGUID(new Guid(text));
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0006C03B File Offset: 0x0006A23B
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!InstanceFinder.IsServer)
			{
				this.Agent.enabled = false;
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0006C056 File Offset: 0x0006A256
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0006C05F File Offset: 0x0006A25F
		protected virtual void Update()
		{
			bool debug = this.DEBUG;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0006C068 File Offset: 0x0006A268
		protected virtual void LateUpdate()
		{
			this.forceIsMoving = false;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0006C071 File Offset: 0x0006A271
		private void UpdateRagdoll()
		{
			if (!this.npc.IsConscious)
			{
				return;
			}
			if (this.anim.Avatar.Ragdolled && this.ragdollStaticTime > 1.5f)
			{
				this.DeactivateRagdoll();
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0006C0A8 File Offset: 0x0006A2A8
		[Button]
		private void Stumble()
		{
			this.timeUntilNextStumble = UnityEngine.Random.Range(5f, 15f);
			if (UnityEngine.Random.Range(1f, 0f) < 0.1f)
			{
				this.ActivateRagdoll_Server();
				return;
			}
			this.timeSinceStumble = 0f;
			this.stumbleDirection = UnityEngine.Random.onUnitSphere;
			this.stumbleDirection.y = 0f;
			this.stumbleDirection.Normalize();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0006C118 File Offset: 0x0006A318
		private void UpdateDestination()
		{
			if (!this.hasDestination)
			{
				return;
			}
			if (this.npc.IsInVehicle)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
				return;
			}
			if (!this.IsMoving && !this.Agent.pathPending && this.CanMove())
			{
				if (this.IsAsCloseAsPossible(this.CurrentDestination, 0.5f))
				{
					if (this.Agent.hasPath)
					{
						this.Agent.ResetPath();
					}
					if (Vector3.Distance(this.CurrentDestination, this.FootPosition) < this.currentMaxDistanceForSuccess || Vector3.Distance(this.CurrentDestination, base.transform.position) < this.currentMaxDistanceForSuccess)
					{
						this.EndSetDestination(NPCMovement.WalkResult.Success);
						return;
					}
					this.EndSetDestination(NPCMovement.WalkResult.Partial);
					return;
				}
				else
				{
					this.SetDestination(this.CurrentDestination, this.walkResultCallback, false, this.currentMaxDistanceForSuccess, 1f);
				}
			}
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0006C1FC File Offset: 0x0006A3FC
		protected virtual void FixedUpdate()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsPaused)
			{
				this.Agent.isStopped = true;
			}
			this.timeSinceHitByCar += Time.fixedDeltaTime;
			this.capsuleCollider.transform.position = this.ragdollCentralRB.transform.position;
			this.UpdateSpeed();
			this.UpdateStumble();
			this.UpdateRagdoll();
			this.UpdateDestination();
			this.RecordVelocity();
			this.UpdateSlippery();
			this.UpdateCache();
			if (!this.anim.Avatar.Ragdolled || !this.CanRecoverFromRagdoll())
			{
				this.ragdollStaticTime = 0f;
				return;
			}
			this.ragdollTime += Time.fixedDeltaTime;
			if (this.ragdollCentralRB.velocity.magnitude < 0.25f)
			{
				this.ragdollStaticTime += Time.fixedDeltaTime;
				return;
			}
			this.ragdollStaticTime = 0f;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0006C2F4 File Offset: 0x0006A4F4
		private void UpdateStumble()
		{
			if (this.Disoriented && this.IsMoving)
			{
				this.timeUntilNextStumble -= Time.fixedDeltaTime;
				if (this.timeUntilNextStumble <= 0f)
				{
					this.Stumble();
				}
			}
			this.timeSinceStumble += Time.fixedDeltaTime;
			if (this.timeSinceStumble < 0.66f)
			{
				this.Agent.Move(this.stumbleDirection * (0.66f - this.timeSinceStumble) * Time.fixedDeltaTime * 7f);
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0006C38C File Offset: 0x0006A58C
		private void UpdateSpeed()
		{
			if ((double)this.MovementSpeedScale >= 0.0)
			{
				this.Agent.speed = Mathf.Lerp(this.WalkSpeed, this.RunSpeed, this.MovementSpeedScale) * this.MoveSpeedMultiplier;
				return;
			}
			this.Agent.speed = 0f;
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0006C3E8 File Offset: 0x0006A5E8
		private void RecordVelocity()
		{
			if (this.timeSinceLastVelocityHistoryRecord > this.velocityHistorySpacing)
			{
				this.timeSinceLastVelocityHistoryRecord = 0f;
				this.desiredVelocityHistory.Add(this.Agent.velocity);
				if (this.desiredVelocityHistory.Count > this.desiredVelocityHistoryLength)
				{
					this.desiredVelocityHistory.RemoveAt(0);
					return;
				}
			}
			else
			{
				this.timeSinceLastVelocityHistoryRecord += Time.fixedDeltaTime;
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0006C458 File Offset: 0x0006A658
		private void UpdateSlippery()
		{
			if (this.SlipperyMode)
			{
				Vector3 vector = Vector3.zero;
				foreach (Vector3 b in this.desiredVelocityHistory)
				{
					vector += b;
				}
				vector /= (float)this.desiredVelocityHistory.Count;
				if (this.Agent.enabled && this.Agent.isOnNavMesh)
				{
					float num = Vector3.Angle(vector, base.transform.forward);
					this.Agent.Move(vector * this.SlipperyModeMultiplier * Time.fixedDeltaTime * Mathf.Clamp01(num / 90f));
				}
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0006C530 File Offset: 0x0006A730
		private void UpdateCache()
		{
			if (this.cacheNextPath && this.Agent.path != null && this.Agent.path.corners.Length > 1)
			{
				this.cacheNextPath = false;
				this.PathCache.AddPath(this.Agent.path.corners[0], this.Agent.path.corners[this.Agent.path.corners.Length - 1], this.Agent.path);
			}
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0006C5C3 File Offset: 0x0006A7C3
		public bool CanRecoverFromRagdoll()
		{
			return !this.npc.behaviour.RagdollBehaviour.Seizure;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0006C5E0 File Offset: 0x0006A7E0
		private void UpdateAvoidance()
		{
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 25f)
			{
				this.Agent.obstacleAvoidanceType = 0;
				return;
			}
			this.Agent.obstacleAvoidanceType = this.DefaultObstacleAvoidanceType;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0006C627 File Offset: 0x0006A827
		public void OnTriggerEnter(Collider other)
		{
			this.CheckHit(other, this.capsuleCollider, false, other.transform.position);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0006C642 File Offset: 0x0006A842
		public void OnCollisionEnter(Collision collision)
		{
			this.CheckHit(collision.collider, collision.contacts[0].thisCollider, true, collision.contacts[0].point);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0006C674 File Offset: 0x0006A874
		private void CheckHit(Collider other, Collider thisCollider, bool isCollision, Vector3 hitPoint)
		{
			float num;
			Player closestPlayer = Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 30f)
			{
				return;
			}
			if ((other.gameObject.layer == LayerMask.NameToLayer("Vehicle") || other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) && !this.anim.Avatar.Ragdolled)
			{
				LandVehicle landVehicle = other.GetComponentInParent<LandVehicle>();
				if (landVehicle == null)
				{
					VehicleHumanoidCollider componentInParent = other.GetComponentInParent<VehicleHumanoidCollider>();
					if (componentInParent != null)
					{
						landVehicle = componentInParent.vehicle;
					}
				}
				if (landVehicle != null && this.npc.CurrentVehicle != landVehicle && Mathf.Abs(landVehicle.speed_Kmh) > 10f)
				{
					this.ActivateRagdoll_Server();
					if (this.onHitByCar != null)
					{
						this.onHitByCar.Invoke(other.GetComponentInParent<LandVehicle>());
					}
					this.timeSinceHitByCar = 0f;
					return;
				}
			}
			else if (other.GetComponentInParent<Skateboard>() != null && !this.anim.Avatar.Ragdolled)
			{
				if (other.GetComponentInParent<Skateboard>().VelocityCalculator.Velocity.magnitude > 2.777778f)
				{
					this.ActivateRagdoll_Server();
					this.npc.PlayVO(EVOLineType.Hurt);
					return;
				}
			}
			else if (other.GetComponentInParent<PhysicsDamageable>() != null && InstanceFinder.IsServer)
			{
				PhysicsDamageable componentInParent2 = other.GetComponentInParent<PhysicsDamageable>();
				float num2 = Mathf.Sqrt(componentInParent2.Rb.mass) * componentInParent2.Rb.velocity.magnitude;
				float num3 = componentInParent2.Rb.velocity.magnitude;
				if (num3 > 40f)
				{
					return;
				}
				if (num3 > 1f)
				{
					num3 = Mathf.Pow(num3, 1.5f);
				}
				else
				{
					num3 = Mathf.Sqrt(num3);
				}
				if (num2 > 10f)
				{
					float num4 = 1f;
					NPCMovement.EStance stance = this.Stance;
					if (stance != NPCMovement.EStance.None)
					{
						if (stance == NPCMovement.EStance.Stanced)
						{
							num4 = 0.5f;
						}
					}
					else
					{
						num4 = 1f;
					}
					float num5 = num2 * 2.5f;
					float num6 = num2 * 0.3f;
					if (num2 > 20f)
					{
						this.npc.Health.TakeDamage(num6, false);
						this.npc.ProcessImpactForce(hitPoint, componentInParent2.Rb.velocity.normalized, num5 * num4);
					}
					Impact impact = new Impact(default(RaycastHit), hitPoint, componentInParent2.Rb.velocity.normalized, num5, num6, EImpactType.PhysicsProp, (num < 15f) ? closestPlayer : null, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
					this.npc.responses.ImpactReceived(impact);
				}
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0006C935 File Offset: 0x0006AB35
		public void Warp(Transform target)
		{
			this.Warp(target.position);
			this.FaceDirection(target.forward, 0.5f);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0006C954 File Offset: 0x0006AB54
		public void Warp(Vector3 position)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsNPCPositionValid(position))
			{
				string str = "NPCMovement.Warp called with invalid position: ";
				Vector3 vector = position;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			this.Agent.Warp(position);
			this.ReceiveWarp(position);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0006C9A8 File Offset: 0x0006ABA8
		[ObserversRpc(ExcludeServer = true)]
		private void ReceiveWarp(Vector3 position)
		{
			this.RpcWriter___Observers_ReceiveWarp_4276783012(position);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0006C9BF File Offset: 0x0006ABBF
		public void VisibilityChange(bool visible)
		{
			this.capsuleCollider.gameObject.SetActive(visible);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0006C9D2 File Offset: 0x0006ABD2
		public bool CanMove()
		{
			return !this.anim.Avatar.Ragdolled && !this.npc.isInBuilding && !this.npc.IsInVehicle;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0006CA04 File Offset: 0x0006AC04
		public void SetAgentType(NPCMovement.EAgentType type)
		{
			string name = type.ToString();
			if (type == NPCMovement.EAgentType.BigHumanoid)
			{
				name = "Big Humanoid";
			}
			if (type == NPCMovement.EAgentType.IgnoreCosts)
			{
				name = "Ignore Costs";
			}
			this.Agent.agentTypeID = NavMeshUtility.GetNavMeshAgentID(name);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0006CA44 File Offset: 0x0006AC44
		public void SetSeat(AvatarSeat seat)
		{
			this.npc.Avatar.Anim.SetSeat(seat);
			this.Agent.enabled = (seat == null && InstanceFinder.IsServer);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0006CA78 File Offset: 0x0006AC78
		public void SetStance(NPCMovement.EStance stance)
		{
			this.Stance = stance;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0006CA84 File Offset: 0x0006AC84
		public void SetGravityMultiplier(float multiplier)
		{
			this.GravityMultiplier = multiplier;
			foreach (ConstantForce constantForce in this.ragdollForceComponents)
			{
				constantForce.force = Physics.gravity * this.GravityMultiplier * constantForce.GetComponent<Rigidbody>().mass;
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0006CB00 File Offset: 0x0006AD00
		public void SetRagdollDraggable(bool draggable)
		{
			this.RagdollDraggable.enabled = draggable;
			this.RagdollDraggableCollider.enabled = draggable;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0006CB1A File Offset: 0x0006AD1A
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void ActivateRagdoll_Server()
		{
			this.RpcWriter___Server_ActivateRagdoll_Server_2166136261();
			this.RpcLogic___ActivateRagdoll_Server_2166136261();
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0006CB28 File Offset: 0x0006AD28
		[ObserversRpc(RunLocally = true)]
		public void ActivateRagdoll(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.RpcWriter___Observers_ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
			this.RpcLogic___ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0006CB5C File Offset: 0x0006AD5C
		[ObserversRpc(RunLocally = true)]
		public void ApplyRagdollForce(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.RpcWriter___Observers_ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
			this.RpcLogic___ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0006CB90 File Offset: 0x0006AD90
		[ObserversRpc(RunLocally = true)]
		public void DeactivateRagdoll()
		{
			this.RpcWriter___Observers_DeactivateRagdoll_2166136261();
			this.RpcLogic___DeactivateRagdoll_2166136261();
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0006CBAC File Offset: 0x0006ADAC
		private bool SmartSampleNavMesh(Vector3 position, out NavMeshHit hit, float minRadius = 1f, float maxRadius = 10f, int steps = 3)
		{
			hit = default(NavMeshHit);
			NavMeshQueryFilter navMeshQueryFilter = default(NavMeshQueryFilter);
			navMeshQueryFilter.agentTypeID = NavMeshUtility.GetNavMeshAgentID("Humanoid");
			navMeshQueryFilter.areaMask = -1;
			for (int i = 0; i < steps; i++)
			{
				float num = Mathf.Lerp(minRadius, maxRadius, (float)(i / steps));
				if (NavMesh.SamplePosition(base.transform.position, ref hit, num, navMeshQueryFilter))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0006CC14 File Offset: 0x0006AE14
		public void SetDestination(Vector3 pos)
		{
			this.SetDestination(pos, null, 1f, 1f);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0006CC28 File Offset: 0x0006AE28
		public void SetDestination(ITransitEntity entity)
		{
			this.SetDestination(NavMeshUtility.GetAccessPoint(entity, this.npc).position);
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0006CC41 File Offset: 0x0006AE41
		public void SetDestination(Vector3 pos, Action<NPCMovement.WalkResult> callback = null, float maximumDistanceForSuccess = 1f, float cacheMaxDistSqr = 1f)
		{
			this.SetDestination(pos, callback, true, maximumDistanceForSuccess, cacheMaxDistSqr);
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0006CC50 File Offset: 0x0006AE50
		private void SetDestination(Vector3 pos, Action<NPCMovement.WalkResult> callback = null, bool interruptExistingCallback = true, float successThreshold = 1f, float cacheMaxDistSqr = 1f)
		{
			if (!this.IsNPCPositionValid(pos))
			{
				string str = "NPCMovement.SetDestination called with invalid position: ";
				Vector3 vector = pos;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			if (this.npc.Avatar.Anim.IsSeated)
			{
				this.npc.Movement.SetSeat(null);
			}
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("NPCMovement.SetDestination called on client", null);
				return;
			}
			if (this.npc.isInBuilding)
			{
				this.npc.ExitBuilding("");
			}
			if (this.DEBUG)
			{
				string fullName = this.npc.fullName;
				string str2 = " SetDestination called: ";
				Vector3 vector = pos;
				Console.Log(fullName + str2 + vector.ToString(), null);
				Debug.DrawLine(this.FootPosition, pos, Color.green, 1f);
			}
			if (!this.CanMove())
			{
				Console.LogWarning("NPCMovement.SetDestination called but CanWalk == false (" + this.npc.fullName + ")", null);
				return;
			}
			if (!this.Agent.isOnNavMesh)
			{
				Console.LogWarning("NPC is not on navmesh; warping to navmesh", null);
				NavMeshHit navMeshHit;
				if (!this.SmartSampleNavMesh(base.transform.position, out navMeshHit, 1f, 10f, 3))
				{
					Console.LogWarning("NavMesh sample failed at " + base.transform.position.ToString(), null);
					return;
				}
				this.Agent.Warp(navMeshHit.position);
				this.Agent.enabled = false;
				this.Agent.enabled = true;
			}
			if (this.walkResultCallback != null && interruptExistingCallback)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
			}
			this.walkResultCallback = callback;
			this.currentMaxDistanceForSuccess = successThreshold;
			if (this.npc.IsInVehicle)
			{
				Console.LogWarning("SetDestination called but NPC is in a vehicle; returning WalkResult.Failed", null);
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			Vector3 vector2;
			if (!this.GetClosestReachablePoint(pos, out vector2))
			{
				if (this.DEBUG)
				{
					string fullName2 = this.npc.fullName;
					string str3 = " failed to find closest reachable point for destination: ";
					Vector3 vector = pos;
					Console.LogWarning(fullName2 + str3 + vector.ToString(), null);
					Debug.DrawLine(this.FootPosition, pos, Color.red, 1f);
				}
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			if (!this.IsNPCPositionValid(vector2))
			{
				string fullName3 = this.npc.fullName;
				string str4 = " failed to find valid reachable point for destination: ";
				Vector3 vector = pos;
				Console.LogWarning(fullName3 + str4 + vector.ToString(), null);
				this.EndSetDestination(NPCMovement.WalkResult.Failed);
				return;
			}
			this.hasDestination = true;
			this.CurrentDestination = pos;
			this.currentDestination_Reachable = vector2;
			NavMeshPath path = this.PathCache.GetPath(this.Agent.transform.position, vector2, cacheMaxDistSqr);
			bool flag = false;
			if (path != null)
			{
				try
				{
					flag = this.Agent.SetPath(path);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Agent.SetDestination error: " + ex.Message, null);
					flag = false;
				}
			}
			if (!flag)
			{
				if (this.DEBUG)
				{
					Console.Log("No cached path for " + this.npc.fullName + "; calculating new path", null);
				}
				try
				{
					this.Agent.SetDestination(vector2);
					this.cacheNextPath = true;
				}
				catch (Exception ex2)
				{
					Console.LogWarning("Agent.SetDestination error: " + ex2.Message, null);
				}
			}
			if (this.IsPaused)
			{
				this.Agent.isStopped = true;
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0006CFA8 File Offset: 0x0006B1A8
		private bool IsNPCPositionValid(Vector3 position)
		{
			return !float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z) && !float.IsInfinity(position.x) && !float.IsInfinity(position.y) && !float.IsInfinity(position.z) && position.magnitude <= 10000f;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0006D018 File Offset: 0x0006B218
		private void EndSetDestination(NPCMovement.WalkResult result)
		{
			if (this.DEBUG)
			{
				Console.Log(this.npc.fullName + " EndSetDestination called: " + result.ToString(), null);
			}
			if (this.walkResultCallback != null)
			{
				this.walkResultCallback(result);
				this.walkResultCallback = null;
			}
			this.hasDestination = false;
			this.CurrentDestination = Vector3.zero;
			this.currentDestination_Reachable = Vector3.zero;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0006D090 File Offset: 0x0006B290
		public void Stop()
		{
			if (this.Agent.isOnNavMesh)
			{
				this.Agent.ResetPath();
				this.Agent.velocity = Vector3.zero;
				this.Agent.isStopped = true;
				this.Agent.isStopped = false;
			}
			if (InstanceFinder.IsServer)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Stopped);
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000045B1 File Offset: 0x000027B1
		public void WarpToNavMesh()
		{
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0006D0EC File Offset: 0x0006B2EC
		public void FacePoint(Vector3 point, float lerpTime = 0.5f)
		{
			Vector3 forward = new Vector3(point.x, base.transform.position.y, point.z) - base.transform.position;
			if (this.FaceDirectionRoutine != null)
			{
				base.StopCoroutine(this.FaceDirectionRoutine);
			}
			if (this.DEBUG)
			{
				string str = "Facing point: ";
				Vector3 vector = point;
				Debug.Log(str + vector.ToString());
			}
			this.FaceDirectionRoutine = base.StartCoroutine(this.FaceDirection_Process(forward, lerpTime));
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0006D17C File Offset: 0x0006B37C
		public void FaceDirection(Vector3 forward, float lerpTime = 0.5f)
		{
			if (this.FaceDirectionRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.FaceDirectionRoutine);
			}
			if (this.DEBUG)
			{
				string str = "Facing dir: ";
				Vector3 vector = forward;
				Debug.Log(str + vector.ToString());
			}
			this.FaceDirectionRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.FaceDirection_Process(forward, lerpTime));
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0006D1DF File Offset: 0x0006B3DF
		protected IEnumerator FaceDirection_Process(Vector3 forward, float lerpTime)
		{
			if (lerpTime > 0f)
			{
				Quaternion startRot = base.transform.rotation;
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					base.transform.rotation = Quaternion.Lerp(startRot, Quaternion.LookRotation(forward, Vector3.up), i / lerpTime);
					yield return new WaitForEndOfFrame();
				}
				startRot = default(Quaternion);
			}
			base.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
			this.FaceDirectionRoutine = null;
			yield break;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0006D1FC File Offset: 0x0006B3FC
		public void PauseMovement()
		{
			this.IsPaused = true;
			this.Agent.isStopped = true;
			this.Agent.velocity = Vector3.zero;
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0006D221 File Offset: 0x0006B421
		public void ResumeMovement()
		{
			this.IsPaused = false;
			if (this.Agent.isOnNavMesh)
			{
				this.Agent.isStopped = false;
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0006D244 File Offset: 0x0006B444
		public bool IsAsCloseAsPossible(Vector3 location, float distanceThreshold = 0.5f)
		{
			Vector3 zero = Vector3.zero;
			return this.GetClosestReachablePoint(location, out zero) && Vector3.Distance(this.FootPosition, zero) < distanceThreshold;
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0006D274 File Offset: 0x0006B474
		public bool GetClosestReachablePoint(Vector3 targetPosition, out Vector3 closestPoint)
		{
			closestPoint = Vector3.zero;
			bool flag = false;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < NPCMovement.cachedClosestPointKeys.Count; i++)
			{
				if (Vector3.SqrMagnitude(NPCMovement.cachedClosestPointKeys[i] - targetPosition) < 1f)
				{
					vector = NPCMovement.cachedClosestReachablePoints[NPCMovement.cachedClosestPointKeys[i]];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				closestPoint = vector;
				return true;
			}
			if (!this.Agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			float num = 3f;
			for (int j = 0; j < 3; j++)
			{
				NavMeshHit navMeshHit;
				if (NavMeshUtility.SamplePosition(targetPosition, out navMeshHit, num * (float)(j + 1), -1, true))
				{
					if (this.DEBUG)
					{
						Console.Log("Hit!", null);
					}
					this.Agent.CalculatePath(navMeshHit.position, navMeshPath);
					if (navMeshPath != null && navMeshPath.corners.Length != 0)
					{
						Vector3 vector2 = navMeshPath.corners[navMeshPath.corners.Length - 1];
						if (this.Agent.isActiveAndEnabled && this.Agent.isOnNavMesh)
						{
							closestPoint = vector2;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0006D3A8 File Offset: 0x0006B5A8
		public bool CanGetTo(Vector3 position, float proximityReq = 1f)
		{
			NavMeshPath navMeshPath = null;
			return this.CanGetTo(position, proximityReq, out navMeshPath);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0006D3C4 File Offset: 0x0006B5C4
		public bool CanGetTo(ITransitEntity entity, float proximityReq = 1f)
		{
			if (entity == null)
			{
				return false;
			}
			foreach (Transform transform in entity.AccessPoints)
			{
				if (!(transform == null) && this.CanGetTo(transform.position, proximityReq))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0006D40C File Offset: 0x0006B60C
		public bool CanGetTo(Vector3 position, float proximityReq, out NavMeshPath path)
		{
			path = null;
			if (Vector3.Distance(position, base.transform.position) <= proximityReq)
			{
				return true;
			}
			if (!this.Agent.isOnNavMesh)
			{
				return false;
			}
			NavMeshHit navMeshHit;
			if (!NavMeshUtility.SamplePosition(position, out navMeshHit, 2f, -1, true))
			{
				return false;
			}
			path = this.GetPathTo(navMeshHit.position, proximityReq);
			if (path == null)
			{
				Debug.DrawLine(this.FootPosition, navMeshHit.position, Color.red, 1f);
				return false;
			}
			if (path.corners.Length < 2)
			{
				Console.LogWarning("Path length < 2", null);
				return false;
			}
			float num = Vector3.Distance(path.corners[path.corners.Length - 1], navMeshHit.position);
			float num2 = Vector3.Distance(navMeshHit.position, position);
			return num <= proximityReq && num2 <= proximityReq;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0006D4DC File Offset: 0x0006B6DC
		private NavMeshPath GetPathTo(Vector3 position, float proximityReq = 1f)
		{
			if (!this.Agent.isOnNavMesh)
			{
				Console.LogWarning("Agent not on nav mesh!", null);
				return null;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			NavMeshHit navMeshHit;
			NavMeshUtility.SamplePosition(position, out navMeshHit, 2f, -1, true);
			if (!this.Agent.CalculatePath(navMeshHit.position, navMeshPath))
			{
				return null;
			}
			float num = Vector3.Distance(navMeshPath.corners[navMeshPath.corners.Length - 1], navMeshHit.position);
			float num2 = Vector3.Distance(navMeshHit.position, position);
			if (num <= proximityReq && num2 <= proximityReq)
			{
				return navMeshPath;
			}
			return null;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0006D640 File Offset: 0x0006B840
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted = true;
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveWarp_4276783012));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_ActivateRagdoll_Server_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_ActivateRagdoll_2690242654));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyRagdollForce_2690242654));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_DeactivateRagdoll_2166136261));
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0006D6D1 File Offset: 0x0006B8D1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCMovementAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0006D6E4 File Offset: 0x0006B8E4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0006D6F4 File Offset: 0x0006B8F4
		private void RpcWriter___Observers_ReceiveWarp_4276783012(Vector3 position)
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
			writer.WriteVector3(position);
			base.SendObserversRpc(0U, writer, channel, 0, false, true, false);
			writer.Store();
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0006D7AC File Offset: 0x0006B9AC
		private void RpcLogic___ReceiveWarp_4276783012(Vector3 position)
		{
			if (!this.IsNPCPositionValid(position))
			{
				string str = "NPCMovement.Warp called with invalid position: ";
				Vector3 vector = position;
				Console.LogWarning(str + vector.ToString(), null);
				return;
			}
			this.Agent.Warp(position);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0006D7F0 File Offset: 0x0006B9F0
		private void RpcReader___Observers_ReceiveWarp_4276783012(PooledReader PooledReader0, Channel channel)
		{
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveWarp_4276783012(position);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0006D824 File Offset: 0x0006BA24
		private void RpcWriter___Server_ActivateRagdoll_Server_2166136261()
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
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0006D8BE File Offset: 0x0006BABE
		public void RpcLogic___ActivateRagdoll_Server_2166136261()
		{
			this.ActivateRagdoll(Vector3.zero, Vector3.zero, 0f);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0006D8D8 File Offset: 0x0006BAD8
		private void RpcReader___Server_ActivateRagdoll_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ActivateRagdoll_Server_2166136261();
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0006D908 File Offset: 0x0006BB08
		private void RpcWriter___Observers_ActivateRagdoll_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
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
			writer.WriteVector3(forcePoint);
			writer.WriteVector3(forceDir);
			writer.WriteSingle(forceMagnitude, 0);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0006D9E0 File Offset: 0x0006BBE0
		public void RpcLogic___ActivateRagdoll_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			this.anim.SetRagdollActive(true);
			if (this.onRagdollStart != null)
			{
				this.onRagdollStart.Invoke();
			}
			if (InstanceFinder.IsServer)
			{
				this.EndSetDestination(NPCMovement.WalkResult.Interrupted);
				this.Agent.enabled = false;
			}
			this.capsuleCollider.gameObject.SetActive(false);
			if (forceMagnitude > 0f)
			{
				this.ApplyRagdollForce(forcePoint, forceDir, forceMagnitude);
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0006DA48 File Offset: 0x0006BC48
		private void RpcReader___Observers_ActivateRagdoll_2690242654(PooledReader PooledReader0, Channel channel)
		{
			Vector3 forcePoint = PooledReader0.ReadVector3();
			Vector3 forceDir = PooledReader0.ReadVector3();
			float forceMagnitude = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ActivateRagdoll_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0006DAAC File Offset: 0x0006BCAC
		private void RpcWriter___Observers_ApplyRagdollForce_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
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
			writer.WriteVector3(forcePoint);
			writer.WriteVector3(forceDir);
			writer.WriteSingle(forceMagnitude, 0);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0006DB84 File Offset: 0x0006BD84
		public void RpcLogic___ApplyRagdollForce_2690242654(Vector3 forcePoint, Vector3 forceDir, float forceMagnitude)
		{
			(from x in this.npc.Avatar.RagdollRBs
			select new
			{
				rb = x,
				dist = Vector3.Distance(x.transform.position, forcePoint)
			} into x
			orderby x.dist
			select x).First().rb.AddForceAtPosition(forceDir.normalized * forceMagnitude, forcePoint, 1);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0006DC08 File Offset: 0x0006BE08
		private void RpcReader___Observers_ApplyRagdollForce_2690242654(PooledReader PooledReader0, Channel channel)
		{
			Vector3 forcePoint = PooledReader0.ReadVector3();
			Vector3 forceDir = PooledReader0.ReadVector3();
			float forceMagnitude = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ApplyRagdollForce_2690242654(forcePoint, forceDir, forceMagnitude);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0006DC6C File Offset: 0x0006BE6C
		private void RpcWriter___Observers_DeactivateRagdoll_2166136261()
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

		// Token: 0x060018DA RID: 6362 RVA: 0x0006DD18 File Offset: 0x0006BF18
		public void RpcLogic___DeactivateRagdoll_2166136261()
		{
			this.capsuleCollider.gameObject.SetActive(this.npc.isVisible);
			this.anim.SetRagdollActive(false);
			base.transform.position = this.anim.Avatar.transform.position;
			base.transform.rotation = this.anim.Avatar.transform.rotation;
			this.anim.Avatar.transform.localPosition = Vector3.zero;
			this.anim.Avatar.transform.localRotation = Quaternion.identity;
			this.velocityCalculator.FlushBuffer();
			if (InstanceFinder.IsServer)
			{
				this.Agent.enabled = false;
				if (!this.Agent.isOnNavMesh)
				{
					NavMeshQueryFilter navMeshQueryFilter = default(NavMeshQueryFilter);
					navMeshQueryFilter.agentTypeID = NavMeshUtility.GetNavMeshAgentID("Humanoid");
					navMeshQueryFilter.areaMask = -1;
					NavMeshHit navMeshHit;
					if (this.SmartSampleNavMesh(base.transform.position, out navMeshHit, 1f, 10f, 3))
					{
						this.Agent.Warp(navMeshHit.position);
					}
					this.Agent.enabled = false;
					this.Agent.enabled = true;
				}
			}
			if (this.onRagdollEnd != null)
			{
				this.onRagdollEnd.Invoke();
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0006DE70 File Offset: 0x0006C070
		private void RpcReader___Observers_DeactivateRagdoll_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DeactivateRagdoll_2166136261();
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0006DE9C File Offset: 0x0006C09C
		protected virtual void dll()
		{
			this.npc = base.GetComponent<NPC>();
			NPC npc = this.npc;
			npc.onVisibilityChanged = (Action<bool>)Delegate.Combine(npc.onVisibilityChanged, new Action<bool>(this.VisibilityChange));
			this.VisibilityChange(this.npc.isVisible);
			base.InvokeRepeating("UpdateAvoidance", 0f, 0.5f);
			for (int i = 0; i < this.npc.Avatar.RagdollRBs.Length; i++)
			{
				this.ragdollForceComponents.Add(this.npc.Avatar.RagdollRBs[i].gameObject.AddComponent<ConstantForce>());
			}
			this.SetRagdollDraggable(false);
			this.SetGravityMultiplier(1f);
		}

		// Token: 0x040015AF RID: 5551
		public const float VEHICLE_RUNOVER_THRESHOLD = 10f;

		// Token: 0x040015B0 RID: 5552
		public const float SKATEBOARD_RUNOVER_THRESHOLD = 10f;

		// Token: 0x040015B1 RID: 5553
		public const float LIGHT_FLINCH_THRESHOLD = 50f;

		// Token: 0x040015B2 RID: 5554
		public const float HEAVY_FLINCH_THRESHOLD = 100f;

		// Token: 0x040015B3 RID: 5555
		public const float RAGDOLL_THRESHOLD = 150f;

		// Token: 0x040015B4 RID: 5556
		public const float MOMENTUM_ANNOYED_THRESHOLD = 10f;

		// Token: 0x040015B5 RID: 5557
		public const float MOMENTUM_LIGHT_FLINCH_THRESHOLD = 20f;

		// Token: 0x040015B6 RID: 5558
		public const float MOMENTUM_HEAVY_FLINCH_THRESHOLD = 40f;

		// Token: 0x040015B7 RID: 5559
		public const float MOMENTUM_RAGDOLL_THRESHOLD = 60f;

		// Token: 0x040015B8 RID: 5560
		public const bool USE_PATH_CACHE = true;

		// Token: 0x040015B9 RID: 5561
		public const float STUMBLE_DURATION = 0.66f;

		// Token: 0x040015BA RID: 5562
		public const float STUMBLE_FORCE = 7f;

		// Token: 0x040015BB RID: 5563
		public const float OBSTACLE_AVOIDANCE_RANGE = 25f;

		// Token: 0x040015BC RID: 5564
		public const float PLAYER_DIST_IMPACT_THRESHOLD = 30f;

		// Token: 0x040015BD RID: 5565
		public static Dictionary<Vector3, Vector3> cachedClosestReachablePoints = new Dictionary<Vector3, Vector3>();

		// Token: 0x040015BE RID: 5566
		public static List<Vector3> cachedClosestPointKeys = new List<Vector3>();

		// Token: 0x040015BF RID: 5567
		public const float CLOSEST_REACHABLE_POINT_CACHE_MAX_SQR_OFFSET = 1f;

		// Token: 0x040015C0 RID: 5568
		public bool DEBUG;

		// Token: 0x040015C1 RID: 5569
		[Header("Settings")]
		public float WalkSpeed = 1.8f;

		// Token: 0x040015C2 RID: 5570
		public float RunSpeed = 7f;

		// Token: 0x040015C3 RID: 5571
		public float MoveSpeedMultiplier = 1f;

		// Token: 0x040015C4 RID: 5572
		public bool SlipperyMode;

		// Token: 0x040015C5 RID: 5573
		public float SlipperyModeMultiplier = 1f;

		// Token: 0x040015C6 RID: 5574
		public ObstacleAvoidanceType DefaultObstacleAvoidanceType = 4;

		// Token: 0x040015C7 RID: 5575
		[Header("References")]
		public NavMeshAgent Agent;

		// Token: 0x040015C8 RID: 5576
		public NPCSpeedController SpeedController;

		// Token: 0x040015C9 RID: 5577
		protected NPC npc;

		// Token: 0x040015CA RID: 5578
		public CapsuleCollider capsuleCollider;

		// Token: 0x040015CB RID: 5579
		[SerializeField]
		protected NPCAnimation anim;

		// Token: 0x040015CC RID: 5580
		[SerializeField]
		protected Rigidbody ragdollCentralRB;

		// Token: 0x040015CD RID: 5581
		public SmoothedVelocityCalculator velocityCalculator;

		// Token: 0x040015CE RID: 5582
		[SerializeField]
		protected Draggable RagdollDraggable;

		// Token: 0x040015CF RID: 5583
		[SerializeField]
		protected Collider RagdollDraggableCollider;

		// Token: 0x040015D0 RID: 5584
		public float MovementSpeedScale;

		// Token: 0x040015D6 RID: 5590
		private float ragdollTime;

		// Token: 0x040015D7 RID: 5591
		private float ragdollStaticTime;

		// Token: 0x040015D8 RID: 5592
		public UnityEvent<LandVehicle> onHitByCar;

		// Token: 0x040015D9 RID: 5593
		public UnityEvent onRagdollStart;

		// Token: 0x040015DA RID: 5594
		public UnityEvent onRagdollEnd;

		// Token: 0x040015DD RID: 5597
		private bool cacheNextPath;

		// Token: 0x040015DE RID: 5598
		private Vector3 currentDestination_Reachable = Vector3.zero;

		// Token: 0x040015DF RID: 5599
		private Action<NPCMovement.WalkResult> walkResultCallback;

		// Token: 0x040015E0 RID: 5600
		private float currentMaxDistanceForSuccess = 0.5f;

		// Token: 0x040015E1 RID: 5601
		private bool forceIsMoving;

		// Token: 0x040015E2 RID: 5602
		private Coroutine FaceDirectionRoutine;

		// Token: 0x040015E3 RID: 5603
		private List<ConstantForce> ragdollForceComponents = new List<ConstantForce>();

		// Token: 0x040015E5 RID: 5605
		private float timeUntilNextStumble;

		// Token: 0x040015E6 RID: 5606
		private float timeSinceStumble = 1000f;

		// Token: 0x040015E7 RID: 5607
		private Vector3 stumbleDirection = Vector3.zero;

		// Token: 0x040015E8 RID: 5608
		private List<Vector3> desiredVelocityHistory = new List<Vector3>();

		// Token: 0x040015E9 RID: 5609
		private int desiredVelocityHistoryLength = 40;

		// Token: 0x040015EA RID: 5610
		private float velocityHistorySpacing = 0.05f;

		// Token: 0x040015EB RID: 5611
		private float timeSinceLastVelocityHistoryRecord;

		// Token: 0x040015EC RID: 5612
		private bool dll_Excuted;

		// Token: 0x040015ED RID: 5613
		private bool dll_Excuted;

		// Token: 0x0200049A RID: 1178
		public enum EAgentType
		{
			// Token: 0x040015EF RID: 5615
			Humanoid,
			// Token: 0x040015F0 RID: 5616
			BigHumanoid,
			// Token: 0x040015F1 RID: 5617
			IgnoreCosts
		}

		// Token: 0x0200049B RID: 1179
		public enum EStance
		{
			// Token: 0x040015F3 RID: 5619
			None,
			// Token: 0x040015F4 RID: 5620
			Stanced
		}

		// Token: 0x0200049C RID: 1180
		public enum WalkResult
		{
			// Token: 0x040015F6 RID: 5622
			Failed,
			// Token: 0x040015F7 RID: 5623
			Interrupted,
			// Token: 0x040015F8 RID: 5624
			Stopped,
			// Token: 0x040015F9 RID: 5625
			Partial,
			// Token: 0x040015FA RID: 5626
			Success
		}
	}
}
