using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IHistoryRepository: IRepository<AnalysisHistory>
    {
        Task<AnalysisHistory> CreateHistoryEntry(AnalysisHistory entry, AnalysisResult result = null);
        Task<AnalysisHistory> UpdateHistoryEntry(AnalysisHistory entry, BaseModel model);
    }
}
