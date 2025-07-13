using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000869 RID: 2153
	[RequireComponent(typeof(TrashContainer))]
	public class TrashContainerContents : MonoBehaviour
	{
		// Token: 0x06003ABF RID: 15039 RVA: 0x000F8C49 File Offset: 0x000F6E49
		protected void Start()
		{
			this.TrashContainer.onTrashLevelChanged.AddListener(new UnityAction(this.UpdateVisuals));
			this.UpdateVisuals();
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x000F8C70 File Offset: 0x000F6E70
		private void UpdateVisuals()
		{
			float t = (float)this.TrashContainer.TrashLevel / (float)this.TrashContainer.TrashCapacity;
			this.ContentsTransform.transform.localPosition = Vector3.Lerp(this.VisualsMinTransform.localPosition, this.VisualsMaxTransform.localPosition, t);
			this.ContentsTransform.transform.localScale = Vector3.Lerp(this.VisualsMinTransform.localScale, this.VisualsMaxTransform.localScale, t);
			this.VisualsContainer.gameObject.SetActive(this.TrashContainer.TrashLevel > 0);
			this.Collider.enabled = (this.TrashContainer.TrashLevel >= this.TrashContainer.TrashCapacity);
		}

		// Token: 0x04002A20 RID: 10784
		public TrashContainer TrashContainer;

		// Token: 0x04002A21 RID: 10785
		[Header("References")]
		public Transform ContentsTransform;

		// Token: 0x04002A22 RID: 10786
		public Transform VisualsContainer;

		// Token: 0x04002A23 RID: 10787
		public Transform VisualsMinTransform;

		// Token: 0x04002A24 RID: 10788
		public Transform VisualsMaxTransform;

		// Token: 0x04002A25 RID: 10789
		public Collider Collider;
	}
}
