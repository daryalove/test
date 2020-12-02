using RopeDetection.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.Models
{
    public class User: EntityBase
    {
        public User()
        {
            AnalyzedObjects = new List<AnalyzedObject>();
            FileDatas = new List<FileData>();
            Models = new List<Model>();
        }

        public string UserFIO { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Признак допуска в систему
        /// </summary>
        public bool IsAllowed { get; set; }

        public List<AnalyzedObject> AnalyzedObjects { get; private set; }
        public List<FileData> FileDatas { get; private set; }
        public List<Model> Models { get; private set; }


        public static User Create(string userFIO, string Email, byte[] passwordHash, byte[] passwordSalt, string UserName = null, bool IsAllowed = false, string description = null)
        {
            var user = new User
            {
                UserName = Email,
                UserFIO = userFIO,
                Description = description,
                Email = Email,
                IsAllowed = IsAllowed,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            return user;
        }
    }
}
