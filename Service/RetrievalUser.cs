using CornHoleRevamp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CornHoleRevamp.Service
{
    public class RetrievalUser
    {
        private readonly AppDbContext _context;

        public RetrievalUser(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUsernameByIdAsync(int userId)
        {
            var user = await _context.Users  // Changed to Users
                       .Where(u => u.Id == userId)
                       .Select(u => u.Name)
                       .FirstOrDefaultAsync();
            return user;
        }
    }
}