# 🎮 WisdomPlay  

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-black?logo=unity)  
![C#](https://img.shields.io/badge/C%23-Game%20Logic-blue?logo=csharp)  
![License](https://img.shields.io/badge/License-MIT-green)  
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20Mobile%20%7C%20Web-lightgrey)  

**WisdomPlay** is a **multilingual 2D storytelling and learning game** built with **Unity**.  
It helps children learn **idioms, proverbs, and moral stories** in a fun and interactive way.  
The game bridges generations: elders guide children to match idioms with their related proverbs, unlocking **short animated stories** that convey values.  

---

## 📖 Table of Contents
- [About the Game](#-about-the-game)  
- [Features](#-features)  
- [Game Modes](#-game-modes)  
- [Gameplay Flow](#-gameplay-flow)  
- [Technology Stack](#-technology-stack)  
- [Architecture](#-architecture)  
- [Project Structure](#-project-structure)  
- [Installation & Setup](#-installation--setup)  
- [How to Play](#-how-to-play)  
- [Data Files (JSON)](#-data-files-json)  
- [Screenshots](#-screenshots)  
- [Demo Video](#-demo-video)  
- [Future Enhancements](#-future-enhancements)  
- [FAQ](#-faq)  
- [Contributing](#-contributing)  
- [Versioning](#-versioning)  
- [License](#-license)  
- [Acknowledgements](#-acknowledgements)  
- [Author](#-author)  

---

## 🧾 About the Game
WisdomPlay is more than a game – it’s a **digital cultural learning tool**.  
- Designed for **children (6–14 years)** to learn moral values.  
- Supports **elders as facilitators**, encouraging family interaction.  
- Content is **language-rich**, **educational**, and **entertaining**.  

---

## ✨ Features
- 🌐 **Multi-language support**: Kannada, English, Hindi, Tamil, Telugu, Sanskrit  
- 📚 **Idioms + Proverbs + Stories** linked together for cultural learning  
- 🎨 **Child-friendly UI/UX**: colorful, simple, responsive design  
- 🎭 **Storytelling mode**: Each correct match unlocks a moral story  
- 🔊 **Interactive audio**: Sound effects and voice feedback  
- 🕹️ **Three levels**: Casual, Ramayana, Mahabharata  
- 📦 **Lightweight JSON data-driven system** for easy updates  

---

## 🎮 Game Modes
1. **Casual Mode** → Everyday idioms and proverbs.  
2. **Ramayana Mode** → Wisdom from *Ramayana* stories.  
3. **Mahabharata Mode** → Proverbs and idioms from *Mahabharata*.  

---

## 🕹️ Gameplay Flow
1. **Select Language**  
2. **Choose Game Mode** (Casual / Ramayana / Mahabharata)  
3. **Elder selects idiom card**  
4. **Child selects matching proverb card**  
   - ✅ Correct → **Story unlocked + animation + audio**  
   - ❌ Wrong → “**Try Again**” feedback  
5. **Progress** → New set of idioms & proverbs  
6. **Level Completion** → Achievement unlocked  

---

## 🛠️ Technology Stack
- **Engine**: Unity 2021.3 LTS  
- **Language**: C#  
- **Data**: JSON (idioms, proverbs, stories)  
- **Animations**: LeanTween  
- **UI**: Unity Canvas, Photoshop assets  
- **Audio**: Unity Audio Engine  

---

## 🏗️ Architecture
<img width="517" height="503" alt="image" src="https://github.com/user-attachments/assets/74ba7344-6322-4076-9ae3-269f2910c6bd" />



---

## 📂 Project Structure
<img width="555" height="497" alt="image" src="https://github.com/user-attachments/assets/eaa44e02-c347-41d5-9e11-742f7d9a9cad" />



---

## ⚙️ Installation & Setup
1. Clone the repository  
   ```bash
   git clone https://github.com/ajay9889-ai/WisdomPlay.git
   cd WisdomPlay
Open in Unity Hub

Version required: Unity 2021.3 LTS or later

Import packages (Unity auto-imports on first run).

Run the game:

Open MainScene.unity

Press ▶️ Play

📝 Data Files (JSON)
Example:

json
Copy code
{
  "idiom": "A blessing in disguise",
  "proverb": "Something that seems bad at first can turn out to be good",
  "story": "Once a farmer lost his horse, but later it returned with two more..."
}
Stored in /Assets/Resources/

Supports multi-language JSON files (en.json, kn.json, hi.json, etc.).



scss
Copy code
![Main Menu](screenshots/mainmenu.png)
![Gameplay](screenshots/gameplay.png)
![Story Display](screenshots/story.png)
🎥 Demo Video
👉 (Add YouTube or Drive link here once gameplay video is ready)

🚀 Future Enhancements
🧑‍🤝‍🧑 Multiplayer elder-child mode

🎤 Voice-based recognition for accessibility

📖 Story library (read stories without gameplay)

🌍 Add more Indian languages (Bengali, Marathi, Gujarati)

🏆 Leaderboards and achievements

❓ FAQ
Q1. What platforms does WisdomPlay support?

Currently tested on PC and Android builds.

Can be exported to iOS and WebGL.

Q2. Do I need an internet connection?

❌ No, all data is stored offline in JSON.

Q3. Can I add my own idioms/proverbs?

✅ Yes, just edit JSON files in /Assets/Resources/.

🤝 Contributing
Fork the repo

Create a feature branch (git checkout -b feature/xyz)

Commit changes (git commit -m "Added feature xyz")

Push to branch (git push origin feature/xyz)

Open a Pull Request

🧾 Versioning
We use Semantic Versioning (SemVer):

v1.0.0 → Initial release

v1.1.0 → New features

v1.1.1 → Bug fixes

📜 License
This project is licensed under the MIT License – see the LICENSE file for details.

🙏 Acknowledgements
Unity for the engine

LeanTween for animations

Cultural wisdom from Ramayana and Mahabharata

Special thanks to educators and parents who inspired this project

👨‍💻 Author
Ajay H A
🎓 MCA Student | 💻 Developer | 🎨 Creative Enthusiast
📧 Contact: [ajayhasrb123@gmail.com]
🌐 GitHub: ajay9889-ai
linkedin: [https://www.linkedin.com/in/ajay-a-38aa84206/]
