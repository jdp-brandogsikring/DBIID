using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DBIID.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public bool ShowNotification { get; set; } = true;

        [JsonConstructor]
        protected Result()
        {
            IsSuccess = true;
        }

        public static Result Success(string message)
        {
            return new Result()
            {
                IsSuccess = true,
                Status = ResultStatus.Success,
                Message = message
            };
        }

        public static Result Error(string message)
        {
            return new Result()
            {
                IsSuccess = true,
                Status = ResultStatus.Error,
                Message = message
            };
        }

        public static Result Warning(string message)
        {
            return new Result()
            {
                IsSuccess = true,
                Status = ResultStatus.Warning,
                Message = message
            };
        }

        public static Result Info(string message)
        {
            return new Result()
            {
                IsSuccess = true,
                Status = ResultStatus.Info,
                Message = message
            };
        }

        public static Result ValidationError(string errors)
        {
            return new Result()
            {
                IsSuccess = false,
                Status = ResultStatus.ValidationError,
                Message = errors,
                ShowNotification = false
            };
        }
    }
}
