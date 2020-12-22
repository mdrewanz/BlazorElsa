using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using Elsa;
using Elsa.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Activities.Email.Activities;
using Elsa.Activities.Http.Activities;
using Elsa.Activities.Timers.Activities;
using Elsa.Activities.Workflows.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using Elsa.Services.Models;


namespace BlazorElsa.Workflows
{
    public class PaymentRequestWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WithName("PaymentApproval")
                .WithDescription("sample document approval workflow to show some of the features of Elsa.")
                .StartWith<ReceiveHttpRequest>(x =>
                {
                    x.Method = HttpMethod.Post.Method;
                    x.Path = new Uri("/payment-request", UriKind.Relative);
                    x.ReadContent = true;
                })
                .Then<SetVariable>(x =>
                {
                    x.VariableName = "Document";
                    x.ValueExpression = new JavaScriptExpression<ExpandoObject>("lastResult().Body");
                })
                .Then<SendEmail>(x =>
                {
                    x.From = new LiteralExpression("ApprovalRequest@rbcapital.com");
                    x.To = new JavaScriptExpression<string>("Document.Aprovadores.NivelA.Email");
                    x.Subject = new JavaScriptExpression<string>("'[APROVACAO]: Pagamento ${Document.Fornecedor.Nome}'");
                    x.Body = new JavaScriptExpression<string>(
                            "`Aprovação pagamento enviada por ${Document.Requisitante.Nome}" +
                            "<br /> Valor ${Document.Valor} <br /> Centro Custo: ${Document.CentroCusto.Codigo}-${Document.CentroCusto.Nome}" +
                            "<br /> Projeto: ${Document.Projeto} <br /> Descrição: ${Document.Descricao} <br />" +
                            "<a href=\"${signalUrl('Approve')}\">Aprovar</a> ou <a href=\"${signalUrl('Reject')}\">Rejeitar</a>`");
                });
        }
    }
}
