using System;
using System.Collections.Generic;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000558 RID: 1368
	public class PatrolGroup
	{
		// Token: 0x06002060 RID: 8288 RVA: 0x00084F99 File Offset: 0x00083199
		public PatrolGroup(FootPatrolRoute route)
		{
			this.Route = route;
			this.CurrentWaypoint = route.StartWaypointIndex;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00084FC0 File Offset: 0x000831C0
		public Vector3 GetDestination(NPC member)
		{
			if (!this.Members.Contains(member))
			{
				Console.LogWarning(member.name + " is not a member of this patrol group!", null);
				return member.transform.position;
			}
			return this.Route.Waypoints[this.CurrentWaypoint].TransformPoint(this.GetMemberOffset(member));
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0008501C File Offset: 0x0008321C
		public void DisbandGroup()
		{
			foreach (NPC npc in new List<NPC>(this.Members))
			{
				(npc as PoliceOfficer).FootPatrolBehaviour.Disable_Networked(null);
				(npc as PoliceOfficer).FootPatrolBehaviour.End_Networked(null);
			}
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00085090 File Offset: 0x00083290
		public void AdvanceGroup()
		{
			this.CurrentWaypoint++;
			if (this.CurrentWaypoint == this.Route.Waypoints.Length)
			{
				this.CurrentWaypoint = 0;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000850BC File Offset: 0x000832BC
		private Vector3 GetMemberOffset(NPC member)
		{
			if (!this.Members.Contains(member))
			{
				Console.LogWarning(member.name + " is not a member of this patrol group!", null);
				return Vector3.zero;
			}
			int num = this.Members.IndexOf(member);
			Vector3 zero = Vector3.zero;
			zero.z -= (float)num * 1f;
			zero.x += ((num % 2 == 0) ? 0.6f : -0.6f);
			return zero;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00085138 File Offset: 0x00083338
		public bool IsGroupReadyToAdvance()
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				if (!(this.Members[i] as PoliceOfficer).FootPatrolBehaviour.IsReadyToAdvance())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x0008517C File Offset: 0x0008337C
		public bool IsPaused()
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				if (this.Members[i].behaviour.activeBehaviour == null || this.Members[i].behaviour.activeBehaviour.GetType() != typeof(FootPatrolBehaviour))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001905 RID: 6405
		public List<NPC> Members = new List<NPC>();

		// Token: 0x04001906 RID: 6406
		public FootPatrolRoute Route;

		// Token: 0x04001907 RID: 6407
		public int CurrentWaypoint;
	}
}
