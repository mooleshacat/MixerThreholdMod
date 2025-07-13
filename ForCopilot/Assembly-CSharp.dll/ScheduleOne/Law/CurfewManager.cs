using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Law
{
	// Token: 0x02000603 RID: 1539
	public class CurfewManager : NetworkSingleton<CurfewManager>
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x00098296 File Offset: 0x00096496
		// (set) Token: 0x0600259B RID: 9627 RVA: 0x0009829E File Offset: 0x0009649E
		public bool IsEnabled { get; protected set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x000982A7 File Offset: 0x000964A7
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x000982AF File Offset: 0x000964AF
		public bool IsCurrentlyActive { get; protected set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000982B8 File Offset: 0x000964B8
		public bool IsCurrentlyActiveWithTolerance
		{
			get
			{
				return this.IsCurrentlyActive && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(2115, 500);
			}
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x000982D8 File Offset: 0x000964D8
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.Disable();
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x0009830C File Offset: 0x0009650C
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsEnabled)
			{
				this.Enable(connection);
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00098324 File Offset: 0x00096524
		[ObserversRpc]
		[TargetRpc]
		public void Enable(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_328543758(conn);
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00098350 File Offset: 0x00096550
		[ObserversRpc]
		public void Disable()
		{
			this.RpcWriter___Observers_Disable_2166136261();
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00098364 File Offset: 0x00096564
		private void MinPass()
		{
			if (!this.IsEnabled)
			{
				this.IsCurrentlyActive = false;
				return;
			}
			string text = "CURFEW TONIGHT\n9PM - 5AM";
			if (NetworkSingleton<TimeManager>.Instance.CurrentTime == 2030 && !this.warningPlayed)
			{
				this.warningPlayed = true;
				if (this.onCurfewWarning != null)
				{
					this.onCurfewWarning.Invoke();
				}
				if (NetworkSingleton<TimeManager>.Instance.ElapsedDays == 0 && this.onCurfewHint != null)
				{
					this.onCurfewHint.Invoke();
				}
				this.CurfewWarningSound.Play();
			}
			VMSBoard[] vmsboards;
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(2100, 500))
			{
				if (!this.IsCurrentlyActive)
				{
					if (!NetworkSingleton<TimeManager>.Instance.SleepInProgress && Singleton<LoadManager>.Instance.TimeSinceGameLoaded > 3f)
					{
						this.CurfewAlarmSound.Play();
					}
					this.IsCurrentlyActive = true;
				}
				text = "CURFEW ACTIVE\n UNTIL 5AM";
				vmsboards = this.VMSBoards;
				for (int i = 0; i < vmsboards.Length; i++)
				{
					vmsboards[i].SetText(text, new Color32(byte.MaxValue, 85, 60, byte.MaxValue));
				}
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(TimeManager.AddMinutesTo24HourTime(2100, -60), 2100))
			{
				this.warningPlayed = false;
				this.IsCurrentlyActive = false;
				text = "CURFEW SOON\n" + (TimeManager.GetMinSumFrom24HourTime(2100) - TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<TimeManager>.Instance.CurrentTime)).ToString() + " MINS";
				vmsboards = this.VMSBoards;
				for (int i = 0; i < vmsboards.Length; i++)
				{
					vmsboards[i].SetText(text);
				}
				return;
			}
			this.warningPlayed = false;
			this.IsCurrentlyActive = false;
			vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].SetText(text);
			}
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00098524 File Offset: 0x00096724
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_Enable_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_2166136261));
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x0009858D File Offset: 0x0009678D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Law.CurfewManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000985A6 File Offset: 0x000967A6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000985B4 File Offset: 0x000967B4
		private void RpcWriter___Observers_Enable_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00098660 File Offset: 0x00096860
		public void RpcLogic___Enable_328543758(NetworkConnection conn)
		{
			this.IsEnabled = true;
			if (this.onCurfewEnabled != null)
			{
				this.onCurfewEnabled.Invoke();
			}
			VMSBoard[] vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000986AC File Offset: 0x000968AC
		private void RpcReader___Observers_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(null);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000986D0 File Offset: 0x000968D0
		private void RpcWriter___Target_Enable_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x00098778 File Offset: 0x00096978
		private void RpcReader___Target_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(base.LocalConnection);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000987A0 File Offset: 0x000969A0
		private void RpcWriter___Observers_Disable_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x0009884C File Offset: 0x00096A4C
		public void RpcLogic___Disable_2166136261()
		{
			this.IsEnabled = false;
			if (this.onCurfewDisabled != null)
			{
				this.onCurfewDisabled.Invoke();
			}
			VMSBoard[] vmsboards = this.VMSBoards;
			for (int i = 0; i < vmsboards.Length; i++)
			{
				vmsboards[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00098898 File Offset: 0x00096A98
		private void RpcReader___Observers_Disable_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000988B8 File Offset: 0x00096AB8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001BBA RID: 7098
		public const int WARNING_TIME = 2030;

		// Token: 0x04001BBB RID: 7099
		public const int CURFEW_START_TIME = 2100;

		// Token: 0x04001BBC RID: 7100
		public const int CURFEW_END_TIME = 500;

		// Token: 0x04001BBF RID: 7103
		[Header("References")]
		public VMSBoard[] VMSBoards;

		// Token: 0x04001BC0 RID: 7104
		public AudioSourceController CurfewWarningSound;

		// Token: 0x04001BC1 RID: 7105
		public AudioSourceController CurfewAlarmSound;

		// Token: 0x04001BC2 RID: 7106
		public UnityEvent onCurfewEnabled;

		// Token: 0x04001BC3 RID: 7107
		public UnityEvent onCurfewDisabled;

		// Token: 0x04001BC4 RID: 7108
		public UnityEvent onCurfewHint;

		// Token: 0x04001BC5 RID: 7109
		public UnityEvent onCurfewWarning;

		// Token: 0x04001BC6 RID: 7110
		private bool warningPlayed;

		// Token: 0x04001BC7 RID: 7111
		private bool dll_Excuted;

		// Token: 0x04001BC8 RID: 7112
		private bool dll_Excuted;
	}
}
