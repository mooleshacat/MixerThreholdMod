using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.ATM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Money
{
	// Token: 0x02000BDA RID: 3034
	public class ATM : NetworkBehaviour, IGUIDRegisterable, IGenericSaveable
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600507D RID: 20605 RVA: 0x00154A62 File Offset: 0x00152C62
		// (set) Token: 0x0600507E RID: 20606 RVA: 0x00154A6A File Offset: 0x00152C6A
		public bool IsBroken { get; protected set; }

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x00154A73 File Offset: 0x00152C73
		// (set) Token: 0x06005080 RID: 20608 RVA: 0x00154A7B File Offset: 0x00152C7B
		public int DaysUntilRepair { get; protected set; }

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06005081 RID: 20609 RVA: 0x00154A84 File Offset: 0x00152C84
		// (set) Token: 0x06005082 RID: 20610 RVA: 0x00154A8C File Offset: 0x00152C8C
		public bool isInUse { get; protected set; }

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06005083 RID: 20611 RVA: 0x00154A95 File Offset: 0x00152C95
		// (set) Token: 0x06005084 RID: 20612 RVA: 0x00154A9D File Offset: 0x00152C9D
		public Guid GUID { get; protected set; }

		// Token: 0x06005085 RID: 20613 RVA: 0x00154AA8 File Offset: 0x00152CA8
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x00154ACE File Offset: 0x00152CCE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Money.ATM_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x00154AE4 File Offset: 0x00152CE4
		protected virtual void Start()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.DayPass));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.DayPass));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onWeekPass = (Action)Delegate.Combine(instance.onWeekPass, new Action(this.WeekPass));
			((IGenericSaveable)this).InitializeSaveable();
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x00154B53 File Offset: 0x00152D53
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsBroken)
			{
				this.Break(connection);
			}
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x00154B6B File Offset: 0x00152D6B
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x00154B7C File Offset: 0x00152D7C
		public void DayPass()
		{
			if (InstanceFinder.IsServer && this.IsBroken)
			{
				int daysUntilRepair = this.DaysUntilRepair;
				this.DaysUntilRepair = daysUntilRepair - 1;
				if (this.DaysUntilRepair <= 0)
				{
					this.Repair();
				}
			}
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x00154BB7 File Offset: 0x00152DB7
		public void WeekPass()
		{
			ATM.WeeklyDepositSum = 0f;
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x00154BC3 File Offset: 0x00152DC3
		public void Hovered()
		{
			if (this.isInUse || this.IsBroken)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.intObj.SetMessage("Use ATM");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x00154BFE File Offset: 0x00152DFE
		public void Interacted()
		{
			if (this.isInUse || this.IsBroken)
			{
				return;
			}
			this.Enter();
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x00154C18 File Offset: 0x00152E18
		public void Enter()
		{
			this.isInUse = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, ATM.viewLerpTime);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.camPos.position, this.camPos.rotation, ATM.viewLerpTime, false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.interfaceATM.SetIsOpen(true);
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x00154C98 File Offset: 0x00152E98
		public void Exit()
		{
			this.isInUse = false;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(ATM.viewLerpTime);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(ATM.viewLerpTime, true, true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x00154CEC File Offset: 0x00152EEC
		private void Impacted(Impact impact)
		{
			if (this.IsBroken)
			{
				return;
			}
			if (impact.ImpactForce >= 165f)
			{
				this.SendBreak();
				if (impact.ImpactSource == Player.Local.NetworkObject)
				{
					Player.Local.VisualState.ApplyState("vandalism", PlayerVisualState.EVisualState.Vandalizing, 0f);
					Player.Local.VisualState.RemoveState("vandalism", 2f);
				}
				base.StartCoroutine(this.<Impacted>g__BreakRoutine|45_0());
			}
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x00154D6C File Offset: 0x00152F6C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBreak()
		{
			this.RpcWriter___Server_SendBreak_2166136261();
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x00154D7A File Offset: 0x00152F7A
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void Break(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Break_328543758(conn);
				this.RpcLogic___Break_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Break_328543758(conn);
			}
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x00154DA4 File Offset: 0x00152FA4
		[ObserversRpc]
		private void Repair()
		{
			this.RpcWriter___Observers_Repair_2166136261();
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x00154DAC File Offset: 0x00152FAC
		[ServerRpc(RequireOwnership = false)]
		private void DropCash()
		{
			this.RpcWriter___Server_DropCash_2166136261();
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x00154DC0 File Offset: 0x00152FC0
		public void Load(GenericSaveData data)
		{
			bool @bool = data.GetBool("broken", false);
			if (@bool)
			{
				this.Break(null);
			}
			this.IsBroken = @bool;
			this.DaysUntilRepair = data.GetInt("daysUntilRepair", 0);
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x00154E00 File Offset: 0x00153000
		public GenericSaveData GetSaveData()
		{
			GenericSaveData genericSaveData = new GenericSaveData(this.GUID.ToString());
			genericSaveData.Add("broken", this.IsBroken);
			genericSaveData.Add("daysUntilRepair", this.DaysUntilRepair);
			return genericSaveData;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x00154E71 File Offset: 0x00153071
		[CompilerGenerated]
		private IEnumerator <Impacted>g__BreakRoutine|45_0()
		{
			int cashDrop = UnityEngine.Random.Range(2, 9);
			int num;
			for (int i = 0; i < cashDrop; i = num + 1)
			{
				this.DropCash();
				yield return new WaitForSeconds(0.2f);
				num = i;
			}
			yield break;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x00154E80 File Offset: 0x00153080
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBreak_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Break_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_Break_328543758));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_Repair_2166136261));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_DropCash_2166136261));
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x00154F11 File Offset: 0x00153111
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Money.ATMAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x00154F24 File Offset: 0x00153124
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x00154F34 File Offset: 0x00153134
		private void RpcWriter___Server_SendBreak_2166136261()
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

		// Token: 0x0600509E RID: 20638 RVA: 0x00154FCE File Offset: 0x001531CE
		private void RpcLogic___SendBreak_2166136261()
		{
			this.DaysUntilRepair = 0;
			this.Break(null);
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x00154FE0 File Offset: 0x001531E0
		private void RpcReader___Server_SendBreak_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendBreak_2166136261();
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x00155010 File Offset: 0x00153210
		private void RpcWriter___Observers_Break_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001550B9 File Offset: 0x001532B9
		private void RpcLogic___Break_328543758(NetworkConnection conn)
		{
			if (this.IsBroken)
			{
				return;
			}
			this.IsBroken = true;
			UnityEvent unityEvent = this.onBreak;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x001550DC File Offset: 0x001532DC
		private void RpcReader___Observers_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Break_328543758(null);
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x00155108 File Offset: 0x00153308
		private void RpcWriter___Target_Break_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x001551B0 File Offset: 0x001533B0
		private void RpcReader___Target_Break_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Break_328543758(base.LocalConnection);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001551D8 File Offset: 0x001533D8
		private void RpcWriter___Observers_Repair_2166136261()
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
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x00155281 File Offset: 0x00153481
		private void RpcLogic___Repair_2166136261()
		{
			if (!this.IsBroken)
			{
				return;
			}
			this.IsBroken = false;
			UnityEvent unityEvent = this.onRepair;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x001552A4 File Offset: 0x001534A4
		private void RpcReader___Observers_Repair_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Repair_2166136261();
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x001552C4 File Offset: 0x001534C4
		private void RpcWriter___Server_DropCash_2166136261()
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
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x00155360 File Offset: 0x00153560
		private void RpcLogic___DropCash_2166136261()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CashPrefab.gameObject, this.CashSpawnPoint.position, this.CashSpawnPoint.rotation);
			gameObject.GetComponent<Rigidbody>().AddForce(this.CashSpawnPoint.forward * UnityEngine.Random.Range(1.5f, 2.5f), 2);
			gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.insideUnitSphere * 2f, 2);
			base.Spawn(gameObject.gameObject, null, default(Scene));
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001553F0 File Offset: 0x001535F0
		private void RpcReader___Server_DropCash_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DropCash_2166136261();
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x00155410 File Offset: 0x00153610
		private void dll()
		{
			PhysicsDamageable damageable = this.Damageable;
			damageable.onImpacted = (Action<Impact>)Delegate.Combine(damageable.onImpacted, new Action<Impact>(this.Impacted));
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x04003C7A RID: 15482
		public const bool DepositLimitEnabled = true;

		// Token: 0x04003C7B RID: 15483
		public const float WEEKLY_DEPOSIT_LIMIT = 10000f;

		// Token: 0x04003C7C RID: 15484
		public const float IMPACT_THRESHOLD_BREAK = 165f;

		// Token: 0x04003C7D RID: 15485
		public const int REPAIR_TIME_DAYS = 0;

		// Token: 0x04003C7E RID: 15486
		public const int MIN_CASH_DROP = 2;

		// Token: 0x04003C7F RID: 15487
		public const int MAX_CASH_DROP = 8;

		// Token: 0x04003C80 RID: 15488
		public static float WeeklyDepositSum = 0f;

		// Token: 0x04003C83 RID: 15491
		public CashPickup CashPrefab;

		// Token: 0x04003C84 RID: 15492
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04003C85 RID: 15493
		[SerializeField]
		protected Transform camPos;

		// Token: 0x04003C86 RID: 15494
		[SerializeField]
		protected ATMInterface interfaceATM;

		// Token: 0x04003C87 RID: 15495
		public Transform AccessPoint;

		// Token: 0x04003C88 RID: 15496
		public Transform CashSpawnPoint;

		// Token: 0x04003C89 RID: 15497
		public PhysicsDamageable Damageable;

		// Token: 0x04003C8A RID: 15498
		[Header("Settings")]
		public static float viewLerpTime = 0.15f;

		// Token: 0x04003C8D RID: 15501
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003C8E RID: 15502
		public UnityEvent onBreak;

		// Token: 0x04003C8F RID: 15503
		public UnityEvent onRepair;

		// Token: 0x04003C90 RID: 15504
		private bool dll_Excuted;

		// Token: 0x04003C91 RID: 15505
		private bool dll_Excuted;
	}
}
