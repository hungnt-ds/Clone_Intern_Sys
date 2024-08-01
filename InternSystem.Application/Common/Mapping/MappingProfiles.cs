using AutoMapper;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Models;
using InternSystem.Application.Features.ClaimManagement.Commands;
using InternSystem.Application.Features.ClaimManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Commands;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Application.Features.InternManagement.CommentManagement.Commands;
using InternSystem.Application.Features.InternManagement.CommentManagement.Models;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Commands;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Commands;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Commands;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Commands;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using InternSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Common.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Auth Mappings
            CreateMap<AspNetUser, GetUserDetailResponse>();
            //CreateMap<AspNetUser, GetAllUserResponse>().ReverseMap();
            CreateMap<AspNetUser, CreateUserCommand>().ReverseMap();
            CreateMap<AspNetUser, GetCurrentUserResponse>();
            CreateMap<AspNetUser, CreateUserResponse>().ReverseMap();
            CreateMap<IdentityRole, GetRoleResponse>().ReverseMap();
            CreateMap<KyThucTap, GetKyThucTapsByNameResponse>().ReverseMap();
            CreateMap<TruongHoc, GetTruongHocByNameResponse>().ReverseMap();
            CreateMap<AspNetUser, GetPagedUsersResponse>()
                .ForMember(dest => dest.ListDuAn, opt => opt.MapFrom(src => src.UserDuAns
                    .Where(ud => ud.DuAn != null)
                    .Select(ud => ud.DuAn.Ten)
                    .ToList()))
                .ForMember(dest => dest.ListViTri, opt => opt.MapFrom(src => src.UserViTris
                    .Where(uv => uv.ViTri != null)
                    .Select(uv => uv.ViTri.Ten)
                    .ToList()))
                .ForMember(dest => dest.ListNhomZalo, opt => opt.MapFrom(src => src.UserNhomZalos
                    .Where(unz => unz.NhomZaloChung != null)
                    .Select(unz => unz.NhomZaloChung.TenNhom)
                    .Concat(src.UserNhomZalos
                        .Where(unz => unz.NhomZaloRieng != null)
                        .Select(unz => unz.NhomZaloRieng.TenNhom))
                    .ToList()))
                .ReverseMap();
            CreateMap<IdentityRole, GetPagedRolesResponse>().ReverseMap();
            CreateMap<AspNetUser, GetAspNetUserResponse>().ReverseMap();
            CreateMap<AspNetUser, GetAllUserResponse>()
                .ForMember(dest => dest.ListDuAn, opt => opt.MapFrom(src => src.UserDuAns
                    .Where(ud => ud.DuAn != null)
                    .Select(ud => ud.DuAn.Ten)
                    .ToList()))
                .ForMember(dest => dest.ListViTri, opt => opt.MapFrom(src => src.UserViTris
                    .Where(uv => uv.ViTri != null)
                    .Select(uv => uv.ViTri.Ten)
                    .ToList()))
                .ForMember(dest => dest.ListNhomZalo, opt => opt.MapFrom(src => src.UserNhomZalos
                    .Where(unz => unz.NhomZaloChung != null)
                    .Select(unz => unz.NhomZaloChung.TenNhom)
                    .Concat(src.UserNhomZalos
                        .Where(unz => unz.NhomZaloRieng != null)
                        .Select(unz => unz.NhomZaloRieng.TenNhom))
                    .ToList()));

            // Interview Mappings
            CreateMap<Comment, GetDetailCommentResponse>()
                .ForMember(dest => dest.TenNguoiComment, opt => opt.MapFrom(src => src.NguoiComment.HoVaTen))
                .ForMember(dest => dest.TenNguoiDuocComment, opt => opt.MapFrom(src => src.NguoiDuocComment.HoTen))
                .ReverseMap();
            CreateMap<Comment, CreateCommentCommand>().ReverseMap();
            CreateMap<Comment, UpdateCommentCommand>().ReverseMap();

            // Report Mappings

            // Communication Mappings
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest); //convert int? to int

            // InternInfo mapping
            CreateMap<InternInfo, GetInternInfoByIdResponse>();
            CreateMap<InternInfo, GetInternInfoResponse>().ReverseMap();
            CreateMap<InternInfo, CreateInternInfoCommand>().ReverseMap();
            CreateMap<InternInfo, CreateInternInfoResponse>();
            CreateMap<InternInfo, UpdateInternInfoCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<InternInfo, SelfUpdateInternInfoCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<InternInfo, UpdateInternInfoResponse>();
            CreateMap<InternInfo, GetPagedInternInfosResponse>().ReverseMap();
            CreateMap<InternInfo, GetFilteredInternInfoByDayResponse>()
                .ForMember(dest => dest.TenTruong, opt => opt.MapFrom(src => src.TruongHoc.Ten))
                .ForMember(dest => dest.TenKyThucTap, opt => opt.MapFrom(src => src.KyThucTap.Ten))
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ReverseMap();

            CreateMap<EmailUserStatus, GetDetailEmailUserStatusResponse>().ReverseMap();
            CreateMap<EmailUserStatus, CreateEmailUserStatusCommand>().ReverseMap();
            CreateMap<EmailUserStatus, UpdateEmailUserStatusCommand>().ReverseMap();

            // Csv Mappings
            CreateMap<ImportDataDto, AspNetUser>().ReverseMap();
            CreateMap<ImportDataDto, InternInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            // TruongHoc mapping
            CreateMap<TruongHoc, GetTruongHocByIdResponse>();
            CreateMap<TruongHoc, CreateTruongHocCommand>().ReverseMap();
            CreateMap<TruongHoc, CreateTruongHocResponse>();
            CreateMap<TruongHoc, UpdateTruongHocCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TruongHoc, UpdateTruongHocResponse>();
            CreateMap<TruongHoc, GetAllTruongHocResponse>();
            CreateMap<TruongHoc, GetPagedTruongHocsResponse>().ReverseMap();

            // KyThucTap Mapping
            CreateMap<KyThucTap, CreateKyThucTapCommand>().ReverseMap();
            CreateMap<KyThucTap, CreateKyThucTapResponse>();
            CreateMap<KyThucTap, GetKyThucTapByIdResponse>()
                 .ForMember(dest => dest.TenTruong, opt => opt.MapFrom(src => src.TruongHoc.Ten));
            CreateMap<KyThucTap, GetAllKyThucTapResponse>()
                 .ForMember(dest => dest.TenTruong, opt => opt.MapFrom(src => src.TruongHoc.Ten));
            CreateMap<KyThucTap, GetKyThucTapByNameResponse>()
                .ForMember(dest => dest.TenTruong, opt => opt.MapFrom(src => src.TruongHoc.Ten));
            CreateMap<KyThucTap, UpdateKyThucTapCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<KyThucTap, UpdateKyThucTapResponse>();
            CreateMap<KyThucTap, GetPagedKyThucTapsResponse>().ReverseMap();

            // DuAn mapping
            CreateMap<DuAn, GetDuAnByIdResponse>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.HoVaTen))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNgheDuAns.Select(cnda => cnda.CongNghe.Ten).ToList()))
                .ReverseMap();
            CreateMap<DuAn, CreateDuAnCommand>().ReverseMap();
            CreateMap<DuAn, CreateDuAnResponse>();
            CreateMap<DuAn, UpdateDuAnCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DuAn, UpdateDuAnResponse>();
            CreateMap<DuAn, GetAllDuAnResponse>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.HoVaTen))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNgheDuAns.Select(cnda => cnda.CongNghe.Ten).ToList()))
                .ReverseMap();
            CreateMap<DuAn, GetDuAnByTenResponse>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.HoVaTen))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNgheDuAns.Select(cnda => cnda.CongNghe.Ten).ToList()))
                .ReverseMap();
            CreateMap<DuAn, GetPagedDuAnsResponse>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.HoVaTen))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNgheDuAns.Select(cnda => cnda.CongNghe.Ten).ToList()))
                .ReverseMap();


            // CauHoi mapping
            CreateMap<CauHoi, GetCauHoiByIdResponse>();
            CreateMap<CauHoi, GetAllCauHoiResponse>()
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CauHoiCongNghes.Select(chcn => chcn.CongNghe.Ten).ToList()))
                .ReverseMap();
            CreateMap<CauHoi, CreateCauHoiCommand>().ReverseMap();
            CreateMap<CauHoi, CreateCauHoiResponse>();
            CreateMap<CauHoi, UpdateCauHoiCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CauHoi, UpdateCauHoiResponse>();
            CreateMap<CauHoi, GetCauHoiByNoiDungResponse>()
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CauHoiCongNghes.Select(chcn => chcn.CongNghe.Ten).ToList()))
                .ReverseMap();
            CreateMap<CauHoi, GetPagedCauHoisResponse>().ReverseMap();

            // CauHoiCongNghe mapping
            CreateMap<CauHoiCongNghe, GetCauHoiCongNgheByIdResponse>()
             .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src =>  src.CongNghe.Ten ))
             .ForMember(dest => dest.NoiDungcauHoi, opt => opt.MapFrom(src => src.CauHoi.NoiDung ))
             .ReverseMap();

            CreateMap<CauHoiCongNghe, GetAllCauHoiCongNgheResponse>()
             .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNghe.Ten))
             .ForMember(dest => dest.NoiDungcauHoi, opt => opt.MapFrom(src => src.CauHoi.NoiDung))
            .ReverseMap();

            CreateMap<CauHoiCongNghe, CreateCauHoiCongNgheCommand>().ReverseMap();
            CreateMap<CauHoiCongNghe, CreateCauHoiCongNgheResponse>();
            CreateMap<CauHoiCongNghe, UpdateCauHoiCongNgheCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CauHoiCongNghe, UpdateCauHoiCongNgheResponse>();
            CreateMap<CauHoiCongNghe, GetPagedCauHoiCongNghesResponse>()
                 .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNghe.Ten))
             .ForMember(dest => dest.NoiDungcauHoi, opt => opt.MapFrom(src => src.CauHoi.NoiDung))
             .ReverseMap();

            // PhongVan mapping
            CreateMap<PhongVan, CreatePhongVanCommand>().ReverseMap();
            CreateMap<PhongVan, CreatePhongVanResponse>();
            CreateMap<PhongVan, GetPhongVanByIdResponse>()
                .ForMember(dest => dest.NoiDungCauHoi, opt => opt.MapFrom(src => src.CauHoiCongNghe.CauHoi.NoiDung))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CauHoiCongNghe.CongNghe.Ten))
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.ThoiGianPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.ThoiGianPhongVan))
                .ReverseMap();
            CreateMap<PhongVan, UpdatePhongVanCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PhongVan, UpdatePhongVanResponse>();
            CreateMap<PhongVan, PhongVan>();
            CreateMap<PhongVan, GetAllPhongVanResponse>()
                .ForMember(dest => dest.NoiDungCauHoi, opt => opt.MapFrom(src => src.CauHoiCongNghe.CauHoi.NoiDung))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CauHoiCongNghe.CongNghe.Ten))
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.ThoiGianPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.ThoiGianPhongVan))
                .ReverseMap();

            // Nhom Zalo mapping
            CreateMap<NhomZalo, CreateNhomZaloResponse>().ReverseMap();
            CreateMap<NhomZalo, GetNhomZaloResponse>().ReverseMap();
            CreateMap<NhomZalo, UpdateNhomZaloResponse>().ReverseMap();
            CreateMap<NhomZalo, DeleteNhomZaloResponse>().ReverseMap();
            CreateMap<NhomZalo, CreateNhomZaloCommand>().ReverseMap();

            CreateMap<AddUserToNhomZaloCommand, UserNhomZalo>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.IdNhomZaloChung, opt => opt.MapFrom(src => src.NhomZaloId))
                .ForMember(dest => dest.IdNhomZaloRieng, opt => opt.MapFrom(src => src.NhomZaloId));

            CreateMap<UserNhomZalo, UpdateUserNhomZaloCommand>().ReverseMap();
            CreateMap<UserNhomZalo, GetUserNhomZaloResponse>().ReverseMap();
            CreateMap<UserNhomZalo, GetPagedUserNhomZaloResponse>().ReverseMap();

            // CongNghe mapping
            CreateMap<CongNghe, CreateCongNgheCommand>().ReverseMap();
            CreateMap<CongNghe, CreateCongNgheResponse>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.DateTime));
            CreateMap<CongNghe, UpdateCongNgheCommand>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CongNghe, UpdateCongNgheResponse>();
            CreateMap<CongNghe, GetAllCongNgheResponse>()
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri!.Ten))
                .ReverseMap();
            CreateMap<CongNghe, GetCongNgheByIdResponse>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.DateTime))
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri!.Ten));

            CreateMap<CongNghe, GetCongNgheByTenResponse>()
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri!.Ten));

            CreateMap<CongNghe, GetPagedCongNghesResponse>()
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri!.Ten))
                .ReverseMap();


            // CongNgheDuAn mapping
            CreateMap<CongNgheDuAn, CreateCongNgheDuAnCommand>().ReverseMap();
            CreateMap<CongNgheDuAn, CreateCongNgheDuAnResponse>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.DateTime));
            CreateMap<CongNgheDuAn, UpdateCongNgheDuAnCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CongNgheDuAn, UpdateCongNgheDuAnResponse>();
            CreateMap<CongNgheDuAn, GetAllCongNgheDuAnResponse>()
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNghe.Ten))
                .ReverseMap();
            CreateMap<CongNgheDuAn, GetCongNgheDuAnByIdResponse>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.DateTime))
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNghe.Ten))
                .ReverseMap();
            CreateMap<CongNgheDuAn, GetPagedCongNgheDuAnsResponse>()
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CongNghe.Ten))
                .ReverseMap();

            // ThongBao mapping 
            CreateMap<ThongBao, CreateThongBaoCommand>().ReverseMap();
            CreateMap<ThongBao, CreateThongBaoResponse>();
            CreateMap<ThongBao, UpdateThongBaoCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ThongBao, UpdateThongBaoResponse>();
            CreateMap<ThongBao, GetThongBaoByIdResponse>()
                .ForMember(dest => dest.TenNguoiNhan, opt => opt.MapFrom(src => src.NguoiNhan.HoVaTen))
                .ForMember(dest => dest.TenNguoiGui, opt => opt.MapFrom(src => src.NguoiGui.HoVaTen));
            CreateMap<ThongBao, GetAllThongBaoResponse>()
                .ForMember(dest => dest.TenNguoiNhan, opt => opt.MapFrom(src => src.NguoiNhan.HoVaTen))
                .ForMember(dest => dest.TenNguoiGui, opt => opt.MapFrom(src => src.NguoiGui.HoVaTen));
            CreateMap<ThongBao, GetPagedThongBaosResponse>()
                .ForMember(dest => dest.TenNguoiNhan, opt => opt.MapFrom(src => src.NguoiNhan.HoVaTen))
                .ForMember(dest => dest.TenNguoiGui, opt => opt.MapFrom(src => src.NguoiGui.HoVaTen))
                .ReverseMap();

            // LichPhongVan mapping
            CreateMap<LichPhongVan, GetLichPhongVanByIdResponse>()
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.TenNguoiDuocPhongVan, opt => opt.MapFrom(src => src.NguoiDuocPhongVan.HoTen))
                .ReverseMap();
            CreateMap<LichPhongVan, CreateLichPhongVanCommand>().ReverseMap();
            CreateMap<LichPhongVan, CreateLichPhongVanResponse>();
            CreateMap<LichPhongVan, UpdateLichPhongVanCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LichPhongVan, UpdateLichPhongVanResponse>();
            CreateMap<DateTimeOffset, DateTime>().ConvertUsing(dt => dt.UtcDateTime);
            CreateMap<LichPhongVan, GetAllLichPhongVanResponse>()
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.TenNguoiDuocPhongVan, opt => opt.MapFrom(src => src.NguoiDuocPhongVan.HoTen))
                .ReverseMap();
            CreateMap<LichPhongVan, GetLichPhongVanByTodayResponse>()
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.TenNguoiDuocPhongVan, opt => opt.MapFrom(src => src.NguoiDuocPhongVan.HoTen))
                .ReverseMap();

            // PhongVan mapping
            CreateMap<PhongVan, CreatePhongVanCommand>().ReverseMap();
            CreateMap<PhongVan, CreatePhongVanResponse>();
            CreateMap<PhongVan, UpdatePhongVanCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PhongVan, UpdatePhongVanResponse>();
            CreateMap<PhongVan, GetPagedPhongVansResponse>()
                .ForMember(dest => dest.NoiDungCauHoi, opt => opt.MapFrom(src => src.CauHoiCongNghe.CauHoi.NoiDung))
                .ForMember(dest => dest.TenCongNghe, opt => opt.MapFrom(src => src.CauHoiCongNghe.CongNghe.Ten))
                .ForMember(dest => dest.TenNguoiPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.NguoiPhongVan.HoVaTen))
                .ForMember(dest => dest.ThoiGianPhongVan, opt => opt.MapFrom(src => src.LichPhongVan.ThoiGianPhongVan))
                .ReverseMap();

            // Task mapping
            CreateMap<Tasks, CreateTaskCommand>().ReverseMap();
            CreateMap<Tasks, TaskResponse>()
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ReverseMap(); 
            CreateMap<Tasks, UpdateTaskCommand>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Tasks, GetPagedTasksResponse>()
                .ForMember(dest => dest.TenDuAn, opt => opt.MapFrom(src => src.DuAn.Ten))
                .ReverseMap();

            // User Task mapping
            CreateMap<ReportTask, TaskReportResponse>().ReverseMap();
            CreateMap<ReportTask, GetReportByIDReponse>()
            .ForMember(dest => dest.HoVaTen, opt => opt.MapFrom(src => src.User.HoVaTen))
            .ReverseMap();

            CreateMap<ReportTask, GetReportAllReponse>()
                .ForMember(dest => dest.HoVaTen, opt => opt.MapFrom(src => src.User.HoVaTen))
                .ReverseMap();
            CreateMap<ReportTask, CreateTaskReportCommand>().ReverseMap();
            CreateMap<ReportTask, UpdateTaskReportCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserTask, GetPagedUserTaskResponse>().ReverseMap();
            CreateMap<UserTask, UserTaskReponse>().ReverseMap();
            CreateMap<ReportTask, GetPagedReportTasksResponse>()
                .ForMember(dest => dest.HoVaTen, opt => opt.MapFrom(src => src.User.HoVaTen))
                .ReverseMap();

            //Task Report mapping
            CreateMap<UserTask, UserTaskReponse>().ReverseMap();
            CreateMap<UserTask, CreateUserTaskCommand>().ReverseMap();
            CreateMap<UserTask, UpdateUserTaskCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Task - NhomZalo mapping
            CreateMap<NhomZaloTask, NhomZaloTaskReponse>().ReverseMap();
            CreateMap<NhomZaloTask, CreateNhomZaloTaskCommand>().ReverseMap();
            CreateMap<NhomZaloTask, UpdateNhomZaloTaskCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserNhomZalo, LeaderResponse>().ReverseMap();

            // Claim - UserCliam mapping
            CreateMap<ApplicationClaim, GetClaimResponse>().ReverseMap();
            CreateMap<ApplicationClaim, AddClaimCommand>().ReverseMap();
            CreateMap<ApplicationClaim, UpdateClaimCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // UserDuAn mapping
            CreateMap<UserDuAn, GetUserDuAnByIdResponse>();
            CreateMap<UserDuAn, GetAllUserDuAnResponse>();
            CreateMap<UserDuAn, CreateUserDuAnCommand>().ReverseMap();
            CreateMap<UserDuAn, CreateUserDuAnResponse>();
            CreateMap<UserDuAn, UpdateUserDuAnCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserDuAn, UpdateUserDuAnResponse>();
            CreateMap<UserDuAn, GetAllUserDuAnResponse>();
            CreateMap<UserDuAn, GetPagedUserDuAnResponse>().ReverseMap();

            //ViTri mapping
            CreateMap<ViTri, GetViTriByIdResponse>();
            CreateMap<ViTri, CreateViTriCommand>().ReverseMap();
            CreateMap<ViTri, CreateViTriResponse>();
            CreateMap<ViTri, UpdateViTriCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ViTri, UpdateViTriResponse>();
            CreateMap<ViTri, GetViTriByTenResponse>();
            CreateMap<ViTri, GetAllViTriResponse>();
            CreateMap<ViTri, GetPagedViTrisResponse>().ReverseMap();

            //UserViTri mapping
            CreateMap<UserViTri, GetUserViTriByIdResponse>()
                .ForMember(dest => dest.TenUser, opt => opt.MapFrom(src => src.AspNetUser.HoVaTen))
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri.Ten));
            CreateMap<UserViTri, CreateUserViTriCommand>().ReverseMap();
            CreateMap<UserViTri, CreateUserViTriResponse>();
            CreateMap<UserViTri, UpdateUserViTriCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserViTri, UpdateUserViTriResponse>();
            CreateMap<UserViTri, GetAllUserViTriResponse>()
                .ForMember(dest => dest.TenUser, opt => opt.MapFrom(src => src.AspNetUser.HoVaTen))
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri.Ten))
                .ReverseMap();
            CreateMap<UserViTri, GetPagedUserViTriResponse>()
                .ForMember(dest => dest.TenUser, opt => opt.MapFrom(src => src.AspNetUser.HoVaTen))
                .ForMember(dest => dest.TenViTri, opt => opt.MapFrom(src => src.ViTri.Ten))
                .ReverseMap();
                

            // Mapping for PaginatedList
            //CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            //    .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }

    // Custom converter for PaginatedList
    //public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    //{
    //    public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
    //    {
    //        var mappedItems = context.Mapper.Map<IReadOnlyCollection<TDestination>>(source.Items);
    //        return new PaginatedList<TDestination>(mappedItems, source.TotalCount, source.PageNumber, source.PageSize);
    //    }
    //}
}
