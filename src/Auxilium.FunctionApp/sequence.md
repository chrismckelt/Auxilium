```mermaid

sequenceDiagram
    participant LogicApp
    participant FunctionApp
    participant LogicApp2
    participant LogAnalytics
    LogicApp->>+FunctionApp: List logic apps for subscription
    FunctionApp->>+LogicApp: List of logic apps
    LogicApp-->>LogicApp2: Query logic app run
    loop GetActionState
        LogicApp2->>LogicApp2: Extract state
    end 
    LogicApp2-->>FunctionApp: Return logic app run details and state
    FunctionApp-->>LogAnalytics: Insert Logic Apps run and state
    
```            
