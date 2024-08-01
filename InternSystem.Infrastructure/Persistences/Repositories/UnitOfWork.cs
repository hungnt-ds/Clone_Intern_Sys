using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities.BaseEntities;
using InternSystem.Infrastructure.Persistences.DBContext;
using InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;

namespace InternSystem.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IUserRepository? _userRepository;
        private IInternInfoRepository? _internInfoRepository;
        private ITruongHocRepository? _truongHocRepository;
        private IKyThucTapRepository? _kyThucTapRepository;
        private IDuAnRepository? _duAnRepository;
        private ICauHoiRepository? _cauHoiRepository;
        private ICauHoiCongNgheRepository? _cauHoiCongNgheRepository;
        private ICongNgheRepository _congNgheRepository;
        private ILichPhongVanRepository? _lichPhongVanRepository; // Thêm repository mới
        private IPhongVanRepository? _PhongVanRepository;
        private INhomZaloRepository? _nhomZaloRepository;
        private IUserNhomZaloRepository? _userNhomZaloRepository;
        private IThongBaoRepository? _thongBaoRepository;
        private IUserDuAnRepository? _userDuAnRepository;
        private ICommentRepository? _commentRepository;
        private IEmailUserStatusRepository? _emailUserStatusRepository;
        private ICongNgheDuAnRepository _congNgheDuAnRepository;
        private IViTriRepository? _viTriRepository;
        private IUserViTriRepository? _userViTriRepository;
        private IDashboardRepository? _dashboardRepository;
        private IMessageRepository? _messageRepository;
        private ITaskRepository? _taskRepository;
        private IReportTaskRepository _reportTaskRepository;
        private IUserTaskRepository _userTaskRepository;
        private INhomZaloTaskRepository _nhomZaloTaskRepository;
        private IClaimRepository _claimRepository;



        public UnitOfWork(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext);
        public IInternInfoRepository InternInfoRepository => _internInfoRepository ??= new InternInfoRepository(_dbContext);
        public ITruongHocRepository TruongHocRepository => _truongHocRepository ??= new TruongHocRepository(_dbContext);
        public IKyThucTapRepository KyThucTapRepository => _kyThucTapRepository ??= new KyThucTapRepository(_dbContext);
        public IDuAnRepository DuAnRepository => _duAnRepository ??= new DuAnRepository(_dbContext);
        public IUserDuAnRepository UserDuAnRepository => _userDuAnRepository ??= new UserDuAnRepository(_dbContext);
        public ILichPhongVanRepository LichPhongVanRepository => _lichPhongVanRepository ??= new LichPhongVanRepository(_dbContext); // Khởi tạo LichPhongVanRepository
        public IPhongVanRepository PhongVanRepository => _PhongVanRepository ??= new PhongVanRepository(_dbContext); // Khởi tạo PhongVanRepository
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_dbContext);
        public IEmailUserStatusRepository EmailUserStatusRepository => _emailUserStatusRepository ??= new EmailUserStatusRepository(_dbContext);
        public IViTriRepository ViTriRepository => _viTriRepository ??= new ViTriRepository(_dbContext);
        public IUserViTriRepository? UserViTriRepository => _userViTriRepository ??= new UserViTriRepository(_dbContext);
        public ICauHoiRepository CauHoiRepository => _cauHoiRepository ??= new CauHoiRepository(_dbContext);
        public ICauHoiCongNgheRepository CauHoiCongNgheRepository => _cauHoiCongNgheRepository ??= new CauHoiCongNgheRepository(_dbContext);
        public ICongNgheRepository CongNgheRepository => _congNgheRepository ??= new CongNgheRepository(_dbContext);
        public INhomZaloRepository NhomZaloRepository => _nhomZaloRepository ??= new NhomZaloRepository(_dbContext);
        public IUserNhomZaloRepository UserNhomZaloRepository => _userNhomZaloRepository ??= new UserNhomZaloRepository(_dbContext);
        public ICongNgheDuAnRepository CongNgheDuAnRepository => _congNgheDuAnRepository ??= new CongNgheDuAnRepository(_dbContext);
        public IThongBaoRepository ThongBaoRepository => _thongBaoRepository ??= new ThongBaoRepository(_dbContext);
        public IDashboardRepository DashboardRepository => _dashboardRepository ??= new DashboardRepository(_dbContext);

        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_dbContext);

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IBaseRepository<T> GetRepository<T>() where T : class, IBaseEntity
        {
            return new BaseRepository<T>(_dbContext);
        }

        public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(_dbContext);

        public IReportTaskRepository ReportTaskRepository => _reportTaskRepository ??= new ReportTaskRepository(_dbContext);

        public IUserTaskRepository UserTaskRepository => _userTaskRepository ??= new UserTaskRepository(_dbContext);
        public INhomZaloTaskRepository NhomZaloTaskRepository => _nhomZaloTaskRepository ??= new NhomZaloTaskRepository(_dbContext);
        public IClaimRepository ClaimRepository => _claimRepository ??= new ClaimRepository(_dbContext);

    }
}