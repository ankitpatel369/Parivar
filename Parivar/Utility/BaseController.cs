using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parivar.Repository.Interface;
using Parivar.Utility.Common;

namespace Parivar.Utility
{
    public class BaseController<T> : Controller where T : BaseController<T>
    {
        #region Fields
        protected JsonResponse JsonResponse => new JsonResponse();

        private IErrorLogService _errorLog;
        protected IErrorLogService ErrorLog => _errorLog ?? (_errorLog = HttpContext.RequestServices.GetService<IErrorLogService>());

        private IHttpContextAccessor _accessor;
        protected IHttpContextAccessor Accessor => _accessor ?? (_accessor = HttpContext.RequestServices.GetService<IHttpContextAccessor>());

        private IWebHostEnvironment _hostingEnvironment;
        protected IWebHostEnvironment HostingEnvironment => _hostingEnvironment ?? (_hostingEnvironment = HttpContext.RequestServices.GetService<IWebHostEnvironment>());

        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());

        private IConfiguration _config;
        protected IConfiguration Config => _config ?? (_config = HttpContext.RequestServices.GetService<IConfiguration>());
        #endregion

        public string GetSortingColumnName(int sortColumnNo)
        {
            return Accessor.HttpContext.Request.Query["mDataProp_" + sortColumnNo][0];
        }

        public string GetPhysicalUrl()
        {
            return Config.GetValue<string>("CommonProperty:PhysicalUrl");
        }

        public string GetClientAppUrl()
        {
            return Config.GetValue<string>("CommonProperty:ClientAppUrl");
        }

        public string GetConfigValue(string key)
        {
            return Config.GetValue<string>(key);
        }

        public string GetS3ServiceUrl(string buketName, string fileName)
        {
            return $@"{Config.GetValue<string>("CommonProperty:S3ServiceUrl").Replace("{buketname}", buketName)}{fileName}";
        }
    }
}