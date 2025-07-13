using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BAF RID: 2991
	public class ShopInterfaceDetailPanel : MonoBehaviour
	{
		// Token: 0x06004F7A RID: 20346 RVA: 0x0014F916 File Offset: 0x0014DB16
		private void Awake()
		{
			this.Panel.gameObject.SetActive(false);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x0014F92C File Offset: 0x0014DB2C
		public void Open(ListingUI _listing)
		{
			this.listing = _listing;
			this.DescriptionLabel.text = this.listing.Listing.Item.Description;
			if (this.listing.Listing.Item.RequiresLevelToPurchase && !this.listing.Listing.Item.IsPurchasable)
			{
				this.UnlockLabel.text = "Unlocks at <color=#2DB92D>" + this.listing.Listing.Item.RequiredRank.ToString() + "</color>";
				this.UnlockLabel.gameObject.SetActive(true);
			}
			else
			{
				this.UnlockLabel.gameObject.SetActive(false);
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Open>g__Wait|6_0());
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x0014F9FD File Offset: 0x0014DBFD
		private void LateUpdate()
		{
			this.Position();
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x0014FA08 File Offset: 0x0014DC08
		private void Position()
		{
			if (this.listing == null)
			{
				return;
			}
			this.Panel.position = this.listing.DetailPanelAnchor.position;
			this.Panel.anchoredPosition = new Vector2(this.Panel.anchoredPosition.x + this.Panel.sizeDelta.x / 2f, this.Panel.anchoredPosition.y);
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x0014FA86 File Offset: 0x0014DC86
		public void Close()
		{
			this.listing = null;
			this.Panel.gameObject.SetActive(false);
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x0014FAA0 File Offset: 0x0014DCA0
		[CompilerGenerated]
		private IEnumerator <Open>g__Wait|6_0()
		{
			this.LayoutGroup.enabled = false;
			yield return new WaitForEndOfFrame();
			this.Panel.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.Panel);
			this.LayoutGroup.CalculateLayoutInputVertical();
			this.LayoutGroup.enabled = true;
			this.Position();
			yield break;
		}

		// Token: 0x04003B91 RID: 15249
		[Header("References")]
		public RectTransform Panel;

		// Token: 0x04003B92 RID: 15250
		public VerticalLayoutGroup LayoutGroup;

		// Token: 0x04003B93 RID: 15251
		public TextMeshProUGUI DescriptionLabel;

		// Token: 0x04003B94 RID: 15252
		public TextMeshProUGUI UnlockLabel;

		// Token: 0x04003B95 RID: 15253
		private ListingUI listing;
	}
}
