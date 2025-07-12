using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005C9 RID: 1481
	public class TransitLineVisuals : MonoBehaviour
	{
		// Token: 0x06002475 RID: 9333 RVA: 0x0009545F File Offset: 0x0009365F
		public void SetSourcePosition(Vector3 position)
		{
			this.Renderer.SetPosition(0, position);
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0009546E File Offset: 0x0009366E
		public void SetDestinationPosition(Vector3 position)
		{
			this.Renderer.SetPosition(1, position);
		}

		// Token: 0x04001B0A RID: 6922
		public LineRenderer Renderer;
	}
}
