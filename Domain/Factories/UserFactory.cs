using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;
public class UserFactory : IUserFactory
{
    public User FromForm(CreateUserForm form)
    {
        return new User
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            Projects = form.Projects
        };
    }

    public User FromForm(UpdateUserForm form)
    {
        return new User
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            Projects = form.Projects
        };
    }
}
