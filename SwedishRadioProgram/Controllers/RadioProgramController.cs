using Microsoft.AspNetCore.Mvc;
using SwedishRadioProgram.Services;

namespace SwedishRadioProgram.Controllers
{
    public class RadioProgramController : Controller
    {
        private readonly SwedishRadioService _radioService;

        public RadioProgramController(SwedishRadioService radioService)
        {
            _radioService = radioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var humorPrograms = await _radioService.GetHumorProgramsWithPodcasts();
            return View(humorPrograms);
        }
    }

}
