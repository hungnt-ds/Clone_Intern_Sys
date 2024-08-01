using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Common.Services.Interfaces
{
    public interface IFileService
    {
        List<ImportDataDto> ReadFromCsv(Stream fileStream);
        List<ImportDataDto> ReadFromExcel(Stream fileStream);
        Task<string> UploadImageAsync(IFormFile file, string folderPath);
        Task DeleteImageAsync(string imageName, string folderPath);
    }
}
