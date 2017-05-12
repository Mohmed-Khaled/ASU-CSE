import numpy as np
from sklearn import svm
from sklearn.metrics import accuracy_score
import matplotlib.pyplot as plt
import matplotlib.patches as patches


training_data = []
training_labels = []
test_data = []
test_labels = []
pos_mean = [1 for pos_m in range(50)]
neg_mean = [-1 for neg_m in range(50)]
pos_mean_vector = np.array([pos_mean]).T
neg_mean_vector = np.array([neg_mean]).T
covariance = 50 * np.identity(50)


# Helper functions
def write_to_file(label, data):
    generated_file = open('{0}.dat'.format(label), 'w')
    for point in data:
        generated_file.write(label)
        generated_file.write(' ')
        feature_no = 1
        for feature in point:
            generated_file.write(str(feature_no))
            generated_file.write(':')
            generated_file.write(str(feature))
            generated_file.write(' ')
            feature_no += 1
        generated_file.write('\n')
    generated_file.close()


def read_from_files(pos_data_filename, neg_data_filename):
    pos_data = []
    neg_data = []
    pos_data_file = open('{0}.dat'.format(pos_data_filename), 'r')
    neg_data_file = open('{0}.dat'.format(neg_data_filename), 'r')
    for line in pos_data_file:
        point = []
        line = line.strip()
        line = line.split(' ')[1:]
        for feature in line:
            feature_value = float(feature.split(':')[1])
            point.append(feature_value)
        pos_data.append(point)
    for line in neg_data_file:
        point = []
        line = line.strip()
        line = line.split(' ')[1:]
        for feature in line:
            feature_value = float(feature.split(':')[1])
            point.append(feature_value)
        neg_data.append(point)
    pos_data_file.close()
    neg_data_file.close()
    return pos_data, neg_data


def mahalanobis_distance(x_vector, mean_vector, covariance_matrix):
    covariance_inverse = np.linalg.inv(covariance_matrix)
    col_v = x_vector - mean_vector
    row_v = np.transpose(col_v)
    return np.matmul(np.matmul(row_v, covariance_inverse), col_v)


# Q.1 generate data and split them
def generate_data():
    pos_data = np.random.multivariate_normal(pos_mean, covariance, 5000)
    neg_data = np.random.multivariate_normal(neg_mean, covariance, 5000)
    # write_to_file('1', pos_data)
    # write_to_file('-1', neg_data)
    return pos_data, neg_data

# pos, neg = generate_data()
pos, neg = read_from_files(pos_data_filename='positive', neg_data_filename='negative')
for i in range(2500):
    training_data.append(pos[i])
    training_labels.append(1)
    training_data.append(neg[i])
    training_labels.append(0)
    test_data.append(pos[i + 2500])
    test_labels.append(1)
    test_data.append(neg[i + 2500])
    test_labels.append(0)

# Q.2 Get parameters that give minimum test error
# training_sizes = [1000 * (i+1) for i in range(5)]
# # chosen_gamma = [0, 0, 0, 0, 0]
# chosen_gamma = [1e-06, 1e-05, 0.0001, 1e-07, 1e-06]
# # chosen_c = [0, 0, 0, 0, 0]
# chosen_c = [10.0, 1.0, 1.0, 10.0, 1.0]
# min_error = [1, 1, 1, 1, 1]
# gamma_values = [1e-7, 1e-6, 1e-5, 1e-4, 1e-3]
# c_values = [1e0, 1e1, 1e2, 1e3, 1e4]
# cnt = 1
# for size in training_sizes:
#     for g_v in gamma_values:
#         for c_v in c_values:
#             clf = svm.SVC(kernel='rbf', gamma=g_v, C=c_v)
#             clf = svm.SVC(kernel='rbf', gamma=chosen_gamma[(size / 1000) - 1], C=chosen_c[(size / 1000) - 1])
#             clf = svm.SVC(kernel='rbf', gamma=1e-6, C=1)
#             clf.fit(training_data[:size], training_labels[:size])
#             predictions = clf.predict(test_data)
#             error = 1 - accuracy_score(test_labels, predictions)
#             if error < min_error[(size / 1000) - 1]:
#                 chosen_gamma[(size / 1000) - 1] = g_v
#                 chosen_c[(size / 1000) - 1] = c_v
#                 min_error[(size / 1000) - 1] = error
#             print '------------------------------------'
#             print cnt
#             print chosen_gamma
#             print chosen_c
#             print min_error
#             print '------------------------------------'
#             cnt += 1

training_sizes = [1000 * (i+1) for i in range(5)]
errors = [[] for i in range(6)]
rbf_params = [(1e-5, 1), (1e-6, 1), (1e-7, 1), (1e-5, 10), (1e-6, 10), (1e-7, 10)]
for size in training_sizes:
    counter = 0
    for param in rbf_params:
        clf = svm.SVC(kernel='rbf', gamma=param[0], C=param[1])
        clf.fit(training_data[:size], training_labels[:size])
        predictions = clf.predict(test_data)
        error = 1 - accuracy_score(test_labels, predictions)
        errors[counter].append(error)
        counter += 1

