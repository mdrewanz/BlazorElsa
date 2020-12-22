using System;
using System.Net;
using System.Net.Http;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Activities.Http.Activities;
using Elsa.Activities.Timers.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using Elsa.Services.Models;

namespace BlazorElsa.Workflows
{
    public class RecurringWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .AsSingleton()
                .WithName("RecursiveFlow")
                .WithDescription("simple recursive workflow with a timer - continuous execution every 5 seconds.")
                .StartWith<ReceiveHttpRequest>(x =>
                {
                    x.Method = HttpMethod.Get.Method;
                    x.Path = new Uri("/recurring", UriKind.Relative);

                })
                .Then<TimerEvent>(x => x.TimeoutExpression = new LiteralExpression<TimeSpan>("00:00:05"))
                .Then<WriteLine>(x => x.TextExpression = new JavaScriptExpression<string>("`It's now ${new Date()}. Let's do this thing!`"));
        }
    }
}
