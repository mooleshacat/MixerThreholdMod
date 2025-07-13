using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScheduleOne.UI
{
	// Token: 0x02000A8E RID: 2702
	public class MaskedObject : UIBehaviour
	{
		// Token: 0x060048A0 RID: 18592 RVA: 0x00130CA2 File Offset: 0x0012EEA2
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (this.initialized)
			{
				this.SetTargetClippingRect();
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x00130CB8 File Offset: 0x0012EEB8
		protected override void Awake()
		{
			base.Awake();
			this.Initialize(this.rootCanvas, this.maskRectTransform);
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x00130CD4 File Offset: 0x0012EED4
		protected override void Start()
		{
			this.canvasRenderersToClip.Add(this.canvasRendererToClip);
			if (this.includeChildren)
			{
				CanvasRenderer[] componentsInChildren = base.GetComponentsInChildren<CanvasRenderer>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != this.canvasRendererToClip)
					{
						this.canvasRenderersToClip.Add(componentsInChildren[i]);
					}
				}
			}
			this.SetTargetClippingRect();
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00130D34 File Offset: 0x0012EF34
		public void Initialize(Canvas rootCanvas, RectTransform maskRectTransform)
		{
			this.rootCanvas = rootCanvas;
			this.maskRectTransform = maskRectTransform;
			this.SetTargetClippingRect();
			this.initialized = true;
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00130D54 File Offset: 0x0012EF54
		private void SetTargetClippingRect()
		{
			Rect rect = this.maskRectTransform.rect;
			rect.center += this.rootCanvas.transform.InverseTransformPoint(this.maskRectTransform.position);
			foreach (CanvasRenderer canvasRenderer in this.canvasRenderersToClip)
			{
				canvasRenderer.EnableRectClipping(rect);
			}
		}

		// Token: 0x04003546 RID: 13638
		[SerializeField]
		private CanvasRenderer canvasRendererToClip;

		// Token: 0x04003547 RID: 13639
		public bool includeChildren;

		// Token: 0x04003548 RID: 13640
		[SerializeField]
		private Canvas rootCanvas;

		// Token: 0x04003549 RID: 13641
		[SerializeField]
		private RectTransform maskRectTransform;

		// Token: 0x0400354A RID: 13642
		private bool initialized;

		// Token: 0x0400354B RID: 13643
		private List<CanvasRenderer> canvasRenderersToClip = new List<CanvasRenderer>();
	}
}
