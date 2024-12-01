using AutoMapper;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Localization;
using Backlog.Core.Domain.Masters;
using Backlog.Core.Domain.WorkItems;
using Backlog.Web.Models.Employees;
using Backlog.Web.Models.Localization;
using Backlog.Web.Models.Masters;
using Backlog.Web.Models.WorkItems;

namespace Backlog.Web.Helpers.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Masters

            CreateMap<Department, DepartmentModel>().ReverseMap();
            CreateMap<Designation, DesignationModel>().ReverseMap();
            CreateMap<Reporter, ReporterModel>().ReverseMap();
            CreateMap<Country, CountryModel>().ReverseMap();
            CreateMap<StateProvince, StateProvinceModel>().ReverseMap();
            CreateMap<EmailAccount, EmailAccountModel>().ReverseMap();
            CreateMap<EmailTemplate, EmailTemplateModel>().ReverseMap();
            CreateMap<Severity, SeverityModel>().ReverseMap();
            CreateMap<Status, StatusModel>().ReverseMap();
            CreateMap<TaskType, TaskTypeModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Menu, MenuItemModel>().ReverseMap();
            CreateMap<Client, ClientModel>().ReverseMap();
            CreateMap<Project, ProjectModel>().ReverseMap();
            CreateMap<Project, ProjectGridModel>().ReverseMap();
            CreateMap<EmployeeProjectMap, EmployeeProjectModel>().ReverseMap();
            CreateMap<Module, ModuleModel>().ReverseMap();
            CreateMap<SubModule, SubModuleModel>();
            CreateMap<SubModuleModel, SubModule>()
                .ForMember(dest => dest.Module, act => act.Ignore());

            #endregion

            #region Localization

            CreateMap<Language, LanguageModel>().ReverseMap();
            CreateMap<LocaleResource, LocaleResourceModel>().ReverseMap();

            #endregion

            #region Employees

            CreateMap<Employee, EmployeeModel>().ReverseMap()
                .ForMember(dest => dest.Code, act => act.Ignore());
            CreateMap<EmployeeRole, EmployeeRoleModel>().ReverseMap();
            CreateMap<EmployeeRolePermission, EmployeeRolePermissionModel>().ReverseMap();
            CreateMap<EmployeeRolePermissionMap, EmployeeRolePermissionMapModel>().ReverseMap();

            #endregion

            #region Work Items

            CreateMap<BacklogItem, BacklogItemModel>().ReverseMap()
                .ForMember(dest => dest.Code, act => act.Ignore());

            #endregion
        }
    }
}
