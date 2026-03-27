using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;

namespace Farola.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already exists");

            var clientRole = await _roleRepository.GetByNameAsync("Client", cancellationToken);
            if (clientRole == null)
                throw new InvalidOperationException("Role 'Client' not found. Ensure roles are seeded.");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);


            var user = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                Surname = request.Surname,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                RoleId = clientRole.Id,
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
