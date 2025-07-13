using System;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C9 RID: 1737
	public class RollerDoor : MonoBehaviour
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x000C90C0 File Offset: 0x000C72C0
		// (set) Token: 0x06002FD5 RID: 12245 RVA: 0x000C90C8 File Offset: 0x000C72C8
		public bool IsOpen { get; protected set; } = true;

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000C90D1 File Offset: 0x000C72D1
		private void Awake()
		{
			this.Door.localPosition = (this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000C90F4 File Offset: 0x000C72F4
		private void LateUpdate()
		{
			this.timeSinceValueChange += Time.deltaTime;
			if (this.timeSinceValueChange < this.LerpTime)
			{
				Vector3 b = this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed;
				this.Door.localPosition = Vector3.Lerp(this.startPos, b, this.timeSinceValueChange / this.LerpTime);
			}
			else
			{
				this.Door.localPosition = (this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed);
			}
			if (this.Blocker != null)
			{
				this.Blocker.gameObject.SetActive(!this.IsOpen);
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000C91A6 File Offset: 0x000C73A6
		public void Open()
		{
			if (this.IsOpen)
			{
				return;
			}
			if (!this.CanOpen())
			{
				return;
			}
			this.IsOpen = true;
			this.timeSinceValueChange = 0f;
			this.startPos = this.Door.localPosition;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000C91DD File Offset: 0x000C73DD
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			this.timeSinceValueChange = 0f;
			this.startPos = this.Door.localPosition;
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanOpen()
		{
			return true;
		}

		// Token: 0x040021A6 RID: 8614
		[Header("Settings")]
		public Transform Door;

		// Token: 0x040021A7 RID: 8615
		public Vector3 LocalPos_Open;

		// Token: 0x040021A8 RID: 8616
		public Vector3 LocalPos_Closed;

		// Token: 0x040021A9 RID: 8617
		public float LerpTime = 1f;

		// Token: 0x040021AA RID: 8618
		public GameObject Blocker;

		// Token: 0x040021AB RID: 8619
		private Vector3 startPos = Vector3.zero;

		// Token: 0x040021AC RID: 8620
		private float timeSinceValueChange;
	}
}
