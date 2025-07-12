using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Tooltips
{
	// Token: 0x02000AA2 RID: 2722
	public class Tooltip : MonoBehaviour
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06004914 RID: 18708 RVA: 0x00132828 File Offset: 0x00130A28
		public Vector3 labelPosition
		{
			get
			{
				if (this.isWorldspace)
				{
					return RectTransformUtility.WorldToScreenPoint(Singleton<GameplayMenu>.Instance.OverlayCamera, this.rect.position);
				}
				return this.rect.position + new Vector3(this.labelOffset.x, this.labelOffset.y, 0f);
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x0013288D File Offset: 0x00130A8D
		// (set) Token: 0x06004916 RID: 18710 RVA: 0x00132895 File Offset: 0x00130A95
		public bool isWorldspace { get; private set; }

		// Token: 0x06004917 RID: 18711 RVA: 0x001328A0 File Offset: 0x00130AA0
		protected virtual void Awake()
		{
			this.rect = base.GetComponent<RectTransform>();
			if (base.GetComponentInParent<GraphicRaycaster>() == null)
			{
				Console.LogWarning("Tooltip has not parent GraphicRaycaster! Tooltip won't ever be activated", null);
			}
			this.canvas = base.GetComponentInParent<Canvas>();
			if (this.canvas != null)
			{
				this.isWorldspace = (this.canvas.renderMode == 2);
			}
		}

		// Token: 0x040035B8 RID: 13752
		[Header("Settings")]
		public string text;

		// Token: 0x040035B9 RID: 13753
		public Vector2 labelOffset;

		// Token: 0x040035BA RID: 13754
		private RectTransform rect;

		// Token: 0x040035BC RID: 13756
		private Canvas canvas;
	}
}
