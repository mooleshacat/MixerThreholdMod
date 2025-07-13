using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B6 RID: 2230
	public class SmoothRotate : MonoBehaviour
	{
		// Token: 0x06003C57 RID: 15447 RVA: 0x000FE5BC File Offset: 0x000FC7BC
		private void Update()
		{
			if (this.Active)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.Speed, this.Aceleration * Time.deltaTime);
			}
			else
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, this.Aceleration * Time.deltaTime);
			}
			base.transform.Rotate(this.Axis, this.currentSpeed * Time.deltaTime, Space.Self);
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x000FE636 File Offset: 0x000FC836
		public void SetActive(bool active)
		{
			this.Active = active;
		}

		// Token: 0x04002B1C RID: 11036
		public bool Active = true;

		// Token: 0x04002B1D RID: 11037
		public float Speed = 5f;

		// Token: 0x04002B1E RID: 11038
		public float Aceleration = 2f;

		// Token: 0x04002B1F RID: 11039
		public Vector3 Axis = Vector3.up;

		// Token: 0x04002B20 RID: 11040
		private float currentSpeed;
	}
}
