using EmployeeManagementWPF.MVVM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository.EmployeeRepo;

internal interface IEmployeeRepository : IRepositoryListener<Employee, string, string>
{
    Task<Employee?> FindByDni(string dni, string idDepartment, string idc);
    Task<IEnumerable<Employee>> FindAllByDepartment(string idDepartment, string idc);
    Task<IEnumerable<Employee>> ContainsName(string name, string idc);
    Task DeleteByDepartment(string idDepartment, string idc);
}
