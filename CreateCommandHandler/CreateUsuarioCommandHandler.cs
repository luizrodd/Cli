
using MediatR;

namespace Application
{
    public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, bool>
    {
        public bool Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
        {
            return;
        }
    }
}