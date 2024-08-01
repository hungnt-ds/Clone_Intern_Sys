using InternSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.DBContext;

public class ApplicationDbContext : IdentityDbContext<AspNetUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    // @.@
    public DbSet<AspNetUser> Users => Set<AspNetUser>();
    public DbSet<DuAn> DuAns { get; set; }
    public DbSet<ThongBao> ThongBaos { get; set; }
    public DbSet<UserDuAn> UserDuAns { get; set; }
    public DbSet<CongNghe> CongNghes { get; set; }
    public DbSet<CongNgheDuAn> CongNgheDuAns { get; set; }
    public DbSet<ViTri> ViTris { get; set; }
    public DbSet<UserViTri> UserViTris { get; set; }
    public DbSet<NhomZalo> NhomZalos { get; set; }
    public DbSet<UserNhomZalo> UserNhomZalos { get; set; }
    public DbSet<PhongVan> PhongVans { get; set; }
    public DbSet<LichPhongVan> LichPhongVans { get; set; }
    public DbSet<InternInfo> InternInfos { get; set; }
    public DbSet<EmailUserStatus> EmailUserStatuses { get; set; }
    public DbSet<KyThucTap> KyThucTaps { get; set; }
    public DbSet<TruongHoc> TruongHocs { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<ReportTask> ReportTasks { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<NhomZaloTask> NhomZaloTasks { get; set; }
    public DbSet<ApplicationClaim> ApplicationClaim { get; set; }
    public DbSet<CauHoi> CauHois { get; set; }
    public DbSet<CauHoiCongNghe> CauHoiCongNghes { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many-to-many AspNetUser - Message
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.IdSender)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.IdReceiver)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ThongBao>()
            .HasOne(tb => tb.NguoiNhan)
            .WithMany()
            .HasForeignKey(tb => tb.IdNguoiNhan)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ThongBao>()
            .HasOne(tb => tb.NguoiGui)
            .WithMany()
            .HasForeignKey(tb => tb.IdNguoiGui)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-many AspNetUser - DuAn
        modelBuilder.Entity<UserDuAn>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDuAns)
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserDuAn>()
            .HasOne(ud => ud.DuAn)
            .WithMany(d => d.UserDuAns)
            .HasForeignKey(ud => ud.DuAnId)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-many NhomZalo - User
        modelBuilder.Entity<UserNhomZalo>()
           .HasOne(unz => unz.User)
           .WithMany(u => u.UserNhomZalos)
           .HasForeignKey(unz => unz.UserId)
           .OnDelete(DeleteBehavior.NoAction);

        // NhomZaloChung - NhomZaloRieng
        modelBuilder.Entity<UserNhomZalo>()
            .HasOne(unz => unz.NhomZaloChung)
            .WithMany(nz => nz.UserNhomZaloChungs)
            .HasForeignKey(unz => unz.IdNhomZaloChung)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserNhomZalo>()
            .HasOne(unz => unz.NhomZaloRieng)
            .WithMany(nz => nz.UserNhomZaloRiengs)
            .HasForeignKey(unz => unz.IdNhomZaloRieng)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-many CauHoi - CongNghe
        modelBuilder.Entity<CauHoiCongNghe>()
            .HasOne(ud => ud.CauHoi)
            .WithMany(u => u.CauHoiCongNghes)
            .HasForeignKey(ud => ud.IdCauHoi)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CauHoiCongNghe>()
            .HasOne(ud => ud.CongNghe)
            .WithMany(d => d.CauHoiCongNghes)
            .HasForeignKey(ud => ud.IdCongNghe)
            .OnDelete(DeleteBehavior.NoAction);

        // Many-to-many AspNetUser - ViTri
        modelBuilder.Entity<UserViTri>()
            .HasOne(uv => uv.AspNetUser)
            .WithMany(u => u.UserViTris)
            .HasForeignKey(uv => uv.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserViTri>()
            .HasOne(uv => uv.ViTri)
            .WithMany(v => v.UserViTris)
            .HasForeignKey(uv => uv.IdViTri)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<InternInfo>()
            .HasOne(i => i.DuAn)
            .WithMany()
            .HasForeignKey(i => i.DuAnId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<EmailUserStatus>()
            .HasOne(eu => eu.NguoiNhan)
            .WithMany(i => i.EmailUserStatuses)
            .HasForeignKey(eu => eu.IdNguoiNhan)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LichPhongVan>()
            .HasOne(lp => lp.NguoiDuocPhongVan)
            .WithMany()
            .HasForeignKey(lp => lp.IdNguoiDuocPhongVan)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<InternInfo>()
            .HasOne(i => i.AspNetUser)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AspNetUser>()
            .HasOne(a => a.InternInfo)
            .WithMany(i => i.AspNetUsers)
            .HasForeignKey(a => a.InternInfoId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Tasks>()
          .HasOne(t => t.DuAn)
          .WithMany(d => d.Tasks)
          .HasForeignKey(t => t.DuAnId)
          .OnDelete(DeleteBehavior.NoAction);

        // Cấu hình cho bảng ReportTask
        modelBuilder.Entity<ReportTask>()
            .HasOne(rt => rt.Task)
            .WithMany(t => t.ReportTasks)
            .HasForeignKey(rt => rt.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<ReportTask>()
        .HasOne(rt => rt.User)
        .WithMany(t => t.reportTasks)
        .HasForeignKey(rt => rt.UserId)
        .OnDelete(DeleteBehavior.NoAction);

        // Cấu hình cho bảng UserTask
        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTasks)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.Task)
            .WithMany(t => t.UserTasks)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<NhomZaloTask>()
            .HasOne(ut => ut.Tasks)
            .WithMany(u => u.NhomZaloTasks)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<NhomZaloTask>()
            .HasOne(ut => ut.nhomZalos)
            .WithMany(t => t.NhomZaloTasks)
            .HasForeignKey(ut => ut.NhomZaloId)
            .OnDelete(DeleteBehavior.NoAction);



        // Cấu hình cho AspNetUserRoles
        //modelBuilder.Entity<AspNetUserRole>()
        //    .HasKey(ur => new { ur.UserId, ur.RoleId });

        //modelBuilder.Entity<AspNetUserRole>()
        //    .HasOne(ur => ur.User)
        //    .WithMany(u => u.UserRoles)
        //    .HasForeignKey(ur => ur.UserId);

        //modelBuilder.Entity<AspNetUserRole>()
        //    .HasOne(ur => ur.Role)
        //    .WithMany(r => r.UserRoles)
        //    .HasForeignKey(ur => ur.RoleId);
    }
}