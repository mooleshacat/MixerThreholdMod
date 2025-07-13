using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C2 RID: 962
	public class EmployeeLoader : NPCLoader
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x0005F983 File Offset: 0x0005DB83
		public override string NPCType
		{
			get
			{
				return typeof(EmployeeData).Name;
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0005F99C File Offset: 0x0005DB9C
		public override void Load(DynamicSaveData saveData)
		{
			this.CreateAndLoadEmployee(saveData);
			base.Load(saveData);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0005F9B0 File Offset: 0x0005DBB0
		protected virtual Employee CreateAndLoadEmployee(DynamicSaveData saveData)
		{
			EmployeeData employeeData = DynamicLoader.ExtractBaseData<EmployeeData>(saveData);
			if (employeeData != null)
			{
				Property property = Singleton<PropertyManager>.Instance.GetProperty(employeeData.AssignedProperty);
				EEmployeeType type = EEmployeeType.Botanist;
				if (employeeData.DataType == typeof(PackagerData).Name)
				{
					type = EEmployeeType.Handler;
				}
				else if (employeeData.DataType == typeof(BotanistData).Name)
				{
					type = EEmployeeType.Botanist;
				}
				else if (employeeData.DataType == typeof(ChemistData).Name)
				{
					type = EEmployeeType.Chemist;
				}
				else if (employeeData.DataType == typeof(CleanerData).Name)
				{
					type = EEmployeeType.Cleaner;
				}
				else
				{
					Console.LogError("Failed to recognize employee type: " + employeeData.DataType, null);
				}
				Console.Log(string.Concat(new string[]
				{
					"Creating employee: ",
					employeeData.FirstName,
					" ",
					employeeData.LastName,
					" (",
					type.ToString(),
					") at ",
					property.PropertyName
				}), null);
				Employee employee = NetworkSingleton<EmployeeManager>.Instance.CreateEmployee_Server(property, type, employeeData.FirstName, employeeData.LastName, employeeData.ID, employeeData.IsMale, employeeData.AppearanceIndex, employeeData.Position, employeeData.Rotation, employeeData.GUID);
				if (employee == null)
				{
					Console.LogWarning("Failed to create employee", null);
				}
				if (employeeData.PaidForToday)
				{
					employee.SetIsPaid();
				}
				return employee;
			}
			return null;
		}
	}
}
