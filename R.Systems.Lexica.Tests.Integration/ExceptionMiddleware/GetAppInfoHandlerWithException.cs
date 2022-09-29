﻿using MediatR;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;

namespace R.Systems.Lexica.Tests.Integration.ExceptionMiddleware;

public class GetAppInfoHandlerWithException : IRequestHandler<GetAppInfoQuery, GetAppInfoResult>
{
    public Task<GetAppInfoResult> Handle(GetAppInfoQuery request, CancellationToken cancellationToken)
    {
        throw new Exception("Test Exception.");
    }
}
