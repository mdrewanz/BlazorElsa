using BlazorElsa.Models;
using Elsa.Scripting.Liquid.Messages;
using MediatR;
using Fluid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorElsa.Handlers
{
    /// <summary>
    /// configure the Liquid template context to allow access to certain models
    /// </summary>
    public class LiquidConfigurationHandler : INotificationHandler<EvaluatingLiquidExpression>
    {
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancell)
        {
            var context = notification.TemplateContext;

            context.MemberAccessStrategy.Register<User>();
            context.MemberAccessStrategy.Register<RegistrationModel>();

            return Task.CompletedTask;
        }
    }
}
