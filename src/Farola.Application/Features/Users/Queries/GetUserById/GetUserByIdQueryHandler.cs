using Farola.Application.DTOs.Users;
using Farola.Domain.Interfaces.Repositories;
using MediatR;

namespace Farola.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic = user.Patronymic,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Area = user.Area,
                Information = user.Information,
                SpecializationId = user.SpecializationId,
                Photo = user.Photo,
                DateRegistration = user.DateRegistration,
                Profession = user.Profession,
                IsClosed = user.IsClosed,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty
            };
        }
    }
}
