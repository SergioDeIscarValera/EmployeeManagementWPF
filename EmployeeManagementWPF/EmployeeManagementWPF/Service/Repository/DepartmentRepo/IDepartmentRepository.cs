using EmployeeManagementWPF.MVVM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository.DepartmentRepo;

internal interface IDepartmentRepository : IRepositoryListener<Department, string, string>
{
    Task<IEnumerable<Department>> ContainsName(string name, string idc);
}
