using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DBIID.Shared.Results
{
    public sealed class Result<TValue> : Result
    {
        public TValue? Value { get; set; }

        [JsonConstructor]
        public Result()
        {

        }

        public static Result<TValue> Success(TValue value)
        {
            return new Result<TValue>()
            {
                IsSuccess = true,
                Status = ResultStatus.Success,
                Message = string.Empty,
                ShowNotification = false,
                Value = value
            };
        }

        public static Result<TValue> Error(string message)
        {
            return new Result<TValue>()
            {
                IsSuccess = false,
                Status = ResultStatus.Error,
                Message = message,
                ShowNotification = true,
            };
        }

        public static Result<TValue> Warning(string message)
        {
            return new Result<TValue>()
            {
                IsSuccess = false,
                Status = ResultStatus.Warning,
                Message = message,
                ShowNotification = true,
            };
        }

        public static Result<TValue> Info(string message)
        {
            return new Result<TValue>()
            {
                IsSuccess = false,
                Status = ResultStatus.Info,
                Message = message,
                ShowNotification = true,
            };
        }
    }
}
