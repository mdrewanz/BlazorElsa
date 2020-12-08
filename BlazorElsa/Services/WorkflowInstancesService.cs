using Elsa.Extensions;
using Elsa.Models;
using Elsa.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorElsa.Services
{
    public class WorkflowInstancesService
    {
        private readonly IWorkflowInstanceStore store;

        public WorkflowInstancesService(IWorkflowInstanceStore store)
        {
            this.store = store;
        }

        public async Task<IList<WorkflowInstance>> GetAllAsync()
        {
            return await store.ListAllAsync().ToListAsync();
        }
    }
}
