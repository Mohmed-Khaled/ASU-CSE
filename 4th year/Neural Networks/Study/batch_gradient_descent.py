import numpy as np
import neuron_calculations

# network parameters
number_of_inputs = 3
number_of_hidden_neurons = 1
number_of_patterns = 2
learning_rate = 0.5
# inputs and targets
p = np.array([[-1, 1, -1], [1, 1, -1]])
t = [1, 0]
print 'Patterns = ', p
print 'Target = ', t

# initial values
error = 1
w = np.random.rand(number_of_hidden_neurons, number_of_inputs)
b = np.random.rand(number_of_hidden_neurons, 1)
# learning
cnt = 0
print "\n=========================LEARNING========================="
while error != 0:
    error = 0
    w_tmp = np.zeros((number_of_hidden_neurons, number_of_inputs))
    b_tmp = np.zeros((number_of_hidden_neurons, 1))
    print '\nIter No. = ', cnt
    print 'Weights = ', w
    print 'Bias = ', b, "\n"
    for i, row in enumerate(p):
        net = np.dot(w, row.T) + b
        output = np.copy(net)
        neuron_calculations.activation_function(output, 1)
        if t[i] != output:
            w_tmp = w_tmp + learning_rate * (t[i] - output) * row
            b_tmp = b_tmp - learning_rate * (t[i] - output)
            error += abs((t[i] - output))
        print 'Input = ', row
        print 'Net = ', net
        print 'Target = ', t[i]
        print 'Output = ', output
        print 'Error = ', (t[i] - output)
    w = w + w_tmp
    b = b + b_tmp
    print "Total Error = ", error
    cnt += 1
