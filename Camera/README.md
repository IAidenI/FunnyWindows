# Camera

**Camera** est une version améliorée du programme [TryCatchMe](https://github.com/IAidenI/ATtiny85/tree/main/TryCatchMe), à laquelle s’ajoute un **système de surveillance par caméra**. Ce programme combine une fenêtre intrusive difficile à fermer avec une diffusion en temps réel de la caméra de l'utilisateur.

## Fonctionnalités

- 🪟 **Comportement de TryCatchMe** :
  - Fenêtre toujours au premier plan, sans barre de titre ni bouton de fermeture.
  - Tremblements et déplacements pour éviter la souris.
  - Blocage partiel des raccourcis système (comme le gestionnaire des tâches).
- 🎥 **Surveillance par caméra** :
  - Dès que le programme est lancé, la webcam de l'utilisateur est activée.
  - Le flux vidéo est envoyé en temps réel au serveur de contrôle.
  - Une nouvelle connexion apparaît automatiquement sur le **tableau de bord web**.
- 🌐 **Interface Web de contrôle** :
  - Il suffit de lancer le fichier `server.py`.
  - L’URL de connexion est affichée dans la console (accessible via navigateur).
  - Affichage en direct du flux caméra de tous les clients connectés.

## Démonstration

<p align="center">
  <img src="./src/images/Demo_Camera.gif"/>
</p>
