using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IDepartmentRepository DepartmentRepository { get; set; }
        IEmployeeRepository EmployeeRepository { get; set; }

        Task<int> CompleteAsync();
    }
}
