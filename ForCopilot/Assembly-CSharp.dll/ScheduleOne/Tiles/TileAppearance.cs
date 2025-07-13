using System;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002D0 RID: 720
	public class TileAppearance : MonoBehaviour
	{
		// Token: 0x06000F6A RID: 3946 RVA: 0x000437D8 File Offset: 0x000419D8
		public void Awake()
		{
			this.SetVisible(false);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x000437E1 File Offset: 0x000419E1
		public void SetVisible(bool visible)
		{
			this.tileMesh.enabled = visible;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x000437F0 File Offset: 0x000419F0
		public void SetColor(ETileColor col)
		{
			Material material = this.mat_White;
			switch (col)
			{
			case ETileColor.White:
				material = this.mat_White;
				break;
			case ETileColor.Blue:
				material = this.mat_Blue;
				break;
			case ETileColor.Red:
				material = this.mat_Red;
				break;
			default:
				Console.LogWarning("GridUnitAppearance: enum type not accounted for.", null);
				break;
			}
			this.tileMesh.material = material;
		}

		// Token: 0x04000F9C RID: 3996
		[Header("References")]
		[SerializeField]
		protected MeshRenderer tileMesh;

		// Token: 0x04000F9D RID: 3997
		[Header("Settings")]
		[SerializeField]
		protected Material mat_White;

		// Token: 0x04000F9E RID: 3998
		[SerializeField]
		protected Material mat_Blue;

		// Token: 0x04000F9F RID: 3999
		[SerializeField]
		protected Material mat_Red;
	}
}
