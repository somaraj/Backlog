using Backlog.Core.Domain.Masters;
using Microsoft.AspNetCore.Http;

namespace Backlog.Service.Masters
{
    public interface IDocumentService
    {
        Task<Document> GetByIdAsync(int id);

        Task<Document> InsertAsync(IFormFile file);

        Task UpdateAsync(int documentId, IFormFile file);

        Task DeleteAsync(Document entity);
    }
}
