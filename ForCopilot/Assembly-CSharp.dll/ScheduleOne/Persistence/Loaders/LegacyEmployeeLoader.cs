using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003CF RID: 975
	public class LegacyEmployeeLoader : LegacyNPCLoader
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0005F983 File Offset: 0x0005DB83
		public override string NPCType
		{
			get
			{
				return typeof(EmployeeData).Name;
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00060134 File Offset: 0x0005E334
		public Employee LoadAndCreateEmployee(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "NPC", out text))
			{
				EmployeeData employeeData = null;
				try
				{
					employeeData = JsonUtility.FromJson<EmployeeData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
					return null;
				}
				if (employeeData == null)
				{
					Console.LogWarning("Failed to load employee data", null);
					return null;
				}
				Property property = Singleton<PropertyManager>.Instance.GetProperty(employeeData.AssignedProperty);
				EEmployeeType type2 = EEmployeeType.Botanist;
				if (employeeData.DataType == typeof(PackagerData).Name)
				{
					type2 = EEmployeeType.Handler;
				}
				else if (employeeData.DataType == typeof(BotanistData).Name)
				{
					type2 = EEmployeeType.Botanist;
				}
				else if (employeeData.DataType == typeof(ChemistData).Name)
				{
					type2 = EEmployeeType.Chemist;
				}
				else if (employeeData.DataType == typeof(CleanerData).Name)
				{
					type2 = EEmployeeType.Cleaner;
				}
				else
				{
					Console.LogError("Failed to recognize employee type: " + employeeData.DataType, null);
				}
				Employee employee = NetworkSingleton<EmployeeManager>.Instance.CreateEmployee_Server(property, type2, employeeData.FirstName, employeeData.LastName, employeeData.ID, employeeData.IsMale, employeeData.AppearanceIndex, employeeData.Position, employeeData.Rotation, employeeData.GUID);
				if (employee == null)
				{
					Console.LogWarning("Failed to create employee", null);
					return null;
				}
				if (employeeData.PaidForToday)
				{
					employee.SetIsPaid();
				}
				base.TryLoadInventory(mainPath, employee);
				return employee;
			}
			return null;
		}
	}
}
