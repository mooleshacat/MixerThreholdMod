using System;
using System.Collections;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vision;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Combat
{
	// Token: 0x0200077C RID: 1916
	public class CombatBehaviour : ScheduleOne.NPCs.Behaviour.Behaviour
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x000D694D File Offset: 0x000D4B4D
		// (set) Token: 0x0600338F RID: 13199 RVA: 0x000D6955 File Offset: 0x000D4B55
		public Player TargetPlayer { get; protected set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x000D695E File Offset: 0x000D4B5E
		// (set) Token: 0x06003391 RID: 13201 RVA: 0x000D6966 File Offset: 0x000D4B66
		public bool IsSearching { get; protected set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x000D696F File Offset: 0x000D4B6F
		// (set) Token: 0x06003393 RID: 13203 RVA: 0x000D6977 File Offset: 0x000D4B77
		public float TimeSinceTargetReacquired { get; protected set; }

		// Token: 0x06003394 RID: 13204 RVA: 0x000D6980 File Offset: 0x000D4B80
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Combat.CombatBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000D699F File Offset: 0x000D4B9F
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.Active && this.TargetPlayer != null)
			{
				this.SetTarget(connection, this.TargetPlayer.NetworkObject);
			}
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000D69D0 File Offset: 0x000D4BD0
		[ObserversRpc(RunLocally = true)]
		public virtual void SetTarget(NetworkConnection conn, NetworkObject target)
		{
			this.RpcWriter___Observers_SetTarget_1824087381(conn, target);
			this.RpcLogic___SetTarget_1824087381(conn, target);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000D69EE File Offset: 0x000D4BEE
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "post combat", 120f, 1);
			this.StartCombat();
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000D6A21 File Offset: 0x000D4C21
		protected override void Resume()
		{
			base.Resume();
			this.StartCombat();
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000D6A2F File Offset: 0x000D4C2F
		protected override void Pause()
		{
			base.Pause();
			this.EndCombat();
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x000D6A3D File Offset: 0x000D4C3D
		protected override void End()
		{
			base.End();
			this.EndCombat();
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000D6A4B File Offset: 0x000D4C4B
		public override void Disable()
		{
			base.Disable();
			this.TargetPlayer = null;
			this.End();
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000D6A60 File Offset: 0x000D4C60
		protected virtual void StartCombat()
		{
			this.CheckPlayerVisibility();
			this.isTargetRecentlyVisible = true;
			this.SetMovementSpeed(this.DefaultMovementSpeed);
			base.Npc.Movement.SetStance(NPCMovement.EStance.Stanced);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Angry", "combat", 0f, 3);
			if (InstanceFinder.IsServer && this.DefaultWeapon != null)
			{
				this.SetWeapon(this.DefaultWeapon.AssetPath);
			}
			this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			this.successfulHits = 0;
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000D6B18 File Offset: 0x000D4D18
		protected void EndCombat()
		{
			this.StopSearching();
			if (InstanceFinder.IsServer && this.currentWeapon != null)
			{
				this.ClearWeapon();
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("combat");
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			base.Npc.Movement.SetStance(NPCMovement.EStance.None);
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("combat");
			if (this.TargetPlayer != null)
			{
				base.Npc.awareness.VisionCone.StateSettings[this.TargetPlayer][PlayerVisualState.EVisualState.Visible].Enabled = false;
			}
			this.timeSinceLastSighting = 10000f;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000D6BE4 File Offset: 0x000D4DE4
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.UpdateLookAt();
			if (InstanceFinder.IsServer && !this.IsTargetValid())
			{
				base.Disable_Networked(null);
				return;
			}
			if (Time.time > this.nextAngryVO && this.PlayAngryVO)
			{
				EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Angry : EVOLineType.Command;
				base.Npc.PlayVO(lineType);
				this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			}
			if (this.isTargetRecentlyVisible)
			{
				this.lastKnownTargetPosition = this.TargetPlayer.Avatar.CenterPoint;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsSearching)
			{
				if (!this.isTargetImmediatelyVisible)
				{
					Console.Log("Combat action: searching", null);
					return;
				}
				this.StopSearching();
			}
			Vector3 centerPoint = base.Npc.Avatar.CenterPoint;
			if (base.Npc.Movement.IsMoving)
			{
				Vector3 currentDestination = base.Npc.Movement.CurrentDestination;
			}
			if (this.isTargetRecentlyVisible)
			{
				if (this.IsTargetInRange(base.Npc.transform.position + Vector3.up * 1f) && this.isTargetImmediatelyVisible)
				{
					if (this.ReadyToAttack(false))
					{
						this.Attack();
						return;
					}
				}
				else if (!this.IsTargetInRange(base.Npc.Movement.CurrentDestination) || !base.Npc.Movement.IsMoving)
				{
					this.RepositionToTargetRange(this.lastKnownTargetPosition);
					return;
				}
			}
			else if (base.Npc.Movement.IsMoving)
			{
				if (Vector3.Distance(base.Npc.Movement.CurrentDestination, this.lastKnownTargetPosition) > 2f)
				{
					base.Npc.Movement.SetDestination(this.lastKnownTargetPosition);
					return;
				}
			}
			else
			{
				if (Vector3.Distance(base.transform.position, this.lastKnownTargetPosition) < 2f)
				{
					this.StartSearching();
					return;
				}
				base.Npc.Movement.SetDestination(this.lastKnownTargetPosition);
			}
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000D6DEE File Offset: 0x000D4FEE
		protected virtual void FixedUpdate()
		{
			if (!base.Active)
			{
				return;
			}
			this.CheckPlayerVisibility();
			this.UpdateTimeout();
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x000D6E05 File Offset: 0x000D5005
		protected void UpdateTimeout()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.timeSinceLastSighting > this.GetSearchTime())
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x000D6E24 File Offset: 0x000D5024
		protected virtual void UpdateLookAt()
		{
			if (this.isTargetImmediatelyVisible && this.TargetPlayer != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.TargetPlayer.MimicCamera.position, 10, true);
			}
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000D6E64 File Offset: 0x000D5064
		protected void SetMovementSpeed(float speed)
		{
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("combat", 5, speed));
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000D6E88 File Offset: 0x000D5088
		[ObserversRpc(RunLocally = true)]
		protected virtual void SetWeapon(string weaponPath)
		{
			this.RpcWriter___Observers_SetWeapon_3615296227(weaponPath);
			this.RpcLogic___SetWeapon_3615296227(weaponPath);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000D6EAC File Offset: 0x000D50AC
		[ObserversRpc(RunLocally = true)]
		protected void ClearWeapon()
		{
			this.RpcWriter___Observers_ClearWeapon_2166136261();
			this.RpcLogic___ClearWeapon_2166136261();
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000D6EC5 File Offset: 0x000D50C5
		protected virtual bool ReadyToAttack(bool checkTarget = true)
		{
			if (this.TimeSinceTargetReacquired < 0.5f && checkTarget)
			{
				return false;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.IsReadyToAttack();
			}
			return this.VirtualPunchWeapon.IsReadyToAttack();
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000D6EFF File Offset: 0x000D50FF
		[ObserversRpc(RunLocally = true)]
		protected virtual void Attack()
		{
			this.RpcWriter___Observers_Attack_2166136261();
			this.RpcLogic___Attack_2166136261();
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000D6F0D File Offset: 0x000D510D
		protected void SucessfulHit()
		{
			this.successfulHits++;
			if (this.GiveUpAfterSuccessfulHits > 0 && this.successfulHits >= this.GiveUpAfterSuccessfulHits)
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000D6F3C File Offset: 0x000D513C
		protected void CheckPlayerVisibility()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			base.Npc.awareness.VisionCone.StateSettings[this.TargetPlayer][PlayerVisualState.EVisualState.Visible].Enabled = !this.isTargetRecentlyVisible;
			if (this.IsPlayerVisible())
			{
				this.playerSightedDuration += Time.fixedDeltaTime;
				this.isTargetImmediatelyVisible = true;
				this.isTargetRecentlyVisible = true;
			}
			else
			{
				this.playerSightedDuration = 0f;
				this.timeSinceLastSighting += Time.fixedDeltaTime;
				this.isTargetImmediatelyVisible = false;
				if (this.timeSinceLastSighting < 2.5f)
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
					this.isTargetRecentlyVisible = true;
				}
				else
				{
					this.isTargetRecentlyVisible = false;
				}
			}
			if (this.isTargetRecentlyVisible)
			{
				this.MarkPlayerVisible();
			}
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000D7016 File Offset: 0x000D5216
		public void MarkPlayerVisible()
		{
			if (this.IsPlayerVisible())
			{
				this.TargetPlayer.CrimeData.RecordLastKnownPosition(true);
				this.timeSinceLastSighting = 0f;
				return;
			}
			this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000D704E File Offset: 0x000D524E
		protected bool IsPlayerVisible()
		{
			return base.Npc.awareness.VisionCone.IsPlayerVisible(this.TargetPlayer);
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000D706C File Offset: 0x000D526C
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject)
			{
				if (!this.isTargetRecentlyVisible)
				{
					this.TimeSinceTargetReacquired = 0f;
				}
				this.isTargetRecentlyVisible = true;
				this.isTargetImmediatelyVisible = true;
				if (this.PlayAngryVO)
				{
					base.Npc.PlayVO(EVOLineType.Angry);
					this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
				}
			}
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000D70EB File Offset: 0x000D52EB
		protected virtual float GetSearchTime()
		{
			return this.DefaultSearchTime;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000D70F4 File Offset: 0x000D52F4
		private void StartSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Console.Log("Combat action: start searching", null);
			this.IsSearching = true;
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("searching", 6, 0.4f));
			this.searchRoutine = base.StartCoroutine(this.SearchRoutine());
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000D7154 File Offset: 0x000D5354
		private void StopSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Console.Log("Combat action: stop searching", null);
			this.IsSearching = false;
			base.Npc.Movement.SpeedController.RemoveSpeedControl("searching");
			this.hasSearchDestination = false;
			if (this.searchRoutine != null)
			{
				base.StopCoroutine(this.searchRoutine);
			}
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000D71B0 File Offset: 0x000D53B0
		private IEnumerator SearchRoutine()
		{
			while (this.IsSearching)
			{
				if (!this.hasSearchDestination)
				{
					this.currentSearchDestination = this.GetNextSearchLocation();
					base.Npc.Movement.SetDestination(this.currentSearchDestination);
					this.hasSearchDestination = true;
				}
				for (;;)
				{
					if (!base.Npc.Movement.IsMoving && base.Npc.Movement.CanMove())
					{
						base.Npc.Movement.SetDestination(this.currentSearchDestination);
					}
					if (Vector3.Distance(base.transform.position, this.currentSearchDestination) < 2f)
					{
						break;
					}
					yield return new WaitForSeconds(1f);
				}
				this.hasSearchDestination = false;
				yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 6f));
			}
			this.searchRoutine = null;
			this.StopSearching();
			yield break;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000D71C0 File Offset: 0x000D53C0
		private Vector3 GetNextSearchLocation()
		{
			float num = Mathf.Lerp(25f, 60f, Mathf.Clamp(this.timeSinceLastSighting / this.TargetPlayer.CrimeData.GetSearchTime(), 0f, 1f));
			num = Mathf.Min(num, Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint));
			return this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, num, 0f);
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000D7248 File Offset: 0x000D5448
		protected bool IsTargetValid()
		{
			return !(this.TargetPlayer == null) && !this.TargetPlayer.IsArrested && !this.TargetPlayer.IsUnconscious && this.TargetPlayer.Health.IsAlive && !this.TargetPlayer.CrimeData.BodySearchPending && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) <= this.GiveUpRange;
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000D72D8 File Offset: 0x000D54D8
		private void RepositionToTargetRange(Vector3 origin)
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			Vector3 randomReachablePointNear = this.GetRandomReachablePointNear(origin, this.GetMaxTargetDistance(), this.GetMinTargetDistance());
			base.Npc.Movement.SetDestination(randomReachablePointNear);
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000D731C File Offset: 0x000D551C
		private Vector3 GetRandomReachablePointNear(Vector3 point, float randomRadius, float minDistance = 0f)
		{
			bool flag = false;
			Vector3 result = point;
			int num = 0;
			while (!flag)
			{
				Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
				Vector3 normalized = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y).normalized;
				NavMeshHit navMeshHit;
				NavMeshUtility.SamplePosition(point + normalized * randomRadius, out navMeshHit, 5f, base.Npc.Movement.Agent.areaMask, true);
				if (base.Npc.Movement.CanGetTo(navMeshHit.position, 2f) && Vector3.Distance(point, navMeshHit.position) > minDistance)
				{
					result = navMeshHit.position;
					break;
				}
				num++;
				if (num > 10)
				{
					Console.LogError("Failed to find search destination", null);
					break;
				}
			}
			return result;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000D73E3 File Offset: 0x000D55E3
		protected float GetMinTargetDistance()
		{
			if (this.overrideTargetDistance)
			{
				return this.targetDistanceOverride;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.MinUseRange;
			}
			return 0f;
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000D7413 File Offset: 0x000D5613
		protected float GetMaxTargetDistance()
		{
			if (this.overrideTargetDistance)
			{
				return this.targetDistanceOverride;
			}
			if (this.currentWeapon != null)
			{
				return this.currentWeapon.MaxUseRange;
			}
			return 1.5f;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000D7444 File Offset: 0x000D5644
		protected bool IsTargetInRange(Vector3 origin = default(Vector3))
		{
			if (origin == default(Vector3))
			{
				origin = base.transform.position;
			}
			float num = Vector3.Distance(origin, this.TargetPlayer.Avatar.CenterPoint);
			return num > this.GetMinTargetDistance() && num < this.GetMaxTargetDistance();
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x000D7504 File Offset: 0x000D5704
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetTarget_1824087381));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetWeapon_3615296227));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_ClearWeapon_2166136261));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_Attack_2166136261));
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000D7584 File Offset: 0x000D5784
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Combat.CombatBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000D759D File Offset: 0x000D579D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000D75AC File Offset: 0x000D57AC
		private void RpcWriter___Observers_SetTarget_1824087381(NetworkConnection conn, NetworkObject target)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteNetworkObject(target);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000D766F File Offset: 0x000D586F
		public virtual void RpcLogic___SetTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			this.TargetPlayer = target.GetComponent<Player>();
			this.playerSightedDuration = 0f;
			this.timeSinceLastSighting = 0f;
			this.TimeSinceTargetReacquired = 0f;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000D76A0 File Offset: 0x000D58A0
		private void RpcReader___Observers_SetTarget_1824087381(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetTarget_1824087381(conn, target);
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000D76EC File Offset: 0x000D58EC
		private void RpcWriter___Observers_SetWeapon_3615296227(string weaponPath)
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
			writer.WriteString(weaponPath);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000D77A4 File Offset: 0x000D59A4
		protected virtual void RpcLogic___SetWeapon_3615296227(string weaponPath)
		{
			if (this.currentWeapon != null)
			{
				if (weaponPath == this.currentWeapon.AssetPath)
				{
					return;
				}
				this.ClearWeapon();
			}
			if (weaponPath == string.Empty)
			{
				return;
			}
			this.VirtualPunchWeapon.onSuccessfulHit.RemoveListener(new UnityAction(this.SucessfulHit));
			this.currentWeapon = (base.Npc.SetEquippable_Return(weaponPath) as AvatarWeapon);
			this.currentWeapon.onSuccessfulHit.AddListener(new UnityAction(this.SucessfulHit));
			if (this.currentWeapon == null)
			{
				Console.LogError("Failed to equip weapon", null);
				return;
			}
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000D7850 File Offset: 0x000D5A50
		private void RpcReader___Observers_SetWeapon_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string weaponPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetWeapon_3615296227(weaponPath);
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000D788C File Offset: 0x000D5A8C
		private void RpcWriter___Observers_ClearWeapon_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000D7938 File Offset: 0x000D5B38
		protected void RpcLogic___ClearWeapon_2166136261()
		{
			if (this.currentWeapon == null)
			{
				return;
			}
			this.currentWeapon.onSuccessfulHit.RemoveListener(new UnityAction(this.SucessfulHit));
			base.Npc.SetEquippable_Networked(null, string.Empty);
			this.currentWeapon = null;
			this.VirtualPunchWeapon.onSuccessfulHit.AddListener(new UnityAction(this.SucessfulHit));
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000D79A4 File Offset: 0x000D5BA4
		private void RpcReader___Observers_ClearWeapon_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearWeapon_2166136261();
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000D79D0 File Offset: 0x000D5BD0
		private void RpcWriter___Observers_Attack_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000D7A79 File Offset: 0x000D5C79
		protected virtual void RpcLogic___Attack_2166136261()
		{
			if (!this.ReadyToAttack(false))
			{
				return;
			}
			if (this.currentWeapon != null)
			{
				this.currentWeapon.Attack();
				return;
			}
			this.VirtualPunchWeapon.Attack();
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000D7AAC File Offset: 0x000D5CAC
		private void RpcReader___Observers_Attack_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Attack_2166136261();
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000D7AD8 File Offset: 0x000D5CD8
		protected override void dll()
		{
			base.Awake();
			VisionCone visionCone = base.Npc.awareness.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
			this.VirtualPunchWeapon.Equip(base.Npc.Avatar);
		}

		// Token: 0x0400246F RID: 9327
		public const float EXTRA_VISIBILITY_TIME = 2.5f;

		// Token: 0x04002470 RID: 9328
		public const float SEARCH_RADIUS_MIN = 25f;

		// Token: 0x04002471 RID: 9329
		public const float SEARCH_RADIUS_MAX = 60f;

		// Token: 0x04002472 RID: 9330
		public const float SEARCH_SPEED = 0.4f;

		// Token: 0x04002473 RID: 9331
		public const float CONSECUTIVE_MISS_ACCURACY_BOOST = 0.1f;

		// Token: 0x04002474 RID: 9332
		public const float REACHED_DESTINATION_DISTANCE = 2f;

		// Token: 0x04002478 RID: 9336
		[Header("General Setttings")]
		public float GiveUpRange = 20f;

		// Token: 0x04002479 RID: 9337
		public float GiveUpTime = 30f;

		// Token: 0x0400247A RID: 9338
		public int GiveUpAfterSuccessfulHits;

		// Token: 0x0400247B RID: 9339
		public bool PlayAngryVO = true;

		// Token: 0x0400247C RID: 9340
		[Header("Movement settings")]
		[Range(0f, 1f)]
		public float DefaultMovementSpeed = 0.6f;

		// Token: 0x0400247D RID: 9341
		[Header("Weapon settings")]
		public AvatarWeapon DefaultWeapon;

		// Token: 0x0400247E RID: 9342
		public AvatarMeleeWeapon VirtualPunchWeapon;

		// Token: 0x0400247F RID: 9343
		[Header("Search settings")]
		public float DefaultSearchTime = 30f;

		// Token: 0x04002480 RID: 9344
		protected bool overrideTargetDistance;

		// Token: 0x04002481 RID: 9345
		protected float targetDistanceOverride;

		// Token: 0x04002482 RID: 9346
		protected bool isTargetRecentlyVisible;

		// Token: 0x04002483 RID: 9347
		protected bool isTargetImmediatelyVisible;

		// Token: 0x04002484 RID: 9348
		protected float timeSinceLastSighting = 10000f;

		// Token: 0x04002485 RID: 9349
		protected float playerSightedDuration;

		// Token: 0x04002486 RID: 9350
		protected Vector3 lastKnownTargetPosition = Vector3.zero;

		// Token: 0x04002487 RID: 9351
		protected AvatarWeapon currentWeapon;

		// Token: 0x04002488 RID: 9352
		protected int successfulHits;

		// Token: 0x04002489 RID: 9353
		protected int consecutiveMissedShots;

		// Token: 0x0400248A RID: 9354
		protected Coroutine rangedWeaponRoutine;

		// Token: 0x0400248B RID: 9355
		protected Coroutine searchRoutine;

		// Token: 0x0400248C RID: 9356
		protected Vector3 currentSearchDestination = Vector3.zero;

		// Token: 0x0400248D RID: 9357
		protected bool hasSearchDestination;

		// Token: 0x0400248E RID: 9358
		private float nextAngryVO;

		// Token: 0x0400248F RID: 9359
		private bool dll_Excuted;

		// Token: 0x04002490 RID: 9360
		private bool dll_Excuted;
	}
}
