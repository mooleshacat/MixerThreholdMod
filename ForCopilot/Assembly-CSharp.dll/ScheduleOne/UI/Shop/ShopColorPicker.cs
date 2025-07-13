using System;
using System.Collections;
using ScheduleOne.Clothing;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BA4 RID: 2980
	public class ShopColorPicker : MonoBehaviour
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06004F24 RID: 20260 RVA: 0x0014E2E9 File Offset: 0x0014C4E9
		public bool IsOpen
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x0014E2F8 File Offset: 0x0014C4F8
		public void Start()
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ColorButtonPrefab, this.ColorButtonParent);
					gameObject.transform.Find("Color").GetComponent<Image>().color = color.GetActualColor();
					gameObject.GetComponent<Button>().onClick.AddListener(delegate()
					{
						this.ColorPicked(color);
					});
					EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = 0;
					entry.callback.AddListener(delegate(BaseEventData data)
					{
						this.ColorHovered(color);
					});
					eventTrigger.triggers.Add(entry);
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x0014E400 File Offset: 0x0014C600
		private void ColorPicked(EClothingColor color)
		{
			if (this.onColorPicked != null)
			{
				this.onColorPicked.Invoke(color);
			}
			this.Close();
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x0014E41C File Offset: 0x0014C61C
		public void Open(ItemDefinition item)
		{
			this.AssetIconImage.sprite = item.Icon;
			this.ColorHovered(EClothingColor.White);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x0014E442 File Offset: 0x0014C642
		private void ColorHovered(EClothingColor color)
		{
			this.AssetIconImage.color = color.GetActualColor();
			this.ColorLabel.text = color.GetLabel();
		}

		// Token: 0x04003B4E RID: 15182
		public Image AssetIconImage;

		// Token: 0x04003B4F RID: 15183
		public TextMeshProUGUI ColorLabel;

		// Token: 0x04003B50 RID: 15184
		public RectTransform ColorButtonParent;

		// Token: 0x04003B51 RID: 15185
		public GameObject ColorButtonPrefab;

		// Token: 0x04003B52 RID: 15186
		public UnityEvent<EClothingColor> onColorPicked = new UnityEvent<EClothingColor>();
	}
}
