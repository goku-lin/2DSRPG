# SmartFlow 协同平台（简历项目）

这是一个可直接写入简历的 ASP.NET 全栈项目模板，目标是体现**复杂业务建模 + 工程化能力 + 实时通信 + 可视化分析**。

## 技术栈
- ASP.NET Core Web API + Controller
- EF Core InMemory（可替换 SQL Server）
- JWT 身份认证 + 角色授权
- SignalR 实时通知
- Chart.js 数据可视化
- Bootstrap 前端控制台

## 亮点（可写简历）
1. 设计了项目、任务、工时、风险预警等核心域模型，支持敏捷交付流程。
2. 使用 JWT + RBAC，保障接口与实时消息链路安全。
3. 通过 Dashboard 聚合服务输出燃尽图、状态分布、风险任务清单。
4. 通过 SignalR 推送任务创建/流转事件，实现多角色实时协同。
5. 提供可运行前端控制台，支持登录、项目切换、看板与图表联动。

## VS 2026 使用建议
1. 打开 `ResumeProject/ResumeProject.csproj`。
2. 运行前将 InMemory 替换为 SQL Server（`UseSqlServer`），并增加迁移。
3. 前端可继续升级为 Blazor WebAssembly 或 React + ASP.NET Hosted。

## 可继续增强
- 引入 Redis + Hangfire 实现延迟任务与告警重试。
- 接入 OpenTelemetry + Prometheus + Grafana 做链路监控。
- 使用 CQRS + MediatR + Outbox 模式提升一致性与扩展性。
