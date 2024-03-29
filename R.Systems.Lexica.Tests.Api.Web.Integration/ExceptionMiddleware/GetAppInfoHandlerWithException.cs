﻿using MediatR;
using R.Systems.Lexica.Core.Queries.GetAppInfo;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.ExceptionMiddleware;

public class GetAppInfoHandlerWithException : IRequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    public Task<GetAppInfoResult> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        throw new Exception("Test Exception.");
    }
}
