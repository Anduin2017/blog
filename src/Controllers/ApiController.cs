﻿using Aiursoft.Blog.Models;
using Aiursoft.Pylon.Attributes;
using Aiursoft.Pylon.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Aiursoft.Blog.Controllers
{
    [LimitPerMin]
    [APIExpHandler]
    [APIModelStateChecker]
    public class ApiController : Controller
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly ServiceLocation _serviceLocation;
        public ApiController(
            UserManager<BlogUser> userManager,
            ServiceLocation serviceLocation)
        {
            _userManager = userManager;
            _serviceLocation = serviceLocation;
        }

        [Route("/manifest.json")]
        public async Task<IActionResult> Manifest()
        {
            var owner = (await _userManager.GetUsersInRoleAsync(Consts.OwnerRoleName))
                .First();
            var descriptionAttribute = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            var model = new ManifestModel
            {
                ShortName = owner.NickName,
                Name = owner.NickName + "'s Blog",
                Description = descriptionAttribute,
                StartUrl = "/",
                Icons = new List<ManifestIcon>()
                {
                    new ManifestIcon
                    {
                        Src = StorageService.GetProbeDownloadAddress(_serviceLocation, owner.IconFilePath) + "?w=48&h=48",
                        Sizes = "48x48",
                        Type = "image/png"
                    },
                    new ManifestIcon
                    {
                        Src = StorageService.GetProbeDownloadAddress(_serviceLocation, owner.IconFilePath) + ".png?w=72&h=72",
                        Sizes = "72x72",
                        Type = "image/png"
                    },
                    new ManifestIcon
                    {
                        Src = StorageService.GetProbeDownloadAddress(_serviceLocation, owner.IconFilePath) + ".png?w=144&h=144",
                        Sizes = "144x144",
                        Type = "image/png"
                    },
                    new ManifestIcon
                    {
                        Src = StorageService.GetProbeDownloadAddress(_serviceLocation, owner.IconFilePath) + ".png?w=240&h=240",
                        Sizes = "240x240",
                        Type = "image/png"
                    },
                    new ManifestIcon
                    {
                        Src = StorageService.GetProbeDownloadAddress(_serviceLocation, owner.IconFilePath) + ".png?w=512&h=512",
                        Sizes = "512x512",
                        Type = "image/png"
                    }
                },
                BackgroundColor = "#3097D1",
                ThemeColor = "#3097D1",
                Display = "standalone",
                Orientation = "portrait"
            };
            return Json(model);
        }
    }
}
