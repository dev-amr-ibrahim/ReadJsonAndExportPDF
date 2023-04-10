using System.Collections.Generic;

namespace TahalfTask.API.Models
{
    public class DomainModel
    {
        public List<DomainSourceBindings> DomainSourceBindings { get; set; } 
    }

    public class DomainSourceBindings
    {
        public Entity Entity { get; set; }
    }

    public class Entity
    {
        public string Name { get; set; } = string.Empty;
        public List<Attribute> Attributes { get; set; } 
    }

    public  class Attribute
    {
        public string Name { get; set; } = string.Empty;
    }
}
