# NarutoFighters ⚔️

Este proyecto consiste en un simulador de combate épico por turnos implementado en **C# (Windows Forms / .NET Framework 4.8)**. El desarrollo toma como base el reto de programación original "Deadpool vs Wolverine", adaptando las mecánicas a los personajes de Naruto y Sasuke.

---

## 📋 Descripción del Ejercicio (Asignación)

El objetivo es simular una batalla épica por turnos entre dos personajes bajo las siguientes reglas y condiciones:

1. **Puntos de Vida (HP):** Ambos personajes inician con **1000 pts** de vida.
2. **Daño de Ataque:** Cada personaje inflige un daño aleatorio por turno:
   - **Naruto:** Entre 10 y 100 pts.
   - **Sasuke:** Entre 10 y 100 pts.
   - Además, existe una probabilidad del **15% de golpe crítico**, duplicando el daño infligido.
3. **Evasión (Esquivo):** Los personajes tienen la capacidad de evitar por completo el ataque contrario:
   - **Sasuke (Sharingan):** **25%** de probabilidades.
   - **Naruto:** **20%** de probabilidades.
4. **Regeneración de Turno:** Los personajes recuperan un **5%** del daño que infligen en cada turno.
5. **Mareo por Daño Máximo:** Si el daño recibido es el máximo (o un crítico equivalente), el personaje defensor queda **mareado**; se salta su siguiente turno de ataque para regenerarse y recupera el **10%** del daño recibido.
6. **Fallo de Ataque:** Hay un **10%** de probabilidad de que el atacante falle por completo su ataque.
7. **Condición de Victoria:** Un personaje pierde cuando su vida llega a **0 o menos**. El otro es declarado ganador.

---

## 🛠️ Lo que se implementó

Para cumplir con la asignación respetando la estructura actual de los Windows Forms y sin modificar los controles visuales existentes, se realizaron las siguientes acciones:

### 1. Control de Interfaces y Flujo de Navegación
* **Pantalla de Inicio (`Inicio.cs`):** Implementación del botón de juego para pasar de manera fluida a la pantalla de selección, ocultando el menú principal.
* **Selección de Personaje (`Seleccion.cs`):** Permite al usuario elegir entre Naruto y Sasuke. El oponente se selecciona de manera aleatoria excluyendo al personaje elegido por el jugador.
* **Pantalla de Resultado (`Resultado.cs`):** Muestra dinámicamente quién ganó la batalla y quién fue derrotado, y gestiona las opciones de revancha (reiniciar al selector) y salida del programa.

### 2. Lógica del Motor de Combate (`Combate.cs`)
* **Bucle Asíncrono de Batalla (`RunBattle`):** Controla los turnos alternados entre jugador y enemigo, añadiendo esperas asíncronas de `1` segundo entre turnos para que el usuario pueda visualizar el combate.
* **Carga Dinámica de Sprites:** Implementación de `LoadSprite` para cargar las imágenes de los personajes en tiempo de ejecución de acuerdo a su estado en el combate:
  - **Naruto:** Reposo (`Naruto.png`), Ataque (`Golpe.png`) y Daño (`Daño.png`).
  - **Sasuke:** Reposo (`Sasuke.png`), Ataque (`sGolpe.png`) y Daño (`sDaño.png`).
* **Bitácora de Combate (`rtbLog`):** Registro a todo color en un control `RichTextBox` que indica detalladamente cada evento (turnos, fallos, esquivos, ataques normales, golpes críticos, regeneraciones y estados de mareo).
* **Compilación Correcta:** Configuración de los metadatos en `NarutoFighters.csproj` para asegurar que los recursos e imágenes se copien automáticamente al directorio de ejecución (`bin/Debug`), logrando una compilación limpia con **0 errores y 0 advertencias**.