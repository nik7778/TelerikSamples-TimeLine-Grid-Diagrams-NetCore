using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LS.ForensicFileTimeline.Services;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LS.ForensicFileTimeline.Models;

namespace LS.ForensicFileTimeline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITimelineService _service;
        private IFileService _fileService;
        private IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IRecordService userService, IMapper mapper, ITimelineService service, IFileService fileService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _fileService = fileService;
        }

        public IActionResult Index()
        { 
            return View();
        }

        public JsonResult GetFiles()
        {
            var files = _fileService.GetAll();
            var newList = files.GroupBy(p => p.Created.Date).Select(p => p.ToList()).ToList();
            var results = new List<TimelineModel>();            

            foreach (var items in newList)
            {
                var result = new TimelineModel
                {
                    Date = items[0].Created.Date,
                    Title = items[0].Created.Date.ToString("MM/dd/yyyy"),
                    Description = $"{items.Count} files",
                    //Actions = new List<TimelineEventModelAction>() {
                    //            new TimelineEventModelAction() { 
                    //                text = "More details", 
                    //                url="/grid/index?date="+ items[0].Created.Date.ToString("MM/dd/yyyy")}
                    //          }
                };

                results.Add(result);
            }

            return Json(results);
        }
    }
}