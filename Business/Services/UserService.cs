using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IUserFactory userFactory, IUserMapper userMapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserFactory _userFactory = userFactory;
    private readonly IUserMapper _userMapper = userMapper;
    public async Task<bool?> AddAsync(CreateUserForm form)
    {
        User userModel = _userFactory.FromForm(form);
        UserEntity userEntity = _userMapper.ToEntity(userModel);
        return await _userRepository.AddAsync(userEntity);
    }



    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.Select(x => _userMapper.ToModel(x));
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var result = await _userRepository.GetAsync(x => x.Id == id);
        if (result == null)
        {
            return null;
        }

        User user = _userMapper.ToModel(result);
        return user;
    }

    public async Task<bool?> UpdateAsync(UpdateUserForm form, User existingUser)
    {
        existingUser.FirstName = form.FirstName;
        existingUser.LastName = form.LastName;
        existingUser.Email = form.Email;
        existingUser.Projects = form.Projects;
        var updatedEntity = _userMapper.ToEntity(existingUser);

        return await _userRepository.UpdateAsync(x => x.Id == existingUser.Id, updatedEntity);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(x => x.Id == id);
    }
}
