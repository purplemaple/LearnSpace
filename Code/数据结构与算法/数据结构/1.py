import time

start_time = time.time()

# for a in range(0, 1000):
#     for b in range(0, 1000):
#         for c in range(0, 1000):
#             if a**2 + b**2 == c**2 and a + b + c == 1000:
#                 print('a = {}, b = {}, c = {}'.format(a, b, c))

for a in range(0, 1000):
    for b in range(0, 1000):
        c = 1000 - a - b
        if a**2 + b**2 == c**2:
            print('a = {}, b = {}, c = {}'.format(a, b, c))



print('Time taken: {} seconds'.format(time.time() - start_time))
print('Done!')

