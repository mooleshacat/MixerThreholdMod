using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;

namespace ScheduleOne.Law
{
	// Token: 0x02000606 RID: 1542
	public class LawManager : Singleton<LawManager>
	{
		// Token: 0x060025D6 RID: 9686 RVA: 0x00098DA0 File Offset: 0x00096FA0
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				PoliceOfficer.Officers.Clear();
			});
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00098DD8 File Offset: 0x00096FD8
		public void PoliceCalled(Player target, Crime crime)
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			Console.Log("Police called on " + target.PlayerName, null);
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(target.CrimeData.LastKnownPosition);
			target.CrimeData.RecordLastKnownPosition(false);
			closestPoliceStation.Dispatch(2, target, PoliceStation.EDispatchType.Auto, false);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00098E30 File Offset: 0x00097030
		public PatrolGroup StartFootpatrol(FootPatrolRoute route, int requestedMembers)
		{
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(route.Waypoints[route.StartWaypointIndex].position);
			if (closestPoliceStation.OfficerPool.Count == 0)
			{
				Console.LogWarning(closestPoliceStation.name + " has no officers in its pool!", null);
				return null;
			}
			PatrolGroup patrolGroup = new PatrolGroup(route);
			List<PoliceOfficer> list = new List<PoliceOfficer>();
			int num = 0;
			while (num < requestedMembers && closestPoliceStation.OfficerPool.Count != 0)
			{
				list.Add(closestPoliceStation.PullOfficer());
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].StartFootPatrol(patrolGroup, false);
			}
			return patrolGroup;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00098ED4 File Offset: 0x000970D4
		public PoliceOfficer StartVehiclePatrol(VehiclePatrolRoute route)
		{
			PoliceStation closestPoliceStation = PoliceStation.GetClosestPoliceStation(route.Waypoints[route.StartWaypointIndex].position);
			if (closestPoliceStation.OfficerPool.Count == 0)
			{
				Console.LogWarning(closestPoliceStation.name + " has no officers in its pool!", null);
				return null;
			}
			LandVehicle landVehicle = closestPoliceStation.CreateVehicle();
			PoliceOfficer policeOfficer = closestPoliceStation.PullOfficer();
			policeOfficer.AssignedVehicle = landVehicle;
			policeOfficer.EnterVehicle(null, landVehicle);
			policeOfficer.StartVehiclePatrol(route, landVehicle);
			return policeOfficer;
		}

		// Token: 0x04001BE0 RID: 7136
		public const int DISPATCH_OFFICER_COUNT = 2;

		// Token: 0x04001BE1 RID: 7137
		public static float DISPATCH_VEHICLE_USE_THRESHOLD = 25f;
	}
}
