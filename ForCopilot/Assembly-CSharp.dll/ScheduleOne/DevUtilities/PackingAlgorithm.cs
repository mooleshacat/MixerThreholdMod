using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200072A RID: 1834
	public class PackingAlgorithm : Singleton<PackingAlgorithm>
	{
		// Token: 0x060031B3 RID: 12723 RVA: 0x000CF57C File Offset: 0x000CD77C
		public List<PackingAlgorithm.StoredItemData> PackItems(List<ItemInstance> datas, int gridX, int gridY)
		{
			List<PackingAlgorithm.StoredItemData> list = new List<PackingAlgorithm.StoredItemData>();
			for (int i = 0; i < datas.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = datas[i].Definition as StorableItemDefinition;
				if (!(storableItemDefinition == null))
				{
					PackingAlgorithm.StoredItemData item = new PackingAlgorithm.StoredItemData(storableItemDefinition.Name, storableItemDefinition.StoredItem.xSize, storableItemDefinition.StoredItem.ySize, datas[i]);
					list.Add(item);
				}
			}
			this.AttemptPack(list, gridX, gridY);
			return list;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000CF5F8 File Offset: 0x000CD7F8
		public List<PackingAlgorithm.StoredItemData> AttemptPack(List<PackingAlgorithm.StoredItemData> rects, int gridX, int gridY)
		{
			List<PackingAlgorithm.StoredItemData> list = (from o in rects
			orderby o.sizeX * o.sizeY
			select o).ToList<PackingAlgorithm.StoredItemData>();
			list.Reverse();
			PackingAlgorithm.Coordinate[,] array = new PackingAlgorithm.Coordinate[gridX, gridY];
			for (int i = 0; i < gridX; i++)
			{
				for (int j = 0; j < gridY; j++)
				{
					array[i, j] = new PackingAlgorithm.Coordinate(i, j);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				List<PackingAlgorithm.Coordinate> list2 = new List<PackingAlgorithm.Coordinate>();
				if (k == 0)
				{
					list2.Add(new PackingAlgorithm.Coordinate(0, 0));
				}
				for (int l = 0; l < gridX; l++)
				{
					for (int m = 0; m < gridY; m++)
					{
						if (array[l, m].occupant == null && this.DoesCoordinateHaveOccupiedAdjacent(array, new PackingAlgorithm.Coordinate(l, m), gridX, gridY))
						{
							list2.Add(new PackingAlgorithm.Coordinate(l, m));
						}
					}
				}
				int regionSize = this.GetRegionSize(array, gridX, gridY);
				int num = int.MaxValue;
				PackingAlgorithm.Coordinate coordinate = null;
				bool flipped = false;
				for (int n = 0; n < list2.Count; n++)
				{
					bool flag = true;
					for (int num2 = 0; num2 < list[k].actualSizeX; num2++)
					{
						for (int num3 = 0; num3 < list[k].actualSizeY; num3++)
						{
							PackingAlgorithm.Coordinate coordinate2 = this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num2, num3), gridX, gridY);
							if (coordinate2 == null)
							{
								flag = false;
							}
							else if (coordinate2.occupant != null)
							{
								flag = false;
							}
							if (!flag)
							{
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						for (int num4 = 0; num4 < list[k].actualSizeX; num4++)
						{
							for (int num5 = 0; num5 < list[k].actualSizeY; num5++)
							{
								this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num4, num5), gridX, gridY).occupant = list[k];
							}
						}
						int num6 = this.GetRegionSize(array, gridX, gridY) - regionSize;
						if (num6 < num)
						{
							num = num6;
							coordinate = list2[n];
							flipped = false;
						}
						for (int num7 = 0; num7 < list[k].actualSizeX; num7++)
						{
							for (int num8 = 0; num8 < list[k].actualSizeY; num8++)
							{
								this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num7, num8), gridX, gridY).occupant = null;
							}
						}
					}
				}
				for (int num9 = 0; num9 < list2.Count; num9++)
				{
					bool flag2 = true;
					list[k].flipped = true;
					for (int num10 = 0; num10 < list[k].actualSizeX; num10++)
					{
						for (int num11 = 0; num11 < list[k].actualSizeY; num11++)
						{
							PackingAlgorithm.Coordinate coordinate3 = this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num10, num11), gridX, gridY);
							if (coordinate3 == null)
							{
								flag2 = false;
							}
							else if (coordinate3.occupant != null)
							{
								flag2 = false;
							}
							if (!flag2)
							{
								break;
							}
						}
						if (!flag2)
						{
							break;
						}
					}
					if (flag2)
					{
						for (int num12 = 0; num12 < list[k].actualSizeX; num12++)
						{
							for (int num13 = 0; num13 < list[k].actualSizeY; num13++)
							{
								this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num12, num13), gridX, gridY).occupant = list[k];
							}
						}
						int num14 = this.GetRegionSize(array, gridX, gridY) - regionSize;
						if (num14 < num)
						{
							num = num14;
							coordinate = list2[num9];
							flipped = true;
						}
						for (int num15 = 0; num15 < list[k].actualSizeX; num15++)
						{
							for (int num16 = 0; num16 < list[k].actualSizeY; num16++)
							{
								this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num15, num16), gridX, gridY).occupant = null;
							}
						}
					}
				}
				if (coordinate == null)
				{
					Console.LogWarning("Unable to resolve rectangle position.", null);
				}
				else
				{
					list[k].flipped = flipped;
					for (int num17 = 0; num17 < list[k].actualSizeX; num17++)
					{
						for (int num18 = 0; num18 < list[k].actualSizeY; num18++)
						{
							this.TransformCoordinatePoint(array, coordinate, new PackingAlgorithm.Coordinate(num17, num18), gridX, gridY).occupant = list[k];
						}
					}
					list[k].xPos = coordinate.x;
					list[k].yPos = coordinate.y;
				}
			}
			return rects;
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000CFAA8 File Offset: 0x000CDCA8
		private bool DoesCoordinateHaveOccupiedAdjacent(PackingAlgorithm.Coordinate[,] grid, PackingAlgorithm.Coordinate coord, int gridX, int gridY)
		{
			PackingAlgorithm.Coordinate coordinate = new PackingAlgorithm.Coordinate(coord.x - 1, coord.y);
			if (this.IsCoordinateInBounds(coordinate, gridX, gridY) && grid[coordinate.x, coordinate.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate2 = new PackingAlgorithm.Coordinate(coord.x + 1, coord.y);
			if (this.IsCoordinateInBounds(coordinate2, gridX, gridY) && grid[coordinate2.x, coordinate2.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate3 = new PackingAlgorithm.Coordinate(coord.x, coord.y - 1);
			if (this.IsCoordinateInBounds(coordinate3, gridX, gridY) && grid[coordinate3.x, coordinate3.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate4 = new PackingAlgorithm.Coordinate(coord.x, coord.y + 1);
			return this.IsCoordinateInBounds(coordinate4, gridX, gridY) && grid[coordinate4.x, coordinate4.y].occupant != null;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000CFBA2 File Offset: 0x000CDDA2
		private bool IsCoordinateInBounds(PackingAlgorithm.Coordinate coord, int gridX, int gridY)
		{
			return coord.x >= 0 && coord.x < gridX && coord.y >= 0 && coord.y < gridY;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000CFBD0 File Offset: 0x000CDDD0
		private void PrintGrid(PackingAlgorithm.Coordinate[,] grid, int gridX, int gridY)
		{
			string text = string.Empty;
			for (int i = 0; i < gridY; i++)
			{
				for (int j = 0; j < gridX; j++)
				{
					if (grid[j, gridY - i - 1].occupant == null)
					{
						text += "*, ";
					}
					else
					{
						text = text + grid[j, gridY - i - 1].occupant.name + ", ";
					}
				}
				text += "\n";
			}
			Console.Log(text, null);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000CFC54 File Offset: 0x000CDE54
		private int GetRegionSize(PackingAlgorithm.Coordinate[,] grid, int gridX, int gridY)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < gridX; i++)
			{
				for (int j = 0; j < gridY; j++)
				{
					if (grid[i, j].occupant != null)
					{
						if (i > num3)
						{
							num3 = i;
						}
						if (j > num4)
						{
							num4 = j;
						}
					}
				}
			}
			return (num3 - num) * (num4 - num2);
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000CFCB4 File Offset: 0x000CDEB4
		private PackingAlgorithm.Coordinate TransformCoordinatePoint(PackingAlgorithm.Coordinate[,] grid, PackingAlgorithm.Coordinate baseCoordinate, PackingAlgorithm.Coordinate offset, int gridX, int gridY)
		{
			if (this.IsCoordinateInBounds(new PackingAlgorithm.Coordinate(baseCoordinate.x + offset.x, baseCoordinate.y + offset.y), gridX, gridY))
			{
				return grid[baseCoordinate.x + offset.x, baseCoordinate.y + offset.y];
			}
			return null;
		}

		// Token: 0x040022FE RID: 8958
		public List<PackingAlgorithm.Rectangle> rectsToPack = new List<PackingAlgorithm.Rectangle>();

		// Token: 0x0200072B RID: 1835
		[Serializable]
		public class Rectangle
		{
			// Token: 0x17000725 RID: 1829
			// (get) Token: 0x060031BB RID: 12731 RVA: 0x000CFD21 File Offset: 0x000CDF21
			public int actualSizeX
			{
				get
				{
					if (this.flipped)
					{
						return this.sizeY;
					}
					return this.sizeX;
				}
			}

			// Token: 0x17000726 RID: 1830
			// (get) Token: 0x060031BC RID: 12732 RVA: 0x000CFD38 File Offset: 0x000CDF38
			public int actualSizeY
			{
				get
				{
					if (this.flipped)
					{
						return this.sizeX;
					}
					return this.sizeY;
				}
			}

			// Token: 0x060031BD RID: 12733 RVA: 0x000CFD4F File Offset: 0x000CDF4F
			public Rectangle(string _name, int x, int y)
			{
				this.name = _name;
				this.sizeX = x;
				this.sizeY = y;
			}

			// Token: 0x040022FF RID: 8959
			public string name;

			// Token: 0x04002300 RID: 8960
			public int sizeX;

			// Token: 0x04002301 RID: 8961
			public int sizeY;

			// Token: 0x04002302 RID: 8962
			public bool flipped;
		}

		// Token: 0x0200072C RID: 1836
		public class StoredItemData : PackingAlgorithm.Rectangle
		{
			// Token: 0x17000727 RID: 1831
			// (get) Token: 0x060031BE RID: 12734 RVA: 0x000CFD6C File Offset: 0x000CDF6C
			public float rotation
			{
				get
				{
					if (!this.flipped)
					{
						return 0f;
					}
					return 90f;
				}
			}

			// Token: 0x060031BF RID: 12735 RVA: 0x000CFD81 File Offset: 0x000CDF81
			public StoredItemData(string _name, int x, int y, ItemInstance _item) : base(_name, x, y)
			{
				this.item = _item;
			}

			// Token: 0x04002303 RID: 8963
			public ItemInstance item;

			// Token: 0x04002304 RID: 8964
			public int xPos;

			// Token: 0x04002305 RID: 8965
			public int yPos;
		}

		// Token: 0x0200072D RID: 1837
		public class Coordinate
		{
			// Token: 0x060031C0 RID: 12736 RVA: 0x000CFD94 File Offset: 0x000CDF94
			public Coordinate(int _x, int _y)
			{
				this.x = _x;
				this.y = _y;
			}

			// Token: 0x04002306 RID: 8966
			public int x;

			// Token: 0x04002307 RID: 8967
			public int y;

			// Token: 0x04002308 RID: 8968
			public PackingAlgorithm.Rectangle occupant;
		}
	}
}
