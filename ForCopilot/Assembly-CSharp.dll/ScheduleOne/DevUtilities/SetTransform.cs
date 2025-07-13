using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000735 RID: 1845
	public class SetTransform : MonoBehaviour
	{
		// Token: 0x060031DC RID: 12764 RVA: 0x000D024E File Offset: 0x000CE44E
		private void Awake()
		{
			if (this.SetOnAwake)
			{
				this.Set();
			}
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000D025E File Offset: 0x000CE45E
		private void Update()
		{
			if (this.SetOnUpdate)
			{
				this.Set();
			}
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000D026E File Offset: 0x000CE46E
		private void LateUpdate()
		{
			if (this.SetOnLateUpdate)
			{
				this.Set();
			}
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000D0280 File Offset: 0x000CE480
		private void Set()
		{
			if (base.gameObject.isStatic)
			{
				Console.LogWarning("SetTransform is being used on a static object.", null);
			}
			if (this.SetPosition)
			{
				base.transform.localPosition = this.LocalPosition;
			}
			if (this.SetRotation)
			{
				base.transform.localRotation = Quaternion.Euler(this.LocalRotation);
			}
			if (this.SetScale)
			{
				base.transform.localScale = this.LocalScale;
			}
		}

		// Token: 0x0400231B RID: 8987
		[Header("Frequency Settings")]
		public bool SetOnAwake = true;

		// Token: 0x0400231C RID: 8988
		public bool SetOnUpdate;

		// Token: 0x0400231D RID: 8989
		public bool SetOnLateUpdate;

		// Token: 0x0400231E RID: 8990
		[Header("Transform Settings")]
		public bool SetPosition;

		// Token: 0x0400231F RID: 8991
		public Vector3 LocalPosition = Vector3.zero;

		// Token: 0x04002320 RID: 8992
		public bool SetRotation;

		// Token: 0x04002321 RID: 8993
		public Vector3 LocalRotation = Vector3.zero;

		// Token: 0x04002322 RID: 8994
		public bool SetScale;

		// Token: 0x04002323 RID: 8995
		public Vector3 LocalScale = Vector3.one;
	}
}
