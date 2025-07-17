using WorkflowEngineApi.Models;

namespace WorkflowEngineApi.Services
{
    public class WorkflowService
    {
        private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
        private readonly Dictionary<Guid, WorkflowInstance> _instances = new();

        public bool AddWorkflowDefinition(WorkflowDefinition definition)
        {
            if (_definitions.ContainsKey(definition.Id))
                return false;

            if (definition.States.Count(s => s.IsInitial) != 1)
                return false;

            _definitions[definition.Id] = definition;
            return true;
        }

        public WorkflowDefinition? GetWorkflowDefinition(string id)
        {
            _definitions.TryGetValue(id, out var definition);
            return definition;
        }

        public WorkflowInstance? StartInstance(string defId)
        {
            if (!_definitions.TryGetValue(defId, out var definition))
                return null;

            var initialState = definition.States.First(s => s.IsInitial);
            var instance = new WorkflowInstance
            {
                Id = Guid.NewGuid(),
                DefinitionId = defId,
                CurrentState = initialState.Id,
                History = new List<StateTransition>()
            };

            _instances[instance.Id] = instance;
            return instance;
        }

        public bool ExecuteAction(Guid instanceId, string actionId)
        {
            if (!_instances.TryGetValue(instanceId, out var instance)) return false;
            if (!_definitions.TryGetValue(instance.DefinitionId, out var definition)) return false;

            var action = definition.Actions.FirstOrDefault(a => a.Id == actionId);
            if (action == null || !action.Enabled) return false;
            if (!action.FromStates.Contains(instance.CurrentState)) return false;

            var targetState = definition.States.FirstOrDefault(s => s.Id == action.ToState);
            if (targetState == null || !targetState.Enabled) return false;
            if (definition.States.First(s => s.Id == instance.CurrentState).IsFinal)
                return false;

            instance.CurrentState = action.ToState;
            instance.History.Add(new StateTransition { ActionId = actionId, Timestamp = DateTime.UtcNow });

            return true;
        }

        public WorkflowInstance? GetInstance(Guid id)
        {
            _instances.TryGetValue(id, out var instance);
            return instance;
        }
    }
}

