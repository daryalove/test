using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.Models
{
    public class ModelAndObject
    {
        public ModelObject ModelObject { get; set; }
        public Guid ModelObjectId { get; set; }

        public Model Model { get; set; }
        public Guid? ModelId { get; set; }
    }
}
