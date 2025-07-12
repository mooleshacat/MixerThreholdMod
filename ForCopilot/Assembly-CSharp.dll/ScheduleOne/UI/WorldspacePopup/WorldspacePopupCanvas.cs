using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000AA0 RID: 2720
	public class WorldspacePopupCanvas : MonoBehaviour
	{
		// Token: 0x06004909 RID: 18697 RVA: 0x001321BC File Offset: 0x001303BC
		private void Update()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			List<WorldspacePopup> list = new List<WorldspacePopup>();
			List<WorldspacePopup> list2 = new List<WorldspacePopup>();
			for (int i = 0; i < WorldspacePopup.ActivePopups.Count; i++)
			{
				if (!this.popupsWithUI.Contains(WorldspacePopup.ActivePopups[i]) && this.ShouldCreateUI(WorldspacePopup.ActivePopups[i]))
				{
					list.Add(WorldspacePopup.ActivePopups[i]);
				}
			}
			for (int j = 0; j < this.popupsWithUI.Count; j++)
			{
				if (!WorldspacePopup.ActivePopups.Contains(this.popupsWithUI[j]) || !this.ShouldCreateUI(this.popupsWithUI[j]))
				{
					list2.Add(this.popupsWithUI[j]);
				}
			}
			foreach (WorldspacePopup worldspacePopup in list)
			{
				this.CreateWorldspaceIcon(worldspacePopup);
				if (worldspacePopup.DisplayOnHUD)
				{
					this.CreateHUDIcon(worldspacePopup);
				}
			}
			foreach (WorldspacePopup worldspacePopup2 in list2)
			{
				this.DestroyWorldspaceIcon(worldspacePopup2);
				if (worldspacePopup2.DisplayOnHUD)
				{
					this.DestroyHUDIcon(worldspacePopup2);
				}
			}
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x0013232C File Offset: 0x0013052C
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			for (int i = 0; i < this.popupsWithUI.Count; i++)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(this.popupsWithUI[i].transform.position).z > 0f)
				{
					Vector3 vector = this.popupsWithUI[i].transform.position + this.popupsWithUI[i].WorldspaceOffset;
					Vector2 v = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(vector);
					float num = 1f;
					if (this.popupsWithUI[i].ScaleWithDistance)
					{
						float f = Vector3.Distance(vector, PlayerSingleton<PlayerCamera>.Instance.transform.position);
						num = 1f / Mathf.Sqrt(f);
					}
					num *= this.popupsWithUI[i].SizeMultiplier;
					num *= 0.4f;
					this.popupsWithUI[i].WorldspaceUI.Rect.position = v;
					this.popupsWithUI[i].WorldspaceUI.Rect.localScale = new Vector3(num, num, 1f);
					this.popupsWithUI[i].WorldspaceUI.gameObject.SetActive(true);
				}
				else
				{
					this.popupsWithUI[i].WorldspaceUI.gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.popupsWithUI.Count; j++)
			{
				if (this.popupsWithUI[j].HUDUI != null)
				{
					float num2 = Vector3.SignedAngle(Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.transform.forward, Vector3.up), (this.popupsWithUI[j].transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized, Vector3.up);
					this.popupsWithUI[j].HUDUI.localRotation = Quaternion.Euler(0f, 0f, 0f - num2);
					this.popupsWithUI[j].HUDUIIcon.transform.up = Vector3.up;
					float num3 = 1f;
					float num4 = Mathf.Abs(num2);
					float num5 = 15f;
					if (num4 < 45f)
					{
						num3 = (num4 - num5) / (45f - num5);
						num3 = Mathf.Clamp01(num3);
					}
					this.popupsWithUI[j].HUDUICanvasGroup.alpha = Mathf.MoveTowards(this.popupsWithUI[j].HUDUICanvasGroup.alpha, num3, Time.deltaTime * 3f);
				}
			}
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x00132610 File Offset: 0x00130810
		private bool ShouldCreateUI(WorldspacePopup popup)
		{
			return Vector3.Distance(popup.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) <= popup.Range;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x0013263C File Offset: 0x0013083C
		private WorldspacePopupUI CreateWorldspaceIcon(WorldspacePopup popup)
		{
			WorldspacePopupUI worldspacePopupUI = popup.CreateUI(this.WorldspaceContainer);
			this.activeWorldspaceUIs.Add(worldspacePopupUI);
			this.popupsWithUI.Add(popup);
			popup.WorldspaceUI = worldspacePopupUI;
			return worldspacePopupUI;
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00132678 File Offset: 0x00130878
		private RectTransform CreateHUDIcon(WorldspacePopup popup)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.HudIconContainerPrefab, this.HudContainer).GetComponent<RectTransform>();
			WorldspacePopupUI huduiicon = popup.CreateUI(component.Find("Container").GetComponent<RectTransform>());
			popup.HUDUI = component;
			popup.HUDUIIcon = huduiicon;
			popup.HUDUICanvasGroup = component.GetComponent<CanvasGroup>();
			popup.HUDUICanvasGroup.alpha = 0f;
			this.activeHUDUIs.Add(component);
			return component;
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x001326EC File Offset: 0x001308EC
		private void DestroyWorldspaceIcon(WorldspacePopup popup)
		{
			for (int i = 0; i < this.activeWorldspaceUIs.Count; i++)
			{
				if (this.activeWorldspaceUIs[i].Popup == popup)
				{
					this.activeWorldspaceUIs[i].Destroy();
					this.activeWorldspaceUIs.RemoveAt(i);
					this.popupsWithUI.Remove(popup);
					return;
				}
			}
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x00132754 File Offset: 0x00130954
		private void DestroyHUDIcon(WorldspacePopup popup)
		{
			for (int i = 0; i < this.activeHUDUIs.Count; i++)
			{
				if (this.activeHUDUIs[i].GetComponentInChildren<WorldspacePopupUI>().Popup == popup)
				{
					this.activeHUDUIs[i].GetComponentInChildren<WorldspacePopupUI>().Destroy();
					UnityEngine.Object.Destroy(this.activeHUDUIs[i].gameObject);
					this.activeHUDUIs.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040035AD RID: 13741
		public const float WORLDSPACE_ICON_SCALE_MULTIPLIER = 0.4f;

		// Token: 0x040035AE RID: 13742
		[Header("References")]
		public RectTransform WorldspaceContainer;

		// Token: 0x040035AF RID: 13743
		public RectTransform HudContainer;

		// Token: 0x040035B0 RID: 13744
		[Header("Prefabs")]
		public GameObject HudIconContainerPrefab;

		// Token: 0x040035B1 RID: 13745
		private List<WorldspacePopupUI> activeWorldspaceUIs = new List<WorldspacePopupUI>();

		// Token: 0x040035B2 RID: 13746
		private List<RectTransform> activeHUDUIs = new List<RectTransform>();

		// Token: 0x040035B3 RID: 13747
		private List<WorldspacePopup> popupsWithUI = new List<WorldspacePopup>();
	}
}
