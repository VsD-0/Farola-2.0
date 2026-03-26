using MediatR;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Exceptions;

namespace Farola.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with id {request.Id} not found");
            return user;
        }
    }
}
