using Demo_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo_Project.Services
{
    public class UploadFileService
    {
        protected AppDbContext _context;
        public UploadFileService(AppDbContext context) {
        this._context = context;    
        }
        public async Task SaveFileContentToDatabase(List<UploadFile> lines, DateTime uploadDate)
        {
            try
            {
                foreach (var line in lines)
                {
                    line.Date = uploadDate;
                }

                _context.Files.AddRange(lines);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to database: {ex.Message}");
                // Handle or log the exception as needed
            }
        }
        public async Task<List<UploadFile>> GetDataAsync()
        {
            var result = await _context.Files.ToListAsync();
            return result;
        }

    }
}
