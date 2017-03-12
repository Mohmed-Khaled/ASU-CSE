import numpy as np
import pylab as pl


def vector_from_array(arr):
    return np.transpose(np.array([arr]))


def classify(g_1, g_2, g_3):
    maximum_g = max(g_1, g_2, g_3)
    if maximum_g == g_1:
        return 1
    elif maximum_g == g_2:
        return 2
    elif maximum_g == g_3:
        return 3


# Question 1
def discriminate_function(x_vector, mean_vector, covariance_matrix, prior, dimension):
    x_vector = vector_from_array(x_vector)
    mean_vector = vector_from_array(mean_vector)
    covariance_det = np.linalg.det(covariance_matrix)
    v = mahalanobis_distance(x_vector, mean_vector, covariance_matrix)
    g = -0.5 * v - dimension * 0.5 * np.log(np.pi * 0.5) - 0.5 * np.log(covariance_det) + np.log(prior)
    return np.linalg.det(g)


# Question 2
def mahalanobis_distance(x_vector, mean_vector, covariance_matrix):
    covariance_inverse = np.linalg.inv(covariance_matrix)
    col_v = x_vector - mean_vector
    row_v = np.transpose(col_v)
    return np.matmul(np.matmul(row_v, covariance_inverse), col_v)


def MAP_classifier(data, mean, covariance, prior):
    inv_covariance = np.linalg.inv(covariance)
    det_covariance = np.linalg.det(covariance1)
    v = data - mean
    return -0.5 * np.sum(np.dot(v, inv_covariance) * v, axis=1) - 0.5 * np.log(det_covariance) + np.log(prior)


def ML_classifier(data, mean, covariance):
    inv_covariance = np.linalg.inv(covariance)
    det_covariance = np.linalg.det(covariance1)
    v = data - mean
    return -0.5 * np.sum(np.dot(v, inv_covariance) * v, axis=1) - 0.5 * np.log(det_covariance)


# Data
w1 = np.array([[-5.01, -5.43, 1.08, 0.86, -2.67, 4.94, -2.51, -2.25, 5.56, 1.03],
               [-8.12, -3.48, -5.52, -3.78, 0.63, 3.29, 2.09, -2.13, 2.86, -3.33],
               [-3.68, -3.54, 1.66, -4.11, 7.39, 2.08, -2.59, -6.94, -2.26, 4.33]])
w2 = np.array([[-0.91, 1.30, -7.75, -5.47, 6.14, 3.60, 5.37, 7.18, -7.39, -7.50],
               [-0.18, -2.06, -4.54, 0.50, 5.72, 1.26, -4.63, 1.46, 1.17, -6.32],
               [-0.05, -3.53, -0.95, 3.92, -4.85, 4.36, -3.65, -6.66, 6.30, -0.31]])
w3 = np.array([[5.35, 5.12, -1.34, 4.48, 7.11, 7.17, 5.75, 0.77, 0.90, 3.52],
               [2.26, 3.22, -5.31, 3.42, 2.39, 4.33, 3.97, 0.27, -0.43, -0.36],
               [8.13, -2.66, -9.87, 5.19, 9.21, -0.98, 6.65, 2.41, -8.71, 6.43]])

mean1 = np.mean(w1, axis=1)
mean2 = np.mean(w2, axis=1)
mean3 = np.mean(w3, axis=1)

covariance1 = np.cov(w1)
covariance2 = np.cov(w2)
covariance3 = np.cov(w3)

# Test Points
point1 = np.array([1, 2, 1])
point2 = np.array([5, 3, 1])
point3 = np.array([0, 0, 0])
point4 = np.array([1, 0, 0])
points = np.array([point1, point2, point3, point4])

# Question 3
prior1 = 0.8
prior2 = 0.1
prior3 = 0.1

g1 = MAP_classifier(points, mean1, covariance1, prior1)
g2 = MAP_classifier(points, mean2, covariance2, prior2)
g3 = MAP_classifier(points, mean3, covariance3, prior3)

print "Using MAP classifier:"
print "\tPoint {0} belongs to class w{1}".format(point1, classify(g1[0], g2[0], g3[0]))
print "\tPoint {0} belongs to class w{1}".format(point2, classify(g1[1], g2[1], g3[1]))
print "\tPoint {0} belongs to class w{1}".format(point3, classify(g1[2], g2[2], g3[2]))
print "\tPoint {0} belongs to class w{1}".format(point4, classify(g1[3], g2[3], g3[3]))

# Question 4
z_lvl = 0
X, Y, Z = np.mgrid[-10:10:100j, -10:10:100j, z_lvl:z_lvl:1j]
drawing_points = np.c_[X.ravel(), Y.ravel(), Z.ravel()]
g1 = MAP_classifier(drawing_points, mean1, covariance1, prior1)
g2 = MAP_classifier(drawing_points, mean2, covariance2, prior2)
g3 = MAP_classifier(drawing_points, mean3, covariance3, prior3)
g1.shape = 100, 100
g2.shape = 100, 100
g3.shape = 100, 100
Dr = np.empty([100, 100])
for i in range(100):
    for j in range(100):
        Dr[i][j] = classify(g1[i][j], g2[i][j], g3[i][j])

X, Y = np.mgrid[-10:10:100j, -10:10:100j]
pl.pcolormesh(X, Y, Dr)
pl.title("Using MAP classifier with X3 = {0}".format(z_lvl))
pl.show()

# Question 5
g1 = ML_classifier(points, mean1, covariance1)
g2 = ML_classifier(points, mean2, covariance2)
g3 = ML_classifier(points, mean3, covariance3)

print "Using ML classifier:"
print "\tPoint {0} belongs to class w{1}".format(point1, classify(g1[0], g2[0], g3[0]))
print "\tPoint {0} belongs to class w{1}".format(point2, classify(g1[1], g2[1], g3[1]))
print "\tPoint {0} belongs to class w{1}".format(point3, classify(g1[2], g2[2], g3[2]))
print "\tPoint {0} belongs to class w{1}".format(point4, classify(g1[3], g2[3], g3[3]))

z_lvl = 0
X, Y, Z = np.mgrid[-10:10:100j, -10:10:100j, z_lvl:z_lvl:1j]
drawing_points = np.c_[X.ravel(), Y.ravel(), Z.ravel()]
g1 = ML_classifier(drawing_points, mean1, covariance1)
g2 = ML_classifier(drawing_points, mean2, covariance2)
g3 = ML_classifier(drawing_points, mean3, covariance3)
g1.shape = 100, 100
g2.shape = 100, 100
g3.shape = 100, 100
Dr = np.empty([100, 100])
for i in range(100):
    for j in range(100):
        Dr[i][j] = classify(g1[i][j], g2[i][j], g3[i][j])

X, Y = np.mgrid[-10:10:100j, -10:10:100j]
pl.pcolormesh(X, Y, Dr)
pl.title("Using ML classifier with X3 = {0}".format(z_lvl))
pl.show()
