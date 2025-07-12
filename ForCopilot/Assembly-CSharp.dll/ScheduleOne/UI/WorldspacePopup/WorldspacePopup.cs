using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000A9D RID: 2717
	public class WorldspacePopup : MonoBehaviour
	{
		// Token: 0x060048F9 RID: 18681 RVA: 0x00131F13 File Offset: 0x00130113
		private void OnEnable()
		{
			if (!WorldspacePopup.ActivePopups.Contains(this))
			{
				WorldspacePopup.ActivePopups.Add(this);
			}
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x00131F2D File Offset: 0x0013012D
		private void OnDisable()
		{
			WorldspacePopup.ActivePopups.Remove(this);
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x00131F3C File Offset: 0x0013013C
		public WorldspacePopupUI CreateUI(RectTransform parent)
		{
			WorldspacePopupUI newUI = UnityEngine.Object.Instantiate<WorldspacePopupUI>(this.UIPrefab, parent);
			newUI.Popup = this;
			newUI.SetFill(this.CurrentFillLevel);
			this.UIs.Add(newUI);
			newUI.onDestroyed.AddListener(delegate()
			{
				this.UIs.Remove(newUI);
			});
			return newUI;
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x00131FB8 File Offset: 0x001301B8
		private void LateUpdate()
		{
			foreach (WorldspacePopupUI worldspacePopupUI in this.UIs)
			{
				worldspacePopupUI.SetFill(this.CurrentFillLevel);
			}
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x00132010 File Offset: 0x00130210
		public void Popup()
		{
			if (this.popupCoroutine != null)
			{
				base.StopCoroutine(this.popupCoroutine);
			}
			this.popupCoroutine = base.StartCoroutine(this.<Popup>g__PopupCoroutine|18_0());
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00132091 File Offset: 0x00130291
		[CompilerGenerated]
		private IEnumerator <Popup>g__PopupCoroutine|18_0()
		{
			base.enabled = true;
			this.SizeMultiplier = 0f;
			float lerpTime = 0.25f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.SizeMultiplier = i / lerpTime;
				yield return new WaitForEndOfFrame();
			}
			this.SizeMultiplier = 1f;
			yield return new WaitForSeconds(0.6f);
			base.enabled = false;
			this.popupCoroutine = null;
			yield break;
		}

		// Token: 0x04003598 RID: 13720
		public static List<WorldspacePopup> ActivePopups = new List<WorldspacePopup>();

		// Token: 0x04003599 RID: 13721
		[Range(0f, 1f)]
		public float CurrentFillLevel = 1f;

		// Token: 0x0400359A RID: 13722
		[Header("Settings")]
		public WorldspacePopupUI UIPrefab;

		// Token: 0x0400359B RID: 13723
		public bool DisplayOnHUD = true;

		// Token: 0x0400359C RID: 13724
		public bool ScaleWithDistance = true;

		// Token: 0x0400359D RID: 13725
		public Vector3 WorldspaceOffset;

		// Token: 0x0400359E RID: 13726
		public float Range = 50f;

		// Token: 0x0400359F RID: 13727
		public float SizeMultiplier = 1f;

		// Token: 0x040035A0 RID: 13728
		[HideInInspector]
		public WorldspacePopupUI WorldspaceUI;

		// Token: 0x040035A1 RID: 13729
		[HideInInspector]
		public RectTransform HUDUI;

		// Token: 0x040035A2 RID: 13730
		[HideInInspector]
		public WorldspacePopupUI HUDUIIcon;

		// Token: 0x040035A3 RID: 13731
		[HideInInspector]
		public CanvasGroup HUDUICanvasGroup;

		// Token: 0x040035A4 RID: 13732
		private List<WorldspacePopupUI> UIs = new List<WorldspacePopupUI>();

		// Token: 0x040035A5 RID: 13733
		private Coroutine popupCoroutine;
	}
}
