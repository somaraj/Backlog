using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IEmailTemplateService
    {
        Task<IPagedList<EmailTemplate>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "");

        Task<EmailTemplate> GetByIdAsync(int id);

        Task<EmailTemplate> GetByNameAsync(string name);

        Task<IList<EmailTemplate>> GetByEmailAccountIdAsync(int emailAccountId);

        Task InsertAsync(EmailTemplate entity);

        Task UpdateAsync(EmailTemplate entity);
    }
}