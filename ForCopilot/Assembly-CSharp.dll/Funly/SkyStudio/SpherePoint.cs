using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E1 RID: 481
	[Serializable]
	public class SpherePoint
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x0002EB2D File Offset: 0x0002CD2D
		public SpherePoint(float horizontalRotation, float verticalRotation)
		{
			this.horizontalRotation = horizontalRotation;
			this.verticalRotation = verticalRotation;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public SpherePoint(Vector3 worldDirection)
		{
			Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
			this.horizontalRotation = vector.x;
			this.verticalRotation = vector.y;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002EB78 File Offset: 0x0002CD78
		public void SetFromWorldDirection(Vector3 worldDirection)
		{
			Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
			this.horizontalRotation = vector.x;
			this.verticalRotation = vector.y;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002EBA4 File Offset: 0x0002CDA4
		public Vector3 GetWorldDirection()
		{
			return SphereUtility.SphericalCoordinateToDirection(new Vector2(this.horizontalRotation, this.verticalRotation));
		}

		// Token: 0x04000B6C RID: 2924
		public float horizontalRotation;

		// Token: 0x04000B6D RID: 2925
		public float verticalRotation;

		// Token: 0x04000B6E RID: 2926
		public const float MinHorizontalRotation = -3.1415927f;

		// Token: 0x04000B6F RID: 2927
		public const float MaxHorizontalRotation = 3.1415927f;

		// Token: 0x04000B70 RID: 2928
		public const float MinVerticalRotation = -1.5707964f;

		// Token: 0x04000B71 RID: 2929
		public const float MaxVerticalRotation = 1.5707964f;
	}
}
