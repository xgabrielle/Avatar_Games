## **Project Description**

This project is a multiplayer implementation of Checkers built in Unity. The focus is on game logic, networking, and AI-driven interaction rather than visual design. Players can compete over the same Wi-Fi network or play against an AI that not only makes moves but also chats in real time.

## **Features**

### **Multiplayer Mode**

  - Connect and play with other players over the same Wi-Fi network using a custom-built server.

  - Handles real-time game state synchronization between clients, ensuring both players see the same board at all times.
  
  - Shared Solution Architecture:
    - A separate shared library was developed so both the Unity client and the server can use the same data structures, move validation logic, and communication protocols. This ensures consistency and reduces duplication between client and server code.

  - Turn management and move validation are processed on the server side, keeping gameplay fair and consistent.

### **AI Opponent with Chat**

  - Play against an AI that makes strategic game moves and interacts via chat, giving the impression of a single cohesive opponent.

  - Technically, the AI is powered by two systems:

    - Game AI:
      - Custom logic that decides moves based on the board state.

    - Chat AI:
      - OpenAI-powered chat that responds dynamically to both the game and player interaction.

  - The integration is designed so the chat feels like it comes from the same AI making the moves, creating a natural and immersive experience.

### **AI Chat Personalities**

  - Select from multiple personalities for the chat only — affecting tone, style, and type of commentary.

### **Flexible Setup**

  - At startup, choose whether to play against the AI or another human player and the AI personality.

  - Simple connection interface for joining a local multiplayer session or starting a game with AI.

  - Seamless switching between AI and multiplayer modes without restarting the app.

## **Tech Stack**

### **Engine:**
  - Unity (C#)

### **Networking:**
  - Custom socket-based server for LAN multiplayer

### **AI:**
  - Custom logic for game decisions + OpenAI-powered chat with selectable personalities

<img width="2004" height="1139" alt="Screenshot 2025-07-29 155723" src="https://github.com/user-attachments/assets/20cb0f5d-0a79-4d5e-8357-3984e868307e" />

<img width="2710" height="1551" alt="Screenshot 2025-08-21 175618" src="https://github.com/user-attachments/assets/a86b6ac4-d91c-4609-89cb-076e427affad" />

https://github.com/user-attachments/assets/3d6c613d-c0e4-42b6-a47d-5abf48e0cbff

### **Next Steps**
  - Add more board games to choose from.

  - Integrate voice chat with AI instead of text.

  - Improve UI/UX and polish visuals.


