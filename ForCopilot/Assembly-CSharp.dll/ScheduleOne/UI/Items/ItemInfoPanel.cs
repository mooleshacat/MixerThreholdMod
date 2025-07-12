using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC2 RID: 3010
	public class ItemInfoPanel : MonoBehaviour
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x00150F59 File Offset: 0x0014F159
		// (set) Token: 0x06004FE6 RID: 20454 RVA: 0x00150F61 File Offset: 0x0014F161
		public bool IsOpen { get; protected set; }

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x00150F6A File Offset: 0x0014F16A
		// (set) Token: 0x06004FE8 RID: 20456 RVA: 0x00150F72 File Offset: 0x0014F172
		public ItemInstance CurrentItem { get; protected set; }

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00150F7B File Offset: 0x0014F17B
		private void Awake()
		{
			this.Close();
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00150F84 File Offset: 0x0014F184
		public void Open(ItemInstance item, RectTransform rect)
		{
			if (this.IsOpen)
			{
				this.Close();
			}
			if (item == null)
			{
				Console.LogWarning("Item is null!", null);
				return;
			}
			this.CurrentItem = item;
			if (item.Definition.CustomInfoContent != null)
			{
				this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(item.Definition.CustomInfoContent, this.ContentContainer);
				this.content.Initialize(item);
			}
			else
			{
				this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(this.DefaultContentPrefab, this.ContentContainer);
				this.content.Initialize(item);
			}
			this.Container.sizeDelta = new Vector2(this.Container.sizeDelta.x, this.content.Height);
			float num = (rect.sizeDelta.y + this.Container.sizeDelta.y) / 2f + this.Offset.y;
			num *= this.Canvas.scaleFactor;
			if (rect.position.y > 200f)
			{
				this.Container.position = rect.position - new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(true);
				this.BottomArrow.SetActive(false);
			}
			else
			{
				this.Container.position = rect.position + new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(false);
				this.BottomArrow.SetActive(true);
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x00151128 File Offset: 0x0014F328
		public void Open(ItemDefinition def, RectTransform rect)
		{
			if (this.IsOpen)
			{
				this.Close();
			}
			if (def == null)
			{
				Console.LogWarning("Item is null!", null);
				return;
			}
			this.CurrentItem = null;
			this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(this.DefaultContentPrefab, this.ContentContainer);
			this.content.Initialize(def);
			float num = (rect.sizeDelta.y + this.Container.sizeDelta.y) / 2f + this.Offset.y;
			num *= this.Canvas.scaleFactor;
			if (rect.position.y > 200f)
			{
				this.Container.position = rect.position - new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(true);
				this.BottomArrow.SetActive(false);
			}
			else
			{
				this.Container.position = rect.position + new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(false);
				this.BottomArrow.SetActive(true);
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x00151267 File Offset: 0x0014F467
		public void Close()
		{
			if (this.content != null)
			{
				UnityEngine.Object.Destroy(this.content.gameObject);
			}
			this.IsOpen = false;
			this.CurrentItem = null;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x04003BE5 RID: 15333
		public const float VERTICAL_THRESHOLD = 200f;

		// Token: 0x04003BE8 RID: 15336
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003BE9 RID: 15337
		public RectTransform ContentContainer;

		// Token: 0x04003BEA RID: 15338
		public GameObject TopArrow;

		// Token: 0x04003BEB RID: 15339
		public GameObject BottomArrow;

		// Token: 0x04003BEC RID: 15340
		public Canvas Canvas;

		// Token: 0x04003BED RID: 15341
		[Header("Settings")]
		public Vector2 Offset = new Vector2(0f, 125f);

		// Token: 0x04003BEE RID: 15342
		[Header("Prefabs")]
		public ItemInfoContent DefaultContentPrefab;

		// Token: 0x04003BEF RID: 15343
		private ItemInfoContent content;
	}
}
