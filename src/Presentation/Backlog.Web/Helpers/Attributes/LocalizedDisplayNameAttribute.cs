﻿using System.ComponentModel;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.ModelBinding;

namespace Backlog.Web.Helpers.Attributes
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute, IModelAttribute
    {
        #region Fields

        private string _resourceValue = string.Empty;
        protected readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        #endregion

        #region Ctor

        public LocalizedDisplayNameAttribute(string resourceKey) : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        #endregion

        #region Properties

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                _resourceValue = ResourceKey;

                ILocalizationService _localizationService = (ILocalizationService)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(ILocalizationService));
                _resourceValue = _localizationService.GetResourceAsync(ResourceKey).Result;

                return _resourceValue;
            }
        }

        public string Name => nameof(LocalizedDisplayNameAttribute);

        #endregion
    }
}
