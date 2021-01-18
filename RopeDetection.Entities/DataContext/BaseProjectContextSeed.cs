using RopeDetection.CommonData;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RopeDetection.Entities.DataContext
{
    public static class BaseProjectContextSeed
    {
        public static void SeedLabels(ModelContext baseProjectContext, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                // TODO: Only run this if using a real database
                // aspnetrunContext.Database.Migrate();
                // aspnetrunContext.Database.EnsureCreated();

                if (!baseProjectContext.ModelObjectTypes.Any())
                {
                    baseProjectContext.ModelObjectTypes.AddRange(GetPreconfiguredLabels());
                    baseProjectContext.SaveChangesAsync();
                }
            }
            catch
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;

                    SeedLabels(baseProjectContext, retryForAvailability);
                }
                throw;
            }
        }

        public static IEnumerable<ModelObjectType> GetPreconfiguredLabels()
        {
            return new List<ModelObjectType>()
            {
               new ModelObjectType()
               {
                   Label = "AD",
                   TextContent = ModelDictionary.Labels["AD"],
               },
               new ModelObjectType()
               {
                   Label = "BD",
                   TextContent = ModelDictionary.Labels["BD"]
               },
               new ModelObjectType()
               {
                   Label = "CD",
                   TextContent = ModelDictionary.Labels["CD"]
               },
               new ModelObjectType()
               {
                   Label = "ED",
                   TextContent = ModelDictionary.Labels["ED"]
               },
               new ModelObjectType()
               {
                   Label = "FD",
                   TextContent = ModelDictionary.Labels["FD"]
               },
               new ModelObjectType()
               {
                   Label = "GD",
                   TextContent = ModelDictionary.Labels["GD"]
               },
               new ModelObjectType()
               {
                   Label = "HD",
                   TextContent = ModelDictionary.Labels["HD"]
               },
               new ModelObjectType()
               {
                   Label = "JD",
                   TextContent = ModelDictionary.Labels["JD"]
               },
               new ModelObjectType()
               {
                   Label = "KD",
                   TextContent = ModelDictionary.Labels["KD"]
               },
               new ModelObjectType()
               {
                   Label = "LD",
                   TextContent = ModelDictionary.Labels["LD"]
               },
               new ModelObjectType()
               {
                   Label = "MD",
                   TextContent = ModelDictionary.Labels["MD"]
               },
               new ModelObjectType()
               {
                   Label = "ND",
                   TextContent = ModelDictionary.Labels["ND"]
               },
               new ModelObjectType()
               {
                   Label = "PD",
                   TextContent = ModelDictionary.Labels["PD"]
               },
               new ModelObjectType()
               {
                   Label = "QD",
                   TextContent = ModelDictionary.Labels["QD"]
               },
               new ModelObjectType()
               {
                   Label = "UW",
                   TextContent = ModelDictionary.Labels["UW"]
               }
            };
        }

       
    }
}
