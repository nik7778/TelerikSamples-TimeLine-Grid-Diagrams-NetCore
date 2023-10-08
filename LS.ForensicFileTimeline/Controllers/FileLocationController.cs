using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LS.ForensicFileTimeline.Models;
using LS.ForensicFileTimeline.Services;
using Microsoft.AspNetCore.Mvc;

namespace LS.ForensicFileTimeline.Controllers
{
    public class FileLocationController : Controller
    {
        private ITimelineService _service;
        private IFileService _fileService;

        public FileLocationController(IRecordService userService, ITimelineService service, IFileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Treemap(string remove)
        {
            ViewBag.Path = remove;
            return PartialView();
        }

        public ActionResult Files(string path)
        {
            var files = _fileService.GetAll();
            var newList = files.GroupBy(p => p.Folder).Select(p => p.ToList()).ToList();
            var results = new List<TreeMapModel>();
            TreeMapModel _files = new TreeMapModel("Files by folder", files.Count(), new List<TreeMapModel>());
            results.Add(_files);

            foreach (var items in newList)
            {
                var folder = items[0].Folder;
                if (!string.IsNullOrEmpty(path))
                    folder = folder.Replace(path, "");

                var model = new TreeMapModel(folder, items.Count(), null);
                _files.Items.Add(model);
            }
            return Json(results);
        }
    }
}