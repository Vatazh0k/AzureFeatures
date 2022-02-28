using logic_app_test.Services;
using logic_app_test.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace logic_app_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly AzureStorage _azureStorage;
        public StorageController(AzureStorage azureStorageService) => _azureStorage = azureStorageService;

        [HttpPost("UploadFileWithDescription")]
        public async Task <IActionResult> UploadImageWithDescription([FromForm] Document document)
        {
            try
            {
                await _azureStorage.Blob.AddFile(document.Content);
                await _azureStorage.Table.AddFileDescription(document.Description, document.Name);
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
                var blobs = await _azureStorage.Blob.GetAll();
                var result = await _azureStorage.Table.GetFilesName(blobs);
                return Ok(result);
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
                var result = await _azureStorage.Blob.ReadFile(fileName);
                return File(result, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
