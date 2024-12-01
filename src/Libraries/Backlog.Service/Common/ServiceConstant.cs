namespace Backlog.Service.Common
{
    public static class ServiceConstant
    {
        #region Permissions

        public static string PermissionsAllowedCacheKey => "wc.bl.per.allowed-{0}-{1}";

        public static string PermissionsAllByEmployeeRoleIdCacheKey => "wc.bl.per.allbyuserroleid-{0}";

        public static string PermissionsPatternCacheKey => "wc.bl.per.";

        #endregion

        #region Auth

        public static string ClaimsIssuer => "wc.bl";

        #endregion Auth

        #region Caching

        public static string PermissionsPrefixCacheKey => "wc.bl.per.";

        public static string EmployeeRolesPrefixCacheKey => "wc.bl.employeerole.";

        public static string EmployeeRolesBySystemNameCacheKey => "wc.bl.employee.systemname-{0}";

        public static string EmployeeRolesAllCacheKey => "wc.bl.employeerole.all-{0}";

        public static string EmployeeRoleIdsCacheKey => "wc.bl.employeerole.ids.{0}-{1}";

        public static string MenuCacheKey => "wc.bl.menu";

        public static string MenuResourceCacheKey => "wc.bl.menures";

        public static string LangCacheKey => "wc.bl.lang";

        public static string ActivityTypesCacheKey => "wc.bl.acttype";

        public static string LocaleStringResourcesByNameCacheKey => "wc.bl.localestringresource.byname.{0}-{1}";

        public static string GenericAttributeCacheKey => "wc.bl.genericattribute.{0}-{1}";

        public static string SettingsAllAsDictionaryCacheKey => "wc.bl.settings.all.dictionary";

        public static string AccessibleProjectCacheKey => "wc.employee.ids.projects";

        #endregion

        #region Defaults

        public static int GridDefaultPageSize = 10;

        public static int SaltKeySize = 10;

        #endregion

        #region Messages

        public static string NotificationListKey => "wc.bl.notificationList";

        public static string MessageTemplatesByNamePrefix => "wc.bl.messagetemplate.byname.{0}";

        #endregion
    }
}
