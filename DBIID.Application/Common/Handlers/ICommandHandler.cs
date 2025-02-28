using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Common.Handlers
{
    public interface ICommandHandler<in IRequest, TResult> : IRequestHandler<IRequest, TResult>
        where IRequest : IRequest<TResult>
    {
    }

    public interface ICommandHandler<TEntity> : IRequestHandler<TEntity>
        where TEntity : IRequest
    {
    }
}
