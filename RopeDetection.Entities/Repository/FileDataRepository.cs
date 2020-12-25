using Microsoft.EntityFrameworkCore;
using RopeDetection.CommonData.ViewModels.FileViewModel;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class FileDataRepository : Repository<FileData>, IFileRepository
    {
        public FileDataRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        //загрузка файлов для обучения
        public async Task<IEnumerable<ImageByteContent>> GetFilesContentForTraining(Guid modelId)
        {
            Model model = await _dbContext.Models.Include(s => s.ModelAndObjects).
                ThenInclude(s => s.ModelObject).ThenInclude(s => s.ModelObjectType)
                .FirstOrDefaultAsync(p => p.Id == modelId);

            if (model == null)
                throw new Exception("Модель не найдена. Просьба создать новую модель.");

            var files = await _dbContext.FileDatas.Where(s => s.ParentType == CommonData.ModelEnums.Parent.ModelObject).ToListAsync();
            var images = new List<ImageByteContent>();

            foreach (var model_obj in model.ModelAndObjects)
            {
                var file = files.Where(s => s.ParentCode == model_obj.ModelObject.Id).FirstOrDefault();

                if ((file.FileType != ".jpg") && (file.FileType != ".png"))
                    continue;

                images.Add(new ImageByteContent()
                {
                    ImageContent = file.FileContent,
                    ImageName = file.FileName,
                    Label = model_obj.ModelObject.ModelObjectType.Label,
                    FileType = file.FileType
                });
            }
            return images;
        }
    }
}
