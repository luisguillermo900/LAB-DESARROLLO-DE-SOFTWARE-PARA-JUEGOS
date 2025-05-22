import pygame
import random
import math
from pygame import mixer

# Inicializar pygame
pygame.init()

# Pantalla
screen = pygame.display.set_mode((800, 600))
pygame.display.set_caption("Space Guardians: Defensa en Equipo")
icon = pygame.transform.scale(pygame.image.load('ufo.png'), (80, 40))
pygame.display.set_icon(icon)

# Fondo
background = pygame.transform.scale(pygame.image.load('background.jpg'), (800, 600))

# Música
mixer.music.load('background.mp3')
mixer.music.play(-1)

# Jugadores
playerImg1 = pygame.transform.scale(pygame.image.load('player.png'), (64, 64))
playerImg2 = pygame.transform.scale(pygame.image.load('player.png'), (64, 64))
player1X, player1Y = 200, 500
player2X, player2Y = 550, 500
player1X_change = 0
player2X_change = 0

# Balas
bulletImg1 = pygame.transform.scale(pygame.image.load('bullet.png'), (10, 30))
bulletImg2 = pygame.transform.scale(pygame.image.load('bullet.png'), (10, 30))
bullet1X, bullet1Y = 0, 500
bullet2X, bullet2Y = 0, 500
bullet1_state = "ready"
bullet2_state = "ready"
bulletY_change = 10

# Enemigos
enemyImg = []
enemyX = []
enemyY = []
enemyY_change = []
num_of_enemies = 6

for i in range(num_of_enemies):
    enemyImg.append(pygame.transform.scale(pygame.image.load('enemy.png'), (40, 40)))
    enemyX.append(random.randint(0, 760))
    enemyY.append(random.randint(0, 150))
    enemyY_change.append(0.1)

# Puntuaciones y vidas
score_p1 = 0
score_p2 = 0
vidas = 3
font = pygame.font.Font('freesansbold.ttf', 24)
game_over_font = pygame.font.Font('freesansbold.ttf', 64)

def show_score():
    p1 = font.render("P1 Score: " + str(score_p1), True, (0, 255, 0))
    p2 = font.render("P2 Score: " + str(score_p2), True, (0, 255, 255))
    life = font.render("Vidas: " + str(vidas), True, (255, 0, 0))
    screen.blit(p1, (10, 10))
    screen.blit(p2, (10, 40))
    screen.blit(life, (10, 70))

def game_over():
    over = game_over_font.render("GAME OVER", True, (255, 255, 255))
    screen.blit(over, (250, 250))

def player(x, y, img):
    screen.blit(img, (x, y))

def fire_bullet(x, y, bulletImg, jugador):
    global bullet1_state, bullet2_state
    if jugador == 1:
        bullet1_state = "fire"
        screen.blit(bulletImg, (x + 27, y))
    else:
        bullet2_state = "fire"
        screen.blit(bulletImg, (x + 27, y))

def enemy(x, y, i):
    screen.blit(enemyImg[i], (x, y))

def isCollision(ex, ey, bx, by):
    distance = math.sqrt((ex - bx) ** 2 + (ey - by) ** 2)
    return distance < 27

# Bucle principal
running = True
while running:
    screen.fill((0, 0, 0))
    screen.blit(background, (0, 0))

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_LEFT:
                player1X_change = -3
            if event.key == pygame.K_RIGHT:
                player1X_change = 3
            if event.key == pygame.K_UP:
                if bullet1_state == "ready":
                    mixer.Sound('laser.mp3').play()
                    bullet1X = player1X
                    fire_bullet(bullet1X, bullet1Y, bulletImg1, 1)

            if event.key == pygame.K_a:
                player2X_change = -3
            if event.key == pygame.K_d:
                player2X_change = 3
            if event.key == pygame.K_w:
                if bullet2_state == "ready":
                    mixer.Sound('laser.mp3').play()
                    bullet2X = player2X
                    fire_bullet(bullet2X, bullet2Y, bulletImg2, 2)

        if event.type == pygame.KEYUP:
            if event.key in [pygame.K_LEFT, pygame.K_RIGHT]:
                player1X_change = 0
            if event.key in [pygame.K_a, pygame.K_d]:
                player2X_change = 0

    # Movimiento jugadores
    player1X += player1X_change
    player2X += player2X_change
    player1X = max(0, min(player1X, 736))
    player2X = max(0, min(player2X, 736))

    # Movimiento enemigos
    for i in range(num_of_enemies):
        if enemyY[i] > 550:
            vidas -= 1
            enemyY[i] = random.randint(0, 150)
            enemyX[i] = random.randint(0, 760)

        enemyY[i] += enemyY_change[i]

        if isCollision(enemyX[i], enemyY[i], bullet1X, bullet1Y):
            mixer.Sound('explosion.mp3').play()
            bullet1Y = 500
            bullet1_state = "ready"
            score_p1 += 1
            enemyX[i] = random.randint(0, 760)
            enemyY[i] = random.randint(0, 150)

        if isCollision(enemyX[i], enemyY[i], bullet2X, bullet2Y):
            mixer.Sound('explosion.mp3').play()
            bullet2Y = 500
            bullet2_state = "ready"
            score_p2 += 1
            enemyX[i] = random.randint(0, 760)
            enemyY[i] = random.randint(0, 150)

        enemy(enemyX[i], enemyY[i], i)

    # Movimiento de balas
    if bullet1Y <= 0:
        bullet1Y = 500
        bullet1_state = "ready"
    if bullet1_state == "fire":
        fire_bullet(bullet1X, bullet1Y, bulletImg1, 1)
        bullet1Y -= bulletY_change

    if bullet2Y <= 0:
        bullet2Y = 500
        bullet2_state = "ready"
    if bullet2_state == "fire":
        fire_bullet(bullet2X, bullet2Y, bulletImg2, 2)
        bullet2Y -= bulletY_change

    player(player1X, player1Y, playerImg1)
    player(player2X, player2Y, playerImg2)
    show_score()

    if vidas <= 0:
        game_over()

    pygame.display.update()
