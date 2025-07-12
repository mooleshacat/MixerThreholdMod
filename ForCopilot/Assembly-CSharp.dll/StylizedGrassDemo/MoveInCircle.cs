using System;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x0200016D RID: 365
	public class MoveInCircle : MonoBehaviour
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x0001FB52 File Offset: 0x0001DD52
		private void Update()
		{
			this.Move();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001FB5C File Offset: 0x0001DD5C
		private void Move()
		{
			float x = Mathf.Sin(Time.realtimeSinceStartup * this.speed) * this.radius + this.offset.x;
			float y = base.transform.position.y + this.offset.y;
			float z = Mathf.Cos(Time.realtimeSinceStartup * this.speed) * this.radius + this.offset.z;
			base.transform.localPosition = new Vector3(x, y, z);
		}

		// Token: 0x040007CB RID: 1995
		public float radius = 1f;

		// Token: 0x040007CC RID: 1996
		public float speed = 1f;

		// Token: 0x040007CD RID: 1997
		public Vector3 offset;
	}
}
