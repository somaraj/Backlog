using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class ClientService : IClientService
    {
        #region Fields

        protected readonly IRepository<Client> _clientRepository;

        #endregion

        #region Ctor
        public ClientService(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Client>> GetPagedListAsync(string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _clientRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => !x.Deleted);
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Client).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c => c.Name.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<Client>> GetAllAsync(bool showDeleted = false)
        {
            return await _clientRepository.GetAllAsync(includeDeleted: showDeleted);
        }

        public async Task<IList<Client>> GetAllActiveAsync(bool showDeleted = false)
        {
            return await _clientRepository.GetAllAsync(q => q.Where(x => x.Active), showDeleted);
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _clientRepository.GetByIdAsync(id);
        }

        public async Task<Client> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _clientRepository.Table
                        orderby c.Id
                        where !c.Deleted && c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Client entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _clientRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Client entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _clientRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Client entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _clientRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
