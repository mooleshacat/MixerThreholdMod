using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000711 RID: 1809
	public class AstarUtility : MonoBehaviour
	{
		// Token: 0x0600310A RID: 12554 RVA: 0x000CCDC8 File Offset: 0x000CAFC8
		public static Vector3 GetClosestPointOnGraph(Vector3 point, string GraphName)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName(GraphName);
			return AstarPath.active.GetNearest(point, nnconstraint).position;
		}
	}
}
