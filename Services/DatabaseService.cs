using AidHopePlus.Data;
using AidHopePlus.Models;
using Microsoft.EntityFrameworkCore;

namespace AidHopePlus.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        // ========== Beneficiaries ==========
        public async Task<List<Beneficiary>> GetAllBeneficiariesAsync()
        {
            return await _context.Beneficiaries
                .OrderBy(b => b.BeneficiaryCode)
                .ToListAsync();
        }

        public async Task<List<Beneficiary>> SearchBeneficiariesAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return await GetAllBeneficiariesAsync();

            search = search.ToLower();
            return await _context.Beneficiaries
                .Where(b => b.BeneficiaryCode.ToLower().Contains(search) ||
                            b.FullName.ToLower().Contains(search) ||
                            (b.ContactNumber != null && b.ContactNumber.Contains(search)) ||
                            b.Status.ToLower().Contains(search))
                .OrderBy(b => b.BeneficiaryCode)
                .ToListAsync();
        }

        public async Task AddBeneficiaryAsync(Beneficiary beneficiary)
        {
            var last = await _context.Beneficiaries
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            int nextNum = (last?.Id ?? 0) + 1;
            beneficiary.BeneficiaryCode = $"B{nextNum:D3}";

            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBeneficiaryAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Update(beneficiary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBeneficiaryAsync(int id)
        {
            var item = await _context.Beneficiaries.FindAsync(id);
            if (item != null)
            {
                _context.Beneficiaries.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // ========== Support Sessions ==========
        public async Task<List<SupportSession>> GetRecentSupportSessionsAsync(int count = 10)
        {
            return await _context.SupportSessions
                .Include(s => s.Beneficiary)
                .OrderByDescending(s => s.SessionDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<SupportSession>> GetAllSupportSessionsAsync()
        {
            return await _context.SupportSessions
                .Include(s => s.Beneficiary)
                .OrderByDescending(s => s.SessionDate)
                .ToListAsync();
        }

        public async Task AddSupportSessionAsync(SupportSession session)
        {
            _context.SupportSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        // ========== Awareness Events ==========
        public async Task<List<AwarenessEvent>> GetAllEventsAsync()
        {
            return await _context.AwarenessEvents
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();
        }

        public async Task AddEventAsync(AwarenessEvent evt)
        {
            _context.AwarenessEvents.Add(evt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var item = await _context.AwarenessEvents.FindAsync(id);
            if (item != null)
            {
                _context.AwarenessEvents.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // ========== Attendance ==========
        public async Task<List<AttendanceRecord>> GetAllAttendanceAsync()
        {
            return await _context.AttendanceRecords
                .Include(a => a.AwarenessEvent)
                .Include(a => a.Beneficiary)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();
        }

        public async Task<List<AttendanceRecord>> SearchAttendanceAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return await GetAllAttendanceAsync();

            search = search.ToLower();
            return await _context.AttendanceRecords
                .Include(a => a.AwarenessEvent)
                .Include(a => a.Beneficiary)
                .Where(a => (a.AttendeeName != null && a.AttendeeName.ToLower().Contains(search)) ||
                            (a.AwarenessEvent != null && a.AwarenessEvent.EventName.ToLower().Contains(search)) ||
                            a.Status.ToLower().Contains(search))
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();
        }

        public async Task AddAttendanceAsync(AttendanceRecord record)
        {
            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var item = await _context.AttendanceRecords.FindAsync(id);
            if (item != null)
            {
                _context.AttendanceRecords.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // ========== Dashboard Stats ==========
        public async Task<(int Beneficiaries, int Sessions, int Events, int Attendance, int Pending)> GetDashboardStatsAsync()
        {
            var beneficiaries = await _context.Beneficiaries.CountAsync();
            var sessions = await _context.SupportSessions.CountAsync();
            var events = await _context.AwarenessEvents.CountAsync();
            var attendance = await _context.AttendanceRecords.CountAsync();
            var pending = 1; // placeholder
            return (beneficiaries, sessions, events, attendance, pending);
        }

        // ========== Reports ==========
        public async Task<Dictionary<string, int>> GetSupportTypeBreakdownAsync()
        {
            return await _context.SupportSessions
                .GroupBy(s => s.SupportType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetBeneficiaryStatusBreakdownAsync()
        {
            return await _context.Beneficiaries
                .GroupBy(b => b.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<List<SupportSession>> GetSessionsByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.SupportSessions
                .Include(s => s.Beneficiary)
                .Where(s => s.SessionDate >= from && s.SessionDate <= to)
                .OrderByDescending(s => s.SessionDate)
                .ToListAsync();
        }

        // ========== Login ==========
        public async Task<User?> LoginAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
        }
    }
}
