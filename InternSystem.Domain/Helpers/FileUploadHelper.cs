﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Hosting;

//namespace InternSystem.Domain.Helpers
//{
//    public class FileUploadHelper
//    {
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        private readonly static string _customDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");

//        public FileUploadHelper(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//            if (!Directory.Exists(_customDirectory))
//            {
//                Directory.CreateDirectory(_customDirectory);
//            }
//        }

//        public static async Task<string> UploadFile(IFormFile file, string id)
//        {
//            if (file.Length > 0)
//            {
//                try
//                {
//                    var fileExtension = Path.GetExtension(file.FileName); // Extract the file extension
//                    var fileName = $"{id}{fileExtension}"; // Use the provided Id with the extracted extension
//                    var filePath = Path.Combine(_customDirectory, fileName);

//                    using (var fileStream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await file.CopyToAsync(fileStream);
//                    }
//                    return fileName;
//                }
//                catch (Exception ex)
//                {
//                    return ex.Message; // Return the error message if an exception occurs
//                }
//            }
//            else
//            {
//                return "Upload failed, file is empty.";
//            }
//        }
//    }
//}
