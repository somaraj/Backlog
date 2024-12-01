using System.Linq.Expressions;
using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ISettingService
    {
        Task<Setting> GetSettingByIdAsync(int settingId);

        Setting GetSettingById(int settingId);

        Task DeleteSettingAsync(Setting setting);

        void DeleteSetting(Setting setting);

        Task DeleteSettingsAsync(IList<Setting> settings);

        Task<Setting> GetSettingAsync(string key);

        Setting GetSetting(string key);

        Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default);

        T GetSettingByKey<T>(string key, T defaultValue = default);

        Task SetSettingAsync<T>(string key, T value, bool clearCache = true);

        void SetSetting<T>(string key, T value, bool clearCache = true);

        Task<IList<Setting>> GetAllSettingsAsync();

        IList<Setting> GetAllSettings();

        Task<bool> SettingExistsAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();

        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();

        Task<T> LoadSettingAsync<T>() where T : ISettings, new();

        T LoadSetting<T>() where T : ISettings, new();

        Task<ISettings> LoadSettingAsync(Type type);

        ISettings LoadSetting(Type type);

        Task SaveSettingAsync<T>(T settings) where T : ISettings, new();

        void SaveSetting<T>(T settings) where T : ISettings, new();

        Task SaveSettingAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();

        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();

        Task InsertSettingAsync(Setting setting, bool clearCache = true);

        void InsertSetting(Setting setting, bool clearCache = true);

        Task UpdateSettingAsync(Setting setting, bool clearCache = true);

        void UpdateSetting(Setting setting, bool clearCache = true);

        Task DeleteSettingAsync<T>() where T : ISettings, new();

        Task DeleteSettingAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();

        Task ClearCacheAsync();

        void ClearCache();

        string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector)
            where TSettings : ISettings, new();
    }
}