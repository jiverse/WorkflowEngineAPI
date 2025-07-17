# Workflow Engine API
A simple, minimal backend service built using .NET 8 and C# that allows us to:

*Define custom workflow state machines (with states and actions)

*Start instances of these workflows

*Transition between states by performing actions

*Track the current state and history of each instance

## Tech Stack
Backend Framework: ASP.NET Core Minimal API (.NET 8)

Language: C#

Tools for testing: Postman / curl / browser


# How to Use the Workflow Engine API

This project lets us define custom workflows (like state machines), start new instances of them, and transition between states based on actions.


## 1️⃣ Start the API

* Run the project using Visual Studio, `dotnet run`.
* Once it’s running, we’ll see something like:

  ```
  Now listening on: http://localhost:5068
  ```

---

## 2️⃣ Create a Workflow Definition

This is where we define our workflow — all the states it can be in, and the actions that move it from one state to another.

In Postman:

* Method: `POST`
* URL: `http://localhost:5068/workflow-definitions`
* Body (JSON):

```json
{
  "id": "my-workflow",
  "name": "Sample Workflow",
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false },
    { "id": "middle", "name": "Middle", "isInitial": false, "isFinal": false },
    { "id": "end", "name": "End", "isInitial": false, "isFinal": true }
  ],
  "actions": [
    { "id": "begin", "name": "Begin", "fromStates": ["start"], "toState": "middle" },
    { "id": "finish", "name": "Finish", "fromStates": ["middle"], "toState": "end" }
  ]
}
```

* We will  get a `200 OK` if it works.

---

## 3️⃣ Start a New Workflow Instance

Now that the workflow is defined, we can start a new instance of it.

* Method: `POST`
* URL: `http://localhost:5068/workflow-definitions/my-workflow/start`

We’ll get a response like:

```json
{
  "id": "99eba76a-c1df-4299-9664-e71bdda51799",
  "definitionId": "my-workflow",
  "currentState": "start",
  "history": []
}
```

➡️ Next, we will copy the `id` from the response. And that’s our `instanceId`.

---

## 4️⃣ Execute an Action (Transition to the Next State)

We can now move this instance from one state to the next using an action.

* To move from **start** to **middle**:

  * Method: `POST`
  * URL:

    ```
    http://localhost:5068/instances/{instanceId}/actions/begin
    ```

* Then we will move from **middle** to **end**:

  * POST
  * URL:

    ```
    http://localhost:5068/instances/{instanceId}/actions/finish
    ```

Just replace the `{instanceId}` with the ID  copied earlier.

---

## 5️⃣ View the Current State and History

We can see what state our instance is in by using the below action.

* Method: `GET`
* URL:

  ```
  http://localhost:5068/instances/{instanceId}
  ```

We’ll get something like:

```json
{
  "id": "99eba76a-c1df-4299-9664-e71bdda51799",
  "definitionId": "my-workflow",
  "currentState": "end",
  "history": [
    { "actionId": "begin", "timestamp": "..." },
    { "actionId": "finish", "timestamp": "..." }
  ]
}

