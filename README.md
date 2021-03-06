# Eventual2PC

最终一致性2PC范式，提供多聚合根之间交互事务的抽象接口。任何基于cqrs + eda 实现多聚合根最终一致性的框架，都可基于此接口进行实现，以达到提高开发效率的目的。

由于事务控制也在业务端，所以此处的 `2PC` 概念，可以等效于 `TCC` 概念；目前不支持 `Timeout` 隐式 `Cancel`。

## 安装

```
dotnet add package Eventual2PC
```

## 实现案例

### 基于ENode的CQRS框架的实现

[ENode.Eventual2PC](https://github.com/berkaroad/ENode.Eventual2PC)

## 文档

### 2PC定义

2pc(two-phase commit protocol）是非常经典的强一致性、中心化的原子提交协议。中心化是指协议中有两类节点：一个中心化协调者节点（coordinator）和N个参与者节点（participant、cohort）。

协议的每一次事务提交分为两个阶段：

- 第一阶段，协调者询问所有的参与者是否可以提交事务（请参与者投票），所有参与者向协调者投票。

- 第二阶段，协调者根据所有参与者的投票结果做出是否事务可以全局提交的决定，并通知所有的参与者执行该决定。

在一个两阶段提交流程中，参与者不能改变自己的投票结果。两阶段提交协议的可以全局提交的前提是所有的参与者都同意提交事务，只要有一个参与者投票选择放弃(abort)事务，则事务必须被放弃。

![2PC Propose/Vote](https://the-paper-trail.org/wp-content/uploads/2010/01/tpc-fault-free-phase-1.png)

![2PC Commit/Abort](https://the-paper-trail.org/wp-content/uploads/2010/01/tpc-fault-free-phase-2.png)

文献参考：[https://www.the-paper-trail.org/post/2008-11-27-consensus-protocols-two-phase-commit/](https://www.the-paper-trail.org/post/2008-11-27-consensus-protocols-two-phase-commit/)

### TCC定义

TCC（Try-Confirm-Cancel）又称补偿事务。其核心思想是："针对每个操作都要注册一个与其对应的确认和补偿（撤销操作）"。它分为三个操作：

- Try阶段：主要是对业务系统做检测及资源预留。

- Confirm阶段：确认执行业务操作。

- Cancel阶段：取消执行业务操作。

TCC事务的处理流程与2PC两阶段提交类似，不过2PC通常都是在跨库的DB层面，而TCC本质上就是一个应用层面的2PC，需要通过业务逻辑来实现。这种分布式事务的实现方式的优势在于，可以让应用自己定义数据库操作的粒度，使得降低锁冲突、提高吞吐量成为可能。

为了满足一致性的要求，confirm和cancel接口还必须实现幂等。

![TCC](https://www.enterpriseintegrationpatterns.com/img/TryConfirmCancelState.png)

文献参考: [https://www.enterpriseintegrationpatterns.com/patterns/conversation/TryConfirmCancel.html](https://www.enterpriseintegrationpatterns.com/patterns/conversation/TryConfirmCancel.html)

### 术语

- `Initiator`: 事务发起方，它是聚合根，用于维护事务状态；如果本身也参与当前事务，那么在发起事务时的业务校验等效于`PreCommit`，在处理 `TransactionCompleted` 事件时修改自身状态等效于 `Commit`

- `ProcessManager`: CQRS中的概念，作为事务相关消息路由的角色，用于协调 `Participant`

- `Coordinator`: 事务协调者，是 `Initiator` + `ProcessManager` 的概念总和

- `Participant`: 事务参与方（仅针对被修改的聚合根，因为新增不会产生业务失败问题；若需要新增的聚合根参与事务，可以在 `ProcessManager` 响应 `AllParticipantPreCommitSucceed` 事件时发送命令消息），它是聚合根，负责接受 `PreCommit`、`Commit`、`Rollback`以处理自身业务

- `Preparation`: `Participant` 的事务准备，表示参与事务的业务修改行为，一个事务准备可以用于不同事务

- `Transaction`: 2PC事务，可以只是一个标识ID（通常使用 `TransactionStarted` 事件的ID），也可以使用代表事务的聚合根（如银行转账的转账事务聚合根）

#### Initiator 命令

- `AddPreCommitSucceedParticipant`: 添加预提交成功的参与方

- `AddPreCommitFailedParticipant`: 添加预提交失败的参与方

- `AddCommittedParticipant`: 添加已提交的参与方

- `AddRolledbackParticipant`: 添加已回滚的参与方

#### Initiator 事件

- `TransactionStarted`: 事务已发起事件

- `PreCommitSucceedParticipantAdded`: 预提交成功的参与者已添加事件

- `PreCommitFailedParticipantAdded`: 预提交失败的参与者已添加事件

- `AllParticipantPreCommitSucceed`: 所有参与者预提交已成功事件

- `AnyParticipantPreCommitFailed`: 任意一个参与者预提交已失败事件

- `CommittedParticipantAdded`: 已提交的参与者已添加事件

- `RolledbackParticipantAdded`: 已回滚的参与者已添加事件

- `TransactionCompleted`: 事务已完成事件，并包含是否事务已提交的状态

#### Participant 命令

- `PreCommit`: 预提交

- `Commit`: 提交

- `Rollback`: 回滚

#### Participant 事件

- `PreCommitSucceed`: 预提交已成功事件

- `PreCommitFailed`: 预提交已失败事件（或领域异常消息）

- `Committed`: 已提交事件

- `Rolledback`: 已回滚事件

### 规约

- 一个聚合根，可以同时扮演 `Initiator` 和 `Transaction`的角色，如银行转账事务聚合根

- 一个聚合根，可以同时扮演事务A中的 `Participant` 和事务B的 `Initiator`

- `Initiator` 的聚合根实例，发起事务时，必须存在至少一个  `Participant`，且不能把自己作为 `Participant`；如果自己参与事务中，则通过发起事务都位置进行业务校验，在处理 `TransactionCompleted` 事件时提交业务修改

- `Initiator` 的聚合根实例，如果处于事务A中，那么将不允许作为事务B的 `Participant`，直到事务A结束，才允许

- `Initiator` 的聚合根实例，仅允许发起一个事务，只有事务完成后，才可以发起其他事务；此处的事务完成，以是否发布`TransactionCompleted`事件为准

- `Participant` 参与事务的业务修改行为有多少个，对应定义多少个`Preparation`，与参与的事务数无关

- `Participant` 的聚合根实例，允许同时参与多个不同的事务；也可以通过业务代码，在 `PreCommit` 时，判断是否存在其他类型的 `Preparation` 来阻止当前 `Preparation` 的 `PreCommit` 操作

- `Initiator` 和 `Participant` 的命令处理都必须支持幂等

### 处理流程

- `Initiator` 发布 `TransactionStarted` 事件

- `ProcessManager` 响应 `TransactionStarted` 事件，并发送 `PreCommit` 命令

- `Participant` 处理命令 `PreCommit`

    - 如果成功，则发布 `PreCommitSucceed` 事件；
    
    - 如果失败，则发布 `PreCommitFailed` 事件（或领域异常）。

- `ProcessManager` 响应 `PreCommitSucceed`，并发送 `AddPreCommitSucceedParticipant` 命令

- `ProcessManager` 响应 `PreCommitFailed`，并发送 `AddPreCommitFailedParticipant` 命令

- `Initiator` 处理命令 `AddPreCommitSucceedParticipant`，发布 `PreCommitSucceedParticipantAdded` 事件

    - 如果所有 `Participant` 的 `PreCommit` 都已处理完成且都成功，则再发布 `AllParticipantPreCommitSucceed` 事件；
    
    - 如果已处理完成，但存在失败，则再发布 `AnyParticipantPreCommitFailed` 事件。

- `Initiator` 处理命令 `AddPreCommitFailedParticipant`，发布 `PreCommitFailedParticipantAdded` 事件

    - 如果所有 `Participant` 的 `PreCommit` 都已处理完成，则发布 `AnyParticipantPreCommitFailed` 事件；
    
    - 如果已处理完成，且都失败，则再发布 `TransactionCompleted` 事件。

- `ProcessManager` 响应 `AllParticipantPreCommitSucceed`，并发送 `Commit` 命令

- `ProcessManager` 响应 `AnyParticipantPreCommitFailed`，并发送 `Rollback` 命令

- `Participant` 处理命令 `Commit`，并发布 `Committed` 事件

- `Participant` 处理命令 `Rollback`，并发布 `Rolledback` 事件

- `ProcessManager` 响应 `Committed`，并发送 `AddCommittedParticipant` 命令

- `ProcessManager` 响应 `Rolledback`，并发送 `AddRolledbackParticipant` 命令

- `Initiator` 处理命令 `AddCommittedParticipant`，发布 `CommittedParticipantAdded` 事件

    - 如果所有 `Participant` 的 `Commit` 都已处理完成，则再发布 `TransactionCompleted` 事件。

- `Initiator` 处理命令 `AddRolledbackParticipant`，发布 `RolledbackParticipantAdded` 事件

    - 如果所有 `Participant` 的 `Rolledback` 都已处理完成，则再发布 `TransactionCompleted` 事件。

## 发布历史

### 1.1.0（2020/04/30）

- 1）`ITransactionInitiator`接口增加属性 `CurrentTransactionId`、`CurrentTransactionType`

- 2）增加事务流转过程中产生的 `Command` 的接口定义

### 1.0.2（2020/4/28）

- 移除异常类 `UnknownTransactionPreparationException`

### 1.0.1（2020/4/27）

- 移除 `TransactionParticipantInfo` 的方法 `ValidateParticipantMustNotExists`，是否抛出异常，由使用方决定

### 1.0.0（2020/4/25）

- 初始版本

## 鸣谢

非常感谢 ENode群，热心群友提出的各种异议，以及 [ENode](http://github.com/tangxuehua/enode) 的作者 汤雪华 阐述2PC概念、ENode设计理念，提供了宝贵的意见。