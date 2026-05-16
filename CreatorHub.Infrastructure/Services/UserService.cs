using CreatorHub.Core.DTOs.Users;
using CreatorHub.Core.Enums;
using CreatorHub.Core.Interfaces;
using CreatorHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreatorHub.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return null;

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new Exception("Потребителят не е намерен.");

        if (dto.DisplayName != null) user.DisplayName = dto.DisplayName;
        if (dto.AvatarUrl != null) user.AvatarUrl = dto.AvatarUrl;

        await _context.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task UpdateRoleAsync(Guid id, UserRole role)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new Exception("Потребителят не е намерен.");

        user.Role = role;
        await _context.SaveChangesAsync();
    }
}