using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001F0 RID: 496
	[RequireComponent(typeof(MeshRenderer))]
	public class WeatherEnclosure : MonoBehaviour
	{
		// Token: 0x04000BC7 RID: 3015
		public Vector2 nearTextureTiling = new Vector3(1f, 1f);

		// Token: 0x04000BC8 RID: 3016
		public Vector2 farTextureTiling = new Vector2(1f, 1f);
	}
}
