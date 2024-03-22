﻿using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly ISocialService _socialService;
        public FooterViewComponent(ILayoutService layoutService,
                                   ISocialService socialService)
        {
            _layoutService = layoutService;
            _socialService = socialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM model = new()
            {
                Socials = await _socialService.GetSocialsAsync(),
                Settings = _layoutService.GetSettingsData()
            };

            return await Task.FromResult(View(model));

        }
    }
}
