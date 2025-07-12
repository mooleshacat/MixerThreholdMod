using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B05 RID: 2821
	public class MessageBubble : MonoBehaviour
	{
		// Token: 0x06004BB2 RID: 19378 RVA: 0x0013DF1C File Offset: 0x0013C11C
		public void SetupBubble(string _text, MessageBubble.Alignment _alignment, bool alignCenter = false)
		{
			this.alignment = _alignment;
			this.text = _text;
			this.alignTextCenter = alignCenter;
			ColorBlock colors = this.button.colors;
			if (this.alignment == MessageBubble.Alignment.Left)
			{
				this.container.anchorMin = new Vector2(0f, 1f);
				this.container.anchorMax = new Vector2(0f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Left;
				colors.disabledColor = MessageBubble.backgroundColor_Left;
				this.content.color = MessageBubble.textColor_Left;
			}
			else if (this.alignment == MessageBubble.Alignment.Right)
			{
				this.container.anchorMin = new Vector2(1f, 1f);
				this.container.anchorMax = new Vector2(1f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Right;
				colors.disabledColor = MessageBubble.backgroundColor_Right;
				this.content.color = MessageBubble.textColor_Right;
			}
			else
			{
				this.container.anchorMin = new Vector2(0.5f, 1f);
				this.container.anchorMax = new Vector2(0.5f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Right;
				colors.disabledColor = MessageBubble.backgroundColor_Right;
				this.content.color = MessageBubble.textColor_Right;
			}
			this.button.colors = colors;
			this.RefreshDisplayedText();
			this.RefreshTriangle();
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x0013E0BC File Offset: 0x0013C2BC
		protected virtual void Update()
		{
			if (this.text != this.displayedText)
			{
				this.RefreshDisplayedText();
			}
			if (this.showTriangle != this.triangleShown)
			{
				this.RefreshTriangle();
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0013E0EC File Offset: 0x0013C2EC
		public virtual void RefreshDisplayedText()
		{
			this.displayedText = this.text;
			this.content.text = this.displayedText;
			if (this.alignTextCenter)
			{
				this.content.alignment = 1;
			}
			else
			{
				this.content.alignment = 0;
			}
			RectTransform component = base.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(Mathf.Clamp(this.content.preferredWidth + 50f, this.bubble_MinWidth, this.bubble_MaxWidth), 75f);
			this.height = Mathf.Clamp(this.content.preferredHeight + 25f, 75f, float.MaxValue);
			component.sizeDelta = new Vector2(component.sizeDelta.x, this.height);
			float num = 1f;
			if (this.alignment == MessageBubble.Alignment.Right)
			{
				num = -1f;
			}
			else if (this.alignment == MessageBubble.Alignment.Center)
			{
				num = 0f;
			}
			if (this.autosetPosition)
			{
				component.anchoredPosition = new Vector2((component.sizeDelta.x / 2f + 25f) * num, -this.height / 2f);
			}
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x0013E214 File Offset: 0x0013C414
		protected virtual void RefreshTriangle()
		{
			this.triangleShown = this.showTriangle;
			this.triangle_Left.gameObject.SetActive(false);
			this.triangle_Right.gameObject.SetActive(false);
			if (this.showTriangle)
			{
				this.triangle_Left.color = this.button.colors.normalColor;
				this.triangle_Right.color = this.button.colors.normalColor;
				if (this.alignment == MessageBubble.Alignment.Left)
				{
					this.triangle_Left.gameObject.SetActive(true);
					return;
				}
				this.triangle_Right.gameObject.SetActive(true);
			}
		}

		// Token: 0x0400380B RID: 14347
		[Header("Settings")]
		public string text = string.Empty;

		// Token: 0x0400380C RID: 14348
		public MessageBubble.Alignment alignment = MessageBubble.Alignment.Left;

		// Token: 0x0400380D RID: 14349
		public bool showTriangle;

		// Token: 0x0400380E RID: 14350
		public float bubble_MinWidth = 75f;

		// Token: 0x0400380F RID: 14351
		public float bubble_MaxWidth = 500f;

		// Token: 0x04003810 RID: 14352
		public bool alignTextCenter;

		// Token: 0x04003811 RID: 14353
		public bool autosetPosition = true;

		// Token: 0x04003812 RID: 14354
		private string displayedText = string.Empty;

		// Token: 0x04003813 RID: 14355
		private bool triangleShown;

		// Token: 0x04003814 RID: 14356
		[Header("References")]
		public RectTransform container;

		// Token: 0x04003815 RID: 14357
		[SerializeField]
		protected Image bubble;

		// Token: 0x04003816 RID: 14358
		[SerializeField]
		protected Text content;

		// Token: 0x04003817 RID: 14359
		[SerializeField]
		protected Image triangle_Left;

		// Token: 0x04003818 RID: 14360
		[SerializeField]
		protected Image triangle_Right;

		// Token: 0x04003819 RID: 14361
		public Button button;

		// Token: 0x0400381A RID: 14362
		public float height;

		// Token: 0x0400381B RID: 14363
		public float spacingAbove;

		// Token: 0x0400381C RID: 14364
		public static Color32 backgroundColor_Left = new Color32(225, 225, 225, byte.MaxValue);

		// Token: 0x0400381D RID: 14365
		public static Color32 textColor_Left = new Color32(50, 50, 50, byte.MaxValue);

		// Token: 0x0400381E RID: 14366
		public static Color32 backgroundColor_Right = new Color32(75, 175, 225, byte.MaxValue);

		// Token: 0x0400381F RID: 14367
		public static Color32 textColor_Right = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04003820 RID: 14368
		public static float baseBubbleSpacing = 5f;

		// Token: 0x02000B06 RID: 2822
		public enum Alignment
		{
			// Token: 0x04003822 RID: 14370
			Center,
			// Token: 0x04003823 RID: 14371
			Left,
			// Token: 0x04003824 RID: 14372
			Right
		}
	}
}
