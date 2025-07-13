using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Tooltips
{
	// Token: 0x02000AA3 RID: 2723
	public class TooltipManager : Singleton<TooltipManager>
	{
		// Token: 0x06004919 RID: 18713 RVA: 0x00132900 File Offset: 0x00130B00
		protected override void Awake()
		{
			base.Awake();
			this.eventSystem = EventSystem.current;
			this.sortedCanvases = (from canvas in this.canvases
			where canvas.GetComponent<GraphicRaycaster>() != null
			orderby canvas.sortingOrder, canvas.transform.GetSiblingIndex()
			select canvas).ToList<Canvas>();
			for (int i = 0; i < this.sortedCanvases.Count; i++)
			{
				this.raycasters.Add(this.sortedCanvases[i].GetComponent<GraphicRaycaster>());
			}
			this.pointerEventData = new PointerEventData(this.eventSystem);
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x001329DE File Offset: 0x00130BDE
		protected virtual void Update()
		{
			this.CheckForTooltipHover();
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x001329E6 File Offset: 0x00130BE6
		protected virtual void LateUpdate()
		{
			if (!this.tooltipShownThisFrame)
			{
				this.anchor.gameObject.SetActive(false);
			}
			this.tooltipShownThisFrame = false;
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x00132A08 File Offset: 0x00130C08
		public void AddCanvas(Canvas canvas)
		{
			this.canvases.Add(canvas);
			this.sortedCanvases = (from c in this.canvases
			where c != null && c.GetComponent<GraphicRaycaster>() != null
			orderby c.sortingOrder, c.transform.GetSiblingIndex()
			select c).ToList<Canvas>();
			this.raycasters.Clear();
			for (int i = 0; i < this.sortedCanvases.Count; i++)
			{
				this.raycasters.Add(this.sortedCanvases[i].GetComponent<GraphicRaycaster>());
			}
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x00132ADC File Offset: 0x00130CDC
		private void CheckForTooltipHover()
		{
			this.pointerEventData.position = Input.mousePosition;
			for (int i = 0; i < this.sortedCanvases.Count; i++)
			{
				if (!(this.sortedCanvases[i] == null) && this.sortedCanvases[i].enabled && this.sortedCanvases[i].gameObject.activeSelf)
				{
					this.rayResults = new List<RaycastResult>();
					this.raycasters[i].Raycast(this.pointerEventData, this.rayResults);
					if (this.rayResults.Count > 0)
					{
						Tooltip componentInParent = this.rayResults[0].gameObject.GetComponentInParent<Tooltip>();
						if (componentInParent != null && componentInParent.enabled)
						{
							this.ShowTooltip(componentInParent.text, componentInParent.labelPosition, componentInParent.isWorldspace);
						}
						return;
					}
				}
			}
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x00132BE0 File Offset: 0x00130DE0
		public void ShowTooltip(string text, Vector2 position, bool worldspace)
		{
			if (text == string.Empty || string.IsNullOrWhiteSpace(text))
			{
				Console.LogWarning("ShowTooltip: text is empty", null);
				return;
			}
			this.tooltipShownThisFrame = true;
			string text2 = this.tooltipLabel.text;
			this.tooltipLabel.text = text;
			if (text2 != text)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.anchor);
				this.tooltipLabel.ForceMeshUpdate(true, true);
			}
			this.anchor.sizeDelta = new Vector2(this.tooltipLabel.renderedWidth + 4f, this.tooltipLabel.renderedHeight + 1f);
			this.anchor.position = position + new Vector2(this.anchor.sizeDelta.x / 2f, -this.anchor.sizeDelta.y / 2f) * this.Canvas.scaleFactor;
			Vector2 anchoredPosition = this.anchor.anchoredPosition;
			float min = Singleton<HUD>.Instance.canvasRect.sizeDelta.x * -0.5f - this.anchor.sizeDelta.x * this.anchor.pivot.x * -1f;
			float max = Singleton<HUD>.Instance.canvasRect.sizeDelta.x * 0.5f - this.anchor.sizeDelta.x * (1f - this.anchor.pivot.x);
			float min2 = Singleton<HUD>.Instance.canvasRect.sizeDelta.y * -0.5f - this.anchor.sizeDelta.y * this.anchor.pivot.y * -1f;
			float max2 = Singleton<HUD>.Instance.canvasRect.sizeDelta.y * 0.5f - this.anchor.sizeDelta.y * (1f - this.anchor.pivot.y);
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, min, max);
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, min2, max2);
			this.anchor.anchoredPosition = anchoredPosition;
			this.anchor.gameObject.SetActive(true);
		}

		// Token: 0x040035BD RID: 13757
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040035BE RID: 13758
		[SerializeField]
		private RectTransform anchor;

		// Token: 0x040035BF RID: 13759
		[SerializeField]
		private TextMeshProUGUI tooltipLabel;

		// Token: 0x040035C0 RID: 13760
		[Header("Canvas")]
		public List<Canvas> canvases = new List<Canvas>();

		// Token: 0x040035C1 RID: 13761
		private List<Canvas> sortedCanvases = new List<Canvas>();

		// Token: 0x040035C2 RID: 13762
		private List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

		// Token: 0x040035C3 RID: 13763
		private EventSystem eventSystem;

		// Token: 0x040035C4 RID: 13764
		private bool tooltipShownThisFrame;

		// Token: 0x040035C5 RID: 13765
		private PointerEventData pointerEventData;

		// Token: 0x040035C6 RID: 13766
		private List<RaycastResult> rayResults = new List<RaycastResult>();
	}
}
