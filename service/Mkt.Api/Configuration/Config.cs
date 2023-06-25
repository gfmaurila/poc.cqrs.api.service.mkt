using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mkt.Api.Filters;
using Mkt.App.Commands.Users.CreateUser;
using Mkt.App.Commands.Users.LoginUser;
using Mkt.App.Commands.Users.UpdateUser;
using Mkt.App.Consumers.User;
using Mkt.App.Validators.Users;
using Mkt.App.ViewModels.Users;
using Mkt.Core.Producer;
using Mkt.Core.Repositories;
using Mkt.Core.Services;
using Mkt.Infra.Auth;
using Mkt.Infra.MessageBus;
using Mkt.Infra.Persistence;
using Mkt.Infra.Persistence.Repositories;
using Mkt.Infra.Producer;
using Mkt.Infra.Services;

namespace Mkt.Api.Configuration;
public class Config
{
    public static void ConfigCommand(IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateUserCommand));
        services.AddMediatR(typeof(UpdateUserCommand));
    }

    public static void ConfigCommandValidator(IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter))).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());
        services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter))).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateUserCommandValidator>());
    }

    public static void ConfigDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MktDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    }

    public static void ConfigRepository(IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void ConfigService(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISendGridEmailService, SendGridEmailService>();
        services.AddScoped<ITwilioWhatsAppService, TwilioWhatsAppService>();
    }

    public static void ConfigRequestHandler(IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<LoginUserCommand, LoginUserViewModel>, LoginUserCommandHandler>();
    }

    public static void ConfigBusService(IServiceCollection services)
    {
        services.AddScoped<IMessageBusService, MessageBusService>();
        services.AddScoped<ISendGridProducer, SendGridProducer>();
        services.AddScoped<ITwilioWhatsAppProducer, TwilioWhatsAppProducer>();
        services.AddHostedService<SendGridGenerateCodeResetConsumer>();
        services.AddHostedService<TwilioGenerateCodeResetConsumer>();
    }
}
