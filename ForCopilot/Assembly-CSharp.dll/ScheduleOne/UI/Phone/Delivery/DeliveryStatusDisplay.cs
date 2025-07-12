using System;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AFE RID: 2814
	public class DeliveryStatusDisplay : MonoBehaviour
	{
		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004B7E RID: 19326 RVA: 0x0013D039 File Offset: 0x0013B239
		// (set) Token: 0x06004B7F RID: 19327 RVA: 0x0013D041 File Offset: 0x0013B241
		public DeliveryInstance DeliveryInstance { get; private set; }

		// Token: 0x06004B80 RID: 19328 RVA: 0x0013D04C File Offset: 0x0013B24C
		public void AssignDelivery(DeliveryInstance instance)
		{
			this.DeliveryInstance = instance;
			this.DestinationLabel.text = this.DeliveryInstance.Destination.PropertyName + " [" + (this.DeliveryInstance.LoadingDockIndex + 1).ToString() + "]";
			this.ShopLabel.text = this.DeliveryInstance.StoreName;
			foreach (StringIntPair stringIntPair in this.DeliveryInstance.Items)
			{
				Transform component = UnityEngine.Object.Instantiate<GameObject>(this.ItemEntryPrefab, this.ItemEntryContainer).GetComponent<RectTransform>();
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				component.Find("Label").GetComponent<Text>().text = stringIntPair.Int.ToString() + "x " + item.Name;
			}
			int num = Mathf.CeilToInt((float)this.DeliveryInstance.Items.Length / 2f);
			this.Rect.sizeDelta = new Vector2(this.Rect.sizeDelta.x, (float)(70 + 20 * num));
			this.RefreshStatus();
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0013D170 File Offset: 0x0013B370
		public void RefreshStatus()
		{
			if (this.DeliveryInstance.Status == EDeliveryStatus.InTransit)
			{
				this.StatusImage.color = this.StatusColor_Transit;
				int timeUntilArrival = this.DeliveryInstance.TimeUntilArrival;
				int num = timeUntilArrival / 60;
				int num2 = timeUntilArrival % 60;
				this.StatusLabel.text = num.ToString() + "h " + num2.ToString() + "m";
				this.StatusTooltip.text = "This delivery is currently in transit.";
				return;
			}
			if (this.DeliveryInstance.Status == EDeliveryStatus.Waiting)
			{
				this.StatusImage.color = this.StatusColor_Waiting;
				this.StatusLabel.text = "Waiting";
				this.StatusTooltip.text = "This delivery is waiting for the loading dock " + (this.DeliveryInstance.LoadingDockIndex + 1).ToString() + " to be empty.";
				return;
			}
			if (this.DeliveryInstance.Status == EDeliveryStatus.Arrived)
			{
				this.StatusImage.color = this.StatusColor_Arrived;
				this.StatusLabel.text = "Arrived";
				this.StatusTooltip.text = "This delivery has arrived and is ready to be unloaded.";
			}
		}

		// Token: 0x040037D1 RID: 14289
		public GameObject ItemEntryPrefab;

		// Token: 0x040037D2 RID: 14290
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x040037D3 RID: 14291
		public Text DestinationLabel;

		// Token: 0x040037D4 RID: 14292
		public Text ShopLabel;

		// Token: 0x040037D5 RID: 14293
		public Image StatusImage;

		// Token: 0x040037D6 RID: 14294
		public Text StatusLabel;

		// Token: 0x040037D7 RID: 14295
		public Tooltip StatusTooltip;

		// Token: 0x040037D8 RID: 14296
		public RectTransform ItemEntryContainer;

		// Token: 0x040037D9 RID: 14297
		[Header("Settings")]
		public Color StatusColor_Transit;

		// Token: 0x040037DA RID: 14298
		public Color StatusColor_Waiting;

		// Token: 0x040037DB RID: 14299
		public Color StatusColor_Arrived;
	}
}
