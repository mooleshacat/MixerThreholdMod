using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200015A RID: 346
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamAbstractBase))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-triggerzone/")]
	public class TriggerZone : MonoBehaviour
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001D618 File Offset: 0x0001B818
		private TriggerZone.TriggerZoneUpdateRate updateRate
		{
			get
			{
				if (UtilsBeamProps.GetDimensions(this.m_Beam) == Dimensions.Dim3D)
				{
					return TriggerZone.TriggerZoneUpdateRate.OnEnable;
				}
				if (!(this.m_DynamicOcclusionRaycasting != null))
				{
					return TriggerZone.TriggerZoneUpdateRate.OnEnable;
				}
				return TriggerZone.TriggerZoneUpdateRate.OnOcclusionChange;
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001D63C File Offset: 0x0001B83C
		private void OnEnable()
		{
			this.m_Beam = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.m_DynamicOcclusionRaycasting = base.GetComponent<DynamicOcclusionRaycasting>();
			TriggerZone.TriggerZoneUpdateRate updateRate = this.updateRate;
			if (updateRate == TriggerZone.TriggerZoneUpdateRate.OnEnable)
			{
				this.ComputeZone();
				base.enabled = false;
				return;
			}
			if (updateRate != TriggerZone.TriggerZoneUpdateRate.OnOcclusionChange)
			{
				return;
			}
			if (this.m_DynamicOcclusionRaycasting)
			{
				this.m_DynamicOcclusionRaycasting.onOcclusionProcessed += this.OnOcclusionProcessed;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001D6A2 File Offset: 0x0001B8A2
		private void OnOcclusionProcessed()
		{
			this.ComputeZone();
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		private void ComputeZone()
		{
			if (this.m_Beam)
			{
				float coneRadiusStart = UtilsBeamProps.GetConeRadiusStart(this.m_Beam);
				float num = UtilsBeamProps.GetFallOffEnd(this.m_Beam) * this.rangeMultiplier;
				float num2 = Mathf.LerpUnclamped(coneRadiusStart, UtilsBeamProps.GetConeRadiusEnd(this.m_Beam), this.rangeMultiplier);
				if (UtilsBeamProps.GetDimensions(this.m_Beam) == Dimensions.Dim3D)
				{
					MeshCollider orAddComponent = base.gameObject.GetOrAddComponent<MeshCollider>();
					Mathf.Min(UtilsBeamProps.GetGeomSides(this.m_Beam), 8);
					Mesh mesh = MeshGenerator.GenerateConeZ_Radii_DoubleCaps(num, coneRadiusStart, num2, 8, false);
					mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
					orAddComponent.sharedMesh = mesh;
					orAddComponent.convex = this.setIsTrigger;
					orAddComponent.isTrigger = this.setIsTrigger;
					return;
				}
				if (this.m_PolygonCollider2D == null)
				{
					this.m_PolygonCollider2D = base.gameObject.GetOrAddComponent<PolygonCollider2D>();
				}
				Vector2[] array = new Vector2[]
				{
					new Vector2(0f, -coneRadiusStart),
					new Vector2(num, -num2),
					new Vector2(num, num2),
					new Vector2(0f, coneRadiusStart)
				};
				if (this.m_DynamicOcclusionRaycasting && this.m_DynamicOcclusionRaycasting.planeEquationWS.IsValid())
				{
					Plane planeEquationWS = this.m_DynamicOcclusionRaycasting.planeEquationWS;
					if (Utils.IsAlmostZero(planeEquationWS.normal.z))
					{
						Vector3 vector = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.zero);
						Vector3 vector2 = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.up);
						if (Utils.IsAlmostZero(Vector3.SqrMagnitude(vector - vector2)))
						{
							vector = planeEquationWS.ClosestPointOnPlaneCustom(Vector3.right);
						}
						vector = base.transform.InverseTransformPoint(vector);
						vector2 = base.transform.InverseTransformPoint(vector2);
						PolygonHelper.Plane2D plane2D = PolygonHelper.Plane2D.FromPoints(vector, vector2);
						if (plane2D.normal.x > 0f)
						{
							plane2D.Flip();
						}
						array = plane2D.CutConvex(array);
					}
				}
				this.m_PolygonCollider2D.points = array;
				this.m_PolygonCollider2D.isTrigger = this.setIsTrigger;
			}
		}

		// Token: 0x04000768 RID: 1896
		public const string ClassName = "TriggerZone";

		// Token: 0x04000769 RID: 1897
		public bool setIsTrigger = true;

		// Token: 0x0400076A RID: 1898
		public float rangeMultiplier = 1f;

		// Token: 0x0400076B RID: 1899
		private const int kMeshColliderNumSides = 8;

		// Token: 0x0400076C RID: 1900
		private VolumetricLightBeamAbstractBase m_Beam;

		// Token: 0x0400076D RID: 1901
		private DynamicOcclusionRaycasting m_DynamicOcclusionRaycasting;

		// Token: 0x0400076E RID: 1902
		private PolygonCollider2D m_PolygonCollider2D;

		// Token: 0x0200015B RID: 347
		private enum TriggerZoneUpdateRate
		{
			// Token: 0x04000770 RID: 1904
			OnEnable,
			// Token: 0x04000771 RID: 1905
			OnOcclusionChange
		}
	}
}
