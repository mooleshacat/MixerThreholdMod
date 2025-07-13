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
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C61 RID: 3169
	public class ModularSwitch : NetworkBehaviour
	{
		// Token: 0x06005930 RID: 22832 RVA: 0x00178C28 File Offset: 0x00176E28
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Misc.ModularSwitch_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x00178C47 File Offset: 0x00176E47
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetIsOn(connection, this.isOn);
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x00178C60 File Offset: 0x00176E60
		protected virtual void LateUpdate()
		{
			if (this.isOn)
			{
				this.button.localEulerAngles = new Vector3(-7f, 0f, 0f);
				return;
			}
			this.button.localEulerAngles = new Vector3(7f, 0f, 0f);
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x00178CB4 File Offset: 0x00176EB4
		public void Hovered()
		{
			if (this.isOn)
			{
				this.intObj.SetMessage("Switch off");
				return;
			}
			this.intObj.SetMessage("Switch on");
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x00178CDF File Offset: 0x00176EDF
		public void Interacted()
		{
			if (this.isOn)
			{
				this.SendIsOn(false);
				return;
			}
			this.SendIsOn(true);
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x00178CF8 File Offset: 0x00176EF8
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendIsOn(bool isOn)
		{
			this.RpcWriter___Server_SendIsOn_1140765316(isOn);
			this.RpcLogic___SendIsOn_1140765316(isOn);
		}

		// Token: 0x06005936 RID: 22838 RVA: 0x00178D0E File Offset: 0x00176F0E
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetIsOn(NetworkConnection conn, bool isOn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOn_214505783(conn, isOn);
				this.RpcLogic___SetIsOn_214505783(conn, isOn);
			}
			else
			{
				this.RpcWriter___Target_SetIsOn_214505783(conn, isOn);
			}
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x00178D44 File Offset: 0x00176F44
		public void SwitchOn()
		{
			if (this.isOn)
			{
				return;
			}
			this.isOn = true;
			if (this.switchedOn != null)
			{
				this.switchedOn.Invoke();
			}
			if (this.onToggled != null)
			{
				this.onToggled(this.isOn);
			}
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				this.SwitchesToSyncWith[i].SwitchOn();
			}
			this.OnAudio.Play();
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x00178DC0 File Offset: 0x00176FC0
		public void SwitchOff()
		{
			if (!this.isOn)
			{
				return;
			}
			this.isOn = false;
			if (this.switchedOff != null)
			{
				this.switchedOff.Invoke();
			}
			if (this.onToggled != null)
			{
				this.onToggled(this.isOn);
			}
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				this.SwitchesToSyncWith[i].SwitchOff();
			}
			this.OffAudio.Play();
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x00178E50 File Offset: 0x00177050
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendIsOn_1140765316));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOn_214505783));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOn_214505783));
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x00178EB3 File Offset: 0x001770B3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Misc.ModularSwitchAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x00178EC6 File Offset: 0x001770C6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x00178ED4 File Offset: 0x001770D4
		private void RpcWriter___Server_SendIsOn_1140765316(bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x00178F7B File Offset: 0x0017717B
		private void RpcLogic___SendIsOn_1140765316(bool isOn)
		{
			this.SetIsOn(null, isOn);
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x00178F88 File Offset: 0x00177188
		private void RpcReader___Server_SendIsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendIsOn_1140765316(flag);
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x00178FC8 File Offset: 0x001771C8
		private void RpcWriter___Observers_SetIsOn_214505783(NetworkConnection conn, bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x0017907E File Offset: 0x0017727E
		private void RpcLogic___SetIsOn_214505783(NetworkConnection conn, bool isOn)
		{
			if (isOn)
			{
				this.SwitchOn();
				return;
			}
			this.SwitchOff();
		}

		// Token: 0x06005942 RID: 22850 RVA: 0x00179090 File Offset: 0x00177290
		private void RpcReader___Observers_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(null, flag);
		}

		// Token: 0x06005943 RID: 22851 RVA: 0x001790CC File Offset: 0x001772CC
		private void RpcWriter___Target_SetIsOn_214505783(NetworkConnection conn, bool isOn)
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
			writer.WriteBoolean(isOn);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005944 RID: 22852 RVA: 0x00179184 File Offset: 0x00177384
		private void RpcReader___Target_SetIsOn_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool flag = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOn_214505783(base.LocalConnection, flag);
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x001791BC File Offset: 0x001773BC
		protected virtual void dll()
		{
			for (int i = 0; i < this.SwitchesToSyncWith.Count; i++)
			{
				if (!this.SwitchesToSyncWith[i].SwitchesToSyncWith.Contains(this))
				{
					this.SwitchesToSyncWith[i].SwitchesToSyncWith.Add(this);
				}
			}
		}

		// Token: 0x04004158 RID: 16728
		public bool isOn;

		// Token: 0x04004159 RID: 16729
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x0400415A RID: 16730
		[SerializeField]
		protected Transform button;

		// Token: 0x0400415B RID: 16731
		public AudioSourceController OnAudio;

		// Token: 0x0400415C RID: 16732
		public AudioSourceController OffAudio;

		// Token: 0x0400415D RID: 16733
		[Header("Settings")]
		[SerializeField]
		protected List<ModularSwitch> SwitchesToSyncWith = new List<ModularSwitch>();

		// Token: 0x0400415E RID: 16734
		public ModularSwitch.ButtonChange onToggled;

		// Token: 0x0400415F RID: 16735
		public UnityEvent switchedOn;

		// Token: 0x04004160 RID: 16736
		public UnityEvent switchedOff;

		// Token: 0x04004161 RID: 16737
		private bool dll_Excuted;

		// Token: 0x04004162 RID: 16738
		private bool dll_Excuted;

		// Token: 0x02000C62 RID: 3170
		// (Invoke) Token: 0x06005947 RID: 22855
		public delegate void ButtonChange(bool isOn);
	}
}
