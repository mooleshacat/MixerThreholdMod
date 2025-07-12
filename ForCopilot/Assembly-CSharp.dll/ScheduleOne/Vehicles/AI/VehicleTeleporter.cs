using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000846 RID: 2118
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleTeleporter : MonoBehaviour
	{
		// Token: 0x06003935 RID: 14645 RVA: 0x000F31A4 File Offset: 0x000F13A4
		public void MoveToGraph(bool resetRotation = true)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, nnconstraint);
			base.transform.position = nearest.position + base.transform.up * base.GetComponent<LandVehicle>().boundingBoxDimensions.y / 2f;
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.identity;
			}
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x000F3234 File Offset: 0x000F1434
		public void MoveToRoadNetwork(bool resetRotation = true)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("Road Nodes");
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, nnconstraint);
			base.transform.position = nearest.position + base.transform.up * base.GetComponent<LandVehicle>().boundingBoxDimensions.y / 2f;
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.identity;
			}
		}
	}
}
