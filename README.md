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
| <img src="https://github.com/user-attachments/assets/80bc3bc8-df7e-42ce-9e62-3a7e5c16874b" width="340"/> | <img src="https://github.com/user-attachments/assets/f3bf355a-a55a-430b-928e-d97fe9ee6b77" width="224"/>|

| 🧑‍🤝‍🧑 Friens Page | 💬 Chat Page |
|------|--------|
| <img src="https://github.com/user-attachments/assets/46460b5c-9bf1-4228-a81e-9b52eeb02c31" width="336"/> | <img src="https://github.com/user-attachments/assets/41c12070-7d5c-4eea-b942-ceeb60bec3b5" width="349"/> | 

| 📚 Plant Encyclopedia | 🌿 Plant Details |
|------|--------|
| <img src="https://github.com/user-attachments/assets/b28ec474-889b-4095-bbd4-dc8272bbb223" width="384"/> | <img src="https://github.com/user-attachments/assets/2f8c2347-7511-4b03-aa6c-ab70173d9871" width="348"/> |

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

If you find this project helpful, please give it a ⭐ to support the development!

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
