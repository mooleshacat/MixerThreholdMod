using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A06 RID: 2566
	[RequireComponent(typeof(CanvasScaler))]
	public class CanvasScaler : MonoBehaviour
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060044FF RID: 17663 RVA: 0x0012195A File Offset: 0x0011FB5A
		public static float NormalizedCanvasScaleFactor
		{
			get
			{
				return Mathf.InverseLerp(0.7f, 1.4f, CanvasScaler.CanvasScaleFactor);
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00121970 File Offset: 0x0011FB70
		public void Awake()
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
			this.referenceResolution = this.canvasScaler.referenceResolution;
			CanvasScaler.OnCanvasScaleFactorChanged = (Action)Delegate.Combine(CanvasScaler.OnCanvasScaleFactorChanged, new Action(this.RefreshScale));
			this.RefreshScale();
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x001219C0 File Offset: 0x0011FBC0
		private void OnDestroy()
		{
			CanvasScaler.OnCanvasScaleFactorChanged = (Action)Delegate.Remove(CanvasScaler.OnCanvasScaleFactorChanged, new Action(this.RefreshScale));
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x001219E2 File Offset: 0x0011FBE2
		private void RefreshScale()
		{
			this.canvasScaler.referenceResolution = this.referenceResolution / CanvasScaler.CanvasScaleFactor / this.ScaleMultiplier;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x00121A0A File Offset: 0x0011FC0A
		public static void SetScaleFactor(float scaleFactor)
		{
			scaleFactor = Mathf.Clamp(scaleFactor, 0.7f, 1.4f);
			CanvasScaler.CanvasScaleFactor = scaleFactor;
			Action onCanvasScaleFactorChanged = CanvasScaler.OnCanvasScaleFactorChanged;
			if (onCanvasScaleFactorChanged == null)
			{
				return;
			}
			onCanvasScaleFactorChanged();
		}

		// Token: 0x040031D9 RID: 12761
		public static float CanvasScaleFactor = 1f;

		// Token: 0x040031DA RID: 12762
		public static Action OnCanvasScaleFactorChanged;

		// Token: 0x040031DB RID: 12763
		public float ScaleMultiplier = 1f;

		// Token: 0x040031DC RID: 12764
		private Vector2 referenceResolution = new Vector2(1920f, 1080f);

		// Token: 0x040031DD RID: 12765
		private CanvasScaler canvasScaler;
	}
}
