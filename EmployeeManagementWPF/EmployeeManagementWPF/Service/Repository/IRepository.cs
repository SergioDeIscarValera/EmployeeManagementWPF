using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository;

interface IRepository<T, ID, IDC>
{
    public Task<IEnumerable<T>> FindAll(IDC idc);
    public Task<T?> FindById(ID id, IDC idc);
    public Task<T?> Save(T t, IDC idc, ID id);
    public Task DeleteById(ID id, IDC idc);
    public Task DeleteAll(IDC idc);
    public Task<bool> ExistsById(ID id, IDC idc);
    public Task<bool> Exists(T t, IDC idc);
    public Task<long> Count(IDC idc);
}
