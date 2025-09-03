# beta-bot-climbing-analytics
AWS + React + DynamoDB chatbot for querying personal climbing data from Mountain Project

# 🧗‍♂️ Beta Bot Climbing Analytics

**An AWS‑powered chatbot and analytics platform for querying your personal climbing history.**  
Upload your Mountain Project "Ticks" (completed climbs) and ask natural‑language questions like:

> *"How many 5.12a climbs have I done?"*  
> *"How many 5.12a climbs in Wyoming?"*  
> *"How many 5.12 and harder climbs have I completed in Ten Sleep?"*

---

## 🚀 Overview

**Beta Bot Climbing Analytics** is a full‑stack project designed to:
- Showcase **senior‑level system design** and cloud architecture skills.
- Demonstrate **event‑driven, serverless, and streaming patterns** at scale.
- Provide a fun, personal use‑case for climbing enthusiasts.

It ingests your climbing data from Mountain Project, stores it in DynamoDB, and exposes both:
- A **React SPA** for upload, exploration, and chat.
- A **natural‑language chatbot** that translates questions into structured queries.

---

## 🏗 Architecture

**Phase 1 (MVP – Serverless‑first)**  
- **Frontend:** React + TypeScript, hosted on S3 + CloudFront  
- **Auth:** AWS Cognito (JWT‑secured APIs)  
- **Import Pipeline:** S3 → SQS → Lambda → DynamoDB (with Step Functions for large jobs)  
- **Data Store:** DynamoDB with GSIs for grade/area queries  
- **Aggregates:** DynamoDB Streams → Lambda for materialized views  
- **APIs:** API Gateway + Lambda (REST endpoints for imports, stats, chat)  
- **IaC:** AWS CDK (TypeScript)  
- **CI/CD:** GitHub Actions with OIDC to AWS

**Phase 2 (Advanced Depth)**  
- **Streaming Backbone:** Amazon MSK (Kafka) for replayable ingestion and enrichment  
- **Containerized Services:** EKS microservices for advanced processing  
- **Scale & Reliability:** Backpressure, DLQs, chaos testing, replay/backfill

---

## 📊 Example Workflow

1. **Authenticate** via Cognito in the SPA.
2. **Upload** your Mountain Project "Ticks" CSV via pre‑signed S3 URL.
3. **Pipeline** parses, validates, and stores ticks in DynamoDB.
4. **Aggregators** update counts by grade, area, and date.
5. **Ask** the chatbot a question in natural language.
6. **Translator** converts your question into a constrained Query DSL.
7. **Query Service** fetches results from aggregates or DynamoDB.
8. **Response** is returned to the chat UI with a short rationale.

---

## 🧩 Tech Stack

| Layer        | Technology |
|--------------|------------|
| **Frontend** | React, TypeScript, React Query, zod |
| **Backend**  | AWS Lambda, API Gateway, Step Functions |
| **Data**     | DynamoDB, DynamoDB Streams |
| **Streaming**| Amazon MSK (Kafka) *(Phase 2)* |
| **Auth**     | AWS Cognito |
| **IaC**      | AWS CDK (TypeScript) |
| **CI/CD**    | GitHub Actions (OIDC to AWS) |
| **Observability** | CloudWatch, X‑Ray, SNS alerts |

---

## 🗂 Project Structure
beta-bot-climbing-analytics/ 
├── infra/ # CDK stacks 
├── backend/ # Lambda handlers, API logic 
├── frontend/ # React SPA 
├── shared/ # Shared types, utils, zod schemas 
├── data-contracts/ # API request/response contracts 
└── README.md


---

## 🛠 Local Development

**Prerequisites:**
- Node.js 18+
- Yarn or pnpm
- AWS CLI configured
- GitHub CLI (for CI/CD testing)

**Setup:**
```bash
# Clone the repo
git clone https://github.com/<your-username>/beta-bot-climbing-analytics.git
cd beta-bot-climbing-analytics

# Install dependencies
yarn install

# Bootstrap CDK (first time only)
cd infra
cdk bootstrap

# Start frontend (dev mode)
cd ../frontend
yarn dev

---

📈 Roadmap
[x] Repo + CI/CD bootstrap

[ ] Auth + SPA scaffold

[ ] MVP import pipeline

[ ] Aggregates + stats API

[ ] Chat NL‑to‑Query

[ ] Load testing & reliability

[ ] Kafka + EKS advanced processing

🤝 Contributing
This is a personal portfolio project, but PRs and suggestions are welcome. If you’re a climber + dev, I’d love to hear your ideas for new analytics or visualizations.

📜 License
MIT License – see LICENSE for details.

🧗 About the Name
In climbing, beta is the insider knowledge about how to complete a route. This bot gives you the beta on your own climbing history — with analytics to match.
