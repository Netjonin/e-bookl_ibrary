using E_BOOKLIBRARY.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Commons
{
    public class Util
    {
        public static ResponseDto<T> BuildResponse<T>(bool status, string message, ModelStateDictionary errs, T data)
        {

            var listOfErrorItems = new List<ErrorItem>();

            if (errs != null)
            {
                foreach (var err in errs)
                {
                    ///err.error.errors
                    var key = err.Key;
                    var errValues = err.Value;
                    var errList = new List<string>();
                    foreach (var errItem in errValues.Errors)
                    {
                        errList.Add(errItem.ErrorMessage);
                        listOfErrorItems.Add(new ErrorItem { Key = key, ErrorMessages = errList });
                    }
                }
            }

            var res = new ResponseDto<T>
            {
                Status = status,
                Message = message,
                Data = data,
                Errors = listOfErrorItems
            };

            return res;
        }
    }
}
