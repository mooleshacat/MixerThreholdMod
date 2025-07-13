using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x020002AE RID: 686
	public class SnakeTile : MonoBehaviour
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0003FD49 File Offset: 0x0003DF49
		// (set) Token: 0x06000E5F RID: 3679 RVA: 0x0003FD51 File Offset: 0x0003DF51
		public SnakeTile.TileType Type { get; private set; }

		// Token: 0x06000E60 RID: 3680 RVA: 0x0003FD5C File Offset: 0x0003DF5C
		public void SetType(SnakeTile.TileType type, int index = 0)
		{
			this.Type = type;
			switch (this.Type)
			{
			case SnakeTile.TileType.Empty:
				base.gameObject.SetActive(false);
				return;
			case SnakeTile.TileType.Snake:
				this.Image.color = this.SnakeColor;
				if (index > 0)
				{
					float a = 1f - 0.8f * Mathf.Sqrt((float)index / 240f);
					this.Image.color = new Color(this.SnakeColor.r, this.SnakeColor.g, this.SnakeColor.b, a);
				}
				base.gameObject.SetActive(true);
				return;
			case SnakeTile.TileType.Food:
				this.Image.color = this.FoodColor;
				base.gameObject.SetActive(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003FE24 File Offset: 0x0003E024
		public void SetPosition(Vector2 position, float tileSize)
		{
			this.Position = position;
			this.RectTransform.anchoredPosition = new Vector2((0.5f + position.x) * tileSize, (0.5f + position.y) * tileSize);
			base.gameObject.name = string.Format("Tile {0}, {1}", position.x, position.y);
			this.RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
		}

		// Token: 0x04000EDA RID: 3802
		public Vector2 Position = Vector2.zero;

		// Token: 0x04000EDB RID: 3803
		public Color SnakeColor;

		// Token: 0x04000EDC RID: 3804
		public Color FoodColor;

		// Token: 0x04000EDD RID: 3805
		public RectTransform RectTransform;

		// Token: 0x04000EDE RID: 3806
		public Image Image;

		// Token: 0x020002AF RID: 687
		public enum TileType
		{
			// Token: 0x04000EE0 RID: 3808
			Empty,
			// Token: 0x04000EE1 RID: 3809
			Snake,
			// Token: 0x04000EE2 RID: 3810
			Food
		}
	}
}
