using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LS.ForensicFileTimeline.Models;
using LS.ForensicFileTimeline.Services;
using Microsoft.AspNetCore.Mvc;

namespace LS.ForensicFileTimeline.Controllers
{
    public class LastAccessedController : Controller
    {
        private ITimelineService _service;
        private IFileService _fileService;
        private IMapper _mapper;
        string[] images = { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "jfif" };
        string[] videos = { "mp4", "mp3", "avi", "flv", "mov", "mpeg-4", "mkv", "avchd", "webm" };

        public LastAccessedController(IRecordService userService, IMapper mapper, ITimelineService service, IFileService fileService)
        {
            _service = service;
            _mapper = mapper;
            _fileService = fileService;
        }


        public IActionResult Index()
        {
            ViewBags(null);
            return View();
        }

        public IActionResult Timeline(bool v1, bool v2, bool v3, bool v4)
        {
            ViewBag.Images = v1;
            ViewBag.Videos = v2;
            ViewBag.Other = v3;
            ViewBag.NoDate = v4;            
            return PartialView("Timeline");
        }

        public IActionResult Grid(string date, bool v1, bool v2, bool v3, string path)
        {
            ViewBag.Date = date;
            ViewBag.Images = v1;
            ViewBag.Videos = v2;
            ViewBag.Other = v3;
            ViewBag.Path = path;
            ViewBags(date);
            return PartialView("Grid");
        }

        public IActionResult GridPartial(string date, bool v1, bool v2, bool v3, string path)
        {
            ViewBag.Date = date;
            ViewBag.Images = v1;
            ViewBag.Videos = v2;
            ViewBag.Other = v3;
            ViewBag.Path = path;
            return PartialView("GridPartial");
        }

        public ActionResult Record_Read([DataSourceRequest] DataSourceRequest request, string date, bool img, bool vid, bool other, string path)
        {
            if (img || vid || other)
            {
                var filterDate = date != null ? Convert.ToDateTime(date).Date : DateTime.Now.Date;
                var files = _fileService.GetAll().Where(x => x.Accessed.Date == filterDate);

                if (!(img && vid && other))
                {
                    if (img && vid)
                        files = files.Where(x => images.Contains(x.Extension.ToLower()) || videos.Contains(x.Extension.ToLower())).ToList();
                    else if (img && other)
                        files = files.Where(x => images.Contains(x.Extension.ToLower()) || (!images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower()))).ToList();
                    else if (vid && other)
                        files = files.Where(x => videos.Contains(x.Extension.ToLower()) || (!images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower()))).ToList();
                    else if (vid)
                        files = files.Where(x => videos.Contains(x.Extension.ToLower())).ToList();
                    else if (other)
                        files = files.Where(x => !images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower())).ToList();
                    else if (img)
                        files = files.Where(x => images.Contains(x.Extension.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(path))
                {
                    foreach (var item in files)
                    {
                        item.Path = item.Path.Replace(path, "");
                    }
                }

                var result = files.ToDataSourceResult(request);
                return Json(result);
            }

            return Json(null);
        }

        public JsonResult GetFiles(bool img, bool vid, bool other, bool nodate)
        {
            if (img || vid || other || nodate)
            {
                var files = _fileService.GetAll().Where(x => x.Accessed != DateTime.Now.Date);

                if (nodate)
                    files = _fileService.GetAll();

                if (!(img && vid && other))
                {
                    if (img && vid)
                        files = files.Where(x => images.Contains(x.Extension.ToLower()) || videos.Contains(x.Extension.ToLower()));
                    else if (img && other)
                        files = files.Where(x => images.Contains(x.Extension.ToLower()) || (!images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower())));
                    else if (vid && other)
                        files = files.Where(x => videos.Contains(x.Extension.ToLower()) || (!images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower())));
                    else if (vid)
                        files = files.Where(x => videos.Contains(x.Extension.ToLower()));
                    else if (other)
                        files = files.Where(x => !images.Contains(x.Extension.ToLower()) && !videos.Contains(x.Extension.ToLower()));
                    else if (img)
                        files = files.Where(x => images.Contains(x.Extension.ToLower()));
                }

                var newList = files.GroupBy(p => p.Accessed.Date).Select(p => p.ToList()).ToList();
                var results = new List<TimelineModel>();

                foreach (var items in newList)
                {
                    var result = new TimelineModel
                    {
                        Date = items[0].Accessed.Date,
                        Title = items[0].Accessed.Date.ToString("MM/dd/yyyy"),
                        Description = $"{items.Count} files",
                        Actions = new List<TimelineEventModelAction>() {
                                new TimelineEventModelAction() {
                                    text = "More details",
                                    url="/grid/index?date="+ items[0].Accessed.Date.ToString("MM/dd/yyyy")}
                              }
                    };

                    results.Add(result);
                }

                return Json(results);
            }

            return Json(null);
        }

        public void ViewBags(string date)
        {
            var files = _fileService.GetAll();

            if (!string.IsNullOrEmpty(date))
                files = _fileService.GetAll().Where(x => x.Accessed.Date == Convert.ToDateTime(date).Date).ToList();

            var imgNr = files.Where(x => images.Contains(x.Extension.ToLower())).Count();
            var videoNr = files.Where(x => videos.Contains(x.Extension.ToLower())).Count();
            var noDateNr = files.Where(x => x.Accessed == DateTime.Now.Date).Count();
            var filesNr = files.Count();
            var otherNr = filesNr - imgNr - videoNr;

            ViewBag.TotalFiles = filesNr;
            ViewBag.Images = imgNr;
            ViewBag.ImagesPrecent = (imgNr * 100) / filesNr;
            ViewBag.Videos = videoNr;
            ViewBag.VideosPrecent = (videoNr * 100) / filesNr;
            ViewBag.Other = otherNr;
            ViewBag.OtherPrecent = (otherNr * 100) / filesNr;
            ViewBag.NoDate = noDateNr;
            ViewBag.NoDatePrecent = (noDateNr * 100) / filesNr;
            ViewBag.WithDate = filesNr - noDateNr;
            ViewBag.WithDatePrecent = ((filesNr - noDateNr) * 100) / filesNr;
            ViewBag.Img0 = files.Where(x => x.Extension.ToLower() == images[0]).Count();
            ViewBag.Img1 = files.Where(x => x.Extension.ToLower() == images[1]).Count();
            ViewBag.Img2 = files.Where(x => x.Extension.ToLower() == images[2]).Count();
            ViewBag.Img3 = files.Where(x => x.Extension.ToLower() == images[3]).Count();
            ViewBag.Vid0 = files.Where(x => x.Extension.ToLower() == videos[0]).Count();
            ViewBag.Vid1 = files.Where(x => x.Extension.ToLower() == videos[1]).Count();
            ViewBag.Vid2 = files.Where(x => x.Extension.ToLower() == videos[2]).Count();
        }
    }
}
