using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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


    public  async Task<bool?> UpdateAsyncc(ProjectEntity updatedEntity, List<ProductEntity> newProducts)
    {
        if (updatedEntity == null)
            return null!;
        try
        {
            var existingEntity = await _dbSet
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == updatedEntity.Id);

            if (existingEntity == null)
                return null!;

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            existingEntity.Products.Clear();
            foreach (var product in newProducts)
            {
                existingEntity.Products.Add(product);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(ProductEntity)} entity:: {ex.Message}");
            return false;
        }
    }


}

