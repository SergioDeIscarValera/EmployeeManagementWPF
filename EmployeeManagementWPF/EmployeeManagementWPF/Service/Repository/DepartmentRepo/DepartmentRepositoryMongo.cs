using EmployeeManagementWPF.MVVM.Model;
using EmployeeManagementWPF.Service.Repository.Mongo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository.DepartmentRepo;

internal class DepartmentRepositoryMongo : MongoRepository<Department, string>, IDepartmentRepository
{
    public DepartmentRepositoryMongo() : base("deparments")
    {
    }

    public async Task<IEnumerable<Department>> ContainsName(string name, string idc) =>
        (await FindAll(idc)).Where(d => d.Name.Contains(name));
}
