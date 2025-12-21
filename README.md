# LuckIsland 项目大纲

## 一、项目描述

### 项目概述

**LuckIsland（幸运岛）** 是一款基于Unity引擎开发的回合制桌面游戏，核心玩法融合了大富翁的地产经营机制与卡牌策略元素。游戏采用高度模块化的架构设计，Logic层与View层完全分离，支持未来扩展为联机多人游戏。

**进展** 持续开发中
# LuckIsland 项目进度表

| 系统/模块 | 状态 | 完成度 | 说明 |
|----------|------|--------|------|
| **核心框架** | ✅ | 100% | Singleton、EventBus、SystemBase、Grid系统 |
| **GameManager** | 🟡 | 60% | 基础框架完成，System管理待完善 |
| **MapSystem** | 🟡 | 60% | 地图管理（Tile/Land关联待完善） |
| **PlayerSystem** | 🟡 | 10% | 基础框架，玩家数据/经济逻辑待完善 |
| **TurnSystem** | 🟡 | 30% | 基础框架，回合流程待完善 |
| **DiceSystem** | ❌ | 0% | 待实现 |
| **MoveSystem** | ❌ | 0% | 待实现 |
| **PropertySystem** | ❌ | 0% | 待实现 |
| **BuildingSystem** | ❌ | 0% | Logic层已完成，System待实现 |
| **CardSystem** | ❌ | 0% | 待实现 |

## Logic层

| Logic类 | 状态 | 完成度 |
|---------|------|--------|
| **MapLogic/TileLogic/LandLogic** | ✅ | 80-90% |
| **PlayerLogic** | ❌ | 30% |
| **BuildingLogic** | 🟡 | 60% |
| **BuildingEffect** | ❌ | 20% | 建筑效果系统（Start/Shop/Property） |

## View层

| View组件 | 状态 | 完成度 |
|---------|------|--------|
| **TileView/LandView/BuildingView** | 🟡 | 60% |
| **PlayerView** | ❌ | 0% |
| **UI系统** | ❌ | 0% |


**总体完成度：约 20%**

### 下一步优先级

1. **TurnSystem** 完善（回合流程）
2. **DiceSystem** 实现
3. **MoveSystem** 实现
4. **PlayerSystem** 完善（经济系统）
5. **PropertySystem** 实现


### 核心玩法

- **回合制流程**：玩家轮流投骰子移动，触发地块事件，使用卡牌策略
- **地图系统**：由Cube组成的地图（Land、Tile、Env），Tile与相邻Land关联，玩家在Tile上移动，可对相邻Land进行操作
- **建筑系统**：包含Start（起点）、Shop（商店）、Property（地产，可建造升级）三种建筑类型
- **经济系统**：金币管理、土地购买、租金收取、建筑升级
- **卡牌系统**：策略卡牌、事件卡牌，影响游戏进程

### 架构特点

- **逻辑表现分离**：Logic层纯C#实现，无Unity依赖，可独立测试和运行
- **事件驱动**：通过EventBus实现系统间解耦通信
- **数据驱动**：使用ScriptableObject配置游戏数据，支持序列化存档
- **网络预留**：通过接口抽象，可无缝切换本地/联网模式