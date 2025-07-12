using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using ScheduleOne.DevUtilities;
using ScheduleOne.Math;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x0200082B RID: 2091
	public class NavigationUtility
	{
		// Token: 0x060038B3 RID: 14515 RVA: 0x000EEEFC File Offset: 0x000ED0FC
		public static Coroutine CalculatePath(Vector3 startPosition, Vector3 destination, NavigationSettings navSettings, DriveFlags flags, Seeker generalSeeker, Seeker roadSeeker, NavigationUtility.NavigationCalculationCallback callback)
		{
			NavigationUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new NavigationUtility.<>c__DisplayClass5_0();
			CS$<>8__locals1.flags = flags;
			CS$<>8__locals1.startPosition = startPosition;
			CS$<>8__locals1.destination = destination;
			CS$<>8__locals1.generalSeeker = generalSeeker;
			CS$<>8__locals1.roadSeeker = roadSeeker;
			CS$<>8__locals1.navSettings = navSettings;
			CS$<>8__locals1.callback = callback;
			return Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<CalculatePath>g__Routine|2());
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000EEF54 File Offset: 0x000ED154
		private static void AdjustExitPoint(PathGroup group)
		{
			if (group.entryToExitPath.vectorPath.Count < 4 || group.exitToDestinationPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.exitToDestinationPath.GetTotalLength() < 5f)
			{
				return;
			}
			for (int i = Mathf.Min(5, group.exitToDestinationPath.vectorPath.Count - 1); i >= 0; i--)
			{
				Vector3 vector = group.exitToDestinationPath.vectorPath[i];
				Vector3 vector2 = Vector3.zero;
				float num = float.MaxValue;
				int num2 = 0;
				for (int j = 0; j < 3; j++)
				{
					int num3 = group.entryToExitPath.vectorPath.Count - 1 - j;
					int index = num3 - 1;
					Vector3 line_end = group.entryToExitPath.vectorPath[num3];
					Vector3 line_start = group.entryToExitPath.vectorPath[index];
					Vector3 closestPointOnFiniteLine = NavigationUtility.GetClosestPointOnFiniteLine(vector, line_start, line_end);
					if (Vector3.Distance(vector, closestPointOnFiniteLine) < num)
					{
						num = Vector3.Distance(vector, closestPointOnFiniteLine);
						vector2 = closestPointOnFiniteLine;
						num2 = num3;
					}
				}
				if (vector2 == Vector3.zero)
				{
					Debug.LogWarning("Failed to find closest entry-to-exit path point");
					return;
				}
				float num4 = 0f;
				for (int k = 0; k < i; k++)
				{
					num4 += Vector3.Distance(group.exitToDestinationPath.vectorPath[k], group.exitToDestinationPath.vectorPath[k + 1]);
				}
				num4 += Vector3.Distance(vector2, group.entryToExitPath.vectorPath[num2]);
				for (int l = num2; l < group.entryToExitPath.vectorPath.Count - 1; l++)
				{
					num4 += Vector3.Distance(group.entryToExitPath.vectorPath[l], group.entryToExitPath.vectorPath[l + 1]);
				}
				if (num < num4 * 0.5f)
				{
					for (int m = num2; m < group.entryToExitPath.vectorPath.Count; m++)
					{
						group.entryToExitPath.vectorPath.RemoveAt(num2);
					}
					group.entryToExitPath.vectorPath.Insert(num2, vector2);
					for (int n = 0; n < i; n++)
					{
						group.exitToDestinationPath.vectorPath.RemoveAt(0);
					}
					Debug.DrawLine(vector, vector2, Color.green, 1f);
					return;
				}
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000EF1B4 File Offset: 0x000ED3B4
		private static void AdjustEntryPoint(PathGroup group)
		{
			if (group.startToEntryPath == null || group.startToEntryPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.startToEntryPath.GetTotalLength() < 5f)
			{
				return;
			}
			if (group.entryToExitPath == null || group.entryToExitPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.entryToExitPath.GetTotalLength() < 5f)
			{
				return;
			}
			float d = 2f;
			Vector3 a = group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 1];
			Vector3 b = group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 2];
			Vector3 normalized = (a - b).normalized;
			Vector3 value = a - normalized * d;
			group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 1] = value;
			Vector3 vector = group.entryToExitPath.vectorPath[0];
			normalized = (group.entryToExitPath.vectorPath[1] - vector).normalized;
			Vector3 value2 = vector + normalized * d;
			group.entryToExitPath.vectorPath[0] = value2;
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000EF304 File Offset: 0x000ED504
		private static bool DoesCloseDistanceExist(List<Vector3> vectorList, Vector3 point, float thresholdDistance)
		{
			using (List<Vector3>.Enumerator enumerator = vectorList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (Vector3.Distance(enumerator.Current, point) <= thresholdDistance)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000EF35C File Offset: 0x000ED55C
		private static IEnumerator GenerateNavigationGroup(Vector3 startPoint, Vector3 entryPoint, NodeLink exitLink, Vector3 exitPoint, Vector3 destination, Seeker generalSeeker, Seeker roadSeeker, NavigationUtility.PathGroupEvent callback)
		{
			NavigationUtility.<>c__DisplayClass9_0 CS$<>8__locals1 = new NavigationUtility.<>c__DisplayClass9_0();
			Vector3 closestPointOnGraph = AstarUtility.GetClosestPointOnGraph(startPoint, "General Vehicle Graph");
			Vector3 destinationOnGraph = AstarUtility.GetClosestPointOnGraph(destination, "General Vehicle Graph");
			CS$<>8__locals1.lastCalculatedPath = null;
			generalSeeker.StartPath(closestPointOnGraph, entryPoint, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			Path path_StartToEntry = CS$<>8__locals1.lastCalculatedPath;
			CS$<>8__locals1.lastCalculatedPath = null;
			Vector3 position = NodeLink.GetClosestLinks(entryPoint)[0].Start.position;
			roadSeeker.StartPath(position, exitLink.Start.position, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			CS$<>8__locals1.lastCalculatedPath.vectorPath[0] = entryPoint;
			CS$<>8__locals1.lastCalculatedPath.vectorPath.Add(exitPoint);
			Path path_EntryToExit = CS$<>8__locals1.lastCalculatedPath;
			CS$<>8__locals1.lastCalculatedPath = null;
			generalSeeker.StartPath(exitPoint, destinationOnGraph, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			Path lastCalculatedPath = CS$<>8__locals1.lastCalculatedPath;
			callback(new PathGroup
			{
				entryPoint = entryPoint,
				startToEntryPath = path_StartToEntry,
				entryToExitPath = path_EntryToExit,
				exitToDestinationPath = lastCalculatedPath
			});
			yield break;
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000EF3AC File Offset: 0x000ED5AC
		public static void DrawPath(PathGroup group, float duration = 10f)
		{
			if (group.startToEntryPath != null)
			{
				for (int i = 1; i < group.startToEntryPath.vectorPath.Count; i++)
				{
					Debug.DrawLine(group.startToEntryPath.vectorPath[i], group.startToEntryPath.vectorPath[i - 1], Color.red, duration);
				}
			}
			if (group.entryToExitPath != null)
			{
				for (int j = 1; j < group.entryToExitPath.vectorPath.Count; j++)
				{
					if (j % 2 == 0)
					{
						Debug.DrawLine(group.entryToExitPath.vectorPath[j], group.entryToExitPath.vectorPath[j - 1], Color.blue, duration);
					}
					else
					{
						Debug.DrawLine(group.entryToExitPath.vectorPath[j], group.entryToExitPath.vectorPath[j - 1], Color.white, duration);
					}
				}
			}
			if (group.exitToDestinationPath != null)
			{
				for (int k = 1; k < group.exitToDestinationPath.vectorPath.Count; k++)
				{
					Debug.DrawLine(group.exitToDestinationPath.vectorPath[k], group.exitToDestinationPath.vectorPath[k - 1], Color.yellow, duration);
				}
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000EF4E8 File Offset: 0x000ED6E8
		private static PathSmoothingUtility.SmoothedPath GetSmoothedPath(PathGroup group)
		{
			List<Vector3> list = new List<Vector3>();
			if (group.startToEntryPath != null)
			{
				list.AddRange(group.startToEntryPath.vectorPath);
			}
			if (group.entryToExitPath != null)
			{
				list.AddRange(group.entryToExitPath.vectorPath);
			}
			if (group.exitToDestinationPath != null)
			{
				list.AddRange(group.exitToDestinationPath.vectorPath);
			}
			return PathSmoothingUtility.CalculateSmoothedPath(list, 5f);
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000EF554 File Offset: 0x000ED754
		public static Vector3 SampleVehicleGraph(Vector3 destination)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			return AstarPath.active.GetNearest(destination, nnconstraint).position;
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x000EF588 File Offset: 0x000ED788
		public static Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
		{
			Vector3 vector = line_end - line_start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float d = Mathf.Clamp(Vector3.Dot(point - line_start, vector), 0f, magnitude);
			return line_start + vector * d;
		}

		// Token: 0x040028A8 RID: 10408
		public const float ROAD_MULTIPLIER = 1f;

		// Token: 0x040028A9 RID: 10409
		public const float OFFROAD_MULTIPLIER = 3f;

		// Token: 0x0200082C RID: 2092
		public enum ENavigationCalculationResult
		{
			// Token: 0x040028AB RID: 10411
			Success,
			// Token: 0x040028AC RID: 10412
			Failed
		}

		// Token: 0x0200082D RID: 2093
		// (Invoke) Token: 0x060038BE RID: 14526
		public delegate void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath path);

		// Token: 0x0200082E RID: 2094
		// (Invoke) Token: 0x060038C2 RID: 14530
		public delegate void PathGroupEvent(PathGroup calculatedGroup);
	}
}
