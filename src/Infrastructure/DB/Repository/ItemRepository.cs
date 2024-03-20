using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB.Repository;

public class ItemRepository : IRepository<Item>
{
    private readonly AppDbContext _context;
    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Item?> Add(Item entity)
    {
        var result = await _context.Items.AddAsync(entity);
        return result.Entity;
    }

    public async Task<Item?> DeleteById(int id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

        if (item != null)
        {
            var result = _context.Items.Remove(item);
            return result.Entity;
        }

        return null;
    }

    public async Task<IEnumerable<Item?>> GetAll(int skip, int take)
    {
        return await _context.Items
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<Item?> GetById(int id)
    {
        return await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

}
