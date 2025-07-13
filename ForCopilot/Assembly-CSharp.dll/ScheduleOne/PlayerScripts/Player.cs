using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FishySteamworks;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Intro;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts.Health;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.Skating;
using ScheduleOne.Stealth;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.UI.MainMenu;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000614 RID: 1556
	public class Player : NetworkBehaviour, ISaveable, IDamageable
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x00099AD8 File Offset: 0x00097CD8
		public bool IsLocalPlayer
		{
			get
			{
				return base.IsOwner;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x00099AE0 File Offset: 0x00097CE0
		// (set) Token: 0x0600260A RID: 9738 RVA: 0x00099AE8 File Offset: 0x00097CE8
		public string PlayerName
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerName>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<PlayerName>k__BackingField(value, true);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x00099AF2 File Offset: 0x00097CF2
		// (set) Token: 0x0600260C RID: 9740 RVA: 0x00099AFA File Offset: 0x00097CFA
		public string PlayerCode
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerCode>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<PlayerCode>k__BackingField(value, true);
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x00099B04 File Offset: 0x00097D04
		// (set) Token: 0x0600260E RID: 9742 RVA: 0x00099B0C File Offset: 0x00097D0C
		public NetworkObject CurrentVehicle
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentVehicle>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			set
			{
				this.RpcWriter___Server_set_CurrentVehicle_3323014238(value);
				this.RpcLogic___set_CurrentVehicle_3323014238(value);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x00099B22 File Offset: 0x00097D22
		// (set) Token: 0x06002610 RID: 9744 RVA: 0x00099B2A File Offset: 0x00097D2A
		public float TimeSinceVehicleExit { get; protected set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x00099B33 File Offset: 0x00097D33
		// (set) Token: 0x06002612 RID: 9746 RVA: 0x00099B3B File Offset: 0x00097D3B
		public bool Crouched { get; private set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x00099B44 File Offset: 0x00097D44
		// (set) Token: 0x06002614 RID: 9748 RVA: 0x00099B4C File Offset: 0x00097D4C
		public NetworkObject CurrentBed
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentBed>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CurrentBed_3323014238(value);
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x00099B58 File Offset: 0x00097D58
		// (set) Token: 0x06002616 RID: 9750 RVA: 0x00099B60 File Offset: 0x00097D60
		public bool IsReadyToSleep
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<IsReadyToSleep>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<IsReadyToSleep>k__BackingField(value, true);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x00099B6A File Offset: 0x00097D6A
		// (set) Token: 0x06002618 RID: 9752 RVA: 0x00099B72 File Offset: 0x00097D72
		public bool IsSkating
		{
			[CompilerGenerated]
			get
			{
				return this.<IsSkating>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_IsSkating_1140765316(value);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x00099B7E File Offset: 0x00097D7E
		// (set) Token: 0x0600261A RID: 9754 RVA: 0x00099B86 File Offset: 0x00097D86
		public Skateboard ActiveSkateboard { get; private set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x00099B8F File Offset: 0x00097D8F
		// (set) Token: 0x0600261C RID: 9756 RVA: 0x00099B97 File Offset: 0x00097D97
		public bool IsSleeping { get; protected set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600261D RID: 9757 RVA: 0x00099BA0 File Offset: 0x00097DA0
		// (set) Token: 0x0600261E RID: 9758 RVA: 0x00099BA8 File Offset: 0x00097DA8
		public bool IsRagdolled { get; protected set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x00099BB1 File Offset: 0x00097DB1
		// (set) Token: 0x06002620 RID: 9760 RVA: 0x00099BB9 File Offset: 0x00097DB9
		public bool IsArrested { get; protected set; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x00099BC2 File Offset: 0x00097DC2
		// (set) Token: 0x06002622 RID: 9762 RVA: 0x00099BCA File Offset: 0x00097DCA
		public bool IsTased { get; protected set; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002623 RID: 9763 RVA: 0x00099BD3 File Offset: 0x00097DD3
		// (set) Token: 0x06002624 RID: 9764 RVA: 0x00099BDB File Offset: 0x00097DDB
		public bool IsUnconscious { get; protected set; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x00099BE4 File Offset: 0x00097DE4
		// (set) Token: 0x06002626 RID: 9766 RVA: 0x00099BEC File Offset: 0x00097DEC
		public float Scale { get; private set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x00099BF5 File Offset: 0x00097DF5
		// (set) Token: 0x06002628 RID: 9768 RVA: 0x00099BFD File Offset: 0x00097DFD
		public Property CurrentProperty { get; protected set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x00099C06 File Offset: 0x00097E06
		// (set) Token: 0x0600262A RID: 9770 RVA: 0x00099C0E File Offset: 0x00097E0E
		public Property LastVisitedProperty { get; protected set; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x00099C17 File Offset: 0x00097E17
		// (set) Token: 0x0600262C RID: 9772 RVA: 0x00099C1F File Offset: 0x00097E1F
		public Business CurrentBusiness { get; protected set; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x00099C28 File Offset: 0x00097E28
		public Vector3 PlayerBasePosition
		{
			get
			{
				return base.transform.position - base.transform.up * (this.CharacterController.height / 2f);
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x00099C5B File Offset: 0x00097E5B
		// (set) Token: 0x0600262F RID: 9775 RVA: 0x00099C63 File Offset: 0x00097E63
		public Vector3 CameraPosition
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CameraPosition>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CameraPosition_4276783012(value);
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x00099C6F File Offset: 0x00097E6F
		// (set) Token: 0x06002631 RID: 9777 RVA: 0x00099C77 File Offset: 0x00097E77
		public Quaternion CameraRotation
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CameraRotation>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CameraRotation_3429297120(value);
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x00099C83 File Offset: 0x00097E83
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x00099C8B File Offset: 0x00097E8B
		public BasicAvatarSettings CurrentAvatarSettings { get; protected set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x00099C94 File Offset: 0x00097E94
		// (set) Token: 0x06002635 RID: 9781 RVA: 0x00099C9C File Offset: 0x00097E9C
		public ProductItemInstance ConsumedProduct { get; private set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x00099CA5 File Offset: 0x00097EA5
		// (set) Token: 0x06002637 RID: 9783 RVA: 0x00099CAD File Offset: 0x00097EAD
		public int TimeSinceProductConsumed { get; private set; }

		// Token: 0x06002638 RID: 9784 RVA: 0x00099CB6 File Offset: 0x00097EB6
		[Button]
		public void LoadDebugAvatarSettings()
		{
			this.SetAppearance(this.DebugAvatarSettings, false);
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x00099CC5 File Offset: 0x00097EC5
		public string SaveFolderName
		{
			get
			{
				if (InstanceFinder.IsServer && base.IsOwner)
				{
					return "Player_0";
				}
				return "Player_" + this.PlayerCode;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x00099CEC File Offset: 0x00097EEC
		public string SaveFileName
		{
			get
			{
				return "Player";
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600263B RID: 9787 RVA: 0x00099CF3 File Offset: 0x00097EF3
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x00099CFB File Offset: 0x00097EFB
		// (set) Token: 0x0600263E RID: 9790 RVA: 0x00099D03 File Offset: 0x00097F03
		public List<string> LocalExtraFiles { get; set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x00099D0C File Offset: 0x00097F0C
		// (set) Token: 0x06002640 RID: 9792 RVA: 0x00099D14 File Offset: 0x00097F14
		public List<string> LocalExtraFolders { get; set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x00099D1D File Offset: 0x00097F1D
		// (set) Token: 0x06002642 RID: 9794 RVA: 0x00099D25 File Offset: 0x00097F25
		public bool HasChanged { get; set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x00099D2E File Offset: 0x00097F2E
		// (set) Token: 0x06002644 RID: 9796 RVA: 0x00099D36 File Offset: 0x00097F36
		public bool avatarVisibleToLocalPlayer { get; private set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x00099D3F File Offset: 0x00097F3F
		// (set) Token: 0x06002646 RID: 9798 RVA: 0x00099D47 File Offset: 0x00097F47
		public bool playerDataRetrieveReturned { get; private set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x00099D50 File Offset: 0x00097F50
		// (set) Token: 0x06002648 RID: 9800 RVA: 0x00099D58 File Offset: 0x00097F58
		public bool playerSaveRequestReturned { get; private set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x00099D61 File Offset: 0x00097F61
		// (set) Token: 0x0600264A RID: 9802 RVA: 0x00099D69 File Offset: 0x00097F69
		public bool PlayerInitializedOverNetwork { get; private set; }

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x00099D72 File Offset: 0x00097F72
		// (set) Token: 0x0600264C RID: 9804 RVA: 0x00099D7A File Offset: 0x00097F7A
		public bool Paranoid { get; set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600264D RID: 9805 RVA: 0x00099D83 File Offset: 0x00097F83
		// (set) Token: 0x0600264E RID: 9806 RVA: 0x00099D8B File Offset: 0x00097F8B
		public bool Sneaky { get; set; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x00099D94 File Offset: 0x00097F94
		// (set) Token: 0x06002650 RID: 9808 RVA: 0x00099D9C File Offset: 0x00097F9C
		public bool Disoriented { get; set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x00099DA5 File Offset: 0x00097FA5
		// (set) Token: 0x06002652 RID: 9810 RVA: 0x00099DAD File Offset: 0x00097FAD
		public bool Seizure { get; set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x00099DB6 File Offset: 0x00097FB6
		// (set) Token: 0x06002654 RID: 9812 RVA: 0x00099DBE File Offset: 0x00097FBE
		public bool Slippery { get; set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x00099DC7 File Offset: 0x00097FC7
		// (set) Token: 0x06002656 RID: 9814 RVA: 0x00099DCF File Offset: 0x00097FCF
		public bool Schizophrenic { get; set; }

		// Token: 0x06002657 RID: 9815 RVA: 0x00099DD8 File Offset: 0x00097FD8
		public static Player GetPlayer(NetworkConnection conn)
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].Connection == conn)
				{
					return Player.PlayerList[i];
				}
			}
			return null;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x00099E20 File Offset: 0x00098020
		public static Player GetRandomPlayer(bool excludeArrestedOrDead = true, bool excludeSleeping = true)
		{
			List<Player> list = new List<Player>();
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if ((!excludeArrestedOrDead || (!Player.PlayerList[i].IsArrested && Player.PlayerList[i].Health.IsAlive)) && (!excludeSleeping || !Player.PlayerList[i].IsSleeping))
				{
					list.Add(Player.PlayerList[i]);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			return list[index];
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x00099EB8 File Offset: 0x000980B8
		public static Player GetPlayer(string playerCode)
		{
			return Player.PlayerList.Find((Player x) => x.PlayerCode == playerCode);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x00099EE8 File Offset: 0x000980E8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.Player_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00099F08 File Offset: 0x00098108
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onTick = (Action)Delegate.Combine(instance3.onTick, new Action(this.Tick));
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x00099F8C File Offset: 0x0009818C
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onTick = (Action)Delegate.Remove(instance2.onTick, new Action(this.Tick));
			}
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance3 = NetworkSingleton<MoneyManager>.Instance;
				instance3.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance3.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0009A01C File Offset: 0x0009821C
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.Connection = base.Owner;
			if (base.IsOwner)
			{
				if (Application.isEditor)
				{
					this.LoadDebugAvatarSettings();
				}
				this.LocalGameObject.gameObject.SetActive(true);
				Player.Local = this;
				if (Player.onLocalPlayerSpawned != null)
				{
					Player.onLocalPlayerSpawned();
				}
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
				if (Singleton<Lobby>.Instance.IsInLobby && !Singleton<Lobby>.Instance.IsHost)
				{
					InstanceFinder.TransportManager.GetTransport<FishySteamworks>().OnClientConnectionState += this.ClientConnectionStateChanged;
				}
				this.FootstepDetector.enabled = false;
				this.PoI.SetMainText("You");
				if (this.PoI.UI != null)
				{
					this.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
				this.NameLabel.gameObject.SetActive(false);
				if (base.IsHost)
				{
					if (Singleton<LoadManager>.Instance.IsGameLoaded)
					{
						this.PlayerLoaded();
					}
					else
					{
						Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.PlayerLoaded));
					}
				}
				CSteamID csteamID = CSteamID.Nil;
				if (SteamManager.Initialized)
				{
					csteamID = SteamUser.GetSteamID();
					this.PlayerName = SteamFriends.GetPersonaName();
				}
				this.SendPlayerNameData(this.PlayerName, csteamID.m_SteamID);
				if (!InstanceFinder.IsServer)
				{
					this.RequestPlayerData(this.PlayerCode);
				}
			}
			else
			{
				this.LocalFootstepDetector.enabled = false;
				this.CapCol.isTrigger = true;
				base.gameObject.name = this.PlayerName + " (" + this.PlayerCode + ")";
				this.PoI.SetMainText(this.PlayerName);
			}
			if (base.IsOwner || InstanceFinder.IsServer || (Singleton<Lobby>.Instance.IsInLobby && Singleton<Lobby>.Instance.IsHost))
			{
				this.CreatePlayerVariables();
			}
			if (Player.onPlayerSpawned != null)
			{
				Player.onPlayerSpawned(this);
			}
			Console.Log("Player spawned (" + this.PlayerName + ")", null);
			this.CrimeData.RecordLastKnownPosition(false);
			if (!base.IsOwner && this.CurrentVehicle != null)
			{
				Console.Log("This player is in a vehicle!", null);
				this.EnterVehicle(this.CurrentVehicle.GetComponent<LandVehicle>());
			}
			Player.PlayerList.Add(this);
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0009A28C File Offset: 0x0009848C
		private void PlayerLoaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.PlayerLoaded));
			if (!base.IsOwner)
			{
				return;
			}
			if (this.PoI != null)
			{
				this.PoI.SetMainText("You");
				if (this.PoI.UI != null)
				{
					this.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
			}
			this.MarkPlayerInitialized();
			if (!this.HasCompletedIntro && !Singleton<LoadManager>.Instance.DebugMode && SceneManager.GetActiveScene().name == "Main")
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(NetworkSingleton<GameManager>.Instance.SpawnPoint.position);
				base.transform.forward = NetworkSingleton<GameManager>.Instance.SpawnPoint.forward;
				Console.Log("Player has not completed intro; playing intro", null);
				Singleton<IntroManager>.Instance.Play();
				Singleton<CharacterCreator>.Instance.onComplete.AddListener(new UnityAction<BasicAvatarSettings>(this.MarkIntroCompleted));
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0009A3A0 File Offset: 0x000985A0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.Owner != connection)
			{
				PlayerData data = new PlayerData(this.PlayerCode, base.transform.position, base.transform.eulerAngles.y, this.HasCompletedIntro);
				string empty = string.Empty;
				string appearanceString = (this.CurrentAvatarSettings != null) ? this.CurrentAvatarSettings.GetJson(true) : string.Empty;
				string clothingString = this.GetClothingString();
				if (this.Crouched)
				{
					this.ReceiveCrouched(connection, true);
				}
				this.ReceivePlayerData(connection, data, empty, appearanceString, clothingString, null);
				this.ReceivePlayerNameData(connection, this.PlayerName, this.PlayerCode);
			}
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x0009A450 File Offset: 0x00098650
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void RequestSavePlayer()
		{
			this.RpcWriter___Server_RequestSavePlayer_2166136261();
			this.RpcLogic___RequestSavePlayer_2166136261();
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0009A45E File Offset: 0x0009865E
		[ObserversRpc]
		[TargetRpc]
		private void ReturnSaveRequest(NetworkConnection conn, bool successful)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReturnSaveRequest_214505783(conn, successful);
			}
			else
			{
				this.RpcWriter___Target_ReturnSaveRequest_214505783(conn, successful);
			}
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0009A488 File Offset: 0x00098688
		[ObserversRpc(RunLocally = true)]
		public void HostExitedGame()
		{
			this.RpcWriter___Observers_HostExitedGame_2166136261();
			this.RpcLogic___HostExitedGame_2166136261();
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0009A4A1 File Offset: 0x000986A1
		private void ClientConnectionStateChanged(ClientConnectionStateArgs args)
		{
			Console.Log("Client connection state changed: " + args.ConnectionState.ToString(), null);
			if (args.ConnectionState == 3 || args.ConnectionState == null)
			{
				this.HostExitedGame();
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0009A4DC File Offset: 0x000986DC
		[ServerRpc(RunLocally = true)]
		public void SendPlayerNameData(string playerName, ulong id)
		{
			this.RpcWriter___Server_SendPlayerNameData_586648380(playerName, id);
			this.RpcLogic___SendPlayerNameData_586648380(playerName, id);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0009A508 File Offset: 0x00098708
		[ServerRpc(RequireOwnership = false)]
		public void RequestPlayerData(string playerCode)
		{
			this.RpcWriter___Server_RequestPlayerData_3615296227(playerCode);
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0009A51F File Offset: 0x0009871F
		[ServerRpc(RunLocally = true)]
		public void MarkPlayerInitialized()
		{
			this.RpcWriter___Server_MarkPlayerInitialized_2166136261();
			this.RpcLogic___MarkPlayerInitialized_2166136261();
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0009A530 File Offset: 0x00098730
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ReceivePlayerData(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
				this.RpcLogic___ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0009A5A4 File Offset: 0x000987A4
		public void SetGravityMultiplier(float multiplier)
		{
			if (base.IsOwner)
			{
				PlayerMovement.GravityMultiplier = multiplier;
			}
			foreach (ConstantForce constantForce in this.ragdollForceComponents)
			{
				constantForce.force = Physics.gravity * multiplier * constantForce.GetComponent<Rigidbody>().mass;
			}
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0009A620 File Offset: 0x00098820
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerNameData(NetworkConnection conn, string playerName, string id)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerNameData_3895153758(conn, playerName, id);
				this.RpcLogic___ReceivePlayerNameData_3895153758(conn, playerName, id);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerNameData_3895153758(conn, playerName, id);
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0009A66D File Offset: 0x0009886D
		public void SendFlashlightOn(bool on)
		{
			this.SendFlashlightOnNetworked(on);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0009A676 File Offset: 0x00098876
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendFlashlightOnNetworked(bool on)
		{
			this.RpcWriter___Server_SendFlashlightOnNetworked_1140765316(on);
			this.RpcLogic___SendFlashlightOnNetworked_1140765316(on);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0009A68C File Offset: 0x0009888C
		[ObserversRpc(RunLocally = true)]
		private void SetFlashlightOn(bool on)
		{
			this.RpcWriter___Observers_SetFlashlightOn_1140765316(on);
			this.RpcLogic___SetFlashlightOn_1140765316(on);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0009A6A2 File Offset: 0x000988A2
		public override void OnStopClient()
		{
			base.OnStopClient();
			Player.PlayerList.Remove(this);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0009A6B6 File Offset: 0x000988B6
		public override void OnStartServer()
		{
			base.OnStartServer();
			base.ServerManager.Objects.OnPreDestroyClientObjects += this.PreDestroyClientObjects;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0009A6DC File Offset: 0x000988DC
		protected virtual void Update()
		{
			this.HasChanged = true;
			if (this.CurrentVehicle != null)
			{
				this.TimeSinceVehicleExit = 0f;
			}
			else
			{
				this.TimeSinceVehicleExit += Time.deltaTime;
			}
			if (!base.IsOwner)
			{
				return;
			}
			if (base.transform.position.y < -20f)
			{
				float y = 0f;
				if (MapHeightSampler.Sample(base.transform.position.x, out y, base.transform.position.z))
				{
					PlayerSingleton<PlayerMovement>.Instance.Teleport(new Vector3(base.transform.position.x, y, base.transform.position.z));
				}
				else
				{
					PlayerSingleton<PlayerMovement>.Instance.Teleport(MapHeightSampler.ResetPosition);
				}
			}
			if (this.ActiveSkateboard != null)
			{
				this.SetCapsuleColliderHeight(1f - this.ActiveSkateboard.Animation.CurrentCrouchShift * 0.3f);
			}
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Player_In_Vehicle", (this.CurrentVehicle != null).ToString(), true);
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void MinPass()
		{
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0009A808 File Offset: 0x00098A08
		protected virtual void Tick()
		{
			if (this.ConsumedProduct != null)
			{
				int timeSinceProductConsumed = this.TimeSinceProductConsumed;
				this.TimeSinceProductConsumed = timeSinceProductConsumed + 1;
				if (this.TimeSinceProductConsumed >= (this.ConsumedProduct.Definition as ProductDefinition).EffectsDuration)
				{
					this.ClearProduct();
				}
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0009A850 File Offset: 0x00098A50
		protected virtual void LateUpdate()
		{
			if (base.IsOwner)
			{
				this.CameraPosition = PlayerSingleton<PlayerCamera>.Instance.transform.position;
				this.CameraRotation = PlayerSingleton<PlayerCamera>.Instance.transform.rotation;
			}
			if (this.Seizure)
			{
				for (int i = 0; i < this.Avatar.RagdollRBs.Length; i++)
				{
					if (this.seizureRotations.Count <= this.Avatar.RagdollRBs.Length)
					{
						this.seizureRotations.Add(Quaternion.identity);
					}
					this.seizureRotations[i] = Quaternion.Lerp(this.seizureRotations[i], Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 30f), Time.deltaTime * 10f);
					this.Avatar.RagdollRBs[i].transform.localRotation *= this.seizureRotations[i];
				}
			}
			this.MimicCamera.transform.position = this.CameraPosition;
			this.MimicCamera.transform.rotation = this.CameraRotation;
			this.EyePosition = this.Avatar.Eyes.transform.position;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0009A994 File Offset: 0x00098B94
		private void RecalculateCurrentProperty()
		{
			Property property = (from x in Property.Properties
			orderby Vector3.Distance(x.BoundingBox.transform.position, this.Avatar.CenterPoint)
			select x).FirstOrDefault<Property>();
			Business business = (from x in Business.Businesses
			orderby Vector3.Distance(x.BoundingBox.transform.position, this.Avatar.CenterPoint)
			select x).FirstOrDefault<Business>();
			if (property == null)
			{
				this.CurrentProperty = null;
			}
			else if (property.DoBoundsContainPoint(this.Avatar.CenterPoint))
			{
				this.CurrentProperty = property;
				this.LastVisitedProperty = this.CurrentProperty;
			}
			else
			{
				this.CurrentProperty = null;
			}
			if (business == null)
			{
				this.CurrentBusiness = null;
				return;
			}
			if (business.DoBoundsContainPoint(this.Avatar.CenterPoint))
			{
				this.CurrentBusiness = business;
				return;
			}
			this.CurrentBusiness = null;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0009AA4D File Offset: 0x00098C4D
		private void FixedUpdate()
		{
			this.ApplyMovementVisuals();
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0009AA58 File Offset: 0x00098C58
		private void ApplyMovementVisuals()
		{
			if (this.IsSkating)
			{
				this.Anim.SetTimeAirborne(0f);
				this.Anim.SetGrounded(true);
				this.Anim.SetDirection(0f);
				this.Anim.SetStrafe(0f);
				return;
			}
			bool isGrounded = this.GetIsGrounded();
			if (isGrounded)
			{
				this.timeAirborne = 0f;
			}
			else
			{
				this.timeAirborne += Time.deltaTime;
			}
			this.Anim.SetTimeAirborne(this.timeAirborne);
			if (this.Crouched)
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 0f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			else
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 1f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			this.Anim.SetGrounded(isGrounded);
			this.Anim.SetCrouched(this.Crouched);
			this.Avatar.transform.localPosition = new Vector3(0f, Mathf.Lerp(this.AvatarOffset_Crouched, this.AvatarOffset_Standing, this.standingScale), 0f);
			Vector3 vector = base.transform.InverseTransformVector(this.VelocityCalculator.Velocity) / (PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier);
			if (this.Crouched)
			{
				this.Anim.SetDirection(this.CrouchWalkMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
				this.Anim.SetStrafe(this.CrouchWalkMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
				return;
			}
			this.Anim.SetDirection(this.WalkingMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
			this.Anim.SetStrafe(this.WalkingMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x0009AC67 File Offset: 0x00098E67
		public void SetVisible(bool vis, bool network = false)
		{
			this.Avatar.SetVisible(vis);
			this.CapCol.enabled = vis;
			if (network)
			{
				this.SetVisible_Networked(vis);
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0009AC8B File Offset: 0x00098E8B
		[ObserversRpc]
		public void PlayJumpAnimation()
		{
			this.RpcWriter___Observers_PlayJumpAnimation_2166136261();
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0009AC94 File Offset: 0x00098E94
		public bool GetIsGrounded()
		{
			float num = PlayerMovement.StandingControllerHeight * (this.Crouched ? PlayerMovement.CrouchHeightMultiplier : 1f) / 2f + 0.1f;
			RaycastHit raycastHit;
			return Physics.SphereCast(base.transform.position, PlayerMovement.ControllerRadius * 0.75f, Vector3.down, ref raycastHit, num, this.GroundDetectionMask);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0009ACF6 File Offset: 0x00098EF6
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendCrouched(bool crouched)
		{
			this.RpcWriter___Server_SendCrouched_1140765316(crouched);
			this.RpcLogic___SendCrouched_1140765316(crouched);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0009AD0C File Offset: 0x00098F0C
		public void SetCrouchedLocal(bool crouched)
		{
			this.Crouched = crouched;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0009AD15 File Offset: 0x00098F15
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void ReceiveCrouched(NetworkConnection conn, bool crouched)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveCrouched_214505783(conn, crouched);
				this.RpcLogic___ReceiveCrouched_214505783(conn, crouched);
			}
			else
			{
				this.RpcWriter___Target_ReceiveCrouched_214505783(conn, crouched);
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0009AD4B File Offset: 0x00098F4B
		[ServerRpc(RunLocally = true)]
		public void SendAvatarSettings(AvatarSettings settings)
		{
			this.RpcWriter___Server_SendAvatarSettings_4281687581(settings);
			this.RpcLogic___SendAvatarSettings_4281687581(settings);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0009AD61 File Offset: 0x00098F61
		[ObserversRpc(BufferLast = true, RunLocally = true)]
		public void SetAvatarSettings(AvatarSettings settings)
		{
			this.RpcWriter___Observers_SetAvatarSettings_4281687581(settings);
			this.RpcLogic___SetAvatarSettings_4281687581(settings);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0009AD77 File Offset: 0x00098F77
		[ObserversRpc]
		private void SetVisible_Networked(bool vis)
		{
			this.RpcWriter___Observers_SetVisible_Networked_1140765316(vis);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0009AD84 File Offset: 0x00098F84
		public void EnterVehicle(LandVehicle vehicle)
		{
			this.CurrentVehicle = vehicle.NetworkObject;
			this.LastDrivenVehicle = vehicle;
			this.Avatar.transform.SetParent(vehicle.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			if (this.onEnterVehicle != null)
			{
				this.onEnterVehicle(vehicle);
			}
			this.SetVisible(false, true);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0009AE00 File Offset: 0x00099000
		public void ExitVehicle(Transform exitPoint)
		{
			if (this.CurrentVehicle == null)
			{
				return;
			}
			this.Avatar.transform.SetParent(base.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			Player.Local.transform.position = exitPoint.position;
			Player.Local.transform.rotation = exitPoint.rotation;
			Player.Local.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			base.GetComponent<NetworkTransform>().ClearReplicateCache();
			if (this.onExitVehicle != null)
			{
				this.onExitVehicle(this.CurrentVehicle.GetComponent<LandVehicle>(), exitPoint);
			}
			this.SetVisible(true, false);
			this.CurrentVehicle = null;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0009AEF0 File Offset: 0x000990F0
		private void PreDestroyClientObjects(NetworkConnection conn)
		{
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.RemoveOwnership();
				this.CurrentVehicle.GetComponent<LandVehicle>().ExitVehicle();
			}
			int count = this.objectsTemporarilyOwnedByPlayer.Count;
			for (int i = 0; i < count; i++)
			{
				Debug.Log("Stripping object ownership back to server: " + this.objectsTemporarilyOwnedByPlayer[i].gameObject.name);
				this.objectsTemporarilyOwnedByPlayer[i].RemoveOwnership();
			}
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0009AF74 File Offset: 0x00099174
		private void CurrentVehicleChanged(NetworkObject oldVeh, NetworkObject newVeh, bool asServer)
		{
			if (base.IsOwner)
			{
				return;
			}
			if (oldVeh == newVeh)
			{
				return;
			}
			if (newVeh != null)
			{
				this.Avatar.transform.SetParent(newVeh.transform);
				this.Avatar.transform.localPosition = Vector3.zero;
				this.Avatar.transform.localRotation = Quaternion.identity;
				this.SetVisible(false, false);
				return;
			}
			this.Avatar.transform.SetParent(base.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			this.SetVisible(true, false);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0009B030 File Offset: 0x00099230
		public static bool AreAllPlayersReadyToSleep()
		{
			if (Player.PlayerList.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (!(Player.PlayerList[i] == null) && !Player.PlayerList[i].IsReadyToSleep)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0009B088 File Offset: 0x00099288
		private void SleepStart()
		{
			this.IsSleeping = true;
			this.ClearProduct();
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0009B097 File Offset: 0x00099297
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetReadyToSleep(bool ready)
		{
			this.RpcWriter___Server_SetReadyToSleep_1140765316(ready);
			this.RpcLogic___SetReadyToSleep_1140765316(ready);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0009B0AD File Offset: 0x000992AD
		private void SleepEnd(int minsSlept)
		{
			this.IsSleeping = false;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0009B0B6 File Offset: 0x000992B6
		public static void Activate()
		{
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0009B0EE File Offset: 0x000992EE
		public static void Deactivate(bool freeMouse)
		{
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (freeMouse)
			{
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0009B128 File Offset: 0x00099328
		public void ExitAll()
		{
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.GetComponent<LandVehicle>().ExitVehicle();
				this.SetVisible(true, false);
			}
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<HUD>.Instance.canvas.enabled = false;
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0009B1A4 File Offset: 0x000993A4
		public void SetVisibleToLocalPlayer(bool vis)
		{
			this.avatarVisibleToLocalPlayer = vis;
			if (vis)
			{
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Player"));
				return;
			}
			LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0009B1F0 File Offset: 0x000993F0
		[ObserversRpc(RunLocally = true)]
		public void SetPlayerCode(string code)
		{
			this.RpcWriter___Observers_SetPlayerCode_3615296227(code);
			this.RpcLogic___SetPlayerCode_3615296227(code);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0009B206 File Offset: 0x00099406
		[ServerRpc]
		public void SendPunch()
		{
			this.RpcWriter___Server_SendPunch_2166136261();
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0009B20E File Offset: 0x0009940E
		[ObserversRpc]
		private void Punch()
		{
			this.RpcWriter___Observers_Punch_2166136261();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x0009B216 File Offset: 0x00099416
		[ServerRpc(RunLocally = true)]
		private void MarkIntroCompleted(BasicAvatarSettings appearance)
		{
			this.RpcWriter___Server_MarkIntroCompleted_3281254764(appearance);
			this.RpcLogic___MarkIntroCompleted_3281254764(appearance);
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0009B22C File Offset: 0x0009942C
		public bool IsPointVisibleToPlayer(Vector3 point, float maxDistance_Visible = 30f, float minDistance_Invisible = 5f)
		{
			float num = Vector3.Distance(point, this.MimicCamera.transform.position);
			RaycastHit raycastHit;
			return num <= maxDistance_Visible && (num < minDistance_Invisible || (this.MimicCamera.InverseTransformPoint(point).z >= 0f && !Physics.Raycast(this.MimicCamera.transform.position, (point - this.MimicCamera.transform.position).normalized, ref raycastHit, Mathf.Min(maxDistance_Visible, num - 0.5f), 1 << LayerMask.NameToLayer("Default"))));
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0009B2CC File Offset: 0x000994CC
		public static Player GetClosestPlayer(Vector3 point, out float distance, List<Player> exclude = null)
		{
			distance = 0f;
			List<Player> list = new List<Player>();
			list.AddRange(Player.PlayerList);
			if (exclude != null)
			{
				list = list.Except(exclude).ToList<Player>();
			}
			Player player = (from x in list
			orderby Vector3.SqrMagnitude(point - x.Avatar.CenterPoint)
			select x).FirstOrDefault<Player>();
			if (player != null)
			{
				distance = Vector3.Distance(point, player.Avatar.CenterPoint);
				return player;
			}
			return null;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0009B34C File Offset: 0x0009954C
		public void SetCapsuleColliderHeight(float normalizedHeight)
		{
			this.CapCol.height = 2f * normalizedHeight;
			this.CapCol.center = new Vector3(0f, -(2f - this.CapCol.height) / 2f, 0f);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0009B39D File Offset: 0x0009959D
		public void SetScale(float scale)
		{
			this.Scale = scale;
			this.ApplyScale();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0009B3AC File Offset: 0x000995AC
		public void SetScale(float scale, float lerpTime)
		{
			Player.<>c__DisplayClass282_0 CS$<>8__locals1 = new Player.<>c__DisplayClass282_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (this.lerpScaleRoutine != null)
			{
				base.StopCoroutine(this.lerpScaleRoutine);
			}
			CS$<>8__locals1.startScale = this.Scale;
			this.lerpScaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__LerpScale|0());
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x0009B40C File Offset: 0x0009960C
		protected virtual void ApplyScale()
		{
			if (this.ActiveSkateboard != null)
			{
				this.ActiveSkateboard.ApplyPlayerScale();
				base.transform.localScale = Vector3.one;
				return;
			}
			base.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0009B465 File Offset: 0x00099665
		public virtual string GetSaveString()
		{
			return this.GetPlayerData().GetJson(true);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0009B473 File Offset: 0x00099673
		public PlayerData GetPlayerData()
		{
			return new PlayerData(this.PlayerCode, base.transform.position, base.transform.eulerAngles.y, this.HasCompletedIntro);
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0009B4A4 File Offset: 0x000996A4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			((ISaveable)this).WriteSubfile(parentFolderPath, "Inventory", this.GetInventoryString());
			if (this.CurrentAvatarSettings != null)
			{
				string appearanceString = this.GetAppearanceString();
				((ISaveable)this).WriteSubfile(parentFolderPath, "Appearance", appearanceString);
			}
			((ISaveable)this).WriteSubfile(parentFolderPath, "Clothing", this.GetClothingString());
			((ISaveable)this).WriteSubfile(parentFolderPath, "Variables", this.GetVariablesString());
			return result;
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0009B512 File Offset: 0x00099712
		public string GetInventoryString()
		{
			return new ItemSet(this.Inventory.ToList<ItemSlot>()).GetJSON();
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0009B529 File Offset: 0x00099729
		public string GetAppearanceString()
		{
			if (this.CurrentAvatarSettings != null)
			{
				return this.CurrentAvatarSettings.GetJson(true);
			}
			return string.Empty;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0009B54B File Offset: 0x0009974B
		public string GetClothingString()
		{
			return new ItemSet(this.Clothing.ItemSlots.ToList<ItemSlot>()).GetJSON();
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0009B568 File Offset: 0x00099768
		public string GetVariablesString()
		{
			List<VariableData> list = new List<VariableData>();
			for (int i = 0; i < this.PlayerVariables.Count; i++)
			{
				if (this.PlayerVariables[i] != null && this.PlayerVariables[i].Persistent)
				{
					list.Add(new VariableData(this.PlayerVariables[i].Name, this.PlayerVariables[i].GetValue().ToString()));
				}
			}
			return new VariableCollectionData(list.ToArray()).GetJson(true);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0009B5F8 File Offset: 0x000997F8
		public virtual void Load(PlayerData data, string containerPath)
		{
			this.Load(data);
			string contentsString;
			if (this.Loader.TryLoadFile(containerPath, "Inventory", out contentsString))
			{
				this.LoadInventory(contentsString);
			}
			else
			{
				Console.LogWarning("Failed to load player inventory under " + containerPath, null);
			}
			string appearanceString;
			if (this.Loader.TryLoadFile(containerPath, "Appearance", out appearanceString))
			{
				this.LoadAppearance(appearanceString);
			}
			else
			{
				Console.LogWarning("Failed to load player appearance under " + containerPath, null);
			}
			string contentsString2;
			if (this.Loader.TryLoadFile(containerPath, "Clothing", out contentsString2))
			{
				this.LoadClothing(contentsString2);
			}
			else
			{
				Console.LogWarning("Failed to load player clothing under " + containerPath, null);
			}
			bool flag = false;
			string text;
			if (this.Loader.TryLoadFile(containerPath, "Variables", out text))
			{
				VariableCollectionData variableCollectionData = null;
				try
				{
					variableCollectionData = JsonUtility.FromJson<VariableCollectionData>(text);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading player variable data: " + ex.Message);
				}
				if (data != null)
				{
					flag = true;
					foreach (VariableData variableData in variableCollectionData.Variables)
					{
						if (variableData != null)
						{
							this.LoadVariable(variableData);
						}
					}
				}
			}
			if (!flag)
			{
				string text2 = Path.Combine(containerPath, "Variables");
				if (Directory.Exists(text2))
				{
					Console.Log("Loading legacy player variables from " + text2, null);
					string[] files = Directory.GetFiles(text2);
					VariablesLoader variablesLoader = new VariablesLoader();
					for (int j = 0; j < files.Length; j++)
					{
						string text3;
						if (variablesLoader.TryLoadFile(files[j], out text3, false))
						{
							VariableData data2 = null;
							try
							{
								data2 = JsonUtility.FromJson<VariableData>(text3);
							}
							catch (Exception ex2)
							{
								Debug.LogError("Error loading player variable data: " + ex2.Message);
							}
							if (data != null)
							{
								this.LoadVariable(data2);
							}
						}
					}
					return;
				}
				Console.LogWarning("Failed to load player variables under " + containerPath, null);
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0009B7D0 File Offset: 0x000999D0
		public virtual void Load(PlayerData data)
		{
			this.playerDataRetrieveReturned = true;
			if (base.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(data.Position);
				base.transform.eulerAngles = new Vector3(0f, data.Rotation, 0f);
			}
			this.HasCompletedIntro = data.IntroCompleted;
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0009B828 File Offset: 0x00099A28
		public virtual void LoadInventory(string contentsString)
		{
			if (string.IsNullOrEmpty(contentsString))
			{
				Console.LogWarning("Empty inventory string", null);
				return;
			}
			if (!base.IsOwner)
			{
				return;
			}
			DeserializedItemSet deserializedItemSet;
			if (!ItemSet.TryDeserialize(contentsString, out deserializedItemSet))
			{
				return;
			}
			for (int i = 0; i < deserializedItemSet.Items.Length; i++)
			{
				if (deserializedItemSet.Items[i] is CashInstance)
				{
					PlayerSingleton<PlayerInventory>.Instance.cashInstance.SetBalance((deserializedItemSet.Items[i] as CashInstance).Balance, false);
				}
				else if (i < 8)
				{
					PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].SetStoredItem(deserializedItemSet.Items[i], false);
				}
				else
				{
					Console.LogWarning("Hotbar slot out of range", null);
				}
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0009B8D4 File Offset: 0x00099AD4
		public virtual void LoadAppearance(string appearanceString)
		{
			if (string.IsNullOrEmpty(appearanceString))
			{
				Console.LogWarning("Empty appearance string", null);
				return;
			}
			BasicAvatarSettings basicAvatarSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			JsonUtility.FromJsonOverwrite(appearanceString, basicAvatarSettings);
			this.SetAppearance(basicAvatarSettings, false);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0009B90C File Offset: 0x00099B0C
		public virtual void LoadClothing(string contentsString)
		{
			if (string.IsNullOrEmpty(contentsString))
			{
				Console.LogWarning("Empty clothing string", null);
				return;
			}
			if (!base.IsOwner)
			{
				return;
			}
			DeserializedItemSet deserializedItemSet;
			if (!ItemSet.TryDeserialize(contentsString, out deserializedItemSet))
			{
				return;
			}
			deserializedItemSet.LoadTo(this.Clothing.ItemSlots);
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0009B954 File Offset: 0x00099B54
		public void SetRagdolled(bool ragdolled)
		{
			if (ragdolled == this.IsRagdolled)
			{
				return;
			}
			this.IsRagdolled = ragdolled;
			this.Avatar.SetRagdollPhysicsEnabled(ragdolled, false);
			this.Avatar.transform.localEulerAngles = Vector3.zero;
			if (base.IsOwner)
			{
				if (this.IsRagdolled)
				{
					LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Player"));
				}
				else
				{
					LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
				}
			}
			if (this.IsRagdolled)
			{
				if (this.onRagdoll != null)
				{
					this.onRagdoll.Invoke();
					return;
				}
			}
			else if (this.onRagdollEnd != null)
			{
				this.onRagdollEnd.Invoke();
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0009BA09 File Offset: 0x00099C09
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public virtual void SendImpact(Impact impact)
		{
			this.RpcWriter___Server_SendImpact_427288424(impact);
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0009BA20 File Offset: 0x00099C20
		[ObserversRpc(RunLocally = true)]
		public virtual void ReceiveImpact(Impact impact)
		{
			this.RpcWriter___Observers_ReceiveImpact_427288424(impact);
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0009BA41 File Offset: 0x00099C41
		public virtual void ProcessImpactForce(Vector3 forcePoint, Vector3 forceDirection, float force)
		{
			if (force >= 50f)
			{
				this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Light);
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0009BA60 File Offset: 0x00099C60
		public virtual void OnDied()
		{
			if (base.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
				Singleton<HUD>.Instance.canvas.enabled = false;
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, 0f, false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Dead");
			}
			this.ClearProduct();
			this.NameLabel.gameObject.SetActive(false);
			this.CapCol.enabled = false;
			this.SetRagdolled(true);
			this.Avatar.MiddleSpineRB.AddForce(base.transform.forward * 30f, 2);
			this.Avatar.MiddleSpineRB.AddRelativeTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f, 2);
			if (this.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.IsArrested = true;
			}
			if (base.Owner.IsLocalClient)
			{
				Singleton<DeathScreen>.Instance.Open();
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0009BB98 File Offset: 0x00099D98
		public virtual void OnRevived()
		{
			this.SetRagdolled(false);
			if (!base.Owner.IsLocalClient)
			{
				this.NameLabel.gameObject.SetActive(true);
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Dead");
			this.CapCol.enabled = true;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0009BBF8 File Offset: 0x00099DF8
		[ObserversRpc(RunLocally = true)]
		public void Arrest()
		{
			this.RpcWriter___Observers_Arrest_2166136261();
			this.RpcLogic___Arrest_2166136261();
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0009BC14 File Offset: 0x00099E14
		public void Free()
		{
			if (!this.IsArrested)
			{
				return;
			}
			if (base.IsOwner)
			{
				Transform transform = Singleton<Map>.Instance.PoliceStation.SpawnPoint;
				if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
				{
					if (Player.Local.LastVisitedProperty != null)
					{
						transform = Player.Local.LastVisitedProperty.InteriorSpawnPoint;
					}
					else if (Property.OwnedProperties.Count > 0)
					{
						transform = Property.OwnedProperties[0].InteriorSpawnPoint;
					}
					else
					{
						transform = Property.UnownedProperties[0].InteriorSpawnPoint;
					}
				}
				PlayerSingleton<PlayerMovement>.Instance.Teleport(transform.position + Vector3.up * 1f);
				base.transform.forward = transform.forward;
				Singleton<HUD>.Instance.canvas.enabled = true;
			}
			this.IsArrested = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Arrested");
			if (this.onFreed != null)
			{
				this.onFreed.Invoke();
			}
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0009BD14 File Offset: 0x00099F14
		[ServerRpc(RunLocally = true)]
		public void SendPassOut()
		{
			this.RpcWriter___Server_SendPassOut_2166136261();
			this.RpcLogic___SendPassOut_2166136261();
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0009BD24 File Offset: 0x00099F24
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void PassOut()
		{
			this.RpcWriter___Observers_PassOut_2166136261();
			this.RpcLogic___PassOut_2166136261();
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0009BD3D File Offset: 0x00099F3D
		[ServerRpc(RunLocally = true)]
		public void SendPassOutRecovery()
		{
			this.RpcWriter___Server_SendPassOutRecovery_2166136261();
			this.RpcLogic___SendPassOutRecovery_2166136261();
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0009BD4C File Offset: 0x00099F4C
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void PassOutRecovery()
		{
			this.RpcWriter___Observers_PassOutRecovery_2166136261();
			this.RpcLogic___PassOutRecovery_2166136261();
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x0009BD65 File Offset: 0x00099F65
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendEquippable_Networked(string assetPath)
		{
			this.RpcWriter___Server_SendEquippable_Networked_3615296227(assetPath);
			this.RpcLogic___SendEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x0009BD7B File Offset: 0x00099F7B
		[ObserversRpc(RunLocally = true)]
		private void SetEquippable_Networked(string assetPath)
		{
			this.RpcWriter___Observers_SetEquippable_Networked_3615296227(assetPath);
			this.RpcLogic___SetEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0009BD91 File Offset: 0x00099F91
		[ServerRpc(RunLocally = true)]
		public void SendEquippableMessage_Networked(string message, int receipt)
		{
			this.RpcWriter___Server_SendEquippableMessage_Networked_3643459082(message, receipt);
			this.RpcLogic___SendEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0009BDAF File Offset: 0x00099FAF
		[ObserversRpc(RunLocally = true)]
		private void ReceiveEquippableMessage_Networked(string message, int receipt)
		{
			this.RpcWriter___Observers_ReceiveEquippableMessage_Networked_3643459082(message, receipt);
			this.RpcLogic___ReceiveEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0009BDCD File Offset: 0x00099FCD
		[ServerRpc(RunLocally = true)]
		public void SendEquippableMessage_Networked_Vector(string message, int receipt, Vector3 data)
		{
			this.RpcWriter___Server_SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
			this.RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0009BDF3 File Offset: 0x00099FF3
		[ObserversRpc(RunLocally = true)]
		private void ReceiveEquippableMessage_Networked_Vector(string message, int receipt, Vector3 data)
		{
			this.RpcWriter___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
			this.RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0009BE19 File Offset: 0x0009A019
		[ServerRpc(RunLocally = true)]
		public void SendAnimationTrigger(string trigger)
		{
			this.RpcWriter___Server_SendAnimationTrigger_3615296227(trigger);
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0009BE2F File Offset: 0x0009A02F
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0009BE65 File Offset: 0x0009A065
		public void SetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.SetTrigger(trigger);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0009BE78 File Offset: 0x0009A078
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ResetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0009BEAE File Offset: 0x0009A0AE
		public void ResetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.ResetTrigger(trigger);
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0009BEC1 File Offset: 0x0009A0C1
		[ServerRpc(RunLocally = true)]
		public void SendAnimationBool(string name, bool val)
		{
			this.RpcWriter___Server_SendAnimationBool_310431262(name, val);
			this.RpcLogic___SendAnimationBool_310431262(name, val);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0009BEDF File Offset: 0x0009A0DF
		[ObserversRpc(RunLocally = true)]
		public void SetAnimationBool(string name, bool val)
		{
			this.RpcWriter___Observers_SetAnimationBool_310431262(name, val);
			this.RpcLogic___SetAnimationBool_310431262(name, val);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0009BF00 File Offset: 0x0009A100
		[ObserversRpc]
		public void Taze()
		{
			this.RpcWriter___Observers_Taze_2166136261();
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0009BF13 File Offset: 0x0009A113
		[ServerRpc(RunLocally = true)]
		public void SetInventoryItem(int index, ItemInstance item)
		{
			this.RpcWriter___Server_SetInventoryItem_2317364410(index, item);
			this.RpcLogic___SetInventoryItem_2317364410(index, item);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0009BF34 File Offset: 0x0009A134
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			for (int i = 0; i < this.Inventory.Length; i++)
			{
				if (this.Inventory[i].ItemInstance != null)
				{
					container.ChangeValue(this.Inventory[i].ItemInstance.GetMonetaryValue());
				}
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0009BF7B File Offset: 0x0009A17B
		[ServerRpc(RunLocally = true)]
		public void SendAppearance(BasicAvatarSettings settings)
		{
			this.RpcWriter___Server_SendAppearance_3281254764(settings);
			this.RpcLogic___SendAppearance_3281254764(settings);
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0009BF94 File Offset: 0x0009A194
		[ObserversRpc(RunLocally = true)]
		public void SetAppearance(BasicAvatarSettings settings, bool refreshClothing)
		{
			this.RpcWriter___Observers_SetAppearance_2139595489(settings, refreshClothing);
			this.RpcLogic___SetAppearance_2139595489(settings, refreshClothing);
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0009BFC0 File Offset: 0x0009A1C0
		public void MountSkateboard(Skateboard board)
		{
			this.SendMountedSkateboard(board.NetworkObject);
			foreach (Collider collider in base.GetComponentsInChildren<Collider>(true))
			{
				foreach (Collider collider2 in board.MainColliders)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Skateboard);
			PlayerSingleton<PlayerCamera>.Instance.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.transform.position - base.transform.forward * 0.5f;
			this.SetVisibleToLocalPlayer(true);
			this.CapCol.enabled = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerMovement>.Instance.Controller.enabled = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("skateboard");
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0009C0E4 File Offset: 0x0009A2E4
		[ServerRpc(RunLocally = true)]
		private void SendMountedSkateboard(NetworkObject skateboardObj)
		{
			this.RpcWriter___Server_SendMountedSkateboard_3323014238(skateboardObj);
			this.RpcLogic___SendMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0009C0FC File Offset: 0x0009A2FC
		[ObserversRpc(RunLocally = true)]
		private void SetMountedSkateboard(NetworkObject skateboardObj)
		{
			this.RpcWriter___Observers_SetMountedSkateboard_3323014238(skateboardObj);
			this.RpcLogic___SetMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0009C120 File Offset: 0x0009A320
		public void DismountSkateboard()
		{
			this.SendMountedSkateboard(null);
			this.SetVisibleToLocalPlayer(false);
			this.CapCol.enabled = true;
			this.SetCapsuleColliderHeight(1f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerMovement>.Instance.Controller.enabled = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Default);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0009C1B8 File Offset: 0x0009A3B8
		public void ConsumeProduct(ProductItemInstance product)
		{
			this.SendConsumeProduct(product);
			this.ConsumeProductInternal(product);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0009C1C8 File Offset: 0x0009A3C8
		[ServerRpc(RequireOwnership = false)]
		private void SendConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Server_SendConsumeProduct_2622925554(product);
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0009C1D4 File Offset: 0x0009A3D4
		[ObserversRpc]
		private void ReceiveConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Observers_ReceiveConsumeProduct_2622925554(product);
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0009C1E0 File Offset: 0x0009A3E0
		private void ConsumeProductInternal(ProductItemInstance product)
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearProduct();
			}
			this.ConsumedProduct = product;
			this.TimeSinceProductConsumed = 0;
			product.ApplyEffectsToPlayer(this);
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0009C205 File Offset: 0x0009A405
		public void ClearProduct()
		{
			if (this.ConsumedProduct != null)
			{
				this.ConsumedProduct.ClearEffectsFromPlayer(this);
				this.ConsumedProduct = null;
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0009C224 File Offset: 0x0009A424
		private void CreatePlayerVariables()
		{
			if (this.VariableDict.Count > 0)
			{
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Creating player variables for ",
				this.PlayerName,
				" (",
				this.PlayerCode,
				")"
			}), null);
			NetworkSingleton<VariableDatabase>.Instance.CreatePlayerVariables(this);
			if (InstanceFinder.IsServer)
			{
				this.SetVariableValue("IsServer", true.ToString(), true);
			}
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0009C2A2 File Offset: 0x0009A4A2
		public BaseVariable GetVariable(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				return this.VariableDict[variableName];
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
			return null;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0009C2DC File Offset: 0x0009A4DC
		public T GetValue<T>(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				return (T)((object)this.VariableDict[variableName].GetValue());
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
			return default(T);
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0009C335 File Offset: 0x0009A535
		public void SetVariableValue(string variableName, string value, bool network = true)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, network);
				return;
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0009C374 File Offset: 0x0009A574
		public void AddVariable(BaseVariable variable)
		{
			if (this.VariableDict.ContainsKey(variable.Name.ToLower()))
			{
				Console.LogError("Variable with name " + variable.Name + " already exists in the database.", null);
				return;
			}
			this.PlayerVariables.Add(variable);
			this.VariableDict.Add(variable.Name.ToLower(), variable);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0009C3D8 File Offset: 0x0009A5D8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendValue(string variableName, string value, bool sendToOwner)
		{
			this.RpcWriter___Server_SendValue_3589193952(variableName, value, sendToOwner);
			this.RpcLogic___SendValue_3589193952(variableName, value, sendToOwner);
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0009C3FE File Offset: 0x0009A5FE
		[TargetRpc]
		private void ReceiveValue(NetworkConnection conn, string variableName, string value)
		{
			this.RpcWriter___Target_ReceiveValue_3895153758(conn, variableName, value);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x0009C412 File Offset: 0x0009A612
		private void ReceiveValue(string variableName, string value)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, false);
				return;
			}
			Console.LogWarning("Failed to find player variable with name: " + variableName, null);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x0009C450 File Offset: 0x0009A650
		public void LoadVariable(VariableData data)
		{
			BaseVariable variable = this.GetVariable(data.Name);
			if (variable == null)
			{
				Console.LogWarning("Failed to find variable with name: " + data.Name, null);
				return;
			}
			variable.SetValue(data.Value, true);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0009C494 File Offset: 0x0009A694
		public Player()
		{
			this.<PlayerName>k__BackingField = "Player";
			this.<PlayerCode>k__BackingField = string.Empty;
			this.Scale = 1f;
			this.<CameraPosition>k__BackingField = Vector3.zero;
			this.<CameraRotation>k__BackingField = Quaternion.identity;
			this.Inventory = new ItemSlot[9];
			this.loader = new PlayerLoader();
			this.LocalExtraFiles = new List<string>
			{
				"Inventory",
				"Appearance",
				"Clothing",
				"Variables"
			};
			this.LocalExtraFolders = new List<string>();
			this.HasChanged = true;
			this.PlayerVariables = new List<BaseVariable>();
			this.VariableDict = new Dictionary<string, BaseVariable>();
			this.standingScale = 1f;
			this.ragdollForceComponents = new List<ConstantForce>();
			this.impactHistory = new List<int>();
			this.seizureRotations = new List<Quaternion>();
			this.equippableMessageIDHistory = new List<int>();
			base..ctor();
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0009C5E6 File Offset: 0x0009A7E6
		[CompilerGenerated]
		private IEnumerator <Taze>g__Tase|321_0()
		{
			this.Avatar.Effects.SetZapped(true, true);
			yield return new WaitForSeconds(2f);
			this.Avatar.Effects.SetZapped(false, true);
			this.IsTased = false;
			if (this.onTasedEnd != null)
			{
				this.onTasedEnd.Invoke();
			}
			yield break;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0009C5F8 File Offset: 0x0009A7F8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<CameraRotation>k__BackingField = new SyncVar<Quaternion>(this, 6U, 0, 0, 0.1f, 1, this.<CameraRotation>k__BackingField);
			this.syncVar___<CameraPosition>k__BackingField = new SyncVar<Vector3>(this, 5U, 0, 0, 0.1f, 1, this.<CameraPosition>k__BackingField);
			this.syncVar___<IsReadyToSleep>k__BackingField = new SyncVar<bool>(this, 4U, 0, 0, -1f, 0, this.<IsReadyToSleep>k__BackingField);
			this.syncVar___<CurrentBed>k__BackingField = new SyncVar<NetworkObject>(this, 3U, 0, 0, -1f, 0, this.<CurrentBed>k__BackingField);
			this.syncVar___<CurrentVehicle>k__BackingField = new SyncVar<NetworkObject>(this, 2U, 0, 0, -1f, 0, this.<CurrentVehicle>k__BackingField);
			this.syncVar___<CurrentVehicle>k__BackingField.OnChange += this.CurrentVehicleChanged;
			this.syncVar___<PlayerCode>k__BackingField = new SyncVar<string>(this, 1U, 1, 0, -1f, 0, this.<PlayerCode>k__BackingField);
			this.syncVar___<PlayerName>k__BackingField = new SyncVar<string>(this, 0U, 1, 0, -1f, 0, this.<PlayerName>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentVehicle_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentBed_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_set_IsSkating_1140765316));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_set_CameraPosition_4276783012));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_set_CameraRotation_3429297120));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_RequestSavePlayer_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ReturnSaveRequest_214505783));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_ReturnSaveRequest_214505783));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_HostExitedGame_2166136261));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerNameData_586648380));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_RequestPlayerData_3615296227));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_MarkPlayerInitialized_2166136261));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerData_3244732873));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerData_3244732873));
			base.RegisterObserversRpc(14U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerNameData_3895153758));
			base.RegisterTargetRpc(15U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerNameData_3895153758));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SendFlashlightOnNetworked_1140765316));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_SetFlashlightOn_1140765316));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_PlayJumpAnimation_2166136261));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SendCrouched_1140765316));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveCrouched_214505783));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveCrouched_214505783));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SendAvatarSettings_4281687581));
			base.RegisterObserversRpc(23U, new ClientRpcDelegate(this.RpcReader___Observers_SetAvatarSettings_4281687581));
			base.RegisterObserversRpc(24U, new ClientRpcDelegate(this.RpcReader___Observers_SetVisible_Networked_1140765316));
			base.RegisterServerRpc(25U, new ServerRpcDelegate(this.RpcReader___Server_SetReadyToSleep_1140765316));
			base.RegisterObserversRpc(26U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerCode_3615296227));
			base.RegisterServerRpc(27U, new ServerRpcDelegate(this.RpcReader___Server_SendPunch_2166136261));
			base.RegisterObserversRpc(28U, new ClientRpcDelegate(this.RpcReader___Observers_Punch_2166136261));
			base.RegisterServerRpc(29U, new ServerRpcDelegate(this.RpcReader___Server_MarkIntroCompleted_3281254764));
			base.RegisterServerRpc(30U, new ServerRpcDelegate(this.RpcReader___Server_SendImpact_427288424));
			base.RegisterObserversRpc(31U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveImpact_427288424));
			base.RegisterObserversRpc(32U, new ClientRpcDelegate(this.RpcReader___Observers_Arrest_2166136261));
			base.RegisterServerRpc(33U, new ServerRpcDelegate(this.RpcReader___Server_SendPassOut_2166136261));
			base.RegisterObserversRpc(34U, new ClientRpcDelegate(this.RpcReader___Observers_PassOut_2166136261));
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_SendPassOutRecovery_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_PassOutRecovery_2166136261));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippable_Networked_3615296227));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_3615296227));
			base.RegisterServerRpc(39U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippableMessage_Networked_3643459082));
			base.RegisterObserversRpc(40U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEquippableMessage_Networked_3643459082));
			base.RegisterServerRpc(41U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippableMessage_Networked_Vector_3190074053));
			base.RegisterObserversRpc(42U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053));
			base.RegisterServerRpc(43U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationTrigger_3615296227));
			base.RegisterObserversRpc(44U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(45U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(46U, new ClientRpcDelegate(this.RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(47U, new ClientRpcDelegate(this.RpcReader___Target_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterServerRpc(48U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationBool_310431262));
			base.RegisterObserversRpc(49U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationBool_310431262));
			base.RegisterObserversRpc(50U, new ClientRpcDelegate(this.RpcReader___Observers_Taze_2166136261));
			base.RegisterServerRpc(51U, new ServerRpcDelegate(this.RpcReader___Server_SetInventoryItem_2317364410));
			base.RegisterServerRpc(52U, new ServerRpcDelegate(this.RpcReader___Server_SendAppearance_3281254764));
			base.RegisterObserversRpc(53U, new ClientRpcDelegate(this.RpcReader___Observers_SetAppearance_2139595489));
			base.RegisterServerRpc(54U, new ServerRpcDelegate(this.RpcReader___Server_SendMountedSkateboard_3323014238));
			base.RegisterObserversRpc(55U, new ClientRpcDelegate(this.RpcReader___Observers_SetMountedSkateboard_3323014238));
			base.RegisterServerRpc(56U, new ServerRpcDelegate(this.RpcReader___Server_SendConsumeProduct_2622925554));
			base.RegisterObserversRpc(57U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveConsumeProduct_2622925554));
			base.RegisterServerRpc(58U, new ServerRpcDelegate(this.RpcReader___Server_SendValue_3589193952));
			base.RegisterTargetRpc(59U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveValue_3895153758));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.PlayerScripts.Player));
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x0009CCD0 File Offset: 0x0009AED0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<CameraRotation>k__BackingField.SetRegistered();
			this.syncVar___<CameraPosition>k__BackingField.SetRegistered();
			this.syncVar___<IsReadyToSleep>k__BackingField.SetRegistered();
			this.syncVar___<CurrentBed>k__BackingField.SetRegistered();
			this.syncVar___<CurrentVehicle>k__BackingField.SetRegistered();
			this.syncVar___<PlayerCode>k__BackingField.SetRegistered();
			this.syncVar___<PlayerName>k__BackingField.SetRegistered();
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x0009CD3B File Offset: 0x0009AF3B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0009CD4C File Offset: 0x0009AF4C
		private void RpcWriter___Server_set_CurrentVehicle_3323014238(NetworkObject value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0009CE4D File Offset: 0x0009B04D
		public void RpcLogic___set_CurrentVehicle_3323014238(NetworkObject value)
		{
			this.sync___set_value_<CurrentVehicle>k__BackingField(value, true);
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0009CE58 File Offset: 0x0009B058
		private void RpcReader___Server_set_CurrentVehicle_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject value = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_CurrentVehicle_3323014238(value);
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x0009CEA8 File Offset: 0x0009B0A8
		private void RpcWriter___Server_set_CurrentBed_3323014238(NetworkObject value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(value);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x0009CFA9 File Offset: 0x0009B1A9
		public void RpcLogic___set_CurrentBed_3323014238(NetworkObject value)
		{
			this.sync___set_value_<CurrentBed>k__BackingField(value, true);
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x0009CFB4 File Offset: 0x0009B1B4
		private void RpcReader___Server_set_CurrentBed_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject value = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CurrentBed_3323014238(value);
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x0009CFF8 File Offset: 0x0009B1F8
		private void RpcWriter___Server_set_IsSkating_1140765316(bool value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(value);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x0009D0F9 File Offset: 0x0009B2F9
		public void RpcLogic___set_IsSkating_1140765316(bool value)
		{
			this.<IsSkating>k__BackingField = value;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x0009D104 File Offset: 0x0009B304
		private void RpcReader___Server_set_IsSkating_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_IsSkating_1140765316(value);
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x0009D148 File Offset: 0x0009B348
		private void RpcWriter___Server_set_CameraPosition_4276783012(Vector3 value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(value);
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0009D249 File Offset: 0x0009B449
		public void RpcLogic___set_CameraPosition_4276783012(Vector3 value)
		{
			this.sync___set_value_<CameraPosition>k__BackingField(value, true);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x0009D254 File Offset: 0x0009B454
		private void RpcReader___Server_set_CameraPosition_4276783012(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 value = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CameraPosition_4276783012(value);
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x0009D298 File Offset: 0x0009B498
		private void RpcWriter___Server_set_CameraRotation_3429297120(Quaternion value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteQuaternion(value, 1);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x0009D39E File Offset: 0x0009B59E
		public void RpcLogic___set_CameraRotation_3429297120(Quaternion value)
		{
			this.sync___set_value_<CameraRotation>k__BackingField(value, true);
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0009D3A8 File Offset: 0x0009B5A8
		private void RpcReader___Server_set_CameraRotation_3429297120(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Quaternion value = PooledReader0.ReadQuaternion(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CameraRotation_3429297120(value);
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x0009D3F0 File Offset: 0x0009B5F0
		private void RpcWriter___Server_RequestSavePlayer_2166136261()
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
			base.SendServerRpc(5U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x0009D48A File Offset: 0x0009B68A
		public void RpcLogic___RequestSavePlayer_2166136261()
		{
			this.playerSaveRequestReturned = false;
			if (InstanceFinder.IsServer)
			{
				Console.Log("Save request received", null);
				Singleton<PlayerManager>.Instance.SavePlayer(this);
				this.ReturnSaveRequest(base.Owner, true);
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x0009D4C0 File Offset: 0x0009B6C0
		private void RpcReader___Server_RequestSavePlayer_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RequestSavePlayer_2166136261();
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x0009D4F0 File Offset: 0x0009B6F0
		private void RpcWriter___Observers_ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
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
			writer.WriteBoolean(successful);
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x0009D5A6 File Offset: 0x0009B7A6
		private void RpcLogic___ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
		{
			Console.Log("Save request returned. Successful: " + successful.ToString(), null);
			this.playerSaveRequestReturned = true;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x0009D5C8 File Offset: 0x0009B7C8
		private void RpcReader___Observers_ReturnSaveRequest_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool successful = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReturnSaveRequest_214505783(null, successful);
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x0009D5FC File Offset: 0x0009B7FC
		private void RpcWriter___Target_ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
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
			writer.WriteBoolean(successful);
			base.SendTargetRpc(7U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x0009D6B4 File Offset: 0x0009B8B4
		private void RpcReader___Target_ReturnSaveRequest_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool successful = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReturnSaveRequest_214505783(base.LocalConnection, successful);
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x0009D6EC File Offset: 0x0009B8EC
		private void RpcWriter___Observers_HostExitedGame_2166136261()
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
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x0009D798 File Offset: 0x0009B998
		public void RpcLogic___HostExitedGame_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.InstanceExists && (Singleton<LoadManager>.Instance.IsLoading || !Singleton<LoadManager>.Instance.IsGameLoaded))
			{
				return;
			}
			Console.Log("Host exited game", null);
			Singleton<LoadManager>.Instance.ExitToMenu(null, new MainMenuPopup.Data("Exited Game", "Host left the game", false), false);
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x0009D7F4 File Offset: 0x0009B9F4
		private void RpcReader___Observers_HostExitedGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___HostExitedGame_2166136261();
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x0009D820 File Offset: 0x0009BA20
		private void RpcWriter___Server_SendPlayerNameData_586648380(string playerName, ulong id)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(playerName);
			writer.WriteUInt64(id, 1);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x0009D934 File Offset: 0x0009BB34
		public void RpcLogic___SendPlayerNameData_586648380(string playerName, ulong id)
		{
			this.ReceivePlayerNameData(null, playerName, id.ToString());
			if (SteamManager.Initialized && SteamFriends.GetFriendRelationship(new CSteamID(id)) != 3 && !base.Owner.IsLocalClient)
			{
				Console.LogError("Player " + playerName + " is not friends with the host. Kicking from game.", null);
				base.Owner.Kick(0, 2, "Not friends with host");
				return;
			}
			this.PlayerName = playerName;
			this.PlayerCode = id.ToString();
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x0009D9B0 File Offset: 0x0009BBB0
		private void RpcReader___Server_SendPlayerNameData_586648380(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string playerName = PooledReader0.ReadString();
			ulong id = PooledReader0.ReadUInt64(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerNameData_586648380(playerName, id);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x0009DA18 File Offset: 0x0009BC18
		private void RpcWriter___Server_RequestPlayerData_3615296227(string playerCode)
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
			writer.WriteString(playerCode);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x0009DAC0 File Offset: 0x0009BCC0
		public void RpcLogic___RequestPlayerData_3615296227(string playerCode)
		{
			PlayerData playerData;
			string inventoryString;
			string appearanceString;
			string clothigString;
			VariableData[] vars;
			Singleton<PlayerManager>.Instance.TryGetPlayerData(playerCode, out playerData, out inventoryString, out appearanceString, out clothigString, out vars);
			string[] array = new string[5];
			array[0] = "Sending player data for ";
			array[1] = playerCode;
			array[2] = " (";
			int num = 3;
			PlayerData playerData2 = playerData;
			array[num] = ((playerData2 != null) ? playerData2.ToString() : null);
			array[4] = ")";
			Console.Log(string.Concat(array), null);
			this.ReceivePlayerData(null, playerData, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x0009DB30 File Offset: 0x0009BD30
		private void RpcReader___Server_RequestPlayerData_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string playerCode = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestPlayerData_3615296227(playerCode);
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x0009DB64 File Offset: 0x0009BD64
		private void RpcWriter___Server_MarkPlayerInitialized_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x0009DC58 File Offset: 0x0009BE58
		public void RpcLogic___MarkPlayerInitialized_2166136261()
		{
			Console.Log(this.PlayerName + " initialized over network", null);
			this.PlayerInitializedOverNetwork = true;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x0009DC78 File Offset: 0x0009BE78
		private void RpcReader___Server_MarkPlayerInitialized_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkPlayerInitialized_2166136261();
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x0009DCB8 File Offset: 0x0009BEB8
		private void RpcWriter___Observers_ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
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
			writer.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated(data);
			writer.WriteString(inventoryString);
			writer.WriteString(appearanceString);
			writer.WriteString(clothigString);
			writer.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated(vars);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x0009DDA4 File Offset: 0x0009BFA4
		public void RpcLogic___ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
		{
			this.playerDataRetrieveReturned = true;
			if (data != null)
			{
				this.Load(data);
				if (!string.IsNullOrEmpty(inventoryString))
				{
					this.LoadInventory(inventoryString);
				}
				if (!string.IsNullOrEmpty(appearanceString))
				{
					this.LoadAppearance(appearanceString);
				}
				if (!string.IsNullOrEmpty(clothigString))
				{
					this.LoadClothing(clothigString);
				}
			}
			else if (base.IsOwner)
			{
				Console.Log("No player data found for this player; first time joining", null);
			}
			if (base.IsOwner)
			{
				if (vars != null)
				{
					foreach (VariableData data2 in vars)
					{
						this.LoadVariable(data2);
					}
				}
				this.PlayerLoaded();
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x0009DE38 File Offset: 0x0009C038
		private void RpcReader___Observers_ReceivePlayerData_3244732873(PooledReader PooledReader0, Channel channel)
		{
			PlayerData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds(PooledReader0);
			string inventoryString = PooledReader0.ReadString();
			string appearanceString = PooledReader0.ReadString();
			string clothigString = PooledReader0.ReadString();
			VariableData[] vars = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerData_3244732873(null, data, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x0009DEB8 File Offset: 0x0009C0B8
		private void RpcWriter___Target_ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
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
			writer.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated(data);
			writer.WriteString(inventoryString);
			writer.WriteString(appearanceString);
			writer.WriteString(clothigString);
			writer.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated(vars);
			base.SendTargetRpc(13U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x0009DFA4 File Offset: 0x0009C1A4
		private void RpcReader___Target_ReceivePlayerData_3244732873(PooledReader PooledReader0, Channel channel)
		{
			PlayerData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds(PooledReader0);
			string inventoryString = PooledReader0.ReadString();
			string appearanceString = PooledReader0.ReadString();
			string clothigString = PooledReader0.ReadString();
			VariableData[] vars = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerData_3244732873(base.LocalConnection, data, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x0009E020 File Offset: 0x0009C220
		private void RpcWriter___Observers_ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
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
			writer.WriteString(playerName);
			writer.WriteString(id);
			base.SendObserversRpc(14U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x0009E0E4 File Offset: 0x0009C2E4
		private void RpcLogic___ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
		{
			this.PlayerName = playerName;
			this.PlayerCode = id;
			base.gameObject.name = this.PlayerName + " (" + id + ")";
			this.PoI.SetMainText(this.PlayerName);
			this.NameLabel.ShowText(this.PlayerName, 0f);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x0009E148 File Offset: 0x0009C348
		private void RpcReader___Observers_ReceivePlayerNameData_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string playerName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerNameData_3895153758(null, playerName, id);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0009E198 File Offset: 0x0009C398
		private void RpcWriter___Target_ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
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
			writer.WriteString(playerName);
			writer.WriteString(id);
			base.SendTargetRpc(15U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x0009E25C File Offset: 0x0009C45C
		private void RpcReader___Target_ReceivePlayerNameData_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string playerName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerNameData_3895153758(base.LocalConnection, playerName, id);
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x0009E2A4 File Offset: 0x0009C4A4
		private void RpcWriter___Server_SendFlashlightOnNetworked_1140765316(bool on)
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
			writer.WriteBoolean(on);
			base.SendServerRpc(16U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x0009E34B File Offset: 0x0009C54B
		private void RpcLogic___SendFlashlightOnNetworked_1140765316(bool on)
		{
			this.SetFlashlightOn(on);
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x0009E354 File Offset: 0x0009C554
		private void RpcReader___Server_SendFlashlightOnNetworked_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendFlashlightOnNetworked_1140765316(on);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x0009E394 File Offset: 0x0009C594
		private void RpcWriter___Observers_SetFlashlightOn_1140765316(bool on)
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
			writer.WriteBoolean(on);
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x0009E44A File Offset: 0x0009C64A
		private void RpcLogic___SetFlashlightOn_1140765316(bool on)
		{
			this.ThirdPersonFlashlight.gameObject.SetActive(on && !base.IsOwner);
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x0009E46C File Offset: 0x0009C66C
		private void RpcReader___Observers_SetFlashlightOn_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetFlashlightOn_1140765316(on);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x0009E4A8 File Offset: 0x0009C6A8
		private void RpcWriter___Observers_PlayJumpAnimation_2166136261()
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

		// Token: 0x0600270E RID: 9998 RVA: 0x0009E551 File Offset: 0x0009C751
		public void RpcLogic___PlayJumpAnimation_2166136261()
		{
			this.Anim.Jump();
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x0009E560 File Offset: 0x0009C760
		private void RpcReader___Observers_PlayJumpAnimation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlayJumpAnimation_2166136261();
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x0009E580 File Offset: 0x0009C780
		private void RpcWriter___Server_SendCrouched_1140765316(bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendServerRpc(19U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x0009E627 File Offset: 0x0009C827
		public void RpcLogic___SendCrouched_1140765316(bool crouched)
		{
			this.ReceiveCrouched(null, crouched);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0009E634 File Offset: 0x0009C834
		private void RpcReader___Server_SendCrouched_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCrouched_1140765316(crouched);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0009E674 File Offset: 0x0009C874
		private void RpcWriter___Target_ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendTargetRpc(20U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x0009E729 File Offset: 0x0009C929
		private void RpcLogic___ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
		{
			if (base.Owner.IsLocalClient)
			{
				return;
			}
			this.Crouched = crouched;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x0009E740 File Offset: 0x0009C940
		private void RpcReader___Target_ReceiveCrouched_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveCrouched_214505783(base.LocalConnection, crouched);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x0009E778 File Offset: 0x0009C978
		private void RpcWriter___Observers_ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendObserversRpc(21U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x0009E830 File Offset: 0x0009CA30
		private void RpcReader___Observers_ReceiveCrouched_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveCrouched_214505783(null, crouched);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x0009E86C File Offset: 0x0009CA6C
		private void RpcWriter___Server_SendAvatarSettings_4281687581(AvatarSettings settings)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendServerRpc(22U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x0009E96D File Offset: 0x0009CB6D
		public void RpcLogic___SendAvatarSettings_4281687581(AvatarSettings settings)
		{
			this.SetAvatarSettings(settings);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0009E978 File Offset: 0x0009CB78
		private void RpcReader___Server_SendAvatarSettings_4281687581(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			AvatarSettings settings = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAvatarSettings_4281687581(settings);
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x0009E9C8 File Offset: 0x0009CBC8
		private void RpcWriter___Observers_SetAvatarSettings_4281687581(AvatarSettings settings)
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
			writer.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendObserversRpc(23U, writer, channel, 0, true, false, false);
			writer.Store();
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x0009EA7E File Offset: 0x0009CC7E
		public void RpcLogic___SetAvatarSettings_4281687581(AvatarSettings settings)
		{
			this.Avatar.LoadAvatarSettings(settings);
			if (base.IsOwner)
			{
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x0009EAB0 File Offset: 0x0009CCB0
		private void RpcReader___Observers_SetAvatarSettings_4281687581(PooledReader PooledReader0, Channel channel)
		{
			AvatarSettings settings = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAvatarSettings_4281687581(settings);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0009EAEC File Offset: 0x0009CCEC
		private void RpcWriter___Observers_SetVisible_Networked_1140765316(bool vis)
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
			writer.WriteBoolean(vis);
			base.SendObserversRpc(24U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x0009EBA2 File Offset: 0x0009CDA2
		private void RpcLogic___SetVisible_Networked_1140765316(bool vis)
		{
			this.Avatar.SetVisible(vis);
			this.CapCol.enabled = vis;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x0009EBBC File Offset: 0x0009CDBC
		private void RpcReader___Observers_SetVisible_Networked_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool vis = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetVisible_Networked_1140765316(vis);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x0009EBF0 File Offset: 0x0009CDF0
		private void RpcWriter___Server_SetReadyToSleep_1140765316(bool ready)
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
			writer.WriteBoolean(ready);
			base.SendServerRpc(25U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x0009EC97 File Offset: 0x0009CE97
		public void RpcLogic___SetReadyToSleep_1140765316(bool ready)
		{
			this.IsReadyToSleep = ready;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x0009ECA0 File Offset: 0x0009CEA0
		private void RpcReader___Server_SetReadyToSleep_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool ready = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetReadyToSleep_1140765316(ready);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x0009ECE0 File Offset: 0x0009CEE0
		private void RpcWriter___Observers_SetPlayerCode_3615296227(string code)
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
			writer.WriteString(code);
			base.SendObserversRpc(26U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x0009ED96 File Offset: 0x0009CF96
		public void RpcLogic___SetPlayerCode_3615296227(string code)
		{
			this.PlayerCode = code;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x0009EDA0 File Offset: 0x0009CFA0
		private void RpcReader___Observers_SetPlayerCode_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string code = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerCode_3615296227(code);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x0009EDDC File Offset: 0x0009CFDC
		private void RpcWriter___Server_SendPunch_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(27U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x0009EED0 File Offset: 0x0009D0D0
		public void RpcLogic___SendPunch_2166136261()
		{
			this.Punch();
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x0009EED8 File Offset: 0x0009D0D8
		private void RpcReader___Server_SendPunch_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___SendPunch_2166136261();
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x0009EF0C File Offset: 0x0009D10C
		private void RpcWriter___Observers_Punch_2166136261()
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
			base.SendObserversRpc(28U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x0009EFB5 File Offset: 0x0009D1B5
		private void RpcLogic___Punch_2166136261()
		{
			this.Avatar.Anim.SetTrigger("Punch");
			if (!base.IsOwner)
			{
				this.PunchSound.Play();
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x0009EFE0 File Offset: 0x0009D1E0
		private void RpcReader___Observers_Punch_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Punch_2166136261();
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x0009F000 File Offset: 0x0009D200
		private void RpcWriter___Server_MarkIntroCompleted_3281254764(BasicAvatarSettings appearance)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(29U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x0009F101 File Offset: 0x0009D301
		private void RpcLogic___MarkIntroCompleted_3281254764(BasicAvatarSettings appearance)
		{
			this.HasCompletedIntro = true;
			Console.Log(this.PlayerName + " has completed intro", null);
			this.SetAppearance(appearance, false);
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x0009F128 File Offset: 0x0009D328
		private void RpcReader___Server_MarkIntroCompleted_3281254764(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			BasicAvatarSettings appearance = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkIntroCompleted_3281254764(appearance);
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x0009F178 File Offset: 0x0009D378
		private void RpcWriter___Server_SendImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendServerRpc(30U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x0009F21F File Offset: 0x0009D41F
		public virtual void RpcLogic___SendImpact_427288424(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x0009F228 File Offset: 0x0009D428
		private void RpcReader___Server_SendImpact_427288424(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Impact impact = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x0009F268 File Offset: 0x0009D468
		private void RpcWriter___Observers_ReceiveImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendObserversRpc(31U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x0009F320 File Offset: 0x0009D520
		public virtual void RpcLogic___ReceiveImpact_427288424(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			float num = 1f;
			this.Health.TakeDamage(impact.ImpactDamage, true, impact.ImpactDamage > 0f);
			if (impact.ImpactType == EImpactType.Punch)
			{
				Singleton<SFXManager>.Instance.PlayImpactSound(ImpactSoundEntity.EMaterial.Punch, impact.HitPoint, impact.ImpactForce);
			}
			else if (impact.ImpactType == EImpactType.BluntMetal)
			{
				Singleton<SFXManager>.Instance.PlayImpactSound(ImpactSoundEntity.EMaterial.BaseballBat, impact.HitPoint, impact.ImpactForce);
			}
			this.ProcessImpactForce(impact.HitPoint, impact.ImpactForceDirection, impact.ImpactForce * num);
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x0009F3D4 File Offset: 0x0009D5D4
		private void RpcReader___Observers_ReceiveImpact_427288424(PooledReader PooledReader0, Channel channel)
		{
			Impact impact = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0009F410 File Offset: 0x0009D610
		private void RpcWriter___Observers_Arrest_2166136261()
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
			base.SendObserversRpc(32U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x0009F4BC File Offset: 0x0009D6BC
		public void RpcLogic___Arrest_2166136261()
		{
			if (this.IsArrested)
			{
				return;
			}
			if (this.onArrested != null)
			{
				this.onArrested.Invoke();
			}
			this.IsArrested = true;
			Debug.Log("Player arrested");
			if (!this.Health.IsAlive)
			{
				return;
			}
			if (base.IsOwner)
			{
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Arrested");
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.LONG_ARM_OF_THE_LAW);
				Singleton<ArrestScreen>.Instance.Open();
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x0009F538 File Offset: 0x0009D738
		private void RpcReader___Observers_Arrest_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Arrest_2166136261();
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0009F564 File Offset: 0x0009D764
		private void RpcWriter___Server_SendPassOut_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(33U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x0009F658 File Offset: 0x0009D858
		public void RpcLogic___SendPassOut_2166136261()
		{
			this.PassOut();
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x0009F660 File Offset: 0x0009D860
		private void RpcReader___Server_SendPassOut_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPassOut_2166136261();
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x0009F6A0 File Offset: 0x0009D8A0
		private void RpcWriter___Observers_PassOut_2166136261()
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
			base.SendObserversRpc(34U, writer, channel, 0, false, false, true);
			writer.Store();
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x0009F74C File Offset: 0x0009D94C
		public void RpcLogic___PassOut_2166136261()
		{
			this.IsUnconscious = true;
			if (this.onPassedOut != null)
			{
				this.onPassedOut.Invoke();
			}
			this.CapCol.enabled = false;
			this.SetRagdolled(true);
			this.Avatar.MiddleSpineRB.AddForce(base.transform.forward * 30f, 2);
			this.Avatar.MiddleSpineRB.AddRelativeTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f, 2);
			if (!this.Health.IsAlive)
			{
				return;
			}
			if (this.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.IsArrested = true;
			}
			if (base.IsOwner)
			{
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Passed out");
				Singleton<PassOutScreen>.Instance.Open();
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x0009F838 File Offset: 0x0009DA38
		private void RpcReader___Observers_PassOut_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PassOut_2166136261();
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x0009F864 File Offset: 0x0009DA64
		private void RpcWriter___Server_SendPassOutRecovery_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(35U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x0009F958 File Offset: 0x0009DB58
		public void RpcLogic___SendPassOutRecovery_2166136261()
		{
			this.PassOutRecovery();
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x0009F960 File Offset: 0x0009DB60
		private void RpcReader___Server_SendPassOutRecovery_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPassOutRecovery_2166136261();
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x0009F9A0 File Offset: 0x0009DBA0
		private void RpcWriter___Observers_PassOutRecovery_2166136261()
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
			base.SendObserversRpc(36U, writer, channel, 0, false, false, true);
			writer.Store();
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x0009FA4C File Offset: 0x0009DC4C
		public void RpcLogic___PassOutRecovery_2166136261()
		{
			Debug.Log("Player recovered from pass out");
			this.IsUnconscious = false;
			this.SetRagdolled(false);
			this.CapCol.enabled = true;
			if (base.IsOwner)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
				this.Energy.RestoreEnergy();
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Passed out");
			}
			if (this.onPassOutRecovery != null)
			{
				this.onPassOutRecovery.Invoke();
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0009FAC4 File Offset: 0x0009DCC4
		private void RpcReader___Observers_PassOutRecovery_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PassOutRecovery_2166136261();
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x0009FAF0 File Offset: 0x0009DCF0
		private void RpcWriter___Server_SendEquippable_Networked_3615296227(string assetPath)
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
			writer.WriteString(assetPath);
			base.SendServerRpc(37U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x0009FB97 File Offset: 0x0009DD97
		public void RpcLogic___SendEquippable_Networked_3615296227(string assetPath)
		{
			this.SetEquippable_Networked(assetPath);
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x0009FBA0 File Offset: 0x0009DDA0
		private void RpcReader___Server_SendEquippable_Networked_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x0009FBE0 File Offset: 0x0009DDE0
		private void RpcWriter___Observers_SetEquippable_Networked_3615296227(string assetPath)
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
			writer.WriteString(assetPath);
			base.SendObserversRpc(38U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x0009FC96 File Offset: 0x0009DE96
		private void RpcLogic___SetEquippable_Networked_3615296227(string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x0009FCA8 File Offset: 0x0009DEA8
		private void RpcReader___Observers_SetEquippable_Networked_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x0009FCE4 File Offset: 0x0009DEE4
		private void RpcWriter___Server_SendEquippableMessage_Networked_3643459082(string message, int receipt)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteInt32(receipt, 1);
			base.SendServerRpc(39U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x0009FDF7 File Offset: 0x0009DFF7
		public void RpcLogic___SendEquippableMessage_Networked_3643459082(string message, int receipt)
		{
			this.ReceiveEquippableMessage_Networked(message, receipt);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x0009FE04 File Offset: 0x0009E004
		private void RpcReader___Server_SendEquippableMessage_Networked_3643459082(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0009FE6C File Offset: 0x0009E06C
		private void RpcWriter___Observers_ReceiveEquippableMessage_Networked_3643459082(string message, int receipt)
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
			writer.WriteString(message);
			writer.WriteInt32(receipt, 1);
			base.SendObserversRpc(40U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0009FF34 File Offset: 0x0009E134
		private void RpcLogic___ReceiveEquippableMessage_Networked_3643459082(string message, int receipt)
		{
			if (this.equippableMessageIDHistory.Contains(receipt))
			{
				return;
			}
			this.equippableMessageIDHistory.Add(receipt);
			this.Avatar.ReceiveEquippableMessage(message, null);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x0009FF60 File Offset: 0x0009E160
		private void RpcReader___Observers_ReceiveEquippableMessage_Networked_3643459082(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x0009FFB4 File Offset: 0x0009E1B4
		private void RpcWriter___Server_SendEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteInt32(receipt, 1);
			writer.WriteVector3(data);
			base.SendServerRpc(41U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000A00D4 File Offset: 0x0009E2D4
		public void RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
		{
			this.ReceiveEquippableMessage_Networked_Vector(message, receipt, data);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000A00E0 File Offset: 0x0009E2E0
		private void RpcReader___Server_SendEquippableMessage_Networked_Vector_3190074053(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(1);
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000A0158 File Offset: 0x0009E358
		private void RpcWriter___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
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
			writer.WriteString(message);
			writer.WriteInt32(receipt, 1);
			writer.WriteVector3(data);
			base.SendObserversRpc(42U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000A022D File Offset: 0x0009E42D
		private void RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
		{
			if (this.equippableMessageIDHistory.Contains(receipt))
			{
				return;
			}
			this.equippableMessageIDHistory.Add(receipt);
			this.Avatar.ReceiveEquippableMessage(message, data);
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000A025C File Offset: 0x0009E45C
		private void RpcReader___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(1);
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000A02C0 File Offset: 0x0009E4C0
		private void RpcWriter___Server_SendAnimationTrigger_3615296227(string trigger)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendServerRpc(43U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000A03C1 File Offset: 0x0009E5C1
		public void RpcLogic___SendAnimationTrigger_3615296227(string trigger)
		{
			this.SetAnimationTrigger_Networked(null, trigger);
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000A03CC File Offset: 0x0009E5CC
		private void RpcReader___Server_SendAnimationTrigger_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000A041C File Offset: 0x0009E61C
		private void RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(44U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000A04D2 File Offset: 0x0009E6D2
		public void RpcLogic___SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.SetAnimationTrigger(trigger);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000A04DC File Offset: 0x0009E6DC
		private void RpcReader___Observers_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000A0518 File Offset: 0x0009E718
		private void RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(45U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000A05D0 File Offset: 0x0009E7D0
		private void RpcReader___Target_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000A0608 File Offset: 0x0009E808
		private void RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(46U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000A06BE File Offset: 0x0009E8BE
		public void RpcLogic___ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.ResetAnimationTrigger(trigger);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000A06C8 File Offset: 0x0009E8C8
		private void RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000A0704 File Offset: 0x0009E904
		private void RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(47U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000A07BC File Offset: 0x0009E9BC
		private void RpcReader___Target_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000A07F4 File Offset: 0x0009E9F4
		private void RpcWriter___Server_SendAnimationBool_310431262(string name, bool val)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteBoolean(val);
			base.SendServerRpc(48U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000A0902 File Offset: 0x0009EB02
		public void RpcLogic___SendAnimationBool_310431262(string name, bool val)
		{
			this.SetAnimationBool(name, val);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000A090C File Offset: 0x0009EB0C
		private void RpcReader___Server_SendAnimationBool_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			bool val = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAnimationBool_310431262(name, val);
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000A096C File Offset: 0x0009EB6C
		private void RpcWriter___Observers_SetAnimationBool_310431262(string name, bool val)
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
			writer.WriteString(name);
			writer.WriteBoolean(val);
			base.SendObserversRpc(49U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000A0A2F File Offset: 0x0009EC2F
		public void RpcLogic___SetAnimationBool_310431262(string name, bool val)
		{
			this.Avatar.Anim.SetBool(name, val);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000A0A44 File Offset: 0x0009EC44
		private void RpcReader___Observers_SetAnimationBool_310431262(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			bool val = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_310431262(name, val);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000A0A90 File Offset: 0x0009EC90
		private void RpcWriter___Observers_Taze_2166136261()
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
			base.SendObserversRpc(50U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000A0B3C File Offset: 0x0009ED3C
		public void RpcLogic___Taze_2166136261()
		{
			this.IsTased = true;
			if (this.onTased != null)
			{
				this.onTased.Invoke();
			}
			if (this.taseCoroutine != null)
			{
				base.StopCoroutine(this.taseCoroutine);
			}
			this.taseCoroutine = base.StartCoroutine(this.<Taze>g__Tase|321_0());
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000A0B8C File Offset: 0x0009ED8C
		private void RpcReader___Observers_Taze_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Taze_2166136261();
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000A0BAC File Offset: 0x0009EDAC
		private void RpcWriter___Server_SetInventoryItem_2317364410(int index, ItemInstance item)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(index, 1);
			writer.WriteItemInstance(item);
			base.SendServerRpc(51U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000A0CBF File Offset: 0x0009EEBF
		public void RpcLogic___SetInventoryItem_2317364410(int index, ItemInstance item)
		{
			this.Inventory[index].SetStoredItem(item, false);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000A0CD0 File Offset: 0x0009EED0
		private void RpcReader___Server_SetInventoryItem_2317364410(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(1);
			ItemInstance item = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetInventoryItem_2317364410(index, item);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000A0D38 File Offset: 0x0009EF38
		private void RpcWriter___Server_SendAppearance_3281254764(BasicAvatarSettings settings)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendServerRpc(52U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000A0E39 File Offset: 0x0009F039
		public void RpcLogic___SendAppearance_3281254764(BasicAvatarSettings settings)
		{
			this.SetAppearance(settings, true);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000A0E44 File Offset: 0x0009F044
		private void RpcReader___Server_SendAppearance_3281254764(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			BasicAvatarSettings settings = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAppearance_3281254764(settings);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000A0E94 File Offset: 0x0009F094
		private void RpcWriter___Observers_SetAppearance_2139595489(BasicAvatarSettings settings, bool refreshClothing)
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
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(settings);
			writer.WriteBoolean(refreshClothing);
			base.SendObserversRpc(53U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000A0F58 File Offset: 0x0009F158
		public void RpcLogic___SetAppearance_2139595489(BasicAvatarSettings settings, bool refreshClothing)
		{
			this.CurrentAvatarSettings = settings;
			AvatarSettings avatarSettings = this.CurrentAvatarSettings.GetAvatarSettings();
			this.Avatar.LoadAvatarSettings(avatarSettings);
			if (refreshClothing)
			{
				this.Clothing.RefreshAppearance();
			}
			this.SetVisibleToLocalPlayer(!base.IsOwner);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000A0FA4 File Offset: 0x0009F1A4
		private void RpcReader___Observers_SetAppearance_2139595489(PooledReader PooledReader0, Channel channel)
		{
			BasicAvatarSettings settings = GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			bool refreshClothing = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAppearance_2139595489(settings, refreshClothing);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000A0FF0 File Offset: 0x0009F1F0
		private void RpcWriter___Server_SendMountedSkateboard_3323014238(NetworkObject skateboardObj)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(skateboardObj);
			base.SendServerRpc(54U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000A10F1 File Offset: 0x0009F2F1
		private void RpcLogic___SendMountedSkateboard_3323014238(NetworkObject skateboardObj)
		{
			this.SetMountedSkateboard(skateboardObj);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000A10FC File Offset: 0x0009F2FC
		private void RpcReader___Server_SendMountedSkateboard_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject skateboardObj = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000A114C File Offset: 0x0009F34C
		private void RpcWriter___Observers_SetMountedSkateboard_3323014238(NetworkObject skateboardObj)
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
			writer.WriteNetworkObject(skateboardObj);
			base.SendObserversRpc(55U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000A1204 File Offset: 0x0009F404
		private void RpcLogic___SetMountedSkateboard_3323014238(NetworkObject skateboardObj)
		{
			if (skateboardObj != null)
			{
				if (this.ActiveSkateboard != null)
				{
					return;
				}
				Skateboard component = skateboardObj.GetComponent<Skateboard>();
				this.IsSkating = true;
				this.ActiveSkateboard = component;
				base.transform.SetParent(component.PlayerContainer);
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				if (this.onSkateboardMounted != null)
				{
					this.onSkateboardMounted(component);
					return;
				}
			}
			else
			{
				if (this.ActiveSkateboard == null)
				{
					return;
				}
				this.IsSkating = false;
				this.ActiveSkateboard = null;
				base.transform.SetParent(null);
				base.transform.rotation = Quaternion.LookRotation(base.transform.forward, Vector3.up);
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				if (this.onSkateboardDismounted != null)
				{
					this.onSkateboardDismounted();
				}
			}
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000A1310 File Offset: 0x0009F510
		private void RpcReader___Observers_SetMountedSkateboard_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject skateboardObj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000A134C File Offset: 0x0009F54C
		private void RpcWriter___Server_SendConsumeProduct_2622925554(ProductItemInstance product)
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
			writer.WriteProductItemInstance(product);
			base.SendServerRpc(56U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000A13F3 File Offset: 0x0009F5F3
		private void RpcLogic___SendConsumeProduct_2622925554(ProductItemInstance product)
		{
			this.ReceiveConsumeProduct(product);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000A13FC File Offset: 0x0009F5FC
		private void RpcReader___Server_SendConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendConsumeProduct_2622925554(product);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000A1430 File Offset: 0x0009F630
		private void RpcWriter___Observers_ReceiveConsumeProduct_2622925554(ProductItemInstance product)
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
			writer.WriteProductItemInstance(product);
			base.SendObserversRpc(57U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000A14E6 File Offset: 0x0009F6E6
		private void RpcLogic___ReceiveConsumeProduct_2622925554(ProductItemInstance product)
		{
			if (base.IsOwner)
			{
				return;
			}
			this.ConsumeProductInternal(product);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000A14F8 File Offset: 0x0009F6F8
		private void RpcReader___Observers_ReceiveConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveConsumeProduct_2622925554(product);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000A152C File Offset: 0x0009F72C
		private void RpcWriter___Server_SendValue_3589193952(string variableName, string value, bool sendToOwner)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			writer.WriteBoolean(sendToOwner);
			base.SendServerRpc(58U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000A15ED File Offset: 0x0009F7ED
		public void RpcLogic___SendValue_3589193952(string variableName, string value, bool sendToOwner)
		{
			if (sendToOwner || !base.IsOwner)
			{
				this.ReceiveValue(variableName, value);
			}
			if (sendToOwner)
			{
				this.ReceiveValue(base.Owner, variableName, value);
			}
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000A1614 File Offset: 0x0009F814
		private void RpcReader___Server_SendValue_3589193952(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			bool sendToOwner = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendValue_3589193952(variableName, value, sendToOwner);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000A1674 File Offset: 0x0009F874
		private void RpcWriter___Target_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendTargetRpc(59U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000A1736 File Offset: 0x0009F936
		private void RpcLogic___ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			this.ReceiveValue(variableName, value);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000A1740 File Offset: 0x0009F940
		private void RpcReader___Target_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(base.LocalConnection, variableName, value);
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000A1788 File Offset: 0x0009F988
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x000A1790 File Offset: 0x0009F990
		public string SyncAccessor_<PlayerName>k__BackingField
		{
			get
			{
				return this.<PlayerName>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerName>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerName>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000A17CC File Offset: 0x0009F9CC
		public override bool Player(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 6U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CameraRotation>k__BackingField(this.syncVar___<CameraRotation>k__BackingField.GetValue(true), true);
					return true;
				}
				Quaternion value = PooledReader0.ReadQuaternion(1);
				this.sync___set_value_<CameraRotation>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 5U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CameraPosition>k__BackingField(this.syncVar___<CameraPosition>k__BackingField.GetValue(true), true);
					return true;
				}
				Vector3 value2 = PooledReader0.ReadVector3();
				this.sync___set_value_<CameraPosition>k__BackingField(value2, Boolean2);
				return true;
			}
			else if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<IsReadyToSleep>k__BackingField(this.syncVar___<IsReadyToSleep>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value3 = PooledReader0.ReadBoolean();
				this.sync___set_value_<IsReadyToSleep>k__BackingField(value3, Boolean2);
				return true;
			}
			else if (UInt321 == 3U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentBed>k__BackingField(this.syncVar___<CurrentBed>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value4 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentBed>k__BackingField(value4, Boolean2);
				return true;
			}
			else if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentVehicle>k__BackingField(this.syncVar___<CurrentVehicle>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value5 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentVehicle>k__BackingField(value5, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerCode>k__BackingField(this.syncVar___<PlayerCode>k__BackingField.GetValue(true), true);
					return true;
				}
				string value6 = PooledReader0.ReadString();
				this.sync___set_value_<PlayerCode>k__BackingField(value6, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerName>k__BackingField(this.syncVar___<PlayerName>k__BackingField.GetValue(true), true);
					return true;
				}
				string value7 = PooledReader0.ReadString();
				this.sync___set_value_<PlayerName>k__BackingField(value7, Boolean2);
				return true;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000A19BB File Offset: 0x0009FBBB
		// (set) Token: 0x0600278C RID: 10124 RVA: 0x000A19C3 File Offset: 0x0009FBC3
		public string SyncAccessor_<PlayerCode>k__BackingField
		{
			get
			{
				return this.<PlayerCode>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerCode>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerCode>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000A19FF File Offset: 0x0009FBFF
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x000A1A07 File Offset: 0x0009FC07
		public NetworkObject SyncAccessor_<CurrentVehicle>k__BackingField
		{
			get
			{
				return this.<CurrentVehicle>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentVehicle>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentVehicle>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000A1A43 File Offset: 0x0009FC43
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000A1A4B File Offset: 0x0009FC4B
		public NetworkObject SyncAccessor_<CurrentBed>k__BackingField
		{
			get
			{
				return this.<CurrentBed>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentBed>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentBed>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x000A1A87 File Offset: 0x0009FC87
		// (set) Token: 0x06002792 RID: 10130 RVA: 0x000A1A8F File Offset: 0x0009FC8F
		public bool SyncAccessor_<IsReadyToSleep>k__BackingField
		{
			get
			{
				return this.<IsReadyToSleep>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<IsReadyToSleep>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<IsReadyToSleep>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000A1ACB File Offset: 0x0009FCCB
		// (set) Token: 0x06002794 RID: 10132 RVA: 0x000A1AD3 File Offset: 0x0009FCD3
		public Vector3 SyncAccessor_<CameraPosition>k__BackingField
		{
			get
			{
				return this.<CameraPosition>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CameraPosition>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CameraPosition>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x000A1B0F File Offset: 0x0009FD0F
		// (set) Token: 0x06002796 RID: 10134 RVA: 0x000A1B17 File Offset: 0x0009FD17
		public Quaternion SyncAccessor_<CameraRotation>k__BackingField
		{
			get
			{
				return this.<CameraRotation>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CameraRotation>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CameraRotation>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A1B54 File Offset: 0x0009FD54
		protected virtual void dll()
		{
			if (InstanceFinder.NetworkManager == null)
			{
				Player.Local = this;
			}
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.SleepStart));
			TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			this.Health.onDie.AddListener(new UnityAction(this.OnDied));
			this.Health.onRevive.AddListener(new UnityAction(this.OnRevived));
			this.Energy.onEnergyDepleted.AddListener(new UnityAction(this.SendPassOut));
			base.InvokeRepeating("RecalculateCurrentProperty", 0f, 0.5f);
			this.InitializeSaveable();
			this.Inventory = new ItemSlot[9];
			for (int i = 0; i < this.Inventory.Length; i++)
			{
				this.Inventory[i] = new ItemSlot();
			}
			foreach (Rigidbody rigidbody in this.Avatar.RagdollRBs)
			{
				Physics.IgnoreCollision(rigidbody.GetComponent<Collider>(), this.CapCol, true);
				this.ragdollForceComponents.Add(rigidbody.gameObject.AddComponent<ConstantForce>());
			}
			this.EyePosition = this.Avatar.Eyes.transform.position;
			this.SetGravityMultiplier(1f);
		}

		// Token: 0x04001C1D RID: 7197
		public const string OWNER_PLAYER_CODE = "Local";

		// Token: 0x04001C1E RID: 7198
		public const float CapColDefaultHeight = 2f;

		// Token: 0x04001C1F RID: 7199
		public List<NetworkObject> objectsTemporarilyOwnedByPlayer = new List<NetworkObject>();

		// Token: 0x04001C20 RID: 7200
		public static Action onLocalPlayerSpawned;

		// Token: 0x04001C21 RID: 7201
		public static Action<Player> onPlayerSpawned;

		// Token: 0x04001C22 RID: 7202
		public static Action<Player> onPlayerDespawned;

		// Token: 0x04001C23 RID: 7203
		public static Player Local;

		// Token: 0x04001C24 RID: 7204
		public static List<Player> PlayerList = new List<Player>();

		// Token: 0x04001C25 RID: 7205
		[Header("References")]
		public GameObject LocalGameObject;

		// Token: 0x04001C26 RID: 7206
		public Avatar Avatar;

		// Token: 0x04001C27 RID: 7207
		public AvatarAnimation Anim;

		// Token: 0x04001C28 RID: 7208
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04001C29 RID: 7209
		public Vector3 EyePosition = Vector3.zero;

		// Token: 0x04001C2A RID: 7210
		public AvatarSettings TestAvatarSettings;

		// Token: 0x04001C2B RID: 7211
		public PlayerVisualState VisualState;

		// Token: 0x04001C2C RID: 7212
		public PlayerVisibility Visibility;

		// Token: 0x04001C2D RID: 7213
		public CapsuleCollider CapCol;

		// Token: 0x04001C2E RID: 7214
		public POI PoI;

		// Token: 0x04001C2F RID: 7215
		public PlayerHealth Health;

		// Token: 0x04001C30 RID: 7216
		public PlayerCrimeData CrimeData;

		// Token: 0x04001C31 RID: 7217
		public PlayerEnergy Energy;

		// Token: 0x04001C32 RID: 7218
		public Transform MimicCamera;

		// Token: 0x04001C33 RID: 7219
		public AvatarFootstepDetector FootstepDetector;

		// Token: 0x04001C34 RID: 7220
		public LocalPlayerFootstepGenerator LocalFootstepDetector;

		// Token: 0x04001C35 RID: 7221
		public CharacterController CharacterController;

		// Token: 0x04001C36 RID: 7222
		public AudioSourceController PunchSound;

		// Token: 0x04001C37 RID: 7223
		public OptimizedLight ThirdPersonFlashlight;

		// Token: 0x04001C38 RID: 7224
		public WorldspaceDialogueRenderer NameLabel;

		// Token: 0x04001C39 RID: 7225
		public PlayerClothing Clothing;

		// Token: 0x04001C3A RID: 7226
		[Header("Settings")]
		public LayerMask GroundDetectionMask;

		// Token: 0x04001C3B RID: 7227
		public float AvatarOffset_Standing = -0.97f;

		// Token: 0x04001C3C RID: 7228
		public float AvatarOffset_Crouched = -0.45f;

		// Token: 0x04001C3D RID: 7229
		[Header("Movement mapping")]
		public AnimationCurve WalkingMapCurve;

		// Token: 0x04001C3E RID: 7230
		public AnimationCurve CrouchWalkMapCurve;

		// Token: 0x04001C40 RID: 7232
		public NetworkConnection Connection;

		// Token: 0x04001C43 RID: 7235
		public Player.VehicleEvent onEnterVehicle;

		// Token: 0x04001C44 RID: 7236
		public Player.VehicleTransformEvent onExitVehicle;

		// Token: 0x04001C45 RID: 7237
		public LandVehicle LastDrivenVehicle;

		// Token: 0x04001C4C RID: 7244
		public Action<Skateboard> onSkateboardMounted;

		// Token: 0x04001C4D RID: 7245
		public Action onSkateboardDismounted;

		// Token: 0x04001C57 RID: 7255
		public bool HasCompletedIntro;

		// Token: 0x04001C5A RID: 7258
		public ItemSlot[] Inventory;

		// Token: 0x04001C5E RID: 7262
		[Header("Appearance debugging")]
		public BasicAvatarSettings DebugAvatarSettings;

		// Token: 0x04001C5F RID: 7263
		private PlayerLoader loader;

		// Token: 0x04001C63 RID: 7267
		public UnityEvent onRagdoll;

		// Token: 0x04001C64 RID: 7268
		public UnityEvent onRagdollEnd;

		// Token: 0x04001C65 RID: 7269
		public UnityEvent onArrested;

		// Token: 0x04001C66 RID: 7270
		public UnityEvent onFreed;

		// Token: 0x04001C67 RID: 7271
		public UnityEvent onTased;

		// Token: 0x04001C68 RID: 7272
		public UnityEvent onTasedEnd;

		// Token: 0x04001C69 RID: 7273
		public UnityEvent onPassedOut;

		// Token: 0x04001C6A RID: 7274
		public UnityEvent onPassOutRecovery;

		// Token: 0x04001C6B RID: 7275
		public List<BaseVariable> PlayerVariables;

		// Token: 0x04001C6C RID: 7276
		public Dictionary<string, BaseVariable> VariableDict;

		// Token: 0x04001C6D RID: 7277
		private float standingScale;

		// Token: 0x04001C6E RID: 7278
		private float timeAirborne;

		// Token: 0x04001C71 RID: 7281
		private Coroutine taseCoroutine;

		// Token: 0x04001C72 RID: 7282
		private List<ConstantForce> ragdollForceComponents;

		// Token: 0x04001C75 RID: 7285
		private List<int> impactHistory;

		// Token: 0x04001C7A RID: 7290
		private List<Quaternion> seizureRotations;

		// Token: 0x04001C7D RID: 7293
		private List<int> equippableMessageIDHistory;

		// Token: 0x04001C7E RID: 7294
		private Coroutine lerpScaleRoutine;

		// Token: 0x04001C7F RID: 7295
		public SyncVar<string> syncVar___<PlayerName>k__BackingField;

		// Token: 0x04001C80 RID: 7296
		public SyncVar<string> syncVar___<PlayerCode>k__BackingField;

		// Token: 0x04001C81 RID: 7297
		public SyncVar<NetworkObject> syncVar___<CurrentVehicle>k__BackingField;

		// Token: 0x04001C82 RID: 7298
		public SyncVar<NetworkObject> syncVar___<CurrentBed>k__BackingField;

		// Token: 0x04001C83 RID: 7299
		public SyncVar<bool> syncVar___<IsReadyToSleep>k__BackingField;

		// Token: 0x04001C84 RID: 7300
		public SyncVar<Vector3> syncVar___<CameraPosition>k__BackingField;

		// Token: 0x04001C85 RID: 7301
		public SyncVar<Quaternion> syncVar___<CameraRotation>k__BackingField;

		// Token: 0x04001C86 RID: 7302
		private bool dll_Excuted;

		// Token: 0x04001C87 RID: 7303
		private bool dll_Excuted;

		// Token: 0x02000615 RID: 1557
		// (Invoke) Token: 0x06002799 RID: 10137
		public delegate void VehicleEvent(LandVehicle vehicle);

		// Token: 0x02000616 RID: 1558
		// (Invoke) Token: 0x0600279D RID: 10141
		public delegate void VehicleTransformEvent(LandVehicle vehicle, Transform exitPoint);
	}
}
