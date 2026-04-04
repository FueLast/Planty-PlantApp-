# 🌿 Plant Social App — MAUI Cross-Platform AI-Powered Social Network

![.NET](https://img.shields.io/badge/.NET-MAUI-blue)
![Architecture](https://img.shields.io/badge/Architecture-MVVM-green)
![Status](https://img.shields.io/badge/status-in%20progress-yellow)

---

## 🚀 Overview

A cross-platform mobile application designed as a **social network for plant lovers** — helping users **learn, share, and care for plants**.

The app is focused on:

* 🌱 beginners in home gardening
* 🤝 social interaction between users
* 🤖 AI-powered assistance for plant care

The long-term vision is to expand from **home plant care** into broader areas like **gardening and agriculture**.

---

## 🎯 Why this project?

This application started as my **graduate (diploma) project**.

I got into home gardening myself and quickly realized:

* Most educational content (like YouTube videos) felt **boring and inefficient**
* Beginners often struggle with **basic plant care**
* There’s no simple, engaging way to learn and track plant care

So I decided to build my own solution:

* Combine **learning + social interaction**
* Use **AI to simplify complex information**
* Remove unnecessary “noise” and make learning **fast and practical**

I believe many people face the same problem:

> You get a plant — but have no idea how to take care of it.

With modern AI tools, this barrier can be removed completely.

---

## ✨ Key Features

* 🔐 User authentication (Supabase)
* 👤 User profiles (avatar, bio, city, age, online status)
* 🌿 Plant management (add, view, track)
* 👥 Social system (friends, profiles)
* 💬 Real-time chat
* 📅 Plant care calendar
* ⭐ Favorites system
* 🤖 AI assistant
  * Plant care tips
  * Q&A about plants
  * Context-aware recommendations (planned)

---

## 📸 Screenshots

| 🏠 Main Page | 👤 Profile |
|------|--------|
| <img src="https://github.com/user-attachments/assets/7295d78a-68d3-4d2f-9a48-3a5a5682929f" width="350"/> | <img src="https://github.com/user-attachments/assets/550bc017-7f8e-4837-88b0-a64565521fd6" width="350"/>|

| 📚 Plant Encyclopedia | 🌿 Plant Details |
|------|--------|
| <img src="https://github.com/user-attachments/assets/b4f08777-fae6-4271-a74a-c60f0fc49f93" width="350"/> | <img src="https://github.com/user-attachments/assets/4fd3d9bf-1a03-42a6-b618-5f7e74015a5d" width="350"/> |

---

## 🏗 Architecture

The project follows a **clean MVVM architecture**:

* View ↔ ViewModel via bindings
* Minimal code-behind
* Business logic separated into services:

  * `PlantService`
  * `FriendService`
* Database via **EF Core + IDbContextFactory**
* Navigation through `INavigationService`
* Full use of **Dependency Injection**
* API layer via HttpClient
* Frontend communicates only with services (no direct DB access)
  
---

## 🛠 Tech Stack

* **C# / .NET MAUI**
* **MVVM (CommunityToolkit.Mvvm)**
* **SQLite + Entity Framework Core**
* **Supabase**
  * Authentication
  * Cloud database (users, chat, social data)
* **SQLite**
  * Local storage (plants, offline data)
* **HttpClient (API integration)**
* **Syncfusion** (calendar)
* **CommunityToolkit.Maui (Popups)**

---

## 🎨 UI/UX Approach

* Clean and scalable UI
* `CollectionView` for efficient lists
* Dynamic content loading (progressive expansion)
* Single visible collection (no duplication)
* No nested scrolling (better performance & UX)
* Custom converters:

  * `ExpandTextHelper`
  * `AvatarHelper`
* Popup-based interactions for editing/adding data

---

## ⚙️ Getting Started

```bash
git clone https://github.com/FueLast/Planty-PlantApp-.git
```

1. Open in **Visual Studio**
2. Configure:

   * Supabase credentials
   * Yandex GPT API key
3. Set up environment variables:

   * SUPABASE_URL
   * SUPABASE_KEY
   * YANDEX_GPT_API_KEY

---

## 📌 Project Status

🚧 Actively in development
Architecture and features are continuously improving

---

## 🤝 Contact

GitHub: [*FueLast*](https://github.com/FueLast)
Mail: *axeduxi@gmail.com*
Telegram: [*Treinta0cho*](https://t.me/treinta0cho)

---

## ⭐ Final Note

This project is not just about plant care —
it’s about making **learning simple, social, and accessible** using modern technologies.

---

## 🗺 Roadmap

- [x] Authentication
- [x] Plant system
- [x] Chat
- [x] AI integration
- [x] Supabase integration
- [x] Friends function
- [ ] Notifications
- [ ] AI recommendations based on user plants
- [ ] Image recognition (plant diagnosis)
