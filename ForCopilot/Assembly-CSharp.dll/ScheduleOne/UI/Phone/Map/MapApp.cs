using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Map
{
	// Token: 0x02000AF5 RID: 2805
	public class MapApp : App<MapApp>
	{
		// Token: 0x06004B3A RID: 19258 RVA: 0x0013BDE4 File Offset: 0x00139FE4
		protected override void Start()
		{
			base.Start();
			this.BackgroundImage.sprite = (NetworkSingleton<GameManager>.Instance.IsTutorial ? this.TutorialMapSprite : this.MainMapSprite);
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x0013BE14 File Offset: 0x0013A014
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("MapAppOpen", open.ToString(), false);
			}
			if (open)
			{
				if (!this.opened && !this.SkipFocusPlayer)
				{
					this.opened = true;
					Player.Local.PoI.UpdatePosition();
					this.FocusPosition(Player.Local.PoI.UI.anchoredPosition);
				}
				if (Player.Local != null && Player.Local.PoI.UI != null)
				{
					Player.Local.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
			}
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x0013BEC8 File Offset: 0x0013A0C8
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				GameInput.GetButton(GameInput.ButtonCode.Right);
				GameInput.GetButton(GameInput.ButtonCode.Left);
				GameInput.GetButton(GameInput.ButtonCode.Forward);
				GameInput.GetButton(GameInput.ButtonCode.Backward);
				float x = this.ContentRect.localScale.x;
				if (x >= this.LabelScrollMin)
				{
					this.LabelGroup.alpha = Mathf.Clamp01((x - this.LabelScrollMin) / (this.LabelScrollMax - this.LabelScrollMin));
					return;
				}
				this.LabelGroup.alpha = 0f;
			}
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x0013BF50 File Offset: 0x0013A150
		public void FocusPosition(Vector2 anchoredPosition)
		{
			this.ContentRect.pivot = new Vector2(0f, 1f);
			float num = 1.3f;
			Vector2 a = new Vector2(-this.ContentRect.sizeDelta.x / 2f, this.ContentRect.sizeDelta.y / 2f);
			a.x -= anchoredPosition.x;
			a.y -= anchoredPosition.y;
			this.ContentRect.localScale = new Vector3(num, num, num);
			this.ContentRect.anchoredPosition = a * num;
		}

		// Token: 0x04003790 RID: 14224
		public const float KeyMoveSpeed = 1.25f;

		// Token: 0x04003791 RID: 14225
		public RectTransform ContentRect;

		// Token: 0x04003792 RID: 14226
		public RectTransform PoIContainer;

		// Token: 0x04003793 RID: 14227
		public Scrollbar HorizontalScrollbar;

		// Token: 0x04003794 RID: 14228
		public Scrollbar VerticalScrollbar;

		// Token: 0x04003795 RID: 14229
		public Image BackgroundImage;

		// Token: 0x04003796 RID: 14230
		public CanvasGroup LabelGroup;

		// Token: 0x04003797 RID: 14231
		[Header("Settings")]
		public Sprite DemoMapSprite;

		// Token: 0x04003798 RID: 14232
		public Sprite MainMapSprite;

		// Token: 0x04003799 RID: 14233
		public Sprite TutorialMapSprite;

		// Token: 0x0400379A RID: 14234
		public float LabelScrollMin = 1.2f;

		// Token: 0x0400379B RID: 14235
		public float LabelScrollMax = 1.5f;

		// Token: 0x0400379C RID: 14236
		[HideInInspector]
		public bool SkipFocusPlayer;

		// Token: 0x0400379D RID: 14237
		private Coroutine contentMoveRoutine;

		// Token: 0x0400379E RID: 14238
		private bool opened;
	}
}
