using CreatorHub.Core.DTOs.Users;
using CreatorHub.Core.Enums;

namespace CreatorHub.Core.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto dto);
    Task UpdateRoleAsync(Guid id, UserRole role);
}