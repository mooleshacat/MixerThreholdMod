using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AF6 RID: 2806
	public class DeliveryApp : App<DeliveryApp>
	{
		// Token: 0x06004B3F RID: 19263 RVA: 0x0013C016 File Offset: 0x0013A216
		protected override void Awake()
		{
			base.Awake();
			this.deliveryShops = base.GetComponentsInChildren<DeliveryShop>(true).ToList<DeliveryShop>();
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x0013C030 File Offset: 0x0013A230
		protected override void Start()
		{
			base.Start();
			if (!this.started)
			{
				this.started = true;
				NetworkSingleton<DeliveryManager>.Instance.onDeliveryCreated.AddListener(new UnityAction<DeliveryInstance>(this.CreateDeliveryStatusDisplay));
				NetworkSingleton<DeliveryManager>.Instance.onDeliveryCompleted.AddListener(new UnityAction<DeliveryInstance>(this.DeliveryCompleted));
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
				for (int i = 0; i < NetworkSingleton<DeliveryManager>.Instance.Deliveries.Count; i++)
				{
					this.CreateDeliveryStatusDisplay(NetworkSingleton<DeliveryManager>.Instance.Deliveries[i]);
				}
			}
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x0013C0E1 File Offset: 0x0013A2E1
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x0013C0EC File Offset: 0x0013A2EC
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (open)
			{
				foreach (DeliveryShop deliveryShop in this.deliveryShops)
				{
					deliveryShop.RefreshShop();
				}
				foreach (DeliveryStatusDisplay deliveryStatusDisplay in this.statusDisplays)
				{
					deliveryStatusDisplay.RefreshStatus();
				}
				if (this.MainScrollRect.verticalNormalizedPosition > 1f)
				{
					this.MainScrollRect.verticalNormalizedPosition = 1f;
				}
				this.OrderSubmittedAnim.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x0013C1C0 File Offset: 0x0013A3C0
		private void OnMinPass()
		{
			if (!base.isOpen)
			{
				return;
			}
			foreach (DeliveryStatusDisplay deliveryStatusDisplay in this.statusDisplays)
			{
				deliveryStatusDisplay.RefreshStatus();
			}
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x0013C21C File Offset: 0x0013A41C
		public void RefreshContent(bool keepScrollPosition = true)
		{
			DeliveryApp.<>c__DisplayClass15_0 CS$<>8__locals1 = new DeliveryApp.<>c__DisplayClass15_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.keepScrollPosition = keepScrollPosition;
			CS$<>8__locals1.scrollPos = this.MainScrollRect.verticalNormalizedPosition;
			base.StartCoroutine(CS$<>8__locals1.<RefreshContent>g__Delay|0());
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0013C25B File Offset: 0x0013A45B
		public void PlayOrderSubmittedAnim()
		{
			this.OrderSubmittedAnim.Play();
			this.OrderSubmittedSound.Play();
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x0013C274 File Offset: 0x0013A474
		private void CreateDeliveryStatusDisplay(DeliveryInstance instance)
		{
			DeliveryStatusDisplay deliveryStatusDisplay = UnityEngine.Object.Instantiate<DeliveryStatusDisplay>(this.StatusDisplayPrefab, this.StatusDisplayContainer);
			deliveryStatusDisplay.AssignDelivery(instance);
			this.statusDisplays.Add(deliveryStatusDisplay);
			this.SortStatusDisplays();
			this.RefreshContent(true);
			this.RefreshNoDeliveriesIndicator();
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x0013C2BC File Offset: 0x0013A4BC
		private void DeliveryCompleted(DeliveryInstance instance)
		{
			DeliveryStatusDisplay deliveryStatusDisplay = this.statusDisplays.FirstOrDefault((DeliveryStatusDisplay d) => d.DeliveryInstance.DeliveryID == instance.DeliveryID);
			if (deliveryStatusDisplay != null)
			{
				this.statusDisplays.Remove(deliveryStatusDisplay);
				UnityEngine.Object.Destroy(deliveryStatusDisplay.gameObject);
			}
			this.RefreshNoDeliveriesIndicator();
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x0013C318 File Offset: 0x0013A518
		private void SortStatusDisplays()
		{
			this.statusDisplays = (from d in this.statusDisplays
			orderby d.DeliveryInstance.GetTimeStatus()
			select d).ToList<DeliveryStatusDisplay>();
			for (int i = 0; i < this.statusDisplays.Count; i++)
			{
				this.statusDisplays[i].transform.SetSiblingIndex(i);
			}
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x0013C387 File Offset: 0x0013A587
		private void RefreshNoDeliveriesIndicator()
		{
			this.NoDeliveriesIndicator.gameObject.SetActive(this.statusDisplays.Count == 0);
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x0013C3A8 File Offset: 0x0013A5A8
		public static void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
		{
			LayoutGroup[] componentsInChildren = root.GetComponentsInChildren<LayoutGroup>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(componentsInChildren[i].GetComponent<RectTransform>());
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(root.GetComponent<LayoutGroup>().GetComponent<RectTransform>());
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x0013C3E8 File Offset: 0x0013A5E8
		public DeliveryShop GetShop(ShopInterface matchingShop)
		{
			return this.deliveryShops.Find((DeliveryShop x) => x.MatchingShop == matchingShop);
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x0013C41C File Offset: 0x0013A61C
		public DeliveryShop GetShop(string shopName)
		{
			return this.deliveryShops.Find((DeliveryShop x) => x.MatchingShop.ShopName == shopName);
		}

		// Token: 0x0400379F RID: 14239
		private List<DeliveryShop> deliveryShops = new List<DeliveryShop>();

		// Token: 0x040037A0 RID: 14240
		public DeliveryStatusDisplay StatusDisplayPrefab;

		// Token: 0x040037A1 RID: 14241
		[Header("References")]
		public Animation OrderSubmittedAnim;

		// Token: 0x040037A2 RID: 14242
		public AudioSourceController OrderSubmittedSound;

		// Token: 0x040037A3 RID: 14243
		public RectTransform StatusDisplayContainer;

		// Token: 0x040037A4 RID: 14244
		public RectTransform NoDeliveriesIndicator;

		// Token: 0x040037A5 RID: 14245
		public ScrollRect MainScrollRect;

		// Token: 0x040037A6 RID: 14246
		public LayoutGroup MainLayoutGroup;

		// Token: 0x040037A7 RID: 14247
		private List<DeliveryStatusDisplay> statusDisplays = new List<DeliveryStatusDisplay>();

		// Token: 0x040037A8 RID: 14248
		private bool started;
	}
}
