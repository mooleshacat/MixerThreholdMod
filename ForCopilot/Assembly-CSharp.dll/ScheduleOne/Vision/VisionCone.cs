using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.WorldspacePopup;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vision
{
	// Token: 0x0200028B RID: 651
	public class VisionCone : NetworkBehaviour
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003BE40 File Offset: 0x0003A040
		protected float effectiveRange
		{
			get
			{
				return this.Range * this.RangeMultiplier;
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0003BE50 File Offset: 0x0003A050
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vision.VisionCone_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0003BE70 File Offset: 0x0003A070
		private void PlayerSpawned(Player plr)
		{
			Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer> dictionary = new Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>();
			for (int i = 0; i < this.StatesOfInterest.Count; i++)
			{
				dictionary.Add(this.StatesOfInterest[i].state, this.StatesOfInterest[i]);
			}
			this.StateSettings.Add(plr, dictionary);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0003BEC9 File Offset: 0x0003A0C9
		private void OnDisable()
		{
			this.ClearEvents();
			this.playerSightDatas.Clear();
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003BEDC File Offset: 0x0003A0DC
		protected virtual void Update()
		{
			if (this.DEBUG_FRUSTRUM)
			{
				this.GetFrustumVertices();
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003BEF0 File Offset: 0x0003A0F0
		protected virtual void FixedUpdate()
		{
			if (!this.VisionEnabled)
			{
				foreach (VisionEvent visionEvent in this.activeVisionEvents)
				{
					visionEvent.EndEvent();
				}
				return;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0003BF4C File Offset: 0x0003A14C
		protected virtual void VisionUpdate()
		{
			if (!base.enabled || !this.VisionEnabled)
			{
				return;
			}
			this.UpdateVision(0.1f);
			this.UpdateEvents(0.1f);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003BF78 File Offset: 0x0003A178
		protected virtual void UpdateEvents(float tickTime)
		{
			foreach (Player player in this.playerSightDatas.Keys)
			{
				if (!(player != Player.Local) && player.Health.IsAlive && !player.IsArrested)
				{
					foreach (PlayerVisualState.VisualState visualState in player.VisualState.visualStates)
					{
						if (this.StateSettings[player].ContainsKey(visualState.state) && this.StateSettings[player][visualState.state].Enabled)
						{
							VisionCone.StateContainer stateContainer = this.StateSettings[player][visualState.state];
							if (this.GetEvent(player, visualState) == null)
							{
								VisionEvent visionEvent = new VisionEvent(this, player, visualState, stateContainer.RequiredNoticeTime);
								visionEvent.UpdateEvent(this.playerSightDatas[player].VisionDelta, tickTime);
								this.activeVisionEvents.Add(visionEvent);
								if (this.onVisionEventStarted != null)
								{
									VisionEventReceipt @event = new VisionEventReceipt(player.NetworkObject, visualState.state);
									this.onVisionEventStarted(@event);
								}
							}
						}
					}
				}
			}
			List<VisionEvent> list = new List<VisionEvent>();
			list.AddRange(this.activeVisionEvents);
			foreach (VisionEvent visionEvent2 in list)
			{
				if (!this.StateSettings[visionEvent2.Target].ContainsKey(visionEvent2.State.state) || !this.StateSettings[visionEvent2.Target][visionEvent2.State.state].Enabled)
				{
					visionEvent2.EndEvent();
				}
			}
			List<VisionEvent> list2 = this.activeVisionEvents.FindAll((VisionEvent x) => x.Target == Player.Local);
			float num = 0f;
			for (int i = 0; i < list2.Count; i++)
			{
				if (this.playerSightDatas.ContainsKey(Player.Local))
				{
					list2[i].UpdateEvent(this.playerSightDatas[Player.Local].VisionDelta, tickTime);
				}
				else
				{
					list2[i].UpdateEvent(0f, tickTime);
				}
				if (list2[i].NormalizedNoticeLevel > num)
				{
					num = list2[i].NormalizedNoticeLevel;
				}
			}
			if (num > 0f && this.WorldspaceIconsEnabled)
			{
				this.QuestionMarkPopup.enabled = true;
				this.QuestionMarkPopup.CurrentFillLevel = num;
				return;
			}
			this.QuestionMarkPopup.enabled = false;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0003C2AC File Offset: 0x0003A4AC
		protected virtual void UpdateVision(float tickTime)
		{
			if (this.npc != null && !this.npc.IsConscious)
			{
				return;
			}
			this.sightedPlayers = new List<Player>();
			if (!this.DisableSightUpdates)
			{
				for (int i = 0; i < Player.PlayerList.Count; i++)
				{
					Player player = Player.PlayerList[i];
					if (this.IsPointWithinSight(player.Avatar.CenterPoint, true, null))
					{
						float num = player.Visibility.CalculateExposureToPoint(this.VisionOrigin.position, this.effectiveRange, this.npc);
						if (player.CurrentVehicle != null && this.IsPointWithinSight(player.CurrentVehicle.transform.position, false, player.CurrentVehicle.GetComponent<LandVehicle>()))
						{
							num = 1f;
						}
						if (num > 0f)
						{
							float num2 = num * this.VisionFalloff.Evaluate(Mathf.Clamp01(Vector3.Distance(this.VisionOrigin.position, player.Avatar.CenterPoint) / this.effectiveRange)) * player.Visibility.CurrentVisibility / 100f;
							if (num2 > this.MinVisionDelta)
							{
								this.sightedPlayers.Add(player);
								VisionCone.PlayerSightData playerSightData;
								if (this.IsPlayerVisible(player, out playerSightData))
								{
									playerSightData.TimeVisible += tickTime;
									playerSightData.VisionDelta = num2;
								}
								else
								{
									playerSightData = new VisionCone.PlayerSightData();
									playerSightData.Player = player;
									playerSightData.TimeVisible = 0f;
									playerSightData.VisionDelta = num2;
									this.playerSightDatas.Add(player, playerSightData);
								}
							}
						}
					}
				}
			}
			foreach (Player player2 in new List<Player>(this.playerSightDatas.Keys))
			{
				if (!this.sightedPlayers.Contains(player2))
				{
					this.playerSightDatas.Remove(player2);
				}
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003C4AC File Offset: 0x0003A6AC
		public virtual void EventReachedZero(VisionEvent _event)
		{
			this.activeVisionEvents.Remove(_event);
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Zero);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003C4EC File Offset: 0x0003A6EC
		public virtual void EventHalfNoticed(VisionEvent _event)
		{
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Half);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003C520 File Offset: 0x0003A720
		public virtual void EventFullyNoticed(VisionEvent _event)
		{
			this.activeVisionEvents.Remove(_event);
			if (this.WorldspaceIconsEnabled && _event.Target.Owner.IsLocalClient)
			{
				this.ExclamationPointPopup.Popup();
				this.ExclamationSound.Play();
			}
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Full);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0003C58E File Offset: 0x0003A78E
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendEventReceipt(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.RpcWriter___Server_SendEventReceipt_3486014028(receipt, level);
			this.RpcLogic___SendEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0003C5AC File Offset: 0x0003A7AC
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public virtual void ReceiveEventReceipt(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.RpcWriter___Observers_ReceiveEventReceipt_3486014028(receipt, level);
			this.RpcLogic___ReceiveEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003C5D8 File Offset: 0x0003A7D8
		public virtual bool IsPointWithinSight(Vector3 point, bool ignoreLoS = false, LandVehicle vehicleToIgnore = null)
		{
			if (Vector3.Distance(point, this.VisionOrigin.position) > this.effectiveRange)
			{
				return false;
			}
			if (Vector3.SignedAngle(this.VisionOrigin.forward, (point - this.VisionOrigin.position).normalized, this.VisionOrigin.up) > 90f)
			{
				return false;
			}
			if (Vector3.SignedAngle(this.VisionOrigin.forward, (point - this.VisionOrigin.position).normalized, this.VisionOrigin.right) > 90f)
			{
				return false;
			}
			Plane[] frustumPlanes = this.GetFrustumPlanes();
			for (int i = 0; i < 6; i++)
			{
				if (frustumPlanes[i].GetDistanceToPoint(point) > 0f)
				{
					return false;
				}
			}
			RaycastHit raycastHit;
			return ignoreLoS || !Physics.Raycast(this.VisionOrigin.position, point - this.VisionOrigin.position, ref raycastHit, Vector3.Distance(point, this.VisionOrigin.position), this.VisibilityBlockingLayers) || (vehicleToIgnore != null && raycastHit.collider.GetComponentInParent<LandVehicle>() == vehicleToIgnore);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0003C70C File Offset: 0x0003A90C
		public VisionEvent GetEvent(Player target, PlayerVisualState.VisualState state)
		{
			return this.activeVisionEvents.Find((VisionEvent x) => x.Target == target && x.State == state);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0003C744 File Offset: 0x0003A944
		public bool IsPlayerVisible(Player player)
		{
			return this.playerSightDatas.ContainsKey(player) && this.playerSightDatas[player].VisionDelta > this.MinVisionDelta;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003C76F File Offset: 0x0003A96F
		public float GetPlayerVisibility(Player player)
		{
			if (this.playerSightDatas.ContainsKey(player))
			{
				return this.playerSightDatas[player].VisionDelta;
			}
			return 0f;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0003C796 File Offset: 0x0003A996
		public bool IsPlayerVisible(Player player, out VisionCone.PlayerSightData data)
		{
			if (this.playerSightDatas.ContainsKey(player))
			{
				data = this.playerSightDatas[player];
				return true;
			}
			data = null;
			return false;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0003C7BA File Offset: 0x0003A9BA
		public virtual void SetGeneralCrimeResponseActive(Player player, bool active)
		{
			if (this.generalCrimeResponseActive == active)
			{
				return;
			}
			this.StateSettings[player][PlayerVisualState.EVisualState.PettyCrime].Enabled = active;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003C7DE File Offset: 0x0003A9DE
		private void OnDie()
		{
			this.ClearEvents();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0003C7E8 File Offset: 0x0003A9E8
		public void ClearEvents()
		{
			this.ExclamationPointPopup.enabled = false;
			this.QuestionMarkPopup.enabled = false;
			VisionEvent[] array = this.activeVisionEvents.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].EndEvent();
			}
			this.activeVisionEvents.Clear();
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003C83C File Offset: 0x0003AA3C
		private Vector3[] GetFrustumVertices()
		{
			Vector3 position = this.VisionOrigin.position;
			Quaternion rotation = this.VisionOrigin.rotation;
			float z = 0f;
			float effectiveRange = this.effectiveRange;
			float minorWidth = this.MinorWidth;
			float minorHeight = this.MinorHeight;
			float num = minorWidth + 2f * this.effectiveRange * Mathf.Tan(this.HorizontalFOV * 0.017453292f / 2f);
			float num2 = minorHeight + 2f * this.effectiveRange * Mathf.Tan(this.VerticalFOV * 0.017453292f / 2f);
			Vector3[] array = new Vector3[8];
			Vector3 vector = position + rotation * new Vector3(-minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector2 = position + rotation * new Vector3(minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector3 = position + rotation * new Vector3(-minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector4 = position + rotation * new Vector3(minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector5 = position + rotation * new Vector3(-num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector6 = position + rotation * new Vector3(num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector7 = position + rotation * new Vector3(-num / 2f, -num2 / 2f, effectiveRange);
			Vector3 vector8 = position + rotation * new Vector3(num / 2f, -num2 / 2f, effectiveRange);
			array[0] = vector;
			array[1] = vector2;
			array[2] = vector3;
			array[3] = vector4;
			array[4] = vector5;
			array[5] = vector6;
			array[6] = vector7;
			array[7] = vector8;
			Debug.DrawLine(vector, vector5, Color.red);
			Debug.DrawLine(vector2, vector6, Color.green);
			Debug.DrawLine(vector3, vector7, Color.blue);
			Debug.DrawLine(vector4, vector8, Color.magenta);
			return array;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0003CA84 File Offset: 0x0003AC84
		private Plane[] GetFrustumPlanes()
		{
			Vector3 position = this.VisionOrigin.position;
			Quaternion rotation = this.VisionOrigin.rotation;
			float z = 0f;
			float effectiveRange = this.effectiveRange;
			float minorWidth = this.MinorWidth;
			float minorHeight = this.MinorHeight;
			float num = minorWidth + 2f * this.effectiveRange * Mathf.Tan(this.HorizontalFOV * 0.017453292f / 2f);
			float num2 = minorHeight + 2f * this.effectiveRange * Mathf.Tan(this.VerticalFOV * 0.017453292f / 2f);
			Plane[] array = new Plane[6];
			Vector3 vector = position + rotation * new Vector3(-minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector2 = position + rotation * new Vector3(minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector3 = position + rotation * new Vector3(-minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector4 = position + rotation * new Vector3(minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector5 = position + rotation * new Vector3(-num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector6 = position + rotation * new Vector3(num / 2f, num2 / 2f, effectiveRange);
			Vector3 c = position + rotation * new Vector3(-num / 2f, -num2 / 2f, effectiveRange);
			Vector3 c2 = position + rotation * new Vector3(num / 2f, -num2 / 2f, effectiveRange);
			array[0] = new Plane(vector2, vector, vector5);
			array[1] = new Plane(vector3, vector4, c2);
			array[2] = new Plane(vector, vector3, c);
			array[3] = new Plane(vector4, vector2, vector6);
			array[4] = new Plane(vector, vector2, vector4);
			array[5] = new Plane(vector6, vector5, c);
			return array;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0003CD88 File Offset: 0x0003AF88
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendEventReceipt_3486014028));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEventReceipt_3486014028));
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0003CDD4 File Offset: 0x0003AFD4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0003CDE7 File Offset: 0x0003AFE7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003CDF8 File Offset: 0x0003AFF8
		private void RpcWriter___Server_SendEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
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
			writer.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated(receipt);
			writer.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated(level);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		public void RpcLogic___SendEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.ReceiveEventReceipt(receipt, level);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0003CEB8 File Offset: 0x0003B0B8
		private void RpcReader___Server_SendEventReceipt_3486014028(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			VisionEventReceipt receipt = GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds(PooledReader0);
			VisionCone.EEventLevel level = GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0003CF08 File Offset: 0x0003B108
		private void RpcWriter___Observers_ReceiveEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
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
			writer.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated(receipt);
			writer.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated(level);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, true);
			writer.Store();
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0003CFCC File Offset: 0x0003B1CC
		public virtual void RpcLogic___ReceiveEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			switch (level)
			{
			case VisionCone.EEventLevel.Start:
				if (this.onVisionEventStarted != null)
				{
					this.onVisionEventStarted(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Half:
				if (this.onVisionEventHalf != null)
				{
					this.onVisionEventHalf(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Full:
				if (this.onVisionEventFull != null)
				{
					this.onVisionEventFull(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Zero:
				if (this.onVisionEventExpired != null)
				{
					this.onVisionEventExpired(receipt);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003D044 File Offset: 0x0003B244
		private void RpcReader___Observers_ReceiveEventReceipt_3486014028(PooledReader PooledReader0, Channel channel)
		{
			VisionEventReceipt receipt = GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds(PooledReader0);
			VisionCone.EEventLevel level = GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0003D090 File Offset: 0x0003B290
		protected virtual void dll()
		{
			if (this.VisionOrigin == null)
			{
				this.VisionOrigin = base.transform;
			}
			this.npc = base.GetComponentInParent<NPC>();
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.PlayerSpawned(Player.PlayerList[i]);
			}
			Player.onPlayerSpawned = (Action<Player>)Delegate.Combine(Player.onPlayerSpawned, new Action<Player>(this.PlayerSpawned));
			if (this.npc != null)
			{
				this.npc.Health.onDie.AddListener(new UnityAction(this.OnDie));
				this.npc.Health.onKnockedOut.AddListener(new UnityAction(this.OnDie));
			}
			base.InvokeRepeating("VisionUpdate", UnityEngine.Random.Range(0f, 0.1f), 0.1f);
		}

		// Token: 0x04000DFD RID: 3581
		public const float VISION_UPDATE_INTERVAL = 0.1f;

		// Token: 0x04000DFE RID: 3582
		public static float UniversalAttentivenessScale = 1f;

		// Token: 0x04000DFF RID: 3583
		public static float UniversalMemoryScale = 1f;

		// Token: 0x04000E00 RID: 3584
		public bool DisableSightUpdates;

		// Token: 0x04000E01 RID: 3585
		[Header("Frustrum Settings")]
		public float HorizontalFOV = 90f;

		// Token: 0x04000E02 RID: 3586
		public float VerticalFOV = 30f;

		// Token: 0x04000E03 RID: 3587
		public float Range = 40f;

		// Token: 0x04000E04 RID: 3588
		public float MinorWidth = 3f;

		// Token: 0x04000E05 RID: 3589
		public float MinorHeight = 1.5f;

		// Token: 0x04000E06 RID: 3590
		public Transform VisionOrigin;

		// Token: 0x04000E07 RID: 3591
		public bool DEBUG_FRUSTRUM;

		// Token: 0x04000E08 RID: 3592
		[Header("Vision Settings")]
		public bool VisionEnabled = true;

		// Token: 0x04000E09 RID: 3593
		public AnimationCurve VisionFalloff;

		// Token: 0x04000E0A RID: 3594
		public LayerMask VisibilityBlockingLayers;

		// Token: 0x04000E0B RID: 3595
		[Range(0f, 2f)]
		public float RangeMultiplier = 1f;

		// Token: 0x04000E0C RID: 3596
		[Header("Interest settings")]
		public List<VisionCone.StateContainer> StatesOfInterest = new List<VisionCone.StateContainer>();

		// Token: 0x04000E0D RID: 3597
		[Header("Notice Settings")]
		public float MinVisionDelta = 0.1f;

		// Token: 0x04000E0E RID: 3598
		public float Attentiveness = 1f;

		// Token: 0x04000E0F RID: 3599
		public float Memory = 1f;

		// Token: 0x04000E10 RID: 3600
		[Header("Worldspace Icons")]
		public bool WorldspaceIconsEnabled = true;

		// Token: 0x04000E11 RID: 3601
		public WorldspacePopup QuestionMarkPopup;

		// Token: 0x04000E12 RID: 3602
		public WorldspacePopup ExclamationPointPopup;

		// Token: 0x04000E13 RID: 3603
		public AudioSourceController ExclamationSound;

		// Token: 0x04000E14 RID: 3604
		public VisionCone.EventStateChange onVisionEventStarted;

		// Token: 0x04000E15 RID: 3605
		public VisionCone.EventStateChange onVisionEventHalf;

		// Token: 0x04000E16 RID: 3606
		public VisionCone.EventStateChange onVisionEventFull;

		// Token: 0x04000E17 RID: 3607
		public VisionCone.EventStateChange onVisionEventExpired;

		// Token: 0x04000E18 RID: 3608
		public Dictionary<Player, Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>> StateSettings = new Dictionary<Player, Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>>();

		// Token: 0x04000E19 RID: 3609
		protected List<VisionEvent> activeVisionEvents = new List<VisionEvent>();

		// Token: 0x04000E1A RID: 3610
		protected Dictionary<Player, VisionCone.PlayerSightData> playerSightDatas = new Dictionary<Player, VisionCone.PlayerSightData>();

		// Token: 0x04000E1B RID: 3611
		protected NPC npc;

		// Token: 0x04000E1C RID: 3612
		private bool generalCrimeResponseActive;

		// Token: 0x04000E1D RID: 3613
		private List<Player> sightedPlayers = new List<Player>();

		// Token: 0x04000E1E RID: 3614
		private bool dll_Excuted;

		// Token: 0x04000E1F RID: 3615
		private bool dll_Excuted;

		// Token: 0x0200028C RID: 652
		public enum EEventLevel
		{
			// Token: 0x04000E21 RID: 3617
			Start,
			// Token: 0x04000E22 RID: 3618
			Half,
			// Token: 0x04000E23 RID: 3619
			Full,
			// Token: 0x04000E24 RID: 3620
			Zero
		}

		// Token: 0x0200028D RID: 653
		[Serializable]
		public class StateContainer
		{
			// Token: 0x04000E25 RID: 3621
			public PlayerVisualState.EVisualState state;

			// Token: 0x04000E26 RID: 3622
			public bool Enabled;

			// Token: 0x04000E27 RID: 3623
			public float RequiredNoticeTime = 0.5f;
		}

		// Token: 0x0200028E RID: 654
		public class PlayerSightData
		{
			// Token: 0x04000E28 RID: 3624
			public Player Player;

			// Token: 0x04000E29 RID: 3625
			public float VisionDelta;

			// Token: 0x04000E2A RID: 3626
			public float TimeVisible;
		}

		// Token: 0x0200028F RID: 655
		// (Invoke) Token: 0x06000DBB RID: 3515
		public delegate void EventStateChange(VisionEventReceipt _event);
	}
}
