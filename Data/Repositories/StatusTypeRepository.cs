using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

internal class StatusTypeRepository(DataContext context) : BaseRepository<StatusTypeEntity>(context), IStatusTypeRepository
{
    private readonly DataContext _context = context;
}

