using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using RopeDetection.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.Entities.Models
{
    public class FileData: EntityBase
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
        public Guid ParentCode { get; set; }
        public int FileIndex { get; set; }
        public Parent ParentType { get; set; }
        public Guid? UserId { get; set; }

        public User User { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.FileDatas.Add(this);
                db.SaveChanges();
            }
        }

        public static FileData Create(Guid fileId, Guid parentCode, string fileName, string fileType, byte[] fileContent, Parent parentType, Guid userId, int fileIndex = 0)
        {
            var file = new FileData
            {
                Id = fileId,
                FileContent = fileContent,
                FileIndex = fileIndex,
                FileName = fileName,
                FileType = fileType,
                ParentCode = parentCode,
                ParentType = parentType,
                UserId = userId
            };
            return file;
        }
    }
}
