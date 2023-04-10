using Microsoft.AspNetCore.Mvc;
using TahalfTask.API.Models;

namespace TahalfTask.API.Service
{
    public interface IJsonService<T> where T : class
    {
        List<T> ReadJson(string path, int numberOfFiles, string title="");
        //List<PageModel> GetPagesByTitle(string path, int numberOfFiles, string title);
        byte[] ConvertToPdf(List<T> entities);
        int NumberOfFilesToRead();
    }
}
