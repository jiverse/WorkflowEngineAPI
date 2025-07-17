namespace WorkflowEngineApi.Models
{
    public class State
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsInitial { get; set; }
        public bool IsFinal { get; set; }
        public bool Enabled { get; set; } = true;
    }

    public class Action
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> FromStates { get; set; } = new();
        public string ToState { get; set; } = "";
        public bool Enabled { get; set; } = true;
    }

    public class WorkflowDefinition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public List<State> States { get; set; } = new();
        public List<Action> Actions { get; set; } = new();
    }

    public class StateTransition
    {
        public string ActionId { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }

    public class WorkflowInstance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DefinitionId { get; set; } = "";
        public string CurrentState { get; set; } = "";
        public List<StateTransition> History { get; set; } = new();
    }
}
