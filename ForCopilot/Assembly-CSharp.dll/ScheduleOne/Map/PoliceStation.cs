using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C8D RID: 3213
	public class PoliceStation : NPCEnterableBuilding
	{
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x0017BFE2 File Offset: 0x0017A1E2
		// (set) Token: 0x06005A22 RID: 23074 RVA: 0x0017BFEA File Offset: 0x0017A1EA
		public float TimeSinceLastDispatch { get; private set; }

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x0017BFF3 File Offset: 0x0017A1F3
		private int deployedVehicleCount
		{
			get
			{
				return (from v in this.deployedVehicles
				where v != null
				select v).Count<LandVehicle>();
			}
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x0017C024 File Offset: 0x0017A224
		protected override void Awake()
		{
			base.Awake();
			if (!PoliceStation.PoliceStations.Contains(this))
			{
				PoliceStation.PoliceStations.Add(this);
			}
			base.InvokeRepeating("CleanVehicleList", 0f, 5f);
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x0017C059 File Offset: 0x0017A259
		private void OnDestroy()
		{
			if (PoliceStation.PoliceStations.Contains(this))
			{
				PoliceStation.PoliceStations.Remove(this);
			}
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x0017C074 File Offset: 0x0017A274
		private void Update()
		{
			this.TimeSinceLastDispatch += Time.deltaTime;
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x0017C088 File Offset: 0x0017A288
		private void CleanVehicleList()
		{
			for (int i = 0; i < this.deployedVehicles.Count; i++)
			{
				if (this.deployedVehicles[i] == null)
				{
					this.deployedVehicles.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x0017C0D0 File Offset: 0x0017A2D0
		public void Dispatch(int requestedOfficerCount, Player targetPlayer, PoliceStation.EDispatchType type = PoliceStation.EDispatchType.Auto, bool beginAsSighted = false)
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("Attempted to dispatch officers from a client, this is not allowed.", null);
				return;
			}
			if (requestedOfficerCount <= 0)
			{
				return;
			}
			if (requestedOfficerCount > 4)
			{
				Console.LogWarning("Attempted to dispatch more than 4 officers, this is not allowed.", null);
				return;
			}
			List<PoliceOfficer> list = new List<PoliceOfficer>();
			for (int i = 0; i < requestedOfficerCount; i++)
			{
				if (this.OfficerPool.Count > 0)
				{
					list.Add(this.PullOfficer());
				}
			}
			if (list.Count == 0)
			{
				Console.LogWarning("Attempted to dispatch officers, but there are no officers in the pool.", null);
				return;
			}
			bool flag = false;
			if (type == PoliceStation.EDispatchType.Auto)
			{
				flag = (Vector3.Distance(targetPlayer.CrimeData.LastKnownPosition, this.SpawnPoint.position) > LawManager.DISPATCH_VEHICLE_USE_THRESHOLD || targetPlayer.CurrentVehicle != null);
			}
			else if (type == PoliceStation.EDispatchType.UseVehicle)
			{
				flag = true;
			}
			if (flag && this.deployedVehicleCount < this.VehicleLimit)
			{
				LandVehicle landVehicle = this.CreateVehicle();
				list[0].AssignedVehicle = landVehicle;
				list[0].EnterVehicle(null, landVehicle);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].BeginVehiclePursuit_Networked(targetPlayer.NetworkObject, landVehicle.NetworkObject, beginAsSighted);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				list[k].BeginFootPursuit_Networked(targetPlayer.NetworkObject, true);
			}
			this.TimeSinceLastDispatch = 0f;
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x0017C21C File Offset: 0x0017A41C
		public PoliceOfficer PullOfficer()
		{
			if (this.OfficerPool.Count == 0)
			{
				return null;
			}
			PoliceOfficer policeOfficer = this.OfficerPool[UnityEngine.Random.Range(0, this.OfficerPool.Count)];
			this.OfficerPool.Remove(policeOfficer);
			policeOfficer.Activate();
			return policeOfficer;
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x0017C26C File Offset: 0x0017A46C
		public LandVehicle CreateVehicle()
		{
			Transform target = this.VehicleSpawnPoints[0];
			for (int i = 0; i < this.VehicleSpawnPoints.Length; i++)
			{
				if (PoliceStation.<CreateVehicle>g__IsSpawnPointAvailable|21_0(this.VehicleSpawnPoints[i]))
				{
					target = this.VehicleSpawnPoints[i];
					break;
				}
			}
			LandVehicle landVehicle = this.PoliceVehiclePrefabs[UnityEngine.Random.Range(0, this.PoliceVehiclePrefabs.Length)];
			Tuple<Vector3, Quaternion> alignmentTransform = landVehicle.GetAlignmentTransform(target, EParkingAlignment.RearToKerb);
			LandVehicle landVehicle2 = NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle(landVehicle.VehicleCode, alignmentTransform.Item1, alignmentTransform.Item2, false);
			this.deployedVehicles.Add(landVehicle2);
			return landVehicle2;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x0017C2FF File Offset: 0x0017A4FF
		public override void NPCEnteredBuilding(NPC npc)
		{
			base.NPCEnteredBuilding(npc);
			if (npc is PoliceOfficer && !this.OfficerPool.Contains(npc as PoliceOfficer))
			{
				this.OfficerPool.Add(npc as PoliceOfficer);
			}
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x0017C334 File Offset: 0x0017A534
		public override void NPCExitedBuilding(NPC npc)
		{
			base.NPCExitedBuilding(npc);
			if (npc is PoliceOfficer)
			{
				this.OfficerPool.Remove(npc as PoliceOfficer);
			}
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x0017C357 File Offset: 0x0017A557
		public static PoliceStation GetClosestPoliceStation(Vector3 point)
		{
			return PoliceStation.PoliceStations[0];
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x0017C398 File Offset: 0x0017A598
		[CompilerGenerated]
		internal static bool <CreateVehicle>g__IsSpawnPointAvailable|21_0(Transform spawnPoint)
		{
			Collider[] array = Physics.OverlapSphere(spawnPoint.position, 2f, 1 << LayerMask.NameToLayer("Vehicle"));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetComponentInParent<LandVehicle>() != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004231 RID: 16945
		public static List<PoliceStation> PoliceStations = new List<PoliceStation>();

		// Token: 0x04004232 RID: 16946
		public int VehicleLimit = 5;

		// Token: 0x04004233 RID: 16947
		[Header("References")]
		public Transform SpawnPoint;

		// Token: 0x04004234 RID: 16948
		public Transform[] VehicleSpawnPoints;

		// Token: 0x04004235 RID: 16949
		public Transform[] PossessedVehicleSpawnPoints;

		// Token: 0x04004236 RID: 16950
		[Header("Prefabs")]
		public LandVehicle[] PoliceVehiclePrefabs;

		// Token: 0x04004237 RID: 16951
		public List<PoliceOfficer> OfficerPool = new List<PoliceOfficer>();

		// Token: 0x04004239 RID: 16953
		[SerializeField]
		private List<LandVehicle> deployedVehicles = new List<LandVehicle>();

		// Token: 0x02000C8E RID: 3214
		public enum EDispatchType
		{
			// Token: 0x0400423B RID: 16955
			Auto,
			// Token: 0x0400423C RID: 16956
			UseVehicle,
			// Token: 0x0400423D RID: 16957
			OnFoot
		}
	}
}
