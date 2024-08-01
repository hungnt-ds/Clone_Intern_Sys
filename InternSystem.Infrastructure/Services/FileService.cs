using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public List<ImportDataDto> ReadFromCsv(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<ImportDataDto>().ToList();
            }
        }

        public List<ImportDataDto> ReadFromExcel(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                });

                DataTable dataTable = dataSet.Tables[0];

                var dtos = new List<ImportDataDto>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var dto = new ImportDataDto
                    {
                        Email = GetValueOrDefault<string>(row, 0),
                        EmailConfirmed = GetValueOrDefault<bool>(row, 1),
                        PhoneNumber = GetValueOrDefault<string>(row, 2),
                        PhoneNumberConfirmed = GetValueOrDefault<bool>(row, 3),
                        IsConfirmed = GetValueOrDefault<bool>(row, 4),
                        TrangThaiThucTap = GetValueOrDefault<string>(row, 5),
                        HoTen = GetValueOrDefault<string>(row, 6),
                        NgaySinh = GetValueOrDefault<DateTime>(row, 7),
                        GioiTinh = GetValueOrDefault<bool>(row, 8),
                        MSSV = GetValueOrDefault<string>(row, 9),
                        EmailTruong = GetValueOrDefault<string>(row, 10),
                        SdtNguoiThan = GetValueOrDefault<string>(row, 11),
                        DiaChi = GetValueOrDefault<string>(row, 12),
                        GPA = GetValueOrDefault<decimal>(row, 13),
                        TrinhDoTiengAnh = GetValueOrDefault<string>(row, 14),
                        LinkFacebook = GetValueOrDefault<string>(row, 15),
                        LinkCv = GetValueOrDefault<string>(row, 16),
                        NganhHoc = GetValueOrDefault<string>(row, 17),
                        TrangThai = GetValueOrDefault<string>(row, 18),
                        Round = GetValueOrDefault<int>(row, 19),
                        ViTriMongMuon = GetValueOrDefault<string>(row, 20),
                        IdTruong = GetValueOrDefault<int>(row, 21)
                    };

                    dtos.Add(dto);
                }

                return dtos;
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Không có tệp nào được tải lên.");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public async Task DeleteImageAsync(string imageName, string folderPath)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, imageName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            await Task.CompletedTask;
        }

        private T GetValueOrDefault<T>(DataRow row, int index)
        {
            object? value = row.ItemArray[index];

            if (value == null || value == DBNull.Value)
            {
                return default(T); // Return default value
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"Cannot cast value '{value}' of type '{value.GetType()}' to type '{typeof(T)}'.", ex);
            }
        }
    }
}
