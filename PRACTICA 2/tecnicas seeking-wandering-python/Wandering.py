import pygame as pg
from random import randint, uniform
vec = pg.math.Vector2

# Configuración de pantalla y colores
WIDTH = 800
HEIGHT = 600
FPS = 60
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
GREEN = (0, 255, 0)
BLUE = (0, 0, 255)
CYAN = (0, 255, 255)
YELLOW = (255, 255, 0)
DARKGRAY = (40, 40, 40)

# Propiedades del Mob
MOB_SIZE = 32
MAX_SPEED = 4
MAX_FORCE = 0.4
RAND_TARGET_TIME = 500
WANDER_RING_DISTANCE = 150
WANDER_RING_RADIUS = 50
WANDER_TYPE = 2

class Mob(pg.sprite.Sprite):
    def __init__(self):
        self.groups = all_sprites
        pg.sprite.Sprite.__init__(self, self.groups)
        self.image = pg.Surface((MOB_SIZE, MOB_SIZE))
        self.image.fill(YELLOW)
        self.rect = self.image.get_rect()
        self.pos = vec(randint(0, WIDTH), randint(0, HEIGHT))
        self.vel = vec(MAX_SPEED, 0).rotate(uniform(0, 360))
        self.acc = vec(0, 0)
        self.rect.center = self.pos
        self.last_update = 0
        self.target = vec(randint(0, WIDTH), randint(0, HEIGHT))

    def seek(self, target):
        self.desired = (target - self.pos).normalize() * MAX_SPEED
        steer = (self.desired - self.vel)
        if steer.length() > MAX_FORCE:
            steer.scale_to_length(MAX_FORCE)
        return steer

    def wander_improved(self):
        future = self.pos + self.vel.normalize() * WANDER_RING_DISTANCE
        target = future + vec(WANDER_RING_RADIUS, 0).rotate(uniform(0, 360))
        self.displacement = target
        return self.seek(target)

    def wander(self):
        now = pg.time.get_ticks()
        if now - self.last_update > RAND_TARGET_TIME:
            self.last_update = now
            self.target = vec(randint(0, WIDTH), randint(0, HEIGHT))
        return self.seek(self.target)

    def update(self):
        if WANDER_TYPE == 1:
            self.acc = self.wander()
        else:
            self.acc = self.wander_improved()
        self.vel += self.acc
        if self.vel.length() > MAX_SPEED:
            self.vel.scale_to_length(MAX_SPEED)
        self.pos += self.vel

        if self.pos.x > WIDTH:
            self.pos.x = 0
        if self.pos.x < 0:
            self.pos.x = WIDTH
        if self.pos.y > HEIGHT:
            self.pos.y = 0
        if self.pos.y < 0:
            self.pos.y = HEIGHT

        self.rect.center = self.pos

    def draw_vectors(self):
        scale = 25
        pg.draw.line(screen, GREEN, self.pos, (self.pos + self.vel * scale), 5)
        pg.draw.line(screen, RED, self.pos, (self.pos + self.desired * scale), 5)
        if WANDER_TYPE == 1:
            pg.draw.circle(screen, CYAN, (int(self.target.x), int(self.target.y)), 8)
        else:
            center = self.pos + self.vel.normalize() * WANDER_RING_DISTANCE
            pg.draw.circle(screen, WHITE, (int(center.x), int(center.y)), WANDER_RING_RADIUS, 1)
            pg.draw.line(screen, CYAN, center, self.displacement, 5)

# Inicialización
pg.init()
screen = pg.display.set_mode((WIDTH, HEIGHT))
clock = pg.time.Clock()

all_sprites = pg.sprite.Group()
Mob()

paused = False
show_vectors = False
running = True

while running:
    clock.tick(FPS)
    for event in pg.event.get():
        if event.type == pg.QUIT:
            running = False
        if event.type == pg.KEYDOWN:
            if event.key == pg.K_ESCAPE:
                running = False
            if event.key == pg.K_SPACE:
                paused = not paused
            if event.key == pg.K_v:
                show_vectors = not show_vectors
            if event.key == pg.K_m:
                Mob()

    if not paused:
        all_sprites.update()

    pg.display.set_caption("{:.2f}".format(clock.get_fps()))
    screen.fill(DARKGRAY)
    all_sprites.draw(screen)

    if show_vectors:
        for sprite in all_sprites:
            sprite.draw_vectors()

    pg.display.flip()
