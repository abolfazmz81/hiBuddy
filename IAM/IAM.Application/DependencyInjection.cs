﻿using IAM.Application.AuthenticationService;
using IAM.Application.UserManagement;
using Microsoft.Extensions.DependencyInjection;

namespace IAM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IAuthPhoneRegister, AuthPhoneRegister>();
        services.AddScoped<IAuthPhoneVerify, AuthPhoneVerify>();
        services.AddScoped<IUserManagement,UserManagement.UserManagement>();
        services.AddScoped<ILoginService, LoginService>();
        return services;
    }
}