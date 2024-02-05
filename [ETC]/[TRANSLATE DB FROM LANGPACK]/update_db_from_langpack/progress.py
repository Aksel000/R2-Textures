import sys

def bar(iteration, total, bar_length=50):
    progress = (iteration / total)
    arrow = '=' * int(round(bar_length * progress))
    spaces = ' ' * (bar_length - len(arrow))

    sys.stdout.write(f'\r[{arrow}{spaces}] {int(progress * 100)}%')
    sys.stdout.flush()

def log(msg):
    sys.stdout.write('\n'+msg)
    sys.stdout.flush()