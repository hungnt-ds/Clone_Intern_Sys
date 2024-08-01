﻿namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models
{
    public class GetAllDuAnResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string LeaderId { get; set; }
        public string LeaderName { get; set; }

        public List<string> TenCongNghe { get; set; }
        public DateTimeOffset ThoiGianBatDau { get; set; }
        public DateTimeOffset ThoiGianKetThuc { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
