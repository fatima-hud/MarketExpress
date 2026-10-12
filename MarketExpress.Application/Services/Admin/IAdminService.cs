using Azure.Core;
using MarketExpress.Application.Dto.Role.Request;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Admin
{
  public  interface IAdminService
    {
    //    Task<Result> AddRole(string adminId,RequestAddRoleDto request);
      //  Task<Result> RemoveRole(string adminId ,RequestAddRoleDto request);
        Task<Result<ResponseDashboardSummaryDto>> GetDashboardSummaryAsync(string adminId);
        Task<Result<ResponseDashboardAnalyticsDto>>GetDashboardAnalyticsAsync(string adminId, DateTime? fromDate, DateTime? endDate);
    }
}
