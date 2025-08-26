using EntityFrameworkDatabaseLibrary.Models;
using DataHandlerLibrary.Models;
using DataHandlerLibrary.Services;

namespace EposRetail.Services
{
    public class UserSessionService
    {
        private readonly DayLogServices _dayLogServices;
        private readonly ShiftServices _shiftServices;
        private readonly SiteServices _siteServices;
        private readonly TillServices _tillServices;

        private PosUser? _currentUser;
        private Site? _currentSite;
        private Till? _currentTill;
        private DayLog? _currentDayLog;
        private Shift? _currentShift;

        // Constructor with dependency injection
        public UserSessionService(
            DayLogServices dayLogServices,
            ShiftServices shiftServices,
            SiteServices siteServices,
            TillServices tillServices)
        {
            _dayLogServices = dayLogServices;
            _shiftServices = shiftServices;
            _siteServices = siteServices;
            _tillServices = tillServices;
        }

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

        // Enhanced methods with automatic database fetching
        public async Task<DayLog?> EnsureDayLogAsync()
        {
            if (CurrentDayLog != null)
                return CurrentDayLog;

            try
            {
                // Try to get the last active daylog
                var dayLog = await _dayLogServices.GetLastDayLog();

                if (dayLog != null && dayLog.DayLog_End_DateTime == null)
                {
                    // Found an active daylog
                    SetDayLog(dayLog);
                    return dayLog;
                }
                else
                {
                    // No active daylog exists, create a new one
                    var newDayLog = new DayLog
                    {
                        DayLog_Start_DateTime = DateTime.UtcNow,
                        Date_Created = DateTime.UtcNow,
                        Last_Modified = DateTime.UtcNow,
                        Created_By_Id = CurrentUser?.User_ID,
                        Last_Modified_By_Id = CurrentUser?.User_ID,
                        Site_Id = CurrentSite?.Site_Id,
                        Till_Id = CurrentTill?.Till_Id
                    };

                    await _dayLogServices.AddAsync(newDayLog);
                    SetDayLog(newDayLog);
                    return newDayLog;
                }
            }
            catch (Exception ex)
            {
                // Log error and return null
                Console.WriteLine($"Error ensuring DayLog: {ex.Message}");
                return null;
            }
        }

        public async Task<Shift?> EnsureShiftAsync()
        {
            if (CurrentShift != null)
                return CurrentShift;

            try
            {
                // Try to get the last shift
                var lastShift = await _shiftServices.GetLastShiftLog();

                if (lastShift != null && lastShift.Shift_End_DateTime == null)
                {
                    // Found an active shift (no end datetime), use it
                    SetShift(lastShift);
                    return lastShift;
                }
                else
                {
                    // Ensure we have a valid DayLog before creating a shift
                    var currentDayLog = await EnsureDayLogAsync();
                    if (currentDayLog == null)
                    {
                        throw new InvalidOperationException("Cannot create shift without a valid DayLog");
                    }

                    // No active shift exists or shift has ended, create a new one
                    var newShift = new Shift
                    {
                        DayLog_Id = currentDayLog.DayLog_Id, // This was missing!
                        Shift_Start_DateTime = DateTime.UtcNow,
                        Date_Created = DateTime.UtcNow,
                        Last_Modified = DateTime.UtcNow,
                        Created_By_Id = CurrentUser?.User_ID,
                        Last_Modified_By_Id = CurrentUser?.User_ID,
                        Site_Id = CurrentSite?.Site_Id,
                        Till_Id = CurrentTill?.Till_Id,
                        PosUser_Id = CurrentUser?.User_ID ?? 1
                    };

                    await _shiftServices.AddAsync(newShift);
                    SetShift(newShift);
                    return newShift;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Shift: {ex.Message}");
                return null;
            }
        }

        public async Task<Site?> EnsureSiteAsync()
        {
            if (CurrentSite != null)
                return CurrentSite;

            try
            {
                // Get the first available site as fallback
                var site = await _siteServices.GetFirstSiteRecord();

                if (site != null)
                {
                    SetSite(site);
                    return site;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Site: {ex.Message}");
                return null;
            }
        }

        public async Task<Till?> EnsureTillAsync()
        {
            if (CurrentTill != null)
                return CurrentTill;

            try
            {
                // Get the first available till as fallback
                var till = await _tillServices.GetFirstTillRecord();

                if (till != null)
                {
                    SetTill(till);
                    return till;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Till: {ex.Message}");
                return null;
            }
        }

        // Enhanced session validation
        public async Task<bool> EnsureCompleteSessionAsync()
        {
            try
            {
                await EnsureSiteAsync();
                await EnsureTillAsync();
                await EnsureDayLogAsync();
                await EnsureShiftAsync();

                return IsSessionValid();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring complete session: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ValidateSessionAsync()
        {
            if (!IsUserLoggedIn())
                return false;

            return await EnsureCompleteSessionAsync();
        }

        // Safe getters that ensure objects exist
        public async Task<int> GetValidDayLogIdAsync()
        {
            var dayLog = await EnsureDayLogAsync();
            return dayLog?.DayLog_Id ?? throw new InvalidOperationException("Unable to ensure valid DayLog");
        }

        public async Task<int> GetValidSiteIdAsync()
        {
            var site = await EnsureSiteAsync();
            return site?.Site_Id ?? throw new InvalidOperationException("Unable to ensure valid Site");
        }

        public async Task<int> GetValidTillIdAsync()
        {
            var till = await EnsureTillAsync();
            return till?.Till_Id ?? throw new InvalidOperationException("Unable to ensure valid Till");
        }

        public async Task<int> GetValidShiftIdAsync()
        {
            var shift = await EnsureShiftAsync();
            return shift?.Shift_Id ?? throw new InvalidOperationException("Unable to ensure valid Shift");
        }

        public int GetValidUserIdAsync()
        {
            return CurrentUser?.User_ID ?? throw new InvalidOperationException("No user logged in");
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