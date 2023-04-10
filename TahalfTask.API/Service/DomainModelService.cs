using ArrayToPdf;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TahalfTask.API.Models;

namespace TahalfTask.API.Service
{
    public class DomainModelService : IJsonService<Entity>
    {
        private List<Entity> entities { get; set; } = new List<Entity>();
        public List<Entity> ReadJson(string path, int numberOfFiles, string title)
        {
            var dirFiles = Directory.GetFiles(path);
            foreach (var file in dirFiles.Take(numberOfFiles))
            {
                using var streamReader = new StreamReader(file);
                var res = streamReader.ReadToEnd();
                var JsonModels = JsonSerializer.Deserialize<DomainModel>(res);

                var entitesArr = from domain in JsonModels?.DomainSourceBindings
                              where domain != null && domain.Entity != null
                              select domain.Entity;

                if(title != null)
                    entitesArr = entitesArr.Where(en => en.Name.ToLower().Contains(title.ToLower()));

                entities.AddRange(entitesArr.ToList());
            }
            return entities;
        }
        public  byte[] ConvertToPdf(List<Entity> entities)
        {
            var items = entities.Select(x => new
            {
                EntityName = x.Name,
                EntityAttributes = String.Join(", \n", x.Attributes.Select(a => a.Name)),
            });
            var pdf = items.ToPdf(schema => schema
                    .Title("Attribute Entities")
                    .ColumnName(m => m.Name.Replace("Prop", "Column #")));
            return pdf;
        }
        public int NumberOfFilesToRead() => entities.Count;

    }
}
