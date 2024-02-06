using EmployeeManagementWPF.MVVM.Model;
using EmployeeManagementWPF.Service.Repository.Mongo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository.EmployeeRepo;

internal class EmployeeRepositoryMongo : MongoRepository<Employee, string>, IEmployeeRepository
{
    public EmployeeRepositoryMongo() : base("employees")
    {
    }

    public async Task<IEnumerable<Employee>> ContainsName(string name, string idc) =>
        (await FindAll(idc)).Where(e => e.Name.Contains(name));

    public async Task DeleteByDepartment(string idDepartment, string idc)
    {
        var employees = await FindAllByDepartment(idDepartment, idc);
        foreach (var employee in employees)
        {
            await DeleteById(employee.Id, idc);
        }
    }

    public async Task<IEnumerable<Employee>> FindAllByDepartment(string idDepartment, string idc) =>
        (await FindAll(idc)).Where(e => e.IdDepartment == idDepartment);

    public async Task<Employee?> FindByDni(string dni, string idDepartment, string idc) =>
        (await FindAllByDepartment(idDepartment, idc)).FirstOrDefault(e => e.Dni == dni);
}
