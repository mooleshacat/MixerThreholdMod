using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009A9 RID: 2473
	public class Eyebrow : MonoBehaviour
	{
		// Token: 0x06004318 RID: 17176 RVA: 0x0011A644 File Offset: 0x00118844
		public void SetScale(float _scale)
		{
			this.scale = _scale;
			this.Model.localScale = new Vector3(this.EyebrowDefaultScale.x, this.EyebrowDefaultScale.y, this.EyebrowDefaultScale.z * this.thickness) * this.scale;
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x0011A69B File Offset: 0x0011889B
		public void SetThickness(float thickness)
		{
			this.thickness = thickness;
			this.SetScale(this.scale);
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x0011A6B0 File Offset: 0x001188B0
		public void SetRestingAngle(float _angle)
		{
			this.restingAngle = _angle;
			base.transform.localRotation = Quaternion.Euler(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, this.restingAngle * ((this.Side == Eyebrow.ESide.Left) ? -1f : 1f));
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0011A710 File Offset: 0x00118910
		public void SetRestingHeight(float normalizedHeight)
		{
			normalizedHeight = Mathf.Clamp(normalizedHeight, -1.1f, 1.5f);
			this.Model.transform.localPosition = new Vector3(this.EyebrowDefaultLocalPos.x, this.EyebrowDefaultLocalPos.y + normalizedHeight * 0.01f, this.EyebrowDefaultLocalPos.z);
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0011A76D File Offset: 0x0011896D
		public void SetColor(Color _col)
		{
			this.col = _col;
			this.Rend.material.color = this.col;
		}

		// Token: 0x04002FD9 RID: 12249
		private const float eyebrowHeightMultiplier = 0.01f;

		// Token: 0x04002FDA RID: 12250
		[SerializeField]
		private Vector3 EyebrowDefaultScale;

		// Token: 0x04002FDB RID: 12251
		[SerializeField]
		private Vector3 EyebrowDefaultLocalPos;

		// Token: 0x04002FDC RID: 12252
		[SerializeField]
		protected Eyebrow.ESide Side;

		// Token: 0x04002FDD RID: 12253
		[SerializeField]
		protected Transform Model;

		// Token: 0x04002FDE RID: 12254
		[SerializeField]
		protected MeshRenderer Rend;

		// Token: 0x04002FDF RID: 12255
		[Header("Eyebrow Data - Readonly")]
		[SerializeField]
		private Color col;

		// Token: 0x04002FE0 RID: 12256
		[SerializeField]
		private float scale = 1f;

		// Token: 0x04002FE1 RID: 12257
		[SerializeField]
		private float thickness = 1f;

		// Token: 0x04002FE2 RID: 12258
		[SerializeField]
		private float restingAngle;

		// Token: 0x020009AA RID: 2474
		public enum ESide
		{
			// Token: 0x04002FE4 RID: 12260
			Right,
			// Token: 0x04002FE5 RID: 12261
			Left
		}
	}
}
