using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Backlog.Core.Caching;
using Backlog.Core.Common;
using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;
using Backlog.Service.Common;

namespace Backlog.Service.Masters
{
    public class SettingService : ISettingService
    {
        #region Fields

        protected readonly IRepository<Setting> _settingRepository;
        protected readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public SettingService(IRepository<Setting> settingRepository,
            ICacheManager cacheManager)
        {
            _settingRepository = settingRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Utilities

        protected async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
        {
            return await _cacheManager.GetAsync(ServiceConstant.SettingsAllAsDictionaryCacheKey, async () =>
            {
                var settings = await GetAllSettingsAsync();

                var dictionary = new Dictionary<string, IList<Setting>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new Setting
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.TryGetValue(resourceName, out var value))
                        dictionary.Add(resourceName, new List<Setting>
                    {
                        settingForCaching
                    });
                    else
                        value.Add(settingForCaching);
                }

                return dictionary;
            });
        }

        protected IDictionary<string, IList<Setting>> GetAllSettingsDictionary()
        {
            return _cacheManager.Get(ServiceConstant.SettingsAllAsDictionaryCacheKey, () =>
            {
                var settings = GetAllSettings();

                var dictionary = new Dictionary<string, IList<Setting>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new Setting
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.TryGetValue(resourceName, out var value))
                        dictionary.Add(resourceName, new List<Setting>
                    {
                        settingForCaching
                    });
                    else
                        value.Add(settingForCaching);
                }

                return dictionary;
            });
        }

        protected async Task SetSettingAsync(Type type, string key, object value, bool clearCache = true)
        {
            ArgumentNullException.ThrowIfNull(key);
            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);

            var allSettings = await GetAllSettingsDictionaryAsync();
            var settingForCaching = allSettings.TryGetValue(key, out var settings) ?
                settings.FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                var setting = await GetSettingByIdAsync(settingForCaching.Id);
                setting.Value = valueStr;
                await UpdateSettingAsync(setting, clearCache);
            }
            else
            {
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                };
                await InsertSettingAsync(setting, clearCache);
            }
        }

        protected void SetSetting(Type type, string key, object value, bool clearCache = true)
        {
            ArgumentNullException.ThrowIfNull(key);
            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsDictionary();
            var settingForCaching = allSettings.TryGetValue(key, out var settings) ?
                settings.FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                };
                InsertSetting(setting, clearCache);
            }
        }

        #endregion

        #region Methods

        public async Task InsertSettingAsync(Setting setting, bool clearCache = true)
        {
            await _settingRepository.InsertAsync(setting);

            if (clearCache)
                await ClearCacheAsync();
        }

        public void InsertSetting(Setting setting, bool clearCache = true)
        {
            _settingRepository.Insert(setting);

            if (clearCache)
                ClearCache();
        }

        public async Task UpdateSettingAsync(Setting setting, bool clearCache = true)
        {
            ArgumentNullException.ThrowIfNull(setting);

            await _settingRepository.UpdateAsync(setting);

            if (clearCache)
                await ClearCacheAsync();
        }

        public void UpdateSetting(Setting setting, bool clearCache = true)
        {
            ArgumentNullException.ThrowIfNull(setting);

            _settingRepository.Update(setting);

            if (clearCache)
                ClearCache();
        }

        public async Task DeleteSettingAsync(Setting setting)
        {
            await _settingRepository.DeleteAsync(setting);

            await ClearCacheAsync();
        }

        public void DeleteSetting(Setting setting)
        {
            _settingRepository.Delete(setting);

            ClearCache();
        }

        public async Task DeleteSettingsAsync(IList<Setting> settings)
        {
            await _settingRepository.DeleteAsync(settings);

            await ClearCacheAsync();
        }

        public async Task<Setting> GetSettingByIdAsync(int settingId)
        {
            return await _settingRepository.GetByIdAsync(settingId);
        }

        public Setting GetSettingById(int settingId)
        {
            return _settingRepository.GetById(settingId);
        }

        public async Task<Setting> GetSettingAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var settings = await GetAllSettingsDictionaryAsync();
            key = key.Trim().ToLowerInvariant();
            if (!settings.TryGetValue(key, out var value))
                return null;

            var settingsByKey = value;
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? await GetSettingByIdAsync(setting.Id) : null;
        }

        public Setting GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsDictionary();
            key = key.Trim().ToLowerInvariant();
            if (!settings.TryGetValue(key, out var value))
                return null;

            var settingsByKey = value;
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? GetSettingById(setting.Id) : null;
        }

        public async Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = await GetAllSettingsDictionaryAsync();
            key = key.Trim().ToLowerInvariant();
            if (!settings.TryGetValue(key, out var value))
                return defaultValue;

            var settingsByKey = value;
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
        }

        public T GetSettingByKey<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsDictionary();
            key = key.Trim().ToLowerInvariant();
            if (!settings.TryGetValue(key, out var value))
                return defaultValue;

            var settingsByKey = value;
            var setting = settingsByKey.FirstOrDefault();

            return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
        }

        public async Task SetSettingAsync<T>(string key, T value, bool clearCache = true)
        {
            await SetSettingAsync(typeof(T), key, value, clearCache);
        }

        public void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            SetSetting(typeof(T), key, value, clearCache);
        }

        public async Task<IList<Setting>> GetAllSettingsAsync()
        {
            var settings = await _settingRepository.GetAllAsync(query =>
            {
                return from s in query
                       orderby s.Name
                       select s;
            });

            return settings;
        }

        public IList<Setting> GetAllSettings()
        {
            var settings = _settingRepository.GetAll(query => from s in query
                                                              orderby s.Name
                                                              select s);

            return settings;
        }

        public async Task<bool> SettingExistsAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var key = GetSettingKey(settings, keySelector);

            var setting = await GetSettingByKeyAsync<string>(key);
            return setting != null;
        }

        public bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var key = GetSettingKey(settings, keySelector);

            var setting = GetSettingByKey<string>(key);
            return setting != null;
        }

        public async Task<T> LoadSettingAsync<T>() where T : ISettings, new()
        {
            return (T)await LoadSettingAsync(typeof(T));
        }

        public T LoadSetting<T>() where T : ISettings, new()
        {
            return (T)LoadSetting(typeof(T));
        }

        public async Task<ISettings> LoadSettingAsync(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                var setting = await GetSettingByKeyAsync<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        public ISettings LoadSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                var setting = GetSettingByKey<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        public async Task SaveSettingAsync<T>(T settings) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                var value = prop.GetValue(settings, null);
                if (value != null)
                    await SetSettingAsync(prop.PropertyType, key, value, false);
                else
                    await SetSettingAsync(key, string.Empty, false);
            }

            await ClearCacheAsync();
        }

        public void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                var value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(prop.PropertyType, key, value, false);
                else
                    SetSetting(key, string.Empty, false);
            }

            ClearCache();
        }

        public async Task SaveSettingAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
             bool clearCache = true) where T : ISettings, new()
        {
            if (keySelector.Body is not MemberExpression member)
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo
                           ?? throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var key = GetSettingKey(settings, keySelector);
            var value = (TPropType)propInfo.GetValue(settings, null);
            if (value != null)
                await SetSettingAsync(key, value, clearCache);
            else
                await SetSettingAsync(key, string.Empty, clearCache);
        }

        public void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new()
        {
            if (keySelector.Body is not MemberExpression member)
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo
                           ?? throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var key = GetSettingKey(settings, keySelector);
            var value = (TPropType)propInfo.GetValue(settings, null);
            if (value != null)
                SetSetting(key, value, clearCache);
            else
                SetSetting(key, string.Empty, clearCache);
        }

        public async Task DeleteSettingAsync<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = await GetAllSettingsAsync();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            await DeleteSettingsAsync(settingsToDelete);
        }

        public async Task DeleteSettingAsync<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            var key = GetSettingKey(settings, keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = await GetAllSettingsDictionaryAsync();
            var settingForCaching = allSettings.TryGetValue(key, out var settings_) ?
                settings_.FirstOrDefault() : null;
            if (settingForCaching == null)
                return;

            var setting = await GetSettingByIdAsync(settingForCaching.Id);
            await DeleteSettingAsync(setting);
        }

        public async Task ClearCacheAsync()
        {
            await _cacheManager.RemoveByPrefixAsync(EntityCacheDefaults<Setting>.Prefix);
        }

        public void ClearCache()
        {
            _cacheManager.RemoveByPrefix(EntityCacheDefaults<Setting>.Prefix);
        }

        public string GetSettingKey<TSettings, T>(TSettings settings, Expression<Func<TSettings, T>> keySelector)
            where TSettings : ISettings, new()
        {
            if (keySelector.Body is not MemberExpression member)
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            if (member.Member is not PropertyInfo propInfo)
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var key = $"{typeof(TSettings).Name}.{propInfo.Name}";

            return key;
        }

        #endregion
    }
}