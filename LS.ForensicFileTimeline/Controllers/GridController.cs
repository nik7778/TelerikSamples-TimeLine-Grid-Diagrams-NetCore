using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LS.ForensicFileTimeline.Entities;
using LS.ForensicFileTimeline.Helpers;
using LS.ForensicFileTimeline.Models;
using LS.ForensicFileTimeline.Services;

namespace LS.ForensicFileTimeline.Controllers
{
    public class GridController : Controller
    {
        private readonly ITimelineService service;

        public GridController(ITimelineService timelineService)
        {
            service = timelineService;
        }

        public IActionResult Index(string date)
        {
            ViewBag.Date = date;
            return View();
        }

        public IActionResult Grid(string folder, string path)
        {
            ViewBag.Folder = folder;
            ViewBag.Path = path;
            return PartialView();
        }

        public IActionResult Details(string id)
        {
            var model = service.GetAll().Where(x => x.Item == id).FirstOrDefault();
            return PartialView("_details", model);
        }

        public ActionResult Record_Read([DataSourceRequest] DataSourceRequest request, string folder, string path)
        {
            var source = service.GetAll().Where(x => x.Folder == folder).ToList();
            
            if (!string.IsNullOrEmpty(path))
            {
                foreach (var item in source)
                {
                    item.Path = item.Path.Replace(path, "");
                }
            }          
            var result = source.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs("Post")]
        public ActionResult Record_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<FileModel> model)
        {
            if (model != null && ModelState.IsValid)
                foreach (var item in model)
                {
                    //_userService.Update(_mapper.Map<Record>(user));
                }

            return Json(model.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public ActionResult Record_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<FileModel> model)
        {
            if (model.Any())
                foreach (var item in model)
                {
                    //_userService.Delete(user.Id);
                }

            return Json(model.ToDataSourceResult(request, ModelState));
        }
    }
}