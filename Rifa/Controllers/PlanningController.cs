using Microsoft.AspNetCore.Mvc;
using Rifa.Models;
using Rifa.Services.Planning;

namespace Rifa.Controllers
{
    public class PlanningController : Controller
    {
        private readonly IPlanningService _planningService;

        public PlanningController(IPlanningService planningService)
        {
            _planningService = planningService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Result([FromBody] ProductionDay data)
        {
            var result = _planningService.ProcessPlanning(data);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetHistory()
        {
            var data = _planningService.GetHistory();
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetDetail(string id)
        {
            var data = _planningService.GetDetail(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdateStatus(string id, bool isActive)
        {
            var data = _planningService.UpdateStatus(id, isActive);
            return Json(data);
        }
    }
}