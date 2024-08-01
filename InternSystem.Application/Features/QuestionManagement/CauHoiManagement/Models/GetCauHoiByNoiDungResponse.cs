﻿namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models
{
    public class GetCauHoiByNoiDungResponse
    {
        public int Id { get; set; }
        public List<string> TenCongNghe { get; set; }
        public string NoiDung { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}