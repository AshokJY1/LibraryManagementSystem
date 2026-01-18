using LibraryAPI.Core.Models;
using LibraryAPI.Core.Services;
using LibraryAPI.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly LibraryDbContext _context;

    public BorrowingRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Borrowing> AddAsync(Borrowing borrowing)
    {
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(borrowing.Id) ?? borrowing;
    }

    public async Task<Borrowing> UpdateAsync(Borrowing borrowing)
    {
        _context.Borrowings.Update(borrowing);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(borrowing.Id) ?? borrowing;
    }

    public async Task<IEnumerable<Borrowing>> GetActiveBorrowingsByUserAsync(int userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .Where(b => b.UserId == userId && b.Status == "Borrowed")
            .ToListAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetBorrowingHistoryByUserAsync(int userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BorrowedDate)
            .ToListAsync();
    }
}