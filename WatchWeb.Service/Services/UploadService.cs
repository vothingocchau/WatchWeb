using AutoMapper;
using AutoMapper.Internal.Mappers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;
using WatchWeb.Common.Constant;
using WatchWeb.Model.EFContext;
using WatchWeb.Model.Entities;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Dto;
using WatchWeb.Service.Models.Response;
using WatchWeb.Service.Settings;
using Image = SixLabors.ImageSharp.Image;

namespace WatchWeb.Service.Services
{
    public class UploadService : IUploadService
    {
        private readonly DataContext _dataContext;
        private Cloudinary _account;
        private readonly IMapper _mapper;

        public UploadService(ICloundinarySetting cloundinarySetting,
            DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            var myAccount = new Account
            {
                ApiKey = cloundinarySetting.ApiKey,
                ApiSecret = cloundinarySetting.ApiSecret,
                Cloud = cloundinarySetting.CloudName
            };

            _account = new Cloudinary(myAccount);
            _mapper = mapper;
        }


        public async Task<BaseResponse<string>> UploadImageAsync(string image, string type, int refId)
        {
            var result = new BaseResponse<string>();

            var validateImage = await isValidUploadImage(image);

            if (!validateImage.IsSuccess)
            {
                return result.Failed(validateImage.Message, string.Empty);
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image)
            };

            var uploadResult = await _account.UploadLargeAsync(uploadParams);

            if (uploadResult == null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_EMPTY_DATA, string.Empty);
            }

            var document = new Model.Entities.Document
            {
                ReferenceId = refId,
                Type = type,
                Status = 1,
                Description = string.Empty,
                Url = uploadResult.Url.ToString()
            };
            await _dataContext.Document.AddAsync(document);
            await _dataContext.SaveChangesAsync();
            return result.Success(MessageResponseConstant.SUCCESSFULLY, uploadResult.Url.ToString());
        }

        private async Task<BaseResponse<string>> isValidUploadImage(string image)
        {
            var result = new BaseResponse<string>();
            var imagebase64 = IsBase64(image);
            if (!imagebase64)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_INVALID_BASE64_STRING, string.Empty);
            }

            Match match = DataImage.DataUriPattern.Match(image);
            string base64Data = match.Groups["data"].Value;
            var imageData = Convert.FromBase64String(base64Data);
            if (imageData == null)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_INVALID_BASE64_STRING, string.Empty);
            }

            var imageOversize = ConvertSizeToMB(image.Length) > 3;
            if (imageOversize)
            {
                return result.Failed(MessageResponseConstant.ERR_MSG_UPLOAD_FILE_SIZE_OVER_MAXIMUM, string.Empty);
            }

            var imgIcon = BytesToImage(imageData, out IImageFormat format);
            if (imgIcon == null)
            {
                return result.Failed("Lỗi upload hình ảnh", string.Empty);
            }

            var imgFormat = format.Name.ToLower();
            if (imgFormat != ImageFormatConstant.JPEG && imgFormat != ImageFormatConstant.PNG)
            {
                return result.Failed("Lỗi upload hình ảnh", string.Empty);
            }

            return result.Success(MessageResponseConstant.SUCCESSFULLY, string.Empty);
        }

        private double ConvertSizeToMB(int dataSizeInBytes)
        {
            return (double)dataSizeInBytes / ImageFormatConstant.MB_SIZE;
        }

        private Image BytesToImage(byte[] imgBytes, out IImageFormat format)
        {
            Stream outputStream = new MemoryStream();

            using (var image = Image.Load(imgBytes, out format))
            {
                image.Mutate(c => c.Grayscale());
                image.Save(outputStream, format);


                return image;
            }
        }

        public static bool IsBase64(string base64String)
        {
            DataImage image = DataImage.TryParse(base64String);
            if (image == null) return false;

            return true;
        }

        public async Task AppendFileUrl<TModel, TFile>(string typeName, Func<TModel, int?> select, Action<TModel, TFile> repareModel, params TModel[] models) where TFile : DocumentOutputDto
        {
            if (models != null && models.Any())
            {
                var referenceIds = models.Select(x => Convert.ToInt32(select.Invoke(x))).ToList();
                var files = await _dataContext.Document.Where(x => x.Type == typeName.ToUpper() && referenceIds.Contains(x.ReferenceId) && x.Status == 1).ToListAsync();
                foreach (var model in models)
                {
                    var item = files.FirstOrDefault(i => i.ReferenceId.ToString()?.ToUpper() == select.Invoke(model).ToString()?.ToUpper());
                    if (item != null)
                    {
                        var fileModel = _mapper.Map<TFile>(item);
                        repareModel.Invoke(model, fileModel);
                    }
                    else
                        repareModel.Invoke(model, null);
                }
            }
        }

        public async Task AppendFileUrls<TModel, TFile>(string typeName, Func<TModel, int?> select, Action<TModel, List<TFile>> repareModel, params TModel[] models) where TFile : DocumentOutputDto
        {
            if (models != null && models.Any())
            {
                var referenceIds = models.Select(x => Convert.ToInt32(select.Invoke(x))).Distinct().ToList();
                var files = await _dataContext.Document
                    .Where(x => x.Type == typeName.ToUpper() && referenceIds.Contains(x.ReferenceId) && x.Status == 1)
                    .ToListAsync();
                foreach (var model in models)
                {
                    var items = files.Where(i => i.ReferenceId.ToString().ToUpper() == select.Invoke(model).ToString().ToUpper()).ToList();
                    var fileModels = new List<TFile>();
                    BindingFileModel(items, fileModels);
                    repareModel.Invoke(model, _mapper.Map<List<TFile>>(fileModels));
                }
            }
        }

        #region private Class

        private void BindingFileModel<TFile>(List<Document> items, List<TFile> fileModels) where TFile : DocumentOutputDto
        {
            foreach (var item in items)
            {
                var fileModel = _mapper.Map<TFile>(item);
                fileModels.Add(fileModel);
            }
        }

        public async Task<BaseResponse<string>> RemoveOldImageAsync(int refId, string type)
        {
            var result = new BaseResponse<string>();
            var image = await _dataContext.Document.Where(x => x.ReferenceId == refId && x.Type == type && x.Status == 1).FirstOrDefaultAsync();
            if (image == null) return result.Failed(MessageResponseConstant.NOTFOUND, string.Empty);
            image.Status = 0;
            _dataContext.Document.Update(image);
            await _dataContext.SaveChangesAsync();
            return result.Success(MessageResponseConstant.SUCCESSFULLY, image.Url);
        }

        public sealed class DataImage
        {
            public static readonly Regex DataUriPattern = new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|jpeg|gif|webp));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

            private DataImage()
            {
            }

            public static DataImage TryParse(string dataUri)
            {
                if (string.IsNullOrWhiteSpace(dataUri)) return null;

                Match match = DataUriPattern.Match(dataUri);
                if (!match.Success) return null;

                string base64Data = match.Groups["data"].Value;

                try
                {
                    byte[] rawData = Convert.FromBase64String(base64Data);
                    return rawData.Length == 0 ? null : new DataImage();
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }

        #endregion private Class

    }
}

