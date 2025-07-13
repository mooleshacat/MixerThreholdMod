using System;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000BD2 RID: 3026
	public class FeatureIcon : MonoBehaviour
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06005051 RID: 20561 RVA: 0x00153CF4 File Offset: 0x00151EF4
		// (set) Token: 0x06005052 RID: 20562 RVA: 0x00153CFC File Offset: 0x00151EFC
		public Feature feature { get; protected set; }

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06005053 RID: 20563 RVA: 0x00153D05 File Offset: 0x00151F05
		// (set) Token: 0x06005054 RID: 20564 RVA: 0x00153D0D File Offset: 0x00151F0D
		public bool isSelected { get; protected set; }

		// Token: 0x06005055 RID: 20565 RVA: 0x00153D18 File Offset: 0x00151F18
		public void AssignFeature(Feature _feature)
		{
			this.feature = _feature;
			this.icon.sprite = this.feature.featureIcon;
			this.text.text = this.feature.featureName;
			this.text.gameObject.SetActive(false);
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x00153D6C File Offset: 0x00151F6C
		public void UpdateTransform()
		{
			Vector3 position = this.feature.featureIconLocation.position;
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(position).z < 0f)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.rectTransform.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(position);
			float num = 0.3f;
			float num2 = 1f;
			float num3 = 3f;
			float num4 = 30f;
			float num5 = Vector3.Distance(position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			float num6 = 1f - Mathf.Clamp((num5 - num3) / (num4 - num3), 0f, 1f);
			float num7 = num + (num2 - num) * num6;
			this.rectTransform.localScale = new Vector3(num7, num7, num7);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x00153E4A File Offset: 0x0015204A
		public void Clicked()
		{
			this.SetIsSelected(!this.isSelected);
			if (this.isSelected)
			{
				Singleton<FeaturesManager>.Instance.OpenFeatureMenu(this.feature);
				return;
			}
			Singleton<FeaturesManager>.Instance.CloseFeatureMenu();
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x00153E80 File Offset: 0x00152080
		public void SetIsSelected(bool s)
		{
			this.isSelected = s;
			if (this.isSelected)
			{
				if (FeatureIcon.selectedFeatureIcon != null && FeatureIcon.selectedFeatureIcon != this)
				{
					FeatureIcon.selectedFeatureIcon.SetIsSelected(false);
				}
				FeatureIcon.selectedFeatureIcon = this;
			}
			else if (FeatureIcon.selectedFeatureIcon == this)
			{
				FeatureIcon.selectedFeatureIcon = null;
			}
			if (!this.hovered)
			{
				this.text.gameObject.SetActive(false);
			}
			this.UpdateColors();
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x00153EFC File Offset: 0x001520FC
		private void UpdateColors()
		{
			if (this.isSelected)
			{
				this.background.color = new Color32(byte.MaxValue, 156, 37, byte.MaxValue);
				this.icon.color = Color.white;
				return;
			}
			this.background.color = Color.white;
			this.icon.color = new Color32(byte.MaxValue, 156, 37, byte.MaxValue);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x00153F7E File Offset: 0x0015217E
		public void PointerEnter()
		{
			this.hovered = true;
			this.text.gameObject.SetActive(true);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x00153F98 File Offset: 0x00152198
		public void PointerExit()
		{
			this.hovered = false;
			if (!this.isSelected)
			{
				this.text.gameObject.SetActive(false);
			}
		}

		// Token: 0x04003C4B RID: 15435
		public static FeatureIcon selectedFeatureIcon;

		// Token: 0x04003C4C RID: 15436
		[Header("References")]
		public RectTransform rectTransform;

		// Token: 0x04003C4D RID: 15437
		public Image icon;

		// Token: 0x04003C4E RID: 15438
		public TextMeshProUGUI text;

		// Token: 0x04003C4F RID: 15439
		public Image background;

		// Token: 0x04003C52 RID: 15442
		private bool hovered;
	}
}
