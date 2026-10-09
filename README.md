# GeoPath

GeoPath is a geometry learning platform designed to help South African high school learners understand Euclidean geometry through guided problem-solving, interactive diagrams, and structured reasoning. The platform combines EucliGeo’s educational approach with GeoPath’s practice engine to support conceptual learning, proof building, performance tracking, and class analytics.

## Project Overview

The platform is built around a learning model that guides students through four thinking stages:

- Abstraction
- Decomposition
- Pattern recognition
- Solving

Learners work through geometry examples using interactive diagrams, theorem references, definitions, and a proof builder. The product is designed to move students beyond memorisation toward reasoning and mastery.

## Target Users

### Learners
- High school students studying Euclidean geometry
- Need guided practice and step-by-step reasoning support
- Benefit from saved progress, assignments, and mastery insights

### Teachers
- Manage classes and assign learning activities
- Track learner progress and identify weak areas
- Access class-level reports and analytics

### Authors / Content Managers
- Create, review, and publish geometry examples, definitions, and theorems
- Ensure content is accurate and pedagogically effective

## Key Features

- Interactive Euclidean geometry diagrams
- Guided learning flow with four reasoning stages
- Theorem and definition library
- Proof-building and step validation
- Learner progress and mastery tracking
- Class analytics and teacher dashboards
- Assignment and performance reporting

## Repository Structure

This repository currently includes the design and prototype work for the product:

- `ProjectStructure/EucliGeo and GeoPath combined design.html` – concept design combining the two products
- `ProjectStructure/EucliGeo wireframes.html` – wireframes for different user flows and roles
- `ProjectStructure/EucliGeo clickable prototype.html` – interactive prototype of the application experience

## Tech Stack

The project is planned to use the following technology stack:

- Backend: C# and .NET
- Database: PostgreSQL
- Front-end: Angular
- UI/UX prototype: HTML, CSS, JavaScript, and SVG for the current design exploration

## Folder Layout

client/src/app
    core                    

server

## Running Locally

Start the API from the repository root:

```bash
cd server
dotnet run --project src/GeoPath.Api
```

The API will be available at `http://localhost:5229`.

In a separate terminal, install the client dependencies and start the Angular development server:

```bash
cd client/geo-path-ui
npm install
npm start
```

The client will be available at `http://localhost:4200/`.

## Product Vision

GeoPath aims to make geometry learning more engaging, structured, and measurable by combining educational design with practical learning analytics. The goal is to help learners reason through geometry problems effectively while giving teachers and content authors the tools to support learning outcomes.

## Status

This repository currently contains the product concept, wireframes, and interactive prototype. The production application architecture is planned around the .NET backend, PostgreSQL database, and Angular front-end listed above.
