using ArrayToPdf;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TahalfTask.API.Models;

namespace TahalfTask.API.Service
{
    public class PageModelService : IJsonService<PageModel>
    {
        private List<PageModel> pages { get; set; } = new List<PageModel>();
        public List<PageModel> ReadJson(string path, int numberOfFiles)
        {
            var dirFiles = Directory.GetFiles(path);
            foreach (var file in dirFiles.Take(numberOfFiles))
            {
                using var streamReader = new StreamReader(file);
                var res = streamReader.ReadToEnd();
                var pagesModel = JsonSerializer.Deserialize<PageModel>(res);
                pages.Add(pagesModel);
            }
            return pages;
        }
        public List<PageModel> ReadJson(string path, int numberOfFiles, string title)
        {
            var pages = ReadJson(path, numberOfFiles);
            var pagesResult = pages.Where(p => p.Title.ToLower().Contains(title.ToLower())).ToList();
            return pagesResult;
        }
        public byte[] ConvertToPdf(List<PageModel> pages)
        {
            var items = pages.Select(x => new
            {
                PageTitle = x.Title,
                PageCode = x.Code,
            });
            var pdf = items.ToPdf(schema => schema
                    .Title("Pages")
                    .ColumnName(m => m.Name.Replace("Prop", "Column #")));
            return pdf;
        }

        public int NumberOfFilesToRead() => pages.Count;
    }
}
