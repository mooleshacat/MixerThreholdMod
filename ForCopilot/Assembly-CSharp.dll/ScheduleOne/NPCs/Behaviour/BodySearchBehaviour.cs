using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000520 RID: 1312
	public class BodySearchBehaviour : Behaviour
	{
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00079208 File Offset: 0x00077408
		public static float BODY_SEARCH_TIME
		{
			get
			{
				if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
				{
					return 2.5f;
				}
				return 4f;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x00079221 File Offset: 0x00077421
		// (set) Token: 0x06001D36 RID: 7478 RVA: 0x00079229 File Offset: 0x00077429
		public Player TargetPlayer { get; protected set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x00079232 File Offset: 0x00077432
		private DialogueDatabase dialogueDatabase
		{
			get
			{
				return this.officer.dialogueHandler.Database;
			}
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00079244 File Offset: 0x00077444
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.BodySearchBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00079258 File Offset: 0x00077458
		protected override void Begin()
		{
			base.Begin();
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_begin"), NetworkSingleton<GameManager>.Instance.IsTutorial ? 4f : 5f);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("bodysearching", 40, 0.15f));
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			base.Npc.PlayVO(EVOLineType.Command);
			if (this.TargetPlayer.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.FocusCameraOnTarget(base.Npc.Avatar.MiddleSpineRB.transform);
			}
			this.TargetPlayer.CrimeData.ResetBodysearchCooldown();
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x00079324 File Offset: 0x00077524
		protected override void Resume()
		{
			base.Resume();
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_begin"), 5f);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("bodysearching", 40, 0.15f));
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
			this.TargetPlayer.CrimeData.ResetBodysearchCooldown();
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000793A4 File Offset: 0x000775A4
		protected override void End()
		{
			base.End();
			if (this.TargetPlayer != null)
			{
				this.TargetPlayer.CrimeData.BodySearchPending = false;
			}
			this.Disable();
			base.Npc.Avatar.Anim.SetBool("PatDown", false);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			this.ClearSpeedControls();
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0007940E File Offset: 0x0007760E
		protected override void Pause()
		{
			base.Pause();
			base.Npc.Avatar.Anim.SetBool("PatDown", false);
			base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			this.ClearSpeedControls();
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00079448 File Offset: 0x00077648
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.searchTime += Time.deltaTime;
			this.UpdateSearch();
			this.UpdateCircle();
			this.UpdateLookAt();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsTargetValid(this.TargetPlayer))
			{
				base.Disable_Networked(null);
				base.End_Networked(null);
				return;
			}
			this.UpdateMovement();
			this.UpdateEscalation();
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000794B0 File Offset: 0x000776B0
		private void UpdateSearch()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.TargetPlayer.IsOwner && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) < 2f && !Singleton<BodySearchScreen>.Instance.IsOpen)
			{
				base.Npc.dialogueHandler.HideWorldspaceDialogue();
				Singleton<BodySearchScreen>.Instance.onSearchClear.AddListener(new UnityAction(this.SearchClean));
				if (!GameManager.IS_TUTORIAL)
				{
					Singleton<BodySearchScreen>.Instance.onSearchFail.AddListener(new UnityAction(this.SearchFail));
				}
				float num = 1f;
				base.Npc.Movement.Stop();
				Singleton<BodySearchScreen>.Instance.Open(this.officer, this.officer.BodySearchDuration * num);
				PlayerSingleton<PlayerCamera>.Instance.StopFocus();
			}
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000795A0 File Offset: 0x000777A0
		protected virtual void UpdateMovement()
		{
			if (InstanceFinder.IsServer && Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) >= 2f)
			{
				bool flag = false;
				if (!base.Npc.Movement.IsMoving)
				{
					flag = true;
				}
				if (Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.Npc.Movement.CurrentDestination) > 2f)
				{
					flag = true;
				}
				if (flag)
				{
					Vector3 newDestination = this.GetNewDestination();
					if (base.Npc.Movement.CanGetTo(newDestination, 2f))
					{
						this.timeSinceCantReach = 0f;
						base.Npc.Movement.SetDestination(this.GetNewDestination());
						return;
					}
					this.timeSinceCantReach += Time.deltaTime;
					if (this.timeSinceCantReach >= 1f)
					{
						this.Escalate();
					}
				}
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x0007968E File Offset: 0x0007788E
		private void SearchClean()
		{
			Singleton<BodySearchScreen>.Instance.onSearchClear.RemoveListener(new UnityAction(this.SearchClean));
			Singleton<BodySearchScreen>.Instance.onSearchFail.RemoveListener(new UnityAction(this.SearchFail));
			this.ConcludeSearch(true);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000796CD File Offset: 0x000778CD
		private void SearchFail()
		{
			Singleton<BodySearchScreen>.Instance.onSearchClear.RemoveListener(new UnityAction(this.SearchClean));
			Singleton<BodySearchScreen>.Instance.onSearchFail.RemoveListener(new UnityAction(this.SearchFail));
			this.ConcludeSearch(false);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0007970C File Offset: 0x0007790C
		private void UpdateEscalation()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			if (this.searchTime >= 15f && this.TargetPlayer.IsOwner && !Singleton<BodySearchScreen>.Instance.IsOpen)
			{
				this.Escalate();
			}
			if (this.timeOutsideRange >= 4f)
			{
				this.Escalate();
			}
			if (this.TargetPlayer.CurrentVehicle != null)
			{
				this.Escalate();
			}
			if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) > Mathf.Max(15f, this.targetDistanceOnStart + 5f))
			{
				this.Escalate();
			}
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000797BC File Offset: 0x000779BC
		protected virtual void UpdateLookAt()
		{
			if (this.TargetPlayer != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.TargetPlayer.MimicCamera.position, 10, true);
			}
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000797F4 File Offset: 0x000779F4
		protected virtual void UpdateCircle()
		{
			if (this.TargetPlayer == null || this.TargetPlayer != Player.Local)
			{
				this.SetArrestCircleAlpha(0f);
				return;
			}
			float num = Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position);
			if (num < 2f)
			{
				this.SetArrestCircleAlpha(this.ArrestCircle_MaxOpacity);
				this.SetArrestCircleColor(new Color32(75, 165, byte.MaxValue, byte.MaxValue));
				return;
			}
			if (num < this.ArrestCircle_MaxVisibleDistance)
			{
				float arrestCircleAlpha = Mathf.Lerp(this.ArrestCircle_MaxOpacity, 0f, (num - 2f) / (this.ArrestCircle_MaxVisibleDistance - 2f));
				this.SetArrestCircleAlpha(arrestCircleAlpha);
				this.SetArrestCircleColor(Color.white);
				return;
			}
			this.SetArrestCircleAlpha(0f);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000798D0 File Offset: 0x00077AD0
		private void SetArrestCircleAlpha(float alpha)
		{
			this.officer.ProxCircle.SetAlpha(alpha);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000798E3 File Offset: 0x00077AE3
		private void SetArrestCircleColor(Color col)
		{
			this.officer.ProxCircle.SetColor(col);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000798F8 File Offset: 0x00077AF8
		private Vector3 GetNewDestination()
		{
			return this.TargetPlayer.Avatar.CenterPoint + (base.transform.position - this.TargetPlayer.Avatar.CenterPoint).normalized * 1.2f;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0007994C File Offset: 0x00077B4C
		private void ClearSpeedControls()
		{
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("bodysearching"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("bodysearching");
			}
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x00079984 File Offset: 0x00077B84
		private bool IsTargetValid(Player player)
		{
			return !(player == null) && !player.IsArrested && !player.IsSleeping && !player.IsUnconscious && player.Health.IsAlive && player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000799DC File Offset: 0x00077BDC
		[ObserversRpc(RunLocally = true)]
		public virtual void AssignTarget(NetworkConnection conn, NetworkObject target)
		{
			this.RpcWriter___Observers_AssignTarget_1824087381(conn, target);
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00079A08 File Offset: 0x00077C08
		public virtual bool DoesPlayerContainItemsOfInterest()
		{
			foreach (ItemSlot itemSlot in PlayerSingleton<PlayerInventory>.Instance.hotbarSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= this.MaxStealthLevel)
						{
							return true;
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00079AB8 File Offset: 0x00077CB8
		public virtual void ConcludeSearch(bool clear)
		{
			if (!clear)
			{
				if (this.ShowPostSearchDialogue)
				{
					base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_escalate"), 2f);
				}
				base.Npc.PlayVO(EVOLineType.Angry);
				this.TargetPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				this.officer.BeginFootPursuit_Networked(this.TargetPlayer.NetworkObject, true);
				if (this.onSearchComplete_ItemsFound != null)
				{
					this.onSearchComplete_ItemsFound.Invoke();
				}
			}
			else
			{
				this.NoItemsOfInterestFound();
				if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
				{
					base.Npc.PlayVO(EVOLineType.Thanks);
				}
				if (this.onSearchComplete_Clear != null)
				{
					this.onSearchComplete_Clear.Invoke();
				}
				if (this.officer.CheckpointBehaviour.Enabled)
				{
					LandVehicle lastDrivenVehicle = this.TargetPlayer.LastDrivenVehicle;
					CheckpointBehaviour checkpointBehaviour = this.officer.CheckpointBehaviour;
					if (lastDrivenVehicle != null && (checkpointBehaviour.Checkpoint.SearchArea1.vehicles.Contains(lastDrivenVehicle) || checkpointBehaviour.Checkpoint.SearchArea2.vehicles.Contains(lastDrivenVehicle)))
					{
						this.officer.dialogueHandler.ShowWorldspaceDialogue("Thanks. I'll now check your vehicle.", 5f);
						checkpointBehaviour.StartSearch(lastDrivenVehicle.NetworkObject, this.TargetPlayer.NetworkObject);
					}
				}
			}
			base.SendEnd();
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00079C14 File Offset: 0x00077E14
		public virtual void Escalate()
		{
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			Debug.Log("Escalating!");
			base.Npc.PlayVO(EVOLineType.Angry);
			base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_escalate"), 2f);
			this.TargetPlayer.CrimeData.AddCrime(new FailureToComply(), 1);
			this.TargetPlayer.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
			this.officer.BeginFootPursuit_Networked(this.TargetPlayer.NetworkObject, true);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00079CA4 File Offset: 0x00077EA4
		public virtual void NoItemsOfInterestFound()
		{
			if (this.ShowPostSearchDialogue)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "bodysearch_all_clear"), 3f);
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00079CF9 File Offset: 0x00077EF9
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_AssignTarget_1824087381));
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00079D29 File Offset: 0x00077F29
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BodySearchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00079D42 File Offset: 0x00077F42
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x00079D50 File Offset: 0x00077F50
		private void RpcWriter___Observers_AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
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

		// Token: 0x06001D54 RID: 7508 RVA: 0x00079E14 File Offset: 0x00078014
		public virtual void RpcLogic___AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			this.TargetPlayer = target.GetComponent<Player>();
			this.TargetPlayer.CrimeData.BodySearchPending = true;
			this.searchTime = 0f;
			this.timeWithinSearchRange = 0f;
			this.timeOutsideRange = 0f;
			this.hasBeenInRange = false;
			this.timeSinceCantReach = 0f;
			this.targetDistanceOnStart = Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00079E98 File Offset: 0x00078098
		private void RpcReader___Observers_AssignTarget_1824087381(PooledReader PooledReader0, Channel channel)
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
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00079EE4 File Offset: 0x000780E4
		protected override void dll()
		{
			base.Awake();
			this.officer = (base.Npc as PoliceOfficer);
		}

		// Token: 0x040017C7 RID: 6087
		public const EStealthLevel MAX_STEALTH_LEVEL = EStealthLevel.None;

		// Token: 0x040017C8 RID: 6088
		public const float BODY_SEARCH_RANGE = 2f;

		// Token: 0x040017C9 RID: 6089
		public const float MAX_SEARCH_TIME = 15f;

		// Token: 0x040017CA RID: 6090
		public const float MAX_TIME_OUTSIDE_RANGE = 4f;

		// Token: 0x040017CB RID: 6091
		public const float RANGE_TO_ESCALATE = 15f;

		// Token: 0x040017CC RID: 6092
		public const float MOVE_SPEED = 0.15f;

		// Token: 0x040017CD RID: 6093
		public const float BODY_SEARCH_COOLDOWN = 30f;

		// Token: 0x040017CF RID: 6095
		[Header("Settings")]
		public float ArrestCircle_MaxVisibleDistance = 5f;

		// Token: 0x040017D0 RID: 6096
		public float ArrestCircle_MaxOpacity = 0.25f;

		// Token: 0x040017D1 RID: 6097
		public bool ShowPostSearchDialogue = true;

		// Token: 0x040017D2 RID: 6098
		[Header("Item of interest settings")]
		public EStealthLevel MaxStealthLevel;

		// Token: 0x040017D3 RID: 6099
		private PoliceOfficer officer;

		// Token: 0x040017D4 RID: 6100
		private float targetDistanceOnStart;

		// Token: 0x040017D5 RID: 6101
		private float searchTime;

		// Token: 0x040017D6 RID: 6102
		private bool hasBeenInRange;

		// Token: 0x040017D7 RID: 6103
		private float timeOutsideRange;

		// Token: 0x040017D8 RID: 6104
		private float timeWithinSearchRange;

		// Token: 0x040017D9 RID: 6105
		private float timeSinceCantReach;

		// Token: 0x040017DA RID: 6106
		[Header("Events")]
		public UnityEvent onSearchComplete_Clear;

		// Token: 0x040017DB RID: 6107
		public UnityEvent onSearchComplete_ItemsFound;

		// Token: 0x040017DC RID: 6108
		private bool dll_Excuted;

		// Token: 0x040017DD RID: 6109
		private bool dll_Excuted;
	}
}
