# 📋 Task Management System

<div align="center">

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

**A professional desktop application for managing projects and tasks with Docker containerization**

[Features](#features) • [Installation](#installation) • [Docker Setup](#docker-setup) • [Screenshots](#screenshots) • [Tech Stack](#tech-stack)

</div>

---

## 📌 Overview

Task Management System is a fully-featured desktop application built with C# WinForms that helps individuals and teams manage projects, track tasks, monitor progress, and meet deadlines efficiently. The application features a modern UI, complete CRUD operations, and Docker containerization for easy database deployment.

### 🎯 Why This Project?

- **Complete OOP Implementation**: Demonstrates all four pillars of Object-Oriented Programming
- **Professional Desktop Application**: Modern UI with real-time updates and progress tracking
- **Docker Ready**: Containerized database for easy deployment and consistent environments
- **Production-Ready Code**: Repository pattern, exception handling, and logging

---

## ✨ Features

### Core Features

| Feature | Description |
|---------|-------------|
| 🔐 **User Authentication** | Secure login and registration with password hashing |
| 📁 **Project Management** | Create, read, update, and delete projects |
| ✅ **Task Management** | Add, edit, delete tasks within projects |
| 📊 **Progress Tracking** | Visual progress bars showing project completion |
| 🎨 **Priority System** | Color-coded priority levels (High/Medium/Low) |
| ⏰ **Deadline Management** | Due date tracking with overdue detection |
| 🏷️ **Status Management** | Task status: Pending, InProgress, Completed |
| 🐳 **Docker Integration** | Containerized SQL Server database |

### Additional Features

- Modern UI with hover effects and color coding
- Real-time progress calculation
- Task grouping by status
- Priority-based sorting
- Overdue task highlighting
- Exception handling with logging
- Environment-based connection management

---

## 🛠️ Technology Stack

### Frontend
- **Framework**: Windows Forms (.NET Framework 4.8)
- **Language**: C#
- **UI Design**: Custom professional theme with card-based layout

### Backend
- **Language**: C#
- **Pattern**: Repository Pattern
- **Architecture**: Layered (Presentation → Business → Data Access)

### Database
- **Development**: SQL Server LocalDB
- **Production**: SQL Server 2022 (Docker Container)
- **ORM**: ADO.NET with stored procedures

### DevOps
- **Containerization**: Docker & Docker Compose
- **Version Control**: Git & GitHub

---

## 📋 Prerequisites

### Required Software

| Software | Version | Download Link |
|----------|---------|---------------|
| Visual Studio | 2022+ | [Download](https://visualstudio.microsoft.com/) |
| .NET Framework | 4.8+ | Included with VS |
| SQL Server LocalDB | 2019+ | [Download](https://go.microsoft.com/fwlink/?linkid=866658) |
| Docker Desktop | 4.0+ | [Download](https://www.docker.com/products/docker-desktop/) |

### Required Workloads for Visual Studio
- .NET desktop development
- Data storage and processing
- SQL Server Express LocalDB

---

## 🚀 Installation Guide

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/TaskManagementSystem.git
cd TaskManagementSystem
