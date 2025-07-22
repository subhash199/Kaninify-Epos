using EntityFrameworkDatabaseLibrary.Models;
using DataHandlerLibrary.Models;

namespace EposRetail.Services
{
    public class UserSessionService
    {
        private PosUser? _currentUser;
        private Site? _currentSite;
        private Till? _currentTill;
        private DayLog? _currentDayLog;
        private Shift? _currentShift;

        // Events for state changes
        public event Action? OnUserChanged;
        public event Action? OnSiteChanged;
        public event Action? OnTillChanged;
        public event Action? OnDayLogChanged;
        public event Action? OnShiftChanged;
        public event Action? OnSessionCleared;

        // Properties with change notifications
        public PosUser? CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnUserChanged?.Invoke();
            }
        }

        public Site? CurrentSite
        {
            get => _currentSite;
            private set
            {
                _currentSite = value;
                OnSiteChanged?.Invoke();
            }
        }

        public Till? CurrentTill
        {
            get => _currentTill;
            private set
            {
                _currentTill = value;
                OnTillChanged?.Invoke();
            }
        }

        public DayLog? CurrentDayLog
        {
            get => _currentDayLog;
            private set
            {
                _currentDayLog = value;
                OnDayLogChanged?.Invoke();
            }
        }

        public Shift? CurrentShift
        {
            get => _currentShift;
            private set
            {
                _currentShift = value;
                OnShiftChanged?.Invoke();
            }
        }

        // Individual setters for flexibility
        public void SetUser(PosUser? user)
        {
            CurrentUser = user;
        }

        public void SetSite(Site? site)
        {
            CurrentSite = site;
        }

        public void SetTill(Till? till)
        {
            CurrentTill = till;
        }

        public void SetDayLog(DayLog? dayLog)
        {
            CurrentDayLog = dayLog;
        }

        public void SetShift(Shift? shift)
        {
            CurrentShift = shift;
        }

        // Batch setter for login scenario
        public void SetSession(PosUser? user, Site? site, Till? till, DayLog? dayLog = null, Shift? shift = null)
        {
            CurrentUser = user;
            CurrentSite = site;
            CurrentTill = till;
            CurrentDayLog = dayLog;
            CurrentShift = shift;
        }

        // Session validation
        public bool IsSessionValid()
        {
            return CurrentUser != null && CurrentSite != null && CurrentTill != null;
        }

        public bool IsUserLoggedIn()
        {
            return CurrentUser != null;
        }

        // Clear session
        public void ClearSession()
        {
            CurrentUser = null;
            CurrentSite = null;
            CurrentTill = null;
            CurrentDayLog = null;
            CurrentShift = null;
            OnSessionCleared?.Invoke();
        }

        // Helper methods for common scenarios
        public int? GetCurrentUserId() => CurrentUser?.User_ID;
        public int? GetCurrentSiteId() => CurrentSite?.Site_Id;
        public int? GetCurrentTillId() => CurrentTill?.Till_Id;
        public int? GetCurrentDayLogId() => CurrentDayLog?.DayLog_Id;
        public int? GetCurrentShiftId() => CurrentShift?.Shift_Id;

        // Get user permissions
        public bool HasPermission(string permission)
        {
            // Implement permission checking logic based on CurrentUser
            // This is a placeholder - implement based on your permission system
            return CurrentUser?.Is_Activated == true;
        }
    }
}