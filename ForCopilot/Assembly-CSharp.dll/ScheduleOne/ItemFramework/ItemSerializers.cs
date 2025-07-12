using System;
using FishNet.Serializing;
using ScheduleOne.Clothing;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Product;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000990 RID: 2448
	public static class ItemSerializers
	{
		// Token: 0x060041EA RID: 16874 RVA: 0x00115794 File Offset: 0x00113994
		private static ItemInstance Read(this Reader reader)
		{
			if (reader.Remaining == 0)
			{
				return null;
			}
			string text = reader.ReadString();
			if (text == typeof(ItemInstance).Name)
			{
				return reader.DirectReadItemInstance();
			}
			if (text == typeof(StorableItemInstance).Name)
			{
				return reader.DirectReadStorableItemInstance();
			}
			if (text == typeof(CashInstance).Name)
			{
				return reader.DirectReadCashInstance();
			}
			if (text == typeof(ClothingInstance).Name)
			{
				return reader.DirectReadClothingInstance();
			}
			if (text == typeof(QualityItemInstance).Name)
			{
				return reader.DirectReadQualityItemInstance();
			}
			if (text == typeof(ProductItemInstance).Name)
			{
				return reader.DirectReadProductItemInstance();
			}
			if (text == typeof(WeedInstance).Name)
			{
				return reader.DirectReadWeedInstance();
			}
			if (text == typeof(MethInstance).Name)
			{
				return reader.DirectReadMethInstance();
			}
			if (text == typeof(CocaineInstance).Name)
			{
				return reader.DirectReadCocaineInstance();
			}
			if (text == typeof(IntegerItemInstance).Name)
			{
				return reader.DirectReadIntegerItemInstance();
			}
			if (text == typeof(WateringCanInstance).Name)
			{
				return reader.DirectReadWateringCanInstance();
			}
			if (text == typeof(TrashGrabberInstance).Name)
			{
				return reader.DirectReadTrashGrabberInstance();
			}
			if (reader.ReadString() == string.Empty)
			{
				return null;
			}
			Console.LogError("ItemSerializers: reader not found for '" + text + "'!", null);
			return null;
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00115948 File Offset: 0x00113B48
		public static void WriteItemInstance(this Writer writer, ItemInstance value)
		{
			if (value is StorableItemInstance)
			{
				writer.WriteStorableItemInstance((StorableItemInstance)value);
				return;
			}
			if (value == null)
			{
				writer.WriteString(typeof(ItemInstance).Name);
				writer.WriteString(string.Empty);
				return;
			}
			writer.WriteString(typeof(ItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x001159BC File Offset: 0x00113BBC
		public static ItemInstance ReadItemInstance(this Reader reader)
		{
			return reader.Read();
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x001159C4 File Offset: 0x00113BC4
		private static ItemInstance DirectReadItemInstance(this Reader reader)
		{
			reader.ReadString() == string.Empty;
			return null;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x001159D8 File Offset: 0x00113BD8
		public static void WriteStorableItemInstance(this Writer writer, StorableItemInstance value)
		{
			if (value is QualityItemInstance)
			{
				writer.WriteQualityItemInstance((QualityItemInstance)value);
				return;
			}
			if (value is CashInstance)
			{
				writer.WriteCashInstance(value as CashInstance);
				return;
			}
			if (value is ClothingInstance)
			{
				writer.WriteClothingInstance(value as ClothingInstance);
				return;
			}
			if (value is IntegerItemInstance)
			{
				writer.WriteIntegerItemInstance(value as IntegerItemInstance);
				return;
			}
			if (value is WateringCanInstance)
			{
				writer.WriteWateringCanInstance(value as WateringCanInstance);
				return;
			}
			if (value is TrashGrabberInstance)
			{
				writer.WriteTrashGrabberInstance(value as TrashGrabberInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(StorableItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x00115A95 File Offset: 0x00113C95
		public static StorableItemInstance ReadStorableItemInstance(this Reader reader)
		{
			return reader.Read() as StorableItemInstance;
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00115AA4 File Offset: 0x00113CA4
		private static StorableItemInstance DirectReadStorableItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new StorableItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16()
			};
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x00115AE0 File Offset: 0x00113CE0
		public static void WriteCashInstance(this Writer writer, CashInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(CashInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteSingle(value.Balance, 0);
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x00115B2C File Offset: 0x00113D2C
		public static CashInstance ReadCashInstance(this Reader reader)
		{
			return reader.Read() as CashInstance;
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00115B3C File Offset: 0x00113D3C
		private static CashInstance DirectReadCashInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			CashInstance cashInstance = new CashInstance();
			cashInstance.ID = text;
			cashInstance.Quantity = (int)reader.ReadUInt16();
			cashInstance.SetBalance(reader.ReadSingle(0), false);
			return cashInstance;
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x00115B88 File Offset: 0x00113D88
		public static void WriteQualityItemInstance(this Writer writer, QualityItemInstance value)
		{
			if (value is ProductItemInstance)
			{
				writer.WriteProductItemInstance(value as ProductItemInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(QualityItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x00115BE9 File Offset: 0x00113DE9
		public static QualityItemInstance ReadQualityItemInstance(this Reader reader)
		{
			return reader.Read() as QualityItemInstance;
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00115BF8 File Offset: 0x00113DF8
		private static QualityItemInstance DirectReadQualityItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new QualityItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16()
			};
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00115C40 File Offset: 0x00113E40
		public static void WriteClothingInstance(this Writer writer, ClothingInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(ClothingInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Color);
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00115C8C File Offset: 0x00113E8C
		public static ClothingInstance ReadClothingInstance(this Reader reader)
		{
			return reader.Read() as ClothingInstance;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00115C9C File Offset: 0x00113E9C
		private static ClothingInstance DirectReadClothingInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new ClothingInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Color = (EClothingColor)reader.ReadUInt16()
			};
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00115CE4 File Offset: 0x00113EE4
		public static void WriteProductItemInstance(this Writer writer, ProductItemInstance value)
		{
			if (value is WeedInstance)
			{
				writer.WriteWeedInstance(value as WeedInstance);
				return;
			}
			if (value is MethInstance)
			{
				writer.WriteMethInstance(value as MethInstance);
				return;
			}
			if (value is CocaineInstance)
			{
				writer.WriteCocaineInstance(value as CocaineInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(ProductItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00115D7B File Offset: 0x00113F7B
		public static ProductItemInstance ReadProductItemInstance(this Reader reader)
		{
			return reader.Read() as ProductItemInstance;
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00115D88 File Offset: 0x00113F88
		private static ProductItemInstance DirectReadProductItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new ProductItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x00115DDC File Offset: 0x00113FDC
		public static void WriteWeedInstance(this Writer writer, WeedInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(WeedInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x00115E34 File Offset: 0x00114034
		public static WeedInstance ReadWeedInstance(this Reader reader)
		{
			return reader.Read() as WeedInstance;
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x00115E44 File Offset: 0x00114044
		private static WeedInstance DirectReadWeedInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new WeedInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x00115E98 File Offset: 0x00114098
		public static void WriteMethInstance(this Writer writer, MethInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(MethInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00115EF0 File Offset: 0x001140F0
		public static MethInstance ReadMethInstance(this Reader reader)
		{
			return reader.Read() as MethInstance;
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00115F00 File Offset: 0x00114100
		private static MethInstance DirectReadMethInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new MethInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x00115F54 File Offset: 0x00114154
		public static void WriteCocaineInstance(this Writer writer, CocaineInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(CocaineInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x00115FAC File Offset: 0x001141AC
		public static CocaineInstance ReadCocaineInstance(this Reader reader)
		{
			return reader.Read() as CocaineInstance;
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00115FBC File Offset: 0x001141BC
		private static CocaineInstance DirectReadCocaineInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new CocaineInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x00116010 File Offset: 0x00114210
		public static void WriteIntegerItemInstance(this Writer writer, IntegerItemInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(IntegerItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Value);
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x0011605C File Offset: 0x0011425C
		public static IntegerItemInstance ReadIntegerItemInstance(this Reader reader)
		{
			return reader.Read() as IntegerItemInstance;
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0011606C File Offset: 0x0011426C
		private static IntegerItemInstance DirectReadIntegerItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new IntegerItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Value = (int)reader.ReadUInt16()
			};
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x001160B4 File Offset: 0x001142B4
		public static void WriteWateringCanInstance(this Writer writer, WateringCanInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(WateringCanInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteSingle(value.CurrentFillAmount, 0);
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00116100 File Offset: 0x00114300
		public static WateringCanInstance ReadWateringCanInstance(this Reader reader)
		{
			return reader.Read() as WateringCanInstance;
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x00116110 File Offset: 0x00114310
		private static WateringCanInstance DirectReadWateringCanInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new WateringCanInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				CurrentFillAmount = reader.ReadSingle(0)
			};
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00116158 File Offset: 0x00114358
		public static void WriteTrashGrabberInstance(this Writer writer, TrashGrabberInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(TrashGrabberInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			string[] array = value.GetTrashIDs().ToArray();
			writer.WriteArray<string>(array, 0, array.Length);
			ushort[] array2 = value.GetTrashUshortQuantities().ToArray();
			writer.WriteArray<ushort>(array2, 0, array2.Length);
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x001161C5 File Offset: 0x001143C5
		public static TrashGrabberInstance ReadTrashGrabberInstance(this Reader reader)
		{
			return reader.Read() as TrashGrabberInstance;
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x001161D4 File Offset: 0x001143D4
		private static TrashGrabberInstance DirectReadTrashGrabberInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance();
			trashGrabberInstance.ID = text;
			trashGrabberInstance.Quantity = (int)reader.ReadUInt16();
			string[] array = new string[20];
			ushort[] array2 = new ushort[20];
			int num = reader.ReadArray<string>(ref array);
			reader.ReadArray<ushort>(ref array2);
			for (int i = 0; i < num; i++)
			{
				trashGrabberInstance.AddTrash(array[i], (int)array2[i]);
			}
			return trashGrabberInstance;
		}

		// Token: 0x04002F07 RID: 12039
		public const bool DEBUG = false;
	}
}
