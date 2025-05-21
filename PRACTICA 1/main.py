import pygame
import math
import random
from pygame import mixer

# Initialize the pygame
pygame.init()

# Create the screen
screen = pygame.display.set_mode((800, 600))

# Background
background = pygame.image.load('background.png')

# Game loop
running = True
while running:
    # Rellenar el fondo
    screen.blit(background, (0, 0))

    # Actualizar la pantalla
    pygame.display.update()

    # Manejar eventos
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False