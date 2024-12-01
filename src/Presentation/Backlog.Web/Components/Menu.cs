using AutoMapper;
using Backlog.Core.Common;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Models.Localization;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Components
{
    public class Menu : ViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IMenuService _menuService;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public Menu(IWorkContext workContext,
            IMenuService menuService,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _workContext = workContext;
            _menuService = menuService;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var employee = await _workContext.GetCurrentEmployeeAsync();
            var menusEntities = await _menuService.GetAllAsync(employee);
            var resources = await _localizationService.GetAllMenuResourceAsync(employee.LanguageId);
            var model = new MenuModel()
            {
                MenuItems = menusEntities.Select(x => _mapper.Map<MenuItemModel>(x)).ToList(),
                LocaleResources = resources.Select(x => _mapper.Map<LocaleResourceModel>(x)).ToList()
            };

            return View(model);
        }
    }
}