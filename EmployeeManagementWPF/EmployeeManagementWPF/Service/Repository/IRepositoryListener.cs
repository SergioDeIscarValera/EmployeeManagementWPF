using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository;

interface IRepositoryListener<T, ID, IDC>: IRepository<T, ID, IDC>
{
    public void AddChangeListener(IDC idc, Action<IEnumerable<T>> action);
    public void RemoveChangeListener(IDC idc);
}
