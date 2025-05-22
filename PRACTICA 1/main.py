import pygame
import math
import random
from pygame import mixer

# Inicializar pygame
pygame.init()

# Crear la pantalla
screen = pygame.display.set_mode((800, 600))

# Cargar fondo (escalado a pantalla completa)
background = pygame.transform.scale(pygame.image.load('background.jpg'), (800, 600))

# Música de fondo
mixer.music.load('background.mp3')
mixer.music.play(-1)

# Título e ícono (UFO alargado)
icon = pygame.transform.scale(pygame.image.load('ufo.png'), (100, 40))
pygame.display.set_icon(icon)
pygame.display.set_caption("Space Invaders")

# Jugador (nave centrada, tamaño 64x64)
playerImg = pygame.transform.scale(pygame.image.load('player.png'), (64, 64))
playerX = 370
playerY = 480
playerX_change = 0

# Enemigos (cuadrados, 40x40)
enemyImg = []
enemyX = []
enemyY = []
enemyX_change = []
enemyY_change = []
num_of_enemies = 6

for i in range(num_of_enemies):
    img = pygame.transform.scale(pygame.image.load('enemy.png'), (40, 40))
    enemyImg.append(img)
    enemyX.append(random.randint(0, 735))
    enemyY.append(random.randint(50, 150))
    enemyX_change.append(1)
    enemyY_change.append(40)

# Bala (pequeña, 5x15)
bulletImg = pygame.transform.scale(pygame.image.load('bullet.png'), (30, 45))
bulletX = 0
bulletY = 480
bulletY_change = 10
bullet_state = "ready"

# Puntaje
score_value = 0
font = pygame.font.Font('freesansbold.ttf', 32)
textX = 10
textY = 10

# Texto de Game Over
over_font = pygame.font.Font('freesansbold.ttf', 64)

def show_score(x, y):
    score = font.render("Score: " + str(score_value), True, (0, 255, 0))
    screen.blit(score, (x, y))

def game_over_text():
    over_text = over_font.render("GAME OVER", True, (0, 255, 0))
    screen.blit(over_text, (200, 250))

def player(x, y):
    screen.blit(playerImg, (x, y))

def enemy(x, y, i):
    screen.blit(enemyImg[i], (x, y))

def fire_bullet(x, y):
    global bullet_state
    bullet_state = "fire"
    screen.blit(bulletImg, (x + 30, y))  # +30 para centrar con nave

def isCollision(enemyX, enemyY, bulletX, bulletY):
    distance = math.sqrt(math.pow(enemyX - bulletX, 2) + math.pow(enemyY - bulletY, 2))
    return distance < 27

# Bucle principal
running = True
while running:
    screen.fill((0, 0, 0))
    screen.blit(background, (0, 0))  # Usar fondo precargado y escalado

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

        # Teclas presionadas
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_LEFT:
                playerX_change = -1
            if event.key == pygame.K_RIGHT:
                playerX_change = 1
            if event.key == pygame.K_SPACE:
                if bullet_state == "ready":
                    bullet_Sound = mixer.Sound('laser.mp3')
                    bullet_Sound.play()
                    bulletX = playerX
                    fire_bullet(bulletX, bulletY)

        # Teclas soltadas
        if event.type == pygame.KEYUP:
            if event.key == pygame.K_LEFT or event.key == pygame.K_RIGHT:
                playerX_change = 0

    # Movimiento del jugador
    playerX += playerX_change
    if playerX <= 0:
        playerX = 0
    elif playerX >= 736:
        playerX = 736

    # Movimiento del enemigo
    for i in range(num_of_enemies):
        if enemyY[i] > 440:
            for j in range(num_of_enemies):
                enemyY[j] = 2000
            game_over_text()
            break

        enemyX[i] += enemyX_change[i]
        if enemyX[i] <= 0:
            enemyX_change[i] = 1
            enemyY[i] += enemyY_change[i]
        elif enemyX[i] >= 736:
            enemyX_change[i] = -1
            enemyY[i] += enemyY_change[i]

        collision = isCollision(enemyX[i], enemyY[i], bulletX, bulletY)
        if collision:
            explosion_Sound = mixer.Sound('explosion.mp3')
            explosion_Sound.play()
            bulletY = 480
            bullet_state = "ready"
            score_value += 1
            enemyX[i] = random.randint(0, 736)
            enemyY[i] = random.randint(50, 150)

        enemy(enemyX[i], enemyY[i], i)

    # Movimiento de la bala
    if bulletY <= 0:
        bulletY = 480
        bullet_state = "ready"

    if bullet_state == "fire":
        fire_bullet(bulletX, bulletY)
        bulletY -= bulletY_change

    player(playerX, playerY)
    show_score(textX, textY)
    pygame.display.update()
