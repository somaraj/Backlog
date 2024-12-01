using Backlog.Core.Caching;

namespace Backlog.Service.Security
{
    public class SystemService : ISystemService
    {
        #region Fields

        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public SystemService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        public async Task ResetCacheAsync()
        {
            await _cacheManager.ClearAsync();
        }

        #endregion
    }
}