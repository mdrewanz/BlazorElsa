using BlazorElsa.Models;
using BlazorElsa.Services;
using Elsa;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorElsa.Activities
{
    [ActivityDefinition(Category = "Users", Description = "create a user", Icon = "fas fa-user-plus", Outcomes = new[] { OutcomeNames.Done })]
    public class CreateUser : Activity
    {
        private readonly IMongoCollection<User> store;
        private readonly IIdGenerator idGenerator;
        private readonly IPasswordHasher passwordHasher;

        public CreateUser(IMongoCollection<User> store, IIdGenerator idGenerator, IPasswordHasher passwordHasher)
        {
            this.store = store;
            this.idGenerator = idGenerator;
            this.passwordHasher = passwordHasher;
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the name of the user to create.")]
        public WorkflowExpression<string> UserName
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the email address of the user to create.")]
        public WorkflowExpression<string> Email
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        [ActivityProperty(Hint ="Enter an expression that evaluates to the password of the user to create.")]
        public WorkflowExpression<string> Password
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var password = await context.EvaluateAsync(Password, cancellationToken);
            var hashedPassword = passwordHasher.HashPassword(password);

            var user = new User
            {
                Id = idGenerator.Generate(),
                Name = await context.EvaluateAsync(UserName, cancellationToken),
                Email = await context.EvaluateAsync(Email, cancellationToken),
                Password = hashedPassword.Hashed,
                PasswordSalt = hashedPassword.Salt,
                IsActive = false
            };

            await store.InsertOneAsync(user, cancellationToken: cancellationToken);

            Output.SetVariable("User", user);

            return Done();
        }
    }
}
