STAR-OIDS

Juego desarrollado en Unity por Miguel Sastre Niño.

Descripción
STAR-OIDS es un microjuego 2D inspirado en el clásico Asteroids, desarrollado como proyecto para la asignatura de Fundamentos de Videojuegos (UPM). El jugador controla una nave (a elegir entre X-Wing y Tie Fighter) que debe sobrevivir y acumular puntos destruyendo asteroides, que se fragmentan en trozos más pequeños al ser impactados. La dificultad aumenta progresivamente con el tiempo.

Características

Movimiento de nave basado en física (impulso + rotación)
Sistema de disparo con cooldown
Generación de asteroides en los bordes de la pantalla, con dificultad creciente
Fragmentación de asteroides (Grande → Mediano → Pequeño)
Sistema de vidas con invulnerabilidad temporal tras recibir daño
Multiplicador de puntuación creciente con el tiempo
Menús de Inicio, Selección de nave, Pausa y Game Over
Object Pooling (balas y asteroides) para optimizar el rendimiento

Tecnologías
Unity (2D, física con Rigidbody2D)
C#
TextMesh Pro (UI)