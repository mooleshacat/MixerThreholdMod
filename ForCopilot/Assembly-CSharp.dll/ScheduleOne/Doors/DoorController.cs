using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C5 RID: 1733
	public class DoorController : NetworkBehaviour
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000C83CD File Offset: 0x000C65CD
		// (set) Token: 0x06002F9F RID: 12191 RVA: 0x000C83D5 File Offset: 0x000C65D5
		public bool IsOpen { get; protected set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000C83DE File Offset: 0x000C65DE
		// (set) Token: 0x06002FA1 RID: 12193 RVA: 0x000C83E6 File Offset: 0x000C65E6
		public bool openedByNPC { get; protected set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000C83EF File Offset: 0x000C65EF
		// (set) Token: 0x06002FA3 RID: 12195 RVA: 0x000C83F7 File Offset: 0x000C65F7
		public float timeSinceNPCSensed { get; protected set; } = float.MaxValue;

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x000C8400 File Offset: 0x000C6600
		// (set) Token: 0x06002FA5 RID: 12197 RVA: 0x000C8408 File Offset: 0x000C6608
		public bool playerDetectedSinceOpened { get; protected set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000C8411 File Offset: 0x000C6611
		// (set) Token: 0x06002FA7 RID: 12199 RVA: 0x000C8419 File Offset: 0x000C6619
		public float timeSincePlayerSensed { get; protected set; } = float.MaxValue;

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x000C8422 File Offset: 0x000C6622
		// (set) Token: 0x06002FA9 RID: 12201 RVA: 0x000C842A File Offset: 0x000C662A
		public float timeInCurrentState { get; protected set; }

		// Token: 0x06002FAA RID: 12202 RVA: 0x000C8434 File Offset: 0x000C6634
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Doors.DoorController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000C8453 File Offset: 0x000C6653
		protected virtual void Start()
		{
			if (this.AutoCloseOnSleep)
			{
				TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(delegate()
				{
					if (this.IsOpen)
					{
						this.SetIsOpen(false, EDoorSide.Interior);
					}
				}));
			}
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000C8480 File Offset: 0x000C6680
		protected virtual void Update()
		{
			this.timeSinceNPCSensed += Time.deltaTime;
			this.timeSincePlayerSensed += Time.deltaTime;
			this.timeInCurrentState += Time.deltaTime;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsOpen && ((this.openedByNPC && this.timeSinceNPCSensed > this.ReturnToOriginalTime) || (this.autoOpenedForPlayer && this.timeSincePlayerSensed > this.ReturnToOriginalTime)))
			{
				this.openedByNPC = false;
				this.autoOpenedForPlayer = false;
				this.PlayerBlocker.enabled = false;
				this.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000C8522 File Offset: 0x000C6722
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsOpen)
			{
				this.SetIsOpen(connection, true, this.lastOpenSide);
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000C8544 File Offset: 0x000C6744
		public virtual void InteriorHandleHovered()
		{
			string text;
			if (this.CanPlayerAccess(EDoorSide.Interior, out text))
			{
				foreach (InteractableObject interactableObject in this.InteriorIntObjs)
				{
					interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
					interactableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				}
				return;
			}
			foreach (InteractableObject interactableObject2 in this.InteriorIntObjs)
			{
				if (text != string.Empty)
				{
					interactableObject2.SetMessage(text);
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				}
				else
				{
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				}
			}
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000C85D2 File Offset: 0x000C67D2
		public virtual void InteriorHandleInteracted()
		{
			if (this.CanPlayerAccess(EDoorSide.Interior))
			{
				if (!this.IsOpen && this.InteriorDoorHandleAnimation != null)
				{
					this.InteriorDoorHandleAnimation.Play();
				}
				this.SetIsOpen_Server(!this.IsOpen, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000C8610 File Offset: 0x000C6810
		public virtual void ExteriorHandleHovered()
		{
			string text;
			if (this.CanPlayerAccess(EDoorSide.Exterior, out text))
			{
				foreach (InteractableObject interactableObject in this.ExteriorIntObjs)
				{
					interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
					interactableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				}
				return;
			}
			foreach (InteractableObject interactableObject2 in this.ExteriorIntObjs)
			{
				if (text != string.Empty)
				{
					interactableObject2.SetMessage(text);
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				}
				else
				{
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				}
			}
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000C869E File Offset: 0x000C689E
		public virtual void ExteriorHandleInteracted()
		{
			if (this.CanPlayerAccess(EDoorSide.Exterior))
			{
				if (!this.IsOpen && this.ExteriorDoorHandleAnimation != null)
				{
					this.ExteriorDoorHandleAnimation.Play();
				}
				this.SetIsOpen_Server(!this.IsOpen, EDoorSide.Exterior, false);
			}
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000C86DC File Offset: 0x000C68DC
		public bool CanPlayerAccess(EDoorSide side)
		{
			string text;
			return this.CanPlayerAccess(side, out text);
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000C86F2 File Offset: 0x000C68F2
		protected virtual bool CanPlayerAccess(EDoorSide side, out string reason)
		{
			reason = this.noAccessErrorMessage;
			if (side != EDoorSide.Interior)
			{
				return side == EDoorSide.Exterior && (this.PlayerAccess == EDoorAccess.Open || this.PlayerAccess == EDoorAccess.EnterOnly);
			}
			return this.PlayerAccess == EDoorAccess.Open || this.PlayerAccess == EDoorAccess.ExitOnly;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000C8730 File Offset: 0x000C6930
		public virtual void NPCVicinityDetected(EDoorSide side)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.timeSinceNPCSensed = 0f;
			if (this.OpenableByNPCs && this.PlayerAccess != EDoorAccess.Open)
			{
				this.PlayerBlocker.enabled = true;
			}
			if (!this.IsOpen && this.OpenableByNPCs)
			{
				this.openedByNPC = true;
				this.SetIsOpen_Server(true, side, false);
			}
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000C878C File Offset: 0x000C698C
		public virtual void PlayerVicinityDetected(EDoorSide side)
		{
			this.timeSincePlayerSensed = 0f;
			if (this.IsOpen)
			{
				this.playerDetectedSinceOpened = true;
			}
			if (!this.IsOpen && this.AutoOpenForPlayer && this.CanPlayerAccess(side))
			{
				this.autoOpenedForPlayer = true;
				this.SetIsOpen_Server(true, side, true);
			}
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000C87DC File Offset: 0x000C69DC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetIsOpen_Server(bool open, EDoorSide accessSide, bool openedForPlayer)
		{
			this.RpcWriter___Server_SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
			this.RpcLogic___SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000C8804 File Offset: 0x000C6A04
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetIsOpen(NetworkConnection conn, bool open, EDoorSide openSide)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOpen_3381113727(conn, open, openSide);
				this.RpcLogic___SetIsOpen_3381113727(conn, open, openSide);
			}
			else
			{
				this.RpcWriter___Target_SetIsOpen_3381113727(conn, open, openSide);
			}
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000C8854 File Offset: 0x000C6A54
		public virtual void SetIsOpen(bool open, EDoorSide openSide)
		{
			if (this.IsOpen != open)
			{
				this.timeInCurrentState = 0f;
			}
			this.IsOpen = open;
			if (this.IsOpen)
			{
				this.playerDetectedSinceOpened = false;
			}
			this.lastOpenSide = openSide;
			if (this.IsOpen)
			{
				this.onDoorOpened.Invoke(openSide);
				return;
			}
			this.onDoorClosed.Invoke();
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000C88B4 File Offset: 0x000C6AB4
		protected virtual void CheckAutoCloseForDistantPlayer()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (this.timeSinceNPCSensed < this.ReturnToOriginalTime)
			{
				return;
			}
			if (this.timeSincePlayerSensed < this.ReturnToOriginalTime)
			{
				return;
			}
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 40f)
			{
				this.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000C8988 File Offset: 0x000C6B88
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetIsOpen_Server_1319291243));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOpen_3381113727));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOpen_3381113727));
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000C89EB File Offset: 0x000C6BEB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000C89FE File Offset: 0x000C6BFE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000C8A0C File Offset: 0x000C6C0C
		private void RpcWriter___Server_SetIsOpen_Server_1319291243(bool open, EDoorSide accessSide, bool openedForPlayer)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(accessSide);
			writer.WriteBoolean(openedForPlayer);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000C8ACD File Offset: 0x000C6CCD
		public void RpcLogic___SetIsOpen_Server_1319291243(bool open, EDoorSide accessSide, bool openedForPlayer)
		{
			this.autoOpenedForPlayer = openedForPlayer;
			if (openedForPlayer)
			{
				this.timeSincePlayerSensed = 0f;
			}
			this.SetIsOpen(null, open, accessSide);
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000C8AF0 File Offset: 0x000C6CF0
		private void RpcReader___Server_SetIsOpen_Server_1319291243(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide accessSide = GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			bool openedForPlayer = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000C8B50 File Offset: 0x000C6D50
		private void RpcWriter___Observers_SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(openSide);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x000C8C13 File Offset: 0x000C6E13
		public void RpcLogic___SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
		{
			this.SetIsOpen(open, openSide);
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000C8C20 File Offset: 0x000C6E20
		private void RpcReader___Observers_SetIsOpen_3381113727(PooledReader PooledReader0, Channel channel)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide openSide = GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_3381113727(null, open, openSide);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000C8C70 File Offset: 0x000C6E70
		private void RpcWriter___Target_SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(openSide);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000C8D34 File Offset: 0x000C6F34
		private void RpcReader___Target_SetIsOpen_3381113727(PooledReader PooledReader0, Channel channel)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide openSide = GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_3381113727(base.LocalConnection, open, openSide);
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000C8D7C File Offset: 0x000C6F7C
		protected virtual void dll()
		{
			this.PlayerBlocker.enabled = false;
			foreach (InteractableObject interactableObject in this.InteriorIntObjs)
			{
				interactableObject.onHovered.AddListener(new UnityAction(this.InteriorHandleHovered));
				interactableObject.onInteractStart.AddListener(new UnityAction(this.InteriorHandleInteracted));
				interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
			}
			foreach (InteractableObject interactableObject2 in this.ExteriorIntObjs)
			{
				interactableObject2.onHovered.AddListener(new UnityAction(this.ExteriorHandleHovered));
				interactableObject2.onInteractStart.AddListener(new UnityAction(this.ExteriorHandleInteracted));
				interactableObject2.SetMessage(this.IsOpen ? "Close" : "Open");
			}
			if (base.gameObject.isStatic)
			{
				Console.LogError("DoorController is static! Doors should not be static!", base.gameObject);
			}
			if (this.AutoCloseOnDistantPlayer)
			{
				base.InvokeRepeating("CheckAutoCloseForDistantPlayer", 2f, 2f);
			}
		}

		// Token: 0x0400217E RID: 8574
		public const float DISTANT_PLAYER_THRESHOLD = 40f;

		// Token: 0x04002180 RID: 8576
		public EDoorAccess PlayerAccess;

		// Token: 0x04002181 RID: 8577
		public bool AutoOpenForPlayer;

		// Token: 0x04002182 RID: 8578
		[Header("References")]
		[SerializeField]
		protected InteractableObject[] InteriorIntObjs;

		// Token: 0x04002183 RID: 8579
		[SerializeField]
		protected InteractableObject[] ExteriorIntObjs;

		// Token: 0x04002184 RID: 8580
		[Tooltip("Used to block player from entering when the door is open for an NPC, but player isn't permitted access.")]
		[SerializeField]
		protected BoxCollider PlayerBlocker;

		// Token: 0x04002185 RID: 8581
		[Header("Animation")]
		[SerializeField]
		protected Animation InteriorDoorHandleAnimation;

		// Token: 0x04002186 RID: 8582
		[SerializeField]
		protected Animation ExteriorDoorHandleAnimation;

		// Token: 0x04002187 RID: 8583
		[Header("Settings")]
		[SerializeField]
		protected bool AutoCloseOnSleep = true;

		// Token: 0x04002188 RID: 8584
		[SerializeField]
		protected bool AutoCloseOnDistantPlayer = true;

		// Token: 0x04002189 RID: 8585
		[Header("NPC Access")]
		[SerializeField]
		protected bool OpenableByNPCs = true;

		// Token: 0x0400218A RID: 8586
		[Tooltip("How many seconds to wait after NPC passes through to return to original state")]
		[SerializeField]
		protected float ReturnToOriginalTime = 0.5f;

		// Token: 0x0400218B RID: 8587
		public UnityEvent<EDoorSide> onDoorOpened;

		// Token: 0x0400218C RID: 8588
		public UnityEvent onDoorClosed;

		// Token: 0x0400218D RID: 8589
		private EDoorSide lastOpenSide = EDoorSide.Exterior;

		// Token: 0x04002190 RID: 8592
		private bool autoOpenedForPlayer;

		// Token: 0x04002194 RID: 8596
		[HideInInspector]
		public string noAccessErrorMessage = string.Empty;

		// Token: 0x04002195 RID: 8597
		private bool dll_Excuted;

		// Token: 0x04002196 RID: 8598
		private bool dll_Excuted;
	}
}
