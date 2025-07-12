using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Dragging
{
	// Token: 0x020006C0 RID: 1728
	public class DragManager : NetworkSingleton<DragManager>
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x000C7489 File Offset: 0x000C5689
		// (set) Token: 0x06002F78 RID: 12152 RVA: 0x000C7491 File Offset: 0x000C5691
		public Draggable CurrentDraggable { get; protected set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x000C749A File Offset: 0x000C569A
		public bool IsDragging
		{
			get
			{
				return this.CurrentDraggable != null;
			}
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000C74A8 File Offset: 0x000C56A8
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (Draggable draggable in this.AllDraggables)
			{
				if (draggable.InitialReplicationMode != Draggable.EInitialReplicationMode.Off && (draggable.InitialReplicationMode == Draggable.EInitialReplicationMode.Full || Vector3.Distance(draggable.initialPosition, draggable.transform.position) > 1f))
				{
					this.SetDraggableTransformData(connection, draggable.GUID.ToString(), draggable.transform.position, draggable.transform.rotation, draggable.Rigidbody.velocity);
				}
			}
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000C7568 File Offset: 0x000C5768
		public void Update()
		{
			if (this.IsDragging)
			{
				bool flag = false;
				LayerMask mask = default(LayerMask);
				mask.value = 1 << LayerMask.NameToLayer("Default");
				mask.value |= 1 << LayerMask.NameToLayer("NPC");
				RaycastHit raycastHit;
				if (Physics.Raycast(PlayerSingleton<PlayerMovement>.Instance.transform.position - PlayerSingleton<PlayerMovement>.Instance.Controller.height * Vector3.up * 0.5f, Vector3.down, ref raycastHit, 0.5f, mask))
				{
					flag = (raycastHit.collider.GetComponentInParent<Draggable>() == this.CurrentDraggable);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact) || !this.IsDraggingAllowed() || Vector3.Distance(this.GetTargetPosition(), this.CurrentDraggable.transform.position) > 1.5f || flag)
				{
					this.StopDragging(this.CurrentDraggable.Rigidbody.velocity);
					return;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.ThrowForce;
					float d = Mathf.Lerp(1f, Mathf.Sqrt(this.CurrentDraggable.Rigidbody.mass), this.MassInfluence);
					Vector3 velocity = this.CurrentDraggable.Rigidbody.velocity + a / d;
					this.CurrentDraggable.Rigidbody.velocity = velocity;
					this.lastThrownDraggable = this.CurrentDraggable;
					this.ThrowSound.transform.position = this.lastThrownDraggable.transform.position;
					float value = Mathf.Sqrt(this.CurrentDraggable.Rigidbody.mass / 30f);
					this.ThrowSound.VolumeMultiplier = Mathf.Clamp(value, 0.4f, 1f);
					this.ThrowSound.PitchMultiplier = Mathf.Lerp(0.6f, 0.4f, Mathf.Clamp01(value));
					this.ThrowSound.Play();
					this.StopDragging(velocity);
				}
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000C7790 File Offset: 0x000C5990
		public void FixedUpdate()
		{
			if (this.lastThrownDraggable != null)
			{
				this.ThrowSound.transform.position = this.lastThrownDraggable.transform.position;
			}
			if (this.IsDragging)
			{
				this.CurrentDraggable.ApplyDragForces(this.GetTargetPosition());
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000C77E4 File Offset: 0x000C59E4
		public bool IsDraggingAllowed()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				return false;
			}
			if (!Player.Local.Health.IsAlive)
			{
				return false;
			}
			if (Player.Local.IsSkating)
			{
				return false;
			}
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.ID == "trashgrabber")
				{
					return false;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.ID == "trashbag" && TrashBag_Equippable.IsHoveringTrash)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000C7879 File Offset: 0x000C5A79
		public void RegisterDraggable(Draggable draggable)
		{
			if (this.AllDraggables.Contains(draggable))
			{
				return;
			}
			this.AllDraggables.Add(draggable);
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000C7896 File Offset: 0x000C5A96
		public void Deregister(Draggable draggable)
		{
			if (this.AllDraggables.Contains(draggable))
			{
				this.AllDraggables.Remove(draggable);
			}
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000C78B4 File Offset: 0x000C5AB4
		public void StartDragging(Draggable draggable)
		{
			if (this.CurrentDraggable != null)
			{
				this.CurrentDraggable.StopDragging();
			}
			this.CurrentDraggable = draggable;
			this.lastHeldDraggable = draggable;
			draggable.StartDragging(Player.Local);
			this.SendDragger(draggable.GUID.ToString(), Player.Local.NetworkObject, draggable.transform.position);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000C7922 File Offset: 0x000C5B22
		[ServerRpc(RequireOwnership = false)]
		private void SendDragger(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.RpcWriter___Server_SendDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000C7938 File Offset: 0x000C5B38
		[ObserversRpc]
		private void SetDragger(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.RpcWriter___Observers_SetDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000C7958 File Offset: 0x000C5B58
		public void StopDragging(Vector3 velocity)
		{
			if (this.CurrentDraggable != null)
			{
				this.CurrentDraggable.StopDragging();
				this.SendDragger(this.CurrentDraggable.GUID.ToString(), null, this.CurrentDraggable.transform.position);
				this.SendDraggableTransformData(this.CurrentDraggable.GUID.ToString(), this.CurrentDraggable.transform.position, this.CurrentDraggable.transform.rotation, velocity);
				this.CurrentDraggable = null;
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000C79F8 File Offset: 0x000C5BF8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendDraggableTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			this.RpcWriter___Server_SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
			this.RpcLogic___SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000C7A28 File Offset: 0x000C5C28
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetDraggableTransformData(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
				this.RpcLogic___SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
			}
			else
			{
				this.RpcWriter___Target_SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000C7A8D File Offset: 0x000C5C8D
		private Vector3 GetTargetPosition()
		{
			return PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 1.25f * this.CurrentDraggable.HoldDistanceMultiplier;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000C7B2C File Offset: 0x000C5D2C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendDragger_807933219));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetDragger_807933219));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendDraggableTransformData_4062762274));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetDraggableTransformData_3831223955));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetDraggableTransformData_3831223955));
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000C7BC3 File Offset: 0x000C5DC3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000C7BDC File Offset: 0x000C5DDC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000C7BEC File Offset: 0x000C5DEC
		private void RpcWriter___Server_SendDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
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
			writer.WriteString(draggableGUID);
			writer.WriteNetworkObject(dragger);
			writer.WriteVector3(position);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000C7CAD File Offset: 0x000C5EAD
		private void RpcLogic___SendDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.SetDragger(draggableGUID, dragger, position);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000C7CB8 File Offset: 0x000C5EB8
		private void RpcReader___Server_SendDragger_807933219(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string draggableGUID = PooledReader0.ReadString();
			NetworkObject dragger = PooledReader0.ReadNetworkObject();
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000C7D0C File Offset: 0x000C5F0C
		private void RpcWriter___Observers_SetDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
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
			writer.WriteString(draggableGUID);
			writer.WriteNetworkObject(dragger);
			writer.WriteVector3(position);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000C7DDC File Offset: 0x000C5FDC
		private void RpcLogic___SetDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			Draggable @object = GUIDManager.GetObject<Draggable>(new Guid(draggableGUID));
			Player x = (dragger != null) ? dragger.GetComponent<Player>() : null;
			if (@object != null)
			{
				if (this.CurrentDraggable != @object && this.lastHeldDraggable != @object)
				{
					@object.Rigidbody.position = position;
				}
				if (dragger != null)
				{
					if (x != null)
					{
						@object.StartDragging(dragger.GetComponent<Player>());
						return;
					}
				}
				else
				{
					@object.StopDragging();
				}
			}
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000C7E60 File Offset: 0x000C6060
		private void RpcReader___Observers_SetDragger_807933219(PooledReader PooledReader0, Channel channel)
		{
			string draggableGUID = PooledReader0.ReadString();
			NetworkObject dragger = PooledReader0.ReadNetworkObject();
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000C7EB4 File Offset: 0x000C60B4
		private void RpcWriter___Server_SendDraggableTransformData_4062762274(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(velocity);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000C7F87 File Offset: 0x000C6187
		private void RpcLogic___SendDraggableTransformData_4062762274(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			this.SetDraggableTransformData(null, guid, position, rotation, velocity);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000C7F98 File Offset: 0x000C6198
		private void RpcReader___Server_SendDraggableTransformData_4062762274(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000C8010 File Offset: 0x000C6210
		private void RpcWriter___Observers_SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(velocity);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000C80F4 File Offset: 0x000C62F4
		private void RpcLogic___SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			Draggable @object = GUIDManager.GetObject<Draggable>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find draggable with GUID " + guid, null);
			}
			if (@object == this.lastThrownDraggable)
			{
				return;
			}
			if (@object == this.lastHeldDraggable)
			{
				return;
			}
			if (@object != null)
			{
				@object.Rigidbody.position = position;
				@object.Rigidbody.rotation = rotation;
				@object.Rigidbody.velocity = velocity;
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000C8174 File Offset: 0x000C6374
		private void RpcReader___Observers_SetDraggableTransformData_3831223955(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetDraggableTransformData_3831223955(null, guid, position, rotation, velocity);
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000C81E8 File Offset: 0x000C63E8
		private void RpcWriter___Target_SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteVector3(velocity);
			base.SendTargetRpc(4U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000C82CC File Offset: 0x000C64CC
		private void RpcReader___Target_SetDraggableTransformData_3831223955(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetDraggableTransformData_3831223955(base.LocalConnection, guid, position, rotation, velocity);
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000C833B File Offset: 0x000C653B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002167 RID: 8551
		public const float DRAGGABLE_OFFSET = 1.25f;

		// Token: 0x04002168 RID: 8552
		public AudioSourceController ThrowSound;

		// Token: 0x04002169 RID: 8553
		[Header("Settings")]
		public float DragForce = 10f;

		// Token: 0x0400216A RID: 8554
		public float DampingFactor = 0.5f;

		// Token: 0x0400216B RID: 8555
		public float TorqueForce = 10f;

		// Token: 0x0400216C RID: 8556
		public float TorqueDampingFactor = 0.5f;

		// Token: 0x0400216D RID: 8557
		public float ThrowForce = 10f;

		// Token: 0x0400216E RID: 8558
		public float MassInfluence = 0.6f;

		// Token: 0x04002170 RID: 8560
		private List<Draggable> AllDraggables = new List<Draggable>();

		// Token: 0x04002171 RID: 8561
		private Draggable lastThrownDraggable;

		// Token: 0x04002172 RID: 8562
		private Draggable lastHeldDraggable;

		// Token: 0x04002173 RID: 8563
		private bool dll_Excuted;

		// Token: 0x04002174 RID: 8564
		private bool dll_Excuted;
	}
}
