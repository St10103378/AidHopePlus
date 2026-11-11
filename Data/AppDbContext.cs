using AidHopePlus.Models;
using Microsoft.EntityFrameworkCore;

namespace AidHopePlus.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<SupportSession> SupportSessions { get; set; }
        public DbSet<AwarenessEvent> AwarenessEvents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=tcp:st10103378.database.windows.net,1433;" +
                "Initial Catalog=AidHopePlusDB;" +
                "Persist Security Info=False;" +
                "User ID=st10103378;" +
                "Password=2025#Lis;" +
                "MultipleActiveResultSets=False;" +
                "Encrypt=True;" +
                "TrustServerCertificate=False;" +
                "Connection Timeout=30;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default staff user (password: staff123 - change in production!)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "staff",
                    PasswordHash = "staff123",
                    FullName = "Staff User",
                    Role = "Staff"
                }
            );

            // Seed Beneficiaries
            modelBuilder.Entity<Beneficiary>().HasData(
                new Beneficiary { Id = 1, BeneficiaryCode = "B001", FullName = "Thabo Mokoena", ContactNumber = "071 234 5678", Status = "HIV Positive" },
                new Beneficiary { Id = 2, BeneficiaryCode = "B002", FullName = "Nomsa Dlamini", ContactNumber = "082 345 6789", Status = "Affected" },
                new Beneficiary { Id = 3, BeneficiaryCode = "B003", FullName = "Sipho Khumalo", ContactNumber = "079 876 5432", Status = "HIV Positive" },
                new Beneficiary { Id = 4, BeneficiaryCode = "B004", FullName = "Lerato Nkosi", ContactNumber = "083 111 2222", Status = "Affected" },
                new Beneficiary { Id = 5, BeneficiaryCode = "B005", FullName = "Anele Zulu", ContactNumber = "076 555 4444", Status = "HIV Positive" },
                new Beneficiary { Id = 6, BeneficiaryCode = "B006", FullName = "Thapelo Sehadi", ContactNumber = "0723456789", Status = "HIV Positive" }
            );

            // Seed Support Sessions
            modelBuilder.Entity<SupportSession>().HasData(
                new SupportSession { Id = 1, BeneficiaryId = 2, SupportType = "Psychosocial Support", SessionDate = new DateTime(2026, 10, 9), FollowUpNotes = "Family counselling session held." },
                new SupportSession { Id = 2, BeneficiaryId = 3, SupportType = "Counselling", SessionDate = new DateTime(2026, 10, 6), FollowUpNotes = "Follow up on treatment progress next week." },
                new SupportSession { Id = 3, BeneficiaryId = 5, SupportType = "Medical Referral", SessionDate = new DateTime(2026, 10, 2), FollowUpNotes = "Referred to Mamelodi clinic for check-up." },
                new SupportSession { Id = 4, BeneficiaryId = 4, SupportType = "Spiritual Care", SessionDate = new DateTime(2026, 9, 27), FollowUpNotes = "Prayer and encouragement session." },
                new SupportSession { Id = 5, BeneficiaryId = 1, SupportType = "Home Visit", SessionDate = new DateTime(2026, 9, 20), FollowUpNotes = "Doing better. Referred to support group." },
                new SupportSession { Id = 6, BeneficiaryId = 3, SupportType = "Treatment Adherence Support", SessionDate = new DateTime(2026, 9, 13), FollowUpNotes = "Reminded about clinic dates." },
                new SupportSession { Id = 7, BeneficiaryId = 2, SupportType = "Food Parcel", SessionDate = new DateTime(2026, 9, 7), FollowUpNotes = "Parcel delivered to the family." },
                new SupportSession { Id = 8, BeneficiaryId = 1, SupportType = "Counselling", SessionDate = new DateTime(2026, 9, 1), FollowUpNotes = "Discussed coping strategies. Check in again in two weeks." }
            );

            // Seed Awareness Events
            modelBuilder.Entity<AwarenessEvent>().HasData(
                new AwarenessEvent { Id = 1, EventName = "Support Group Meeting", Location = "Clinic", EventDate = new DateTime(2026, 10, 25) },
                new AwarenessEvent { Id = 2, EventName = "Health Education Day", Location = "Town Hall", EventDate = new DateTime(2026, 10, 1) },
                new AwarenessEvent { Id = 3, EventName = "Youth Workshop", Location = "2BC Centre", EventDate = new DateTime(2026, 9, 11) },
                new AwarenessEvent { Id = 4, EventName = "HIV Awareness Talk", Location = "Community Hall", EventDate = new DateTime(2026, 8, 22) }
            );

            // Seed Attendance Records (8 records to match dashboard)
            modelBuilder.Entity<AttendanceRecord>().HasData(
                new AttendanceRecord { Id = 1, AwarenessEventId = 2, BeneficiaryId = 1, AttendeeName = "Thabo Mokoena", AttendanceDate = new DateTime(2026, 10, 1), Status = "Present" },
                new AttendanceRecord { Id = 2, AwarenessEventId = 2, BeneficiaryId = 2, AttendeeName = "Nomsa Dlamini", AttendanceDate = new DateTime(2026, 10, 1), Status = "Present" },
                new AttendanceRecord { Id = 3, AwarenessEventId = 2, BeneficiaryId = 3, AttendeeName = "Sipho Khumalo", AttendanceDate = new DateTime(2026, 10, 1), Status = "Present" },
                new AttendanceRecord { Id = 4, AwarenessEventId = 3, BeneficiaryId = 4, AttendeeName = "Lerato Nkosi", AttendanceDate = new DateTime(2026, 9, 11), Status = "Present" },
                new AttendanceRecord { Id = 5, AwarenessEventId = 3, BeneficiaryId = 5, AttendeeName = "Anele Zulu", AttendanceDate = new DateTime(2026, 9, 11), Status = "Late" },
                new AttendanceRecord { Id = 6, AwarenessEventId = 4, BeneficiaryId = 1, AttendeeName = "Thabo Mokoena", AttendanceDate = new DateTime(2026, 8, 22), Status = "Present" },
                new AttendanceRecord { Id = 7, AwarenessEventId = 4, BeneficiaryId = 6, AttendeeName = "Thapelo Sehadi", AttendanceDate = new DateTime(2026, 8, 22), Status = "Present" },
                new AttendanceRecord { Id = 8, AwarenessEventId = 1, BeneficiaryId = 2, AttendeeName = "Nomsa Dlamini", AttendanceDate = new DateTime(2026, 10, 25), Status = "Present", Notes = "Registered in advance" }
            );
        }
    }
}
