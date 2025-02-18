using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    private readonly DataContext _context = context;

    //TODO: Increase functionality to add custom methods for getting Projects
}

