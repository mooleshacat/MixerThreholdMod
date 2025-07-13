using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A35 RID: 2613
	public class HUD : Singleton<HUD>
	{
		// Token: 0x06004641 RID: 17985 RVA: 0x00126B1F File Offset: 0x00124D1F
		protected override void Awake()
		{
			base.Awake();
			this.eventSystem = EventSystem.current;
			this.managementSlotContainer.gameObject.SetActive(true);
			this.HideTopScreenText();
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x00126B49 File Offset: 0x00124D49
		public void SetCrosshairVisible(bool vis)
		{
			this.crosshair.gameObject.SetActive(vis);
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x00126B5C File Offset: 0x00124D5C
		public void SetBlackOverlayVisible(bool vis, float fadeTime)
		{
			if (this.blackOverlayFade != null)
			{
				base.StopCoroutine(this.blackOverlayFade);
			}
			this.blackOverlayFade = base.StartCoroutine(this.FadeBlackOverlay(vis, fadeTime));
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00126B86 File Offset: 0x00124D86
		protected virtual void Update()
		{
			this.RefreshFPS();
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00126B90 File Offset: 0x00124D90
		private void FixedUpdate()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.SleepPrompt.gameObject.SetActive(NetworkSingleton<TimeManager>.Instance.CurrentTime == 400);
			if (NetworkSingleton<CurfewManager>.InstanceExists)
			{
				if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
				{
					this.CurfewPrompt.text = "Police curfew in effect until 5AM";
					this.CurfewPrompt.color = new Color32(byte.MaxValue, 108, 88, 60);
					this.CurfewPrompt.gameObject.SetActive(true);
				}
				else if (NetworkSingleton<CurfewManager>.Instance.IsEnabled && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(2030, 500))
				{
					this.CurfewPrompt.text = "Police curfew starting soon";
					this.CurfewPrompt.color = new Color32(byte.MaxValue, 182, 88, 60);
					this.CurfewPrompt.gameObject.SetActive(true);
				}
				else
				{
					this.CurfewPrompt.gameObject.SetActive(false);
				}
			}
			this.UpdateQuestEntryTitle();
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x00126CA0 File Offset: 0x00124EA0
		private void UpdateQuestEntryTitle()
		{
			int num = 0;
			for (int i = 0; i < this.QuestEntryContainer.childCount; i++)
			{
				if (this.QuestEntryContainer.GetChild(i).gameObject.activeSelf)
				{
					num++;
				}
			}
			this.QuestEntryTitle.enabled = (num > 1);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x00126CF0 File Offset: 0x00124EF0
		private void RefreshFPS()
		{
			this._previousFPS.Add(1f / Time.unscaledDeltaTime);
			if (this._previousFPS.Count > this.SampleSize)
			{
				this._previousFPS.RemoveAt(0);
			}
			this.fpsLabel.text = Mathf.Floor(this.GetAverageFPS()).ToString() + " FPS";
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x00126D5C File Offset: 0x00124F5C
		private float GetAverageFPS()
		{
			float num = 0f;
			for (int i = 0; i < this._previousFPS.Count; i++)
			{
				num += this._previousFPS[i];
			}
			return num / (float)this._previousFPS.Count;
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00126DA2 File Offset: 0x00124FA2
		protected virtual void LateUpdate()
		{
			if (!this.radialIndicatorSetThisFrame)
			{
				this.radialIndicator.enabled = false;
			}
			this.radialIndicatorSetThisFrame = false;
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00126DBF File Offset: 0x00124FBF
		protected IEnumerator FadeBlackOverlay(bool visible, float fadeTime)
		{
			if (visible)
			{
				this.blackOverlay.enabled = true;
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Blackout");
			}
			float startAlpha = this.blackOverlay.color.a;
			float endAlpha = 1f;
			if (!visible)
			{
				endAlpha = 0f;
			}
			for (float i = 0f; i < fadeTime; i += Time.unscaledDeltaTime)
			{
				this.blackOverlay.color = new Color(this.blackOverlay.color.r, this.blackOverlay.color.g, this.blackOverlay.color.b, Mathf.Lerp(startAlpha, endAlpha, i / fadeTime));
				yield return new WaitForEndOfFrame();
			}
			this.blackOverlay.color = new Color(this.blackOverlay.color.r, this.blackOverlay.color.g, this.blackOverlay.color.b, endAlpha);
			this.blackOverlayFade = null;
			if (!visible)
			{
				this.blackOverlay.enabled = false;
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Blackout");
			}
			yield break;
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x00126DDC File Offset: 0x00124FDC
		public void ShowRadialIndicator(float fill)
		{
			this.radialIndicatorSetThisFrame = true;
			this.radialIndicator.fillAmount = fill;
			this.radialIndicator.enabled = true;
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x00126E00 File Offset: 0x00125000
		public void ShowTopScreenText(string t)
		{
			this.topScreenText.text = t;
			this.topScreenText_Background.sizeDelta = new Vector2(this.topScreenText.preferredWidth + 30f, this.topScreenText_Background.sizeDelta.y);
			this.topScreenText_Background.gameObject.SetActive(true);
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x00126E5B File Offset: 0x0012505B
		public void HideTopScreenText()
		{
			this.topScreenText_Background.gameObject.SetActive(false);
		}

		// Token: 0x040032ED RID: 13037
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040032EE RID: 13038
		public RectTransform canvasRect;

		// Token: 0x040032EF RID: 13039
		public Image crosshair;

		// Token: 0x040032F0 RID: 13040
		[SerializeField]
		protected Image blackOverlay;

		// Token: 0x040032F1 RID: 13041
		[SerializeField]
		protected Image radialIndicator;

		// Token: 0x040032F2 RID: 13042
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x040032F3 RID: 13043
		[SerializeField]
		protected TextMeshProUGUI topScreenText;

		// Token: 0x040032F4 RID: 13044
		[SerializeField]
		protected RectTransform topScreenText_Background;

		// Token: 0x040032F5 RID: 13045
		public Text fpsLabel;

		// Token: 0x040032F6 RID: 13046
		public RectTransform cashSlotContainer;

		// Token: 0x040032F7 RID: 13047
		public RectTransform cashSlotUI;

		// Token: 0x040032F8 RID: 13048
		public RectTransform onlineBalanceContainer;

		// Token: 0x040032F9 RID: 13049
		public RectTransform onlineBalanceSlotUI;

		// Token: 0x040032FA RID: 13050
		public RectTransform managementSlotContainer;

		// Token: 0x040032FB RID: 13051
		public ItemSlotUI managementSlotUI;

		// Token: 0x040032FC RID: 13052
		public RectTransform HotbarContainer;

		// Token: 0x040032FD RID: 13053
		public RectTransform SlotContainer;

		// Token: 0x040032FE RID: 13054
		public ItemSlotUI discardSlot;

		// Token: 0x040032FF RID: 13055
		public Image discardSlotFill;

		// Token: 0x04003300 RID: 13056
		public TextMeshProUGUI selectedItemLabel;

		// Token: 0x04003301 RID: 13057
		public RectTransform QuestEntryContainer;

		// Token: 0x04003302 RID: 13058
		public TextMeshProUGUI QuestEntryTitle;

		// Token: 0x04003303 RID: 13059
		public CrimeStatusUI CrimeStatusUI;

		// Token: 0x04003304 RID: 13060
		public BalanceDisplay OnlineBalanceDisplay;

		// Token: 0x04003305 RID: 13061
		public BalanceDisplay SafeBalanceDisplay;

		// Token: 0x04003306 RID: 13062
		public CrosshairText CrosshairText;

		// Token: 0x04003307 RID: 13063
		public RectTransform UnreadMessagesPrompt;

		// Token: 0x04003308 RID: 13064
		public TextMeshProUGUI SleepPrompt;

		// Token: 0x04003309 RID: 13065
		public TextMeshProUGUI CurfewPrompt;

		// Token: 0x0400330A RID: 13066
		[Header("Settings")]
		public Gradient RedGreenGradient;

		// Token: 0x0400330B RID: 13067
		private int SampleSize = 60;

		// Token: 0x0400330C RID: 13068
		private List<float> _previousFPS = new List<float>();

		// Token: 0x0400330D RID: 13069
		private EventSystem eventSystem;

		// Token: 0x0400330E RID: 13070
		private Coroutine blackOverlayFade;

		// Token: 0x0400330F RID: 13071
		private bool radialIndicatorSetThisFrame;
	}
}
