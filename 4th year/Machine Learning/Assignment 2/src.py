import operator
import math

# TRAINING
with open("train") as f:
    training_content = f.readlines()

training_content = [x.strip().split(' ', 1)[-1] for x in training_content]
number_of_emails = float(len(training_content))

vocabulary = {}
for line in training_content:
    line = line.split(' ', 1)[-1]
    line = line.split(' ')
    for i in range(0, len(line), 2):
        if line[i] in vocabulary:
            vocabulary[line[i]] += int(line[i+1])
        else:
            vocabulary[line[i]] = int(line[i+1])

number_of_unique_words_vocabulary = len(vocabulary)

prior_spam = 0
prior_ham = 0
words_spam = {}
words_ham = {}
for line in training_content:
    if line.split(' ', 1)[0] == "spam":
        prior_spam += 1
        line_words = line.split(' ', 1)[-1].split(' ')
        for i in range(0, len(line_words), 2):
            if line_words[i] in words_spam:
                words_spam[line_words[i]] += int(line_words[i+1])
            else:
                words_spam[line_words[i]] = int(line_words[i+1])
    if line.split(' ', 1)[0] == "ham":
        prior_ham += 1
        line_words = line.split(' ', 1)[-1].split(' ')
        for i in range(0, len(line_words), 2):
            if line_words[i] in words_ham:
                words_ham[line_words[i]] += int(line_words[i+1])
            else:
                words_ham[line_words[i]] = int(line_words[i+1])

prior_spam /= number_of_emails
prior_ham /= number_of_emails
number_of_words_spam = sum(words_spam.itervalues())
number_of_words_ham = sum(words_ham.itervalues())

words_Likelihood_spam = {}
words_Likelihood_ham = {}
for word in vocabulary.iterkeys():
    words_Likelihood_spam[word] = (words_spam.get(word, 0) + 1.0) / \
                                      (number_of_words_spam + number_of_unique_words_vocabulary)
    words_Likelihood_ham[word] = (words_ham.get(word, 0) + 1.0) / \
                                      (number_of_words_ham + number_of_unique_words_vocabulary)

sorted_likelihood_spam = sorted(words_Likelihood_spam.items(), key=operator.itemgetter(1))
sorted_likelihood_spam.reverse()
sorted_likelihood_ham = sorted(words_Likelihood_ham.items(), key=operator.itemgetter(1))
sorted_likelihood_ham.reverse()

print "P(spam) = ", prior_spam
print "5 most likely words given that a document is spam: "
for i in range(5):
    print "\t", i+1, "- p(", sorted_likelihood_spam[i][0], "|spam) = ", sorted_likelihood_spam[i][1]
print "5 most likely words given that a document is ham: "
for i in range(5):
    print "\t", i+1, "- p(", sorted_likelihood_ham[i][0], "|ham) = ", sorted_likelihood_ham[i][1]

# TESTING
with open("test") as f:
    testing_content = f.readlines()

testing_content = [x.strip().split(' ', 1)[-1] for x in testing_content]
number_of_tests = len(testing_content)
correct_classifications = 0
# mail_cnt = 1
for test in testing_content:
    test = test.split(' ', 1)
    correct_class = test[0]
    test_words = test[1].split(' ')
    p_spam = math.log(prior_spam)
    p_ham = math.log(prior_ham)
    for i in range(0, len(test_words), 2):
        p_spam += int(test_words[i + 1]) * math.log(words_Likelihood_spam[test_words[i]])
        p_ham += int(test_words[i + 1]) * math.log(words_Likelihood_ham[test_words[i]])
    if p_spam >= p_ham:
        test_class = "spam"
    else:
        test_class = "ham"
    if test_class == correct_class:
        correct_classifications += 1
    # print "mail {0} is {1} spam and {2} ham.".format(mail_cnt, p_spam, p_ham)
    # print "mail {0} is classified as {1} and it is {2}.".format(mail_cnt, test_class, correct_class)
    # mail_cnt += 1

print "Accuracy = {0}%".format(correct_classifications * 100.0 / number_of_tests)
