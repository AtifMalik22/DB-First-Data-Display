using Demo_Project.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Demo_Project.Services
{
    public class AccountsService
    {
        protected AppDbContext _context;
        public AccountsService(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<List<v_Accounts>> GetDataAsync()
        {
            var result= await _context.v_Accounts.ToListAsync();
            return result;
        }
        public async Task<bool> InsertData(v_Accounts accounts)
        {
            try
            {
                if (accounts.id == 0)
                {
                    await _context.v_Accounts.AddAsync(accounts);
                }
                else
                {
                    var dep = _context.v_Accounts.Where(x => x.id == accounts.id).FirstOrDefault();
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
