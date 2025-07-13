using System;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C93 RID: 3219
	public class TrafficLight : MonoBehaviour
	{
		// Token: 0x06005A47 RID: 23111 RVA: 0x0017CA40 File Offset: 0x0017AC40
		protected virtual void Start()
		{
			this.ApplyState();
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x0017CA48 File Offset: 0x0017AC48
		protected virtual void Update()
		{
			if (this.appliedState != this.state)
			{
				this.ApplyState();
			}
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x0017CA60 File Offset: 0x0017AC60
		protected virtual void ApplyState()
		{
			this.appliedState = this.state;
			this.redMesh.material = this.redOff_Mat;
			this.orangeMesh.material = this.orangeOff_Mat;
			this.greenMesh.material = this.greenOff_Mat;
			switch (this.state)
			{
			case TrafficLight.State.Red:
				this.redMesh.material = this.redOn_Mat;
				return;
			case TrafficLight.State.Orange:
				this.orangeMesh.material = this.orangeOn_Mat;
				return;
			case TrafficLight.State.Green:
				this.greenMesh.material = this.greenOn_Mat;
				return;
			default:
				return;
			}
		}

		// Token: 0x04004258 RID: 16984
		public static float amberTime = 3f;

		// Token: 0x04004259 RID: 16985
		[Header("References")]
		[SerializeField]
		protected MeshRenderer redMesh;

		// Token: 0x0400425A RID: 16986
		[SerializeField]
		protected MeshRenderer orangeMesh;

		// Token: 0x0400425B RID: 16987
		[SerializeField]
		protected MeshRenderer greenMesh;

		// Token: 0x0400425C RID: 16988
		[Header("Materials")]
		[SerializeField]
		protected Material redOn_Mat;

		// Token: 0x0400425D RID: 16989
		[SerializeField]
		protected Material redOff_Mat;

		// Token: 0x0400425E RID: 16990
		[SerializeField]
		protected Material orangeOn_Mat;

		// Token: 0x0400425F RID: 16991
		[SerializeField]
		protected Material orangeOff_Mat;

		// Token: 0x04004260 RID: 16992
		[SerializeField]
		protected Material greenOn_Mat;

		// Token: 0x04004261 RID: 16993
		[SerializeField]
		protected Material greenOff_Mat;

		// Token: 0x04004262 RID: 16994
		[Header("Settings")]
		public TrafficLight.State state;

		// Token: 0x04004263 RID: 16995
		private TrafficLight.State appliedState;

		// Token: 0x02000C94 RID: 3220
		public enum State
		{
			// Token: 0x04004265 RID: 16997
			Red,
			// Token: 0x04004266 RID: 16998
			Orange,
			// Token: 0x04004267 RID: 16999
			Green
		}
	}
}
