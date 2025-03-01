using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{

    public async Task<ProjectEntity?> GetProjectByIdAsync(int projectId)
    {
        //TODO: Uncomment wheen the ProjectModel is implemented
        //var result = await (from project in _dbSet
        //                    where project.Id == projectId
        //                    join status in _context.StatusTypes on project.StatusId equals status.Id
        //                    join customer in _context.Customers on project.CustomerId equals customer.Id
        //                    join user in _context.Users on project.UserId equals user.Id
        //                    select new ProjectModel
        //                    {
        //                        project.Id,
        //                        project.Title,
        //                        project.Description,
        //                        Status = status.StatusName,
        //                        Customer = customer.CustomerName,
        //                        User = $"{user.FirstName} {user.LastName}",
        //                        Products = project.Products.Select(x => x.ProductName),
        //                        project.StartDate,
        //                        project.EndDate
        //                    }
        //                    ).FirstOrDefaultAsync();
        //return result;

        //Make sure to change the return type to ProjectModel when uncommenting the above code

        return await _dbSet.FirstOrDefaultAsync(x => x.Id == projectId);

        //TODO: When Businnes and Presentation are implemented, implement the rest of EagerLoading in the service layer and the presentation layer
    }
}

