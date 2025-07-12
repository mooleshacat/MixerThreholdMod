using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x02000653 RID: 1619
	public class NetworkedInteractableToggleable : NetworkBehaviour
	{
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000ADBF5 File Offset: 0x000ABDF5
		// (set) Token: 0x060029FD RID: 10749 RVA: 0x000ADBFD File Offset: 0x000ABDFD
		public bool IsActivated { get; private set; }

		// Token: 0x060029FE RID: 10750 RVA: 0x000ADC06 File Offset: 0x000ABE06
		public void Start()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000ADC40 File Offset: 0x000ABE40
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsActivated)
			{
				this.SetState(connection, true);
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000ADC5C File Offset: 0x000ABE5C
		public void Hovered()
		{
			if (Time.time - this.lastActivated < this.CoolDown)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.IntObj.SetMessage(this.IsActivated ? this.DeactivateMessage : this.ActivateMessage);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000ADCB7 File Offset: 0x000ABEB7
		public void Interacted()
		{
			this.SendToggle();
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000ADCBF File Offset: 0x000ABEBF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendToggle()
		{
			this.RpcWriter___Server_SendToggle_2166136261();
			this.RpcLogic___SendToggle_2166136261();
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000ADCD0 File Offset: 0x000ABED0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetState(NetworkConnection conn, bool activated)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetState_214505783(conn, activated);
				this.RpcLogic___SetState_214505783(conn, activated);
			}
			else
			{
				this.RpcWriter___Target_SetState_214505783(conn, activated);
			}
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000ADD11 File Offset: 0x000ABF11
		public void PoliceDetected()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsActivated)
			{
				this.SendToggle();
			}
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000ADD68 File Offset: 0x000ABF68
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendToggle_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetState_214505783));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetState_214505783));
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000ADDCB File Offset: 0x000ABFCB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Interaction.NetworkedInteractableToggleableAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000ADDDE File Offset: 0x000ABFDE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000ADDEC File Offset: 0x000ABFEC
		private void RpcWriter___Server_SendToggle_2166136261()
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000ADE86 File Offset: 0x000AC086
		public void RpcLogic___SendToggle_2166136261()
		{
			this.SetState(null, !this.IsActivated);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000ADE98 File Offset: 0x000AC098
		private void RpcReader___Server_SendToggle_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendToggle_2166136261();
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000ADEC8 File Offset: 0x000AC0C8
		private void RpcWriter___Observers_SetState_214505783(NetworkConnection conn, bool activated)
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
			writer.WriteBoolean(activated);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000ADF80 File Offset: 0x000AC180
		public void RpcLogic___SetState_214505783(NetworkConnection conn, bool activated)
		{
			if (this.IsActivated == activated)
			{
				return;
			}
			this.lastActivated = Time.time;
			this.IsActivated = !this.IsActivated;
			if (this.onToggle != null)
			{
				this.onToggle.Invoke();
			}
			if (this.IsActivated)
			{
				this.onActivate.Invoke();
				return;
			}
			this.onDeactivate.Invoke();
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000ADFE4 File Offset: 0x000AC1E4
		private void RpcReader___Observers_SetState_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool activated = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetState_214505783(null, activated);
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000AE020 File Offset: 0x000AC220
		private void RpcWriter___Target_SetState_214505783(NetworkConnection conn, bool activated)
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
			writer.WriteBoolean(activated);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000AE0D8 File Offset: 0x000AC2D8
		private void RpcReader___Target_SetState_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool activated = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetState_214505783(base.LocalConnection, activated);
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000ADDDE File Offset: 0x000ABFDE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001E9C RID: 7836
		public string ActivateMessage = "Activate";

		// Token: 0x04001E9D RID: 7837
		public string DeactivateMessage = "Deactivate";

		// Token: 0x04001E9E RID: 7838
		public float CoolDown;

		// Token: 0x04001E9F RID: 7839
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04001EA0 RID: 7840
		public UnityEvent onToggle = new UnityEvent();

		// Token: 0x04001EA1 RID: 7841
		public UnityEvent onActivate = new UnityEvent();

		// Token: 0x04001EA2 RID: 7842
		public UnityEvent onDeactivate = new UnityEvent();

		// Token: 0x04001EA3 RID: 7843
		private float lastActivated;

		// Token: 0x04001EA4 RID: 7844
		private bool dll_Excuted;

		// Token: 0x04001EA5 RID: 7845
		private bool dll_Excuted;
	}
}
