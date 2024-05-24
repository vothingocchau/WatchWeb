using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchWeb.Model.Entities;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Response;

namespace WatchWeb.Service.IServices
{
    public interface IUploadService
    {
         Task<BaseResponse<string>> UploadImageAsync(string image, string type, int refId);
        Task<BaseResponse<string>> RemoveOldImageAsync(int refId, string type);
        Task AppendFileUrl<TModel, TFile>(string moduleName, Func<TModel, int?> select,
              Action<TModel, TFile> repareModel, params TModel[] models) where TFile : DocumentOutputDto;

        Task AppendFileUrls<TModel, TFile>(string moduleName, Func<TModel, int?> select,
            Action<TModel, List<TFile>> repareModel, params TModel[] models)
            where TFile : DocumentOutputDto;
    }
}
