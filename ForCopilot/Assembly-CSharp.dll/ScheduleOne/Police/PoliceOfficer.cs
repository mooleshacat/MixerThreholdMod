using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.FX;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Police
{
	// Token: 0x0200034C RID: 844
	public class PoliceOfficer : NPC
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0005007C File Offset: 0x0004E27C
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x00050084 File Offset: 0x0004E284
		public NetworkObject TargetPlayerNOB
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<TargetPlayerNOB>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<TargetPlayerNOB>k__BackingField(value, true);
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00050090 File Offset: 0x0004E290
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Police.PoliceOfficer_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x000500AF File Offset: 0x0004E2AF
		protected override void Start()
		{
			base.Start();
			this.belt = this.Avatar.GetComponentInChildren<PoliceBelt>();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x000500C8 File Offset: 0x0004E2C8
		protected override void Update()
		{
			base.Update();
			if (InstanceFinder.IsServer)
			{
				this.UpdateBodySearch();
			}
			this.UpdateChatter();
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x000500E4 File Offset: 0x0004E2E4
		protected void FixedUpdate()
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Wanted].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Suspicious].Enabled = (this.TargetPlayerNOB == null && !Player.PlayerList[i].CrimeData.BodySearchPending && Player.PlayerList[i].CrimeData.TimeSinceLastBodySearch > 30f);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DisobeyingCurfew].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DrugDealing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Vandalizing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Pickpocketing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Brandishing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DischargingWeapon].Enabled = (this.TargetPlayerNOB == null);
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x000502FE File Offset: 0x0004E4FE
		protected override void MinPass()
		{
			base.MinPass();
			if (base.CurrentBuilding == null && InstanceFinder.IsServer && this.AutoDeactivate)
			{
				this.CheckDeactivation();
			}
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0005032C File Offset: 0x0004E52C
		private void CheckDeactivation()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.TargetPlayerNOB != null)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.ScheduleManager.ActiveAction != null)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.CheckpointBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.FootPatrolBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.VehiclePatrolBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.BodySearchBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.SentryBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (!base.IsConscious)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.RagdollBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.GenericDialogueBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.FacePlayerBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			this.timeSinceReadyToPool += 1f;
			if (this.timeSinceReadyToPool < 1f)
			{
				return;
			}
			if (!this.movement.IsMoving && Singleton<Map>.InstanceExists)
			{
				if (this.movement.IsAsCloseAsPossible(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position, 1f))
				{
					this.Deactivate();
					return;
				}
				if (this.movement.CanGetTo(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position, 1f))
				{
					this.movement.SetDestination(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position);
				}
				else
				{
					this.Deactivate();
				}
			}
			bool flag = false;
			foreach (Player player in Player.PlayerList)
			{
				if (player.IsPointVisibleToPlayer(this.Avatar.CenterPoint, 30f, 5f))
				{
					flag = true;
					break;
				}
				if (this.AssignedVehicle != null && player.IsPointVisibleToPlayer(this.AssignedVehicle.transform.position, 30f, 5f))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.timeSinceReadyToPool += 1f;
				this.timeSinceOutOfSight += 1f;
				if (this.timeSinceOutOfSight > 1f)
				{
					this.Deactivate();
					return;
				}
			}
			else
			{
				this.timeSinceOutOfSight = 0f;
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00050688 File Offset: 0x0004E888
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginFootPursuit_Networked(NetworkObject target, bool includeColleagues = true)
		{
			this.RpcWriter___Server_BeginFootPursuit_Networked_419679943(target, includeColleagues);
			this.RpcLogic___BeginFootPursuit_Networked_419679943(target, includeColleagues);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000506B4 File Offset: 0x0004E8B4
		[ObserversRpc(RunLocally = true)]
		private void BeginFootPursuitTest(string playerCode)
		{
			this.RpcWriter___Observers_BeginFootPursuitTest_3615296227(playerCode);
			this.RpcLogic___BeginFootPursuitTest_3615296227(playerCode);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x000506D5 File Offset: 0x0004E8D5
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginVehiclePursuit_Networked(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.RpcWriter___Server_BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
			this.RpcLogic___BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000506FC File Offset: 0x0004E8FC
		[ObserversRpc(RunLocally = true)]
		private void BeginVehiclePursuit(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.RpcWriter___Observers_BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
			this.RpcLogic___BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0005072D File Offset: 0x0004E92D
		public void BeginBodySearch_LocalPlayer()
		{
			this.BeginBodySearch_Networked(Player.Local.NetworkObject);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0005073F File Offset: 0x0004E93F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginBodySearch_Networked(NetworkObject target)
		{
			this.RpcWriter___Server_BeginBodySearch_Networked_3323014238(target);
			this.RpcLogic___BeginBodySearch_Networked_3323014238(target);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00050755 File Offset: 0x0004E955
		[ObserversRpc(RunLocally = true)]
		private void BeginBodySearch(NetworkObject target)
		{
			this.RpcWriter___Observers_BeginBodySearch_3323014238(target);
			this.RpcLogic___BeginBodySearch_3323014238(target);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0005076C File Offset: 0x0004E96C
		[ObserversRpc(RunLocally = true)]
		public virtual void AssignToCheckpoint(CheckpointManager.ECheckpointLocation location)
		{
			this.RpcWriter___Observers_AssignToCheckpoint_4087078542(location);
			this.RpcLogic___AssignToCheckpoint_4087078542(location);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0005078D File Offset: 0x0004E98D
		public void UnassignFromCheckpoint()
		{
			this.CheckpointBehaviour.Disable_Networked(null);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000507AC File Offset: 0x0004E9AC
		public void StartFootPatrol(PatrolGroup group, bool warpToStartPoint)
		{
			this.FootPatrolBehaviour.SetGroup(group);
			this.FootPatrolBehaviour.Enable_Networked(null);
			if (warpToStartPoint)
			{
				this.movement.Warp(group.GetDestination(this));
			}
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000507DB File Offset: 0x0004E9DB
		public void StartVehiclePatrol(VehiclePatrolRoute route, LandVehicle vehicle)
		{
			this.VehiclePatrolBehaviour.Vehicle = vehicle;
			this.VehiclePatrolBehaviour.SetRoute(route);
			this.VehiclePatrolBehaviour.Enable_Networked(null);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00050801 File Offset: 0x0004EA01
		public virtual void AssignToSentryLocation(SentryLocation location)
		{
			this.SentryBehaviour.AssignLocation(location);
			this.SentryBehaviour.Enable();
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0005081A File Offset: 0x0004EA1A
		public void UnassignFromSentryLocation()
		{
			this.SentryBehaviour.UnassignLocation();
			this.SentryBehaviour.Disable();
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00050832 File Offset: 0x0004EA32
		public void Activate()
		{
			this.timeSinceReadyToPool = 0f;
			this.timeSinceOutOfSight = 0f;
			base.ExitBuilding("");
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00050858 File Offset: 0x0004EA58
		public void Deactivate()
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogError("Attempted to deactivate an officer on the client", null);
				return;
			}
			if (this.AssignedVehicle != null)
			{
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<Deactivate>g__Wait|59_0());
			}
			base.EnterBuilding(null, Singleton<Map>.Instance.PoliceStation.GUID.ToString(), 0);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000508BC File Offset: 0x0004EABC
		protected override bool ShouldNoticeGeneralCrime(Player player)
		{
			return !(this.TargetPlayerNOB != null) && base.ShouldNoticeGeneralCrime(player);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00014B5A File Offset: 0x00012D5A
		public override bool ShouldSave()
		{
			return false;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000508D5 File Offset: 0x0004EAD5
		public override string GetNameAddress()
		{
			return "Officer " + this.LastName;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000508E8 File Offset: 0x0004EAE8
		private void UpdateChatter()
		{
			this.chatterCountDown -= Time.deltaTime;
			if (this.chatterCountDown <= 0f)
			{
				this.chatterCountDown = UnityEngine.Random.Range(15f, 45f);
				if (this.ChatterEnabled && this.ChatterVO.gameObject.activeInHierarchy)
				{
					this.ChatterVO.Play(EVOLineType.PoliceChatter);
				}
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00050950 File Offset: 0x0004EB50
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (PoliceOfficer.OnPoliceVisionEvent != null)
			{
				PoliceOfficer.OnPoliceVisionEvent(visionEventReceipt);
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00050964 File Offset: 0x0004EB64
		public virtual void UpdateBodySearch()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.CanInvestigate())
			{
				return;
			}
			if (this.currentBodySearchInvestigation != null)
			{
				this.UpdateExistingInvestigation();
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00050985 File Offset: 0x0004EB85
		private bool CanInvestigate()
		{
			return !this.VehiclePursuitBehaviour.Active && !this.PursuitBehaviour.Active && !this.BodySearchBehaviour.Active && !(base.CurrentBuilding != null);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000509C4 File Offset: 0x0004EBC4
		private void UpdateExistingInvestigation()
		{
			if (!this.CanInvestigatePlayer(this.currentBodySearchInvestigation.Target))
			{
				this.StopBodySearchInvestigation();
				return;
			}
			Player target = this.currentBodySearchInvestigation.Target;
			float playerVisibility = this.awareness.VisionCone.GetPlayerVisibility(target);
			float suspiciousness = target.VisualState.Suspiciousness;
			float num = Mathf.Lerp(0.2f, 2f, suspiciousness);
			float num2 = Mathf.Lerp(0.4f, 1f, playerVisibility);
			float num3 = Mathf.Lerp(1f, 0.05f, Vector3.Distance(this.Avatar.CenterPoint, target.Avatar.CenterPoint) / 12f);
			float num4 = num2 * num * num3;
			if (Application.isEditor && Input.GetKey(KeyCode.B))
			{
				num4 = 0.5f;
			}
			if (num4 < 0.08f)
			{
				num4 = -0.08f;
			}
			else if (num4 < 0.12f)
			{
				num4 = 0f;
			}
			this.currentBodySearchInvestigation.ChangeProgress(num4 * Time.deltaTime);
			if (this.currentBodySearchInvestigation.CurrentProgress >= 1f)
			{
				this.ConductBodySearch(this.currentBodySearchInvestigation.Target);
				this.StopBodySearchInvestigation();
				return;
			}
			if (this.currentBodySearchInvestigation.CurrentProgress <= -0.1f)
			{
				this.StopBodySearchInvestigation();
				return;
			}
			if (this.currentBodySearchInvestigation.CurrentProgress >= 0f)
			{
				float speed = Mathf.Lerp(0.05f, 0f, this.currentBodySearchInvestigation.CurrentProgress);
				base.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("consideringbodysearch", 5, speed));
				this.Avatar.LookController.OverrideLookTarget(target.EyePosition, 10, this.currentBodySearchInvestigation.CurrentProgress >= 0.2f);
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00050B78 File Offset: 0x0004ED78
		private void CheckNewInvestigation()
		{
			if (this.currentBodySearchInvestigation != null)
			{
				return;
			}
			if (!this.CanInvestigate())
			{
				return;
			}
			if (this.BodySearchChance <= 0f)
			{
				return;
			}
			foreach (Player player in Player.PlayerList)
			{
				if (this.CanInvestigatePlayer(player) && Vector3.Distance(this.Avatar.CenterPoint, player.Avatar.CenterPoint) <= 8f)
				{
					float playerVisibility = this.awareness.VisionCone.GetPlayerVisibility(player);
					if (playerVisibility >= 0.2f)
					{
						float suspiciousness = player.VisualState.Suspiciousness;
						float num = Mathf.Lerp(0.2f, 2f, suspiciousness);
						float num2 = Mathf.Lerp(0.4f, 1f, playerVisibility);
						float num3 = Mathf.Lerp(0.5f, 1f, this.Suspicion);
						float num4 = Mathf.Clamp01(this.BodySearchChance * num * num2 * num3 * 1f);
						if (UnityEngine.Random.Range(0f, 1f) < num4)
						{
							this.currentBodySearchInvestigation = new Investigation(player);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00050CBC File Offset: 0x0004EEBC
		private void StartBodySearchInvestigation(Player player)
		{
			Console.Log("Starting body search investigation", null);
			this.currentBodySearchInvestigation = new Investigation(player);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00050CD5 File Offset: 0x0004EED5
		private void StopBodySearchInvestigation()
		{
			this.currentBodySearchInvestigation = null;
			base.Movement.SpeedController.RemoveSpeedControl("consideringbodysearch");
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00050CF3 File Offset: 0x0004EEF3
		public void ConductBodySearch(Player player)
		{
			Console.Log("Conducting body search on " + player.PlayerName, null);
			this.BodySearchBehaviour.AssignTarget(null, player.NetworkObject);
			this.BodySearchBehaviour.Enable_Networked(null);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00050D2C File Offset: 0x0004EF2C
		private bool CanInvestigatePlayer(Player player)
		{
			return !(player == null) && player.Health.IsAlive && !player.CrimeData.BodySearchPending && player.CrimeData.CurrentPursuitLevel <= PlayerCrimeData.EPursuitLevel.None && player.CrimeData.TimeSinceLastBodySearch >= 60f && !player.IsArrested;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00050DF6 File Offset: 0x0004EFF6
		[CompilerGenerated]
		private IEnumerator <Deactivate>g__Wait|59_0()
		{
			yield return new WaitUntil(() => !this.AssignedVehicle.isOccupied);
			this.AssignedVehicle.DestroyVehicle();
			yield break;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00050E18 File Offset: 0x0004F018
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<TargetPlayerNOB>k__BackingField = new SyncVar<NetworkObject>(this, 1U, 1, 0, -1f, 0, this.<TargetPlayerNOB>k__BackingField);
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_BeginFootPursuit_Networked_419679943));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_BeginFootPursuitTest_3615296227));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_BeginVehiclePursuit_Networked_2261819652));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_BeginVehiclePursuit_2261819652));
			base.RegisterServerRpc(39U, new ServerRpcDelegate(this.RpcReader___Server_BeginBodySearch_Networked_3323014238));
			base.RegisterObserversRpc(40U, new ClientRpcDelegate(this.RpcReader___Observers_BeginBodySearch_3323014238));
			base.RegisterObserversRpc(41U, new ClientRpcDelegate(this.RpcReader___Observers_AssignToCheckpoint_4087078542));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Police.PoliceOfficer));
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00050F1A File Offset: 0x0004F11A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<TargetPlayerNOB>k__BackingField.SetRegistered();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00050F3E File Offset: 0x0004F13E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00050F4C File Offset: 0x0004F14C
		private void RpcWriter___Server_BeginFootPursuit_Networked_419679943(NetworkObject target, bool includeColleagues = true)
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
			writer.WriteNetworkObject(target);
			writer.WriteBoolean(includeColleagues);
			base.SendServerRpc(35U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00051000 File Offset: 0x0004F200
		public virtual void RpcLogic___BeginFootPursuit_Networked_419679943(NetworkObject target, bool includeColleagues = true)
		{
			if (target == null)
			{
				Console.LogError("Attempted to begin foot pursuit with null target", null);
				return;
			}
			this.BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.BodySearchBehaviour.Disable_Networked(null);
			if (includeColleagues)
			{
				if (this.FootPatrolBehaviour.Enabled && this.FootPatrolBehaviour.Group != null)
				{
					for (int i = 0; i < this.FootPatrolBehaviour.Group.Members.Count; i++)
					{
						if (!(this.FootPatrolBehaviour.Group.Members[i] == this))
						{
							(this.FootPatrolBehaviour.Group.Members[i] as PoliceOfficer).BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
				if (this.CheckpointBehaviour.Enabled && this.CheckpointBehaviour.Checkpoint != null)
				{
					for (int j = 0; j < this.CheckpointBehaviour.Checkpoint.AssignedNPCs.Count; j++)
					{
						if (!(this.CheckpointBehaviour.Checkpoint.AssignedNPCs[j] == this))
						{
							(this.CheckpointBehaviour.Checkpoint.AssignedNPCs[j] as PoliceOfficer).BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
				if (this.SentryBehaviour.Enabled && this.SentryBehaviour.AssignedLocation != null)
				{
					for (int k = 0; k < this.SentryBehaviour.AssignedLocation.AssignedOfficers.Count; k++)
					{
						if (!(this.SentryBehaviour.AssignedLocation.AssignedOfficers[k] == this))
						{
							this.SentryBehaviour.AssignedLocation.AssignedOfficers[k].BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000511E0 File Offset: 0x0004F3E0
		private void RpcReader___Server_BeginFootPursuit_Networked_419679943(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			bool includeColleagues = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginFootPursuit_Networked_419679943(target, includeColleagues);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00051230 File Offset: 0x0004F430
		private void RpcWriter___Observers_BeginFootPursuitTest_3615296227(string playerCode)
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
			writer.WriteString(playerCode);
			base.SendObserversRpc(36U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000512E8 File Offset: 0x0004F4E8
		private void RpcLogic___BeginFootPursuitTest_3615296227(string playerCode)
		{
			this.TargetPlayerNOB = Player.GetPlayer(playerCode).NetworkObject;
			if (this.TargetPlayerNOB == null)
			{
				Console.LogError("Attempted to begin foot pursuit with null target", null);
				return;
			}
			this.PursuitBehaviour.AssignTarget(null, this.TargetPlayerNOB);
			this.PursuitBehaviour.Enable();
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00051340 File Offset: 0x0004F540
		private void RpcReader___Observers_BeginFootPursuitTest_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string playerCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginFootPursuitTest_3615296227(playerCode);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0005137C File Offset: 0x0004F57C
		private void RpcWriter___Server_BeginVehiclePursuit_Networked_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
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
			writer.WriteNetworkObject(target);
			writer.WriteNetworkObject(vehicle);
			writer.WriteBoolean(beginAsSighted);
			base.SendServerRpc(37U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0005143D File Offset: 0x0004F63D
		public virtual void RpcLogic___BeginVehiclePursuit_Networked_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.BeginVehiclePursuit(target, vehicle, beginAsSighted);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00051448 File Offset: 0x0004F648
		private void RpcReader___Server_BeginVehiclePursuit_Networked_2261819652(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			NetworkObject vehicle = PooledReader0.ReadNetworkObject();
			bool beginAsSighted = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000514A8 File Offset: 0x0004F6A8
		private void RpcWriter___Observers_BeginVehiclePursuit_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
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
			writer.WriteNetworkObject(target);
			writer.WriteNetworkObject(vehicle);
			writer.WriteBoolean(beginAsSighted);
			base.SendObserversRpc(38U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00051578 File Offset: 0x0004F778
		private void RpcLogic___BeginVehiclePursuit_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.TargetPlayerNOB = target.GetComponent<Player>().NetworkObject;
			this.VehiclePursuitBehaviour.vehicle = vehicle.GetComponent<LandVehicle>();
			this.VehiclePursuitBehaviour.AssignTarget(this.TargetPlayerNOB.GetComponent<Player>());
			if (beginAsSighted)
			{
				this.VehiclePursuitBehaviour.BeginAsSighted();
			}
			this.VehiclePursuitBehaviour.Enable();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x000515D8 File Offset: 0x0004F7D8
		private void RpcReader___Observers_BeginVehiclePursuit_2261819652(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			NetworkObject vehicle = PooledReader0.ReadNetworkObject();
			bool beginAsSighted = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00051638 File Offset: 0x0004F838
		private void RpcWriter___Server_BeginBodySearch_Networked_3323014238(NetworkObject target)
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
			writer.WriteNetworkObject(target);
			base.SendServerRpc(39U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000516DF File Offset: 0x0004F8DF
		public virtual void RpcLogic___BeginBodySearch_Networked_3323014238(NetworkObject target)
		{
			this.BeginBodySearch(target);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000516E8 File Offset: 0x0004F8E8
		private void RpcReader___Server_BeginBodySearch_Networked_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginBodySearch_Networked_3323014238(target);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00051728 File Offset: 0x0004F928
		private void RpcWriter___Observers_BeginBodySearch_3323014238(NetworkObject target)
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
			writer.WriteNetworkObject(target);
			base.SendObserversRpc(40U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000517DE File Offset: 0x0004F9DE
		private void RpcLogic___BeginBodySearch_3323014238(NetworkObject target)
		{
			this.TargetPlayerNOB = target.GetComponent<Player>().NetworkObject;
			this.BodySearchBehaviour.AssignTarget(null, target);
			this.BodySearchBehaviour.Enable();
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0005180C File Offset: 0x0004FA0C
		private void RpcReader___Observers_BeginBodySearch_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginBodySearch_3323014238(target);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00051848 File Offset: 0x0004FA48
		private void RpcWriter___Observers_AssignToCheckpoint_4087078542(CheckpointManager.ECheckpointLocation location)
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
			writer.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated(location);
			base.SendObserversRpc(41U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00051900 File Offset: 0x0004FB00
		public virtual void RpcLogic___AssignToCheckpoint_4087078542(CheckpointManager.ECheckpointLocation location)
		{
			this.movement.Warp(NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(location).transform.position);
			this.CheckpointBehaviour.SetCheckpoint(location);
			this.CheckpointBehaviour.Enable();
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.CheckpointDialogue;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005195C File Offset: 0x0004FB5C
		private void RpcReader___Observers_AssignToCheckpoint_4087078542(PooledReader PooledReader0, Channel channel)
		{
			CheckpointManager.ECheckpointLocation location = GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignToCheckpoint_4087078542(location);
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x00051997 File Offset: 0x0004FB97
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x0005199F File Offset: 0x0004FB9F
		public NetworkObject SyncAccessor_<TargetPlayerNOB>k__BackingField
		{
			get
			{
				return this.<TargetPlayerNOB>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<TargetPlayerNOB>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<TargetPlayerNOB>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000519DC File Offset: 0x0004FBDC
		public override bool PoliceOfficer(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 1U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<TargetPlayerNOB>k__BackingField(this.syncVar___<TargetPlayerNOB>k__BackingField.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_<TargetPlayerNOB>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00051A30 File Offset: 0x0004FC30
		protected override void dll()
		{
			base.Awake();
			if (!PoliceOfficer.Officers.Contains(this))
			{
				PoliceOfficer.Officers.Add(this);
			}
			this.PursuitBehaviour.onEnd.AddListener(delegate()
			{
				this.TargetPlayerNOB = null;
			});
			base.InvokeRepeating("CheckNewInvestigation", 1f, 1f);
			this.chatterCountDown = UnityEngine.Random.Range(15f, 45f);
			VisionCone visionCone = this.awareness.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
		}

		// Token: 0x040011CB RID: 4555
		public const float DEACTIVATION_TIME = 1f;

		// Token: 0x040011CC RID: 4556
		public const float INVESTIGATION_COOLDOWN = 60f;

		// Token: 0x040011CD RID: 4557
		public const float INVESTIGATION_MAX_DISTANCE = 8f;

		// Token: 0x040011CE RID: 4558
		public const float INVESTIGATION_MIN_VISIBILITY = 0.2f;

		// Token: 0x040011CF RID: 4559
		public const float INVESTIGATION_CHECK_INTERVAL = 1f;

		// Token: 0x040011D0 RID: 4560
		public const float BODY_SEARCH_CHANCE_DEFAULT = 0.1f;

		// Token: 0x040011D1 RID: 4561
		public const float MIN_CHATTER_INTERVAL = 15f;

		// Token: 0x040011D2 RID: 4562
		public const float MAX_CHATTER_INTERVAL = 45f;

		// Token: 0x040011D3 RID: 4563
		public static Action<VisionEventReceipt> OnPoliceVisionEvent;

		// Token: 0x040011D4 RID: 4564
		public static List<PoliceOfficer> Officers = new List<PoliceOfficer>();

		// Token: 0x040011D6 RID: 4566
		public LandVehicle AssignedVehicle;

		// Token: 0x040011D7 RID: 4567
		[Header("References")]
		public PursuitBehaviour PursuitBehaviour;

		// Token: 0x040011D8 RID: 4568
		public VehiclePursuitBehaviour VehiclePursuitBehaviour;

		// Token: 0x040011D9 RID: 4569
		public BodySearchBehaviour BodySearchBehaviour;

		// Token: 0x040011DA RID: 4570
		public CheckpointBehaviour CheckpointBehaviour;

		// Token: 0x040011DB RID: 4571
		public FootPatrolBehaviour FootPatrolBehaviour;

		// Token: 0x040011DC RID: 4572
		public ProximityCircle ProxCircle;

		// Token: 0x040011DD RID: 4573
		public VehiclePatrolBehaviour VehiclePatrolBehaviour;

		// Token: 0x040011DE RID: 4574
		public SentryBehaviour SentryBehaviour;

		// Token: 0x040011DF RID: 4575
		public PoliceChatterVO ChatterVO;

		// Token: 0x040011E0 RID: 4576
		[Header("Dialogue")]
		public DialogueContainer CheckpointDialogue;

		// Token: 0x040011E1 RID: 4577
		[Header("Tools")]
		public AvatarEquippable BatonPrefab;

		// Token: 0x040011E2 RID: 4578
		public AvatarEquippable TaserPrefab;

		// Token: 0x040011E3 RID: 4579
		public AvatarEquippable GunPrefab;

		// Token: 0x040011E4 RID: 4580
		[Header("Settings")]
		public bool AutoDeactivate = true;

		// Token: 0x040011E5 RID: 4581
		public bool ChatterEnabled = true;

		// Token: 0x040011E6 RID: 4582
		[Header("Behaviour Settings")]
		[Range(0f, 1f)]
		public float Suspicion = 0.5f;

		// Token: 0x040011E7 RID: 4583
		[Range(0f, 1f)]
		public float Leniency = 0.5f;

		// Token: 0x040011E8 RID: 4584
		[Header("Body Search Settings")]
		[Range(0f, 1f)]
		public float BodySearchChance = 0.1f;

		// Token: 0x040011E9 RID: 4585
		[Range(1f, 10f)]
		public float BodySearchDuration = 5f;

		// Token: 0x040011EA RID: 4586
		[HideInInspector]
		public PoliceBelt belt;

		// Token: 0x040011EB RID: 4587
		private float timeSinceReadyToPool;

		// Token: 0x040011EC RID: 4588
		private float timeSinceOutOfSight;

		// Token: 0x040011ED RID: 4589
		private float chatterCountDown;

		// Token: 0x040011EE RID: 4590
		private Investigation currentBodySearchInvestigation;

		// Token: 0x040011EF RID: 4591
		public SyncVar<NetworkObject> syncVar___<TargetPlayerNOB>k__BackingField;

		// Token: 0x040011F0 RID: 4592
		private bool dll_Excuted;

		// Token: 0x040011F1 RID: 4593
		private bool dll_Excuted;
	}
}
