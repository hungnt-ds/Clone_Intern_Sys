using InternSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models
{
    public class GetPagedReportTasksResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string HoVaTen { get; set; }
        public int TaskId { get; set; }
        public string MoTa { get; set; }
        public string NoiDungBaoCao { get; set; }
        public DateTime NgayBaoCao { get; set; }
        public string TrangThai { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