plt.plot(training_sizes, errors[0], 'r', training_sizes, errors[1], 'g', training_sizes, errors[2], 'c',
         training_sizes, errors[3], 'm', training_sizes, errors[4], 'y', training_sizes, errors[5], 'b')
plt.xlabel('Training Size')
plt.ylabel('Test Error')
red_patch = patches.Patch(color='red', label='Using RBF gamma=1e-5 and 1')
green_patch = patches.Patch(color='green', label='Using RBF gamma=1e-6 and 1')
cyan_patch = patches.Patch(color='cyan', label='Using RBF gamma=1e-7 and 1')
magenta_patch = patches.Patch(color='magenta', label='Using RBF gamma=1e-5 and 10')
yellow_patch = patches.Patch(color='yellow', label='Using RBF gamma=1e-6 and 10')
blue_patch = patches.Patch(color='blue', label='Using RBF gamma=1e-7 and 10')
plt.legend(handles=[red_patch, green_patch, cyan_patch, magenta_patch, yellow_patch, blue_patch])
plt.show()

training_sizes = [1000 * (i+1) for i in range(2)]
errors = [[] for i in range(5)]
poly_degrees = [1, 2, 3, 4, 5]
for size in training_sizes:
    for d in poly_degrees:
        clf = svm.SVC(kernel='poly', degree=d)
        clf.fit(training_data[:size], training_labels[:size])
        predictions = clf.predict(test_data)
        error = 1 - accuracy_score(test_labels, predictions)
        errors[d-1].append(error)

plt.plot(training_sizes, errors[0], 'r', training_sizes, errors[1], 'g',
         training_sizes, errors[2], 'c', training_sizes, errors[3], 'm', training_sizes, errors[4], 'y')
plt.xlabel('Training Size')
plt.ylabel('Test Error')
red_patch = patches.Patch(color='red', label='Using Polynomial 1st degree')
green_patch = patches.Patch(color='green', label='Using Polynomial 2nd degree')
cyan_patch = patches.Patch(color='cyan', label='Using Polynomial 3rd degree')
magenta_patch = patches.Patch(color='magenta', label='Using Polynomial 4th degree')
yellow_patch = patches.Patch(color='yellow', label='Using Polynomial 5th degree')
plt.legend(handles=[red_patch, green_patch, cyan_patch, magenta_patch, yellow_patch])
plt.show()

# Q.3 Using 1st degree polynomial kernel
training_sizes = [1000 * (i+1) for i in range(5)]
test_errors = []
training_errors = []
test_errors2 = []
training_errors2 = []
for size in training_sizes:
    clf = svm.SVC(kernel='poly', degree=1)
    clf.fit(training_data[:size], training_labels[:size])
    predictions = clf.predict(test_data)
    error = 1 - accuracy_score(test_labels, predictions)
    test_errors.append(error)
    predictions = clf.predict(training_data)
    error = 1 - accuracy_score(training_labels, predictions)
    training_errors.append(error)
    clf = svm.SVC(kernel='rbf', gamma=1e-5, C=1)
    clf.fit(training_data[:size], training_labels[:size])
    predictions = clf.predict(test_data)
    error = 1 - accuracy_score(test_labels, predictions)
    test_errors2.append(error)
    predictions = clf.predict(training_data)
    error = 1 - accuracy_score(training_labels, predictions)
    training_errors2.append(error)

plt.plot(training_sizes, test_errors, 'b', training_sizes, training_errors, 'r',
         training_sizes, test_errors2, 'm', training_sizes, training_errors2, 'g')
blue_patch = patches.Patch(color='blue', label='Test Error (Poly)')
red_patch = patches.Patch(color='red', label='Training Error (Poly)')
magenta_patch = patches.Patch(color='magenta', label='Test Error (RBF)')
green_patch = patches.Patch(color='green', label='Training Error (RBF)')
plt.legend(handles=[blue_patch, red_patch, magenta_patch, green_patch])
plt.show()

# Q.4 Using minimum distance classifier
predictions = []
for t in test_data:
    x = np.array([t]).T
    distance_from_pos = mahalanobis_distance(x, pos_mean_vector, covariance)
    distance_from_neg = mahalanobis_distance(x, neg_mean_vector, covariance)
    if distance_from_pos <= distance_from_neg:
        predictions.append(1)
    else:
        predictions.append(0)

print "Bayes Error      = ", 1 - accuracy_score(test_labels, predictions)
print "SVM Error (Poly) = ", min(test_errors)
print "SVM Error (RBF)  = ", min(test_errors2)
