# Eventual2PC

最终一致性二阶段提交范式，简化多聚合根之间交互事务的实现。任何基于cqrs + eda 实现多聚合根最终一致性的框架，都可使用。

## 安装

```
dotnet add package Eventual2PC
```

## 文档

### 术语定义

- `Initiator`: 作为事务发起方，它是聚合根

- `Participant`: 作为事务参与方（仅被修改的聚合根，新增的，不会产生业务失败问题），它是聚合根

- `Preparation`: 事务准备，对应一个具体的业务修改动作；同一个 `Participant` 参与多个事务时，应定义多个事务准备

- `Transaction`: 2PC事务，从开始事务，到事务完成，贯穿整个事务生命周期；可以只是一个标识ID（通常使用 `TransactionStarted` 事件的ID），也可以使用代表事务的聚合根（如银行转账的转账事务聚合根）

- `ProcessManager`: CQRS中的概念，作为事务相关消息路由的角色，负责响应 `DomainEvent`、`DomainException` 消息，并发送 `Command` 消息

#### Initiator 事件定义

- `TransactionStarted`: 事务已发起事件

- `PreCommitSucceedParticipantAdded`: 预提交成功的参与者已添加事件

- `PreCommitFailedParticipantAdded`: 预提交失败的参与者已添加事件

- `AllParticipantPreCommitSucceed`: 所有参与者预提交已成功事件

- `AnyParticipantPreCommitFailed`: 任意一个参与者预提交已失败事件

- `CommittedParticipantAdded`: 已提交的参与者已添加事件（Option）

- `RolledbackParticipantAdded`: 已回滚的参与者已添加事件（Option）

- `TransactionCompleted`: 事务已完成事件（Option），并包含是否事务已提交的状态

#### Participant 事件定义

- `PreCommitSucceed`: 预提交已成功事件

- `PreCommitFailed`: 预提交已失败事件（或领域异常消息）

- `Committed`: 已提交事件

- `Rolledback`: 已回滚事件


### 规约

- 一个聚合根，可以同时扮演 `Initiator` 和 `Transaction`的角色，如银行转账事务聚合根

- 一个聚合根，可以同时扮演事务A中的 `Participant` 和事务B的 `Initiator`

- `Initiator` 的聚合根实例，发起事务时，必须存在至少一个  `Participant`，且不能把自己作为 `Participant`

- `Initiator` 的聚合根实例，如果处于事务A中，那么将不允许作为事务B的 `Participant`，直到事务A结束，才允许

- `Initiator` 的聚合根实例，仅允许发起一个事务，只有事务完成后，才可以发起其他事务；此处的事务完成，可以是 `AllParticipantPreCommitSucceed`、`AnyParticipantPreCommitFailed`、`TransactionCompleted` 之一

- 一个 `Participant`，参与多少个事务，需对应定义多少个`Preparation`

- `Participant` 的聚合根实例，允许同时参与多个不同的事务；也可以通过业务代码，在 `PreCommit` 时，判断是否存在其他类型的 `Preparation` 来阻止参与多个事务

- `Initiator` 必须发布事件 `TransactionStarted`、`PreCommitSucceedParticipantAdded`、`PreCommitFailedParticipantAdded`、`AllParticipantPreCommitSucceed`、`AnyParticipantPreCommitFailed`

- `Participant` 必须发布事件 `PreCommitSucceed`、`PreCommitFailed`、`Committed`、`Rolledback`

- 如果需要关注 `Transaction` 是否已完成，则 `Initiator`  需要发布事件 `CommittedParticipantAdded`、`RolledbackParticipantAdded`、`TransactionCompleted`

### 处理流程

- `Initiator` 发布 `TransactionStarted` 事件；

- `ProcessManager` 响应 `TransactionStarted` 事件，并发送 `PreCommit` 命令；

- `Participant` 接受命令 `PreCommit`，如果成功，则发布 `PreCommitSucceed` 事件；如果失败，则发布 `PreCommitFailed` 事件（或领域异常）；

- `ProcessManager` 响应 `PreCommitSucceed`，并发送 `AddPreCommitSucceedParticipant` 命令；

- `ProcessManager` 响应 `PreCommitFailed`，并发送 `AddPreCommitFailedParticipant` 命令；

- `Initiator` 接受命令 `AddPreCommitSucceedParticipant`，发布 `PreCommitSucceedParticipantAdded` 事件；如果所有 `Participant` 的 `PreCommit` 都已处理完成，则发布 `AllParticipantPreCommitSucceed` 事件；

- `Initiator` 接受命令 `AddPreCommitFailedParticipant`，发布 `PreCommitFailedParticipantAdded` 事件；如果所有 `Participant` 的 `PreCommit` 都已处理完成，则发布 `AnyParticipantPreCommitFailed` 事件；

- `ProcessManager` 响应 `AllParticipantPreCommitSucceed`，并发送 `Commit` 命令；

- `ProcessManager` 响应 `AnyParticipantPreCommitFailed`，并发送 `Rollback` 命令；

- `Participant` 接受命令 `Commit`，并发布 `Committed` 事件；

- `Participant` 接受命令 `Rollback`，并发布 `Rolledback` 事件；

- `ProcessManager` 响应 `Committed`，并发送 `AddCommittedParticipant` 命令；

- `ProcessManager` 响应 `Rolledback`，并发送 `AddRolledbackParticipant` 命令；

- `Initiator` 接受命令 `AddCommittedParticipant`，发布 `CommittedParticipantAdded` 事件；如果所有 `Participant` 的 `Commit` 都已处理完成，则发布 `TransactionCompleted` 事件；

- `Initiator` 接受命令 `AddRolledbackParticipant`，发布 `RolledbackParticipantAdded` 事件；如果所有 `Participant` 的 `Rolledback` 都已处理完成，则发布 `TransactionCompleted` 事件。

## 发布历史

### 1.0.2（2020/4/28）

- 移除异常类 `UnknownTransactionPreparationException`

### 1.0.1（2020/4/27）

- 移除 `TransactionParticipantInfo` 的方法 `ValidateParticipantMustNotExists`，是否抛出异常，由使用方决定

### 1.0.0（2020/4/25）

- 初始版本
