﻿using InternSystem.Domain.Entities;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models
{
    public class CreateThongBaoResponse : ThongBao
    {
        public string? Errors { get; set; }
    }
}