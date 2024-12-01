using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IEmailAccountService
    {
        Task<IPagedList<EmailAccount>> GetPagedListAsync(string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "");

        Task<EmailAccount> GetByIdAsync(int id);

        Task<EmailAccount> GetByNameAsync(string name);

        Task<IList<EmailAccount>> GetAllActiveAsync();

        Task<EmailAccount> GetFirstActiveAsync();

        Task InsertAsync(EmailAccount entity);

        Task UpdateAsync(EmailAccount entity);

        Task DeleteAsync(EmailAccount entity);
    }
}