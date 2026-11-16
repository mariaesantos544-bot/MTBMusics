using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MTBMusics.Services;

public interface IImageService
{
    bool IsConfigured { get; }
    Task<string> UploadImageAsync(IFormFile arquivo, ImageFolder pasta);
    Task<bool> DeleteImageAsync(string imageUrl);
}
