using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A47 RID: 2631
	public class MouseTooltip : Singleton<MouseTooltip>
	{
		// Token: 0x060046B5 RID: 18101 RVA: 0x00128A43 File Offset: 0x00126C43
		public void ShowTooltip(string text, Color col)
		{
			this.TooltipLabel.text = text;
			this.TooltipLabel.color = col;
			this.tooltipShownThisFrame = true;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x00128A64 File Offset: 0x00126C64
		public void ShowIcon(Sprite sprite, Color col)
		{
			this.IconImg.sprite = sprite;
			this.IconImg.color = col;
			this.iconShownThisFrame = true;
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x00128A88 File Offset: 0x00126C88
		private void LateUpdate()
		{
			this.TooltipLabel.gameObject.SetActive(this.tooltipShownThisFrame);
			this.IconRect.gameObject.SetActive(this.iconShownThisFrame);
			this.IconRect.position = Input.mousePosition + this.IconOffset;
			if (this.iconShownThisFrame)
			{
				this.TooltipRect.position = Input.mousePosition + this.TooltipOffset_WithIcon;
			}
			else
			{
				this.TooltipRect.position = Input.mousePosition + this.TooltipOffset_NoIcon;
			}
			this.tooltipShownThisFrame = false;
			this.iconShownThisFrame = false;
		}

		// Token: 0x04003370 RID: 13168
		[Header("References")]
		public RectTransform IconRect;

		// Token: 0x04003371 RID: 13169
		public Image IconImg;

		// Token: 0x04003372 RID: 13170
		public RectTransform TooltipRect;

		// Token: 0x04003373 RID: 13171
		public TextMeshProUGUI TooltipLabel;

		// Token: 0x04003374 RID: 13172
		[Header("Settings")]
		public Vector3 TooltipOffset_NoIcon;

		// Token: 0x04003375 RID: 13173
		public Vector3 TooltipOffset_WithIcon;

		// Token: 0x04003376 RID: 13174
		public Vector3 IconOffset;

		// Token: 0x04003377 RID: 13175
		[Header("Colors")]
		public Color Color_Invalid;

		// Token: 0x04003378 RID: 13176
		[Header("Sprites")]
		public Sprite Sprite_Cross;

		// Token: 0x04003379 RID: 13177
		private bool tooltipShownThisFrame;

		// Token: 0x0400337A RID: 13178
		private bool iconShownThisFrame;
	}
}
