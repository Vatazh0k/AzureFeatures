﻿using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using logic_app_test.Services;
using System;
using logic_app_test.ViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using logic_app_test.AzureModel;
using System.IO;

namespace logic_app_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly CloudTableClient _cloudTableClient;

        public HomeController(
            BlobServiceClient BlobServiceClient,
            CloudTableClient _cloudTableClient)//should i inject just a storage account and get blob and table from it?
        {
            this._blobServiceClient = BlobServiceClient;
            this._cloudTableClient = _cloudTableClient;
        }
         
        [HttpPost("UploadFileWithDescription")]
        public IActionResult UploadImageWithDescription([FromForm] FileWithDescription file)
        {
            try
            {
                AzureStorageService.AddFileToBlob(file.File, _blobServiceClient);
                AzureStorageService.AddDescriptionToTable(file, _cloudTableClient);
                return Ok("Success");//Make atomarity
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllFilesNameWithDescription")]
        public IActionResult GetFilesNameWithDescription()
        {
            try
            {
                return Ok(AzureStorageService.GetAllFilesName(_blobServiceClient));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


            [HttpGet("GetFileWithDescriptionByName")]
        public IActionResult GetFileWithDescriptionWithDescription(string fileName)
        {
            try
            {
                return File(AzureStorageService.ReadFileFromBlob(fileName, _blobServiceClient), "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
