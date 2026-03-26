using MediatR;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;

namespace Farola.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already exists");

            // TODO: хэширование пароля (позже)
            var user = new User
            {
                Email = request.Email,
                Password = request.Password, // временно
                Surname = request.Surname,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                Patronymic = request.Patronymic,
                Profession = request.Profession,
                Area = request.Area,
                Information = request.Information,
                SpecializationId = request.SpecializationId,
                Photo = request.Photo,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            await _userRepository.AddAsync(user);
            return user.Id;
        }
    }
}
