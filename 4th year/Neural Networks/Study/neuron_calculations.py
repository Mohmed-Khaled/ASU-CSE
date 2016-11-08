import numpy as np
import math


def threshold(a):
    if a > 0:
        return 1
    return 0


def linear(a):
    alpha = 1
    return alpha * a


def sigmoid(a):
    return float(1) / (1 + math.exp(-a))


def activation_function(a, function):
    for x in np.nditer(a, op_flags=['readwrite']):
        if function == 1:
            x[...] = threshold(x[...])
        elif function == 2:
            x[...] = linear(x[...])
        elif function == 3:
            x[...] = sigmoid(x[...])
