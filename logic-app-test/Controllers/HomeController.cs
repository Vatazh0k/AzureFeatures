using logic_app_test.Services;
using logic_app_test.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace logic_app_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AzureStorageService _azureStorage;
        public HomeController(AzureStorageService azureStorageService) => _azureStorage = azureStorageService;

        [HttpPost("UploadFileWithDescription")]
        public async Task <IActionResult> UploadImageWithDescription([FromForm] Document document)
        {
            try
            {
                await _azureStorage.AddFileToBlob(document.Content);
                await _azureStorage.AddFileDescriptionToTable(document.Description, document.Name);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllFilesNameWithDescription")]
        public async Task<IActionResult> GetFilesNameWithDescription()
        { 
            try
            {
                return Ok(await _azureStorage.GetAllFilesName());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetFileWithDescriptionByName")]
        public async Task<IActionResult> GetFileWithDescriptionWithDescription(string fileName)
        {
            try
            {
                return File(await _azureStorage.ReadFileFromBlob(fileName), "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
