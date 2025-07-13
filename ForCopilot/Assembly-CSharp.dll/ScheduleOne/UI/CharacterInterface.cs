using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A09 RID: 2569
	public class CharacterInterface : MonoBehaviour
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600450F RID: 17679 RVA: 0x00121CA3 File Offset: 0x0011FEA3
		// (set) Token: 0x06004510 RID: 17680 RVA: 0x00121CAB File Offset: 0x0011FEAB
		public bool IsOpen { get; private set; }

		// Token: 0x06004511 RID: 17681 RVA: 0x00121CB4 File Offset: 0x0011FEB4
		private void Awake()
		{
			this.Close();
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00121CBC File Offset: 0x0011FEBC
		private void LateUpdate()
		{
			if (this.IsOpen)
			{
				foreach (ClothingSlotUI clothingSlotUI in this.ClothingSlots)
				{
					Transform component = clothingSlotUI.GetComponent<RectTransform>();
					Transform transform = this.SlotAlignmentPoints[clothingSlotUI];
					component.position = RectTransformUtility.WorldToScreenPoint(Singleton<GameplayMenu>.Instance.OverlayCamera, transform.position);
				}
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00121D1C File Offset: 0x0011FF1C
		public void Open()
		{
			if (this.SlotAlignmentPoints.Count == 0)
			{
				ClothingSlotUI[] clothingSlots = this.ClothingSlots;
				for (int i = 0; i < clothingSlots.Length; i++)
				{
					ClothingSlotUI slotUI = clothingSlots[i];
					slotUI.AssignSlot(Player.Local.Clothing.ClothingSlots[slotUI.SlotType]);
					CharacterDisplay.SlotAlignmentPoint slotAlignmentPoint = Singleton<CharacterDisplay>.Instance.AlignmentPoints.FirstOrDefault((CharacterDisplay.SlotAlignmentPoint x) => x.SlotType == slotUI.SlotType);
					if (slotAlignmentPoint != null)
					{
						this.SlotAlignmentPoints.Add(slotUI, slotAlignmentPoint.Point);
					}
					else
					{
						Console.LogError(string.Format("No alignment point found for slot type {0}", slotUI.SlotType), null);
					}
				}
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
			this.LateUpdate();
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x00121E03 File Offset: 0x00120003
		public void Close()
		{
			this.IsOpen = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x040031E8 RID: 12776
		public ClothingSlotUI[] ClothingSlots;

		// Token: 0x040031E9 RID: 12777
		public RectTransform Container;

		// Token: 0x040031EA RID: 12778
		public Slider RotationSlider;

		// Token: 0x040031EB RID: 12779
		private Dictionary<ClothingSlotUI, Transform> SlotAlignmentPoints = new Dictionary<ClothingSlotUI, Transform>();
	}
}
