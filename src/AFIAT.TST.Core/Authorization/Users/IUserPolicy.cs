﻿using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace AFIAT.TST.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
