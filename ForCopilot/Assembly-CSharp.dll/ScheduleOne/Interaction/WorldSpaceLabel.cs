using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Interaction
{
	// Token: 0x02000654 RID: 1620
	public class WorldSpaceLabel
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x000AE110 File Offset: 0x000AC310
		public WorldSpaceLabel(string _text, Vector3 _position)
		{
			this.text = _text;
			this.position = _position;
			this.rect = UnityEngine.Object.Instantiate<GameObject>(Singleton<InteractionManager>.Instance.WSLabelPrefab, Singleton<InteractionManager>.Instance.wsLabelContainer).GetComponent<RectTransform>();
			this.textComp = this.rect.GetComponent<Text>();
			Singleton<InteractionManager>.Instance.activeWSlabels.Add(this);
			this.RefreshDisplay();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000AE1B4 File Offset: 0x000AC3B4
		public void RefreshDisplay()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(this.position).z < -3f || !this.active)
			{
				this.rect.gameObject.SetActive(false);
				return;
			}
			this.textComp.text = this.text;
			this.textComp.color = this.color;
			this.rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.position);
			float num = Mathf.Clamp(1f / Vector3.Distance(this.position, PlayerSingleton<PlayerCamera>.Instance.transform.position), 0f, 1f) * Singleton<InteractionManager>.Instance.displaySizeMultiplier * this.scale;
			this.rect.localScale = new Vector3(num, num, 1f);
			this.rect.gameObject.SetActive(true);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000AE2AD File Offset: 0x000AC4AD
		public void Destroy()
		{
			Singleton<InteractionManager>.Instance.activeWSlabels.Remove(this);
			this.rect.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(this.rect.gameObject);
		}

		// Token: 0x04001EA6 RID: 7846
		public string text = string.Empty;

		// Token: 0x04001EA7 RID: 7847
		public Color32 color = Color.white;

		// Token: 0x04001EA8 RID: 7848
		public Vector3 position = Vector3.zero;

		// Token: 0x04001EA9 RID: 7849
		public float scale = 1f;

		// Token: 0x04001EAA RID: 7850
		public RectTransform rect;

		// Token: 0x04001EAB RID: 7851
		public Text textComp;

		// Token: 0x04001EAC RID: 7852
		public bool active = true;
	}
}
