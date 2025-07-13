using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EPOOutline;
using FishNet;
using FishNet.Component.Ownership;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using Pathfinding;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.Vehicles.Modification;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000800 RID: 2048
	[RequireComponent(typeof(VehicleCamera))]
	[RequireComponent(typeof(NetworkTransform))]
	[RequireComponent(typeof(PredictedOwner))]
	[RequireComponent(typeof(VehicleCollisionDetector))]
	[RequireComponent(typeof(PhysicsDamageable))]
	public class LandVehicle : NetworkBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x000E84F4 File Offset: 0x000E66F4
		public string VehicleName
		{
			get
			{
				return this.vehicleName;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x000E84FC File Offset: 0x000E66FC
		public string VehicleCode
		{
			get
			{
				return this.vehicleCode;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003732 RID: 14130 RVA: 0x000E8504 File Offset: 0x000E6704
		public float VehiclePrice
		{
			get
			{
				return this.vehiclePrice;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x000E850C File Offset: 0x000E670C
		// (set) Token: 0x06003734 RID: 14132 RVA: 0x000E8514 File Offset: 0x000E6714
		public bool IsPlayerOwned { get; protected set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x000E851D File Offset: 0x000E671D
		// (set) Token: 0x06003736 RID: 14134 RVA: 0x000E8525 File Offset: 0x000E6725
		public bool IsVisible { get; protected set; } = true;

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x000E852E File Offset: 0x000E672E
		// (set) Token: 0x06003738 RID: 14136 RVA: 0x000E8536 File Offset: 0x000E6736
		public Guid GUID { get; protected set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x000E853F File Offset: 0x000E673F
		// (set) Token: 0x0600373A RID: 14138 RVA: 0x000E8547 File Offset: 0x000E6747
		public float DistanceToLocalCamera { get; private set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x000E8550 File Offset: 0x000E6750
		public Vector3 boundingBoxDimensions
		{
			get
			{
				return new Vector3(this.boundingBox.size.x * this.boundingBox.transform.localScale.x, this.boundingBox.size.y * this.boundingBox.transform.localScale.y, this.boundingBox.size.z * this.boundingBox.transform.localScale.z);
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x000E85D4 File Offset: 0x000E67D4
		public Transform driverEntryPoint
		{
			get
			{
				return this.exitPoints[0];
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x000E85E2 File Offset: 0x000E67E2
		public Rigidbody Rb
		{
			get
			{
				return this.rb;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600373E RID: 14142 RVA: 0x000E85EA File Offset: 0x000E67EA
		public float ActualMaxSteeringAngle
		{
			get
			{
				if (!this.MaxSteerAngleOverridden)
				{
					return this.maxSteeringAngle;
				}
				return this.OverriddenMaxSteerAngle;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x000E8601 File Offset: 0x000E6801
		// (set) Token: 0x06003740 RID: 14144 RVA: 0x000E8609 File Offset: 0x000E6809
		public bool MaxSteerAngleOverridden { get; private set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x000E8612 File Offset: 0x000E6812
		// (set) Token: 0x06003742 RID: 14146 RVA: 0x000E861A File Offset: 0x000E681A
		public float OverriddenMaxSteerAngle { get; private set; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x000E8623 File Offset: 0x000E6823
		// (set) Token: 0x06003744 RID: 14148 RVA: 0x000E862B File Offset: 0x000E682B
		public EVehicleColor OwnedColor { get; private set; } = EVehicleColor.White;

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x000E8634 File Offset: 0x000E6834
		public int Capacity
		{
			get
			{
				return this.Seats.Length;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06003746 RID: 14150 RVA: 0x000E863E File Offset: 0x000E683E
		public int CurrentPlayerOccupancy
		{
			get
			{
				return this.Seats.Count((VehicleSeat s) => s.isOccupied);
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x000E866A File Offset: 0x000E686A
		// (set) Token: 0x06003748 RID: 14152 RVA: 0x000E8672 File Offset: 0x000E6872
		public bool localPlayerIsDriver { get; protected set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000E867B File Offset: 0x000E687B
		// (set) Token: 0x0600374A RID: 14154 RVA: 0x000E8683 File Offset: 0x000E6883
		public bool localPlayerIsInVehicle { get; protected set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x000E868C File Offset: 0x000E688C
		// (set) Token: 0x0600374C RID: 14156 RVA: 0x000E8694 File Offset: 0x000E6894
		public bool isOccupied { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x000E869D File Offset: 0x000E689D
		public Player DriverPlayer
		{
			get
			{
				if (this.Seats[0].Occupant != null)
				{
					return this.Seats[0].Occupant;
				}
				return null;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000E86C4 File Offset: 0x000E68C4
		public List<Player> OccupantPlayers
		{
			get
			{
				return (from s in this.Seats
				where s.isOccupied
				select s.Occupant).ToList<Player>();
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600374F RID: 14159 RVA: 0x000E8724 File Offset: 0x000E6924
		// (set) Token: 0x06003750 RID: 14160 RVA: 0x000E872C File Offset: 0x000E692C
		public NPC[] OccupantNPCs { get; protected set; } = new NPC[0];

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06003751 RID: 14161 RVA: 0x000E8735 File Offset: 0x000E6935
		// (set) Token: 0x06003752 RID: 14162 RVA: 0x000E873D File Offset: 0x000E693D
		public float speed_Kmh { get; protected set; }

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x000E8746 File Offset: 0x000E6946
		public float speed_Ms
		{
			get
			{
				return this.speed_Kmh / 3.6f;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x000E8754 File Offset: 0x000E6954
		public float speed_Mph
		{
			get
			{
				return this.speed_Kmh * 0.621371f;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x000E8762 File Offset: 0x000E6962
		// (set) Token: 0x06003756 RID: 14166 RVA: 0x000E876A File Offset: 0x000E696A
		public float currentThrottle { get; protected set; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x000E8773 File Offset: 0x000E6973
		// (set) Token: 0x06003758 RID: 14168 RVA: 0x000E877B File Offset: 0x000E697B
		public bool brakesApplied
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<brakesApplied>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<brakesApplied>k__BackingField(value, true);
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x000E8785 File Offset: 0x000E6985
		// (set) Token: 0x0600375A RID: 14170 RVA: 0x000E878D File Offset: 0x000E698D
		public bool isReversing
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<isReversing>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<isReversing>k__BackingField(value, true);
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x000E8797 File Offset: 0x000E6997
		// (set) Token: 0x0600375C RID: 14172 RVA: 0x000E879F File Offset: 0x000E699F
		public bool isStatic { get; protected set; }

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x000E87A8 File Offset: 0x000E69A8
		// (set) Token: 0x0600375E RID: 14174 RVA: 0x000E87B0 File Offset: 0x000E69B0
		public bool handbrakeApplied { get; protected set; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000E87B9 File Offset: 0x000E69B9
		public float boundingBaseOffset
		{
			get
			{
				return base.transform.InverseTransformPoint(this.boundingBox.transform.position).y + this.boundingBox.size.y * 0.5f;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x000E87F2 File Offset: 0x000E69F2
		public bool isParked
		{
			get
			{
				return this.CurrentParkingLot != null;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x000E8800 File Offset: 0x000E6A00
		// (set) Token: 0x06003762 RID: 14178 RVA: 0x000E8808 File Offset: 0x000E6A08
		public ParkingLot CurrentParkingLot { get; protected set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x000E8811 File Offset: 0x000E6A11
		// (set) Token: 0x06003764 RID: 14180 RVA: 0x000E8819 File Offset: 0x000E6A19
		public ParkingSpot CurrentParkingSpot { get; protected set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x000E8824 File Offset: 0x000E6A24
		public string SaveFolderName
		{
			get
			{
				return this.vehicleCode + "_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x000E885C File Offset: 0x000E6A5C
		public string SaveFileName
		{
			get
			{
				return "Vehicle";
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x000E8863 File Offset: 0x000E6A63
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x000E886B File Offset: 0x000E6A6B
		// (set) Token: 0x0600376A RID: 14186 RVA: 0x000E8873 File Offset: 0x000E6A73
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x000E887C File Offset: 0x000E6A7C
		// (set) Token: 0x0600376C RID: 14188 RVA: 0x000E8884 File Offset: 0x000E6A84
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x000E888D File Offset: 0x000E6A8D
		// (set) Token: 0x0600376E RID: 14190 RVA: 0x000E8895 File Offset: 0x000E6A95
		public bool HasChanged { get; set; } = true;

		// Token: 0x0600376F RID: 14191 RVA: 0x000E88A0 File Offset: 0x000E6AA0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.LandVehicle_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000E88C0 File Offset: 0x000E6AC0
		public override void OnStartServer()
		{
			base.OnStartServer();
			base.NetworkObject.GiveOwnership(base.LocalConnection);
			this.rb.isKinematic = false;
			this.rb.interpolation = 1;
			if (this.SpawnAsPlayerOwned)
			{
				this.IsPlayerOwned = true;
				this.SetIsPlayerOwned(null, true);
			}
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000E8914 File Offset: 0x000E6B14
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			this.SetOwnedColor(connection, this.OwnedColor);
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (this.Seats[i].Occupant != null)
				{
					this.SetSeatOccupant(connection, i, this.Seats[i].Occupant.Connection);
				}
			}
			if (this.isParked)
			{
				this.Park_Networked(connection, this.CurrentParkData);
			}
			if (this.IsPlayerOwned)
			{
				this.SetIsPlayerOwned(connection, true);
			}
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000E89A5 File Offset: 0x000E6BA5
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.rb.isKinematic = false;
			if (!base.IsOwner && !InstanceFinder.IsHost)
			{
				this.rb.isKinematic = true;
			}
			this.rb.interpolation = 1;
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000E89E0 File Offset: 0x000E6BE0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetIsPlayerOwned(NetworkConnection conn, bool playerOwned)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsPlayerOwned_214505783(conn, playerOwned);
				this.RpcLogic___SetIsPlayerOwned_214505783(conn, playerOwned);
			}
			else
			{
				this.RpcWriter___Target_SetIsPlayerOwned_214505783(conn, playerOwned);
			}
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000E8A24 File Offset: 0x000E6C24
		private void RefreshPoI()
		{
			if (this.POI != null)
			{
				if (this.IsPlayerOwned)
				{
					this.POI.SetMainText(string.Concat(new string[]
					{
						"Owned Vehicle\n(",
						Singleton<VehicleColors>.Instance.GetColorName(this.OwnedColor),
						" ",
						this.VehicleName,
						")"
					}));
					this.POI.enabled = true;
					return;
				}
				this.POI.enabled = false;
			}
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000E8AAA File Offset: 0x000E6CAA
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000E8ABC File Offset: 0x000E6CBC
		protected virtual void Start()
		{
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			if (this.centerOfMass != null)
			{
				this.rb.centerOfMass = base.transform.InverseTransformPoint(this.centerOfMass.transform.position);
			}
			this.ApplyOwnedColor();
			if (this.GUID == Guid.Empty)
			{
				this.GUID = GUIDManager.GenerateUniqueGUID();
			}
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
			if (this.UseHumanoidCollider)
			{
				this.HumanoidColliderContainer.vehicle = this;
				this.HumanoidColliderContainer.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
				Collider[] componentsInChildren2 = this.HumanoidColliderContainer.GetComponentsInChildren<Collider>(true);
				foreach (Collider collider in componentsInChildren)
				{
					foreach (Collider collider2 in componentsInChildren2)
					{
						if (this.DEBUG)
						{
							Debug.Log("Ignoring collision between " + collider.name + " and " + collider2.name);
						}
						Physics.IgnoreCollision(collider, collider2, true);
					}
				}
			}
			else
			{
				this.HumanoidColliderContainer.gameObject.SetActive(false);
			}
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Remove(instance2.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onMinutePass = (Action)Delegate.Combine(instance3.onMinutePass, new Action(this.OnMinPass));
			if (!NetworkSingleton<VehicleManager>.Instance.AllVehicles.Contains(this))
			{
				NetworkSingleton<VehicleManager>.Instance.AllVehicles.Add(this);
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x000E8CBA File Offset: 0x000E6EBA
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (this.localPlayerIsInVehicle)
			{
				action.Used = true;
				this.ExitVehicle();
			}
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x000E8CE4 File Offset: 0x000E6EE4
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			if (this.HumanoidColliderContainer != null)
			{
				UnityEngine.Object.Destroy(this.HumanoidColliderContainer.gameObject);
			}
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Remove(instance2.onMinutePass, new Action(this.OnMinPass));
			}
			if (NetworkSingleton<VehicleManager>.InstanceExists)
			{
				NetworkSingleton<VehicleManager>.Instance.AllVehicles.Remove(this);
			}
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x000E8D82 File Offset: 0x000E6F82
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			if (this.IsPlayerOwned)
			{
				container.ChangeValue(this.GetVehicleValue());
			}
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x000E8D98 File Offset: 0x000E6F98
		protected virtual void Update()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			bool flag = this.localPlayerIsDriver || base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost);
			this.rb.interpolation = (flag ? 1 : 0);
			this.HasChanged = true;
			if (this.localPlayerIsInVehicle && GameInput.GetButtonDown(GameInput.ButtonCode.Interact) && !GameInput.IsTyping)
			{
				this.ExitVehicle();
			}
			if (this.IsPlayerOwned)
			{
				if (!this.localPlayerIsDriver && (NetworkSingleton<TimeManager>.Instance.SleepInProgress || Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.Camera.transform.position) > 30f))
				{
					this.rb.isKinematic = true;
				}
				else if (base.NetworkObject.Owner == null || base.NetworkObject.OwnerId == -1 || base.NetworkObject.Owner == base.LocalConnection)
				{
					this.rb.isKinematic = false;
				}
			}
			if (this.overrideControls)
			{
				this.currentThrottle = this.throttleOverride;
				this.sync___set_value_currentSteerAngle(this.steerOverride * this.ActualMaxSteeringAngle, true);
			}
			else
			{
				this.UpdateThrottle();
				this.UpdateSteerAngle();
			}
			this.ApplySteerAngle();
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000E8EE4 File Offset: 0x000E70E4
		protected virtual void FixedUpdate()
		{
			float item = base.transform.InverseTransformDirection(base.transform.position - this.lastFramePosition).z / Time.fixedDeltaTime * 3.6f;
			this.previousSpeeds.Add(item);
			if (this.previousSpeeds.Count > this.previousSpeedsSampleSize)
			{
				this.previousSpeeds.RemoveAt(0);
			}
			if (this.isStatic || !this.localPlayerIsDriver)
			{
				float num = 0f;
				foreach (float num2 in this.previousSpeeds)
				{
					num += num2;
				}
				float speed_Kmh = num / (float)this.previousSpeeds.Count;
				this.speed_Kmh = speed_Kmh;
			}
			else
			{
				this.speed_Kmh = base.transform.InverseTransformDirection(this.rb.velocity).z * 3.6f;
			}
			this.lastFramePosition = base.transform.position;
			if (!this.isStatic && !this.Rb.isKinematic)
			{
				this.ApplyThrottle();
				this.rb.AddForce(-base.transform.up * this.speed_Kmh * this.downforce);
			}
			else
			{
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
				this.sync___set_value_currentSteerAngle(0f, true);
			}
			if (!this.isStatic)
			{
				if ((base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost)) && base.transform.position.y < -20f)
				{
					if (this.rb != null)
					{
						this.rb.velocity = Vector3.zero;
						this.rb.angularVelocity = Vector3.zero;
					}
					float num3 = 0f;
					if (MapHeightSampler.Sample(base.transform.position.x, out num3, base.transform.position.z))
					{
						this.SetTransform(new Vector3(base.transform.position.x, num3 + 3f, base.transform.position.z), Quaternion.identity);
					}
					else
					{
						this.SetTransform(MapHeightSampler.ResetPosition, Quaternion.identity);
					}
				}
				if (this.localPlayerIsDriver && Mathf.Abs(this.speed_Kmh) < 5f)
				{
					int num4 = 0;
					for (int i = 0; i < this.wheels.Count; i++)
					{
						if (!this.wheels[i].IsWheelGrounded())
						{
							num4++;
						}
					}
					if (num4 >= 2)
					{
						this.rb.AddRelativeTorque(Vector3.forward * 8f * -Mathf.Clamp(this.SyncAccessor_currentSteerAngle / this.ActualMaxSteeringAngle, -1f, 1f), 5);
					}
				}
			}
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x000E91E4 File Offset: 0x000E73E4
		protected virtual void OnMinPass()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists || this == null || base.transform == null)
			{
				this.DistanceToLocalCamera = 100000f;
				return;
			}
			this.DistanceToLocalCamera = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x000E9240 File Offset: 0x000E7440
		protected virtual void LateUpdate()
		{
			if (this.HumanoidColliderContainer != null)
			{
				this.HumanoidColliderContainer.transform.position = base.transform.position;
				this.HumanoidColliderContainer.transform.rotation = base.transform.rotation;
			}
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x000E9291 File Offset: 0x000E7491
		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(collision);
			}
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000E92A7 File Offset: 0x000E74A7
		[ServerRpc(RequireOwnership = false)]
		protected virtual void SetOwner(NetworkConnection conn)
		{
			this.RpcWriter___Server_SetOwner_328543758(conn);
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x000E92B4 File Offset: 0x000E74B4
		[ObserversRpc]
		protected virtual void OnOwnerChanged()
		{
			this.RpcWriter___Observers_OnOwnerChanged_2166136261();
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x000E92C7 File Offset: 0x000E74C7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetTransform_Server(Vector3 pos, Quaternion rot)
		{
			this.RpcWriter___Server_SetTransform_Server_3848837105(pos, rot);
			this.RpcLogic___SetTransform_Server_3848837105(pos, rot);
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x000E92E5 File Offset: 0x000E74E5
		[ObserversRpc(RunLocally = true)]
		public void SetTransform(Vector3 pos, Quaternion rot)
		{
			this.RpcWriter___Observers_SetTransform_3848837105(pos, rot);
			this.RpcLogic___SetTransform_3848837105(pos, rot);
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x000E9304 File Offset: 0x000E7504
		public void DestroyVehicle()
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("DestroyVehicle called on client!", null);
				return;
			}
			if (this.isOccupied)
			{
				Console.LogError("Can't destroy vehicle while occupied.", base.gameObject);
				return;
			}
			if (this.isParked)
			{
				this.ExitPark_Networked(null, false);
			}
			if (this.HumanoidColliderContainer != null)
			{
				UnityEngine.Object.Destroy(this.HumanoidColliderContainer.gameObject);
			}
			base.Despawn(null);
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x000E937C File Offset: 0x000E757C
		protected virtual void UpdateThrottle()
		{
			this.currentThrottle = 0f;
			if (this.localPlayerIsDriver)
			{
				this.currentThrottle = GameInput.MotionAxis.y;
				if (this.DriverPlayer.IsTased)
				{
					this.currentThrottle = 0f;
				}
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x000E93C4 File Offset: 0x000E75C4
		protected virtual void ApplyThrottle()
		{
			bool handbrakeApplied = this.handbrakeApplied;
			this.handbrakeApplied = false;
			if (this.localPlayerIsDriver || this.overrideControls)
			{
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
				if (this.isReversing)
				{
					this.isReversing = false;
				}
				foreach (Wheel wheel in this.wheels)
				{
					wheel.wheelCollider.motorTorque = 0.0001f;
					wheel.wheelCollider.brakeTorque = 0f;
				}
				if (this.localPlayerIsDriver)
				{
					this.handbrakeApplied = GameInput.GetButton(GameInput.ButtonCode.Handbrake);
				}
				if (this.handbrakeApplied && Mathf.Abs(this.speed_Kmh) > 4f)
				{
					this.brakesApplied = true;
					if (!handbrakeApplied && this.onHandbrakeApplied != null)
					{
						this.onHandbrakeApplied.Invoke();
					}
				}
				if (this.currentThrottle != 0f && (Mathf.Abs(this.speed_Kmh) < 4f || Mathf.Sign(this.speed_Kmh) == Mathf.Sign(this.currentThrottle)))
				{
					if (this.speed_Kmh < -0.1f && this.currentThrottle < 0f && !this.isReversing)
					{
						this.isReversing = true;
					}
					float num = this.motorTorque.Evaluate(Mathf.Abs(this.speed_Kmh));
					if (this.isReversing)
					{
						num = this.motorTorque.Evaluate(Mathf.Abs(this.speed_Kmh) / this.reverseMultiplier);
					}
					WheelCollider[] array = this.driveWheels;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].motorTorque = this.currentThrottle * num * this.diffGearing / 2f;
					}
					goto IL_2B7;
				}
				if (this.currentThrottle == 0f)
				{
					goto IL_2B7;
				}
				if (Mathf.Abs(this.currentThrottle) > 0.05f && !this.brakesApplied)
				{
					this.brakesApplied = true;
				}
				using (List<Wheel>.Enumerator enumerator = this.wheels.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Wheel wheel2 = enumerator.Current;
						wheel2.wheelCollider.brakeTorque = Mathf.Abs(this.currentThrottle) * this.brakeForce.Evaluate(Mathf.Abs(this.speed_Kmh));
					}
					goto IL_2B7;
				}
			}
			foreach (Wheel wheel3 in this.wheels)
			{
				wheel3.wheelCollider.motorTorque = 0f;
			}
			if (!this.isOccupied)
			{
				if (!this.handbrakeApplied)
				{
					this.handbrakeApplied = true;
				}
				if (this.isReversing)
				{
					this.isReversing = false;
				}
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
			}
			IL_2B7:
			if (this.handbrakeApplied)
			{
				foreach (WheelCollider wheelCollider in this.handbrakeWheels)
				{
					wheelCollider.motorTorque = 0f;
					wheelCollider.brakeTorque = this.handBrakeForce;
				}
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000E96EC File Offset: 0x000E78EC
		public void ApplyHandbrake()
		{
			this.handbrakeApplied = true;
			foreach (WheelCollider wheelCollider in this.handbrakeWheels)
			{
				wheelCollider.motorTorque = 0f;
				wheelCollider.brakeTorque = this.handBrakeForce;
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x000E972E File Offset: 0x000E792E
		[ServerRpc(RequireOwnership = false)]
		private void SetSteeringAngle(float sa)
		{
			this.RpcWriter___Server_SetSteeringAngle_431000436(sa);
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x000E973C File Offset: 0x000E793C
		protected virtual void UpdateSteerAngle()
		{
			if (this.localPlayerIsDriver)
			{
				this.sync___set_value_currentSteerAngle(this.lastFrameSteerAngle, true);
				if (this.DriverPlayer.IsTased || Player.Local.Seizure)
				{
					this.sync___set_value_currentSteerAngle(Mathf.MoveTowards(this.SyncAccessor_currentSteerAngle, UnityEngine.Random.Range(-this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), this.steerRate * Time.deltaTime), true);
				}
				else
				{
					float num = 1f;
					if (Player.Local.Disoriented)
					{
						num = -1f;
					}
					if (GameInput.GetButton(GameInput.ButtonCode.Left))
					{
						this.sync___set_value_currentSteerAngle(Mathf.Clamp(this.SyncAccessor_currentSteerAngle - this.steerRate * Time.deltaTime * num, -this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), true);
					}
					if (GameInput.GetButton(GameInput.ButtonCode.Right))
					{
						this.sync___set_value_currentSteerAngle(Mathf.Clamp(this.SyncAccessor_currentSteerAngle + this.steerRate * Time.deltaTime * num, -this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), true);
					}
					if (!GameInput.GetButton(GameInput.ButtonCode.Left) && !GameInput.GetButton(GameInput.ButtonCode.Right))
					{
						this.sync___set_value_currentSteerAngle(Mathf.MoveTowards(this.SyncAccessor_currentSteerAngle, 0f, this.steerRate * Time.deltaTime), true);
					}
				}
				if (Mathf.Abs(this.lastReplicatedSteerAngle - this.SyncAccessor_currentSteerAngle) > 3f)
				{
					this.lastReplicatedSteerAngle = this.SyncAccessor_currentSteerAngle;
					this.SetSteeringAngle(this.SyncAccessor_currentSteerAngle);
				}
				this.lastFrameSteerAngle = this.SyncAccessor_currentSteerAngle;
			}
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x000E98A4 File Offset: 0x000E7AA4
		protected virtual void ApplySteerAngle()
		{
			float num = this.SyncAccessor_currentSteerAngle;
			if (this.flipSteer)
			{
				num *= -1f;
			}
			WheelCollider[] array = this.steerWheels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].steerAngle = num;
			}
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x000E98E8 File Offset: 0x000E7AE8
		private void DelaySetStatic(bool stat)
		{
			LandVehicle.<>c__DisplayClass224_0 CS$<>8__locals1 = new LandVehicle.<>c__DisplayClass224_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stat = stat;
			base.StartCoroutine(CS$<>8__locals1.<DelaySetStatic>g__Wait|0());
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x000E9918 File Offset: 0x000E7B18
		public virtual void SetIsStatic(bool stat)
		{
			this.isStatic = stat;
			if (this.isStatic)
			{
				this.rb.isKinematic = true;
			}
			else
			{
				this.rb.isKinematic = false;
			}
			foreach (Wheel wheel in this.wheels)
			{
				wheel.SetIsStatic(this.isStatic);
			}
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x000E9998 File Offset: 0x000E7B98
		public void AlignTo(Transform target, EParkingAlignment type, bool network = false)
		{
			Tuple<Vector3, Quaternion> alignmentTransform = this.GetAlignmentTransform(target, type);
			base.transform.rotation = alignmentTransform.Item2;
			base.transform.position = alignmentTransform.Item1;
			this.rb.position = alignmentTransform.Item1;
			this.rb.rotation = alignmentTransform.Item2;
			if (network)
			{
				this.SetTransform_Server(alignmentTransform.Item1, alignmentTransform.Item2);
			}
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x000E9A08 File Offset: 0x000E7C08
		public Tuple<Vector3, Quaternion> GetAlignmentTransform(Transform target, EParkingAlignment type)
		{
			Quaternion quaternion = target.rotation;
			if (type == EParkingAlignment.FrontToKerb)
			{
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
			}
			Vector3 vector = target.position + target.up * (this.boundingBoxDimensions.y / 2f - this.boundingBox.transform.localPosition.y);
			if (type == EParkingAlignment.FrontToKerb)
			{
				vector += target.forward * (this.boundingBoxDimensions.z / 2f - this.boundingBox.transform.localPosition.y);
			}
			else
			{
				vector += target.forward * (this.boundingBoxDimensions.z / 2f - this.boundingBox.transform.localPosition.y);
			}
			return new Tuple<Vector3, Quaternion>(vector, quaternion);
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x000E9AF6 File Offset: 0x000E7CF6
		public float GetVehicleValue()
		{
			return this.VehiclePrice;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000E9AFE File Offset: 0x000E7CFE
		public void OverrideMaxSteerAngle(float maxAngle)
		{
			this.OverriddenMaxSteerAngle = maxAngle;
			this.MaxSteerAngleOverridden = true;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x000E9B0E File Offset: 0x000E7D0E
		public void ResetMaxSteerAngle()
		{
			this.MaxSteerAngleOverridden = false;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x000E9B17 File Offset: 0x000E7D17
		public void SetObstaclesActive(bool active)
		{
			this.NavmeshCut.enabled = active;
			this.NavMeshObstacle.carving = active;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x000E9B34 File Offset: 0x000E7D34
		public VehicleSeat GetFirstFreeSeat()
		{
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (!this.Seats[i].isOccupied)
				{
					return this.Seats[i];
				}
			}
			return null;
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x000E9B70 File Offset: 0x000E7D70
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetSeatOccupant(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSeatOccupant_3428404692(conn, seatIndex, occupant);
				this.RpcLogic___SetSeatOccupant_3428404692(conn, seatIndex, occupant);
			}
			else
			{
				this.RpcWriter___Target_SetSeatOccupant_3428404692(conn, seatIndex, occupant);
			}
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x000E9BBD File Offset: 0x000E7DBD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SetSeatOccupant_Server(int seatIndex, NetworkConnection conn)
		{
			this.RpcWriter___Server_SetSeatOccupant_Server_3266232555(seatIndex, conn);
			this.RpcLogic___SetSeatOccupant_Server_3266232555(seatIndex, conn);
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000E9BDC File Offset: 0x000E7DDC
		private void Hovered()
		{
			if (!this.IsPlayerOwned)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.CurrentPlayerOccupancy < this.Capacity)
			{
				this.intObj.SetMessage("Enter vehicle");
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetMessage("Vehicle full");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000E9C45 File Offset: 0x000E7E45
		private void Interacted()
		{
			if (this.justExitedVehicle)
			{
				return;
			}
			if (!this.IsPlayerOwned)
			{
				return;
			}
			if (this.CurrentPlayerOccupancy < this.Capacity)
			{
				this.EnterVehicle();
			}
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000E9C70 File Offset: 0x000E7E70
		private void EnterVehicle()
		{
			if (this.justExitedVehicle)
			{
				return;
			}
			this.localPlayerIsInVehicle = true;
			this.localPlayerSeat = this.GetFirstFreeSeat();
			this.localPlayerIsDriver = this.localPlayerSeat.isDriverSeat;
			this.SetSeatOccupant_Server(Array.IndexOf<VehicleSeat>(this.Seats, this.localPlayerSeat), Player.Local.Connection);
			this.closestExitPoint = this.GetClosestExitPoint(this.localPlayerSeat.transform.position);
			Player.Local.EnterVehicle(this);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Vehicle);
			if (PlayerSingleton<PlayerInventory>.InstanceExists)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			}
			if (this.localPlayerIsDriver)
			{
				base.NetworkObject.SetLocalOwnership(Player.Local.Connection);
				this.SetOwner(Player.Local.Connection);
			}
			this.SetObstaclesActive(!this.isOccupied);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x000E9D58 File Offset: 0x000E7F58
		public void ExitVehicle()
		{
			if (this.localPlayerIsDriver)
			{
				this.SetOwner(null);
			}
			this.localPlayerIsInVehicle = false;
			this.localPlayerIsDriver = false;
			if (this.localPlayerSeat != null)
			{
				this.SetSeatOccupant_Server(Array.IndexOf<VehicleSeat>(this.Seats, this.localPlayerSeat), null);
				this.localPlayerSeat = null;
			}
			List<Transform> list = new List<Transform>();
			list.Add(this.closestExitPoint);
			list.AddRange(this.exitPoints);
			Transform validExitPoint = this.GetValidExitPoint(list);
			Player.Local.ExitVehicle(validExitPoint);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Default);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			this.SetObstaclesActive(!this.isOccupied);
			this.justExitedVehicle = true;
			base.Invoke("EndJustExited", 0.05f);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000E9E42 File Offset: 0x000E8042
		private void EndJustExited()
		{
			this.justExitedVehicle = false;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000E9E4B File Offset: 0x000E804B
		public Transform GetExitPoint(int seatIndex = 0)
		{
			return this.exitPoints[seatIndex];
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x000E9E5C File Offset: 0x000E805C
		private Transform GetClosestExitPoint(Vector3 pos)
		{
			Transform transform = null;
			for (int i = 0; i < this.exitPoints.Count; i++)
			{
				if (transform == null || Vector3.Distance(this.exitPoints[i].position, pos) < Vector3.Distance(transform.transform.position, pos))
				{
					transform = this.exitPoints[i];
				}
			}
			return transform;
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x000E9EC4 File Offset: 0x000E80C4
		private Transform GetValidExitPoint(List<Transform> possibleExitPoints)
		{
			LayerMask mask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			mask |= 1 << LayerMask.NameToLayer("Vehicle");
			mask |= 1 << LayerMask.NameToLayer("Terrain");
			for (int i = 0; i < possibleExitPoints.Count; i++)
			{
				if (Physics.OverlapSphere(possibleExitPoints[i].position, 0.35f, mask).Length == 0)
				{
					return possibleExitPoints[i];
				}
			}
			Console.LogWarning("Unable to find clear exit point for vehicle. Using first exit point.", null);
			return possibleExitPoints[0];
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000E9F78 File Offset: 0x000E8178
		public void AddNPCOccupant(NPC npc)
		{
			int num = (from x in this.OccupantNPCs
			where x != null
			select x).Count<NPC>();
			if (!this.OccupantNPCs.Contains(npc))
			{
				for (int i = 0; i < this.OccupantNPCs.Length; i++)
				{
					if (this.OccupantNPCs[i] == null)
					{
						this.OccupantNPCs[i] = npc;
						break;
					}
				}
			}
			this.isOccupied = true;
			this.SetObstaclesActive(!this.isOccupied);
			if (num == 0 && this.onVehicleStart != null)
			{
				this.onVehicleStart.Invoke();
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000EA020 File Offset: 0x000E8220
		public void RemoveNPCOccupant(NPC npc)
		{
			for (int i = 0; i < this.OccupantNPCs.Length; i++)
			{
				if (this.OccupantNPCs[i] == npc)
				{
					this.OccupantNPCs[i] = null;
				}
			}
			if ((from x in this.OccupantNPCs
			where x != null
			select x).Count<NPC>() == 0)
			{
				this.isOccupied = false;
				if (this.onVehicleStop != null)
				{
					this.onVehicleStop.Invoke();
				}
			}
			this.SetObstaclesActive(!this.isOccupied);
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000EA0B3 File Offset: 0x000E82B3
		public virtual bool CanBeRecovered()
		{
			return this.IsPlayerOwned && !this.isOccupied && !this.isStatic;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x000EA0D0 File Offset: 0x000E82D0
		public virtual void RecoverVehicle()
		{
			VehicleRecoveryPoint closestRecoveryPoint = VehicleRecoveryPoint.GetClosestRecoveryPoint(base.transform.position);
			base.transform.position = closestRecoveryPoint.transform.position + Vector3.up * 2f;
			base.transform.up = Vector3.up;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000EA128 File Offset: 0x000E8328
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendOwnedColor(EVehicleColor col)
		{
			this.RpcWriter___Server_SendOwnedColor_911055161(col);
			this.RpcLogic___SendOwnedColor_911055161(col);
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000EA13E File Offset: 0x000E833E
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		protected virtual void SetOwnedColor(NetworkConnection conn, EVehicleColor col)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetOwnedColor_1679996372(conn, col);
				this.RpcLogic___SetOwnedColor_1679996372(conn, col);
			}
			else
			{
				this.RpcWriter___Target_SetOwnedColor_1679996372(conn, col);
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000EA174 File Offset: 0x000E8374
		public virtual void ApplyColor(EVehicleColor col)
		{
			if (col == EVehicleColor.Custom)
			{
				this.DisplayedColor = col;
				return;
			}
			this.DisplayedColor = col;
			Material material = Singleton<VehicleColors>.Instance.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == this.DisplayedColor).material;
			for (int i = 0; i < this.BodyMeshes.Length; i++)
			{
				this.BodyMeshes[i].Renderer.materials[this.BodyMeshes[i].MaterialIndex].color = material.color;
			}
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000EA1F4 File Offset: 0x000E83F4
		public void ApplyOwnedColor()
		{
			this.ApplyColor(this.OwnedColor);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000EA204 File Offset: 0x000E8404
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			if (this.outlineEffect == null)
			{
				this.outlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.outlineEffect.OutlineParameters.BlurShift = 0f;
				this.outlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.outlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.outlineRenderers)
				{
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						OutlineTarget outlineTarget = new OutlineTarget(componentsInChildren[i], 0);
						this.outlineEffect.TryAddTarget(outlineTarget);
					}
				}
			}
			this.outlineEffect.OutlineParameters.Color = BuildableItem.GetColorFromOutlineColorEnum(color);
			Color32 colorFromOutlineColorEnum = BuildableItem.GetColorFromOutlineColorEnum(color);
			colorFromOutlineColorEnum.a = 9;
			this.outlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", colorFromOutlineColorEnum);
			this.outlineEffect.enabled = true;
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000EA340 File Offset: 0x000E8540
		public void HideOutline()
		{
			if (this.outlineEffect != null)
			{
				this.outlineEffect.enabled = false;
			}
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000EA35C File Offset: 0x000E855C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void Park_Networked(NetworkConnection conn, ParkData parkData)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Park_Networked_2633993806(conn, parkData);
				this.RpcLogic___Park_Networked_2633993806(conn, parkData);
			}
			else
			{
				this.RpcWriter___Target_Park_Networked_2633993806(conn, parkData);
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000EA394 File Offset: 0x000E8594
		public void Park(NetworkConnection conn, ParkData parkData, bool network)
		{
			if (this.isParked)
			{
				this.ExitPark(true);
			}
			if (network)
			{
				this.Park_Networked(conn, parkData);
				return;
			}
			this.CurrentParkingLot = GUIDManager.GetObject<ParkingLot>(parkData.lotGUID);
			if (this.CurrentParkingLot == null)
			{
				Console.LogWarning("LandVehicle.Park: parking lot not found with the given GUID.", null);
				return;
			}
			this.CurrentParkData = parkData;
			if (parkData.spotIndex < 0 || parkData.spotIndex >= this.CurrentParkingLot.ParkingSpots.Count)
			{
				this.SetVisible(false);
			}
			else
			{
				this.CurrentParkingSpot = this.CurrentParkingLot.ParkingSpots[parkData.spotIndex];
				this.CurrentParkingSpot.SetOccupant(this);
				this.AlignTo(this.CurrentParkingSpot.AlignmentPoint, parkData.alignment, false);
			}
			this.SetIsStatic(true);
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000EA45F File Offset: 0x000E865F
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ExitPark_Networked(NetworkConnection conn, bool moveToExitPoint = true)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ExitPark_Networked_214505783(conn, moveToExitPoint);
				this.RpcLogic___ExitPark_Networked_214505783(conn, moveToExitPoint);
			}
			else
			{
				this.RpcWriter___Target_ExitPark_Networked_214505783(conn, moveToExitPoint);
			}
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000EA498 File Offset: 0x000E8698
		public void ExitPark(bool moveToExitPoint = true)
		{
			if (this.CurrentParkingLot == null)
			{
				return;
			}
			if (this.CurrentParkingLot.ExitPoint != null && moveToExitPoint)
			{
				this.AlignTo(this.CurrentParkingLot.ExitPoint, this.CurrentParkingLot.ExitAlignment, false);
			}
			this.CurrentParkData = null;
			this.CurrentParkingLot = null;
			if (this.CurrentParkingSpot != null)
			{
				this.CurrentParkingSpot.SetOccupant(null);
				this.CurrentParkingSpot = null;
			}
			this.SetIsStatic(false);
			this.SetVisible(true);
			base.gameObject.SetActive(true);
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000EA52F File Offset: 0x000E872F
		public void SetVisible(bool vis)
		{
			this.IsVisible = vis;
			this.vehicleModel.gameObject.SetActive(vis);
			this.HumanoidColliderContainer.gameObject.SetActive(vis);
			this.boundingBox.gameObject.SetActive(vis);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000EA56C File Offset: 0x000E876C
		public List<ItemInstance> GetContents()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			if (this.Storage != null)
			{
				list.AddRange(this.Storage.GetAllItems());
			}
			return list;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000EA59F File Offset: 0x000E879F
		public virtual VehicleData GetVehicleData()
		{
			return new VehicleData(this.GUID, this.vehicleCode, base.transform.position, base.transform.rotation, this.OwnedColor, this.GetContentsSet());
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000EA5D4 File Offset: 0x000E87D4
		public string GetSaveString()
		{
			return this.GetVehicleData().GetJson(true);
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000EA5E2 File Offset: 0x000E87E2
		private ItemSet GetContentsSet()
		{
			if (this.Storage == null || this.Storage.ItemCount == 0)
			{
				return null;
			}
			return new ItemSet(this.Storage.ItemSlots);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000EA614 File Offset: 0x000E8814
		public virtual void Load(VehicleData data, string containerPath)
		{
			this.SetGUID(new Guid(data.GUID));
			this.SetTransform(data.Position, data.Rotation);
			this.SetOwnedColor(null, Enum.Parse<EVehicleColor>(data.Color));
			if (this.Storage != null)
			{
				if (data.VehicleContents != null && data.VehicleContents.Items != null)
				{
					DeserializedItemSet deserializedItemSet;
					if (ItemSet.TryDeserialize(data.VehicleContents, out deserializedItemSet))
					{
						deserializedItemSet.LoadTo(this.Storage.ItemSlots);
						return;
					}
				}
				else if (File.Exists(Path.Combine(containerPath, "Contents.json")))
				{
					Console.LogWarning("Loading legacy vehicle contents.", null);
					string json;
					DeserializedItemSet deserializedItemSet2;
					if (this.Loader.TryLoadFile(containerPath, "Contents", out json) && ItemSet.TryDeserialize(json, out deserializedItemSet2))
					{
						deserializedItemSet2.LoadTo(this.Storage.ItemSlots);
					}
				}
			}
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x000EA8C4 File Offset: 0x000E8AC4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<isReversing>k__BackingField = new SyncVar<bool>(this, 2U, 1, 0, 0.1f, 1, this.<isReversing>k__BackingField);
			this.syncVar___<brakesApplied>k__BackingField = new SyncVar<bool>(this, 1U, 1, 0, 0.1f, 1, this.<brakesApplied>k__BackingField);
			this.syncVar___currentSteerAngle = new SyncVar<float>(this, 0U, 1, 0, 0.05f, 1, this.currentSteerAngle);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsPlayerOwned_214505783));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_SetIsPlayerOwned_214505783));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetOwner_328543758));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_OnOwnerChanged_2166136261));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SetTransform_Server_3848837105));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetTransform_3848837105));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SetSteeringAngle_431000436));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetSeatOccupant_3428404692));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_SetSeatOccupant_3428404692));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetSeatOccupant_Server_3266232555));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendOwnedColor_911055161));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetOwnedColor_1679996372));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetOwnedColor_1679996372));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_Park_Networked_2633993806));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_Park_Networked_2633993806));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ExitPark_Networked_214505783));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_ExitPark_Networked_214505783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.LandVehicle));
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000EAAFC File Offset: 0x000E8CFC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<isReversing>k__BackingField.SetRegistered();
			this.syncVar___<brakesApplied>k__BackingField.SetRegistered();
			this.syncVar___currentSteerAngle.SetRegistered();
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x000EAB30 File Offset: 0x000E8D30
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x000EAB40 File Offset: 0x000E8D40
		private void RpcWriter___Observers_SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
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
			writer.WriteBoolean(playerOwned);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x000EABF8 File Offset: 0x000E8DF8
		public void RpcLogic___SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
		{
			this.IsPlayerOwned = playerOwned;
			StorageEntity storageEntity;
			if (base.TryGetComponent<StorageEntity>(out storageEntity))
			{
				storageEntity.AccessSettings = (playerOwned ? StorageEntity.EAccessSettings.Full : StorageEntity.EAccessSettings.Closed);
				foreach (ItemSlot itemSlot in storageEntity.ItemSlots)
				{
					itemSlot.SetFilterable(true);
				}
			}
			this.RefreshPoI();
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000EAC70 File Offset: 0x000E8E70
		private void RpcReader___Observers_SetIsPlayerOwned_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsPlayerOwned_214505783(null, playerOwned);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000EACAC File Offset: 0x000E8EAC
		private void RpcWriter___Target_SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
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
			writer.WriteBoolean(playerOwned);
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000EAD64 File Offset: 0x000E8F64
		private void RpcReader___Target_SetIsPlayerOwned_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsPlayerOwned_214505783(base.LocalConnection, playerOwned);
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000EAD9C File Offset: 0x000E8F9C
		private void RpcWriter___Server_SetOwner_328543758(NetworkConnection conn)
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
			writer.WriteNetworkConnection(conn);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000EAE43 File Offset: 0x000E9043
		protected virtual void RpcLogic___SetOwner_328543758(NetworkConnection conn)
		{
			base.NetworkObject.GiveOwnership(conn);
			this.OnOwnerChanged();
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x000EAE58 File Offset: 0x000E9058
		private void RpcReader___Server_SetOwner_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwner_328543758(conn2);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000EAE8C File Offset: 0x000E908C
		private void RpcWriter___Observers_OnOwnerChanged_2166136261()
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

		// Token: 0x060037C1 RID: 14273 RVA: 0x000EAF38 File Offset: 0x000E9138
		protected virtual void RpcLogic___OnOwnerChanged_2166136261()
		{
			if (base.NetworkObject.Owner == base.LocalConnection || (base.NetworkObject.OwnerId == -1 && InstanceFinder.IsHost))
			{
				Console.Log("Local client owns vehicle", null);
				this.rb.isKinematic = false;
				this.rb.interpolation = 1;
				base.GetComponent<NetworkTransform>().ClearReplicateCache();
				base.GetComponent<NetworkTransform>().ForceSend();
				return;
			}
			Console.Log("Local client no longer owns vehicle", null);
			if (!InstanceFinder.IsHost || (InstanceFinder.IsHost && !this.localPlayerIsDriver && this.CurrentPlayerOccupancy > 0))
			{
				this.rb.interpolation = 0;
				Debug.Log("No interpolation");
				this.rb.isKinematic = false;
				this.rb.isKinematic = true;
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000EB004 File Offset: 0x000E9204
		private void RpcReader___Observers_OnOwnerChanged_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___OnOwnerChanged_2166136261();
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000EB024 File Offset: 0x000E9224
		private void RpcWriter___Server_SetTransform_Server_3848837105(Vector3 pos, Quaternion rot)
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
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot, 1);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000EB0DD File Offset: 0x000E92DD
		public void RpcLogic___SetTransform_Server_3848837105(Vector3 pos, Quaternion rot)
		{
			this.SetTransform(pos, rot);
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000EB0E8 File Offset: 0x000E92E8
		private void RpcReader___Server_SetTransform_Server_3848837105(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 pos = PooledReader0.ReadVector3();
			Quaternion rot = PooledReader0.ReadQuaternion(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetTransform_Server_3848837105(pos, rot);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000EB13C File Offset: 0x000E933C
		private void RpcWriter___Observers_SetTransform_3848837105(Vector3 pos, Quaternion rot)
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
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot, 1);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000EB204 File Offset: 0x000E9404
		public void RpcLogic___SetTransform_3848837105(Vector3 pos, Quaternion rot)
		{
			base.transform.position = pos;
			base.transform.rotation = rot;
			this.rb.position = pos;
			this.rb.rotation = rot;
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000EB238 File Offset: 0x000E9438
		private void RpcReader___Observers_SetTransform_3848837105(PooledReader PooledReader0, Channel channel)
		{
			Vector3 pos = PooledReader0.ReadVector3();
			Quaternion rot = PooledReader0.ReadQuaternion(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetTransform_3848837105(pos, rot);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000EB28C File Offset: 0x000E948C
		private void RpcWriter___Server_SetSteeringAngle_431000436(float sa)
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
			writer.WriteSingle(sa, 0);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000EB338 File Offset: 0x000E9538
		private void RpcLogic___SetSteeringAngle_431000436(float sa)
		{
			this.sync___set_value_currentSteerAngle(sa, true);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000EB344 File Offset: 0x000E9544
		private void RpcReader___Server_SetSteeringAngle_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float sa = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetSteeringAngle_431000436(sa);
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000EB37C File Offset: 0x000E957C
		private void RpcWriter___Observers_SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
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
			writer.WriteInt32(seatIndex, 1);
			writer.WriteNetworkConnection(occupant);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000EB444 File Offset: 0x000E9644
		private void RpcLogic___SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
		{
			Player occupant2 = this.Seats[seatIndex].Occupant;
			this.Seats[seatIndex].Occupant = Player.GetPlayer(occupant);
			occupant != null;
			if (seatIndex == 0)
			{
				if (occupant != null)
				{
					if (this.onVehicleStart != null)
					{
						this.onVehicleStart.Invoke();
					}
				}
				else if (this.onVehicleStop != null)
				{
					this.onVehicleStop.Invoke();
				}
			}
			if (occupant != null)
			{
				if (this.onPlayerEnterVehicle != null)
				{
					this.onPlayerEnterVehicle(this.Seats[seatIndex].Occupant);
				}
			}
			else if (this.onPlayerExitVehicle != null)
			{
				this.onPlayerExitVehicle(occupant2);
			}
			this.isOccupied = (this.Seats.Count((VehicleSeat s) => s.isOccupied) > 0);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000EB520 File Offset: 0x000E9720
		private void RpcReader___Observers_SetSeatOccupant_3428404692(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(1);
			NetworkConnection occupant = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_3428404692(null, seatIndex, occupant);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000EB574 File Offset: 0x000E9774
		private void RpcWriter___Target_SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
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
			writer.WriteInt32(seatIndex, 1);
			writer.WriteNetworkConnection(occupant);
			base.SendTargetRpc(8U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000EB63C File Offset: 0x000E983C
		private void RpcReader___Target_SetSeatOccupant_3428404692(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(1);
			NetworkConnection occupant = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_3428404692(base.LocalConnection, seatIndex, occupant);
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000EB68C File Offset: 0x000E988C
		private void RpcWriter___Server_SetSeatOccupant_Server_3266232555(int seatIndex, NetworkConnection conn)
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
			writer.WriteInt32(seatIndex, 1);
			writer.WriteNetworkConnection(conn);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000EB745 File Offset: 0x000E9945
		private void RpcLogic___SetSeatOccupant_Server_3266232555(int seatIndex, NetworkConnection conn)
		{
			this.SetSeatOccupant(null, seatIndex, conn);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000EB750 File Offset: 0x000E9950
		private void RpcReader___Server_SetSeatOccupant_Server_3266232555(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int seatIndex = PooledReader0.ReadInt32(1);
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_Server_3266232555(seatIndex, conn2);
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000EB7A4 File Offset: 0x000E99A4
		private void RpcWriter___Server_SendOwnedColor_911055161(EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000EB84B File Offset: 0x000E9A4B
		public void RpcLogic___SendOwnedColor_911055161(EVehicleColor col)
		{
			this.SetOwnedColor(null, col);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000EB858 File Offset: 0x000E9A58
		private void RpcReader___Server_SendOwnedColor_911055161(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			EVehicleColor col = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendOwnedColor_911055161(col);
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000EB898 File Offset: 0x000E9A98
		private void RpcWriter___Target_SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendTargetRpc(11U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000EB94D File Offset: 0x000E9B4D
		protected virtual void RpcLogic___SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
		{
			this.OwnedColor = col;
			this.ApplyOwnedColor();
			this.RefreshPoI();
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000EB964 File Offset: 0x000E9B64
		private void RpcReader___Target_SetOwnedColor_1679996372(PooledReader PooledReader0, Channel channel)
		{
			EVehicleColor col = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwnedColor_1679996372(base.LocalConnection, col);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000EB99C File Offset: 0x000E9B9C
		private void RpcWriter___Observers_SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000EBA54 File Offset: 0x000E9C54
		private void RpcReader___Observers_SetOwnedColor_1679996372(PooledReader PooledReader0, Channel channel)
		{
			EVehicleColor col = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetOwnedColor_1679996372(null, col);
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000EBA90 File Offset: 0x000E9C90
		private void RpcWriter___Observers_Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
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
			writer.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated(parkData);
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000EBB46 File Offset: 0x000E9D46
		private void RpcLogic___Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
		{
			this.Park(conn, parkData, false);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000EBB54 File Offset: 0x000E9D54
		private void RpcReader___Observers_Park_Networked_2633993806(PooledReader PooledReader0, Channel channel)
		{
			ParkData parkData = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Park_Networked_2633993806(null, parkData);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000EBB90 File Offset: 0x000E9D90
		private void RpcWriter___Target_Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
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
			writer.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated(parkData);
			base.SendTargetRpc(14U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000EBC48 File Offset: 0x000E9E48
		private void RpcReader___Target_Park_Networked_2633993806(PooledReader PooledReader0, Channel channel)
		{
			ParkData parkData = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Park_Networked_2633993806(base.LocalConnection, parkData);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000EBC80 File Offset: 0x000E9E80
		private void RpcWriter___Observers_ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
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
			writer.WriteBoolean(moveToExitPoint);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000EBD36 File Offset: 0x000E9F36
		public void RpcLogic___ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
		{
			this.ExitPark(moveToExitPoint);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000EBD40 File Offset: 0x000E9F40
		private void RpcReader___Observers_ExitPark_Networked_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool moveToExitPoint = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitPark_Networked_214505783(null, moveToExitPoint);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000EBD7C File Offset: 0x000E9F7C
		private void RpcWriter___Target_ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
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
			writer.WriteBoolean(moveToExitPoint);
			base.SendTargetRpc(16U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000EBE34 File Offset: 0x000EA034
		private void RpcReader___Target_ExitPark_Networked_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool moveToExitPoint = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ExitPark_Networked_214505783(base.LocalConnection, moveToExitPoint);
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000EBE6B File Offset: 0x000EA06B
		// (set) Token: 0x060037E7 RID: 14311 RVA: 0x000EBE73 File Offset: 0x000EA073
		public float SyncAccessor_currentSteerAngle
		{
			get
			{
				return this.currentSteerAngle;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.currentSteerAngle = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___currentSteerAngle.SetValue(value, value);
				}
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000EBEB0 File Offset: 0x000EA0B0
		public override bool LandVehicle(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<isReversing>k__BackingField(this.syncVar___<isReversing>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_<isReversing>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<brakesApplied>k__BackingField(this.syncVar___<brakesApplied>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value2 = PooledReader0.ReadBoolean();
				this.sync___set_value_<brakesApplied>k__BackingField(value2, Boolean2);
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
					this.sync___set_value_currentSteerAngle(this.syncVar___currentSteerAngle.GetValue(true), true);
					return true;
				}
				float value3 = PooledReader0.ReadSingle(0);
				this.sync___set_value_currentSteerAngle(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000EBF8F File Offset: 0x000EA18F
		// (set) Token: 0x060037EA RID: 14314 RVA: 0x000EBF97 File Offset: 0x000EA197
		public bool SyncAccessor_<brakesApplied>k__BackingField
		{
			get
			{
				return this.<brakesApplied>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<brakesApplied>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<brakesApplied>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060037EB RID: 14315 RVA: 0x000EBFD3 File Offset: 0x000EA1D3
		// (set) Token: 0x060037EC RID: 14316 RVA: 0x000EBFDB File Offset: 0x000EA1DB
		public bool SyncAccessor_<isReversing>k__BackingField
		{
			get
			{
				return this.<isReversing>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<isReversing>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<isReversing>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000EC018 File Offset: 0x000EA218
		protected virtual void dll()
		{
			this.OccupantNPCs = new NPC[this.Seats.Length];
			this.boundingBox.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			for (int i = 0; i < this.driveWheels.Length; i++)
			{
				this.wheels.Add(this.driveWheels[i].GetComponent<Wheel>());
			}
			for (int j = 0; j < this.steerWheels.Length; j++)
			{
				if (!this.wheels.Contains(this.steerWheels[j].GetComponent<Wheel>()))
				{
					this.wheels.Add(this.steerWheels[j].GetComponent<Wheel>());
				}
			}
			this.OwnedColor = this.DefaultColor;
			this.InitializeSaveable();
			if (base.GetComponent<StorageEntity>() != null)
			{
				base.GetComponent<StorageEntity>().AccessSettings = StorageEntity.EAccessSettings.Closed;
			}
			this.SetObstaclesActive(true);
			this.RefreshPoI();
		}

		// Token: 0x0400275D RID: 10077
		public const float KINEMATIC_THRESHOLD_DISTANCE = 30f;

		// Token: 0x0400275E RID: 10078
		public const float MAX_TURNOVER_SPEED = 5f;

		// Token: 0x0400275F RID: 10079
		public const float TURNOVER_FORCE = 8f;

		// Token: 0x04002760 RID: 10080
		public const bool USE_WHEEL = false;

		// Token: 0x04002761 RID: 10081
		public const float SPEED_DISPLAY_MULTIPLIER = 1.4f;

		// Token: 0x04002762 RID: 10082
		public bool DEBUG;

		// Token: 0x04002763 RID: 10083
		[Header("Settings")]
		[SerializeField]
		protected string vehicleName = "Vehicle";

		// Token: 0x04002764 RID: 10084
		[SerializeField]
		protected string vehicleCode = "vehicle_code";

		// Token: 0x04002765 RID: 10085
		[SerializeField]
		protected float vehiclePrice = 1000f;

		// Token: 0x04002768 RID: 10088
		public bool UseHumanoidCollider = true;

		// Token: 0x0400276A RID: 10090
		public bool SpawnAsPlayerOwned;

		// Token: 0x0400276C RID: 10092
		[Header("References")]
		[SerializeField]
		protected GameObject vehicleModel;

		// Token: 0x0400276D RID: 10093
		[SerializeField]
		protected WheelCollider[] driveWheels;

		// Token: 0x0400276E RID: 10094
		[SerializeField]
		protected WheelCollider[] steerWheels;

		// Token: 0x0400276F RID: 10095
		[SerializeField]
		protected WheelCollider[] handbrakeWheels;

		// Token: 0x04002770 RID: 10096
		[HideInInspector]
		public List<Wheel> wheels = new List<Wheel>();

		// Token: 0x04002771 RID: 10097
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04002772 RID: 10098
		[SerializeField]
		protected List<Transform> exitPoints = new List<Transform>();

		// Token: 0x04002773 RID: 10099
		[SerializeField]
		protected Rigidbody rb;

		// Token: 0x04002774 RID: 10100
		public VehicleSeat[] Seats;

		// Token: 0x04002775 RID: 10101
		public BoxCollider boundingBox;

		// Token: 0x04002776 RID: 10102
		public VehicleAgent Agent;

		// Token: 0x04002777 RID: 10103
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04002778 RID: 10104
		public StorageDoorAnimation Trunk;

		// Token: 0x04002779 RID: 10105
		public NavMeshObstacle NavMeshObstacle;

		// Token: 0x0400277A RID: 10106
		public NavmeshCut NavmeshCut;

		// Token: 0x0400277B RID: 10107
		public VehicleHumanoidCollider HumanoidColliderContainer;

		// Token: 0x0400277C RID: 10108
		public POI POI;

		// Token: 0x0400277D RID: 10109
		[SerializeField]
		protected Transform centerOfMass;

		// Token: 0x0400277E RID: 10110
		[SerializeField]
		protected Transform cameraOrigin;

		// Token: 0x0400277F RID: 10111
		[SerializeField]
		protected VehicleLights lights;

		// Token: 0x04002780 RID: 10112
		[Header("Steer settings")]
		[SerializeField]
		protected float maxSteeringAngle = 25f;

		// Token: 0x04002781 RID: 10113
		[SerializeField]
		protected float steerRate = 50f;

		// Token: 0x04002782 RID: 10114
		[SerializeField]
		protected bool flipSteer;

		// Token: 0x04002785 RID: 10117
		[Header("Drive settings")]
		[SerializeField]
		protected AnimationCurve motorTorque = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 200f),
			new Keyframe(50f, 300f),
			new Keyframe(200f, 0f)
		});

		// Token: 0x04002786 RID: 10118
		public float TopSpeed = 60f;

		// Token: 0x04002787 RID: 10119
		[Range(2f, 16f)]
		[SerializeField]
		protected float diffGearing = 4f;

		// Token: 0x04002788 RID: 10120
		[SerializeField]
		protected float handBrakeForce = 300f;

		// Token: 0x04002789 RID: 10121
		[SerializeField]
		protected AnimationCurve brakeForce = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 200f),
			new Keyframe(50f, 300f),
			new Keyframe(200f, 0f)
		});

		// Token: 0x0400278A RID: 10122
		[Range(0.5f, 10f)]
		[SerializeField]
		protected float downforce = 1f;

		// Token: 0x0400278B RID: 10123
		[Range(0f, 1f)]
		[SerializeField]
		protected float reverseMultiplier = 0.35f;

		// Token: 0x0400278C RID: 10124
		[Header("Color Settings")]
		[SerializeField]
		protected LandVehicle.BodyMesh[] BodyMeshes;

		// Token: 0x0400278D RID: 10125
		public EVehicleColor DefaultColor = EVehicleColor.White;

		// Token: 0x0400278F RID: 10127
		private EVehicleColor DisplayedColor = EVehicleColor.White;

		// Token: 0x04002790 RID: 10128
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> outlineRenderers = new List<GameObject>();

		// Token: 0x04002791 RID: 10129
		protected Outlinable outlineEffect;

		// Token: 0x04002792 RID: 10130
		[Header("Control overrides")]
		public bool overrideControls;

		// Token: 0x04002793 RID: 10131
		public float throttleOverride;

		// Token: 0x04002794 RID: 10132
		public float steerOverride;

		// Token: 0x04002795 RID: 10133
		[Header("Storage settings")]
		public StorageEntity Storage;

		// Token: 0x04002796 RID: 10134
		private VehicleSeat localPlayerSeat;

		// Token: 0x0400279C RID: 10140
		private List<float> previousSpeeds = new List<float>();

		// Token: 0x0400279D RID: 10141
		private int previousSpeedsSampleSize = 20;

		// Token: 0x0400279F RID: 10143
		[SyncVar]
		public float currentSteerAngle;

		// Token: 0x040027A0 RID: 10144
		private float lastFrameSteerAngle;

		// Token: 0x040027A1 RID: 10145
		private float lastReplicatedSteerAngle;

		// Token: 0x040027A2 RID: 10146
		private bool justExitedVehicle;

		// Token: 0x040027A7 RID: 10151
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x040027A8 RID: 10152
		private Transform closestExitPoint;

		// Token: 0x040027A9 RID: 10153
		[HideInInspector]
		public ParkData CurrentParkData;

		// Token: 0x040027AC RID: 10156
		private VehicleLoader loader = new VehicleLoader();

		// Token: 0x040027B0 RID: 10160
		public LandVehicle.VehiclePlayerEvent onPlayerEnterVehicle;

		// Token: 0x040027B1 RID: 10161
		public LandVehicle.VehiclePlayerEvent onPlayerExitVehicle;

		// Token: 0x040027B2 RID: 10162
		public UnityEvent onVehicleStart;

		// Token: 0x040027B3 RID: 10163
		public UnityEvent onVehicleStop;

		// Token: 0x040027B4 RID: 10164
		public UnityEvent onHandbrakeApplied;

		// Token: 0x040027B5 RID: 10165
		public UnityEvent<Collision> onCollision = new UnityEvent<Collision>();

		// Token: 0x040027B6 RID: 10166
		public SyncVar<float> syncVar___currentSteerAngle;

		// Token: 0x040027B7 RID: 10167
		public SyncVar<bool> syncVar___<brakesApplied>k__BackingField;

		// Token: 0x040027B8 RID: 10168
		public SyncVar<bool> syncVar___<isReversing>k__BackingField;

		// Token: 0x040027B9 RID: 10169
		private bool dll_Excuted;

		// Token: 0x040027BA RID: 10170
		private bool dll_Excuted;

		// Token: 0x02000801 RID: 2049
		[Serializable]
		public class BodyMesh
		{
			// Token: 0x040027BB RID: 10171
			public MeshRenderer Renderer;

			// Token: 0x040027BC RID: 10172
			public int MaterialIndex;
		}

		// Token: 0x02000802 RID: 2050
		// (Invoke) Token: 0x060037F0 RID: 14320
		public delegate void VehiclePlayerEvent(Player player);
	}
}
