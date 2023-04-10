using Microsoft.AspNetCore.Mvc;
using TahalfTask.API.Models;
using TahalfTask.API.Service;

namespace TahalfTask.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JsonReaderController : ControllerBase
    {
        private static readonly string _pathToDomainModels = @"C:\Users\PC\Desktop\tahalaf\6126\PROJECT_NAME\PROJECT_NAME_MainModule\Core\DomainModel\DomainModels";
        private static readonly string _pathToPagesModels = @"C:\Users\PC\Desktop\tahalaf\6126\PROJECT_NAME\PROJECT_NAME_MainModule\WebClient\Pages\Pages";

        private readonly ILogger<JsonReaderController> _logger;
        private readonly IJsonService<Entity> _entityService;
        private readonly IJsonService<PageModel> _pageService;


        public JsonReaderController(ILogger<JsonReaderController> logger, IJsonService<Entity> entityService, IJsonService<PageModel> pageService)
        {
            _logger = logger;
            _entityService = entityService;
            _pageService = pageService;
        }

        /// <summary>
        /// Read Number of JSON files and return PDF file 
        /// </summary>
        /// <param name="files"></param>
        /// <returns>PDF file</returns>

        [HttpGet("ReadJsonFile")]
        public IActionResult ReadJsonFile(int files = 100)
        {

            if (files <= 0)
                return BadRequest("Number of files must be greater than 0");

            if (!Directory.Exists(_pathToDomainModels))
                return NotFound($"Not found directory for specific: {_pathToDomainModels}");

            // Read json file and get object of entities and attributes names
            var entites = _entityService.ReadJson(_pathToDomainModels, files);

            if (_entityService.NumberOfFilesToRead() == 0)
                return NotFound($"Not found files for specific directory: {_pathToDomainModels}");

            // Convert entities to PDF
            var pdfFile = _entityService.ConvertToPdf(entites);
            return File(pdfFile, "application/pdf", "domain-modedls.pdf");
        }


        [HttpGet("GetPgesByTitle")]
        public IActionResult GetPagesByTitle(string title = "dashboard", int files = 500)
        {
            if (files <= 0)
                return BadRequest("Number of files must be greater than 0");

            if (!Directory.Exists(_pathToPagesModels))
                return NotFound($"Not found directory for specific: {_pathToPagesModels}");

            // Read json file and get object of entities and attributes names
            var pages = _pageService.ReadJson(_pathToPagesModels, files, title);

            // Convert entities to PDF
            var pdfFile = _pageService.ConvertToPdf(pages);
            return File(pdfFile, "application/pdf", "page-model.pdf");
        }
    }
}