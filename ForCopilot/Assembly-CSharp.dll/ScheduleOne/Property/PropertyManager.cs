using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Property
{
	// Token: 0x02000857 RID: 2135
	public class PropertyManager : Singleton<PropertyManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x000F567C File Offset: 0x000F387C
		public string SaveFolderName
		{
			get
			{
				return "Properties";
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000F567C File Offset: 0x000F387C
		public string SaveFileName
		{
			get
			{
				return "Properties";
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000F5683 File Offset: 0x000F3883
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x000F568B File Offset: 0x000F388B
		// (set) Token: 0x060039E9 RID: 14825 RVA: 0x000F5693 File Offset: 0x000F3893
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x000F569C File Offset: 0x000F389C
		// (set) Token: 0x060039EB RID: 14827 RVA: 0x000F56A4 File Offset: 0x000F38A4
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x000F56AD File Offset: 0x000F38AD
		// (set) Token: 0x060039ED RID: 14829 RVA: 0x000F56B5 File Offset: 0x000F38B5
		public bool HasChanged { get; set; } = true;

		// Token: 0x060039EE RID: 14830 RVA: 0x000F56BE File Offset: 0x000F38BE
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x000F56CC File Offset: 0x000F38CC
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < Property.OwnedProperties.Count; i++)
			{
				try
				{
					if (Property.OwnedProperties[i].ShouldSave())
					{
						new SaveRequest(Property.OwnedProperties[i], containerFolder);
						list.Add(Property.OwnedProperties[i].SaveFolderName);
					}
				}
				catch (Exception ex)
				{
					Console.LogError("Error saving property: " + Property.OwnedProperties[i].PropertyCode + " - " + ex.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			for (int j = 0; j < Property.UnownedProperties.Count; j++)
			{
				try
				{
					if (Property.UnownedProperties[j].ShouldSave())
					{
						new SaveRequest(Property.UnownedProperties[j], containerFolder);
						list.Add(Property.UnownedProperties[j].SaveFolderName);
					}
				}
				catch (Exception ex2)
				{
					Console.LogError("Error saving property: " + Property.OwnedProperties[j].PropertyCode + " - " + ex2.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			return list;
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x000F5824 File Offset: 0x000F3A24
		public virtual void DeleteUnapprovedFiles(string parentFolderPath)
		{
			string[] directories = Directory.GetDirectories(((ISaveable)this).GetContainerFolder(parentFolderPath));
			for (int i = 0; i < directories.Length; i++)
			{
				new DirectoryInfo(directories[i]);
				Directory.Delete(directories[i], true);
			}
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x000F5860 File Offset: 0x000F3A60
		public void LoadProperty(PropertyData propertyData)
		{
			Property property = Property.UnownedProperties.FirstOrDefault((Property p) => p.PropertyCode == propertyData.PropertyCode);
			if (property == null)
			{
				property = Property.OwnedProperties.FirstOrDefault((Property p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				property = Business.UnownedBusinesses.FirstOrDefault((Business p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				property = Business.OwnedBusinesses.FirstOrDefault((Business p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				Console.LogWarning("Property not found for data: " + propertyData.PropertyCode, null);
				return;
			}
			property.Load(propertyData);
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x000F5924 File Offset: 0x000F3B24
		public Property GetProperty(string code)
		{
			Property property = Property.UnownedProperties.FirstOrDefault((Property p) => p.PropertyCode == code);
			if (property == null)
			{
				property = Property.OwnedProperties.FirstOrDefault((Property p) => p.PropertyCode == code);
			}
			return property;
		}

		// Token: 0x040029C1 RID: 10689
		private PropertiesLoader loader = new PropertiesLoader();
	}
}
