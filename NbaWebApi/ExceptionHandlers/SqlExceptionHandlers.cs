using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NbaWebApi.ExceptionHandlers
{
    public static class SqlExceptionHandlers
    {
        public static string DuplicatePrimaryKeyException(DbUpdateException ex)
        {
            var match = GetMatch(ex);
            if (match.Success)
            {
                return $"Player Id: {match.Value} already exists";
            }
            else
            {
                return $"Malformed request. Have you checked your types?";
            }
        }

        public static string DuplicateCompositePrimaryKeyException(DbUpdateException ex, int[] existing)
        {
            var match = GetMatch(ex);
            if (match.Success)
            {
                var ids = string.Join(", ", existing);
                return $"Player Id: ({ids}) already exists";
            }
            else
            {
                return $"Malformed request. Have you checked your types?";
            }
        }

        private static Match GetMatch(DbUpdateException ex)
        {
            var pattern = @"(\([0-9, ]+\))";
            var input = ex.InnerException.Message;
            return Regex.Match(input, pattern);
        }
    }
}
