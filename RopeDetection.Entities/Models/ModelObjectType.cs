using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.Models
{
    public class ModelObjectType: EntityBase
    {
        public ModelObjectType()
        {
            this.ModelObjects = new List<ModelObject>();
        }
       
        // Прогнозируемой значение(тип дефекта)
        public string Label { get; set; }
        //Расшифровка дефекта
        public string TextContent { get; set; }
        public List<ModelObject> ModelObjects { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.ModelObjectTypes.Add(this);
                db.SaveChanges();
            }
        }
    }
}
