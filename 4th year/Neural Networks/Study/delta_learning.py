import numpy as np
import neuron_calculations

# network parameters
number_of_inputs = 3
number_of_hidden_neurons = 1
number_of_patterns = 2
learning_rate = 0.1
# inputs and targets
p = np.array([[-1, 1, -1], [1, 1, -1]])
t = [1, 0]
print 'Patterns = ', p
print 'Target = ', t

# initial values
error = np.ones((number_of_patterns, 1))
w = np.random.rand(number_of_hidden_neurons, number_of_inputs)
b = np.random.rand(number_of_hidden_neurons, 1)
# learning
cnt = 0
while (error - np.zeros((number_of_patterns, 1))).any():
    net = np.dot(w, p.T) + b
    output = np.copy(net)
    neuron_calculations.activation_function(output, 1)
    error = t - output
    w = w + learning_rate * np.dot(error, p)
    b = b + learning_rate * error
    print 'Iter No. = ', cnt
    print 'Weights = ', w
    print 'Bias = ', b
    print 'Net = ', net
    print 'Output = ', output
    print 'Error = ', error
    cnt += 1
