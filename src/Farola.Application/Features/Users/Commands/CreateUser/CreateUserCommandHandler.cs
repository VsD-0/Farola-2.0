using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using Farola.Domain.ValueObjects;
using MediatR;

namespace Farola.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleCacheService _roleCacheService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IRoleCacheService roleCacheService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleCacheService = roleCacheService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already exists");

            var role = await _roleCacheService.GetRoleByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
                throw new InvalidOperationException($"Role with id {request.RoleId} not found.");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);


            var user = new User
            {
                Email = new Email(request.Email),
                PhoneNumber = new PhoneNumber(request.PhoneNumber),
                Password = _passwordHasher.HashPassword(request.Password),
                Surname = request.Surname,
                Name = request.Name,
                Patronymic = request.Patronymic,
                RoleId = request.RoleId,
                Area = request.Area,
                Information = request.Information,
                SpecializationId = request.SpecializationId,
                Photo = request.Photo,
                Profession = request.Profession,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
