using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.MediatR.Command
{
    public class AddNewJob : IRequest
    {

    }

    public class AddNewJobHandler : IRequestHandler<AddNewJob>
    {
        public Task<Unit> Handle(AddNewJob request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}