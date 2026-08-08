# Efficient DL Notes 🚀

欢迎来到我的深度学习（DL）高效推理与训练技术知识库！

本仓库主要记录通过与 Gemini 等 AI 深度对话、查阅最新论文及工程实践所沉淀的硬核干货。核心目标是探究：**如何利用最新的算法、模型融合与硬件架构优化，用更少的资源（特别是显存）实现更快、更好的模型推理与训练。**

---

## 🗺️ 技术知识导航

点击下方目录可直接跳转至对应领域的结构化笔记：

### 🧱 01. 硬件架构与底层优化 (Hardware Architecture)
* [GPU 架构深度解析](./01-Hardware-Architecture/GPU-Architecture.md) — Tensor Core、显存带宽与算力瓶颈分析。
* [TPU 架构与编译优化](./01-Hardware-Architecture/TPU-Architecture.md) — 矩阵乘法单元（MXU）与 XLA 编译机制。

### ✂️ 02. 模型压缩与显存管理 (Model Compression)
* [低比特量化技术](./02-Model-Compression/Quantization.md) — INT8/INT4、AWQ、GPTQ 及 KV Cache 量化实践。
* [显存节省算法](./02-Model-Compression/Memory-Efficient.md) — FlashAttention 变体、Activation Checkpointing（算力换显存）。
* [结构化剪枝与知识蒸馏](./02-Model-Compression/Pruning-Distill.md) — 减小模型体积与加速推理的组合拳。

### ⚡ 03. 高效推理引擎与最新算法 (Inference Engine)
* [vLLM 极致吞吐优化](./03-Inference-Engine/vLLM-Optimization.md) — PagedAttention 机制与 Continuous Batching 拆解。
* [TensorRT-LLM 部署实践](./03-Inference-Engine/TensorRT-LLM.md) — NVIDIA 官方极致加速方案与模型转换流程。
* [投机采样算法 (Speculative Decoding)](./03-Inference-Engine/Speculative-Decoding.md) — 用小模型引导大模型，打破自回归推理速度限制。

### 🤝 04. 模型融合与高效微调 (Model Merging)
* [前沿模型融合算法](./04-Model-Merging/Model-Soup-MoE.md) — Model Soups、SLERP、以及无损转换为 MoE（混合专家模型）。
* [参数高效微调 (PEFT)](./04-Model-Merging/PEFT-LoRA.md) — LoRA、QLoRA、DoRA 的显存收益与训练速度横向对比。

---

## 📝 笔记整理规范

为了保证每篇笔记的“信息密度”，所有问答均抛弃 AI 的客套话，严格采用以下结构归档：
1. **💡 核心痛点与结论**：用 3 句话说清这个技术解决了什么“显存/速度”痛点，以及最终结论。
2. **🛠️ 关键原理 / 核心代码**：不贴冗长的全量代码，只保留最核心的算法公式、对比表格或关键配置。
3. **🧠 深度追问 (Deep Dive)**：记录打破沙锅问到底的边缘案例（Edge Cases）与技术边界。

---

## 📈 追踪的前沿技术栈

本仓库长期关注并保持更新的技术标签：
`FlashAttention-3` | `vLLM` | `TensorRT-LLM` | `AWQ/GPTQ` | `Model Merging` | `Speculative Decoding` | `XLA`

---
*“用最少的 VRAM，跑最快的 Inference。” 如果这些笔记对你有启发，欢迎点个 Star 🌟 一起交流！*
