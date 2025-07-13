using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000880 RID: 2176
	public class ActiveOnMeshVisible : MonoBehaviour
	{
		// Token: 0x06003B93 RID: 15251 RVA: 0x000FC544 File Offset: 0x000FA744
		private void LateUpdate()
		{
			if (this.Mesh.isVisible && !this.isVisible)
			{
				this.isVisible = true;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(!this.Reverse);
				}
				return;
			}
			if (!this.Mesh.isVisible && this.isVisible)
			{
				this.isVisible = false;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(this.Reverse);
				}
			}
		}

		// Token: 0x04002A8E RID: 10894
		public MeshRenderer Mesh;

		// Token: 0x04002A8F RID: 10895
		public GameObject[] ObjectsToActivate;

		// Token: 0x04002A90 RID: 10896
		public bool Reverse;

		// Token: 0x04002A91 RID: 10897
		private bool isVisible = true;
	}
}
